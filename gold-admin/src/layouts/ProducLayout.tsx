import React from 'react';
import { Outlet } from 'react-router-dom';

const ProducLayout: React.FC = () => {
    return (
        <>
            <Outlet />
        </>
    );
};

export default ProducLayout;
