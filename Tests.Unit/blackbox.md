# Black Box Testing Analysis: Password Validator

## Specifications

The password validator enforces the following requirements:

-   **Length**: Between 8 and 50 characters
-   **Uppercase**: At least one uppercase letter
-   **Number**: At least one number
-   **Special Character**: At least one special character

## Equivalence Partitioning

| Partition ID | Condition         | Type    | Description                    |
| ------------ | ----------------- | ------- | ------------------------------ |
| VP1          | Length            | Valid   | 8-50 characters                |
| VP2          | Uppercase         | Valid   | Contains ≥ 1 uppercase letter  |
| VP3          | Number            | Valid   | Contains ≥ 1 number            |
| VP4          | Special Character | Valid   | Contains ≥ 1 special character |
| IP1          | Length            | Invalid | < 8 characters                 |
| IP2          | Length            | Invalid | > 50 characters                |
| IP3          | Uppercase         | Invalid | No uppercase letters           |
| IP4          | Number            | Invalid | No numbers                     |
| IP5          | Special Character | Invalid | No special characters          |

## Boundary Value Analysis

| Condition      | Boundary Values |
| -------------- | --------------- |
| Minimum length | 7, 8, 9         |
| Maximum length | 49, 50, 51      |

## Decision Table

| Test Case | Length 8-50 | Has Uppercase | Has Number | Has Special Char | Expected Result |
| --------- | ----------- | ------------- | ---------- | ---------------- | --------------- |
| TC1       | ✓           | ✓             | ✓          | ✓                | Valid           |
| TC2       | ✗ (< 8)     | ✓             | ✓          | ✓                | Invalid         |
| TC3       | ✗ (> 50)    | ✓             | ✓          | ✓                | Invalid         |
| TC4       | ✓           | ✗             | ✓          | ✓                | Invalid         |
| TC5       | ✓           | ✓             | ✗          | ✓                | Invalid         |
| TC6       | ✓           | ✓             | ✓          | ✗                | Invalid         |
| TC7       | ✓           | ✗             | ✗          | ✗                | Invalid         |
| TC8       | ✗ (< 8)     | ✗             | ✗          | ✗                | Invalid         |

## Test Cases

| ID   | Description                                | Input                   | Expected Result |
| ---- | ------------------------------------------ | ----------------------- | --------------- |
| TC1  | Valid password with all requirements       | `Password1!`            | Valid           |
| TC2  | Valid at minimum boundary                  | `Pass123!`              | Valid           |
| TC3  | Valid at maximum boundary                  | `Aa1!` + 46 chars       | Valid           |
| TC4  | Too short (7 chars)                        | `Pass1!`                | Invalid         |
| TC5  | Below minimum boundary                     | `Pas12!A`               | Invalid         |
| TC6  | Too long (51 chars)                        | `Aa1!` + 47 chars       | Invalid         |
| TC7  | Above maximum boundary                     | `Password1!` + 41 chars | Invalid         |
| TC8  | No uppercase                               | `password1!`            | Invalid         |
| TC9  | No number                                  | `Password!`             | Invalid         |
| TC10 | No special character                       | `Password1`             | Invalid         |
| TC11 | Only lowercase                             | `passwordonly`          | Invalid         |
| TC12 | Only uppercase                             | `PASSWORDONLY`          | Invalid         |
| TC13 | Only numbers                               | `12345678`              | Invalid         |
| TC14 | Only special characters                    | `!@#$%^&*`              | Invalid         |
| TC15 | Empty string                               | `` (empty)              | Invalid         |
| TC16 | Multiple uppercase, numbers, special chars | `Pass123!@#Word`        | Valid           |
| TC17 | Special chars at boundaries                | `!Password1!`           | Valid           |
| TC18 | Exactly 8 characters                       | `Passw0rd!`             | Valid           |
| TC19 | Exactly 50 characters                      | `A1!` + 47 chars        | Valid           |

## Edge Cases to Consider

-   **Whitespace characters**: Leading/trailing spaces, spaces within password
-   **Unicode characters**: Emoji, non-ASCII characters
-   **SQL injection attempts**: `'; DROP TABLE--`
-   **XSS attempts**: `<script>alert('XSS')</script>`
-   **NULL/undefined inputs**: Null values, undefined, missing input
