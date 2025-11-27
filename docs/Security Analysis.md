# Security Analysis - Campaign Codex

## Executive Summary

This document provides a comprehensive security analysis of the Campaign Codex application, identifying potential security risks and documenting which defenses are already implemented.

---

## ✅ Defenses Already Implemented

### 1. Authentication & Authorization

| Defense                         | Implementation                                       | Location                |
| ------------------------------- | ---------------------------------------------------- | ----------------------- |
| **ASP.NET Core Identity**       | Uses built-in Identity framework for user management | `Program.cs`            |
| **Global Authorization Policy** | All endpoints require authentication by default      | `Program.cs` line 70-72 |
| **Role-based Authorization**    | Identity Roles configured                            | `Program.cs` line 65    |
| **Cookie-based Authentication** | Secure cookie authentication with `useCookies=true`  | `useAccount.ts` line 15 |

```csharp
// All controllers require authentication by default
var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
opt.Filters.Add(new AuthorizeFilter(policy));
```

### 2. Password Security

| Defense                            | Implementation                                             | Location                           |
| ---------------------------------- | ---------------------------------------------------------- | ---------------------------------- |
| **Password Complexity Validation** | Min 8 chars, uppercase, lowercase, digit, special char     | `PasswordValidator.cs`             |
| **Password Length Limits**         | 8-50 characters                                            | `PasswordValidator.cs` lines 9-10  |
| **Breached Password Check**        | Integration with Have I Been Pwned API (k-anonymity model) | `PasswordValidator.cs` lines 29-47 |
| **Unique Email Requirement**       | Users must have unique emails                              | `Program.cs` line 63               |

```csharp
// Password is checked against HIBP database before registration
if (await PasswordValidator.IsPwned(registerDto.Password))
{
    ModelState.AddModelError("Password", "Password is Pwned. Pick a more secure password.");
    return ValidationProblem();
}
```

### 3. Input Sanitization (XSS Prevention)

| Defense                              | Implementation                                                 | Location                             |
| ------------------------------------ | -------------------------------------------------------------- | ------------------------------------ |
| **HTML Sanitization**                | Uses `Ganss.Xss.HtmlSanitizer` for user-generated HTML content | Multiple services                    |
| **Wiki Content Sanitization**        | Content sanitized on create and update                         | `WikiService.cs` lines 35, 127       |
| **Notes Content Sanitization**       | Notes sanitized on update                                      | `NotesService.cs` line 56            |
| **Character Backstory Sanitization** | Backstory sanitized on create and update                       | `CharactersService.cs` lines 48, 108 |

```csharp
var sanitizer = new HtmlSanitizer();
entry.Content = sanitizer.Sanitize(wikiEntryDto.Content);
```

### 4. Rate Limiting

| Defense                        | Implementation                                                 | Location                 |
| ------------------------------ | -------------------------------------------------------------- | ------------------------ |
| **Global Rate Limiting**       | Fixed window rate limiter (50 requests/10 seconds per user/IP) | `Program.cs` lines 78-94 |
| **User/IP Based Partitioning** | Rate limits partitioned by user identity or IP address         | `Program.cs` lines 80-83 |

```csharp
options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(httpContext =>
    RateLimitPartition.GetFixedWindowLimiter(
        httpContext.User.Identity?.Name ?? httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown",
        partition => new FixedWindowRateLimiterOptions
        {
            AutoReplenishment = true,
            PermitLimit = 50,
            QueueLimit = 0,
            Window = TimeSpan.FromSeconds(10),
        }
    )
);
```

### 5. SQL Injection Prevention

| Defense                       | Implementation                         | Location          |
| ----------------------------- | -------------------------------------- | ----------------- |
| **Entity Framework Core ORM** | Uses parameterized queries via EF Core | All services      |
| **LINQ Queries**              | No raw SQL queries found in codebase   | All data access   |
| **Strongly Typed DbContext**  | Type-safe database operations          | `AppDbContext.cs` |

### 6. CORS Configuration

| Defense                 | Implementation                                 | Location                   |
| ----------------------- | ---------------------------------------------- | -------------------------- |
| **Restricted Origins**  | CORS configured with specific allowed origins  | `Program.cs` lines 105-109 |
| **Credentials Allowed** | `AllowCredentials()` for cookie authentication | `Program.cs` line 107      |

```csharp
app.UseCors(opt =>
    opt.AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials()
        .WithOrigins("http://localhost:3000", "http://localhost", "http://46.224.39.173")
);
```

### 7. Authorization Checks (IDOR Prevention)

| Defense                        | Implementation                                    | Location                 |
| ------------------------------ | ------------------------------------------------- | ------------------------ |
| **Campaign Ownership Checks**  | Validates user is DM or player before access      | `CampaignService.cs`     |
| **Character Ownership Checks** | Validates user owns character before modification | `CharactersService.cs`   |
| **Notes Ownership Checks**     | Validates user owns notes before access           | `NotesService.cs`        |
| **Wiki Entry DM Checks**       | Only DM can delete wiki entries                   | `WikiService.cs` line 92 |
| **Photo Upload Authorization** | Validates permissions before photo upload         | `PhotoService.cs`        |

### 8. Error Handling

| Defense                              | Implementation                                 | Location                             |
| ------------------------------------ | ---------------------------------------------- | ------------------------------------ |
| **Global Exception Middleware**      | Catches and handles all exceptions             | `ExceptionMiddleware.cs`             |
| **Environment-Aware Error Messages** | Stack traces hidden in production              | `ExceptionMiddleware.cs` lines 30-32 |
| **Validation Error Handling**        | FluentValidation exceptions handled gracefully | `ExceptionMiddleware.cs` lines 42-67 |

```csharp
var response = env.IsDevelopment()
    ? new AppException(context.Response.StatusCode, ex.Message, ex.StackTrace)
    : new AppException(context.Response.StatusCode, ex.Message, null);
```

### 9. Concurrency Control

| Defense                    | Implementation                                    | Location                       |
| -------------------------- | ------------------------------------------------- | ------------------------------ |
| **Optimistic Concurrency** | `Xmin` token prevents concurrent update conflicts | `WikiService.cs` lines 117-121 |

### 10. Logging

| Defense                 | Implementation                | Location                         |
| ----------------------- | ----------------------------- | -------------------------------- |
| **Serilog Integration** | Structured logging configured | `Program.cs` lines 24-27         |
| **Exception Logging**   | All exceptions logged         | `ExceptionMiddleware.cs` line 26 |

### 11. Client-Side Validation

| Defense                     | Implementation                          | Location               |
| --------------------------- | --------------------------------------- | ---------------------- |
| **Zod Schema Validation**   | Frontend form validation                | `src/lib/schemas/*.ts` |
| **Email Format Validation** | Validates email format                  | `registerSchema.ts`    |
| **Password Confirmation**   | Confirms password match on registration | `registerSchema.ts`    |

---

## ⚠️ Security Risks & Recommendations

### 1. **CRITICAL: Exposed API Secrets in Configuration** 🔴

**Risk Level:** CRITICAL

**Issue:** Cloudinary API credentials are hardcoded in `appsettings.json`:

```json
"CloudinarySettings": {
    "CloudName": "dkcqshlp4",
    "ApiKey": "426284213378781",
    "ApiSecret": "WRaHhyJ4ta2kFKWKHjOrWWba4OA"
}
```

**Impact:** Anyone with access to the repository can use these credentials to:

-   Upload malicious content to your Cloudinary account
-   Delete existing photos
-   Incur costs on your account

**Recommendation:**

1. **Immediately rotate the Cloudinary API credentials**
2. Move secrets to environment variables or a secrets manager
3. Add `appsettings.json` to `.gitignore` (use `appsettings.Development.json.template` instead)
4. Use Azure Key Vault, AWS Secrets Manager, or similar in production

---

### 2. **HIGH: HTTPS Not Enforced** 🟠

**Risk Level:** HIGH

**Issue:** HTTPS redirection is commented out in `Program.cs`:

```csharp
// app.UseHttpsRedirection();
```

**Impact:**

-   Data transmitted over HTTP can be intercepted (man-in-the-middle attacks)
-   Session cookies can be stolen
-   Credentials can be captured in transit

**Recommendation:**

1. Enable `app.UseHttpsRedirection();`
2. Configure HSTS headers
3. Ensure SSL certificates are properly configured

---

### 3. **HIGH: Missing File Upload Validation** 🟠

**Risk Level:** HIGH

**Issue:** Photo uploads in `CloudinaryService.cs` only check file length, not file type:

```csharp
if (file.Length <= 0)
{
    return null;
}
```

**Impact:**

-   Malicious files (executables, scripts) could be uploaded
-   Storage quota abuse
-   Potential for stored XSS if file names are not sanitized

**Recommendation:**

1. Validate file extensions (allow only `.jpg`, `.png`, `.gif`, `.webp`)
2. Validate MIME types
3. Validate file content (magic bytes)
4. Set maximum file size limits
5. Sanitize file names

```csharp
private static readonly string[] AllowedExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
private const long MaxFileSize = 5 * 1024 * 1024; // 5MB

public async Task<PhotoUploadResult?> UploadPhoto(IFormFile file)
{
    if (file.Length <= 0 || file.Length > MaxFileSize)
        return null;

    var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
    if (!AllowedExtensions.Contains(extension))
        return null;

    // ... rest of upload logic
}
```

---

### 4. **MEDIUM: Missing Anti-Forgery Tokens (CSRF)** 🟡

**Risk Level:** MEDIUM

**Issue:** No CSRF protection implemented for state-changing operations.

**Impact:**

-   Attackers could trick authenticated users into performing unwanted actions
-   Cross-site request forgery attacks possible

**Recommendation:**

1. Implement anti-forgery tokens for API endpoints
2. Use `SameSite` cookie attribute (set to `Strict` or `Lax`)
3. Consider implementing custom CSRF middleware for APIs

---

### 5. **MEDIUM: Password Validation Mismatch (Frontend vs Backend)** 🟡

**Risk Level:** MEDIUM

**Issue:** Frontend allows 6-character passwords, backend requires 8:

**Frontend (`registerSchema.ts`):**

```typescript
password: z.string().min(6, 'Password must be at least 6 characters'),
```

**Backend (`PasswordValidator.cs`):**

```csharp
private const int MinLength = 8;
```

**Impact:**

-   User confusion when passwords are rejected
-   Potential for weaker passwords if validation differs

**Recommendation:**

1. Sync validation rules between frontend and backend
2. Consider implementing shared validation contracts

---

### 6. **MEDIUM: Missing Security Headers** 🟡

**Risk Level:** MEDIUM

**Issue:** No security headers configured (CSP, X-Frame-Options, etc.)

**Recommendation:**
Add security headers middleware:

```csharp
app.Use(async (context, next) =>
{
    context.Response.Headers.Append("X-Content-Type-Options", "nosniff");
    context.Response.Headers.Append("X-Frame-Options", "DENY");
    context.Response.Headers.Append("X-XSS-Protection", "1; mode=block");
    context.Response.Headers.Append("Referrer-Policy", "strict-origin-when-cross-origin");
    context.Response.Headers.Append("Content-Security-Policy", "default-src 'self'");
    await next();
});
```

---

### 7. **MEDIUM: Missing Input Length Validation on DTOs** 🟡

**Risk Level:** MEDIUM

**Issue:** DTOs lack `[MaxLength]` or `[StringLength]` attributes:

```csharp
// CreateCampaignDto.cs - No length limit on Name
public record CreateCampaignDto
{
    public required string Name { get; init; }
}
```

**Impact:**

-   Denial of service via extremely large inputs
-   Database storage issues

**Recommendation:**
Add data annotations:

```csharp
public record CreateCampaignDto
{
    [Required]
    [StringLength(100, MinimumLength = 1)]
    public required string Name { get; init; }
}
```

---

### 8. **LOW: Swagger/OpenAPI Exposed in Development** 🟢

**Risk Level:** LOW (correctly scoped)

**Issue:** OpenAPI documentation is only exposed in development, which is correct behavior:

```csharp
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(...);
}
```

**Status:** ✅ Properly implemented

---

### 9. **LOW: Debug Logging in Production** 🟢

**Risk Level:** LOW

**Issue:** Serilog is configured with `MinimumLevel.Debug()`:

```csharp
Log.Logger = new LoggerConfiguration().MinimumLevel.Debug().WriteTo.Console().CreateLogger();
```

**Recommendation:**
Configure different log levels for environments:

```csharp
var logConfig = new LoggerConfiguration()
    .MinimumLevel.Is(builder.Environment.IsDevelopment() ? LogEventLevel.Debug : LogEventLevel.Information)
    .WriteTo.Console()
    .CreateLogger();
```

---

### 10. **INFO: Database Seeding in Production** ℹ️

**Risk Level:** INFO

**Issue:** Database is deleted and reseeded on every startup:

```csharp
await context.Database.EnsureDeletedAsync();
await context.Database.MigrateAsync();
await DbInitializer.SeedData(context, userManager);
```

**Impact:** This will delete all data on every restart

**Recommendation:**
Guard this for development only:

```csharp
if (app.Environment.IsDevelopment())
{
    await context.Database.EnsureDeletedAsync();
    await DbInitializer.SeedData(context, userManager);
}
await context.Database.MigrateAsync();
```

---

## Security Checklist Summary

| Category               | Status | Notes                            |
| ---------------------- | ------ | -------------------------------- |
| Authentication         | ✅     | ASP.NET Core Identity            |
| Authorization          | ✅     | Global policy + ownership checks |
| Password Security      | ✅     | Complexity + HIBP check          |
| XSS Prevention         | ✅     | HTML Sanitizer                   |
| SQL Injection          | ✅     | EF Core ORM                      |
| Rate Limiting          | ✅     | 50 req/10 sec                    |
| CORS                   | ✅     | Configured with specific origins |
| Secrets Management     | ❌     | Credentials in config file       |
| HTTPS                  | ❌     | Disabled                         |
| File Upload Validation | ⚠️     | Partial - size only              |
| CSRF Protection        | ❌     | Not implemented                  |
| Security Headers       | ❌     | Not configured                   |
| Input Validation       | ⚠️     | Partial - no length limits       |
| Error Handling         | ✅     | Environment-aware                |
| Logging                | ✅     | Serilog configured               |

---

## Priority Action Items

1. **IMMEDIATE:** Rotate and secure Cloudinary API credentials
2. **HIGH:** Enable HTTPS redirection
3. **HIGH:** Implement file upload validation (type, size, content)
4. **MEDIUM:** Add CSRF protection
5. **MEDIUM:** Configure security headers
6. **MEDIUM:** Sync frontend/backend password validation
7. **LOW:** Add input length validation to DTOs
8. **LOW:** Configure environment-specific logging levels
9. **LOW:** Guard database seeding for development only

---

_Analysis Date: November 27, 2025_
_Analyzed by: GitHub Copilot_
