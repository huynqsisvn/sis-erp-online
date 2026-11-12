# HRQTCT - Work History (Quá trình công tác)

## Overview
Manages work history records for employees. Supports add, edit, view, delete, and print.

## Database
- **dmqtct**: Main table
- **v_dmqtct**: View

## Key Fields
- `ma_qtct`: Record ID (GUID)
- `ma_ns`: Employee code
- `stt`: Order number (auto-calculated)
- `tu_ngay` / `den_ngay`: From/To date
- `cong_viec`: Job title
- `donvi_ct`: Work unit
- `ma_cv`: Position code
- `ghi_chu`: Note
- `status`: Active status (0 = Inactive, 1 = Active)

## Validation
- Employee code is required
- Work unit is required
- Order number must be > 0

## Print Functionality
**Components:**
- `HrqtctPrintModel.cs`: Data model for printing (HrqtctPrintModel, HrqtctPrintRow)
- Print dialog popup: Shows employee code and total records count
- F7 key or Print button triggers print dialog

**Print Process:**
1. Click Print button or press F7
2. Popup appears showing employee info and record count
3. Click OK to generate report (future: integrate with Report Designer template)
4. Data is prepared in HrqtctPrintModel format

**Report Data Structure:**
- Employee code (ma_ns)
- Employee name (ten_ns)
- Print date (ngay_in)
- Detail rows with: STT, From Date, To Date, Job Title, Work Unit, Position, Note

## Components
- `Hrqtct.razor`: Main list page (display all data, filter by employee)
- `EditHrqtct.razor`: Add/Edit/View form
- `HRQTCT.cs`: Model class
- `HrqtctPrintModel.cs`: Print data model
- `README.md`: This file

## Future Enhancements
- Create Report Designer templates (V/Hrqtct.cs, E/Hrqtct.cs) for PDF export
- Add export to Excel function

