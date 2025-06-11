# 🚀 Dự án [Yen_Sao_Gold]

Đây là dự án [Next.js / React / Node.js] sử dụng Axios, Redux Toolkit, và cấu hình `.env` để gọi API.

---

## 🔧 Thiết lập môi trường `.env`

Để dự án chạy đúng, bạn cần tạo file `.env.local` h ở thư mục gốc với các biến sau:

### 🔑 nội dung `.env.local`

### 📍 Ý nghĩa

| Biến                       | Mô tả                                                                                                               |
| -------------------------- | ------------------------------------------------------------------------------------------------------------------- |
| `NEXT_PUBLIC_API_BASE_URL` | URL của server backend mà Axios sẽ gọi. Phải bắt đầu bằng `NEXT_PUBLIC_` để Next.js cho phép dùng trên trình duyệt. |

---

## 📁 Bước thiết lập đầy đủ

1. Tạo file `.env.local` trong thư mục gốc của project:

    ```bash
    touch .env.local

    ```

2. Thêm cái biên như trên vào file đó
3. Chạy lại project
