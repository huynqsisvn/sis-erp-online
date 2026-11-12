# Danh m?c Tài kho?n Ngân hàng (Bank Account Category)

## Thông tin chung
- **Menu ID**: `wa.dm.03`
- **Page**: `dmtknh`
- **Th? m?c**: `SisCa\SisCa\Pages\List\Catknh`

## C?u trúc Files

### 1. Dmtknh.cs - Model Class
Model cho b?ng `dmtknh` v?i các tr??ng:
- **tk** (Primary Key): Tài kho?n k? toán
- **tk_nh**: S? tài kho?n ngân hàng
- **ten_nh**: Tên ngân hàng
- **ten_nh2**: Tên ngân hàng 2
- **tinh_thanh**: T?nh thành
- **dien_thoai**: ?i?n tho?i
- **fax**: S? fax
- **e_mail**: ??a ch? email
- **home_page**: Trang web
- **doi_tac**: Ng??i liên h?
- **ghi_chu**: Ghi chú
- **status**: Tr?ng thái

### 2. DmtknhComponent.razor - List Component  
Component danh sách v?i:
- Routes: `/dmtknh` và `/dmtknh/{Menu_id}`
- Keyboard shortcuts (F2/F3/F4/F6/F8)
- Import/Export
- Change ID
- Authorization: `admin` ho?c `dmtknh`

### 3. EditDmtknh.razor - Edit Form
Form nh?p li?u v?i các ch? ??: View, Edit, New, Copy

## Các ch?c n?ng
- ? Xem (F2)
- ? Thêm m?i (F4)
- ? S?a (F3)
- ? Xóa (F8)
- ? Copy
- ? ??i mã (F6)
- ? Import t? Excel
- ? Export
- ? Tìm ki?m

## Database View
View `v_dmtknh` c?n join v?i b?ng `dmtk` ?? l?y tên tài kho?n:

```sql
CREATE VIEW v_dmtknh AS
SELECT 
    nh.*,
    tk.ten_tk,
    tk.ten_tk2
FROM dmtknh nh
LEFT JOIN dmtk tk ON nh.tk = tk.tk
```

## Validation
- Tài kho?n (tk): B?t bu?c
- Tên ngân hàng (ten_nh): B?t bu?c

## Phím t?t
- **F2**: Xem
- **F3**: S?a  
- **F4**: Thêm m?i
- **F6**: ??i mã
- **F8**: Xóa

## Default Values
- Status: "1" (?ang s? d?ng)

## Localization Keys C?n Thêm
```xml
<Item Key="Bank account number">Tài kho?n ngân hàng</Item>
<Item Key="Province/City">T?nh thành</Item>
<Item Key="Website">Trang web</Item>
<Item Key="Contact person">Ng??i liên h?</Item>
<Item Key="Account is required">Ch?a nh?p tài kho?n</Item>
<Item Key="Bank name is required">Ch?a nh?p tên ngân hàng</Item>
<Item Key="Change account code">??i mã tài kho?n</Item>
```

## Build Status
? Files created successfully
