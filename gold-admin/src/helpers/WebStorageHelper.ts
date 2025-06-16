type StorageType = 'localStorage' | 'sessionStorage'

class WebStorageHelper {
    private storage: Storage

    constructor(type: StorageType) {
        this.storage = type === 'localStorage' ? localStorage : sessionStorage
    }

    setStorageItem<T>(key: string, value: T): void {
        try {
            const serialized = JSON.stringify(value)
            this.storage.setItem(key, serialized)
        } catch (error) {
            console.error('Error setting item to storage:', error)
        }
    }

    getStorageItem<T>(key: string): T | null {
        try {
            const serialized = this.storage.getItem(key)
            if (serialized === null) {
                return null
            }
            return JSON.parse(serialized)
        } catch (error) {
            console.error('Error getting item from storage:', error)
            return null
        }
    }

    removeStorageItem(key: string): void {
        try {
            this.storage.removeItem(key)
        } catch (error) {
            console.error('Error removing item from storage:', error)
        }
    }

    clear(): void {
        try {
            this.storage.clear()
        } catch (error) {
            console.error('Error clearing storage:', error)
        }
    }
}

// create instances for local and session storage
const localStorageHelper = new WebStorageHelper('localStorage')
const sessionStorageHelper = new WebStorageHelper('sessionStorage')

export { localStorageHelper, sessionStorageHelper }
