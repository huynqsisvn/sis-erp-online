# HRHDHV - Internship Contract Management (Quản lý Hợp đồng Thử việc)

## Overview
This module manages internship contracts for employees, including creating, editing, viewing, deleting, and printing contract information.

## Features
- **Create New**: Add new internship contracts with full employee and company information
- **Edit**: Modify existing contract details
- **View**: View contract information in read-only mode
- **Delete**: Remove contracts from the system
- **Print**: Generate reports for contracts
- **Export**: Export contract data to Excel

## Files Structure
- `Hdhv.cs` - Main model for internship contract
- `HdhvModel.cs` - Display models for list and detail views
- `FrmHrhdhv.razor` - Main list component
- `EditHrhdhv.razor` - Form for create/edit/view operations

## Menu Configuration
- **Menu ID**: wr.ct.04
- **Page Name**: HRHDHV
- **Database Table**: dmhdhv

## Key Fields
- **Contract Number** (Ma_hdhv) - Primary key
- **Company Representative** (Ma_ns_ct) - Auto-linked from dmns
- **Employee** (Ma_ns) - Auto-linked from dmns
- **Contract Type** (Ma_lhd) - Reference to dmlhd
- **From/To Date** - Contract period
- **Work Location & Address** - Work details
- **Position** (Ma_cv) - Reference to dmcv
- **Salary & Allowances** - Compensation details
- **Effective/Contract Date** - Dates for contract execution
- **Status** - Active (1) or Inactive (0)

## Validation Rules
- Contract number is required and must be unique
- Company representative is required
- Employee is required
- From date must be <= To date (if To date is specified)
- Effective date must be >= Contract date
- All date fields are required

## Language Support
- Vietnamese and English labels via Lap dictionary
- All messages and notifications are localized
