import {
    IconUser,
    IconChartInfographic,
    IconDashboard,
    IconBrandTwitter,
    IconBrandWish,
    IconReportAnalytics,
    IconArchive,
} from '@tabler/icons-react';

const generals = {
    id: 'generals',
    title: 'generals',
    type: 'group',
    children: [
        {
            id: 'products',
            title: 'Products',
            type: 'item',
            url: '/products',
            icon: <IconBrandTwitter stroke={1.5} />,
            target: true,
        },
        {
            id: 'categories',
            title: 'Categories',
            type: 'item',
            url: '/categories',
            icon: <IconBrandWish stroke={1.5} />,
            target: true,
        },
        {
            id: 'analytics',
            title: 'Analytics',
            type: 'item',
            url: '/analytics',
            icon: <IconChartInfographic stroke={1.5} />,
            target: true,
        },
        {
            id: 'blogs',
            title: 'Blogs',
            type: 'item',
            url: '/blogs',
            icon: <IconReportAnalytics stroke={1.5} />,
            target: true,
        },
    ],
};

const dashboard = {
    id: 'group-dashboard',
    title: 'Navigation',
    type: 'group',
    children: [
        {
            id: 'dashboard',
            title: 'Dashboard',
            type: 'item',
            url: '/dashboard',
            icon: <IconDashboard stroke={1.5} />,
            breadcrumbs: false,
        },
    ],
};

const settings = {
    id: 'settings',
    title: 'Settings',
    type: 'group',
    children: [
        {
            id: 'profile',
            title: 'Profile',
            type: 'item',
            url: '/profile',
            icon: <IconUser stroke={1.5} />,
            breadcrumbs: false,
        },
        {
            id: 'inventory',
            title: 'Inventory',
            type: 'item',
            url: '/inventory',
            icon: <IconArchive stroke={1.5} />,
            breadcrumbs: false,
        },
    ],
};

const menuItems = {
    items: [dashboard, generals, settings],
};

export default menuItems;
