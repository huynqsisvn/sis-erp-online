# HRTPGD - Family Relationship Management (Quan h? gia ?ình)

## Overview
This module manages family relationship information for employees. It allows users to add, edit, view, and delete family member details linked to employees.

## Features
- **View**: Display all family members, filter by employee
- **Add**: Create new family member records
- **Edit**: Modify existing family member information  
- **Delete**: Remove family member records
- **Export**: Export family member data to file

## Database Tables
- **dmtpgd**: Main table for family members
- **v_dmtpgd**: View for family members data

## Key Fields
- `ma_tpgd`: Family member ID (unique identifier)
- `ma_ns`: Employee ID (filter and reference)
- `ten_tpgd`: Family member name
- `stt`: Order number (auto-calculated)
- `ma_qhgd`: Family relationship type
- `nguoi_phu_thuoc`: Dependent status (0 = No, 1 = Yes)
- `status`: Active status (0 = Inactive, 1 = Active)

## Validation Rules
- Employee code is required
- Family member name is required
- Personal tax code must be numeric (if provided)
- ID number must be numeric (if provided)
- Birth year must be numeric (if provided)
- Order number is auto-calculated

## Localization
All labels are localized in `hr-vi.xml` with English translations for bilingual support.

## Components
- `Hrtpgd.razor`: Main list component with filter and actions
- `EditHrtpgd.razor`: Detail form for create/edit/view operations
- `HRTPGD.cs`: Data model class
