# HRNVNTS - Nhân viên ngh? thai s?n / Maternity Leave

## Thông tin
- **Menu_id**: wr.ct.19
- **Page**: hrnvnts
- **B?ng d? li?u**: dmnvnts
- **View**: v_dmnvnts
- **Khóa chính**: idts (GUID)

## Các file
| File | Mô t? |
|------|-------|
| `Hrnvnts.cs` | Model ánh x? b?ng dmnvnts |
| `HrnvntsPrintModel.cs` | Model in báo cáo |
| `FrmHrnvnts.razor` | Trang danh sách + toolbar (Browse) |
| `EditHrnvnts.razor` | Form thêm / s?a / xem |

## Ch?c n?ng
- **F2 / Xem**: Xem thông tin nhân viên ngh? thai s?n
- **F3 / S?a**: S?a thông tin
- **F4 / Thêm**: Thêm m?i
- **F7 / In**: In danh sách (m?u t? thi?t k?)
- **F8 / Xóa**: Xóa b?n ghi
- **K?t xu?t**: Xu?t Excel

## M?u in
Các tr??ng in: STT, Mã nhân viên, Tên nhân viên, Ngày sinh, B? ph?n, Ngày ngh? thai s?n, Ngày k?t thúc thai s?n, Ghi chú.
M?u designer t?o riêng theo file report.

## Ngôn ng?
C?p nh?t trong `hr-vi.xml` và `hr-en.xml` v?i prefix key `Maternity ...`.
