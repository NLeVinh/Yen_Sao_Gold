import { useState } from 'react';
import { useRouter } from 'next/navigation';
import { login } from '@/services/userService';
import { setCookie } from '@/helpers/storageHelper';
import { LoginPayload } from '@/types/User';
import { AUTH_MESSAGES } from '@/constants';
import { LoginResponse } from '@/types/Response';

interface UseLoginReturn {
    loading: boolean;
    errorMessage: string;
    handleLogin: (data: LoginPayload) => Promise<void>;
    clearError: () => void;
}

const LOGIN_MESSAGES = AUTH_MESSAGES.LOGIN;

export const useLogin = (): UseLoginReturn => {
    const [loading, setLoading] = useState(false);
    const [errorMessage, setErrorMessage] = useState('');
    const router = useRouter();

    const getErrorMessage = (err: any): string => {
        return err.response?.data?.message || err.message || LOGIN_MESSAGES.DEFAULT_ERROR;
    };

    const handleLoginSuccess = (resData: NonNullable<LoginResponse['data']>) => {
        setCookie('accessToken', resData.accessToken, resData.expiresAt);
        setCookie('refreshToken', resData.refreshToken, resData.expiresAt);
        router.push('/');
    };

    const handleLogin = async (data: LoginPayload): Promise<void> => {
        setLoading(true);
        setErrorMessage('');

        try {
            const res: LoginResponse = await login(data);
            const resData = res.data;

            if (!resData || !resData.accessToken || !resData.expiresAt) {
                throw new Error(LOGIN_MESSAGES.INVALID_DATA);
            }

            handleLoginSuccess(resData);
        } catch (err: any) {
            setErrorMessage(getErrorMessage(err));
        } finally {
            setLoading(false);
        }
    };

    const clearError = () => {
        setErrorMessage('');
    };

    return {
        loading,
        errorMessage,
        handleLogin,
        clearError,
    };
};
