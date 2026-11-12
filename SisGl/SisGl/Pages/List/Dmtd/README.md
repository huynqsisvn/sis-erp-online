# Ch?c n?ng c?p nh?t Danh m?c t? do (DMTD)

## Mô t?
Module qu?n lý danh m?c t? do (Free Code) cho h? th?ng k? toán.

## Các file ?ã t?o

### 1. **Dmtd.cs** - Model class
- ???ng d?n: `SisGl\SisGl\Pages\List\Dmtd\Dmtd.cs`
- Ch?c n?ng: ??nh ngh?a c?u trúc d? li?u cho danh m?c t? do
- Các tr??ng chính:
  - `ma_td`: Mã t? do (khóa chính)
  - `ten_td`: Tên t? do
  - `ten_td2`: Tên t? do 2
  - `ma_tra_cuu`: Mã tra c?u
  - `ma_dvcs`: Mã ??n v? c? s?
  - `status`: Tr?ng thái s? d?ng
  - `ghi_chu`: Ghi chú
  - Các tr??ng audit: Date, Time, User_id, User_name (cho c? create và update)

### 2. **DmtdComponent.razor** - Trang danh sách
- ???ng d?n: `SisGl\SisGl\Pages\List\Dmtd\DmtdComponent.razor`
- Route: `/Dmtd` ho?c `/Dmtd/{Menu_id}`
- Ch?c n?ng:
  - Hi?n th? danh sách các mã t? do
  - Tìm ki?m, s?p x?p
  - Thêm m?i, s?a, xóa, sao chép
  - Thay ??i mã (Change ID)
  - **H? tr? phím t?t (Keyboard Shortcuts)**
- Authorization: Yêu c?u role `admin` ho?c `dmtd`

### 3. **EditDmtd.razor** - Form thêm/s?a
- ???ng d?n: `SisGl\SisGl\Pages\List\Dmtd\EditDmtd.razor`
- Ch?c n?ng:
  - Form nh?p li?u cho danh m?c t? do
  - H? tr? các ch? ??: View, New, Edit, Copy
  - Validation d? li?u
  - Auto-complete cho các tr??ng liên k?t (ma_dvcs)
  - H? tr? ?a ngôn ng? (Ti?ng Vi?t/English)

### 4. **FrmDmtd.razor** - Form popup
- ???ng d?n: `SisGl\SisGl\Pages\List\Dmtd\FrmDmtd.razor`
- Route: `/frmdmtd`, `/frmdmtd/{menu_id}`, `/frmdmtd/{menu_id}/{ma_td}`
- Ch?c n?ng: Hi?n th? EditDmtd trong popup window

## Phím t?t (Keyboard Shortcuts)

Module h? tr? các phím t?t sau trên trang danh sách:

| Phím | Ch?c n?ng | Mô t? |
|------|-----------|-------|
| **F2** | Xem (View) | Xem thông tin chi ti?t c?a dòng ?ang ch?n |
| **F3** | S?a (Edit) | S?a thông tin c?a dòng ?ang ch?n |
| **F4** | Thêm m?i (New) | T?o m?i m?t b?n ghi |
| **F6** | Thay ??i mã (Change ID) | Thay ??i mã c?a dòng ?ang ch?n |
| **F8** | Xóa (Delete) | Xóa dòng ?ang ch?n |
| **Double-click** | Xem (View) | Xem thông tin chi ti?t b?ng cách double-click vào dòng |

### L?u ý v? phím t?t:
- Các phím t?t ch? ho?t ??ng khi không có popup/dialog nào ?ang m?
- ?? s? d?ng phím t?t, ??m b?o grid ?ang ???c focus
- Phím t?t F6 (Change ID) ch? hi?n th? n?u ???c c?u hình trong `dmdm.doi_ma`

## C?u trúc Database

### B?ng: `dmtd`
```sql
CREATE TABLE dmtd (
    ma_td VARCHAR(20) PRIMARY KEY,      -- Mã t? do
    ten_td NVARCHAR(100),                -- Tên t? do
    ten_td2 NVARCHAR(100),               -- Tên t? do 2
    ma_tra_cuu VARCHAR(50),              -- Mã tra c?u
    ma_dvcs VARCHAR(20),                 -- Mã ??n v? c? s?
    status INT DEFAULT 1,                -- Tr?ng thái (1: ?ang dùng, 0: Không dùng)
    ghi_chu NVARCHAR(500),               -- Ghi chú
    
    -- Audit fields
    [date] DATE,                         -- Ngày s?a
    [time] VARCHAR(8),                   -- Gi? s?a
    user_id VARCHAR(20),                 -- ID ng??i s?a
    user_name NVARCHAR(100),             -- Tên ng??i s?a
    date0 DATE,                          -- Ngày t?o
    time0 VARCHAR(8),                    -- Gi? t?o
    user_id0 VARCHAR(20),                -- ID ng??i t?o
    user_name0 NVARCHAR(100)             -- Tên ng??i t?o
);
```

### View: `v_dmtd`
```sql
CREATE VIEW v_dmtd AS
SELECT 
    ma_td,
    ten_td,
    ten_td2,
    ma_tra_cuu,
    ma_dvcs,
    status,
    ghi_chu,
    [date],
    [time],
    user_id,
    user_name,
    date0,
    time0,
    user_id0,
    user_name0
FROM dmtd;
```

## C?u hình Menu

Thêm vào b?ng `dmdm` (Danh m?c danh m?c):

```sql
INSERT INTO dmdm (menu_id, table_name, table_view, key_field, title, fields, order_by)
VALUES (
    'wg.dm.td',                          -- Menu ID
    'dmtd',                              -- Tên b?ng
    'v_dmtd',                            -- Tên view
    'ma_td',                             -- Khóa chính
    'Danh m?c t? do.Free Code',          -- Tiêu ?? (VN.EN)
    'ma_td:120:h=Mã t? do:FL;ten_td:250:h=Tên t? do;ten_td2:250:h=Tên t? do 2;ma_tra_cuu:120:h=Mã tra c?u;ma_dvcs:100:h=??n v?;status:80:h=Tr?ng thái:C',
    'ma_td'                              -- S?p x?p theo
);
```

## C?u hình Quy?n (Authorization)

Thêm role `dmtd` vào b?ng `dmrole` ho?c gán quy?n cho user:

```sql
-- Thêm role m?i
INSERT INTO dmrole (role_id, role_name) VALUES ('dmtd', 'Danh m?c t? do');

-- Gán quy?n cho user
INSERT INTO user_roles (user_id, role_id) VALUES ('your_user_id', 'dmtd');
```

## S? d?ng

### 1. Truy c?p trang danh sách
- URL: `https://your-domain/Dmtd`
- Ho?c t? menu: Danh m?c > Danh m?c t? do

### 2. Thêm m?i
- Click nút "Thêm m?i" (New) ho?c nh?n **F4**
- Nh?p thông tin:
  - **Mã t? do**: B?t bu?c, t? ??ng sinh ho?c nh?p th? công
  - **Tên t? do**: B?t bu?c
  - **Tên t? do 2**: Tùy ch?n (cho ti?ng Anh ho?c tên khác)
  - **Mã tra c?u**: Tùy ch?n
  - **??n v? c? s?**: Ch?n t? danh sách
  - **Tr?ng thái**: 1 = ?ang s? d?ng, 0 = Không s? d?ng
  - **Ghi chú**: Tùy ch?n
- Click "L?u" (Save)

### 3. S?a
- Double-click vào dòng c?n s?a
- Ho?c ch?n dòng và click "S?a" (Edit) ho?c nh?n **F3**
- L?u ý: Không th? s?a mã khi ? ch? ?? Edit

### 4. Xem
- Ch?n dòng và nh?n **F2**
- Ho?c double-click vào dòng

### 5. Sao chép
- Ch?n dòng c?n copy
- Click "Sao chép" (Copy)
- H? th?ng s? t?o b?n ghi m?i v?i mã t? ??ng

### 6. Xóa
- Ch?n dòng c?n xóa
- Click "Xóa" (Delete) ho?c nh?n **F8**
- Xác nh?n xóa

### 7. Thay ??i mã
- Ch?n dòng c?n thay ??i mã
- Click "Thay ??i mã" (Change ID) ho?c nh?n **F6**
- Nh?p mã m?i
- H? th?ng s? c?p nh?t toàn b? các b?ng liên quan

## Tích h?p v?i các module khác

Danh m?c t? do ???c s? d?ng trong:
- Phi?u k? toán (ma_td, ma_td2, ma_td3)
- Các báo cáo t?ng h?p
- Tra c?u và phân tích d? li?u

## ?a ngôn ng?

Module h? tr? ?a ngôn ng? (Ti?ng Vi?t/English) thông qua:
- `IStringLocalizer<SisGl.Pages.App>` cho các label chung
- `IStringLocalizer<DmtdComponent>` cho các label riêng

## Validation

- **Mã t? do**: Không ???c r?ng, t? ??ng chuy?n v? ch? hoa
- **Tên t? do**: Không ???c r?ng
- Ki?m tra trùng mã khi thêm m?i/sao chép

## Audit Trail

H? th?ng t? ??ng l?u:
- Ng??i t?o, ngày gi? t?o
- Ng??i s?a cu?i, ngày gi? s?a cu?i

## L?u ý k? thu?t

1. **K? th?a t? `ListBaseComponent`**: S? d?ng các ch?c n?ng base nh? phân trang, tìm ki?m, toolbar, **keyboard shortcuts**
2. **K? th?a t? `FrmListBaseComponent`**: Form có s?n các ch?c n?ng validation, audit
3. **Auto-complete**: S? d?ng component `AutocompleteComponent` cho các tr??ng liên k?t
4. **Responsive**: Form t? ??ng ?i?u ch?nh theo kích th??c màn hình
5. **Keyboard navigation**: H? tr? phím Tab ?? di chuy?n gi?a các tr??ng và phím F2-F8 cho các thao tác CRUD
6. **JavaScript Integration**: S? d?ng `blazorKeyListener` ?? b?t các s? ki?n bàn phím toàn c?c

## Troubleshooting

### L?i: "Authorization failed"
- Ki?m tra user ?ã ???c gán role `admin` ho?c `dmtd`

### L?i: "Table 'dmtd' not found"
- T?o b?ng `dmtd` và view `v_dmtd` trong database

### L?i: "Menu not found"
- Thêm c?u hình vào b?ng `dmdm` v?i menu_id = 'wg.dm.td'

### Form không hi?n th? ?úng
- Ki?m tra c?u hình `fields` trong b?ng `dmdm`
- Xóa cache browser và reload

### Phím t?t không ho?t ??ng
- Ki?m tra JavaScript file `blazorKeyListener` ?ã ???c load
- ??m b?o grid ?ang ???c focus (click vào grid tr??c khi dùng phím t?t)
- Ki?m tra console browser ?? xem có l?i JavaScript không

## M? r?ng

?? m? r?ng ch?c n?ng:
1. Thêm tr??ng vào model `Dmtd.cs`
2. Thêm UI control vào `EditDmtd.razor`
3. C?p nh?t database schema
4. C?p nh?t c?u hình `fields` trong `dmdm`
5. Thêm phím t?t m?i b?ng cách override ph??ng th?c `OnKeyDown(int keyCode)` trong component
