import { lazy } from 'react';

import Loadable from '@/components/shared/Loader/LoadableComponent';

import PrivateRoute from './PrivateRoute';

const DashboardPage = Loadable(lazy(() => import('@/pages/Home')));
const AboutPage = Loadable(lazy(() => import('@/pages/About')));
const NotFoundPage = Loadable(lazy(() => import('@/pages/NotFound')));
const ProductsPage = Loadable(lazy(() => import('@/pages/Products/Products')));
const CategoriesPage = Loadable(lazy(() => import('@/pages/Categories/Categories')));

const MainRoutes = {
    path: '/',
    element: <PrivateRoute />,
    children: [
        {
            path: '/',
            element: <DashboardPage />,
        },
        {
            path: 'dashboard',
            element: <DashboardPage />,
        },
        {
            path: '404',
            element: <NotFoundPage />,
        },
        {
            path: 'about',
            element: <AboutPage />,
        },
        {
            path: 'products',
            element: <ProductsPage />,
        },
        {
            path: 'categories',
            element: <CategoriesPage />,
        },
    ],
};

export default MainRoutes;
