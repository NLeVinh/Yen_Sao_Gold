import React from 'react'
import { Outlet } from 'react-router-dom'

const AuthenticationLayout: React.FC = () => {
    return (
        <>
            <Outlet />
        </>
    )
}

export default AuthenticationLayout
