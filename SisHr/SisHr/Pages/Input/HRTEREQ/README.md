# HRTEREQ – Thông tin tuyển dụng / Recruitment Request

- **Menu_id**: `wr.ct.41`
- **Page**: `hrtereq`
- **Table**: `dmtuyendung` / View: `v_dmtuyendung`

## Files

| File | Mô tả |
|------|-------|
| `Hrtereq.cs` | Model row mapping bảng `dmtuyendung` |
| `FrmHrtereq.razor` | Form nhập liệu (Thêm / Sửa / Xem) |
| `FrmHrtereq.razor.cs` | Code-behind: load, validate, save, logic autocomplete |
| `HrtereqComponent.razor` | Trang danh sách + xử lý toolbar (F2/F3/F4/F8/Export) |

## Chức năng

- **F4 / New** – Thêm yêu cầu tuyển dụng mới
- **F3 / Edit** – Sửa thông tin
- **F2 / View** – Xem (readonly)
- **F8 / Delete** – Xóa (có xác nhận)
- **Export** – Xuất danh sách

## Validate

- Số yêu cầu: bắt buộc, không trùng
- Bộ phận (ma_bpns): bắt buộc
- Ngày yêu cầu: bắt buộc
