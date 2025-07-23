export interface User {
    id: number;
    name: string;
    email: string;
    avatar: string;
    createdAt: string;
}

export interface LoginPayload {
    email: string;
    password: string;
}
