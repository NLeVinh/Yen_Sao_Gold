import { createBrowserRouter } from 'react-router-dom'

// project import
import MainRoutes from './MainRoute'
import AuthRoutes from './AuthRoute'

const router = createBrowserRouter([MainRoutes, AuthRoutes], {
    basename: import.meta.env.VITE_APP_BASE_NAME,
})

export default router
