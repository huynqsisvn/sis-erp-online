# FACDPSTS - Bảng cân đối phát sinh TSCĐ

- Menu ID: wf.bc.24
- Page: facdpsts
- Folder: SisFa/SisFa/Pages/Opening/Facdpsts/

Ngắn gọn:
- Component Blazor chứa form lọc và grid giống các báo cáo khác.
- Dữ liệu lấy bằng stored procedure (trong DB) — component gọi `myDb.LoadStoreProcedure`.
- Tính năng: xem (browse), in (report), xuất Excel (Export).

Ghi chú nhanh:
- Form lọc gồm: Năm, Từ kỳ, Đến kỳ, Mã VND/ngoại tệ (mặc định VND), nhóm tài sản, loại tài sản, tk cp, tk kh, nh_ts1/2/3, mã bpsd.
- Vbrowse cấu hình trong `curCommand.Vbrowse1` giống mẫu đã cho trong yêu cầu.
- Không thêm truy vấn SQL mới; sử dụng stored procedure tên `FACDPSTS` (mặc định) hoặc lấy từ `curCommand.Store_proc`.
