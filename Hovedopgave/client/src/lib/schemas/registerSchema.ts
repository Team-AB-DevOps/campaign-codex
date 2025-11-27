import { z } from 'zod';

export const registerSchema = z
    .object({
        email: z.string().email('Please provide a valid email address'),
        displayName: z
            .string()
            .min(2, 'Display name must be at least 2 characters'),
        password: z.string().min(8, 'Password must be at least 8 characters'),
        confirmPassword: z.string().min(1, 'Please confirm your password'),
    })
    .refine((data) => data.password === data.confirmPassword, {
        message: "Passwords don't match",
        path: ['confirmPassword'],
    });

export type RegisterSchema = z.infer<typeof registerSchema>;
