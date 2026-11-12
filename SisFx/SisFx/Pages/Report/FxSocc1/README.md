# FxSocc1 – Sổ CCDC

## Thông tin chung
- **Menu_id**: `wf.xb.17`
- **Trang**: `/fxsocc1`
- **Stored Procedure**: `FxSocc1`

## Điều kiện lọc
- Năm
- Không hiện các CCDC giảm trước ngày
- Ngày mở sổ
- Mẫu VND/Ngoại tệ (read-only, mặc định VND)
- Lọc nâng cao: nhóm công cụ, loại cc, mã BPSD, TK chi phí, nhóm cc 1/2/3

## Chức năng
- Kết xuất Excel (Export)
- In báo cáo (Print) — tự động xuất PDF theo ngôn ngữ hiện tại (V/ hoặc E/)
- Thay đổi ĐVCS trên thanh tiêu đề → dữ liệu lưới tự cập nhật

## Cấu trúc file

| File | Mô tả |
|------|-------|
| `FxSocc1Model.cs` | Model dữ liệu (FxSocc1Model, FxSocc1Row) — dùng cho browse grid |
| `FxSocc1PrintModel.cs` | Model dữ liệu in (FxSocc1PrintModel, FxSocc1PrintRow) — dùng cho report |
| `FxSocc1Cpn.razor` | Component Blazor: form lọc + màn hình brow + chức năng in/export |
| `V/FxSocc1.cs` | Mẫu in tiếng Việt (code-behind) |
| `V/FxSocc1.Designer.cs` | Mẫu in tiếng Việt (designer) |
| `E/FxSocc1.cs` | Mẫu in tiếng Anh (code-behind) |
| `E/FxSocc1.Designer.cs` | Mẫu in tiếng Anh (designer) |
| `README.md` | File này |

## Cấu trúc cột in báo cáo

**PageHeader (Tiêu đề cột):**
- STT, Số CT, Ngày CT, Mã CC, Tên CC, Nguyên Giá, ĐVT, Ghi Chú

**Detail (Chi tiết dòng):**
- Stt, So_ct, Ngay_ct, Ma_cc, Ten_cc, Nguyen_gia, Dvt, Ghi_chu

**ReportFooter (Kết thúc báo cáo):**
- Tổng cộng
- Ký tên: Người lập biểu, Kế toán trưởng (hoặc tương đương tiếng Anh)
- Ngày tháng năm

## Hướng dẫn tùy chỉnh Designer

Để chỉnh sửa mẫu in:

1. **Mở file Designer:**
   - `V/FxSocc1.Designer.cs` (cho tiếng Việt)
   - `E/FxSocc1.Designer.cs` (cho tiếng Anh)

2. **Các phần chính có thể tùy chỉnh:**
   - **ReportHeader**: Thay đổi `lbltitle.Text`, `lblten_cty.Text`, `lbldia_chi.Text`
   - **PageHeader**: Thêm/xóa cột trong bảng `table1` (tableCell1 → tableCell8)
   - **Detail**: Thêm/xóa cột trong bảng `table3` (tableCell9 → tableCell16)
   - **ReportFooter**: Chỉnh sửa ký tên và ngày tháng

3. **Thêm trường mới:**
   - Thêm property vào `FxSocc1PrintRow` trong `FxSocc1PrintModel.cs`
   - Thêm mapping trong `Print()` method của `FxSocc1Cpn.razor`
   - Thêm cột mới trong bảng Detail của Designer

## Localization

Các label và text được dịch thông qua file `fx-vi.xml` (tiếng Việt) và `fx-en.xml` (tiếng Anh):

```xml
<Item Key="Tool ledger">Sổ CCDC</Item>
<Item Key="Do not show tools decreased before date:">Không hiện các CCDC giảm trước ngày:</Item>
<Item Key="Book opening date:">Ngày mở sổ:</Item>
```

## Chức năng Print (In báo cáo)

**Cách hoạt động:**
1. Người dùng click nút "Print" (F7 hoặc toolbar)
2. Component lấy dữ liệu từ `TblData` (bảng hiển thị trên màn hình)
3. Tạo object `FxSocc1PrintModel` và populate dữ liệu chi tiết
4. Chọn mẫu in dựa trên ngôn ngữ hiện tại: `V.FxSocc1` (Việt) hoặc `E.FxSocc1` (Anh)
5. Tạo instance của report, gán DataSource
6. Xuất PDF và tải về trình duyệt

**Lưu ý:**
- PDF được đặt tên: `FxSocc1_yyyyMMdd_HHmmss.pdf`
- Cần JS function `downloadFile()` trong app.js để xử lý tải file

## Liên kết với Database

**Stored Procedure `FxSocc1`:**
```sql
EXEC FxSocc1 
    @Nam = 2024,
    @Ky = 12,
    @Ngay_giam_ts = NULL,
    @Ma_bpcc = 'BP001',
    @Advance = '1=1',
    @group = 1
```

**Tham số:**
- `@Nam`: Năm (int)
- `@Ky`: Kỳ (int, 1-12)
- `@Ngay_giam_ts`: Ngày giảm (datetime, nullable)
- `@Ma_bpcc`: Mã bộ phận (varchar, nullable)
- `@Advance`: Điều kiện lọc nâng cao (nvarchar 4000)
- `@group`: Kiểu nhóm (int, 1 = chi tiết, >1 = tóm tắt)

