export interface ApiResponse<T> {
    success: boolean;
    message: string;
    data: T | null;
}

export type LoginResponse = ApiResponse<{
    accessToken: string;
    refreshToken: string;
    expiresAt: string;
    role: string;
    permissions: string[];
}>;
