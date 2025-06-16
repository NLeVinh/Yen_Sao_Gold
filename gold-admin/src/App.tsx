import { RouterProvider } from 'react-router-dom';

import router from './routes';
import { AuthProvider } from './contexts/Athentication/AuthContext';
import { ToastContainer } from 'react-toastify';
import '@/assets/styles/main.scss';

(window as any).global = window;

function App() {
    return (
        <AuthProvider>
            <ToastContainer />
            <RouterProvider router={router} />
        </AuthProvider>
    );
}

export default App;
