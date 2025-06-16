// ToastNotification.tsx
import { STATUS_TYPES } from '@/constants/generalConstant';
import React from 'react';
import { ToastContainer, toast } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';

export const showToast = (message: string, type: 'success' | 'error') => {
    if (type === STATUS_TYPES.SUCCESS) {
        toast.success(message);
    } else {
        toast.error(message);
    }
};

const ToastNotificationComponent: React.FC = () => {
    return <ToastContainer />;
};

export default ToastNotificationComponent;
