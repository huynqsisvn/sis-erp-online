# Vào Số Dư Ban Đầu Các Khế Ước (Contract Opening Balance)

## Thông tin chung
- **Menu ID**: `wa.sd.01`
- **Page**: `cakusd0`
- **Thư mục**: `SisCa\SisCa\Pages\Opening\Cakusd0`
- **Bảng dữ liệu**: `cdku` (view: `v_cdku`)

## Cấu trúc Files

### 1. Cakusd0.cs - Model  
Model đơn giản cho bảng `cdku` với các trường chính:
- **nam, tk, ma_ku** (Composite Key)
- **du_no00, du_co00**: Dư nợ/có VND
- **du_no_nt00, du_co_nt00**: Dư nợ/có ngoại tệ
- **ma_dvcs, ma_hd, ma_vv, ma_bpht**: Phân loại
- Audit fields: date, time, user_id, etc.

### 2. Cakusd0Component.razor - List Component
- Routes: `/cakusd0`, `/cakusd0/{Menu_id}`
- Filter: **Tài khoản** (Account filter only)
- Date badges: Data entry start date, Financial year start date
- Keyboard shortcuts: F2 (View), F3 (Edit), F4 (New), F8 (Delete)
- Import/Export support
- Data entry lock check

### 3. EditCakusd0.razor - Edit Form
Form nhập số dư với các trường:
- Mã ĐVCS (read-only)
- Mã khế ước (autocomplete)
- Tk tiền vay (autocomplete)
- Mã ngoại tệ (read-only)
- Dư có/nợ ban đầu VND
- Dư có/nợ ban đầu ngoại tệ

## Chức năng
- ✅ Xem số dư (F2)
- ✅ Thêm mới số dư (F4)
- ✅ Sửa số dư (F3)
- ✅ Xóa (F8)
- ✅ Import từ Excel
- ✅ Export
- ✅ Filter theo tài khoản
- ✅ Kiểm tra khóa sổ
- ✅ Hiển thị ngày bắt đầu nhập liệu và ngày đầu năm tài chính

## Filter Layout
Giống Arsd0 (Customer Opening Balance):
```
[Tài khoản: 131] [Ngày bắt đầu nhập liệu: 01-01-2025] [Ngày đầu năm tài chính: 01-01-2025]
```

## Validation
- Mã đơn vị: Bắt buộc
- Tài khoản: Bắt buộc
- Mã khế ước: Bắt buộc
- Không cho nhập đồng thời dư nợ và dư có
- Kiểm tra khóa sổ trước khi thêm/sửa/xóa

## Logic nghiệp vụ
- Khi nhập dư nợ, tự động xóa dư có
- Khi nhập dư có, tự động xóa dư nợ
- Ngoại tệ chỉ nhập được khi tài khoản là tài khoản ngoại tệ
- Composite key: nam + tk + ma_ku
- Kiểm tra M_NGAY_KS >= M_NGAY_KY1 để khóa nhập liệu

## Keyboard Shortcuts
- **F2**: View
- **F3**: Edit
- **F4**: New
- **F8**: Delete (removed from OnKeyDown, use toolbar)

## Localization Keys
```xml
<Item Key="Loan account">Tk tiền vay</Item>
<Item Key="Opening Dr. balance VND">Dư có ban đầu VND</Item>
<Item Key="Opening Dr. balance FC">Dư có ban đầu n.tệ</Item>
<Item Key="Opening Cr. balance VND">Dư nợ ban đầu VND</Item>
<Item Key="Opening Cr. balance FC">Dư nợ ban đầu n.tệ</Item>
<Item Key="Data entry start date">Ngày bắt đầu nhập liệu</Item>
<Item Key="Define financial year">Ngày đầu năm tài chính</Item>
<Item Key="Data entry locked until">Dữ liệu đã khóa đến ngày</Item>
```

## Build Status
✅ Files created successfully
✅ Filter layout matches Arsd0
✅ No compilation errors

