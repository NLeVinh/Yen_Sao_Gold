// PrivateRoute.tsx
import React from 'react'
import { Navigate } from 'react-router-dom'
import { useAuth } from '@/contexts/Athentication/AuthContext'

import DashboardLayout from '@/layouts/DashboardLayout'

const PrivateRoute: React.FC = () => {
    const { isAuthenticated } = useAuth()

    return isAuthenticated ? <DashboardLayout /> : <Navigate to="/login" />
}

export default PrivateRoute
