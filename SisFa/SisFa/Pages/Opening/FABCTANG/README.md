# FABCTANG - Bảng kê TSCĐ tăng trong kỳ

Báo cáo truy xuất danh sách các tài sản cố định tăng trong kỳ.

## Tính năng:
- **Lọc dữ liệu**: Theo khoảng thời gian (Từ kỳ - Đến kỳ), Năm.
- **Lọc nâng cao**: Lý do tăng, Nhóm tài sản, Loại tài sản, Mã BPSD, Mã DVCS.
- **Tiện ích**: Export Excel trực tiếp từ dữ liệu lưới, in báo cáo động.
- **Route / Page**: `/fabctang` & `/fabctang/{Menu_id}`
- **Menu_id Mặc định**: `wf.bc.12`

## Thành phần kỹ thuật:
- `FABCTANG.razor`: Component giao diện xử lý lưới DevExpress và form filter.
- `FABCTANGModel.cs`: Model cấu trúc dữ liệu phản chiếu chuỗi Data của Stored Procedure `FABCTANG`.