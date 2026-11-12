# DMQTBH - Insurance History (Quá trình tham gia b?o hi?m nhân viên)

## Overview
Manages employee social insurance (BHXH) and health insurance (BHYT) participation history.

## Features
- Create, Edit, View, Delete, Export, Print

## Files
- `Qtbh.cs` - Data model
- `FrmDmqtbh.razor` - List/browse component
- `EditDmqtbh.razor` - Add/edit/view form

## Configuration
- **Menu ID**: wr.ct.09
- **Table**: DMQTBH / View: v_DMQTBH
- **Primary Key**: qtbhid (GUID)

## Key Fields
- `ma_ns` – Employee code (required)
- `so_the_bhxh` / `tu_ngay` / `den_ngay` – Social insurance
- `so_the_bhyt` / `bhyt_tungay` / `bhyt_denngay` – Health insurance
- `donvi_bh`, `donvi_cv` – Insurance unit / occupation
- `ghi_chu`, `status` – Note, status
