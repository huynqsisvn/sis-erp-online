# FANHTS - Danh mục phân nhóm tài sản cố định

## Thông tin
- **Menu ID**: wf.dm.06
- **Page**: fanhts
- **Bảng**: dmnhts
- **Khóa chính**: loai_nh, ma_nh (composite key)

## Files
- **Dmnhts.cs**: Model
- **FanhtsComponent.razor**: Danh sách (Browse) + Lọc loại nhóm + Phân nhóm
- **EditFanhts.razor**: Form nhập liệu (3 trường: Mã nhóm, Tên nhóm, Tên 2)
- **FrmFanhts.razor**: Popup wrapper
- **FrmPhanNhomTS.razor**: Chức năng phân nhóm tài sản (gán/hủy TS vào nhóm)

## Chức năng
- Lọc theo loại nhóm khi mở (popup chọn loại nhóm 1-9)
- Xem/Thêm/Sửa/Xóa/Copy nhóm tài sản
- Phân nhóm tài sản: gán/hủy tài sản vào nhóm (nút Đổi mã trên toolbar)
- Phím tắt: F2 (Xem), F3 (Sửa), F4 (Thêm), F8 (Xóa)
- Kiểm tra tồn tại phát sinh trước khi cho sửa mã (dmts.nh_ts1/nh_ts2/nh_ts3)
