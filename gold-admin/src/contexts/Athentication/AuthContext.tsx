// AuthContext.tsx
import React, { createContext, useContext, useState, ReactNode } from 'react';
import { apiClient } from '@/http/apiClient';
import { USER_ENPOINT } from '@/constants/apiEndpointConstant';
import { STORAGE_KEY } from '@/constants/webStorageConstant';
import { localStorageHelper } from '@/helpers/WebStorageHelper';
import { mutate } from 'swr';
import { ROLES } from '@/constants/roleConstant';
import { showToast } from '@/components/shared/ToastNotification/ToastNotificationComponent';
import { LoginResponse } from '@/constants/typesConstants';

interface AuthContextType {
    isAuthenticated: boolean;
    login: (email: string, password: string) => Promise<void>;
    logout: () => void;
}

const AuthContext = createContext<AuthContextType | undefined>(undefined);

export const AuthProvider: React.FC<{ children: ReactNode }> = ({ children }) => {
    const [isAuthenticated, setIsAuthenticated] = useState<boolean>(() => {
        const token = localStorageHelper.getStorageItem(STORAGE_KEY.ACCESS_TOKEN);
        return true;
    });

    const login = async (email: string, password: string) => {
        try {
            const response = await apiClient.post<LoginResponse>(USER_ENPOINT.LOGIN, {
                email,
                password,
            });
            const { Token, RefreshToken, Role } = response;

            if (
                Role.includes(ROLES.SUPER_ADMIN) ||
                Role.includes(ROLES.ADMIN) ||
                Role.includes(ROLES.STAFF)
            ) {
                localStorageHelper.setStorageItem(STORAGE_KEY.ACCESS_TOKEN, Token);
                localStorageHelper.setStorageItem(STORAGE_KEY.REFRESH_TOKEN, RefreshToken);

                setIsAuthenticated(true);
                mutate('user');
            } else {
                showToast('Login failed. Please check your credentials and try again.', 'error');
            }
        } catch (error) {
            console.error('Login failed:', error);
            showToast('Login failed. Please check your credentials and try again.', 'error');
        }
    };

    const logout = () => {
        localStorageHelper.removeStorageItem(STORAGE_KEY.ACCESS_TOKEN);
        localStorageHelper.removeStorageItem(STORAGE_KEY.REFRESH_TOKEN);
        setIsAuthenticated(false);
        mutate('user');
    };

    return (
        <AuthContext.Provider value={{ isAuthenticated, login, logout }}>
            {children}
        </AuthContext.Provider>
    );
};

export const useAuth = () => {
    const context = useContext(AuthContext);
    if (!context) {
        throw new Error('useAuth must be used within an AuthProvider');
    }
    return context;
};
