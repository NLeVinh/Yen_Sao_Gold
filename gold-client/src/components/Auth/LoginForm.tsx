'use client';

import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { Button, Typography, Box } from '@mui/material';

import FormInput from '../shared/Control/FormInput';
import { LoginPayload } from '@/types/User';
import { loginSchema } from '@/schemas/authSchemas';
import { useLogin } from '@/hooks/useLogin';
import { AUTH_UI } from '@/constants';

const UI_MESSAGES = AUTH_UI.LOGIN;

export default function LoginForm() {
    const { loading, errorMessage, handleLogin } = useLogin();

    const {
        control,
        handleSubmit,
        formState: { errors },
    } = useForm<LoginPayload>({
        resolver: zodResolver(loginSchema),
    });

    const onSubmit = (data: LoginPayload) => {
        handleLogin(data);
    };

    return (
        <Box className="login-form__container">
            <Box component="form" onSubmit={handleSubmit(onSubmit)} className="login-form">
                <Typography variant="h5" className="login-form__title">
                    {UI_MESSAGES.TITLE}
                </Typography>

                <FormInput
                    name="email"
                    label={UI_MESSAGES.EMAIL_LABEL}
                    control={control}
                    errors={errors}
                    error={!!errors.email}
                    helperText={errors.email?.message}
                />

                <FormInput
                    name="password"
                    label={UI_MESSAGES.PASSWORD_LABEL}
                    type="password"
                    control={control}
                    errors={errors}
                    error={!!errors.password}
                    helperText={errors.password?.message}
                />

                {errorMessage && (
                    <Typography color="error" fontSize="0.9rem" className="login-form__error">
                        {errorMessage}
                    </Typography>
                )}

                <Button
                    type="submit"
                    variant="contained"
                    disabled={loading}
                    className="login-form__submit"
                >
                    {loading ? UI_MESSAGES.LOADING_TEXT : UI_MESSAGES.SUBMIT_TEXT}
                </Button>
            </Box>
        </Box>
    );
}
