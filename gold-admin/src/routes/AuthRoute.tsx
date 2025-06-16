import { lazy } from 'react';

import Loadable from '@/components/shared/Loader/LoadableComponent';
import AuthenticationLayout from '@/layouts/AuthenticationLayout';

const AuthenticationPage = Loadable(
    lazy(() => import('@/pages/Authentication/AuthenticationPage'))
);

const AuthRoutes = {
    path: '/',
    element: <AuthenticationLayout />,
    children: [
        {
            path: '/login',
            element: <AuthenticationPage />,
        },
    ],
};

export default AuthRoutes;
