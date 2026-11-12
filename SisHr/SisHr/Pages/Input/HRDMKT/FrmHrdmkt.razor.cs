using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using SisCom.Model;
using SisCom.Shared;
using SisHr.Pages.Input.HRDMKT;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Data;
using System.Threading.Tasks;

namespace SisHr.Pages.Input.HRDMKT
{
    public partial class FrmHrdmkt
    {
        public HrdmktRow CurRow { get; set; } = new HrdmktRow();

        private bool IsNew  => curActionTask.ToString() == "New";
        private bool IsEdit => curActionTask.ToString() == "Edit";
        private bool IsView => curActionTask.ToString() == "View";

        private static readonly List<StatusItem> LSStatus = new List<StatusItem>
        {
            new StatusItem { Ma = "0", Ten = "0 - Không sử dụng" },
            new StatusItem { Ma = "1", Ten = "1 - Có" }
        };

        protected override void OnInitialized()
        {
            sqlTableName = "dmkt";
            sqlTableView = "v_dmkt";
            SqlTableKey  = "ktid";
            CheckSqlTableKey = "ktid";
            ParentPage   = string.IsNullOrEmpty(ParentPage) ? "hrdmkt" : ParentPage;
            CurRow       = new HrdmktRow();

            if (string.IsNullOrEmpty(Menu_id))
                Menu_id = "wr.ct.13";

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
                    await JavacriptS.InvokeVoidAsync("eval", "document.getElementById('ma_quyet_dinh_input')?.focus()");
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
            AllowEditMa   = false;
            StateHasChanged();
        }

        async Task ViewRecord(string key)
        {
            CurRow        = new HrdmktRow();
            AllowEditMode = false;
            AllowEditMa   = false;
            await LoadData(key);
        }

        async Task EditRecord(string key)
        {
            CurRow        = new HrdmktRow();
            AllowEditMode = true;
            AllowEditMa   = false;
            await LoadData(key);
        }

        async Task NewRecord(string key)
        {
            CurRow                   = new HrdmktRow();
            CurRow.Ktid              = null;  // Let database generate IDENTITY
            CurRow.Ngay_cap_nhat     = DateTime.Now.Date;
            CurRow.So_lan_thuong_phat = 0;
            CurRow.Status            = "1";
            CurRow.Ghi_chu           = "";
            CurRow.Ly_do_thuong      = "";
            CurRow.Ly_do_phat        = "";
            CurRow.Ma_ns             = key ?? "";

            AllowEditMode = true;
            AllowEditMa   = true;
            StateHasChanged();
        }

        async Task LoadData(string key)
        {
            try
            {
                string safe = key.Trim().Replace("'", "''");
                string sql  = "SELECT * FROM " + sqlTableView + " WHERE ktid = '" + safe + "'";
                DataSet ds  = await myDb.LoadDataset(sql);
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    List<HrdmktRow> list = myDb.ConvertDataTable<HrdmktRow>(ds.Tables[0]);
                    if (list != null && list.Count > 0)
                    {
                        CurRow                   = list[0];
                        CurRow.Ghi_chu           = CurRow.Ghi_chu       ?? "";
                        CurRow.Ly_do_thuong      = CurRow.Ly_do_thuong  ?? "";
                        CurRow.Ly_do_phat        = CurRow.Ly_do_phat    ?? "";
                        CurRow.Status            = CurRow.Status         ?? "1";
                        CurRow.So_lan_thuong_phat = CurRow.So_lan_thuong_phat ?? 0;
                        LoadLookupNames();
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

        void LoadLookupNames()
        {
            if (!string.IsNullOrWhiteSpace(CurRow.Ma_ns))
            {
                string safe = CurRow.Ma_ns.Trim().Replace("'", "''");
                CurRow.Ten_ns = myDb.GetValue("SELECT ten_ns FROM dmns WHERE ma_ns = '" + safe + "'")?.ToString() ?? "";
            }

            if (!string.IsNullOrWhiteSpace(CurRow.Ma_hinh_thuc))
            {
                string safe = CurRow.Ma_hinh_thuc.Trim().Replace("'", "''");
                CurRow.Ten_hinh_thuc = myDb.GetValue("SELECT ten_hinh_thuc FROM dmht WHERE ma_hinh_thuc = '" + safe + "'")?.ToString() ?? "";
            }
        }

        void Ma_ns_Changed(ExpandoObject value)
        {
            CurRow.Ten_ns = "";
            if (!string.IsNullOrWhiteSpace(CurRow.Ma_ns))
            {
                string safe = CurRow.Ma_ns.Trim().Replace("'", "''");
                CurRow.Ten_ns = myDb.GetValue("SELECT ten_ns FROM dmns WHERE ma_ns = '" + safe + "'")?.ToString() ?? "";
            }
            CurRow.Ngay_cap_nhat = DateTime.Now.Date;
            StateHasChanged();
        }

        void Ma_hinh_thuc_Changed(ExpandoObject value)
        {
            CurRow.Ten_hinh_thuc = "";
            if (!string.IsNullOrWhiteSpace(CurRow.Ma_hinh_thuc))
            {
                string safe = CurRow.Ma_hinh_thuc.Trim().Replace("'", "''");
                CurRow.Ten_hinh_thuc = myDb.GetValue("SELECT ten_hinh_thuc FROM dmht WHERE ma_hinh_thuc = '" + safe + "'")?.ToString() ?? "";
            }

            // Logic: enable/disable reason fields based on hinh_thuc (mirrors WPF logic)
            string maHt = (CurRow.Ma_hinh_thuc ?? "").Trim().ToUpper();
            if (maHt == "THUONG")
            {
                IsLydoPhatEnabled   = false;
                IsLydoThuongEnabled = true;
                CurRow.Ly_do_phat   = "";
            }
            else if (maHt == "PHAT")
            {
                IsLydoThuongEnabled = false;
                IsLydoPhatEnabled   = true;
                CurRow.Ly_do_thuong = "";
            }
            else
            {
                IsLydoThuongEnabled = true;
                IsLydoPhatEnabled   = true;
            }

            StateHasChanged();
        }

        // Track enabled state for ly_do fields (mirrors WPF txtma_hinh_thuc_PreviewLostFocus logic)
        private bool IsLydoThuongEnabled { get; set; } = true;
        private bool IsLydoPhatEnabled   { get; set; } = true;

        async Task SaveHrdmkt()
        {
            try
            {
                if (!CheckValid())
                    return;

                CurRow.Ma_ns          = (CurRow.Ma_ns          ?? "").Trim().ToUpper();
                CurRow.Ma_quyet_dinh  = (CurRow.Ma_quyet_dinh  ?? "").Trim().ToUpper();
                CurRow.Ma_hinh_thuc   = (CurRow.Ma_hinh_thuc   ?? "").Trim();
                CurRow.Ghi_chu        = (CurRow.Ghi_chu         ?? "").Trim();
                CurRow.Ly_do_thuong   = (CurRow.Ly_do_thuong    ?? "").Trim();
                CurRow.Ly_do_phat     = (CurRow.Ly_do_phat      ?? "").Trim();
                CurRow.So_lan_thuong_phat = CurRow.So_lan_thuong_phat ?? 0;
                CurRow.Status         = CurRow.Status ?? "1";

                int userId      = curUser?.User_id   ?? 0;
                string userName = curUser?.User_name ?? "";
                DateTime now    = DateTime.Now;

                if (IsNew)
                {
                    CurRow.Date0      = now.Date;
                    CurRow.Time0      = now.ToString("HH:mm:ss");
                    CurRow.User_id0   = userId;
                    CurRow.User_name0 = userName;
                    CurRow.Date       = now.Date;
                    CurRow.Time       = now.ToString("HH:mm:ss");
                    CurRow.User_id    = userId;
                    CurRow.User_name  = userName;

                    // NOTE: Save BOTH ly_do fields regardless of ma_hinh_thuc (mirrors WPF logic)
                    // Only UI disables the field, but both values are saved to database
                    await myDb.InsertOneRow<HrdmktRow>(CurRow, sqlTableName);
                    myStateMN.SetNewThongbao(Lap["Successfully added new"] + ": " + CurRow.Ma_ns);
                }
                else if (IsEdit)
                {
                    CurRow.Date      = now.Date;
                    CurRow.Time      = now.ToString("HH:mm:ss");
                    CurRow.User_id   = userId;
                    CurRow.User_name = userName;

                    // NOTE: Save BOTH ly_do fields regardless of ma_hinh_thuc (mirrors WPF logic)
                    // Only UI disables the field, but both values are saved to database

                    // Use N prefix for NVARCHAR to properly handle Vietnamese characters
                    string safeMaNs = (CurRow.Ma_ns ?? "").Replace("'", "''");
                    string safeQuyet = (CurRow.Ma_quyet_dinh ?? "").Replace("'", "''");
                    string safeHinh = (CurRow.Ma_hinh_thuc ?? "").Replace("'", "''");
                    string safeLydoT = (CurRow.Ly_do_thuong ?? "").Replace("'", "''");
                    string safeLydoP = (CurRow.Ly_do_phat ?? "").Replace("'", "''");
                    string safeGhiChu = (CurRow.Ghi_chu ?? "").Replace("'", "''");
                    string safeStatus = (CurRow.Status ?? "1").Replace("'", "''");
                    string safeTime = (CurRow.Time ?? "").Replace("'", "''");
                    string safeUserName = (CurRow.User_name ?? "").Replace("'", "''");
                    string safeKtid = Ma_ma.Replace("'", "''");

                    string updateSql = $@"UPDATE {sqlTableName} SET 
                        ma_ns = N'{safeMaNs}',
                        ma_quyet_dinh = N'{safeQuyet}',
                        ngay_cap_nhat = '{CurRow.Ngay_cap_nhat:yyyy-MM-dd}',
                        ma_hinh_thuc = N'{safeHinh}',
                        so_lan_thuong_phat = {CurRow.So_lan_thuong_phat ?? 0},
                        ly_do_thuong = N'{safeLydoT}',
                        ly_do_phat = N'{safeLydoP}',
                        ghi_chu = N'{safeGhiChu}',
                        status = N'{safeStatus}',
                        date = '{now.Date:yyyy-MM-dd}',
                        time = N'{safeTime}',
                        user_id = {CurRow.User_id ?? 0},
                        user_name = N'{safeUserName}'
                    WHERE ktid = '{safeKtid}'";

                    await myDb.ExecuteSqlQueryAsync(updateSql);
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
            if (string.IsNullOrWhiteSpace(CurRow.Ma_ns))
            {
                myStateMN.SetNewThongbao(Lap["Employee code is required"] + "!");
                return false;
            }

            // Validate ma_quyet_dinh: check for invalid characters (mirrors WPF Checkvalidtxt logic)
            if (!string.IsNullOrWhiteSpace(CurRow.Ma_quyet_dinh))
            {
                string invalid = CheckInvalidCode(CurRow.Ma_quyet_dinh.TrimEnd());
                if (!string.IsNullOrEmpty(invalid))
                {
                    myStateMN.SetNewThongbao(Lap["The code cannot contain characters"] + "!");
                    return false;
                }
            }

            return true;
        }

        // Mirrors WPF SysFunc.CheckInValidCode – checks for disallowed special characters in a code
        private string CheckInvalidCode(string code)
        {
            char[] invalid = { '\'', '"', ';', '-', '-', '/', '\\', '<', '>', '(', ')', '[', ']', '{', '}' };
            string found = "";
            foreach (char c in invalid)
            {
                if (code.Contains(c))
                    found += c + " ";
            }
            return found.Trim();
        }

        public class StatusItem
        {
            public string Ma  { get; set; }
            public string Ten { get; set; }
        }
    }
}
