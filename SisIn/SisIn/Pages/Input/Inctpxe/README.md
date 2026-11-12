# Phi?u xu?t ?i?u chuy?n (INCTPXE)

## Mô t?
Module qu?n lý phi?u xu?t ?i?u chuy?n v?t t? gi?a các kho trong h? th?ng kho v?n.

## Thông tin module
- **Menu ID**: `wi.ct.03`
- **Mã ch?ng t?**: `PXE`
- **B?ng phi?u**: `ph85`
- **B?ng chi ti?t**: `ct85`
- **Authorization**: Yêu c?u role `admin` ho?c `inctpxe`

## Các file ?ã t?o

### 1. **Ph85.cs** - Model Header
- ???ng d?n: `SisIn\SisIn\Pages\Input\Inctpxe\Ph85.cs`
- Ch?c n?ng: ??nh ngh?a c?u trúc d? li?u cho ph?n header phi?u xu?t ?i?u chuy?n
- Các tr??ng chính:
  - Thông tin ch?ng t?: `stt_rec`, `ma_ct`, `so_ct`, `ngay_ct`
  - Kho xu?t/nh?p: `ma_kho` (kho xu?t), `ma_khon` (kho nh?p)
  - Thông tin l?nh ?i?u ??ng: `so_lenhdd`, `ngay_lenhdd`, `cua_lenhdd`, `viec_lenhdd`, etc.
  - T?ng s?: `t_so_luong`, `t_tien`, `t_tien_nt`
  - Ngo?i t?: `ma_nt`, `ty_gia`, `ty_giaf`
  - Các tr??ng mã t? do: `ma_td1`, `ma_td2`, `ma_td3`
  - Audit trail: `date`, `time`, `user_id`, `date0`, `time0`, `user_id0`

### 2. **Ct85.cs** - Model Detail
- ???ng d?n: `SisIn\SisIn\Pages\Input\Inctpxe\Ct85.cs`
- Ch?c n?ng: ??nh ngh?a c?u trúc d? li?u chi ti?t v?t t?
- Các tr??ng chính:
  - V?t t?: `ma_vt`, `ten_vt`, `dvt`, `so_luong`
  - Tài kho?n: `tk_vt` (n?), `ma_nx_i` (có)
  - Giá tr?: `gia`, `tien`, `gia_nt`, `tien_nt`
  - ??n v? tính ph?: `dvt1`, `he_so1`, `so_luong1`
  - Lô hàng: `ma_lo`, `lot_nbr`, `han_sd_lot`
  - Mã t? do chi ti?t: `ma_td_i`, `ma_td2_i`, `ma_td3_i`, `ma_td4_i`
  - Phân lo?i: `ma_vv_i`, `ma_bpht_i`, `ma_phi_i`, etc.

### 3. **InctpxeComponent.razor** - Component danh sách
- ???ng d?n: `SisIn\SisIn\Pages\Input\Inctpxe\InctpxeComponent.razor`
- Route: `/Inctpxe` ho?c `/Inctpxe/{Menu_id}`
- Ch?c n?ng:
  - Hi?n th? danh sách phi?u xu?t ?i?u chuy?n (grid master)
  - Hi?n th? chi ti?t v?t t? (grid detail)
  - B? l?c nâng cao (Filter)
  - In phi?u xu?t
  - Import t? Excel
  - H? tr? ?a ngôn ng?

## C?u trúc Database

### B?ng ph85 (Header)
```sql
CREATE TABLE [dbo].[ph85](
    [stt_rec] [udt_stt_rec] NULL,         -- Mã phi?u (PK)
    [ma_ct] [udt_ma_ct] NULL,             -- Mã ch?ng t? = 'PXE'
    [ngay_ct] [udt_ngay_ct] NULL,         -- Ngày ch?ng t?
    [so_ct] [udt_so_ct] NULL,             -- S? ch?ng t?
    [ma_kho] [udt_ma_kho] NULL,           -- Kho xu?t
    [ma_khon] [udt_ma_kho] NULL,          -- Kho nh?p
    [so_lenhdd] [udt_ten_dien_giai] NULL, -- S? l?nh ?i?u ??ng
    [ngay_lenhdd] [udt_ngay_ct] NULL,     -- Ngày l?nh ?i?u ??ng
    [t_so_luong] [udt_so_luong] NULL,     -- T?ng s? l??ng
    [ma_nt] [udt_ma_nt] NULL,             -- Mã ngo?i t?
    [ty_gia] [udt_ty_gia] NULL,           -- T? giá
    [t_tien_nt] [udt_tien_nt] NULL,       -- T?ng ti?n NT
    [t_tien] [udt_tien] NULL,             -- T?ng ti?n
    [status] [char](1) NULL,              -- Tr?ng thái
    [px_gia_dd] [numeric](1, 0) NULL,    -- Xu?t giá ?ích danh
    [sua_tien] [numeric](1, 0) NULL,     -- S?a ti?n
    [sl_in] [int] NULL DEFAULT(0),       -- S? l?n in
    -- Các tr??ng khác...
)
```

### B?ng ct85 (Detail)
```sql
CREATE TABLE [dbo].[ct85](
    [stt_rec] [udt_stt_rec] NULL,         -- Link t?i ph85
    [stt_rec0] [udt_stt_rec0] NULL,       -- STT dòng
    [ma_vt] [udt_ma_vt] NULL,             -- Mã v?t t?
    [tk_vt] [udt_tk] NULL,                -- TK n? (v?t t?)
    [ma_nx_i] [udt_tk] NULL,              -- TK có (nh?p kho)
    [so_luong] [udt_so_luong] NULL,       -- S? l??ng
    [dvt1] [udt_dvt] NULL,                -- ??n v? tính ph?
    [he_so1] [udt_he_so_vt] NULL,         -- H? s? quy ??i
    [so_luong1] [udt_so_luong] NULL,      -- S? l??ng quy ??i
    [gia_nt] [udt_gia_nt] NULL,           -- Giá NT
    [gia] [udt_gia] NULL,                 -- Giá
    [tien_nt] [udt_tien_nt] NULL,         -- Ti?n NT
    [tien] [udt_tien] NULL,               -- Ti?n
    [ma_lo] [udt_ma_lo] NULL,             -- Mã lô
    -- Các tr??ng khác...
)
```

## Tính n?ng chính

### 1. T?o m?i phi?u xu?t ?i?u chuy?n
- Nh?p thông tin kho xu?t, kho nh?p
- Nh?p thông tin l?nh ?i?u ??ng (n?u có)
- Nh?p chi ti?t v?t t? xu?t
- H? th?ng t? ??ng tính t?ng s? l??ng, t?ng ti?n

### 2. S?a phi?u
- Ch? s?a ???c phi?u ch?a ch?t s? (status)
- C?p nh?t thông tin header và detail
- T? ??ng c?p nh?t l?i t?ng

### 3. Xóa phi?u
- Ki?m tra ?i?u ki?n xóa (status, ràng bu?c d? li?u)
- Xóa c? header và detail

### 4. In phi?u
- In phi?u xu?t theo m?u chu?n
- H? tr? in nhi?u liên
- Hi?n th? ??y ?? thông tin v?t t?, tài kho?n

### 5. B? l?c nâng cao
Cho phép l?c theo:
- Kho?ng ngày ch?ng t?
- Kho?ng s? ch?ng t?
- Quy?n ch?ng t? (ma_qs)
- Mã giao d?ch (ma_gd)
- Kho xu?t (ma_kho)
- Kho nh?p (ma_khon)
- Mã v?t t?
- Tr?ng thái (status)
- ??n v? c? s? (ma_dvcs)
- Ng??i t?o (user filter)

### 6. Import t? Excel
- Import hàng lo?t phi?u xu?t ?i?u chuy?n
- Validation d? li?u import
- Hi?n th? l?i chi ti?t

## Quy trình nghi?p v?

### A. T?o phi?u xu?t ?i?u chuy?n
1. Nh?p thông tin header:
   - Ch?n kho xu?t (`ma_kho`)
   - Ch?n kho nh?p (`ma_khon`)
   - Nh?p ngày ch?ng t? (`ngay_ct`)
   - Ch?n quy?n s? (`ma_qs`)
   - Nh?p thông tin l?nh ?i?u ??ng (tùy ch?n)

2. Nh?p chi ti?t v?t t?:
   - Ch?n mã v?t t? (`ma_vt`)
   - Nh?p s? l??ng (`so_luong`)
   - H? th?ng t? ??ng l?y giá xu?t kho
   - Ch?n tài kho?n n?/có (n?u c?n)
   - Nh?p thông tin lô hàng (n?u qu?n lý lô)

3. L?u phi?u:
   - H? th?ng ki?m tra validation
   - Ki?m tra t?n kho
   - C?p nh?t t?ng s? l??ng, t?ng ti?n
   - T?o ??nh kho?n k? toán t? ??ng

### B. X? lý giá xu?t
Module h? tr? nhi?u ph??ng pháp tính giá:
- **Giá bình quân**: T? ??ng tính t? t?n kho
- **Giá ?ích danh** (`px_gia_dd = 1`): S? d?ng giá t? phi?u nh?p c? th?
- **Nh?p tay** (`sua_tien = 1`): Cho phép nh?p giá th? công

### C. ??nh kho?n t? ??ng
Khi l?u phi?u, h? th?ng t? ??ng t?o ??nh kho?n:
```
N?: tk_vt (Tài kho?n v?t t? kho nh?p)
Có: ma_nx_i (Tài kho?n v?t t? kho xu?t)
```

## Validation

Module th?c hi?n các ki?m tra sau:

### 1. Ki?m tra header
- ? Kho xu?t không ???c r?ng
- ? Kho nh?p không ???c r?ng
- ? Kho xu?t khác kho nh?p
- ? Ngày ch?ng t? h?p l? (>= ngày m? s?, <= ngày hi?n t?i)
- ? Quy?n s? h?p l?
- ? S? ch?ng t? không trùng

### 2. Ki?m tra detail
- ? Mã v?t t? h?p l?
- ? S? l??ng > 0
- ? Giá xu?t >= 0
- ? Tài kho?n n?/có h?p l?
- ? Ki?m tra t?n kho (n?u b?t M_CHK_TON_VT)
- ? Mã t? do h?p l? (n?u có)

## C?u hình h? th?ng

### 1. Tham s? c?u hình (dmct)
```sql
INSERT INTO dmct (ma_ct, m_phdbf, m_ctdbf, ma_post, ma_gd)
VALUES ('PXE', 'ph85', 'ct85', '0', 'PXE')
```

### 2. Tham s? ki?m tra t?n kho
- `M_CHK_TON_VT = 1`: C?nh báo xu?t âm
- `M_CHK_TON_VT = 2`: Không cho phép xu?t âm

### 3. Qu?n lý lô hàng
- `M_QL_LO_CK = 1`: B?t qu?n lý lô

### 4. Làm tròn
- `M_ROUND`: Làm tròn ti?n VND
- `M_ROUND_NT`: Làm tròn ti?n ngo?i t?
- `M_ROUND_GIA`: Làm tròn giá

## Phân quy?n

Thêm role và gán quy?n:
```sql
-- Thêm role
INSERT INTO dmrole (role_id, role_name) 
VALUES ('inctpxe', 'Phi?u xu?t ?i?u chuy?n')

-- Gán quy?n cho user
INSERT INTO user_roles (user_id, role_id) 
VALUES ('user_id', 'inctpxe')
```

## ?a ngôn ng?

Module h? tr? 2 ngôn ng?:
- **Ti?ng Vi?t** (m?c ??nh)
- **English**

Các label ???c ??nh ngh?a trong file resource và ???c load thông qua `IStringLocalizer`.

## Tích h?p v?i module khác

### 1. K? toán t?ng h?p (GL)
- T? ??ng t?o bút toán k? toán
- ??ng b? tr?ng thái ch?t s?

### 2. Qu?n lý t?n kho (IN)
- C?p nh?t t?n kho khi l?u/xóa phi?u
- Ki?m tra t?n kho khi xu?t

### 3. Qu?n lý lô hàng
- Track s? lô, h?n s? d?ng
- Báo cáo theo lô

## L?u ý k? thu?t

### 1. K? th?a t? TransBaseComponent
- S? d?ng các method base: `Loaddata()`, `V_Moi()`, `V_Sua()`, `V_Xoa()`, etc.
- Override method `V_In()` ?? custom báo cáo
- Override method `ShowError()` ?? custom validation

### 2. Blazor Component Lifecycle
- `OnInitialized()`: Kh?i t?o tham s?, filter
- `OnAfterRenderAsync()`: ??ng ký event handler
- `Dispose()`: Cleanup resources

### 3. Performance
- Grid s? d?ng virtualization cho d? li?u l?n
- Lazy loading cho dropdown
- Debounce cho autocomplete

## Troubleshooting

### L?i: "Kho xu?t/nh?p không h?p l?"
- Ki?m tra mã kho trong `dmkho`
- Ki?m tra quy?n truy c?p kho c?a user

### L?i: "Xu?t âm kho"
- Ki?m tra t?n kho hi?n t?i
- Ki?m tra tham s? `M_CHK_TON_VT`

### L?i: "Tài kho?n không h?p l?"
- Ki?m tra ??nh kho?n trong `dmtk`
- Ki?m tra tài kho?n không ph?i t?ng h?p

### L?i: "Ngày ch?ng t? không h?p l?"
- Ki?m tra ngày m? s? (`M_NGAY_BAT_DAU`)
- Ki?m tra k? k? toán hi?n t?i

## TODO - Các tính n?ng c?n hoàn thi?n

1. ? Model classes (Ph85.cs, Ct85.cs)
2. ? Component danh sách (InctpxeComponent.razor)
3. ? Form nh?p li?u (FrmInctpxe.razor) - **C?N T?O**
4. ? Stored procedure Post (PXE-Post) - **C?N T?O**
5. ? Report template (PXE_Report) - **C?N T?O**
6. ? Validation rules - **C?N B? SUNG**
7. ? Unit tests - **C?N T?O**

## Tham kh?o

- Code WPF g?c: `INCTPXE\FrmINCTPXE.xaml.cs`
- Module t??ng t?: `Inctpxd` (Phi?u xu?t bán hàng)
- Base class: `TransBaseComponent`

## ?? Tr?ng thái hi?n t?i

### ? ?ã hoàn thành:
1. **Model Classes** (Ph85.cs, Ct85.cs) - Hoàn ch?nh 100%
2. **Component danh sách** (InctpxeComponent.razor) - Hoàn ch?nh 90%
   - Master-detail grid
   - Filter popup
   - Import Excel
   - Print function
   - Validation

### ? ?ang xây d?ng:
3. **Form nh?p li?u** (FrmInctpxe.razor) - Hoàn thành 60%
   - UI layout hoàn ch?nh
   - Grid chi ti?t ?ã setup
   - C?n b? sung logic x? lý:
     * Tính t?ng ti?n, s? l??ng
     * T? ??ng l?y giá t? phi?u nh?p
     * Ki?m tra t?n kho
     * Validation ??y ??
     * Save logic

## ?? H??ng d?n hoàn thi?n FrmInctpxe.razor

### B??c 1: B? sung các method x? lý giá tr?
Tham kh?o t? `FrmInctpxd.razor` ?? implement các method sau:

```csharp
// 1. Tính toán t?ng ti?n khi thay ??i s? l??ng/giá
void OnGridCT_Value_Changed(decimal newvalue, string name)
{
    switch (name.ToLower())
    {
        case "so_luong":
            CurrentEditModel.So_luong = newvalue;
            // Tính l?i giá và ti?n
            // Xem logic chi ti?t trong FrmInctpxd.razor dòng 1100-1150
            break;
        
        case "gia_nt":
            CurrentEditModel.Gia_nt = newvalue;
            // Tính l?i ti?n NT và ti?n VND
            break;
        
        case "tien_nt":
            CurrentEditModel.Tien_nt = newvalue;
            // Tính ng??c l?i giá n?u c?n
            break;
    }
    PreSum_All(); // C?p nh?t t?ng
}

// 2. Tính t?ng s? l??ng và ti?n
void PreSum_All()
{
    if (CurrentEditModel != null)
    {
        CurPh.T_so_luong = LSCt.Sum(x => x.So_luong);
        CurPh.T_tien_nt = LSCt.Sum(x => x.Tien_nt);
        CurPh.T_tien = LSCt.Sum(x => x.Tien);
    }
}
```

### B??c 2: X? lý khi ch?n mã v?t t?
```csharp
async void OnGridCT_DMDM_Changed(ExpandoObject value, string fieldName)
{
    switch (fieldName.ToLower())
    {
        case "ma_vt":
            // L?y thông tin v?t t? t? danh m?c
            CurrentEditModel.Ma_vt = value.GetValue("ma_vt");
            CurrentEditModel.Ten_vt = value.GetValue("ten_vt");
            CurrentEditModel.Dvt = value.GetValue("dvt");
            CurrentEditModel.Tk_vt = value.GetValue("tk_vt");
            CurrentEditModel.Ma_nx_i = value.GetValue("tk_vt"); // Ho?c t? kho nh?p
            
            // Ki?m tra t?n kho
            if (CurrentEditModel.Vt_ton_kho == 1)
            {
                CurrentEditModel.Ton13 = GetTon13(
                    CurPh.Ma_kho, 
                    CurrentEditModel.Ma_vt, 
                    CurrentEditModel.Ma_vv_i
                );
            }
            break;
            
        case "ma_lo":
            // X? lý khi ch?n lô hàng
            break;
    }
}

// 3. L?y t?n kho 13
public decimal GetTon13(string ma_kho, string ma_vt, string ma_vv)
{
    string sql = $"exec [GetTon13] '{ma_kho}', '{ma_vt}', '{ma_vv}'";
    DataSet ds = myDb.LoadDataSet(sql);
    if (ds.Tables[0].Rows.Count > 0)
        return Convert.ToDecimal(ds.Tables[0].Rows[0]["ton13"]);
    return 0;
}
```

### B??c 3: X? lý t? giá
```csharp
void OnAutoCompleteMaNTChanged(ExpandoObject value)
{
    string sql = $"select ty_gia from dmtgnt where ma_nt = '{CurPh.Ma_nt}'";
    DataSet ds = myDb.LoadDataSet(sql);
    if (ds != null && ds.Tables.Count > 0)
    {
        CurPh.Ty_gia = decimal.Parse(ds.Tables[0].Rows[0][0].ToString());
    }
    
    NotMaNt0Voucher = !CurPh.Ma_nt.Equals(M_ma_nt0);
    TyGiaValueChanged(true);
}

private async Task TyGiaValueChanged(bool isMaNtChanged)
{
    decimal tyGia = CurPh.Ty_gia;
    
    // C?p nh?t giá và ti?n cho t?t c? dòng chi ti?t
    foreach (var ct in LSCt)
    {
        ct.Gia = Math.Round(ct.Gia_nt * tyGia, M_ROUND_GIA);
        ct.Tien = Math.Round(ct.Tien_nt * tyGia, M_ROUND);
    }
    
    CurPh.T_tien = LSCt.Sum(a => a.Tien);
    await InvokeAsync(StateHasChanged);
}
```

### B??c 4: Validation và Save
```csharp
async Task<bool> CheckValid()
{
    // 1. Ki?m tra kho xu?t
    if (string.IsNullOrEmpty(CurPh.Ma_kho))
    {
        Message = Lap["Ch?a có kho xu?t!"];
        return false;
    }
    
    // 2. Ki?m tra kho nh?p
    if (string.IsNullOrEmpty(CurPh.Ma_khon))
    {
        Message = Lap["Ch?a có kho nh?p!"];
        return false;
    }
    
    // 3. Ki?m tra kho xu?t khác kho nh?p
    if (CurPh.Ma_kho == CurPh.Ma_khon)
    {
        Message = Lap["Kho xu?t và kho nh?p ph?i khác nhau!"];
        return false;
    }
    
    // 4. Ki?m tra ngày ch?ng t?
    if (CurPh.Ngay_ct == default)
    {
        Message = Lap["Ch?a vào ngày h?ch toán!"];
        return false;
    }
    
    // 5. Ki?m tra chi ti?t
    if (LSCt.Count == 0)
    {
        Message = Lap["Ch?a vào chi ti?t v?t t?!"];
        return false;
    }
    
    // 6. Ki?m tra t?ng dòng chi ti?t
    for (int i = 0; i < LSCt.Count; i++)
    {
        var row = LSCt[i];
        
        if (string.IsNullOrEmpty(row.Ma_vt))
        {
            Message = Lap["Ch?a nh?p mã v?t t?!"];
            return false;
        }
        
        if (row.So_luong == 0)
        {
            Message = Lap["S? l??ng ph?i l?n h?n 0!"];
            return false;
        }
        
        if (string.IsNullOrEmpty(row.Tk_vt))
        {
            Message = Lap["Ch?a vào tài kho?n n?!"];
            return false;
        }
        
        if (string.IsNullOrEmpty(row.Ma_nx_i))
        {
            Message = Lap["Ch?a vào tài kho?n có!"];
            return false;
        }
    }
    
    return true;
}

private async Task Save()
{
    if (!await CheckValid())
        return;
    
    await GridCT.SaveChangesAsync();
    
    // C?p nh?t audit fields
    CurPh.Date = DateTime.Now.Date;
    CurPh.Time = DateTime.Now.TimeOfDay;
    CurPh.User_id = curUser.User_id.ToString();
    CurPh.Ma_dvcs = myStateMN.GetMa_Dvcs();
    
    // Cân b?ng ti?n n?u c?n
    CanBangTien();
    
    // C?p nh?t ngày ct, s? ct cho chi ti?t
    foreach (Ct85 ct in LSCt)
    {
        ct.Ngay_ct = CurPh.Ngay_ct;
        ct.Ma_ct = CurPh.Ma_ct;
        ct.So_ct = CurPh.So_ct;
    }
    
    if (curActionTask == ActionTask.New)
    {
        CurPh.Date0 = DateTime.Now.Date;
        CurPh.Time0 = DateTime.Now.TimeOfDay;
        CurPh.User_id0 = curUser.User_id.ToString();
        
        // Insert vào database
        myDb.UpdateDataRowFromModel<Ph85>(DsTrans.Tables[0].Rows[0], CurPh);
        await myDb.UpdateRow(M_Phdbf, DsTrans.Tables[0].Rows[0], 
            "stt_rec='" + CurPh.Stt_rec + "'", "stt_rec,row_id");
        
        if (LSCt.Count > 0)
            await myDb.InsertManyRow<Ct85>(LSCt, M_Ctdbf, "stt_rec", CurPh.Stt_rec);
    }
    else if (curActionTask == ActionTask.Edit)
    {
        // Update database
        myDb.UpdateDataRowFromModel<Ph85>(DsTrans.Tables[0].Rows[0], CurPh);
        await myDb.UpdateRow(M_Phdbf, DsTrans.Tables[0].Rows[0], 
            "stt_rec='" + CurPh.Stt_rec + "'", "stt_rec,row_id");
        
        if (LSCt.Count > 0)
        {
            await myDb.SaveDataTable<Ct85>(LSCt, M_Ctdbf, 
                "stt_rec", CurPh.Stt_rec);
        }
    }
    
    // Post voucher
    if (CurPh.Status == "2" || CurPh.Status == "1")
    {
        Post();
        UpdateTonKho();
    }
    
    IsSaved = true;
    Message = String.Format(Lap["Successfully saved: {0}"], CurPh.So_ct);
}

private void Post()
{
    string postStore = "[PXE-Post]";
    string sql = $"exec {postStore} '{CurPh.Stt_rec}', 0";
    myDb.ExecuteSqlQueryAsync(sql);
}

private async Task UpdateTonKho()
{
    // C?p nh?t t?n kho cho các dòng chi ti?t
    // Xem logic trong FrmInctpxd.razor
}
```

### B??c 5: X? lý checkbox
```csharp
private async Task ChkSua_tien_Click(bool value)
{
    CurPh.Sua_tien = value ? 1 : 0;
    if (CurPh.Sua_tien == 0)
    {
        // T? ??ng tính l?i ti?n
        foreach (var ct in LSCt)
        {
            ct.Tien_nt = Math.Round(ct.So_luong * ct.Gia_nt, M_ROUND_NT);
            ct.Tien = Math.Round(ct.Tien_nt * CurPh.Ty_gia, M_ROUND);
        }
        PreSum_All();
    }
}

private async Task ChkPx_gia_dd_Click(bool value)
{
    CurPh.Px_gia_dd = value ? 1 : 0;
    if (CurPh.Px_gia_dd == 0)
    {
        // Reset giá v? 0 n?u không xu?t giá ?ích danh
        foreach (Ct85 ct in LSCt)
        {
            ct.Gia_nt = 0;
            ct.Tien_nt = 0;
            ct.Gia = 0;
            ct.Tien = 0;
        }
        CurPh.T_tien = 0;
        CurPh.T_tien_nt = 0;
        StateHasChanged();
    }
}
```

## ?? Notes quan tr?ng

1. **Lu?ng nghi?p v? chính:**
   - Ch?n kho xu?t, kho nh?p
   - Nh?p chi ti?t v?t t?
   - H? th?ng t? ??ng l?y giá (n?u không ch?n giá ?ích danh)
   - Tính t?ng ti?n t? ??ng
   - Validate ??y ?? tr??c khi l?u
   - Post t? ??ng c?p nh?t t?n kho

2. **Khác bi?t so v?i Phi?u xu?t bán (PXD):**
   - Không có khách hàng (ma_kh)
   - Không có thu?
   - Có kho nh?p (ma_khon)
   - Có thông tin l?nh ?i?u ??ng

3. **C?n t?o Stored Procedure:**
   ```sql
   CREATE PROCEDURE [PXE-Post]
       @stt_rec CHAR(11)
   AS
   BEGIN
       -- Logic post phi?u xu?t ?i?u chuy?n
       -- C?p nh?t t?n kho
       -- T?o bút toán k? toán
   END
   ```
