// schemas/authSchemas.ts
import { z } from 'zod';

export const loginSchema = z.object({
    email: z.string().email('Email không hợp lệ'),
    password: z.string().min(8, 'Mật khẩu ít nhất 8 ký tự'),
});

export const registerSchema = z
    .object({
        email: z.string().email('Email không hợp lệ'),
        password: z.string().min(8, 'Mật khẩu ít nhất 8 ký tự'),
        confirmPassword: z.string(),
        name: z.string().min(2, 'Tên ít nhất 2 ký tự'),
    })
    .refine((data) => data.password === data.confirmPassword, {
        message: 'Mật khẩu không khớp',
        path: ['confirmPassword'],
    });
