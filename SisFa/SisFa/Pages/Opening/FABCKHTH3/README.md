# FABCKHTH3 - Bảng tổng hợp khấu hao TSCĐ - theo 2 chỉ tiêu

- Menu ID: wf.bc.05
- Page: fabckhth3
- Folder: SisFa/SisFa/Pages/Opening/FABCKHTH3/

Ngắn gọn:
- Component Blazor chứa form lọc và lưới giống các báo cáo cùng thư mục.
- Dữ liệu lấy bằng stored procedure (đã có), component chỉ gọi myDb.LoadStoreProcedure.
- Tính năng: xem, in (Report), xuất Excel (Export). 

File chính:
- `FABCKHTH3Model.cs` - model dữ liệu
- `FABCKHTH3Cpn.razor` - component chính (form lọc + grid + export/print)

Ghi chú:
- Thiết kế theo mẫu `FABCKHTH2` để giữ nhất quán UI và chức năng.
