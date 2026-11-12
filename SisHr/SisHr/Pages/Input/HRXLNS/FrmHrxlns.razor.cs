using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using SisCom.Model;
using SisCom.Shared;
using SisHr.Pages.Input.HRXLNS;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Data;
using System.Threading.Tasks;

namespace SisHr.Pages.Input.HRXLNS
{
    public partial class FrmHrxlns
    {
        public HrxlnsRow CurRow { get; set; } = new HrxlnsRow();

        // Helper: avoid direct ActionTask enum reference in .cs partial (use string comparison like FrmDmxnns pattern)
        private bool IsNew  => curActionTask.ToString() == "New";
        private bool IsEdit => curActionTask.ToString() == "Edit";
        private bool IsView => curActionTask.ToString() == "View";

        // Status list for ComboBox
        private static readonly List<StatusItem> LSStatus = new List<StatusItem>
        {
            new StatusItem { Ma = "0", Ten = "0 - Có" },
            new StatusItem { Ma = "1", Ten = "1 - Không s? d?ng" }
        };

        protected override void OnInitialized()
        {
            sqlTableName = "dmxlns";
            sqlTableView = "v_dmxlns";
            SqlTableKey = "idxl";
            CheckSqlTableKey = "idxl";
            ParentPage = string.IsNullOrEmpty(ParentPage) ? "hrxlns" : ParentPage;
            CurRow = new HrxlnsRow();

            if (string.IsNullOrEmpty(Menu_id))
                Menu_id = "wr.ct.12";

            base.OnInitialized();
        }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                await PageLoad();

                if (IsView)
                {
                    if (!string.IsNullOrEmpty(Ma_ma))
                        await ViewRecord(Ma_ma);
                }
                else if (IsEdit)
                {
                    if (!string.IsNullOrEmpty(Ma_ma))
                        await EditRecord(Ma_ma);
                }
                else if (IsNew)
                {
                    await NewRecord(Ma_ma);
                }
                else
                {
                    if (!string.IsNullOrEmpty(Ma_ma))
                        await ViewRecord(Ma_ma);
                }
            }
            catch (Exception ex)
            {
                myStateMN.SetNewThongbao("Error: " + ex.Message);
                StateHasChanged();
            }
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            try
            {
                if (firstRender)
                {
                    await base.OnAfterRenderAsync(firstRender);
                    await JavacriptS.InvokeVoidAsync("eval", "document.getElementById('nam_input')?.focus()");
                    StateHasChanged();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        async Task OnEdit()
        {
            AllowEditMode = true;
            AllowEditMa = false;
            StateHasChanged();
        }

        async Task ViewRecord(string key)
        {
            CurRow = new HrxlnsRow();
            AllowEditMode = false;
            AllowEditMa = false;
            await LoadData(key);
        }

        async Task EditRecord(string key)
        {
            CurRow = new HrxlnsRow();
            AllowEditMode = true;
            AllowEditMa = false;
            await LoadData(key);
        }

        async Task NewRecord(string key)
        {
            CurRow = new HrxlnsRow();
            CurRow.Idxl = Guid.NewGuid().ToString();
            CurRow.Nam = DateTime.Now.Year;
            CurRow.Thang = DateTime.Now.Month;
            CurRow.Status = "0";
            CurRow.Ghi_chu = "";
            CurRow.Ma_ns = key ?? "";

            AllowEditMode = true;
            AllowEditMa = true;
            StateHasChanged();
        }

        async Task LoadData(string key)
        {
            try
            {
                string safe = key.Trim().Replace("'", "''");
                string sql = "SELECT * FROM " + sqlTableView + " WHERE idxl = '" + safe + "'";
                DataSet ds = await myDb.LoadDataset(sql);
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    List<HrxlnsRow> list = myDb.ConvertDataTable<HrxlnsRow>(ds.Tables[0]);
                    if (list != null && list.Count > 0)
                    {
                        CurRow = list[0];
                        CurRow.Ghi_chu = CurRow.Ghi_chu ?? "";
                        CurRow.Status = CurRow.Status ?? "0";
                    }
                }
                StateHasChanged();
            }
            catch (Exception ex)
            {
                myStateMN.SetNewThongbao("Error loading data: " + ex.Message);
                StateHasChanged();
            }
        }

        void Ma_ns_Changed(ExpandoObject value)
        {
            StateHasChanged();
        }

        void Ma_xlld_Changed(ExpandoObject value)
        {
            StateHasChanged();
        }

        async Task SaveHrxlns()
        {
            try
            {
                if (!CheckValid())
                    return;

                CurRow.Ghi_chu = CurRow.Ghi_chu ?? "";
                CurRow.Ma_ns = (CurRow.Ma_ns ?? "").Trim();
                CurRow.Ma_xlld = (CurRow.Ma_xlld ?? "").Trim();

                int userId = curUser?.User_id ?? 0;
                string userName = curUser?.User_name ?? "";
                DateTime now = DateTime.Now;

                if (IsNew)
                {
                    string safeNs = CurRow.Ma_ns.Replace("'", "''");
                    string sqlCheck = "SELECT COUNT(1) FROM " + sqlTableName + " WHERE ma_ns='" + safeNs + "' AND nam=" + CurRow.Nam + " AND thang=" + CurRow.Thang;
                    DataTable dtChk = myDb.GetTable(sqlCheck);
                    if (dtChk != null && dtChk.Rows.Count > 0 && Convert.ToInt32(dtChk.Rows[0][0]) > 0)
                    {
                        myStateMN.SetNewThongbao(Lap["Employee classification already exists for this month"] + "!");
                        return;
                    }

                    CurRow.Date0 = now.Date;
                    CurRow.Time0 = now.ToString("HH:mm:ss");
                    CurRow.User_id0 = userId;
                    CurRow.User_name0 = userName;
                    CurRow.Date = now.Date;
                    CurRow.Time = now.ToString("HH:mm:ss");
                    CurRow.User_id = userId;
                    CurRow.User_name = userName;

                    await myDb.InsertOneRow<HrxlnsRow>(CurRow, sqlTableName);
                    myStateMN.SetNewThongbao(Lap["Successfully added new"] + ": " + CurRow.Ma_ns);
                }
                else if (IsEdit)
                {
                    CurRow.Date = now.Date;
                    CurRow.Time = now.ToString("HH:mm:ss");
                    CurRow.User_id = userId;
                    CurRow.User_name = userName;

                    string safeId = Ma_ma.Replace("'", "''");
                    await myDb.UpdateOneRow1<HrxlnsRow>(CurRow, sqlTableName, "", "idxl = '" + safeId + "'");
                    myStateMN.SetNewThongbao(Lap["Edit successfully"] + ": " + CurRow.Ma_ns);
                }

                if (DxWindowmodel != null)
                    myModal.CloseModal(DxWindowmodel);

                if (ShowAdd == true)
                    OnClose.InvokeAsync();
                else
                    myNavi.NavigateTo(ParentPage);
            }
            catch (Exception ex)
            {
                myStateMN.SetNewThongbao(Lap["Save failed"] + ": " + ex.Message);
                StateHasChanged();
            }
        }

        bool CheckValid()
        {
            if (CurRow.Nam < 1900 || CurRow.Nam > 2500)
            {
                myStateMN.SetNewThongbao(Lap["Year is required"] + "!");
                return false;
            }
            if (CurRow.Thang < 1 || CurRow.Thang > 12)
            {
                myStateMN.SetNewThongbao(Lap["Month is required"] + "!");
                return false;
            }
            if (string.IsNullOrWhiteSpace(CurRow.Ma_ns))
            {
                myStateMN.SetNewThongbao(Lap["Employee code is required"] + "!");
                return false;
            }
            if (string.IsNullOrWhiteSpace(CurRow.Ma_xlld))
            {
                myStateMN.SetNewThongbao(Lap["Labor classification code is required"] + "!");
                return false;
            }
            return true;
        }

        // Helper class for Status ComboBox
        public class StatusItem
        {
            public string Ma { get; set; }
            public string Ten { get; set; }
        }
    }
}

