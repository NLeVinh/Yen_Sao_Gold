// constants/messages.ts

export const MESSAGES = {
    // Authentication Messages
    AUTH: {
        LOGIN: {
            INVALID_DATA: 'Dữ liệu đăng nhập không hợp lệ',
            DEFAULT_ERROR: 'Đăng nhập thất bại',
            SUCCESS: 'Đăng nhập thành công',
            INVALID_CREDENTIALS: 'Email hoặc mật khẩu không đúng',
            ACCOUNT_LOCKED: 'Tài khoản đã bị khóa',
            TOO_MANY_ATTEMPTS: 'Quá nhiều lần thử. Vui lòng thử lại sau',
        },

        REGISTER: {
            SUCCESS: 'Đăng ký thành công',
            DEFAULT_ERROR: 'Đăng ký thất bại',
            EMAIL_EXISTS: 'Email đã được sử dụng',
            USERNAME_EXISTS: 'Tên đăng nhập đã tồn tại',
            WEAK_PASSWORD: 'Mật khẩu quá yếu',
            PASSWORDS_NOT_MATCH: 'Mật khẩu xác nhận không khớp',
        },

        LOGOUT: {
            SUCCESS: 'Đăng xuất thành công',
            ERROR: 'Có lỗi khi đăng xuất',
        },

        COMMON: {
            NETWORK_ERROR: 'Lỗi kết nối mạng',
            SERVER_ERROR: 'Lỗi máy chủ',
            UNAUTHORIZED: 'Bạn không có quyền truy cập',
            SESSION_EXPIRED: 'Phiên đăng nhập đã hết hạn',
            TOKEN_INVALID: 'Token không hợp lệ',
        },
    },

    // Profile Messages
    PROFILE: {
        UPDATE: {
            SUCCESS: 'Cập nhật thông tin thành công',
            ERROR: 'Cập nhật thông tin thất bại',
            AVATAR_SUCCESS: 'Cập nhật ảnh đại diện thành công',
            AVATAR_ERROR: 'Cập nhật ảnh đại diện thất bại',
            PASSWORD_SUCCESS: 'Đổi mật khẩu thành công',
            PASSWORD_ERROR: 'Đổi mật khẩu thất bại',
            CURRENT_PASSWORD_WRONG: 'Mật khẩu hiện tại không đúng',
        },

        DELETE: {
            SUCCESS: 'Xóa tài khoản thành công',
            ERROR: 'Xóa tài khoản thất bại',
            CONFIRMATION_REQUIRED: 'Vui lòng xác nhận để xóa tài khoản',
        },

        VALIDATION: {
            PHONE_INVALID: 'Số điện thoại không hợp lệ',
            DATE_OF_BIRTH_INVALID: 'Ngày sinh không hợp lệ',
            NAME_REQUIRED: 'Họ tên là bắt buộc',
            NAME_TOO_SHORT: 'Họ tên phải có ít nhất 2 ký tự',
        },
    },

    // System Messages
    SYSTEM: {
        LOADING: 'Đang tải...',
        SAVING: 'Đang lưu...',
        PROCESSING: 'Đang xử lý...',
        NO_DATA: 'Không có dữ liệu',
        EMPTY_LIST: 'Danh sách trống',
        SEARCH_NO_RESULTS: 'Không tìm thấy kết quả',
        PAGE_NOT_FOUND: 'Trang không tồn tại',
        ACCESS_DENIED: 'Truy cập bị từ chối',
        MAINTENANCE: 'Hệ thống đang bảo trì',
        COMING_SOON: 'Tính năng sắp ra mắt',
    },

    // Validation Messages
    VALIDATION: {
        REQUIRED: 'Trường này là bắt buộc',
        EMAIL_INVALID: 'Email không hợp lệ',
        PASSWORD_MIN_LENGTH: 'Mật khẩu phải có ít nhất {min} ký tự',
        PASSWORD_MAX_LENGTH: 'Mật khẩu không được vượt quá {max} ký tự',
        PASSWORD_PATTERN: 'Mật khẩu phải chứa ít nhất 1 chữ hoa, 1 chữ thường và 1 số',
        PHONE_INVALID: 'Số điện thoại không hợp lệ',
        URL_INVALID: 'URL không hợp lệ',
        NUMBER_MIN: 'Giá trị phải lớn hơn hoặc bằng {min}',
        NUMBER_MAX: 'Giá trị phải nhỏ hơn hoặc bằng {max}',
        STRING_MIN_LENGTH: 'Phải có ít nhất {min} ký tự',
        STRING_MAX_LENGTH: 'Không được vượt quá {max} ký tự',
        FILE_SIZE_TOO_LARGE: 'Kích thước file quá lớn (tối đa {max}MB)',
        FILE_TYPE_INVALID: 'Định dạng file không được hỗ trợ',
    },

    // File Upload Messages
    FILE: {
        UPLOAD_SUCCESS: 'Tải file lên thành công',
        UPLOAD_ERROR: 'Tải file lên thất bại',
        DELETE_SUCCESS: 'Xóa file thành công',
        DELETE_ERROR: 'Xóa file thất bại',
        SIZE_TOO_LARGE: 'File quá lớn',
        TYPE_NOT_SUPPORTED: 'Định dạng file không được hỗ trợ',
        DRAG_DROP_HERE: 'Kéo thả file vào đây',
        CLICK_TO_SELECT: 'Hoặc click để chọn file',
    },
} as const;

export const formatMessage = (message: string, params: Record<string, string | number>): string => {
    return Object.entries(params).reduce(
        (formatted, [key, value]) => formatted.replace(`{${key}}`, String(value)),
        message
    );
};
