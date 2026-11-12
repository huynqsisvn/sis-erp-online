# FXBCTGTH3 - Báo cáo T?ng h?p T?ng Gi?m CCDC (2)

## Thông tin c? b?n
- **Menu ID**: `wf.xb.23`
- **Page**: `/fxbctgth3`
- **Store Procedure**: `FXBCTGTH3`
- **Target Framework**: .NET 8.0
- **Platform**: Blazor

## Tham s? Store Procedure
```
@Nam (INT) - N?m báo cáo
@tuKy (INT) - Tháng b?t ??u (1-12)
@denKy (INT) - Tháng k?t thúc (1-12)
@ma_bpcc (NVARCHAR(50)) - Mã b? ph?n (t??ng ?ng ma_dvcs)
@nh_cc (NVARCHAR(50)) - Nhóm công c? (b?t bu?c)
@condition (NVARCHAR(MAX)) - ?i?u ki?n SQL l?c nâng cao
```

## Files T?o Ra

### Core Files
- **FXBCTGTH3.cs** - Model d? li?u
  - `FXBCTGTH3Model` - Container báo cáo
  - `FXBCTGTH3Row` - Dòng d? li?u (h? tr? 30 c?t L_L01-L_L30)

- **FXBCTGTH3Cpn.razor** - Component Blazor chính
  - Form l?c (popup)
  - L??i d? li?u (FrmBrowseComponent)
  - K?t xu?t Excel
  - In báo cáo

### Report Classes (DevExpress)
- **V/FXBCTGTH3.cs** / **V/FXBCTGTH3.Designer.cs** - Báo cáo Ti?ng Vi?t
- **E/FXBCTGTH3.cs** / **E/FXBCTGTH3.Designer.cs** - Báo cáo Ti?ng Anh

### Documentation
- **README.md** - File này
- **SETUP_GUIDE.md** - H??ng d?n thi?t l?p chi ti?t

## Ch?c N?ng Chính

### Form L?c
- **N?m** (b?t bu?c): 1900-2079
- **T? k?** (b?t bu?c): 1-12 v?i hi?n th? ngày ??u k?
- **??n k?** (b?t bu?c): 1-12 v?i hi?n th? ngày cu?i k?
- **Nhóm công c?** (b?t bu?c): Autocomplete t? dmnhomcc
- **M?u VND/ngo?i t?** (readonly): M?c ??nh VND
- **L?c nâng cao** (tu? ch?n):
  - Lo?i công c? (dmplcc)
  - Mã BPSD (dmbpts)
  - Tài kho?n chi phí (dmtk)
  - Nhóm cc 1, 2, 3 (dmnhcc1, dmnhcc2, dmnhcc3)

### L??i Hi?n Th?
- Pivot columns theo lo?i công c? t? SP
- Formatting t? ??ng: giá tr? s? (#,##0), dòng bold cho tiêu ??
- H? tr? chuy?n ngôn ng?: Vi?t (chi_tieu) / Anh (chi_tieu2)

### Tính N?ng Thêm
1. **K?t xu?t Excel** - Export 3 ??nh d?ng (xlsx, xls, csv)
2. **In báo cáo** - DevExpress viewer v?i tùy ch?n m?u
3. **T? ??ng theo DVCS** - D? li?u thay ??i khi ch?n DVCS khác

## Quy T?c Ki?m Tra D? Li?u (Validation)

| Tr??ng | Quy T?c | Thông Báo L?i |
|--------|---------|---------------|
| N?m | 1900 ? n?m ? 2079 | "N?m không h?p l? (ph?i > 0)." |
| T? k? | 1 ? t? k? ? 12 | "T? k? ph?i t? 1 ??n 12." |
| ??n k? | 1 ? ??n k? ? 12 | "??n k? ph?i t? 1 ??n 12." |
| T? k? ? ??n k? | t? k? ? ??n k? | "T? k? không ???c l?n h?n ??n k?." |
| Nhóm cc | Không tr?ng | "Ch?a vào nhóm công c?!" (ngoài form) |

## Phím T?t

| Phím | Ch?c N?ng | Ghi Chú |
|------|----------|--------|
| **Ctrl+F** | M? form l?c | Ho?c nh?n nút Search |
| **F7** | In báo cáo | Ho?c nh?n nút Print |
| **Enter** | Xác nh?n l?c | Khi focus ? nút OK |
| **Esc** | ?óng popup | ?óng form l?c ho?c báo cáo |

## Tích H?p C?n Thi?t

### 1. Database
- Store Procedure: **FXBCTGTH3** (?ã s?n có)
- B?ng d? li?u: v_ctdmcc, dmcc, dmplcc, dmbpts, dmtk, dmnhcc1/2/3

### 2. Localization (wwwroot/Lang/fx-vi.xml)
?ã thêm s?n các key sau:
- Tool group is required!
- Year is invalid (must be > 0).
- From period must be 1 to 12.
- From period cannot be greater than To period.
- (Các key khác ?ã t?n t?i, ???c s? d?ng l?i)

### 3. Menu Configuration
C?n thêm vào c?u hình menu:
```
Menu_id: wf.xb.23
Label: Báo cáo T?ng h?p T?ng Gi?m CCDC (2)
Page: fxbctgth3
Parent: [Báo cáo CCDC]
Icon: (tu? ch?n)
```

### 4. Authorization
Quy?n truy c?p: `admin` ho?c `fxbctgth3` (VIEW)
Quy?n Export: `fxbctgth3` (EXPORT)
Quy?n Print: `fxbctgth3` (PRINT)

## So Sánh v?i FXBCTGTH1

| Tính N?ng | FXBCTGTH1 | FXBCTGTH3 |
|-----------|-----------|----------|
| Store Proc | FXBCTGTH1 | FXBCTGTH3 |
| Menu ID | wf.xb.22 | wf.xb.23 |
| Form L?c | Gi?ng | Gi?ng |
| L??i | Gi?ng | Gi?ng |
| Export | Có | Có |
| In | Có (F7) | Có (F7) |
| M?u In | Có | Không (ch? xem/in nhanh) |

## Localization & Ngôn Ng?

### Ti?ng Vi?t
- Menu: "Báo cáo T?ng h?p T?ng Gi?m CCDC (2)"
- Form: T?t c? tr??ng dùng label Vi?t
- Báo cáo: Dùng `[chi_tieu]` (V/FXBCTGTH3.cs)
- Ngày: `[v_tuKydenKy]`

### Ti?ng Anh
- Menu: "Summary Report of Tool Increase/Decrease (2)"
- Form: T?t c? tr??ng dùng label Anh
- Báo cáo: Dùng `[chi_tieu2]` (E/FXBCTGTH3.cs)
- Ngày: `[e_tuKydenKy]`

## H? Tr? & K? Thu?t

### Ki?m Tra D? Li?u
```csharp
// Ki?m tra SP
SELECT * FROM FXBCTGTH3 2024, 1, 12, '', 'nhom001', '1=1'

// Ki?m tra authorization
SELECT * FROM [dbo].[PermissionDetail] WHERE MenuId = 'wf.xb.23'

// Ki?m tra localization
SELECT * FROM [dbo].[ResourceDesc] WHERE ResourceKey LIKE '%Tool group is required%'
```

### Debugging
- B?t debug trong browser: F12 ? Console
- Ki?m tra Network: Network tab
- Log t? component: Console.WriteLine

## Các File Liên Quan

- **FXBCTGTH1** (Reference): `SisFx\SisFx\Pages\Report\FXBCTGTH1\`
- **Localization**: `SISERPSME\wwwroot\Lang\fx-vi.xml`
- **Base Classes**: `SisFx\Pages\Report\` (RepoBaseComponent, FrmBrowseComponent)
- **Store Procedures**: SQL Server database `SISERPOL2025`

## Thay ??i G?n ?ây

- [2026-03-25] T?o ch?c n?ng FXBCTGTH3
  - Files: FXBCTGTH3.cs, FXBCTGTH3Cpn.razor, V/E designers
  - Localization: C?p nh?t fx-vi.xml
  - Documentation: README.md, SETUP_GUIDE.md

---

**L?u ý**: ?? s? d?ng ch?c n?ng này, ??m b?o r?ng:
1. SP FXBCTGTH3 ?ã ???c t?o trong SQL Server
2. Menu wf.xb.23 ?ã ???c thêm vào c?u hình
3. Quy?n truy c?p ?ã ???c c?p cho ng??i dùng
4. Localization ?ã ???c t?i ?úng cách
