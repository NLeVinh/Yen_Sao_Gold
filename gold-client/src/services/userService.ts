import { apiClient } from '@/http/apiClient';
import { User } from '@/types/User';
import { ENDPOINT } from '@/constants/apiConstant';
import { LoginPayload } from '@/types/User';
import { LoginResponse } from '@/types/Response';

const getUsers = () => apiClient.get<User[]>(ENDPOINT.USER);

const getUserById = (id: number) => apiClient.get<User>(`${ENDPOINT.USER}/${id}`);

const createUser = (data: Partial<User>) =>
    apiClient.post<User, Partial<User>>(ENDPOINT.USER, data);

const updateUser = (id: number, data: Partial<User>) =>
    apiClient.put<User, Partial<User>>(`${ENDPOINT.USER}/${id}`, data);

const deleteUser = (id: number) => apiClient.delete<null>(`${ENDPOINT.USER}/${id}`);

const login = (data: LoginPayload) =>
    apiClient.post<LoginResponse, LoginPayload>(`${ENDPOINT.AUTH}/login`, data);

export { getUsers, getUserById, createUser, deleteUser, updateUser, login };
