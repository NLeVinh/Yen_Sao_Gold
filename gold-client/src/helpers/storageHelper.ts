// cookie
export const setCookie = (name: string, value: string, expiresAt?: string) => {
    try {
        let cookieStr = `${name}=${value}; path=/`;
        if (expiresAt) {
            const expires = new Date(expiresAt).toUTCString();
            cookieStr += `; expires=${expires}`;
        }
        document.cookie = cookieStr;
    } catch (error) {
        console.error(`Error setting cookie "${name}":`, error);
    }
};

export const getCookie = (name: string): string | null => {
    try {
        const match = document.cookie.match(new RegExp('(^| )' + name + '=([^;]+)'));
        return match ? decodeURIComponent(match[2]) : null;
    } catch (error) {
        console.error(`Error getting cookie "${name}":`, error);
        return null;
    }
};

export const removeCookie = (name: string) => {
    try {
        document.cookie = `${name}=; path=/; expires=Thu, 01 Jan 1970 00:00:00 UTC`;
    } catch (error) {
        console.error(`Error removing cookie "${name}":`, error);
    }
};

// local storage
export const setLocal = (key: string, value: any) => {
    try {
        localStorage.setItem(key, JSON.stringify(value));
    } catch (error) {
        console.error(`Error setting localStorage key "${key}":`, error);
    }
};

export const getLocal = <T>(key: string): T | null => {
    try {
        const item = localStorage.getItem(key);
        return item ? JSON.parse(item) : null;
    } catch (error) {
        console.error(`Error getting localStorage key "${key}":`, error);
        return null;
    }
};

export const removeLocal = (key: string) => {
    try {
        localStorage.removeItem(key);
    } catch (error) {
        console.error(`Error removing localStorage key "${key}":`, error);
    }
};
