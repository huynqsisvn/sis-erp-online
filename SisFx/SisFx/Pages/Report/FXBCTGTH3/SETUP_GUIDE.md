# FXBCTGTH3 - Báo cáo T?ng h?p T?ng Gi?m CCDC (2)

## Tóm T?t
Ch?c n?ng báo cáo t?ng h?p t?ng gi?m công c? d?ng c? (CCDC) l?n th? 2 - phiên b?n nâng cao v?i form l?c t??ng t? FXBCTGTH1 nh?ng ch? khác m?t s? ?i?u ki?n nh?.

## Thông Tin C? B?n
- **Menu ID**: `wf.xb.23`
- **Page URL**: `/fxbctgth3`
- **Project**: SisFx
- **Th? m?c**: `SisFx/SisFx/Pages/Report/FXBCTGTH3/`
- **Store Procedure**: `FXBCTGTH3`
- **Ngôn ng?**: .NET 8.0 / Blazor

## C?u Trúc File

```
FXBCTGTH3/
??? FXBCTGTH3.cs                    # Model d? li?u
??? FXBCTGTH3Cpn.razor              # Component chính Blazor
??? README.md                        # H??ng d?n
??? V/                              # Báo cáo ti?ng Vi?t
?   ??? FXBCTGTH3.cs
?   ??? FXBCTGTH3.Designer.cs
??? E/                              # Báo cáo ti?ng Anh
    ??? FXBCTGTH3.cs
    ??? FXBCTGTH3.Designer.cs
```

## Các File T?o Ra

### 1. **FXBCTGTH3.cs** (Model)
- `FXBCTGTH3Model`: Container ch?a d? li?u báo cáo (Nam, TuKy, DenKy, v_title_report, e_title_report, etc.)
- `FXBCTGTH3Row`: ??i di?n cho m?i dòng d? li?u trong báo cáo (chi_tieu, T_L_, L_L01-L_L30, etc.)

### 2. **FXBCTGTH3Cpn.razor** (Component)
Component Blazor chính v?i các tính n?ng:
- **Form l?c** (popup):
  - N?m (spinner)
  - T? k? (1-12)
  - ??n k? (1-12)
  - Nhóm công c? (b?t bu?c)
  - M?u VND/ngo?i t? (readonly, m?c ??nh VND)
  - L?c nâng cao: lo?i cc, mã BPSD, tk cp, nhóm cc 1/2/3
- **L??i hi?n th?** (FrmBrowseComponent)
- **K?t xu?t Excel** (Export)
- **In báo cáo** (Print)
- **Phím t?t**: Ctrl+F (m? l?c), F7 (in)

### 3. **V/FXBCTGTH3.cs & V/FXBCTGTH3.Designer.cs**
- Báo cáo DevExpress XtraReport ti?ng Vi?t
- Chi ti?t t? [chi_tieu], ngày t? [v_tuKydenKy]

### 4. **E/FXBCTGTH3.cs & E/FXBCTGTH3.Designer.cs**
- Báo cáo DevExpress XtraReport ti?ng Anh
- Chi ti?t t? [chi_tieu2], ngày t? [e_tuKydenKy]

## C?u Hình C?n Thi?t

### 1. Database - Store Procedure
S? d?ng SP `FXBCTGTH3` có tham s?:
```sql
EXEC FXBCTGTH3
    @Nam INT,
    @tuKy INT,
    @denKy INT,
    @ma_bpcc NVARCHAR(50),
    @nh_cc NVARCHAR(50),
    @condition NVARCHAR(MAX)
```

**Tr? v?**:
- DataTable 0: C?u hình báo cáo (vbrowse, ebrowse, v_title_report, e_title_report, value_detail_report)
- DataTable 1: D? li?u chi ti?t báo cáo

### 2. Localization (fx-vi.xml)
Các Lap s? d?ng (?ã c?p nh?t):
- `Tool group is required!` - Nhóm công c? b?t bu?c
- `Year is invalid (must be > 0).` - N?m không h?p l?
- `From period must be 1 to 12.` - T? k? ph?i t? 1 ??n 12
- `To period must be 1 to 12.` - ??n k? ph?i t? 1 ??n 12
- `From period cannot be greater than To period.` - T? k? không ???c l?n h?n ??n k?
- `Error loading data:` - L?i t?i d? li?u
- `No data to print` - Không có d? li?u ?? in
- `Export successfully` - Xu?t thành công
- Các Lap khác (?ã có s?n trong fx-vi.xml)

### 3. Authorization
Yêu c?u quy?n `admin` ho?c `fxbctgth3` ?? truy c?p trang.

## H??ng D?n S? D?ng

### B??c 1: M? Ch?c N?ng
- Truy c?p menu: **Báo cáo ? CCDC ? Báo cáo T?ng h?p T?ng Gi?m (2)**
- Ho?c truy c?p tr?c ti?p: `/fxbctgth3`

### B??c 2: Nh?p ?i?u Ki?n L?c
1. **N?m**: Nh?p n?m c?n báo cáo (m?c ??nh là n?m hi?n t?i)
2. **T? k?**: Nh?p tháng b?t ??u (1-12)
3. **??n k?**: Nh?p tháng k?t thúc (1-12)
4. **Nhóm công c?**: **B?T BU?C** - Ch?n nhóm cc t? danh m?c
5. **M?u VND/ngo?i t?**: M?c ??nh là VND (readonly)
6. **(Tùy ch?n) L?c Nâng Cao**:
   - Lo?i công c?
   - Mã BPSD
   - Tài kho?n chi phí
   - Nhóm cc 1, 2, 3

### B??c 3: Hi?n Th? D? Li?u
- Nh?n nút **OK** ?? l?c và hi?n th? d? li?u
- L??i t? ??ng hi?n th? các dòng v?i c?t ???c xáo quanh theo lo?i cc

### B??c 4: Thao Tác D? Li?u
- **In báo cáo**: Nh?n F7 ho?c nút **Print** ? Ch?n m?u in ? Xem tr??c/In
- **K?t xu?t Excel**: Nh?n nút **Export** ? Ch?n ??nh d?ng ? T?i file

### B??c 5: S?a ??i B? L?c
- Nh?n **Ctrl+F** ho?c nút **Search** ?? m? l?i form l?c

## ??i Ngôn Ng?

Ch?c n?ng h? tr? chuy?n ??i t? ??ng gi?a Ti?ng Vi?t và Ti?ng Anh:
- Ti?ng Vi?t: Form l?c, tiêu ?? c?t, v?n b?n t? `[chi_tieu]`
- Ti?ng Anh: Form l?c, tiêu ?? c?t, v?n b?n t? `[chi_tieu2]`
- Ngôn ng? báo cáo in: V/FXBCTGTH3.cs (Vi?t), E/FXBCTGTH3.cs (Anh)

## Quy T?c Validation

| Tr??ng | Quy T?c |
|--------|---------|
| N?m | T? 1900 ??n 2079 |
| T? k? | 1 ??n 12 |
| ??n k? | 1 ??n 12 |
| T? k? vs ??n k? | T? k? ? ??n k? |
| Nhóm cc | **B?t bu?c** - Không ???c tr?ng |
| DVCS | L?c theo DVCS hi?n t?i |

## Phím T?t

| Phím | Ch?c N?ng |
|------|----------|
| **F7** | In báo cáo |
| **Ctrl+F** | M? form l?c |
| **Enter** | Xác nh?n l?c (khi con tr? ? nút OK) |
| **Esc** | ?óng form l?c |

## Thông Báo L?i

| L?i | Nguyên Nhân | Gi?i Pháp |
|-----|-----------|----------|
| "N?m không h?p l?" | N?m < 1900 ho?c > 2079 | Nh?p n?m h?p l? |
| "T? k? ph?i t? 1 ??n 12" | T? k? ngoài kho?ng 1-12 | Nh?p t? k? t? 1 ??n 12 |
| "Nhóm công c? b?t bu?c!" | Nhóm cc b? tr?ng | Ch?n nhóm cc t? danh m?c |
| "T? k? không ???c l?n h?n ??n k?" | T? k? > ??n k? | Ch?nh l?i t? k? ? ??n k? |
| "Không có d? li?u ?? in" | SP không tr? v? d? li?u | Ki?m tra ?i?u ki?n l?c ho?c SP |

## Tích H?p

### Tích H?p Vào Navigation/Menu
C?n thêm menu item vào file c?u hình menu v?i:
- **Menu_id**: `wf.xb.23`
- **Label**: `Báo cáo T?ng h?p T?ng Gi?m CCDC (2)` / `Summary Report of Tool Increase/Decrease (2)`
- **Page**: `fxbctgth3`
- **Icon**: (tu? ch?n)
- **Parent**: Báo cáo CCDC

### C?p Quy?n
C?u hình quy?n cho menu `wf.xb.23` trong b?ng phân quy?n:
- **VIEW**: Xem báo cáo
- **EXPORT**: K?t xu?t Excel
- **PRINT**: In báo cáo

## Ghi Chú

1. **T??ng T? FXBCTGTH1**: Ch?c n?ng này t??ng t? FXBCTGTH1 nh?ng có các ?i?u ki?n l?c t?ng chút khác bi?t
2. **Không có ch?c n?ng In**: Báo cáo này ch? h? tr? **Xem tr??c** và **In** t? DevExpress, KHÔNG có ch?c n?ng t?o m?u in riêng
3. **D? li?u Pivot**: C?t d? li?u t? ??ng xoay quanh theo lo?i cc t? SP
4. **Format S?**: T?t c? giá tr? s? hi?n th? d?ng: `#,##0` (phân tách hàng nghìn)
5. **DVCS**: D? li?u t? ??ng thay ??i theo DVCS ???c ch?n (OnDvcsChanged)

## H? Tr? Thêm

- Xem file FXBCTGTH1 ?? so sánh c?u trúc
- Xem SP `FXBCTGTH3` ?? hi?u cách l?y d? li?u
- Xem file localization `fx-vi.xml` ?? thêm Lap m?i
