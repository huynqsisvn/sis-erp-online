# HRCPCCDC - Cấp phát công cụ dụng cụ

**Menu_id:** `wr.ct.31`  
**Page route:** `/hrcpccdc`  
**Thư mục:** `SisHr/SisHr/Pages/Input/HRCPCCDC/`

## Các file

| File | Mô tả |
|---|---|
| `Hrcpccdc.cs` | Model: `HrcpccdcRow`, `HrcpccdcDtRow` |
| `HrcpccdcModel.cs` | Model in: `HrcpccdcModel`, `HrcpccdcDtPrint` |
| `FrmHrcpccdc.razor` | Form nhập liệu (header + lưới chi tiết) |
| `FrmHrcpccdc.razor.cs` | Code-behind: load, save, validate, grid logic |
| `HrcpccdcComponent.razor` | Trang danh sách, toolbar, thêm/sửa/xem/xóa/in/kết xuất |
| `V/HRCPCCDC.cs` | Report class |
| `V/HRCPCCDC.Designer.cs` | Report designer (programmatic) |
| `V/HRCPCCDC.resx` | Report resources |

## Bảng dữ liệu

- **Bảng phần đầu:** `DMCPCCDC` — view: `v_DMCPCCDC`
- **Bảng chi tiết:** `DMCPCCDCDT` — view: `v_DMCPCCDCDT`
- **View danh sách:** `v_DMCPCCDCDTDetails`
- **Khóa chính:** `so_ct`

## Chức năng

- **Thêm (F4):** Nhập ngày, số chứng từ, diễn giải + lưới chi tiết (mã NV, mã CCDC, số lượng, ngày cấp phát, ngày hết hạn, ghi chú)
- **Sửa (F3):** Load dữ liệu, xóa chi tiết cũ rồi insert lại
- **Xem (F2):** Chỉ đọc
- **Xóa (F8):** Xóa chi tiết trước, sau đó xóa phần đầu
- **In (F7):** In mẫu cấp phát công cụ dụng cụ
- **Kết xuất:** Export danh sách

## Validate

- Số chứng từ bắt buộc, không trùng (khi thêm mới)
- Ngày chứng từ bắt buộc
- Phải có ít nhất một dòng chi tiết hợp lệ (có mã NV và mã CCDC)
- Dòng lưới không có mã NV hoặc mã CCDC sẽ bị bỏ qua khi lưu
