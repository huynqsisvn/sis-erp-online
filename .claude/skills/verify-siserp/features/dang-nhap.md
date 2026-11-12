# Tính năng: Đăng nhập

Nguồn sự thật: `SisCom/SisCom/Pages/LoginComponent.razor`. Mọi tên bên dưới lấy từ file này,
từ `SISERPSME/Program.cs`, `SISERPSME/wwwroot/Lang/vi.xml` và `SisCom/SisCom/Services/Totp/*`.

> **Trạng thái xác minh:** phần "Hành vi" suy ra từ mã nguồn. **Chưa có lần chạy thật nào**
> (xem `../evidence/2026-11-12-run1.md`: ứng dụng không dựng được trong sandbox).
> Các ô đánh dấu `[chưa chạy]` phải được xác nhận ở lần chạy đầu tiên có môi trường đầy đủ.

## Trạng thái ban đầu

- Chưa đăng nhập; trình duyệt mới (không có `localStorage`).
- DB có sẵn tài khoản thử. Có hai nguồn, tra theo thứ tự:
  1. bảng `userinfo` qua `myDb.GetUserbyname(user_name, Enc.Encrypt(password))`
     (`GetUserbyname` và `Enc` nằm trong DLL, repo không có mã nguồn);
  2. bảng `userinfo_web` (`user_name`, `password`) nếu bước 1 trả `null`. Người dùng này chạy với quyền của
     tài khoản `Guess`.
- TOTP tắt: bảng `sys_totp_setting` không tồn tại hoặc `is_enabled = 0`
  (`TotpSettingService.GetEnabledAsync` bắt ngoại lệ và trả `false`).
  Tài khoản `Is_admin == 1` luôn bỏ qua TOTP.
- Tài khoản thử không bị khóa (`Islocked != "1"`).
- Không đoán thuật toán mã hóa mật khẩu. Hãy dùng một tài khoản thử đã có trong DB thử, hoặc tạo qua chức năng
  quản trị người dùng của chính ứng dụng.

## Route / endpoint

| Mục | Giá trị | Nguồn |
|---|---|---|
| Trang đăng nhập | `GET /` (`@page "/"`, `@layout Empty`) | `LoginComponent.razor:1-2` |
| Kênh tương tác | SignalR `/_blazor` (`MapBlazorHub`) | `Program.cs` |
| Không có API đăng nhập REST | Không có controller đăng nhập. Controllers hiện có: `CultureController`, `Qrkiemke`, `ReportingControllers`, `SystemController`, `UploadController` | `SISERPSME/Controllers` |
| Sau khi đăng nhập thành công | `NavigateTo(BaseUri + defaultUrl)`. Mặc định `about` → `/about` (`SisCom/Shared/About.razor`). Nếu `Wr_fav` của user có menu: `{Mobile_page}/{Menu_id với '.' → '_'}` | `LoginComponent.razor:496-521, 597` |
| URL chạy local | `https://localhost:1170`, `http://localhost:1169` | `SISERPSME/Properties/launchSettings.json` |

## Selector

| Phần tử | Selector | Nguồn |
|---|---|---|
| Form | `form.login-formv` (`EditForm`, `OnValidSubmit="Onsubbmit"`) | dòng 314 |
| Tên đăng nhập | `form.login-formv .inputWithIcon input[type="text"]` (không có `id`; placeholder là khóa `username`) | dòng 315-316 |
| Mật khẩu | `#tbPassword` (`DxTextBox`, `InputId="tbPassword"`) | dòng 322 |
| Nút đăng nhập | `#btnlogin` | dòng 333 |
| Thông báo lỗi | `h6.text-danger` (ban đầu chứa khoảng trắng `"  "`) | dòng 310 |
| Ô mã TOTP | `input[maxlength="6"]` | dòng 369, 384 |
| Nút xác nhận TOTP | `button.totp-confirm-btn` | dòng 375, 390 |
| Lỗi TOTP | `.totp-input ~ h6.text-danger` | dòng 371-373 |

## Payload

Không có JSON. Blazor gửi sự kiện qua SignalR. Mô hình dữ liệu là `Users` (trong `SisData.Model`, không có
mã nguồn): `User_name` (đã `Trim()`), `Password` (mã hóa bằng `Enc.Encrypt`).

## Hành động và kết quả mong đợi

| # | Hành động | Kết quả mong đợi | Trạng thái |
|---|---|---|---|
| A | Nhập tài khoản và mật khẩu đúng (không TOTP) → bấm `#btnlogin` | Chuyển khỏi `/` (mặc định `/about`). Thêm 1 dòng vào bảng `userlogs`: `menu_id = 'LOGIN\<User_id>'`, `user_id = <user_name>`, `host = <session_guid>` | [chưa chạy] |
| B | Đúng tên, sai mật khẩu | Ở lại `/`. `h6.text-danger` = `Tên đăng nhập hoặc mật khẩu không đúng !` (khóa `errnameorpass`, `vi.xml`). Không thêm dòng `userlogs` | [chưa chạy] |
| C | Tên không tồn tại | Giống B | [chưa chạy] |
| D | Để trống tên hoặc mật khẩu | Giống B (nhánh `IsNullOrEmpty`, không truy vấn DB) | [chưa chạy] |
| E | Tài khoản `Islocked == "1"` | Ở lại `/`, hiện thông báo khóa. Lưu ý khóa lệch chữ hoa: mã dùng `"...contact administrator!"`, `vi.xml` có `"...contact Administrator!"` → có thể hiện nguyên khóa thay vì bản dịch | [chưa chạy] |
| F | TOTP bật, tài khoản không phải admin | Không chuyển trang. Hiện `input[maxlength="6"]`. Lần đầu có QR và `code.totp-secret`, nhập mã 6 số đúng → vào hệ thống. Sai mã → `Mã xác thực không đúng, vui lòng thử lại` | [chưa chạy] |

`scripts/drive-login.js` hiện tự động hóa A, B, C: `--expect success` cho A và `--expect failure` cho B, C.
D, E, F làm thủ công hoặc mở rộng script.

## Điều kiện lỗi và dấu hiệu môi trường hỏng

- `Onsubbmit` là `async void`: nếu DB không kết nối được, ngoại lệ bị bắt và chỉ ghi vào file qua
  `WLog` (`@"..\Sqlerr.txt"`). **Giao diện không hiện thông báo nào**, tức `h6.text-danger` vẫn trống sau khi bấm.
  Trống ≠ đăng nhập đúng: phải kiểm tra URL và `Sqlerr.txt`.
- Trên Linux, `\` không phải dấu phân tách đường dẫn nên `..\Sqlerr.txt` và `..\Totp_debug.txt`
  tạo file tên đúng như vậy trong thư mục làm việc. `Sqlerr.txt`, `Totp_debug.txt`, `AutoCompleteerr.txt` ở gốc repo
  là file đã được commit. Lần chạy không được để chúng đổi nội dung (xem Cleanup).
- `errnameorpass` không có trong `SISERPSME/wwwroot/Lang/en.xml` (chỉ có `vi.xml`, `fa-vi.xml`), nên khi `userlang` là
  `EN` thông báo lỗi có thể hiện sai. Chạy bằng ngôn ngữ mặc định `Vi`.
- Mật khẩu mặc định của tài khoản `Guess` được viết cứng trong mã (hash). Không dùng và không sao chép.
