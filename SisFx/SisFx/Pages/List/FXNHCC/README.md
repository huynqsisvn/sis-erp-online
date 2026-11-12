# FXNHCC - Danh mục phân nhóm công cụ dụng cụ

## Thông tin
- **Menu ID**: wf.xd.04
- **Page**: fxnhcc
- **Bảng**: dmnhcc
- **Khóa chính**: loai_nh, ma_nh (composite key)

## Files
- **Dmnhcc.cs**: Model
- **FxnhccComponent.razor**: Danh sách (Browse) + Lọc loại nhóm + Phân nhóm
- **EditFxnhcc.razor**: Form nhập liệu (Mã nhóm, Tên nhóm, Tên 2)
- **FrmFxnhcc.razor**: Popup wrapper
- **FrmPhanNhomCC.razor**: Chức năng phân nhóm CCDC (gán/hủy CCDC vào nhóm)

## Chức năng
- Lọc theo loại nhóm khi mở (popup chọn loại nhóm 1-3)
- Xem/Thêm/Sửa/Xóa/Copy nhóm CCDC
- Phân nhóm CCDC: gán/hủy công cụ vào nhóm (nút Phân nhóm trên toolbar)
- Phím tắt: F2 (Xem), F3 (Sửa), F4 (Thêm), F8 (Xóa)
- Kiểm tra tồn tại phát sinh trước khi cho sửa mã hoặc xóa (dmcc.nh_cc1/nh_cc2/nh_cc3)
