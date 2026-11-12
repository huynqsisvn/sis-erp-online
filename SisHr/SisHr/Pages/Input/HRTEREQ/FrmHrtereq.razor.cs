using Blazored.LocalStorage;
using DevExpress.Blazor;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using SisCom;
using SisCom.Model;
using System;
using System.Data;
using System.Dynamic;
using System.Threading.Tasks;

namespace SisHr.Pages.Input.HRTEREQ
{
    public partial class FrmHrtereq
    {
        [Inject] public ILocalStorageService myLcStore { get; set; }

        private DotNetObjectReference<FrmHrtereq> Pageobj;

        public string UserLang = "";
        public bool IsLangEn = false;

        public HrtereqRow CurRow { get; set; } = new HrtereqRow();

        private bool IsNew  => curActionTask.ToString() == "New";
        private bool IsEdit => curActionTask.ToString() == "Edit";
        private bool IsView => curActionTask.ToString() == "View";

        protected override void OnInitialized()
        {
            sqlTableName     = "dmtuyendung";
            sqlTableView     = "v_dmtuyendung";
            SqlTableKey      = "so_yeu_cau";
            CheckSqlTableKey = "so_yeu_cau";
            ParentPage       = string.IsNullOrEmpty(ParentPage) ? "hrtereq" : ParentPage;
            CurRow           = new HrtereqRow();

            if (string.IsNullOrEmpty(Menu_id))
                Menu_id = "wr.ct.41";

            base.OnInitialized();
        }

        private async Task CreateNewRecord()
        {
            CurRow                  = new HrtereqRow();
            CurRow.Ngay_yeu_cau     = DateTime.Now.Date;
            CurRow.Status           = 1;
            CurRow.So_luong         = 0;
            CurRow.Tuoi_tu          = 0;
            CurRow.Tuoi_den         = 0;
            CurRow.So_thang_kinh_nghiem = 0;

            AllowEditMode = true;
            AllowEditMa   = true;
        }

        private async Task LoadData(string key)
        {
            try
            {
                string safe  = key.Trim().Replace("'", "''");
                string sql   = "SELECT * FROM v_dmtuyendung WHERE so_yeu_cau = '" + safe + "'";
                DataSet ds   = await myDb.LoadDataset(sql);
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    var list = myDb.ConvertDataTable<HrtereqRow>(ds.Tables[0]);
                    if (list != null && list.Count > 0)
                        CurRow = list[0];
                }

                if (IsView)
                {
                    AllowEditMa   = false;
                    AllowEditMode = false;
                }
                else if (IsEdit)
                {
                    AllowEditMode = true;
                    AllowEditMa   = false;
                }

                StateHasChanged();
            }
            catch (Exception ex)
            {
                myStateMN.SetNewThongbao(Lap["Error loading data"] + ": " + ex.Message);
                StateHasChanged();
            }
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            try
            {
                if (firstRender)
                {
                    UserLang = await myLcStore.GetItemAsync<string>("userlang");
                    if (!string.IsNullOrEmpty(UserLang))
                        IsLangEn = !UserLang.Equals("Vi");

                    string langCode = IsLangEn ? "en" : "vi";
                    await Lap.LoadLocalizationAsync($"hr-{langCode}");

                    string action = curActionTask.ToString();
                    if (action == "View" || action == "Edit")
                    {
                        if (!string.IsNullOrEmpty(Ma_ma))
                            await LoadData(Ma_ma);
                    }
                    else if (action == "New")
                    {
                        await CreateNewRecord();
                    }

                    await JavacriptS.InvokeVoidAsync("eval", "document.getElementById('so_yeu_cau_input')?.focus()");

                    Pageobj = DotNetObjectReference.Create(this);
                    await JavacriptS.InvokeVoidAsync("SetPageObj", Pageobj);
                    await JavacriptS.InvokeVoidAsync("AttachEnterAsTab");

                    StateHasChanged();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        void MaBpns_Changed(ExpandoObject value, string fieldName = "")
        {
            if (value == null) return;
            var dict = (IDictionary<string, object>)value;
            CurRow.Ten_bpns = dict.ContainsKey("ten_bpns") ? dict["ten_bpns"]?.ToString() ?? "" : "";
            if (dict.ContainsKey("ma_dvcs"))
                CurRow.Ma_dvcs = dict["ma_dvcs"]?.ToString() ?? "";
            InvokeAsync(StateHasChanged);
        }

        void MaCn_Changed(ExpandoObject value, string fieldName = "")
        {
            if (value == null) return;
            var dict = (IDictionary<string, object>)value;
            CurRow.Ten_cn = dict.ContainsKey("ten_cn") ? dict["ten_cn"]?.ToString() ?? "" : "";
            InvokeAsync(StateHasChanged);
        }

        void MaVh_Changed(ExpandoObject value, string fieldName = "")
        {
            if (value == null) return;
            var dict = (IDictionary<string, object>)value;
            CurRow.Ten_vh = dict.ContainsKey("ten_vh") ? dict["ten_vh"]?.ToString() ?? "" : "";
            InvokeAsync(StateHasChanged);
        }

        void MaCv_Changed(ExpandoObject value, string fieldName = "")
        {
            if (value == null) return;
            var dict = (IDictionary<string, object>)value;
            CurRow.Ten_cv = dict.ContainsKey("ten_cv") ? dict["ten_cv"]?.ToString() ?? "" : "";
            InvokeAsync(StateHasChanged);
        }

        void MaGioitinh_Changed(ExpandoObject value, string fieldName = "")
        {
            if (value == null) return;
            var dict = (IDictionary<string, object>)value;
            CurRow.Ten_gioitinh = dict.ContainsKey("ten_gioitinh") ? dict["ten_gioitinh"]?.ToString() ?? "" : "";
            InvokeAsync(StateHasChanged);
        }

        void HoKhau_Changed(ExpandoObject value, string fieldName = "")
        {
            if (value == null) return;
            var dict = (IDictionary<string, object>)value;
            CurRow.Ten_tinhthanh = dict.ContainsKey("ten_tinhthanh") ? dict["ten_tinhthanh"]?.ToString() ?? "" : "";
            InvokeAsync(StateHasChanged);
        }

        void NoiLamViec_Changed(ExpandoObject value, string fieldName = "")
        {
            if (value == null) return;
            var dict = (IDictionary<string, object>)value;
            // noi_lam_viec also uses dmtinhthanh
            InvokeAsync(StateHasChanged);
        }

        private async Task SaveData()
        {
            try
            {
                await Task.Delay(200);

                if (!CheckValid())
                    return;

                int userId      = curUser != null ? curUser.User_id : 0;
                string userName = curUser != null ? curUser.User_name : "";
                DateTime now    = DateTime.Now;

                string safeSo = CurRow.So_yeu_cau.Trim().Replace("'", "''");

                if (IsNew)
                {
                    string sqlCheck = "SELECT COUNT(1) FROM dmtuyendung WHERE so_yeu_cau='" + safeSo + "'";
                    DataTable dtChk = myDb.GetTable(sqlCheck);
                    if (dtChk != null && dtChk.Rows.Count > 0 && Convert.ToInt32(dtChk.Rows[0][0]) > 0)
                    {
                        myStateMN.SetNewThongbao(Lap["Recruitment request number already exists"] + "!");
                        await JavacriptS.InvokeVoidAsync("eval", "document.getElementById('so_yeu_cau_input')?.focus()");
                        return;
                    }

                    CurRow.Date0      = now.Date;
                    CurRow.Time0      = now.ToString("HH:mm:ss");
                    CurRow.Date       = now.Date;
                    CurRow.Time       = now.ToString("HH:mm:ss");
                    CurRow.User_id0   = userId;
                    CurRow.User_name0 = userName;
                    CurRow.User_id    = userId;
                    CurRow.User_name  = userName;

                    await myDb.InsertOneRow<HrtereqRow>(CurRow, "dmtuyendung");
                    myStateMN.SetNewThongbao(Lap["Successfully added new"] + ": " + CurRow.So_yeu_cau);
                }
                else if (IsEdit)
                {
                    CurRow.Date      = now.Date;
                    CurRow.Time      = now.ToString("HH:mm:ss");
                    CurRow.User_id   = userId;
                    CurRow.User_name = userName;

                    string whereClause = "so_yeu_cau = '" + safeSo + "'";
                    await myDb.UpdateOneRow1<HrtereqRow>(CurRow, "dmtuyendung", "", whereClause);
                    myStateMN.SetNewThongbao(Lap["Edit successfully"] + ": " + CurRow.So_yeu_cau);
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
            if (string.IsNullOrWhiteSpace(CurRow.So_yeu_cau))
            {
                myStateMN.SetNewThongbao(Lap["Recruitment request number is required"] + "!");
                return false;
            }

            if (string.IsNullOrWhiteSpace(CurRow.Ma_bpns))
            {
                myStateMN.SetNewThongbao(Lap["Department is required"] + "!");
                return false;
            }

            if (CurRow.Ngay_yeu_cau == null)
            {
                myStateMN.SetNewThongbao(Lap["Request date is required"] + "!");
                return false;
            }

            return true;
        }

        public void Dispose()
        {
            Pageobj?.Dispose();
        }
    }
}
