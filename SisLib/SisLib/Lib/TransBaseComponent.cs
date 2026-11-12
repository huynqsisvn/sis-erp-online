using Blazored.LocalStorage;
using DevExpress.Blazor;
using DevExpress.CodeParser.Html;
using DevExpress.Data.Browsing;
using DevExpress.DataAccess.ConnectionParameters;
using DevExpress.Office.NumberConverters;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Localization;
using Microsoft.JSInterop;
using SisCom.Model;
using SisCom.Shared;
using SisData.Data;
using SisData.Dataaccess;
using SisData.Model;
using SisData.Service;
using SisFunc;
using SisLib.Ctrl;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata;
using System.Runtime.InteropServices.JavaScript;
using System.Text;
using System.Threading.Tasks;

namespace SisLib.Lib
{
    public class TransBaseComponent : ComponentBase, IDisposable
    {
        #region Parameter
        [Inject] public IOptionsManager myOption { get; set; }
        [Inject] public ISysVarManager mySysvar { get; set; }
        [Inject] public IDxWindowService myModal { get; set; }
        [Inject] public IDbManager myDb { get; set; }
        [Inject] public ISqldataacceess mySqldb { get; set; }
        [Inject] public ILocalStorageService myLcStore { get; set; }
        [Inject] public Statemanagerment myStateMN { get; set; }
        [Inject] public NavigationManager myNavi { get; set; }
        [Inject] public ICommandManager myCommand { get; set; }
        [Inject] public IJSRuntime JavacriptS { get; set; }
        [Inject] public XmlStringLocalizer Lap { get; set; }
        [Inject] public IDialogService myDlSvc { get; set; }
        [Inject] public ISisTypeManager mySType { get; set; }
        public bool? ResultDl { get; set; } = null;
        public string MessageDl { get; set; } = "";        
        [Parameter] public string Menu_id { get; set; }
        public string Ws_Id = string.Empty;
        public string Ma_ct = string.Empty;
        public string Nh_menu;
        public Command curCommand = new Command();
        public Dmct curDmct = new Dmct();
        public DataTable tbStatus = (DataTable)null;
        public DataSet DsTrans = (DataSet)null;
        public ActionTask curActionTask { get; set; } = ActionTask.None;
        public FrmBrowseComponent oBrowsePH;
        public FrmBrowseComponent oBrowseCT;
        public FrmBrowseComponent oBrowseGT;

        public string M_Phdbf = string.Empty;
        public string M_Ctdbf = string.Empty;
        public string M_Ctgtdbf = string.Empty;
        public string SqlTableKey = "stt_rec";
        public string SqlTableKeyValue = "";

        public DataTable TblPH = (DataTable)null;
        public DataTable TblCT = (DataTable)null;
        public DataTable TblGT = (DataTable)null;

        public string FbrowsePH = "";
        public string FbrowseCT = "";
        public string FbrowseGT = "";
        public string phFilter = "1=1";
        public string ctFilter = "1=1";
        public string ctgtFilter = "1=1";

        public DxPopup OptwRef;
        public DxPopup popFilter { get; set; }
        public bool ShowLoadding { get; set; } = false;
        public bool ShowLoaddingVoucher { get; set; } = true;
        public bool popFilterVisible { get; set; } = false;
        public bool IsShowFrm { get; set; } = false;
        public bool IsShowImport { get; set; } = false;
        public Dictionary<string, object> Par { get; set; } = new Dictionary<string, object>();
        public Type FrmType { get; set; }
        public IList LSData;
        public string mPage = "Socthda";
        public string editPage = "frmSocthda";        
        
        public Users curUser;
        public bool IsLangEn = false;
        public string UserLang = "";
        public static string LanguageID = "";
        public DateTime Ngay_ct1 = DateTime.Now.Date;
        public DateTime Ngay_ct2 = DateTime.Now.Date;

        public string filterView = string.Empty;
        public string M_ma_nt0 = string.Empty;        
        public string M_MST_CHECK = string.Empty;
        public int M_User_Id = 0;
        public string M_ngay_lct = string.Empty;
        public string M_ong_ba = string.Empty;
        public int M_sl_ct0 = 0;
        
        public string M_MA_THUE = "";
        public string M_WS_ID = "";
        public string M_IP_SL = "";
        public string M_IP_GIA = "";
        public string M_IP_GIA_NT = "";
        public string M_IP_TIEN = "";
        public string M_IP_TIEN_NT = "";
        public string M_IP_TY_GIA = "";
        public string M_IP_TY_GIAF = "";

        public int M_ROUND;
        public int M_ROUND_NT;
        public int M_ROUND_GIA;
        public int M_ROUND_GIA_NT;
        public string[] Check_Valid_Store;
        public string[] Post_store;
        public string[] Process_store;
        public string Deletemessage { get; set; }       
        public object SelectedItem { get; set; }
        public int iRow = 0;
        public int pageSize = 20;
        public bool IsPageSize = true;
        public bool IsImportPopupVisible { get; set; } = false; 
        public Dmimex CurDmimex { get; set; }

        private DotNetObjectReference<TransBaseComponent>? _dotNetHelper;
        #endregion
        public virtual void CurItem(object obj)
        {
            if (obj != null)
            {
                SelectedItem = obj;
                DataRowView rview = (DataRowView)SelectedItem;
                SqlTableKeyValue = rview[SqlTableKey].ToString().Trim();
                if(TblCT != null)
                    TblCT.DefaultView.RowFilter = SqlTableKey + "='" + SqlTableKeyValue + "'";
                if(TblGT != null)
                {
                    TblGT.DefaultView.RowFilter = SqlTableKey + "='" + SqlTableKeyValue + "'";
                }
                iRow = TblPH.DefaultView.Cast<DataRowView>()
                      .ToList()
                      .FindIndex(row => row == rview);                
            }
            else
            {
                if (TblCT != null) TblCT.DefaultView.RowFilter = SqlTableKey + "=''";
                if (TblGT != null) TblGT.DefaultView.RowFilter = SqlTableKey + "=''";
            }
        }
        protected override void OnInitialized()
        {
            if (String.IsNullOrEmpty(Menu_id))
                Menu_id = "we.sm.23";
            else
            {
                Menu_id = Menu_id.Replace("_", ".");
            }            
            curUser = myStateMN.GetCurUser();
            Ngay_ct1 = myStateMN.GetStartDate();
            Ngay_ct2 = myStateMN.GetEndDate();

            base.OnInitialized();
            
        }
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await Language();                
                await PageLoad();
                if (curCommand == null)
                {
                    myStateMN.SetToolBar(new MyToolbar("toolbar", false));
                    return;
                }
                await Lap.LoadLocalizationAsync(String.Format("{0}-{1}", String.IsNullOrEmpty(LanguageID) ? curCommand.Ma_phan_he.Trim().ToLower() : LanguageID, UserLang.Trim()));
                string Menuid = Menu_id.Replace("_", ".");
                if (curUser.Is_admin == 1 || curUser.CheckRight(Menuid, "VIEW"))
                {
                    myStateMN.SetToolBar(new MyToolbar("toolbar", true));
                    myStateMN.SetToolBar(new MyToolbar("New", true));
                    myStateMN.SetToolBar(new MyToolbar("Edit", true));                    
                    myStateMN.SetToolBar(new MyToolbar("Delete", true));
                    myStateMN.SetToolBar(new MyToolbar("View", true));
                    myStateMN.SetToolBar(new MyToolbar("Print", true));
                    if (IsShowImport) myStateMN.SetToolBar(new MyToolbar("Import", true));
                    myStateMN.SetToolBar(new MyToolbar("Export", true));
                    myStateMN.SetToolBar(new MyToolbar("Search", true));
                    myStateMN.SetToolBar(new MyToolbar("Quit", false));
                }
                else
                {
                    myStateMN.SetToolBar(new MyToolbar("toolbar", false));
                }

                myStateMN.OnActionTaskChanged += OnActionTaskchanged;
                myStateMN.OnLangguaChanged += OnLangguachange;
                myStateMN.OnDvcsChanged += OnDvcsChanged;
                _dotNetHelper = DotNetObjectReference.Create(this);
                await JavacriptS.InvokeVoidAsync("blazorKeyListener.init", _dotNetHelper);
                ShowLoaddingVoucher = false;
                StateHasChanged();
            }
        }
        public virtual void OnDvcsChanged(string ma_dvcs)
        {
            _ = Loaddata(false);
            StateHasChanged();
        }
        public virtual async Task PageLoad()
        {
            M_MST_CHECK = myOption.GetOptionsValue("M_MST_CHECK").ToString().Trim();
            M_MA_THUE = myOption.GetOptionsValue("M_MA_THUE").ToString();
            Ws_Id = myOption.GetOptionsValue("WS_ID").ToString().Trim();
            M_ma_nt0 = myOption.GetOptionsValue("M_MA_NT0").ToString().Trim();

            curCommand = await myCommand.GetSubMenuBymenuId(Menu_id.Replace("_", "."));
            if (curCommand != null)
            {
                mPage = curCommand.Mobile_page;
                Nh_menu = String.IsNullOrEmpty(curCommand.Nh_menu)?"":curCommand.Nh_menu.Trim();
                Ma_ct = curCommand.Ma_ct.Trim();

                string[] strBrowse = (IsLangEn? curCommand.Ebrowse1: curCommand.Vbrowse1).Split('|');
                FbrowsePH = String.IsNullOrEmpty(strBrowse[0])? @"stt_rec:110:h=stt rec;ngay_ct:80:h=Ngày c.từ:FL:D;so_ct:85:h=Số hđ:FL:HR:LINKCT" : strBrowse[0];
                FbrowseCT = strBrowse.Length > 1?strBrowse[1]: @"stt_rec:110:h=stt rec;stt_rec0:110:h=stt rec0";
                FbrowseGT = strBrowse.Length > 2 ? strBrowse[2] : @"stt_rec:110:h=stt rec;stt_rec0:110:h=stt rec0";

                //Load dmct
                string strsql = "Select * from dmct where ma_ct = '" + Ma_ct + "'";
                DataSet dsdmct = myDb.LoadDataSet(strsql);
                if(dsdmct.Tables.Count > 0)
                {                    
                    curDmct = myDb.ConvertDataRow<Dmct>(dsdmct.Tables[0].Rows[0]);
                    M_Phdbf = curDmct.M_phdbf;
                    M_Ctdbf = curDmct.M_ctdbf;
                    M_Ctgtdbf = curDmct.M_ctgtdbf;
                    Process_store = curDmct.Process_Store.Trim().Split('|');
                    Post_store = curDmct.Post_store.Trim().Split('|');
                    Check_Valid_Store = curDmct.Check_valid_store.Trim().Split('|');
                    pageSize = curDmct.M_sl_ct0>0?curDmct.M_sl_ct0: pageSize;
                    IsPageSize = pageSize > curDmct.M_sl_ct0;
                    var maimex = myDb.GetValue("Dmimexct", "ma_imex", "ma_ct='" + curDmct.Ma_ct.Trim() + "'"); maimex = maimex ?? "";
                    IsShowImport = !String.IsNullOrEmpty(maimex.ToString());
                }
                else
                {
                    myStateMN.SetNewThongbao(String.Format(Lap["No voucher {0}"],Ma_ct) + " !");
                    return;
                }
                await Loaddata();
                //string strph = SisFunc.Converts.DataTable2Model(DsTrans.Tables[0], "Ph82");
                //string strct = SisFunc.Converts.DataTable2Model(DsTrans.Tables[1], "Ct82");
                
            }
            else
            {
                myStateMN.SetNewThongbao(Lap["No Command"] + " !");
                return;
            }            
        }
        public virtual async Task Loaddata(bool isStartup = true)
        {
            string _ma_dvcs = myStateMN.GetMa_Dvcs();
            string strFilter = (String.IsNullOrEmpty(phFilter)?"1=1": phFilter) + (_ma_dvcs.ToUpper().Trim().Equals("ALL") ? "" : " AND ma_dvcs like ''" + _ma_dvcs.Trim() + "%''");
            string strsql = "Exec " + Process_store[0].Trim() + " @ma_ct='" + Ma_ct + "',@sPhFilter ='" + strFilter + "',@sCtFilter='" + ctFilter + "',@sGtFilter='" + ctgtFilter + "',@sl_ct=" + (isStartup?curDmct.M_sl_ct0.ToString().Trim():"-1");
            DsTrans = myDb.LoadDataSet(strsql);
            TblPH = DsTrans.Tables.Count > 0 ? DsTrans.Tables[0] : (DataTable)null;
            TblCT = DsTrans.Tables.Count > 1 ? DsTrans.Tables[1] : (DataTable)null;
            TblGT = DsTrans.Tables.Count > 2 ? DsTrans.Tables[2] : (DataTable)null;
            pageSize = curDmct.M_sl_ct0 > 0 ? curDmct.M_sl_ct0 : pageSize;
            IsPageSize = (TblPH.Rows.Count > pageSize?true:false);

            if (DsTrans.Tables.Count <= 0)
            {
                myStateMN.SetNewThongbao(String.Format(Lap["No data {0}"], Ma_ct) + " !");
                return;
            }
            var rowsToDelete = TblPH.AsEnumerable().Where(r => string.IsNullOrWhiteSpace(r["ma_dvcs"]?.ToString()) && string.IsNullOrWhiteSpace(r["so_ct"]?.ToString()) && string.IsNullOrWhiteSpace(r["ngay_ct"]?.ToString())).ToList();

            foreach (var row in rowsToDelete)
            {
                TblPH.Rows.Remove(row);
            }

            if (TblCT != null)
            {
                TblCT.DefaultView.Sort = "stt_rec0 asc";
                if (TblPH.DefaultView.Count > 0)
                    TblCT.DefaultView.RowFilter = SqlTableKey + "='" + TblPH.DefaultView[0][SqlTableKey].ToString().Trim() + "'";
            }
            if (TblGT != null)
            {
                TblGT.DefaultView.Sort = "stt_rec0 asc";
                if (TblGT.DefaultView.Count > 0 && TblPH.DefaultView.Count > 0)
                {
                    TblGT.DefaultView.RowFilter = SqlTableKey + "='" + TblPH.DefaultView[0][SqlTableKey].ToString().Trim() + "'";
                }
            }
            if (TblPH != null)
                TblPH.DefaultView.Sort = "ngay_ct desc, so_ct desc";
        }
        public virtual void OnActionTaskchanged(ActionTask ac)
        {
            switch (ac)
            {
                case ActionTask.New:
                    myStateMN.SetCurrentActionTask(ActionTask.New);
                    V_Moi();
                    break;
                case ActionTask.Copy:
                    myStateMN.SetCurrentActionTask(ActionTask.Copy);
                    V_Copy();
                    break;
                case ActionTask.Edit:
                    myStateMN.SetCurrentActionTask(ActionTask.Edit);
                    V_Sua();
                    break;
                case ActionTask.View:
                    myStateMN.SetCurrentActionTask(ActionTask.View);
                    V_Xem();
                    break;
                case ActionTask.Search:
                    myStateMN.SetCurrentActionTask(ActionTask.Search);
                    V_Tim();
                    break;
                case ActionTask.Print:
                    myStateMN.SetCurrentActionTask(ActionTask.Print);
                    V_In();
                    break;
                case ActionTask.Delete:
                    V_Xoa();
                    break;
                case ActionTask.Import:
                    V_Import();
                    break;
                case ActionTask.Export:
                    ExportData(1);
                    break;
                case ActionTask.Export2:
                    ExportData(2);
                    break;
                case ActionTask.Export3:
                    ExportData(3);
                    break;
            }
        }
        public virtual void V_Moi()
        {
            string _ma_dvcs = myStateMN.GetMa_Dvcs();
            if (String.IsNullOrWhiteSpace(_ma_dvcs) || _ma_dvcs.ToUpper().Trim() == "ALL")
            {
                myStateMN.SetNewThongbao(Lap["Please select a unit!"]);
                return;
            }
            DataRowView rview = (DataRowView)SelectedItem;
            if (rview != null)
            {
                DataRowView newData = (SelectedItem as DataRowView);
                SqlTableKeyValue = newData[SqlTableKey].ToString().Trim();
            }
            else
            {
                SqlTableKeyValue = "";
            }    
            Par.Clear();
            Par.Add("Stt_rec", (object)SqlTableKeyValue);
            Par.Add("Ma_ct", (object)Ma_ct);
            Par.Add("Menu_id", (object)Menu_id);
            Par.Add("curActionTask", (object)ActionTask.New);
            Par.Add("Closeform", EventCallback.Factory.Create<bool>(this, CloseForm));

            IsShowFrm = true;
            StateHasChanged();           
        }
        public virtual void V_Copy()
        {
            string _ma_dvcs = myStateMN.GetMa_Dvcs();
            if (String.IsNullOrWhiteSpace(_ma_dvcs) || _ma_dvcs.ToUpper().Trim() == "ALL")
            {
                myStateMN.SetNewThongbao(Lap["Please select a unit!"]);
                return;
            }

            DataRowView rview = (DataRowView)SelectedItem;
            if (rview != null)
            {
                
            }
        }
        public virtual void V_Sua()
        {
            string _ma_dvcs = myStateMN.GetMa_Dvcs();
            if (String.IsNullOrWhiteSpace(_ma_dvcs) || _ma_dvcs.ToUpper().Trim() == "ALL")
            {
                myStateMN.SetNewThongbao(Lap["Please select a unit!"]);
                return;
            }

            DataRowView rview = (DataRowView)SelectedItem;
            if (rview != null)
            {
                SqlTableKeyValue = rview[SqlTableKey].ToString().Trim();
                if (rview.Row.Table.Columns.Contains("ngay_ct") && rview.Row.Table.Rows.Count > 1 && rview["ngay_ct"] != DBNull.Value && !NgayTC.CheckValidNgayKs(mySysvar, myDb, (DateTime?)rview["ngay_ct"], Ma_ct))
                {
                    myStateMN.SetNewThongbao(Lap["Data is locked, cannot be edited!"]);
                    return;
                }
                if ((!(Ma_ct == "HDA") && !(Ma_ct == "HD1") || !(rview["stt_rec_pt"].ToString().Trim() != "")) && CheckPhanBo(rview,myDb,Post_store,Check_Valid_Store, SqlTableKeyValue) == 1)
                {                    
                    myStateMN.SetNewThongbao(Lap["Do not edit paid invoices!"]);
                    return;
                }
                
                Par.Clear();
                Par.Add("Stt_rec", (object)SqlTableKeyValue);
                Par.Add("Ma_ct", (object)Ma_ct);
                Par.Add("Menu_id", (object)Menu_id);
                Par.Add("curActionTask", (object)ActionTask.Edit);
                Par.Add("Closeform", EventCallback.Factory.Create<bool>(this, CloseForm));

                IsShowFrm = true;
                StateHasChanged();
            }
            else
                myStateMN.SetNewThongbao(Lap["No data selected"]);
        }
        public async void OnRDBClick(GridRowClickEventArgs e)
        {
            V_Xem();
        }
        public virtual void V_Xem()
        {
            DataRowView rview = (DataRowView)SelectedItem;
            if (rview != null)
            {
                DataRowView newData = (SelectedItem as DataRowView);
                SqlTableKeyValue = newData[SqlTableKey].ToString().Trim();

                Par.Clear();
                Par.Add("Stt_rec", (object)SqlTableKeyValue);
                Par.Add("Ma_ct", (object)Ma_ct);
                Par.Add("Menu_id", (object)Menu_id);
                Par.Add("curActionTask", (object)ActionTask.View);
                Par.Add("Closeform", EventCallback.Factory.Create<bool>(this, CloseForm));

                IsShowFrm = true;
                StateHasChanged();
            }
            else
                myStateMN.SetNewThongbao(Lap["No data selected"]);
        }
        public virtual void V_Tim()
        {
            popFilterVisible = true;            
            StateHasChanged();
        }
        async void CloseForm(bool value)
        {
            if (value)
            {
                await Loaddata(false);
            }
            IsShowFrm = false;
            StateHasChanged();
        }
        public virtual async void V_Xoa()
        {
            string _ma_dvcs = myStateMN.GetMa_Dvcs();
            if (String.IsNullOrWhiteSpace(_ma_dvcs) || _ma_dvcs.ToUpper().Trim() == "ALL")
            {
                myStateMN.SetNewThongbao(Lap["Please select a unit!"]);
                return;
            }

            try
            {
                DataRowView rview = (DataRowView)SelectedItem;
                if (rview != null)
                {
                    SqlTableKeyValue = rview[SqlTableKey].ToString().Trim();
                    if (rview.Row.Table.Columns.Contains("ngay_ct") && rview.Row.Table.Rows.Count > 1 && rview["ngay_ct"] != DBNull.Value && !NgayTC.CheckValidNgayKs(mySysvar, myDb, (DateTime?)rview["ngay_ct"], Ma_ct))
                    {
                        myStateMN.SetNewThongbao(Lap["Data is locked, cannot be edited!"]);
                        return;
                    }
                    if ((!(Ma_ct == "HDA") && !(Ma_ct == "HD1") || !(rview["stt_rec_pt"].ToString().Trim() != "")) && CheckPhanBo(rview, myDb, Post_store, Check_Valid_Store, SqlTableKeyValue) == 1)
                    {
                        myStateMN.SetNewThongbao(Lap["Do not edit paid invoices!"]);
                        return;
                    }
                    MessageDl = String.Format(Lap["Do you want to delete :{0} ?"], rview["so_ct"].ToString().Trim());
                    ResultDl = await myDlSvc.ConfirmAsync(new MessageBoxOptions()
                    {
                        Text = MessageDl,
                        Title = Lap["Confirm deletion of voucher"],
                        OkButtonText = Lap["OK"],
                        CancelButtonText = Lap["Cancel"],
                        ShowIcon = true,
                        ShowCloseButton = false,
                        RenderStyle = MessageBoxRenderStyle.Danger,
                    });
                    if (ResultDl == true)
                    {
                        DeleteVoucher(SqlTableKeyValue);
                    }

                    StateHasChanged();
                }
                else
                    myStateMN.SetNewThongbao(Lap["No data selected"]);
            }
            catch(Exception ex)
            {
                myStateMN.SetNewThongbao(ex.Message);
            }
        }
        public virtual async void V_In()
        {
            DataRowView rview = (DataRowView)SelectedItem;
            if (rview == null)
            {               
                myStateMN.SetNewThongbao(Lap["No data selected"]);
                return;
            }
           
            DataSet DataSource = CopyVoucherToDataSet(SqlTableKeyValue);
            //if (DataSource != null && DataSource.Tables.Count > 0)
            //{
            //    LSData = new List<HDBanleModel>();
            //    HDBanleModel _curModel = new HDBanleModel();                
            //    _curModel.LSPh = myDb.ConvertDataTable<Ph83>(DataSource.Tables[0]);
            //    _curModel.LSCt = myDb.ConvertDataTable<Ct83>(DataSource.Tables[1]);
            //    LSData.Add(_curModel);
            //}
            //ShowReport(LSData, typeof(HDBanleModel));           
        }
        public virtual async void V_Import()
        {
            string sql = "select * from Dmimexct where Upper(ma_ct) = '" + curDmct.Ma_ct.ToUpper().Trim() + "'";
            DataSet ds = await myDb.LoadDataset(sql);
            List<Dmimex> lsdmimex = myDb.ConvertDataTable<Dmimex>(ds.Tables[0]);
            if (lsdmimex != null && lsdmimex.Count > 0)
            {
                CurDmimex = lsdmimex[0];
                if (IsLangEn)
                {
                    CurDmimex.Ten = String.IsNullOrEmpty(CurDmimex.Ten2) ? CurDmimex.Ten : CurDmimex.Ten2;
                }
                myStateMN.SetCurrentActionTask(ActionTask.Import);
                CurDmimex.Overwrite = "0";
                IsImportPopupVisible = true;
            }
            StateHasChanged();
        }
        public async Task ShowReport(IList LSItems, Type Itype)
        {
            int _so_lien = (curDmct.So_lien == null? 1: curDmct.So_lien);
            Dictionary<string, object> Par = new Dictionary<string, object>();
            Par.Add("LSItems", (object)LSItems);
            Par.Add("Menu_id", (object)Menu_id);
            Par.Add("M_so_lien", (object)_so_lien);
            var aty = typeof(ReportVcComponent<>).MakeGenericType(new[] { Itype });
            DxWindowModel model = new DxWindowModel(aty, Par, true, "BC");
            Par.Add("DxWindowmodel", model);
            myModal.Show(model);
        }
        public virtual async void OnLangguachange(MyLang lang)
        {
            if (lang.IsnewLang)
            {
                await Language();
            }
        }
        public virtual async Task Language()
        {
            try
            {
                IsLangEn = false;
                UserLang = await myLcStore.GetItemAsync<string>("userlang");
                if (!string.IsNullOrEmpty(UserLang))
                {
                    if (UserLang.Equals("En"))
                    {
                        IsLangEn = true;
                    }
                }
                StateHasChanged();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public virtual void DeleteVoucher(string _stt_rec)
        {
            string strdele = "DeleteVoucher";
            string strpara = "cMa_ct;stt_rec";
            string strparavalue = Ma_ct + ";" + _stt_rec;
            myDb.LoadStoreProcedure(strdele,strpara,strparavalue);
            foreach(DataTable tbl in DsTrans.Tables)
            {
                foreach (DataRow row in tbl.Select(SqlTableKey + "='" + _stt_rec + "'"))
                    tbl.Rows.Remove(row);
            }
            if(iRow > TblPH.Rows.Count-1)
            {
                iRow = TblPH.Rows.Count - 1;
            }
            if (iRow >= 0)
                CurItem(TblPH.DefaultView[iRow]);
        }
        public virtual DataSet CopyVoucherToDataSet(string _stt_rec)
        {
            DataSet dtsPrt = new DataSet();
            foreach (DataTable tbl in DsTrans.Tables)
            {
                DataTable tblNew = tbl.Clone();
                foreach (DataRow row in tbl.Select(SqlTableKey + "='" + _stt_rec + "'"))
                {
                    tblNew.ImportRow(row);
                }
                dtsPrt.Tables.Add(tblNew);
            }
            return dtsPrt;
        }
        public virtual async void ExportData(int type)
        {
            oBrowsePH.ExPortGrid(type);
            string ToastMessage = Lap["Export successfully"].ToString();
            myStateMN.SetNewThongbao(ToastMessage);
            StateHasChanged();
        }
        public virtual void LoadOptions()
        {
            M_ma_nt0 = myOption.GetOptionsValue("M_MA_NT0");
            M_MA_THUE = myOption.GetOptionsValue("M_MA_THUE");
            M_WS_ID = myOption.GetOptionsValue("M_WS_ID").Trim().ToUpper();
            M_IP_SL = myOption.GetOptionsValue("M_IP_SL");
            M_IP_GIA = myOption.GetOptionsValue("M_IP_GIA");
            M_IP_GIA_NT = myOption.GetOptionsValue("M_IP_GIA_NT");
            M_IP_TIEN = myOption.GetOptionsValue("M_IP_TIEN");
            M_IP_TIEN_NT = myOption.GetOptionsValue("M_IP_TIEN_NT");
            M_IP_TY_GIA = myOption.GetOptionsValue("M_IP_TY_GIA");
            M_IP_TY_GIAF = myOption.GetOptionsValue("M_IP_TY_GIAF");
        }
        public void BtnFilterCancel()
        {
            popFilterVisible = false;
            StateHasChanged();
        }
        [JSInvokable]
        public void OnKeyDown(int keyCode)
        {
            if (keyCode == 13)
            {
                myStateMN.SetCurrentActionTask(ActionTask.View);
                if (!IsShowFrm && !popFilterVisible && !IsImportPopupVisible) V_Xem();
            }
            else if (keyCode == 113)
            {
                myStateMN.SetCurrentActionTask(ActionTask.View);
                if (!IsShowFrm && !popFilterVisible && !IsImportPopupVisible) V_Xem();
            }
            if (keyCode == 114) // F3 có mã ASCII là 114
            {
                myStateMN.SetCurrentActionTask(ActionTask.Edit);
                if (!IsShowFrm && !popFilterVisible && !IsImportPopupVisible) V_Sua();
            }
            if (keyCode == 115)
            {
                myStateMN.SetCurrentActionTask(ActionTask.New);
                if (!IsShowFrm && !popFilterVisible && !IsImportPopupVisible) V_Moi();
            }
            if (keyCode == 118)
            {
                myStateMN.SetCurrentActionTask(ActionTask.Delete);
                if (!IsShowFrm && !popFilterVisible && !IsImportPopupVisible) V_In();
            }
            if (keyCode == 119)
            {
                myStateMN.SetCurrentActionTask(ActionTask.Delete);
                if(!IsShowFrm && !popFilterVisible && !IsImportPopupVisible) V_Xoa();
            }
        }
        public async ValueTask DisposeAsync()
        {
            if (_dotNetHelper is not null)
            {
                try
                {
                    await JavacriptS.InvokeVoidAsync("blazorKeyListener.remove", _dotNetHelper);
                    _dotNetHelper.Dispose();
                }
                catch{ }
            }
        }
        public static int CheckPhanBo(DataRowView drview,IDbManager myDb, string[] Post_store, string[] Check_Valid_Store, string stt_rec)
        {
            if (drview.Row.Table.Columns.Contains("stt_rec_hd") && !string.IsNullOrEmpty(drview["stt_rec_hd"].ToString().Trim()))
                return 0;
            int num = 0;
            if (drview.Row.Table.Columns.Contains("ma_ct"))
            {
                string upper = drview["ma_ct"].ToString().ToUpper();
                string str1 = "PN1;PN9;HD9;PN2;PN6;HD1;HD2;HD6;PC1;BN1;PT1;BC1;PNG;PNA;PNB;PNC;HDM;HDA;HDB;HDX;PXF;PNF;HD5;HDL";
                string str2 = "HD3;HD4;PN9;HD9;PC1;BN1;PT1;BC1;PXF";
                if (upper.Equals("PK1"))
                {
                    string format1 = "exec [dbo].{0} @stt_rec";
                    string strsql = ((Post_store == null || Post_store.Length <= 0) ? string.Format(format1, (object)"InPostCttt20") : string.Format(format1, Post_store[0]));                    
                    strsql += "  = '" + stt_rec + "'";                    
                    int result = (int)myDb.GetValue(strsql);

                    if (result > 0)
                    {
                        string format2 = "exec [dbo].{0} @stt_rec = '{1}', @tablename ='{2}'";
                        string chkVa_str = (Check_Valid_Store == null || Check_Valid_Store.Length <= 1) ? string.Format(format2, (object)"CheckEditVoucher", stt_rec, "cttt20") : string.Format(format2, (object)Check_Valid_Store[1], stt_rec, "cttt20");                        
                        num = (int)myDb.GetValue(chkVa_str);
                    }
                    else
                    {
                        string format2 = "exec [dbo].{0} @stt_rec = '{1}', @tablename ='{2}'";
                        string chkVa_str = (Check_Valid_Store == null || Check_Valid_Store.Length <= 2) ? string.Format(format2, (object)"CheckEditVouchertt", stt_rec, "cttt20") : string.Format(format2, (object)Check_Valid_Store[2], stt_rec, "cttt20");
                        num = (int)myDb.GetValue(chkVa_str);
                    }

                    if (num == 0)
                    {
                        string format2 = "exec [dbo].{0} @stt_rec = '{1}'";
                        string chkVa_str = (Post_store == null || Post_store.Length <= 1) ? string.Format(format2, (object)"InPostCttt30", stt_rec) : string.Format(format2, (object)Post_store[1], stt_rec);                        
                        if ((int)myDb.GetValue(chkVa_str) > 0)
                        {
                            string format3 = "exec [dbo].{0} @stt_rec = '{1}', @tablename ='{2}'";
                            string chk_str = (Check_Valid_Store == null || Check_Valid_Store.Length <= 1) ? string.Format(format3, (object)"CheckEditVoucher", stt_rec, "cttt30") : string.Format(format3, (object)Check_Valid_Store[1], stt_rec, "cttt30");                           
                            num = (int)myDb.GetValue(chk_str);
                        }
                        else
                        {
                            string format3 = "exec [dbo].{0} @stt_rec = '{1}', @tablename ='{2}'";
                            string chk_str = (Check_Valid_Store == null || Check_Valid_Store.Length <= 2) ? string.Format(format3, (object)"CheckEditVouchertt", stt_rec, "cttt30") : string.Format(format3, (object)Check_Valid_Store[2], stt_rec, "cttt30");
                            num = (int)myDb.GetValue(chk_str);
                        }
                    }
                }
                else
                {
                    if (str1.Contains(upper))
                    {
                        bool flag = true;
                        if (upper.Equals("HD6") || upper.Equals("PN6"))
                        {
                            if (drview.Row.Table.Columns.Contains("ma_gd") && drview["ma_gd"].ToString().Equals("2"))
                                flag = false;
                        }
                        else if (upper.Equals("PC1") || upper.Equals("BN1"))
                        {
                            if (drview.Row.Table.Columns.Contains("ma_gd") && !drview["ma_gd"].ToString().Equals("4"))
                                flag = false;
                        }
                        else if ((upper.Equals("PT1") || upper.Equals("BC1")) && (drview.Row.Table.Columns.Contains("ma_gd") && !drview["ma_gd"].ToString().Equals("4")))
                            flag = false;
                        if (flag)
                        {
                            string format = "exec [dbo].{0} @stt_rec = '{1}'";
                            string chvl_str = (Check_Valid_Store == null || Check_Valid_Store.Length <= 1) ? string.Format(format, (object)"CheckEditVoucher", stt_rec) : string.Format(format, (object)Check_Valid_Store[1], stt_rec);
                            num = (int)myDb.GetValue(chvl_str);
                        }
                    }
                    if (str2.Contains(upper) && num == 0)
                    {
                        bool flag = true;
                        if (upper.Equals("PC1") || upper.Equals("BN1"))
                        {
                            if (drview.Row.Table.Columns.Contains("ma_gd") && (drview["ma_gd"].ToString().Equals("1") || drview["ma_gd"].ToString().Equals("4")))
                                flag = false;
                        }
                        else if ((upper.Equals("PT1") || upper.Equals("BC1")) && drview.Row.Table.Columns.Contains("ma_gd") && (drview["ma_gd"].ToString().Equals("1") || drview["ma_gd"].ToString().Equals("4")))
                            flag = false;
                        if (flag)
                        {
                            string format = "exec [dbo].{0} @stt_rec = '{1}'";
                            string chvl_str = (Check_Valid_Store == null || Check_Valid_Store.Length <= 2) ? string.Format(format, (object)"CheckEditVouchertt", stt_rec) : string.Format(format, (object)Check_Valid_Store[2], stt_rec);
                            num = (int)myDb.GetValue(chvl_str);
                        }
                    }
                }
            }
            return num;
        }
        public static async void ViewCT(string pStt_rec, string pMa_ct, Statemanagerment myStateMN, IDbManager myDb, ISisTypeManager mySType, ILocalStorageService Lcstore, IDxWindowService Modal, XmlStringLocalizer Lap)
        {            
            string _stt_rec = pStt_rec.Trim();
            string _ma_ct = pMa_ct.Trim();
            if (String.IsNullOrEmpty(_ma_ct))
            {
                myStateMN.SetNewThongbao(Lap["No Command"] + " !");
                return;
            }
            ICommandManager comm = myStateMN.GetCommand();
            Command _command = comm.GetMenuByMact(_ma_ct);
            if (_command == null)
            {
            myStateMN.SetNewThongbao(Lap["No Command"] + " !");
                return;
            }

            string _form = myDb.GetValue("dmct", "[form]", "ma_ct = '" + _ma_ct.ToString().Trim() + "'").ToString().Trim();
            string pmenu_id = _command.Menu_id.Trim();
            if (!String.IsNullOrEmpty(pmenu_id) && !String.IsNullOrEmpty(_command.Mobile_page) && !String.IsNullOrEmpty(_form))
            {

                SisType sTypw = mySType.GetValue(_command.Mobile_page.ToLower().Trim());
                if (sTypw != null)
                {
                    Users curuser = await Lcstore.GetItemAsync<Users>("user");
                    if (curuser == null)
                    {
                    myStateMN.SetNewThongbao(Lap["No User, Please log back in"] + " !");
                        return;
                    }
                    if (!curuser.CheckRight(pmenu_id, "EDIT") && !curuser.CheckRight(pmenu_id, "READ"))
                    {
                    myStateMN.SetNewThongbao(Lap["You don't have permission"] + " !");
                        return;
                    }
                    ActionTask pActionTask = ActionTask.View;                   
                    Dictionary<string, object> Par = new Dictionary<string, object>();
                    Par.Add("Stt_rec", (object)_stt_rec);
                    Par.Add("Ma_ct", (object)_ma_ct);
                    Par.Add("Menu_id", (object)pmenu_id);
                    Par.Add("curActionTask", (object)pActionTask);

                    string tname = "frm" + _form.ToLower().Trim();
                    Type[] types = sTypw.Type.Assembly.GetTypes().Where(t => t.Namespace == sTypw.Type.Namespace && t.Name.ToLower() == tname.ToLower()).ToArray();
                    Type HDtype = types.FirstOrDefault();
                    if (HDtype == null)
                    {
                        myStateMN.SetNewThongbao(@Lap["No Command"] + " !");
                        return;
                    }
                    DxWindowModel model = new DxWindowModel(HDtype, Par, true, "CT");
                    Par.Add("DxWindowmodel", model);
                    Modal.Show(model);
                }
            }
        }
        public virtual async void ImportOK()
        {
            Console.WriteLine("ok");
            await Loaddata();
            StateHasChanged();
        }
        public void HandleFreeCodeErrors(DataTable table, string columnName)
        {
            string displayColumn = columnName switch
            {
                "ma_td_i" => IsLangEn ? "ma_td_i:H= Free code 1" : "ma_td_i:H=Mã td 1",
                "ma_td2_i" => IsLangEn ? "ma_td2_i:H= Free code 2" : "ma_td2_i:H=Mã td 2",
                "ma_td3_i" => IsLangEn ? "ma_td3_i:H= Free code 3" : "ma_td3_i:H=Mã td 3",
                _ => ""
            };

            string message = columnName switch
            {
                "ma_td_i" => IsLangEn ? "List of free code 1 is not valid" : "Danh sách mã td 1 không có trong danh mục mã tự do 1",
                "ma_td2_i" => IsLangEn ? "List of free code 2 is not valid" : "Danh sách mã td 2 không có trong danh mục mã tự do 2",
                "ma_td3_i" => IsLangEn ? "List of free code 3 is not valid" : "Danh sách mã td 3 không có trong danh mục mã tự do 3",
                _ => ""
            };

            DisplayErrorData(table, displayColumn, message);
        }
        public void DisplayErrorData(DataTable table, string fields, string title)
        {
            SisLib.ReportLib.BrowTable(myModal, table, fields, " : " + title, Lap["Cancel"]);
        }
        public virtual async void Dispose()
        {
            if (_dotNetHelper is not null)
            {
                try
                {
                    await JavacriptS.InvokeVoidAsync("blazorKeyListener.remove", _dotNetHelper);
                    _dotNetHelper.Dispose();
                }
                catch { }
            }
            myStateMN.OnActionTaskChanged -= OnActionTaskchanged;
            myStateMN.OnLangguaChanged -= OnLangguachange;
            myStateMN.OnDvcsChanged -= OnDvcsChanged;
        }
    }
}
