import axios, { AxiosRequestConfig, AxiosResponse } from 'axios';
import api from './axiosConfig';

export const apiClient = {
    get: async <T>(url: string, config?: AxiosRequestConfig): Promise<T> => {
        const res: AxiosResponse<T> = await api.get(url, config);
        return res.data;
    },

    post: async <T, D = unknown>(
        url: string,
        data?: D,
        config?: AxiosRequestConfig
    ): Promise<T> => {
        const res: AxiosResponse<T> = await api.post(url, data, config);
        return res.data;
    },

    put: async <T, D = unknown>(url: string, data?: D, config?: AxiosRequestConfig): Promise<T> => {
        const res: AxiosResponse<T> = await api.put(url, data, config);
        return res.data;
    },

    delete: async <T>(url: string, config?: AxiosRequestConfig): Promise<T> => {
        const res: AxiosResponse<T> = await api.delete(url, config);
        return res.data;
    },
};
