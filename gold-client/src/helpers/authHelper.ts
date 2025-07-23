import { jwtVerify, SignJWT, JWTPayload as JoseJWTPayload } from 'jose';

const JWT_SECRET = new TextEncoder().encode(process.env.JWT_SECRET || 'fallback-secret');
const JWT_ISSUER = process.env.JWT_ISSUER || 'YenSaoGoldIssuer';
const JWT_AUDIENCE = process.env.JWT_AUDIENCE || 'YenSaoGoldClient';

interface CustomJWTPayload extends JoseJWTPayload {
    sub: string; // user id
    email?: string;
    name?: string;
    role?: string;
    type?: string;
    'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier'?: string;
    TokenType?: string;
}

// Verify JWT Token (for access tokens)
export async function verifyJWT(token: string): Promise<CustomJWTPayload | null> {
    try {
        const result = await jwtVerify(token, JWT_SECRET, {
            issuer: JWT_ISSUER,
            audience: JWT_AUDIENCE,
        });

        // The payload from jwtVerify is always defined if verification succeeds
        const payload = result.payload as CustomJWTPayload;

        if (!payload) {
            return null;
        }

        return payload;
    } catch (error) {
        console.error('JWT verification failed:', error);
        return null;
    }
}

export async function verifyRefreshToken(token: string): Promise<{ userId: string } | null> {
    try {
        const { payload } = await jwtVerify(token, JWT_SECRET, {
            issuer: JWT_ISSUER,
            audience: JWT_AUDIENCE,
        });

        if (payload.type !== 'refresh') {
            return null;
        }

        return { userId: payload.sub as string };
    } catch (error) {
        console.error('Refresh token verification failed:', error);
        return null;
    }
}
