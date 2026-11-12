# Danh m?c Kh? ??c (Contract Category)

## Thông tin chung
- **Menu ID**: `wa.dm.02`
- **Page**: `dmku`
- **Th? m?c**: `SisCa\SisCa\Pages\List\Caku`
- **K? th?a**: Các Component k? th?a t? `FrmListBaseComponent` và `ListBaseComponent`

## C?u trúc Files

### 1. **Dmku.cs** - Model Class
- ???ng d?n: `SisCa\SisCa\Pages\List\Caku\Dmku.cs`
- Ch?c n?ng: ??nh ngh?a c?u trúc d? li?u cho danh m?c kh? ??c
- Các tr??ng chính:
  - **Mã kh? ??c**: `ma_ku` (Primary Key)
  - **Thông tin c? b?n**: `so_ku`, `ten_ku`, `ten_ku2`, `ma_tra_cuu`
  - **Thông tin kh? ??c cha**: `ma_ku_me`
  - **??i tác cho vay**: `ma_kh`, `ten_kh`
  - **Ngày tháng**: `ngay_ku`, `ngay_ku1`, `ngay_ku2`
  - **S? ngày vay**: `sn_vay`
  - **Lo?i kh? ??c**: `loai_ku`
  - **Bao g?m c? ngày tr?**: `tinh_tra`
  - **Lãi su?t**: `lai_suat1` (lãi su?t vay tháng), `lai_suat2` (lãi su?t quá h?n tháng)
  - **Tài kho?n**: `tk` (Tài kho?n ng??i bán)
  - **Ti?n t?**: `ma_nt`, `tien0`, `tien_nt0`
  - **Phân lo?i**: `nh_ku1`, `nh_ku2`, `nh_ku3`
  - **Mã t? do**: `ma_td1`, `ma_td2`, `ma_td3`
  - **Ngày t? do**: `ngay_td1`, `ngay_td2`, `ngay_td3`
  - **S? l??ng t? do**: `sl_td1`, `sl_td2`, `sl_td3`
  - **Ghi chú t? do**: `gc_td1`, `gc_td2`, `gc_td3`
  - **Ghi chú**: `ghi_chu`
  - **Tr?ng thái**: `status`, `status1`
  - **Audit fields**: `date`, `time`, `user_id`, `date0`, `time0`, `user_id0`, `user_name`, `user_name0`

### 2. **DmkuComponent.razor** - Component danh sách
- ???ng d?n: `SisCa\SisCa\Pages\List\Caku\DmkuComponent.razor`
- Route: `/dmku` ho?c `/dmku/{Menu_id}`
- Ch?c n?ng:
  - Hi?n th? danh sách kh? ??c trong grid
  - Tìm ki?m và l?c d? li?u
  - Thêm m?i, s?a, xóa, copy kh? ??c
  - Thay ??i mã kh? ??c
  - Import t? Excel
  - H? tr? ?a ngôn ng?

### 3. **EditDmku.razor** - Form nh?p li?u
- ???ng d?n: `SisCa\SisCa\Pages\List\Caku\EditDmku.razor`
- Ch?c n?ng:
  - Form nh?p/s?a thông tin kh? ??c
  - Các ch? ??: View, Edit, New, Copy
  - Validation d? li?u
  - T? ??ng load tên khách hàng khi ch?n mã khách hàng
  - Auto-focus vào tr??ng mã kh? ??c

## C?u trúc Database

### B?ng dmku
```sql
CREATE TABLE [dbo].[dmku](
    [ma_ku] [udt_ma_ku] NOT NULL,              -- Mã kh? ??c (PK)
    [so_ku] [udt_so_ct] NULL,                  -- S? kh? ??c
    [ten_ku] [udt_ten_dien_giai] NULL,         -- Tên kh? ??c
    [ten_ku2] [udt_ten_dien_giai] NULL,        -- Tên kh? ??c 2
    [ma_ku_me] [udt_ma_ku] NULL,               -- Mã kh? ??c m?
    [ma_kh] [udt_ma_kh] NULL,                  -- ??i t??ng cho vay
    [ngay_ku] [udt_ngay_ct] NULL,              -- Ngày kh? ??c
    [ngay_ku1] [udt_ngay_ct] NULL,             -- Ngày vay
    [ngay_ku2] [udt_ngay_ct] NULL,             -- Ngày ?áo h?n
    [sn_vay] [udt_so_ngay] NULL,               -- S? ngày vay
    [loai_ku] [char](1) NULL,                  -- Lo?i kh? ??c (default: 1)
    [tinh_tra] [int] NULL,                     -- Bao g?m c? ngày tr? (default: 0)
    [lai_suat1] [udt_lai_suat] NULL,           -- Lãi su?t vay tháng
    [lai_suat2] [udt_lai_suat] NULL,           -- Lãi su?t quá h?n tháng
    [tk] [udt_tk] NULL,                        -- Tài kho?n ti?n vay
    [ma_nt] [udt_ma_nt] NULL,                  -- Mã ngo?i t?
    [tien0] [udt_tien] NULL,                   -- Giá tr? vay
    [tien_nt0] [udt_tien_nt] NULL,             -- Giá tr? vay ngo?i t?
    [ma_vv] [udt_ma_vv] NULL,                  -- Mã v? vi?c
    [ma_hd] [udt_ma_hop_dong] NULL,            -- Mã h?p ??ng
    [ma_hdm] [udt_ma_hop_dong] NULL,           -- Mã h?p ??ng mua
    [nh_ku1] [udt_nhom_ku] NULL,               -- Nhóm kh? ??c 1
    [nh_ku2] [udt_nhom_ku] NULL,               -- Nhóm kh? ??c 2
    [nh_ku3] [udt_nhom_ku] NULL,               -- Nhóm kh? ??c 3
    [ma_td1] [udt_ma_td] NULL,                 -- Mã t? do 1
    [ma_td2] [udt_ma_td] NULL,                 -- Mã t? do 2
    [ma_td3] [udt_ma_td] NULL,                 -- Mã t? do 3
    [ngay_td1] [udt_ngay_ct] NULL,             -- Ngày t? do 1
    [ngay_td2] [udt_ngay_ct] NULL,             -- Ngày t? do 2
    [ngay_td3] [udt_ngay_ct] NULL,             -- Ngày t? do 3
    [sl_td1] [udt_so_luong] NULL,              -- S? l??ng t? do 1
    [sl_td2] [udt_so_luong] NULL,              -- S? l??ng t? do 2
    [sl_td3] [udt_so_luong] NULL,              -- S? l??ng t? do 3
    [gc_td1] [udt_ghi_chu_td] NULL,            -- Ghi chú t? do 1
    [gc_td2] [udt_ghi_chu_td] NULL,            -- Ghi chú t? do 2
    [gc_td3] [udt_ghi_chu_td] NULL,            -- Ghi chú t? do 3
    [ma_tra_cuu] [udt_ma_tra_cuu] NULL,        -- Mã ph?
    [ghi_chu] [udt_ghi_chu] NULL,              -- Ghi chú
    [status] [char](1) NULL,                   -- Tr?ng thái
    [status1] [char](1) NULL,                  -- Tr?ng thái 1
    [date] [udt_ngay_ct] NULL,                 -- Ngày c?p nh?t
    [time] [udt_time] NULL,                    -- Gi? c?p nh?t
    [user_id] [udt_user_id] NULL,              -- User c?p nh?t
    [user_name] [udt_user_name] NULL,          -- Tên user c?p nh?t
    [date0] [udt_ngay_ct] NULL,                -- Ngày t?o
    [time0] [udt_time] NULL,                   -- Gi? t?o
    [user_id0] [udt_user_id] NULL,             -- User t?o
    [user_name0] [udt_user_name] NULL,         -- Tên user t?o
    [search] [udt_search] NULL,                -- T? khóa tìm ki?m
    CONSTRAINT [PK_dmku] PRIMARY KEY CLUSTERED ([ma_ku] ASC)
)
```

### View v_dmku
- File SQL: `SisCa\SisCa\Pages\List\Caku\v_dmku.sql`
- M? r?ng thông tin t? các b?ng liên quan:
  - `dmkh`: Tên khách hàng
  - `dmnt`: Tên ngo?i t?
  - `dmtk`: Tên tài kho?n

## Các tr??ng m?c ??nh khi t?o m?i

- `Status` = "1" (S? d?ng)
- `Loai_ku` = "1"
- `Tinh_tra` = 1 (Có)
- `Ngay_ku` = Ngày hi?n t?i
- `Ngay_ku1` = Ngày hi?n t?i
- `Sn_vay` = 90 (ngày)
- `Lai_suat1` = 4.00%
- `Lai_suat2` = 0.00%
- `Ma_nt` = "USD"
- `Tk` = "331"

## Validation

1. **Mã kh? ??c** (`ma_ku`): B?t bu?c nh?p
2. **Tên kh? ??c** (`ten_ku`): B?t bu?c nh?p
3. Ki?m tra trùng mã khi thêm m?i

## Tích h?p

- **Authorization**: Yêu c?u role `admin` ho?c `dmku`
- **Localization**: H? tr? ?a ngôn ng? qua `@Lap` (ti?ng Vi?t) và `IsLangEn`
- **AutocompleteComponent**: S? d?ng cho các tr??ng:
  - `ma_ku_me`: Danh m?c kh? ??c (dmku)
  - `ma_kh`: Danh m?c khách hàng (dmkh)
  - `tk`: Danh m?c tài kho?n (dmtk)
  - `ma_nt`: Danh m?c ngo?i t? (dmnt)

## Ghi chú k? thu?t

- K? th?a t? `FrmListBaseComponent` cho form edit
- K? th?a t? `ListBaseComponent` cho component danh sách
- S? d?ng DevExpress Blazor Components (DxTextBox, DxDateEdit, DxSpinEdit, DxComboBox, DxMemo)
- H? tr? tìm ki?m full-text qua tr??ng `search`
- Audit trail t? ??ng c?p nh?t khi thêm m?i/s?a
