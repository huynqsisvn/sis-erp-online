# HRDMNVCT - Business Trip (Nhân viên công tác)

## Overview
Manages employee business trip records.

## Features
- Create, Edit, View, Delete, Export, Print

## Files
- `Hrdmnvct.cs` - Data model
- `FrmHrdmnvct.razor` - List/browse component
- `EditHrdmnvct.razor` - Add/edit/view form

## Configuration
- **Menu ID**: wr.ct.21
- **Table**: dmnvct / View: v_dmnvct
- **Primary Key**: nvctid (GUID)

## Key Fields
- `ma_ns` – Employee code (required)
- `ma_quyet_dinh` – Decision number
- `ngay_di` / `ngay_ve` – Departure / Return date
- `ma_nct` – Trip destination code
- `chi_phi` – Trip cost
- `noi_dung`, `ket_qua`, `ghi_chu` – Content, Result, Note
- `status` – Status (1 = In use, 0 = Not in use)
