# HRKN - Danh m?c Kh? n?ng

**Menu_id:** `wr.dm.21` | **Page:** `hrkn` | **Table:** `DMKN` | **View:** `v_DMKN`

## Các file

| File | Mô t? |
|------|-------|
| `Dmkn.cs` | Model ánh x? b?ng DMKN (t?t c? các c?t bao g?m tài li?u) |
| `HrknComponent.razor` | Trang danh sách, route `/hrkn` |
| `EditHrkn.razor` | Form thêm/s?a/xem/copy v?i 2 tab (thông tin chính + tài li?u) |
| `FrmHrkn.razor` | Popup wrapper cho EditHrkn |

## C?u trúc Form

### Tab 1: Thông tin chính
- Mã kh? n?ng (`ma_kn`) — b?t bu?c
- Tên kh? n?ng (`ten_kn`) — b?t bu?c
- Tên kh? n?ng 2 (`ten_kn2`)
- Ghi chú (`ghi_chu`)
- Tr?ng thái (`status`) — 0/1

### Tab 2: Tài li?u
- 3 b? tài li?u, m?i b? g?m:
  - Mã tài li?u (`ma_td`)
  - Ngày tài li?u (`ngay_td`)
  - S? l??ng (`sl_td`)
  - Ghi chú tài li?u (`gc_td`)

## Ch?c n?ng

? Thêm, S?a, Xem, Copy, ??i mã, K?t xu?t, Xóa
? Validate: ki?m tra b?t bu?c, trùng mã, l?ng nhau
? Validate xóa: không xóa khi ?ã phát sinh
? ?a ngôn ng?: Vi?t/Anh (t? `hr-vi.xml`)
