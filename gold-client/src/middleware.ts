import { NextRequest, NextResponse } from 'next/server';
import { verifyJWT } from './helpers/authHelper';

const ROUTE_CONFIG = {
    // Public routes - accessible to everyone
    PUBLIC: [
        '/',
        '/products',
        '/products/[slug]',
        '/categories',
        '/search',
        '/about',
        '/contact',
        '/terms',
    ],

    // Auth routes - redirect to home if already logged in
    AUTH: ['/login', '/register', '/forgot-password', '/reset-password'],

    // Protected routes - require authentication
    PROTECTED: ['/profile'],

    // Guest allowed but enhanced with auth
    GUEST_ENHANCED: ['/cart', '/checkout', '/reviews'],
} as const;

async function verifyToken(token: string) {
    try {
        const result = await verifyJWT(token);
        return result;
    } catch (error) {
        console.error('Token verification failed:', error);
        return null;
    }
}

// ✅ OPTION 1: Named export (current approach - keep this)
export async function middleware(request: NextRequest) {
    try {
        const { pathname } = request.nextUrl;
        const token = request.cookies.get('accessToken')?.value;
        const user = token ? await verifyToken(token) : null;

        // Check route type
        const isPublic = ROUTE_CONFIG.PUBLIC.some(
            (route) => pathname === route || pathname.startsWith(route.replace('[slug]', ''))
        );

        const isAuth = ROUTE_CONFIG.AUTH.some((route) => pathname.startsWith(route));
        const isProtected = ROUTE_CONFIG.PROTECTED.some((route) => pathname.startsWith(route));
        const isGuestEnhanced = ROUTE_CONFIG.GUEST_ENHANCED.some((route) =>
            pathname.startsWith(route)
        );

        // Handle protected routes
        if (isProtected) {
            if (!user) {
                const loginUrl = new URL('/login', request.url);
                loginUrl.searchParams.set('redirect', pathname);
                loginUrl.searchParams.set('message', 'Vui lòng đăng nhập để tiếp tục');
                return NextResponse.redirect(loginUrl);
            }
        }

        // Handle auth routes
        if (isAuth && user) {
            return NextResponse.redirect(new URL('/', request.url));
        }

        // Add user context to all routes
        const requestHeaders = new Headers(request.headers);
        if (user) {
            // Handle different token formats
            const userId = user.aud || '';
            const userRole = user.role || 'user';
            const userEmail = user.email || '';

            // requestHeaders.set('x-user-id', userId);
            requestHeaders.set('x-user-role', userRole);
            requestHeaders.set('x-user-email', userEmail);
        }
        requestHeaders.set('x-is-authenticated', user ? 'true' : 'false');

        return NextResponse.next({
            request: {
                headers: requestHeaders,
            },
        });
    } catch (error) {
        console.error('Middleware error:', error);
        // Allow request to continue on error
        return NextResponse.next();
    }
}

export const config = {
    matcher: [
        /*
         * Match all request paths except for the ones starting with:
         * - api (API routes)
         * - _next/static (static files)
         * - _next/image (image optimization files)
         * - favicon.ico (favicon file)
         */
        '/((?!api|_next/static|_next/image|favicon.ico).*)',
    ],
};
