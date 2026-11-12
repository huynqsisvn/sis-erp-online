using DevExpress.Blazor;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using SisCom.Shared;
using SisData.Model;
using SisData.Service;
using SisFunc;
using SisLib.Lib;
using SisPo.Pages.Input.Apctpn1;
using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;

namespace SisPo.Pages.Input.Poctpnb
{
    public partial class FrmPoctpnb : TranBaseComponent
    {
        #region Properties       
        private Ph72 CurPh { get; set; }
        private Ct72 CurCt { get; set; }
        private Ct72gt CurCtgt { get; set; }
        
        private List<Ct72> LSCt { get; set; }
        private List<Ph72> LSPh { get; set; }
        private List<Ct72gt> LSCtgt { get; set; }
        private List<PNBModel> LSData { get; set; }
        
        private Ph72 LastPh { get; set; }
        
        public DataTable TblDmkh { get; set; }
        public DataTable TblDmtk { get; set; }
        public DataTable TblDmmagd { get; set; }
        
        private DxGrid GridPh;
        private int activeTabIndex = 0;
        
        private string end_GridCt = "end_input_GridCt0";
        private string filter_status = "ma_ct like '%PNB%' and status = 1";
        private string printPage = "hdlprint";
        private string mPage { get; set; }
        
        public DotNetObjectReference<FrmPoctpnb> Pageobj;
        
        private string _filter_qs = "";
        string _filter_pt = "";
        string _filter_gd = "1=0";

        public bool SD_THUE_DB = false;
        private string M_QL_LO_CK = "0";
        
        private Ct72 CurrentEditModel;
        private Ct72gt CurrentEditModelctgt;
        private Ct72 CurrentEditModeltax;

        public bool ParentReload = false;
        public bool AllowTaoPC = false;
        
        private bool isBlockingCTGT = false;
        private bool isBlockingCT = false;
        private bool isBlockingTAX = false;

        public ElementReference GridWrapper;
        public ElementReference GridGTWrapper;
        public ElementReference GridTAXWrapper;
        public IGrid GridFE { get; set; }
        public IGrid GridTAX { get; set; }        
        public override object SelectedDataItemCT
        {
            get => selectedDataItemCT;
            set
            {
                if (isBlockingCT) return;
                if (selectedDataItemCT != null && string.IsNullOrWhiteSpace(((Ct72)selectedDataItemCT).Tk_vt))
                {
                    isBlockingCT = true;
                    myStateMN.SetNewThongbao(Lap["Code cannot be empty"] + "!");
                    _ = InvokeAsync(async () =>
                    {
                        if (GridCT.IsEditing()) 
                        {
                            await GridCT.SaveChangesAsync();
                        }
                        await Task.Yield();
                        await GridCT.StartEditDataItemAsync(LSCt.IndexOf((Ct72)selectedDataItemCT), "Tk_vt");
                        isBlockingCT = false;
                    });

                    return;
                }
                selectedDataItemCT = value;
            }
        }
        public object selectedDataItemTAX;
        public virtual object SelectedDataItemTAX
        {
            get => selectedDataItemTAX;
            set
            {
                selectedDataItemTAX = value;
            }
        }

        private bool IsShowDialog = false;
        private TaskCompletionSource<bool> dialogTcs;
        private bool confirmResult = false;
        public bool VisiblePC = false;
        public int Sua_tien { get; set; }
        #endregion
        
        #region Initialization
        protected async override Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            
            CurPh = new Ph72();
            CurCt = new Ct72();
            CurCtgt = new Ct72gt();
            LSCt = new List<Ct72>();
            LSPh = new List<Ph72>();
            LSCtgt = new List<Ct72gt>();
            LastPh = new Ph72();
            
            _filter_qs = String.Format("ma_cts = '' OR ma_cts like '%{0}%'", Ma_ct);
            
            LoadQuyenso();
            LoadOptions();
            LoadLookups();
            
            M_QL_LO_CK = myOption.GetOptionsValue("M_QL_LO_CK")?.Trim() ?? "0";
            SD_THUE_DB = myOption.GetOptionsValue("M_SD_THUE_DB")?.Trim() == "1";
            
            ParentReload = false;
            
            switch (curActionTask)
            {
                case ActionTask.View:
                    Pagetitle = Lap["View"] + ": " + (IsLangEn ? curDmct.Ten_ct2 : curDmct.Ten_ct);
                    V_Xem();
                    break;
                case ActionTask.Edit:
                    Pagetitle = Lap["Edit"] + ": " + (IsLangEn ? curDmct.Ten_ct2?.Trim() : curDmct.Ten_ct?.Trim());
                    V_Sua();
                    break;
                case ActionTask.New:
                    Pagetitle = Lap["New"] + ": " + (IsLangEn ? curDmct.Ten_ct2 : curDmct.Ten_ct);
                    V_Moi();
                    break;
                default:
                    Pagetitle = Lap["View"] + ": " + (IsLangEn ? curDmct.Ten_ct2 : curDmct.Ten_ct);
                    V_Xem();
                    break;
            }
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                UserLang = await myLcStore.GetItemAsync<string>("userlang");
                await Lap.LoadLocalizationAsync(String.Format("{0}-{1}",curCommand.Ma_phan_he.Trim().ToLower(), UserLang.Trim()));
                Pageobj = DotNetObjectReference.Create(this);
                await JavacriptS.InvokeVoidAsync("SetPageObj", Pageobj);
                await JavacriptS.InvokeVoidAsync("AttachEnterAsTab");
                await JavacriptS.InvokeVoidAsync("eval", "document.getElementById('ma_khach_input')?.focus()");

            }
            await JavacriptS.InvokeVoidAsync("changeTextNewRowButton", Lap["Click here to add a new row"].ToString().Trim());
        }

        private void LoadLookups()
        {
            try
            {
                TblDmtk = myDb.LoadDataSet("select tk,ten_tk,loai_cl_no from dmtk where loai_tk=1").Tables[0];
                TblDmkh = myDb.LoadDataSet("select ma_kh,ten_kh,dia_chi,ma_so_thue,tk,doi_tac,nh_kh3,tk_nh,ma_thck from dmkh where status <> 0 order by ma_kh").Tables[0];
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        #endregion
        
        #region Form Actions
        public override void V_Moi()
        {
            string _ma_dvcs = myStateMN.GetMa_Dvcs();
            if (String.IsNullOrWhiteSpace(_ma_dvcs) || _ma_dvcs.ToUpper().Trim() == "ALL")
            {
                myStateMN.SetNewThongbao(Lap["Please select a unit!"]);
                return;
            }

            try
            {
                AllowEditMode = true;
                curActionTask = ActionTask.New; 
                
                CurPh.Stt_rec = GetStt_rec();
                Stt_rec = CurPh.Stt_rec;
                LoadVoucherData(Stt_rec);
                CurPh.Ws_id = M_WS_ID;
                CurPh.Ma_ct = Ma_ct;
                CurPh.Ngay_ct = DateTime.Now.Date;
                CurPh.Ngay_lct = DateTime.Now.Date;
                CurPh.Ma_qs = GetDMQS(Ma_ct, CurPh.Ngay_ct, curUser.User_id);
                CurPh.Ma_nt = M_ma_nt0;
                CurPh.Ty_gia = 1;
                CurPh.Status = curDmct.Ma_post;
                CurPh.Loai_pb = "1";
                CurPh.Ma_dvcs = Ma_dvcs;
                CurPh.User_id = curUser.User_id.ToString();
                
                NotMaNt0Voucher = !CurPh.Ma_nt?.Trim().Equals(M_ma_nt0?.Trim()) ?? false;
                
                StateHasChanged();                
            }
            catch (Exception ex)
            {
                myStateMN.SetNewThongbao($"Lỗi thêm mới: {ex.Message}");
            }
        }
        
        public override void V_Sua()
        {
            string _ma_dvcs = myStateMN.GetMa_Dvcs();
            if (String.IsNullOrWhiteSpace(_ma_dvcs) || _ma_dvcs.ToUpper().Trim() == "ALL")
            {
                myStateMN.SetNewThongbao(Lap["Please select a unit!"]);
                return;
            }

            try
            {
                AllowEditMode = true;
                curActionTask = ActionTask.Edit;
                
                if (!string.IsNullOrEmpty(Stt_rec))
                {
                    LoadVoucherData(Stt_rec);

                    if (!SisData.Lib.CheckValidNgayKs(mySysvar, myDb, CurPh.Ngay_ct, Ma_ct))
                    {
                        AllowEditMode = false;
                        curActionTask = ActionTask.View;
                        myStateMN.SetNewThongbao(Lap["Data is locked, cannot be edited!"]);
                        return;
                    }

                    string str2 = myOption.GetOptionsValue("M_TK_TK_VT");
                    if (!string.IsNullOrEmpty(CurPh.Ma_nx))
                    {
                        foreach (string str3 in str2.Split(','))
                        {
                            if (!string.IsNullOrWhiteSpace(str3.ToString()) && CurPh.Ma_nx.Trim().StartsWith(str3.ToString().Trim()))
                            {
                                AllowTaoPC = true;
                                break;
                            }
                            else
                            {
                                AllowTaoPC = false;
                                CurPh.Tao_pc = 0;
                            }
                        }
                    }
                }
                
                StateHasChanged();
            }
            catch (Exception ex)
            {
                myStateMN.SetNewThongbao($"Lỗi V_Sua: {ex.Message}");
            }
        }
        
        public override void V_Xem()
        {
            try
            {
                AllowEditMode = false;
                curActionTask = ActionTask.View;
                
                if (!string.IsNullOrEmpty(Stt_rec))
                {
                    LoadVoucherData(Stt_rec);
                }
                
                StateHasChanged();
            }
            catch (Exception ex)
            {
                myStateMN.SetNewThongbao($"Lỗi V_Xem: {ex.Message}");
            }
        }
        
        private void LoadVoucherData(string sttRec)
        {
            try
            {
                string phFilter = $"stt_rec = ''{sttRec}''";
                string strsql = $"Exec {curDmct.Process_Store.Trim().Split('|')[0].Trim()} @ma_ct='{Ma_ct}',@sPhFilter ='{phFilter}',@sCtFilter='1=1',@sGtFilter='1=1'";
                DsTrans = myDb.LoadDataSet(strsql);
                
                if (DsTrans?.Tables.Count > 0 && DsTrans.Tables[0].Rows.Count > 0)
                {
                    CurPh = myDb.ConvertDataRow<Ph72>(DsTrans.Tables[0].Rows[0]);
                    NotMaNt0Voucher = !CurPh.Ma_nt?.Trim().Equals(M_ma_nt0?.Trim()) ?? false;
                }
                
                if (DsTrans?.Tables.Count > 1)
                {
                    LSCt = myDb.ConvertDataTable<Ct72>(DsTrans.Tables[1]);
                }
                
                if (DsTrans?.Tables.Count > 2)
                {
                    LSCtgt = myDb.ConvertDataTable<Ct72gt>(DsTrans.Tables[2]);
                }
                
                AllowTaoPC = !string.IsNullOrEmpty(CurPh.Stt_rec_pc);
            }
            catch (Exception ex)
            {
                myStateMN.SetNewThongbao($"Lỗi LoadVoucherData: {ex.Message}");
            }
        }
        #endregion
        
        #region Event Handlers
        private async Task OnNew()
        {
            V_Moi();
            StateHasChanged();
            await Task.Delay(100);
            await JavacriptS.InvokeVoidAsync("eval", "document.getElementById('ma_khach_input')?.focus()");
        }
        
        private async Task OnCopy()
        {
            string _ma_dvcs = myStateMN.GetMa_Dvcs();
            if (String.IsNullOrWhiteSpace(_ma_dvcs) || _ma_dvcs.ToUpper().Trim() == "ALL")
            {
                myStateMN.SetNewThongbao(Lap["Please select a unit!"]);
                return;
            }

            if (!string.IsNullOrEmpty(Stt_rec))
            {
                LoadVoucherData(Stt_rec);
                CurPh.Stt_rec = GetStt_rec();
                CurPh.So_ct = "";
                foreach (var ct in LSCt)
                {
                    ct.Stt_rec = CurPh.Stt_rec;
                    ct.So_ct = "";
                }
                foreach (var ct in LSCtgt)
                {
                    ct.Stt_rec = CurPh.Stt_rec;
                    ct.So_ct = "";
                }
                AllowEditMode = true;
                curActionTask = ActionTask.New;
            }
            StateHasChanged();
            await Task.Delay(100);
            await JavacriptS.InvokeVoidAsync("eval", "document.getElementById('ma_khach_input')?.focus()");
        }
        
        private async Task OnEdit()
        {
            V_Sua();
        }
        
        private async Task OnSave()
        {
            try
            {
                if (IsSaving) return;
                IsSaving = true;
                
                if (!ValidateVoucher())
                {
                    IsSaving = false;
                    return;
                }
                
                bool result = await SaveVoucher();
                
                if (result)
                {
                    myStateMN.SetNewThongbao(Lap["Saved successfully"]);
                    IsSaved = true;
                    ParentReload = true;
                    
                    if (curActionTask == ActionTask.New || curActionTask == ActionTask.Copy || curActionTask == ActionTask.Edit)
                    {
                        AllowEditMode = false;
                        curActionTask = ActionTask.View;
                    }
                }
                else
                {
                    //myStateMN.SetNewThongbao(Lap["Save failed"]);
                }
                
                IsSaving = false;
                StateHasChanged();
            }
            catch (Exception ex)
            {
                IsSaving = false;
                myStateMN.SetNewThongbao($"Lỗi OnSave: {ex.Message}");
            }
        }
        
        private async Task OnCancel()
        {
            if (Closeform.HasDelegate)
            {
                await Closeform.InvokeAsync(ParentReload);
            }
            else
            {
                myNavi.NavigateTo($"/list/{Menu_id}");
            }
        }
        
        private async Task OnPrint()
        {
            try
            {
                IsShowVoucher = false;
                
                LSData = new List<PNBModel>();
                var pnbModel = new PNBModel
                {
                    LSPh = new List<Ph72> { CurPh },
                    LSCt = LSCt,
                    LSCtgt = LSCtgt
                };

                string tk_no = "";
                var uniqueTk = new HashSet<string>();
                for (int i = 0; i < pnbModel.LSCt.Count; i++)
                {
                    string ma = pnbModel.LSCt[i].Tk_vt?.Trim();
                    if (string.IsNullOrEmpty(ma)) continue;

                    if (uniqueTk.Add(ma))
                    {
                        tk_no += ma + "\n";
                    }
                    pnbModel.LSCt[i].Ma_nx = pnbModel.LSPh[0].Ma_nx;
                    pnbModel.LSCt[i].Stt = i + 1;
                }
                pnbModel.Ma_so_thue_cty = myOption.GetOptionsValue("M_MA_THUE").Trim();
                pnbModel.TotalString = SisFunc.DocSo.Doc_so(pnbModel.LSPh[0].T_tien0, myOption, myDb, IsLangEn);
                pnbModel.TotalTTString = SisFunc.DocSo.Doc_so(pnbModel.LSPh[0].T_tt, myOption, myDb, IsLangEn);
                pnbModel.TotalStringNT = SisFunc.DocSo.Doc_so(pnbModel.LSPh[0].T_tien_nt0, myOption, myDb, IsLangEn);
                pnbModel.TotalTTStringNT = SisFunc.DocSo.Doc_so(pnbModel.LSPh[0].T_tt_nt, myOption, myDb, IsLangEn);
                pnbModel.Ten_lien = "Liên 1 : giao khách hàng";
                pnbModel.So_lien = curDmct.So_lien;
                pnbModel.Ma_kho = pnbModel.LSCt.Max(x => x.Ma_kho_i);
                pnbModel.Ma_kho = pnbModel.Ma_kho.Trim() + " - " + myDb.GetValue("dmkho", IsLangEn ? "ten_kho2" : "ten_kho", "ma_kho='" + pnbModel.Ma_kho.Trim() + "'").ToString().Trim();
                pnbModel.NgayString = ToDateString(pnbModel.LSPh[0].Ngay_ct);
                pnbModel.TknoString = tk_no;
                LSData.Add(pnbModel);
                
                StateHasChanged();
            }
            catch (Exception ex)
            {
                myStateMN.SetNewThongbao($"{ex.Message}");
            }
        }
        
        private async Task OnPrintBack()
        {
            IsShowVoucher = true;
            StateHasChanged();
        }
        public override void OnTemplateChanged(int newSttTemplate)
        {
            Stt_mau_temlate = newSttTemplate;
            SD_THUE_DB = (GetOptionTemplate(Stt_mau_temlate, "M_SD_THUE_DB") == 1);
        }
        #endregion

        #region Grid Events
        private void OnTabChanged(TabClickEventArgs e)
        {
            if (GridPh == null && LSCt.Count > 0 && LSPh.Count == 0 && activeTabIndex == 0)
            {
                LSPh.Add(CurPh);
            }
            if (activeTabIndex == 2 && String.IsNullOrWhiteSpace(CurPh.Tk_i))
            {
                CurPh.Tk_i = CurPh.Ma_nx;
            }
        }
        
        private async void OnGridCTLostFocus(FocusEventArgs e)
        {
            bool stillInside = await JavacriptS.InvokeAsync<bool>("focusInside", "grid-wrapper");
            if (stillInside) return;
            
            if (GridCT.IsEditing() && activeTabIndex == 0)
                await GridCT.SaveChangesAsync();
        }
        private async void OnGridTAXLostFocus(FocusEventArgs e)
        {
            bool stillInside = await JavacriptS.InvokeAsync<bool>("focusInside", "gridtax-wrapper");
            if (stillInside) return;

            if (GridTAX.IsEditing())
                await GridTAX.SaveChangesAsync();
        }

        private async void OnGridCTGTLostFocus(FocusEventArgs e)
        {
            bool stillInside = await JavacriptS.InvokeAsync<bool>("focusInside", "gridgt-wrapper");
            if (stillInside) return;
            
            if (GridCTGT != null && GridCTGT.IsEditing() && activeTabIndex == 3)
                await GridCTGT.SaveChangesAsync();
        }
        
        private async Task GridCT_OnKeyDown(KeyboardEventArgs e)
        {
            if (!AllowEditMode)
                return;

            switch (e.Key)
            {
                case "Enter":
                    if (activeTabIndex == 0)
                    {
                        await GridCT.SaveChangesAsync();
                    }
                    else if (activeTabIndex == 1)
                    {
                        // Tab Import tax - không cho thêm dòng mới
                        await GridTAX.SaveChangesAsync();
                        // Di chuyển đến dòng tiếp theo (nếu có) - không thêm dòng mới
                        Ct72 current = (Ct72)SelectedDataItemTAX;
                        if (current != null)
                        {
                            var index = LSCt.IndexOf(current);
                            if (index >= 0 && index < LSCt.Count - 1)
                            {
                                await GridTAX.StartEditDataItemAsync(LSCt[index + 1]);
                            }
                        }
                        return; // Thoát sớm để không gọi SaveAndMoveNextRow
                    }
                    else if (activeTabIndex == 2)
                    {
                        // Tab Charges (GridFE) - không cho thêm dòng mới
                        await GridFE.SaveChangesAsync();
                        // Di chuyển đến dòng tiếp theo (nếu có) - không thêm dòng mới
                        Ct72 current = (Ct72)SelectedDataItemCT;
                        if (current != null)
                        {
                            var index = LSCt.IndexOf(current);
                            if (index >= 0 && index < LSCt.Count - 1)
                            {
                                await GridFE.StartEditDataItemAsync(LSCt[index + 1]);
                            }
                        }
                        return; // Thoát sớm để không gọi SaveAndMoveNextRow
                    }
                    else if (activeTabIndex == 3)
                    {
                        await GridCTGT.SaveChangesAsync();
                    }
                    await SaveAndMoveNextRow();
                    break;

                case "F4":
                    // Add new row - CHỈ cho phép ở tab Details (0) và Tax (3)
                    if (activeTabIndex == 0)
                    {
                        if (GridCT.IsEditing())
                        {
                            await GridCT.SaveChangesAsync();
                        }
                        await Task.Delay(50);
                        await AddNewRowCT();
                    }
                    else if (activeTabIndex == 3)
                    {
                        if (GridCTGT.IsEditing())
                        {
                            await GridCTGT.SaveChangesAsync();
                        }
                        await Task.Delay(50);
                        await AddNewCTGT();
                    }
                    // Tab Import tax (1) và Charges (2) - KHÔNG cho phép thêm dòng mới
                    break;

                case "F8":
                    // Delete selected row
                    if (SelectedDataItemCT != null)
                    {
                        // GridCT.ShowRowDeleteConfirmation(SelectedDataItemCT);
                    }
                    else
                    {
                        Message = Lap["No data selected"];
                        StateHasChanged();
                    }
                    break;

                case "Escape":
                    // Cancel current edit
                    try
                    {
                        await GridCT.CancelEditAsync();
                        // if user cancels a newly added empty row, remove it
                        if (LSCt.Count > 0 && CurrentEditModel != null && string.IsNullOrEmpty(CurrentEditModel.Ma_vt))
                            LSCt.Remove(CurrentEditModel);
                        StateHasChanged();
                    }
                    catch { }
                    break;
            }
        }
        
        private async Task AddNewRowCT()
        {
            try
            {
                if (GridCT.IsEditing())
                {
                    await GridCT.SaveChangesAsync();
                }
                
                await Task.Delay(50);
                await GridCT.StartEditNewRowAsync();
                
                await Task.Delay(50);
                
                var newRow = LSCt.LastOrDefault();
                if (newRow != null)
                {
                    string focusId = $"Ma_vt_{newRow.Stt_rec0}";
                    await JavacriptS.InvokeVoidAsync("eval", $"setTimeout(() => {{ var el = document.getElementById('{focusId}'); if(el) {{ var input = el.querySelector('input'); if(input) input.focus(); }} }}, 100);");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"AddNewRowCT Error: {ex.Message}");
            }
        }
        
        private async Task OnDeleteRowCT(int rowIndex, int tabIndex)
        {
            if (rowIndex >= 0 && rowIndex < LSCt.Count)
            {
                LSCt.RemoveAt(rowIndex);
                TinhTong();
                StateHasChanged();
            }
        }
        
        private async Task StartEditDataItemByClickCT(GridRowClickEventArgs e)
        {
            if (GridCT.IsEditing())
                await GridCT.SaveChangesAsync();
            if (AllowEditMode)
            {
                await GridCT.StartEditRowAsync(e.VisibleIndex);
            }
        }
        private async Task StartEditDataItemByClickCP(GridRowClickEventArgs e)
        {
            if (GridFE.IsEditing())
                await GridFE.SaveChangesAsync();
            if (AllowEditMode)
            {
                await GridFE.StartEditRowAsync(e.VisibleIndex);
            }
        }
        [JSInvokable]
        public async Task SaveAndMoveNextRow()
        {
            if (activeTabIndex == 1)
            {
                // Tab Import tax - KHÔNG cho thêm dòng mới
                await GridTAX.SaveChangesAsync();
                Ct72 current = (Ct72)SelectedDataItemTAX;
                if (current != null)
                {
                    var index = LSCt.IndexOf(current);
                    // Chỉ di chuyển đến dòng tiếp theo nếu có, KHÔNG thêm dòng mới
                    if (index >= 0 && index < LSCt.Count - 1)
                    {
                        await GridTAX.StartEditDataItemAsync(LSCt[index + 1]);
                    }
                }
            }
            else if (activeTabIndex == 2)
            {
                // Tab Charges (GridFE) - KHÔNG cho thêm dòng mới
                await GridFE.SaveChangesAsync();
                Ct72 current = (Ct72)SelectedDataItemCT;
                if (current != null)
                {
                    var index = LSCt.IndexOf(current);
                    // Chỉ di chuyển đến dòng tiếp theo nếu có, KHÔNG thêm dòng mới
                    if (index >= 0 && index < LSCt.Count - 1)
                    {
                        await GridFE.StartEditDataItemAsync(LSCt[index + 1]);
                    }
                }
            }
            else if (activeTabIndex == 3)
            {
                // Tab Tax
                await GridCTGT.SaveChangesAsync();
                Ct72gt current = (Ct72gt)SelectedDataItemCTGT;
                var index = LSCtgt.IndexOf(current);
                if (index >= 0 && index < LSCtgt.Count - 1)
                {
                    await GridCTGT.StartEditDataItemAsync(LSCtgt[index + 1]);
                }
                else if (index == LSCtgt.Count - 1)
                {
                    // Nếu là dòng cuối cùng, thêm dòng mới
                    await AddNewCTGT();
                }
            }
            else
            {
                // Tab Details (activeTabIndex == 0)
                await GridCT.SaveChangesAsync();
                Ct72 current = (Ct72)SelectedDataItemCT;
                var index = LSCt.IndexOf(current);
                if (index >= 0 && index < LSCt.Count - 1)
                {
                    await GridCT.StartEditDataItemAsync(LSCt[index + 1]);
                }
                else if (index == LSCt.Count - 1)
                {
                    // Nếu là dòng cuối cùng, thêm dòng mới
                    await AddNewRowCT();
                }
            }
        }

        [JSInvokable]
        public async Task CallGridEditMode(string value)
        {
            if (!string.IsNullOrEmpty(value) && AllowEditMode)
            {
                if (value.Equals(end_GridCt) && activeTabIndex == 0)
                {
                    await AddNewRowCT();
                }
                else if (activeTabIndex == 3)
                {
                    await AddNewCTGT();
                }
            }
        }
        async Task StartEditDataItemByClickCTGT()
        {
            if (GridCTGT.IsEditing())
                await GridCTGT.SaveChangesAsync();
            if (SelectedDataItemCTGT != null)
            {
                await GridCTGT.StartEditDataItemAsync(SelectedDataItemCTGT);
            }
        }
        async Task StartEditDataItemByClickTAX(GridRowClickEventArgs e)
        {
            if (GridTAX.IsEditing())
                await GridTAX.SaveChangesAsync();
            if (AllowEditMode)
            {
                GridTAX.StartEditRowAsync(e.VisibleIndex);
            }
        }

        private void Grid_CustomizeEditModelCT(GridCustomizeEditModelEventArgs e)
        {
            if (e.IsNew)
            {
                var newCt = (Ct72)e.EditModel;
                newCt.Stt_rec = CurPh.Stt_rec;
                newCt.Stt = LSCt.Count > 0 ? LSCt.Max(a => a.Stt) + 1 : 1;
                newCt.Stt = LSCt.Count + 1 < newCt.Stt ? newCt.Stt : LSCt.Count + 1;
                newCt.Stt_rec0 = string.Format("{0:0000}", newCt.Stt);
                newCt.Ma_ct = Ma_ct;
                newCt.Ngay_ct = CurPh.Ngay_ct;
                newCt.So_ct = CurPh.So_ct;
                newCt.He_so1 = 1;
                
                _ = InvokeAsync(async () =>
                {
                    await Task.Delay(100);
                    string focusId = $"Ma_vt_{newCt.Stt_rec0}";
                    await JavacriptS.InvokeVoidAsync("eval", $"var el = document.getElementById('{focusId}'); if(el) {{ var input = el.querySelector('input'); if(input) {{ input.focus(); input.select(); }} }}");
                });
            }
            
            CurrentEditModel = (Ct72)e.EditModel;
        }
        private void Grid_CustomizeEditModelCP(GridCustomizeEditModelEventArgs e)
        {           
            CurrentEditModel = (Ct72)e.EditModel;
        }

        private async Task Grid_EditModelSavingCT(GridEditModelSavingEventArgs e)
        {
            var ct = (Ct72)e.EditModel;
            
            if (e.IsNew)
            {
                LSCt.Add(ct);
            }
            else
            {
                var index = LSCt.FindIndex(x => x.Stt_rec0 == ct.Stt_rec0);
                if (index >= 0)
                {
                    LSCt[index] = ct;
                }
            }
            
            TinhTong();
            await Task.CompletedTask;
        }
        private async Task Grid_EditModelSavingCP(GridEditModelSavingEventArgs e)
        {
            var ct = (Ct72)e.EditModel;

            if (e.IsNew)
            {
                
            }
            else
            {
                var index = LSCt.FindIndex(x => x.Stt_rec0 == ct.Stt_rec0);
                if (index >= 0)
                {
                    LSCt[index] = ct;
                }
            }

            TinhTong();
            await Task.CompletedTask;
        }

        async Task AddNewCTGT()
        {
            try
            {
                if (GridCTGT.IsEditing())
                {
                    await GridCTGT.SaveChangesAsync();
                }

                await Task.Delay(50);
                await GridCTGT.StartEditNewRowAsync();

                await Task.Delay(50);

                var newRow = LSCtgt.LastOrDefault();
                if (newRow != null)
                {
                    await JavacriptS.InvokeVoidAsync("eval", "setTimeout(() => { var inputs = document.querySelectorAll('[id^=\"Ma_ms\"]'); if(inputs.length > 0) { var lastInput = inputs[inputs.length - 1].querySelector('input'); if(lastInput) lastInput.focus(); } }, 100);");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"AddNewCTGT Error: {ex.Message}");
            }
        }
        void Grid_CustomizeEditModelCTGT(GridCustomizeEditModelEventArgs e)
        {
            CurrentEditModelctgt = (Ct72gt)e.EditModel;
            if (e.IsNew)
            {
                CurrentEditModelctgt.Stt = LSCtgt.Count > 0 ? LSCtgt.Max(a => a.Stt) + 1 : 1;
                CurrentEditModelctgt.Stt = LSCtgt.Count + 1 < CurrentEditModelctgt.Stt ? CurrentEditModelctgt.Stt : LSCtgt.Count + 1;
                CurrentEditModelctgt.Stt_rec0 = string.Format("{0:0000}", CurrentEditModelctgt.Stt);
                CurrentEditModelctgt.Ma_ms = myDb.GetValue("Select ma_ms from dmms where ma_ms = '1'").ToString();
                CurrentEditModelctgt.Stt_rec = CurPh.Stt_rec;
                CurrentEditModelctgt.So_ct = CurPh.So_ct;
                CurrentEditModelctgt.Ngay_ct = CurPh.Ngay_ct;
                CurrentEditModelctgt.Ngay_ct0 = CurPh.Ngay_ct;
                CurrentEditModelctgt.Ma_ct = CurPh.Ma_ct;
                CurrentEditModelctgt.T_tt = LSCt.Sum(x => x.Tien0 + x.Nk + x.Db);
                CurrentEditModelctgt.T_tt_nt = LSCt.Sum(x => x.Tien_nt0 + x.Nk_nt + x.Db_nt);
                CurrentEditModelctgt.T_tien = LSCt.Sum(x => x.Tien0 + x.Nk + x.Db);
                CurrentEditModelctgt.T_tien_nt = LSCt.Sum(x => x.Tien_nt0 + x.Nk_nt + x.Db_nt);
                CurrentEditModelctgt.Ma_kh = CurPh.Ma_kh;
                CurrentEditModelctgt.Dia_chi = CurPh.Dia_chi;
                CurrentEditModelctgt.Ma_so_thue = CurPh.Ma_so_thue;
                CurrentEditModelctgt.Ten_kh = CurPh.Ten_kh;
                CurrentEditModelctgt.Tk_du = CurPh.Ma_nx; 
                if(NotMaNt0Voucher && CurPh.Ty_gia_hq != 0)
                {
                    CurrentEditModelctgt.T_tien = Math.Round(CurrentEditModelctgt.T_tien_nt * CurPh.Ty_gia_hq,M_ROUND_NT);
                }                
            }
        }
        void Grid_CustomizeEditModelTAX(GridCustomizeEditModelEventArgs e)
        {
            CurrentEditModeltax = (Ct72)e.EditModel;            
        }
        async Task Grid_EditModelSavingTAX(GridEditModelSavingEventArgs e)
        {
            if (e.IsNew)
            {               
                return;
            }            
            
            var editingItem = (Ct72)e.EditModel;
            if (editingItem == null) return;            
            var targetItem = LSCt.FirstOrDefault(x => x.Stt_rec == editingItem.Stt_rec && x.Stt_rec0 == editingItem.Stt_rec0);
            if (targetItem != null)
            {
                targetItem.Gia_nt1 = editingItem.Gia_nt1;
                targetItem.Gia1 = editingItem.Gia1;
                targetItem.Tien_nt1 = editingItem.Tien_nt1;
                targetItem.Tien1 = editingItem.Tien1;
                targetItem.Ts_nk = editingItem.Ts_nk;
                targetItem.Nk_nt = editingItem.Nk_nt;
                targetItem.Nk = editingItem.Nk;
                targetItem.Ts_db = editingItem.Ts_db;
                targetItem.Db_nt = editingItem.Db_nt;
                targetItem.Db = editingItem.Db;
                targetItem.Gia_nt2 = editingItem.Gia_nt2;
                targetItem.Gia2 = editingItem.Gia2;
                targetItem.Tien_nt2 = editingItem.Tien_nt2;
                targetItem.Tien2 = editingItem.Tien2;

                //Cập nhật thuế NK và DB vào CurPh
                CurPh.T_nk = LSCt.Sum(x => x.Nk);
                CurPh.T_nk_nt = LSCt.Sum(x => x.Nk_nt);
                CurPh.T_db = LSCt.Sum(x => x.Db);
                CurPh.T_db_nt = LSCt.Sum(x => x.Db_nt);
            }
        }
        async Task Grid_EditModelSavingCTGT(GridEditModelSavingEventArgs e)
        {

            if (e.IsNew)
            {
                LSCtgt.Add(CurrentEditModelctgt);
            }
            else
            {
                Ct72gt cts = LSCtgt.Where(a => a.Stt_rec.Equals(CurrentEditModelctgt.Stt_rec) && a.Stt == CurrentEditModelctgt.Stt).FirstOrDefault();
                if (cts != null)
                {
                    cts.Dvt = CurrentEditModelctgt.Dvt;
                    cts.So_luong = CurrentEditModelctgt.So_luong;
                    cts.Dvt = CurrentEditModelctgt.Dvt;
                    cts.Ma_ms = CurrentEditModelctgt.Ma_ms;
                    cts.Ngay_ct0 = CurrentEditModelctgt.Ngay_ct0;
                    cts.So_ct0 = CurrentEditModelctgt.So_ct0;
                    cts.Kh_mau_hd = CurrentEditModelctgt.Kh_mau_hd;
                    cts.So_seri0 = CurrentEditModelctgt.So_seri0;
                    cts.Ma_kh = CurrentEditModelctgt.Ma_kh;
                    cts.Dia_chi = CurrentEditModelctgt.Dia_chi;
                    cts.Ma_so_thue = CurrentEditModelctgt.Ma_so_thue;
                    cts.Ten_kh = CurrentEditModelctgt.Ten_kh;
                    cts.T_tien_nt = CurrentEditModelctgt.T_tien_nt;
                    cts.T_tien = CurrentEditModelctgt.T_tien;
                    cts.Ma_thue = CurrentEditModelctgt.Ma_thue;
                    cts.Thue_suat = CurrentEditModelctgt.Thue_suat;
                    cts.T_thue = CurrentEditModelctgt.T_thue;
                    cts.T_thue_nt = CurrentEditModelctgt.T_thue_nt;
                    cts.T_tt_nt = CurrentEditModelctgt.T_tt_nt;
                    cts.T_tt = CurrentEditModelctgt.T_tt;
                    cts.Tk_thue_no = CurrentEditModelctgt.Tk_thue_no;
                    cts.Tk_du = CurrentEditModelctgt.Tk_du;
                    cts.Ma_nt = CurPh.Ma_nt;
                    cts.Ma_thck = CurrentEditModelctgt.Ma_thck;
                    cts.Han_tt = CurrentEditModelctgt.Han_tt;
                }
            }
        }

        private async Task OnDeleteRowCTGT(int visibleIndex)
        {
            if (visibleIndex < 0 || visibleIndex >= LSCtgt.Count)
                return;

            var itemToDelete = LSCtgt[visibleIndex];

            var confirm = await myDlSvc.ConfirmAsync(new MessageBoxOptions
            {
                Text = Lap["Do you want to delete"] + " ?",
                Title = Lap["Confirm deletion"],
                OkButtonText = Lap["Yes"],
                CancelButtonText = Lap["No"],
                ShowIcon = true,
                RenderStyle = MessageBoxRenderStyle.Danger
            });

            if (confirm)
            {
                LSCtgt.Remove(itemToDelete);
                int _stt = 1;
                LSCtgt.ForEach((Ct72gt _Ct11) =>
                {
                    _Ct11.Stt = _stt;
                    _Ct11.Stt_rec0 = string.Format("{0:0000}", _Ct11.Stt);
                    _stt++;
                });
                UpdateTotalHT();
                StateHasChanged();
            }
        }
        private void UpdateTotalHT()
        {
            var details = LSCt;
            decimal sum_tien0 = details.Where(x => CurrentEditModel == null || x.Stt_rec0 != CurrentEditModel.Stt_rec0).Sum(x => x.Tien0) + (CurrentEditModel?.Tien0 ?? 0);
            decimal sum_tien_nt0 = details.Where(x => CurrentEditModel == null || x.Stt_rec0 != CurrentEditModel.Stt_rec0).Sum(x => x.Tien_nt0) + (CurrentEditModel?.Tien_nt0 ?? 0);           
            decimal sum_so_luong = details.Where(x => CurrentEditModel == null || x.Stt_rec0 != CurrentEditModel.Stt_rec0).Sum(x => x.So_luong) + (CurrentEditModel?.So_luong ?? 0);
            decimal sum_thue_nt = LSCtgt.Where(x => CurrentEditModelctgt == null || x.Stt_rec0 != CurrentEditModelctgt.Stt_rec0).Sum(x => x.T_thue_nt) + (CurrentEditModelctgt?.T_thue_nt ?? 0);
            decimal sum_thue = LSCtgt.Where(x => CurrentEditModelctgt == null || x.Stt_rec0 != CurrentEditModelctgt.Stt_rec0).Sum(x => x.T_thue) + (CurrentEditModelctgt?.T_thue ?? 0);
            decimal sum_nk = LSCt.Where(x => CurrentEditModeltax == null || x.Stt_rec0 != CurrentEditModeltax.Stt_rec0).Sum(x => x.Nk) + (CurrentEditModeltax?.Nk ?? 0);
            decimal sum_db = LSCt.Where(x => CurrentEditModeltax == null || x.Stt_rec0 != CurrentEditModeltax.Stt_rec0).Sum(x => x.Db) + (CurrentEditModeltax?.Db ?? 0);

            decimal t_tien0 = Math.Round(sum_tien0, M_ROUND);
            decimal t_tien_nt0 = Math.Round(sum_tien_nt0, M_ROUND_NT); 
            decimal t_cp = Math.Round(CurPh.T_cp, M_ROUND);
            decimal t_cp_nt = Math.Round(CurPh.T_cp_nt, M_ROUND_NT);
            decimal t_thue = Math.Round(sum_thue, M_ROUND);
            decimal t_thue_nt = Math.Round(sum_thue_nt, M_ROUND_NT);
            decimal t_nk = Math.Round(sum_nk, M_ROUND);
            decimal t_db = Math.Round(sum_db, M_ROUND);

            if (!NotMaNt0Voucher)
            {
                t_tien0 = t_tien_nt0;               
                t_cp = t_cp_nt;
                t_thue = t_thue_nt;
            }
            
            decimal t_tien_sau_ck = t_tien0 ;
            decimal t_tien_sau_ck_nt = t_tien_nt0;            
            decimal t_tt = t_tien_sau_ck + t_cp + t_thue;
            decimal t_tt_nt = t_tien_sau_ck_nt + t_cp_nt + t_thue_nt;

            CurPh.T_tien0 = t_tien0;
            CurPh.T_tien_nt0 = t_tien_nt0;           
            CurPh.T_cp = t_cp;
            CurPh.T_cp_nt = t_cp_nt;
            CurPh.T_thue = t_thue;
            CurPh.T_thue_nt = t_thue_nt;
            CurPh.T_tt = t_tt;
            CurPh.T_tt_nt = t_tt_nt;
            CurPh.T_tc = CurPh.T_tien0 + CurPh.T_cp + CurPh.T_nk + CurPh.T_db;
            CurPh.T_tc_nt = CurPh.T_tien_nt0 + CurPh.T_cp_nt + CurPh.T_nk_nt + CurPh.T_db_nt;
            CurPh.T_so_luong = sum_so_luong;            

            StateHasChanged();

        }

        private void UpdateTotalThue()
        {
            if (curActionTask == ActionTask.View || CurPh == null)
                return;


            decimal tThueNt = LSCtgt.Where(x => CurrentEditModelctgt == null || x.Stt_rec0 != CurrentEditModelctgt.Stt_rec0)
                .Sum(x => x.T_thue_nt) + (CurrentEditModelctgt?.T_thue_nt ?? 0);
            CurPh.T_thue_nt = tThueNt;


            decimal tTienNt = CurPh.T_tien_nt;

            decimal tongCongNt = tTienNt + tThueNt;
            CurPh.T_tt_nt = tongCongNt;

            if (!NotMaNt0Voucher)
            {
                decimal tThueVnd = LSCtgt
                    .Where(x => CurrentEditModelctgt == null || x.Stt_rec0 != CurrentEditModelctgt.Stt_rec0)
                    .Sum(x => x.T_thue)
                    + (CurrentEditModelctgt?.T_thue ?? 0);
                decimal tTienVnd = CurPh.T_tien;             // Tiền hàng đã quy đổi

                CurPh.T_thue = Math.Round(tThueVnd, M_ROUND);
                CurPh.T_tt = Math.Round(tTienVnd + tThueVnd, M_ROUND);
            }
            else
            {
                CurPh.T_thue = tThueNt;
                CurPh.T_tt = tongCongNt;
            }
            StateHasChanged();
        }
        async Task Grid_DataItemDeletingCTGT(GridDataItemDeletingEventArgs e)
        {
            var editCt72 = (Ct72gt)e.DataItem;
            LSCtgt.Remove(editCt72);
            int _stt = 1;
            LSCtgt.ForEach((Ct72gt _Ct11) =>
            {
                _Ct11.Stt = _stt;
                _Ct11.Stt_rec0 = string.Format("{0:0000}", _Ct11.Stt);
                _stt++;
            });
        }
        void OnGridCTGT_DMDM_Changed(ExpandoObject value, string ma_dm)
        {
            switch (ma_dm.ToLower())
            {
                case "so_ct0":
                    {
                        bool isVND = CurPh.Ma_nt?.Trim() == M_ma_nt0?.Trim();                        
                        if (string.IsNullOrEmpty(CurrentEditModelctgt.Ma_kh))
                        {
                            CurrentEditModelctgt.Ma_kh = CurPh.Ten_kh;
                            CurrentEditModelctgt.Dia_chi = string.IsNullOrEmpty(CurPh.Dia_chi) ? CurPh.Dia_chi : CurPh.Dia_chi;
                            CurrentEditModelctgt.Ma_so_thue = CurPh.Ma_so_thue;
                            CurrentEditModelctgt.Han_tt = CurPh.Han_tt;
                        }
                        
                        decimal tongDaPhanBo_Nt = LSCtgt
                            .Where(x => x.Stt_rec == CurPh.Stt_rec && x.So_ct0 != CurrentEditModelctgt.So_ct0)
                            .Sum(x => x.T_tien_nt);

                        decimal tongDaPhanBo_Vnd = LSCtgt
                            .Where(x => x.Stt_rec == CurPh.Stt_rec && x.So_ct0 != CurrentEditModelctgt.So_ct0)
                            .Sum(x => x.T_tien);
                        
                        decimal tongTienGoc_Nt = CurPh.T_tien_nt0;
                        decimal tongTienGoc_Vnd = CurPh.T_tien0;
                        
                        bool cungTaiKhoan = string.IsNullOrEmpty(CurPh.Tk_i) || CurPh.Tk_i == CurPh.Ma_nx;
                        bool cungKhachHang = string.IsNullOrEmpty(CurPh.Ma_kh_i) || CurPh.Ma_kh_i == CurPh.Ma_kh;

                        if (cungTaiKhoan && cungKhachHang)
                        {
                            tongTienGoc_Nt += CurPh.T_cp_nt;
                            tongTienGoc_Vnd += CurPh.T_cp;
                        }                        
                        decimal conLai_Nt = tongTienGoc_Nt - tongDaPhanBo_Nt;
                        decimal conLai_Vnd = tongTienGoc_Vnd - tongDaPhanBo_Vnd;
                        
                        bool isLastRow = LSCtgt.Count(x => x.Stt_rec == CurPh.Stt_rec) == 1;
                        bool chuaNhapTien = (CurrentEditModelctgt.T_tien_nt) == 0;

                        if (isLastRow || chuaNhapTien)
                        {
                            CurrentEditModelctgt.T_tien_nt = Math.Round(conLai_Nt, M_ROUND_NT);
                            CurrentEditModelctgt.T_tien = Math.Round(conLai_Vnd, M_ROUND);
                        }

                        decimal thueSuat = CurrentEditModelctgt.Thue_suat;
                        decimal thue_Nt = Math.Round(CurrentEditModelctgt.T_tien_nt * thueSuat / 100m, M_ROUND_NT);
                        decimal thue_Vnd = Math.Round(CurrentEditModelctgt.T_tien * thueSuat / 100m, M_ROUND);

                        CurrentEditModelctgt.T_thue_nt = thue_Nt;
                        CurrentEditModelctgt.T_thue = thue_Vnd;                        
                        CurrentEditModelctgt.T_tt_nt = (CurrentEditModelctgt.T_tien_nt) + thue_Nt;
                        CurrentEditModelctgt.T_tt = (CurrentEditModelctgt.T_tien) + thue_Vnd;
                        
                        if (isVND)
                        {
                            CurrentEditModelctgt.T_tien = CurrentEditModelctgt.T_tien_nt;
                            CurrentEditModelctgt.T_thue = CurrentEditModelctgt.T_thue_nt;
                            CurrentEditModelctgt.T_tt = CurrentEditModelctgt.T_tt_nt;
                        }
                        
                        UpdateTotalHT();

                        StateHasChanged();
                        break;
                    }

                case "ma_kh":
                    {
                        CurrentEditModelctgt.Ten_kh = value.Where(a => a.Key.Equals("ten_kh")).FirstOrDefault().Value.ToString();
                        CurrentEditModelctgt.Dia_chi = value.Where(a => a.Key.Equals("dia_chi")).FirstOrDefault().Value.ToString();
                        CurrentEditModelctgt.Ma_so_thue = value.Where(a => a.Key.Equals("ma_so_thue")).FirstOrDefault().Value.ToString();

                        break;
                    }
                case "ma_thue":
                    {

                        CurrentEditModelctgt.Thue_suat = decimal.Parse(value.Where(a => a.Key.Equals("thue_suat")).FirstOrDefault().Value.ToString());
                        CurrentEditModelctgt.Tk_thue_no = value.Where(a => a.Key.Equals("tk_thue_no")).FirstOrDefault().Value.ToString();
                        //CurrentEditModelctgt.Tk_du = value.Where(a => a.Key.Equals("tk_thue_no")).FirstOrDefault().Value.ToString();
                        CurPh.Tk_thue_no = value.Where(a => a.Key.Equals("tk_thue_no")).FirstOrDefault().Value.ToString();
                        Decimal num7 = CurrentEditModelctgt.T_tien_nt;
                        Decimal num8 = CurrentEditModelctgt.T_tien;
                        Decimal num9 = CurrentEditModelctgt.Thue_suat;
                        Decimal num2 = new Decimal(0);
                        Decimal num10 = Converts.Round(num7 * (num9 / 100), M_IP_TIEN_NT);
                        CurrentEditModelctgt.T_thue_nt = num10;
                        CurrentEditModelctgt.T_tt_nt = (num7 + num10);

                        Decimal num11 = Converts.Round(num8 * num9 / 100, M_IP_TIEN);
                        CurrentEditModelctgt.T_thue = num11;
                        CurrentEditModelctgt.T_tt = (num8 + num11);

                        if (!NotMaNt0Voucher)
                        {
                            CurrentEditModelctgt.T_thue = CurrentEditModelctgt.T_thue_nt;
                            CurrentEditModelctgt.T_tt = CurrentEditModelctgt.T_tt_nt;
                        }
                        UpdateTotalHT();
                        break;

                    }
                case "ma_thck":
                    {
                        CurrentEditModelctgt.Han_tt = int.Parse(value.Where(a => a.Key.Equals("han_tt")).FirstOrDefault().Value.ToString());                       
                        break;

                    }

            }

        }
        void OnGridCTGT_Value_Changed(decimal newvalue, string name)
        {
            int sua_tien = 0;
            int.TryParse(CurPh.Sua_tien.ToString(), out sua_tien);
            if (!string.IsNullOrEmpty(name))
            {
                switch (name.ToLower())
                {
                    case "so_luong":
                        {
                            if (newvalue == null)
                            {
                                CurrentEditModelctgt.So_luong = 0;
                            }
                            else
                            {
                                CurrentEditModelctgt.So_luong = Convert.ToDecimal(newvalue);
                            }

                            decimal soLuong = CurrentEditModelctgt.So_luong;
                            decimal gia_nt = CurrentEditModelctgt.Gia_nt;                            
                            if ((CurrentEditModelctgt.T_tien_nt) == 0 && soLuong != 0 && gia_nt != 0)
                            {
                                decimal tien_nt_moi = Math.Round(soLuong * gia_nt, M_ROUND_NT); // hoặc M_ROUND
                                CurrentEditModelctgt.T_tien_nt = tien_nt_moi;                                
                                
                                if (!NotMaNt0Voucher)
                                {
                                    CurrentEditModelctgt.T_tien = tien_nt_moi;
                                }
                                else
                                {
                                    decimal ty_gia = CurPh.Ty_gia_hq > 0 ? CurPh.Ty_gia_hq : (CurPh.Ty_gia > 0 ? CurPh.Ty_gia:1m);
                                    CurrentEditModelctgt.T_tien = Math.Round(tien_nt_moi * ty_gia, M_ROUND);
                                }
                            }
                            UpdateTotalHT();
                            break;
                        }
                    case "gia_nt":
                        {
                            if (newvalue == null)
                            {
                                CurrentEditModelctgt.Gia_nt = 0;
                            }
                            else
                            {
                                CurrentEditModelctgt.Gia_nt = Convert.ToDecimal(newvalue);
                            }

                            decimal soLuong = CurrentEditModelctgt.So_luong;
                            decimal gia_nt = CurrentEditModelctgt.Gia_nt;

                            // === CHỈ TÍNH TIỀN NẾU CHƯA CÓ TIỀN (người dùng chưa sửa tay) ===
                            if ((CurrentEditModelctgt.T_tien_nt) == 0 && soLuong != 0 && gia_nt != 0)
                            {
                                decimal tien_nt_moi = Math.Round(soLuong * gia_nt, M_ROUND_NT); // hoặc M_ROUND
                                CurrentEditModelctgt.T_tien_nt = tien_nt_moi;

                                // Nếu là VND → đồng bộ sang tiền VND
                                bool isVND = CurPh.Ma_nt?.Trim() == M_ma_nt0?.Trim();
                                if (isVND)
                                {
                                    CurrentEditModelctgt.T_tien = tien_nt_moi;
                                }
                                else
                                {
                                    decimal ty_gia = CurPh.Ty_gia > 0 ? CurPh.Ty_gia : 1m;
                                    CurrentEditModelctgt.T_tien = Math.Round(tien_nt_moi * ty_gia, M_ROUND);
                                }
                            }
                            UpdateTotalHT();
                            break;
                        }
                    case "thue_suat":
                        {

                            Decimal t_tien_nt = CurrentEditModelctgt.T_tien_nt;
                            Decimal t_tien = CurrentEditModelctgt.T_tien;
                            Decimal thue_suat = CurrentEditModelctgt.Thue_suat = newvalue;
                            Decimal num0 = t_tien_nt * (thue_suat / 100) * CurPh.Ty_gia;
                            Decimal num1 = t_tien * (thue_suat / 100) * CurPh.Ty_gia;

                            if (num0 != 0)
                            {
                                CurrentEditModelctgt.T_thue_nt = num0;
                                CurrentEditModelctgt.T_tt_nt = num0 + t_tien_nt;
                            }
                            if (num1 != 0)
                            {
                                CurrentEditModelctgt.T_thue = num1;
                                CurrentEditModelctgt.T_tt = num1 + t_tien;
                            }
                            if (!NotMaNt0Voucher)
                            {
                                CurrentEditModelctgt.T_thue = CurrentEditModelctgt.T_thue_nt;
                                CurrentEditModelctgt.T_tt = CurrentEditModelctgt.T_tt_nt;

                            }
                            UpdateTotalHT();
                            break;
                        }
                    case "t_thue_nt":
                        {
                            Decimal thue_suat = CurrentEditModelctgt.T_thue_nt = newvalue;
                            Decimal t_tien_nt = CurrentEditModelctgt.T_tien_nt;
                            Decimal t_thue_nt = CurrentEditModelctgt.T_thue_nt;

                            if (t_thue_nt == 0)
                            {
                                Decimal thue_suat2 = CurrentEditModelctgt.Thue_suat;
                                t_thue_nt = t_tien_nt * (thue_suat / 100) * CurPh.Ty_gia;
                                CurrentEditModelctgt.T_thue_nt = t_thue_nt;
                            }
                            CurrentEditModelctgt.T_tt_nt = t_tien_nt + t_thue_nt;
                            Decimal num0 = t_thue_nt * CurPh.Ty_gia;
                            if (num0 != 0)
                            {
                                CurrentEditModelctgt.T_thue = num0;
                                CurrentEditModelctgt.T_tt = CurrentEditModelctgt.T_tien + num0;
                            }
                            if (!NotMaNt0Voucher)
                            {
                                CurrentEditModelctgt.T_thue = CurrentEditModelctgt.T_thue_nt;
                                CurrentEditModelctgt.T_tt = CurrentEditModelctgt.T_tt_nt;

                            }
                            UpdateTotalHT();
                            break;

                        }
                    case "t_thue":
                        {
                            CurrentEditModelctgt.T_thue = newvalue;
                            Decimal t_tien = CurrentEditModelctgt.T_tien;
                            Decimal t_thue = newvalue;
                            if (t_thue == 0)
                            {
                                Decimal thue_suat = CurrentEditModelctgt.Thue_suat;
                                t_thue = t_tien * thue_suat / 100;
                                CurrentEditModelctgt.T_thue = t_thue;
                            }
                            CurrentEditModelctgt.T_tt = t_tien + t_thue;
                            UpdateTotalHT();
                            break;

                        }
                    case "t_tien_nt":
                        {
                            CurrentEditModelctgt.T_tien_nt = newvalue;
                            Decimal t_tien_nt = CurrentEditModelctgt.T_tien_nt;
                            Decimal thue_suat = CurrentEditModelctgt.Thue_suat;
                            Decimal num0 = t_tien_nt * thue_suat / 100 * CurPh.Ty_gia;

                            if (num0 != 0)
                            {
                                CurrentEditModelctgt.T_thue_nt = num0;
                                Decimal num1 = num0 * CurPh.Ty_gia;
                                if (num1 != 0)
                                {
                                    CurrentEditModelctgt.T_thue = num1;
                                    CurrentEditModelctgt.T_tt = CurrentEditModelctgt.T_tien + num1;
                                }
                            }
                            CurrentEditModelctgt.T_tt_nt = t_tien_nt + CurrentEditModelctgt.T_thue_nt;
                            if (CurrentEditModelctgt.T_tien == 0 && CurPh.Ty_gia != 0)
                            {
                                Decimal num1 = t_tien_nt * CurPh.Ty_gia;
                                CurrentEditModelctgt.T_tien = num1;
                                Decimal num2 = num1 * thue_suat / 100;
                                if (num2 != 0)
                                {
                                    CurrentEditModelctgt.T_thue = num2;
                                }
                                CurrentEditModelctgt.T_tt = num1 + CurrentEditModelctgt.T_thue;
                            }
                            if (!NotMaNt0Voucher)
                            {
                                CurrentEditModelctgt.T_thue = CurrentEditModelctgt.T_thue_nt;
                                CurrentEditModelctgt.T_tt = CurrentEditModelctgt.T_tt_nt;

                            }
                            break;
                            UpdateTotalHT();
                        }
                    case "t_tien":
                        {
                            CurrentEditModelctgt.T_tien = newvalue;
                            if (newvalue == 0)
                            {
                                Decimal num3 = CurrentEditModelctgt.T_tien_nt * CurPh.Ty_gia;
                                CurrentEditModelctgt.T_tien = num3;
                            }
                            Decimal thue_suat = CurrentEditModelctgt.Thue_suat;
                            Decimal t_tien = CurrentEditModelctgt.T_tien;
                            Decimal num0 = t_tien * thue_suat / 100;

                            if (num0 != 0)
                            {
                                CurrentEditModelctgt.T_thue = num0;
                            }
                            CurrentEditModelctgt.T_tt = t_tien + CurrentEditModelctgt.T_thue;
                            UpdateTotalHT();

                            break;
                        }
                    case "han_tt":
                        {
                            CurrentEditModelctgt.Han_tt = (int)newvalue;                            
                            break;
                        }
                }
            }
        }
        private void Grid_CustomizeSummaryDisplayText(GridCustomizeSummaryDisplayTextEventArgs e)
        {
            string str = "{0:" + e.Item.ValueDisplayFormat + "}";
            e.DisplayText = string.Format(str, e.Value);
        }
        #endregion
        
        #region Autocomplete Events
        private async Task OnAutoCompleteKhachhangChanged(ExpandoObject value)
        {
            if (value == null) return;
            
            var dict = value as IDictionary<string, object>;
            if (dict.ContainsKey("ma_kh"))
            {
                CurPh.Ma_kh = dict["ma_kh"]?.ToString();
            }
            
            if (dict.ContainsKey("dia_chi"))
            {
                CurPh.Dia_chi = dict["dia_chi"]?.ToString()?.Trim();
            }
            
            if (dict.ContainsKey("ma_so_thue"))
            {
                CurPh.Ma_so_thue = dict["ma_so_thue"]?.ToString()?.Trim();
            }
            if (dict.ContainsKey("doi_tac"))
            {
                CurPh.Ong_ba = String.IsNullOrWhiteSpace(CurPh.Ong_ba)?dict["doi_tac"]?.ToString()?.Trim(): CurPh.Ong_ba;
            }

            StateHasChanged();
        }
        
        private async Task OnAutoCompleteDMNXChanged(ExpandoObject value)
        {
            if (value == null) return;
            
            var dict = value as IDictionary<string, object>;
            if (dict.ContainsKey("tk"))
            {
                CurPh.Ma_nx = dict["tk"]?.ToString();
            }
            
            string str2 = myOption.GetOptionsValue("M_TK_TK_VT");
            if (!string.IsNullOrEmpty(CurPh.Ma_nx))
            {
                foreach (string str3 in str2.Split(','))
                {
                    if (!string.IsNullOrWhiteSpace(str3.ToString()) && CurPh.Ma_nx.Trim().StartsWith(str3.ToString().Trim()))
                    {
                        AllowTaoPC = true;
                        break;
                    }
                    else
                    {
                        AllowTaoPC = false;
                        CurPh.Tao_pc = 0;
                    }
                }
            }

            StateHasChanged();
        }

        void OnAutoCompleteQuyensoChanged(ExpandoObject value)
        {
            string ma_qs = value.Where(a => a.Key.Equals("ma_qs")).FirstOrDefault().Value?.ToString() ?? "";
            curQuyenso = GetNewSo_ct(ma_qs);
            CurPh.So_ct = string.Format(curQuyenso.Ma_qs, curQuyenso.So_ct);
            StateHasChanged();
        }

        private async Task OnAutoCompleteMaNTChanged(ExpandoObject value)
        {
            if (value == null) return;
            
            var dict = value as IDictionary<string, object>;
            if (dict.ContainsKey("ma_nt"))
            {
                CurPh.Ma_nt = dict["ma_nt"]?.ToString();
                NotMaNt0Voucher = !CurPh.Ma_nt?.Equals(M_ma_nt0?.Trim()) ?? false;                
                CurPh.Ty_gia = GetRates(CurPh.Ma_nt, CurPh.Ngay_ct);
                if ((CurPh.Ty_gia_hq == 0 || CurPh.Ty_gia_hq == 1) && CurPh.Ty_gia > 0)
                {
                    CurPh.Ty_gia_hq = CurPh.Ty_gia;
                }

                TinhTienQuyDoi();
            }
            
            StateHasChanged();
        }
        
        private void OnValueTyGiaChanged(decimal newValue)
        {
            CurPh.Ty_gia = newValue;
            if((CurPh.Ty_gia_hq == 0 || CurPh.Ty_gia_hq == 1) && newValue > 0)
            {
                CurPh.Ty_gia_hq = CurPh.Ty_gia;
            }
            TinhTienQuyDoi();
            StateHasChanged();
        }
        private void OnValueTyGiaHQChanged(decimal newValue)
        {
            CurPh.Ty_gia_hq = newValue;
            TinhTienQuyDoi();
            StateHasChanged();
        }

        private void OnValueChiPhiChanged(decimal newValue, string fieldName)
        {
            if (fieldName == "T_cp_nt")
            {
                CurPh.T_cp_nt = newValue;
                CurPh.T_cp = Math.Round(newValue * CurPh.Ty_gia, 0);
            }
            else if (fieldName == "T_cp")
            {
                CurPh.T_cp = newValue;
                if (CurPh.Ty_gia > 0 && CurPh.T_cp_nt == 0)
                {
                    CurPh.T_cp_nt = Math.Round(newValue / CurPh.Ty_gia, 0);
                }
            }
            
            TinhTong();
            StateHasChanged();
        }
        private void OnValueDBChanged(decimal newValue, string fieldName)
        {
            if (fieldName == "T_db_nt")
            {
                CurPh.T_db_nt = newValue;
                CurPh.T_db = Math.Round(newValue * CurPh.Ty_gia_hq, 0);
            }
            else if (fieldName == "T_db")
            {
                CurPh.T_db = newValue;
                if (CurPh.Ty_gia_hq > 0 && CurPh.T_db_nt == 0)
                {
                    CurPh.T_db_nt = Math.Round(newValue / CurPh.Ty_gia_hq, 0);
                }
            }
            // Thues NK
            if (fieldName == "T_nk_nt")
            {
                CurPh.T_nk_nt = newValue;
                CurPh.T_nk = Math.Round(newValue * CurPh.Ty_gia_hq, 0);
            }
            else if (fieldName == "T_nk")
            {
                CurPh.T_nk = newValue;
                if (CurPh.Ty_gia_hq > 0 && CurPh.T_nk_nt == 0)
                {
                    CurPh.T_nk_nt = Math.Round(newValue / CurPh.Ty_gia_hq, 0);
                }
            }

            TinhTong();
            StateHasChanged();
        }
        #endregion

        #region Grid Cell Events
        private void OnGridCT_DMDM_Changed(ExpandoObject value, string fieldName)
        {
            if (CurrentEditModel == null || value == null) return;
            
            var dict = value as IDictionary<string, object>;
            
            switch (fieldName)
            {
                case "Ma_vt":
                    CurrentEditModel.Ma_vt = dict["ma_vt"]?.ToString();
                    CurrentEditModel.Ten_vt = dict.ContainsKey("ten_vt") ? dict["ten_vt"]?.ToString() : "";
                    CurrentEditModel.Dvt = dict.ContainsKey("dvt") ? dict["dvt"]?.ToString() : "";
                    //CurrentEditModel.Dvt1 = CurrentEditModel.Dvt;
                    CurrentEditModel.Tk_vt = dict.ContainsKey("tk_vt") ? dict["tk_vt"]?.ToString() : "";
                    CurrentEditModel.Vt_ton_kho = dict.ContainsKey("vt_ton_kho") ? Convert.ToInt32(dict["vt_ton_kho"]) : 0;

                    string _filter_dvt = String.Format(" ma_vt = '{0}'", CurrentEditModel.Ma_vt);
                    object heso = myDb.GetValue("select hs_qd from v_dmvtdvt where ma_vt = '" + CurrentEditModel.Ma_vt.Trim() + "' AND dvt1=N'" + (CurrentEditModel.Dvt1?.Trim() ?? "") + "'");

                    if (heso != null && heso != DBNull.Value && decimal.TryParse(heso.ToString(), out decimal parsedHeso))
                    {
                        CurrentEditModel.He_so1 = parsedHeso;
                    }
                    else
                    {
                        CurrentEditModel.Dvt1 = value.Where(a => a.Key.Equals("dvt")).FirstOrDefault().Value?.ToString() ?? "";
                        CurrentEditModel.He_so1 = 1;
                    }

                    LoadTonKho(CurrentEditModel);
                    break;
                    
                case "Dvt1":
                    CurrentEditModel.Dvt1 = dict["dvt1"]?.ToString();
                    CurrentEditModel.He_so1 = dict.ContainsKey("hs_qd") ? Convert.ToDecimal(dict["hs_qd"]) : 1;
                    break;
                    
                case "Ma_kho_i":
                    CurrentEditModel.Ma_kho_i = dict["ma_kho"]?.ToString();
                    LoadTonKho(CurrentEditModel);
                    break;
                    
                case "Ma_lo":
                    if (dict.ContainsKey("ma_lo"))
                    {
                        CurrentEditModel.Ma_lo = dict["ma_lo"]?.ToString();
                        CurrentEditModel.Han_sd_lot = dict.ContainsKey("han_sd") ? (DateTime?)dict["han_sd"] : null;
                        LoadTonKho(CurrentEditModel);
                    }
                    break;
                    
                case "Tk_vt":
                    if (dict.ContainsKey("tk"))
                    {
                        CurrentEditModel.Tk_vt = dict["tk"]?.ToString();
                    }
                    break;
            }
            
            StateHasChanged();
        }
        
        private void OnGridCT_Value_Changed(decimal newValue, string fieldName)
        {
            if (CurrentEditModel == null) return;
            
            switch (fieldName)
            {
                case "So_luong":
                    CurrentEditModel.So_luong = newValue;
                    CurrentEditModel.So_luong1 = newValue * CurrentEditModel.He_so1;
                    TinhTienHang(CurrentEditModel);
                    break;
                    
                case "Gia_nt0":
                    CurrentEditModel.Gia_nt0 = newValue;
                    CurrentEditModel.Gia0 = Math.Round(newValue * CurPh.Ty_gia, 0);
                    TinhTienHang(CurrentEditModel);
                    break;
                    
                case "Gia0":
                    CurrentEditModel.Gia0 = newValue;
                    if (CurPh.Ty_gia > 0)
                    {
                        CurrentEditModel.Gia_nt0 = Math.Round(newValue / CurPh.Ty_gia, 0);
                    }
                    TinhTienHang(CurrentEditModel);
                    break;
                    
                case "Tien_nt0":
                    CurrentEditModel.Tien_nt0 = newValue;
                    CurrentEditModel.Tien0 = Math.Round(newValue * CurPh.Ty_gia, 0);
                    if (CurrentEditModel.So_luong > 0)
                    {
                        CurrentEditModel.Gia_nt0 = Math.Round(newValue / CurrentEditModel.So_luong, 0);
                        CurrentEditModel.Gia0 = Math.Round(CurrentEditModel.Tien0 / CurrentEditModel.So_luong, 0);
                    }
                    break;
                    
                case "Tien0":
                    CurrentEditModel.Tien0 = newValue;
                    if (CurPh.Ty_gia > 0)
                    {
                        CurrentEditModel.Tien_nt0 = Math.Round(newValue / CurPh.Ty_gia, 0);
                    }
                    if (CurrentEditModel.So_luong > 0)
                    {
                        CurrentEditModel.Gia0 = Math.Round(newValue / CurrentEditModel.So_luong, 0);
                        CurrentEditModel.Gia_nt0 = Math.Round(CurrentEditModel.Tien_nt0 / CurrentEditModel.So_luong, 0);
                    }
                    break;
            }
            
            StateHasChanged();
        }

        private void OnGridTAX_Value_Changed(decimal newValue, string name)
        {
            if (!string.IsNullOrEmpty(name))
            {
                decimal ty_gia_hq = CurPh.Ty_gia_hq > 0 ? CurPh.Ty_gia_hq : 1m;
                bool isVND = !NotMaNt0Voucher;

                switch (name.ToLower())
                {
                    case "gia_nt1":
                        {
                            CurrentEditModeltax.Gia_nt1 = newValue;
                            CurrentEditModeltax.Gia1 = Math.Round(newValue * ty_gia_hq, M_ROUND_GIA);
                            decimal So_luong = CurrentEditModeltax.So_luong;
                            if (newValue * So_luong != 0)
                            {
                                CurrentEditModeltax.Tien_nt1 = Math.Round(newValue * So_luong, M_ROUND_NT);
                                CurrentEditModeltax.Tien1 = Math.Round(CurrentEditModeltax.Tien_nt1 * ty_gia_hq, M_ROUND);
                                if (CurrentEditModeltax.Ts_nk != 0)
                                {
                                    CurrentEditModeltax.Nk_nt = Math.Round(CurrentEditModeltax.Tien_nt1 * CurrentEditModeltax.Ts_nk / 100, M_ROUND_NT);
                                    CurrentEditModeltax.Nk = Math.Round(CurrentEditModeltax.Nk_nt * ty_gia_hq, M_ROUND);
                                }
                            }
                            if (isVND)
                            {
                                CurrentEditModeltax.Gia1 = CurrentEditModeltax.Gia_nt1;
                                CurrentEditModeltax.Tien1 = CurrentEditModeltax.Tien_nt1;
                                CurrentEditModeltax.Nk = CurrentEditModeltax.Nk_nt;
                            }
                            TinhTien2TAX();
                            break;
                        }
                    case "gia1":
                        {
                            CurrentEditModeltax.Gia1 = newValue;
                            decimal So_luong = CurrentEditModeltax.So_luong;
                            if (newValue * So_luong != 0)
                            {
                                CurrentEditModeltax.Tien1 = Math.Round(newValue * So_luong, M_ROUND);
                                if (CurrentEditModeltax.Ts_nk != 0)
                                {
                                    CurrentEditModeltax.Nk = Math.Round(CurrentEditModeltax.Tien1 * CurrentEditModeltax.Ts_nk / 100, M_ROUND);
                                }
                            }
                            TinhTien2TAX();
                            break;
                        }
                    case "tien_nt1":
                        {
                            CurrentEditModeltax.Tien_nt1 = newValue;
                            CurrentEditModeltax.Tien1 = isVND ? newValue : Math.Round(newValue * ty_gia_hq, M_ROUND);
                            if (CurrentEditModeltax.So_luong > 0)
                            {
                                CurrentEditModeltax.Gia_nt1 = Math.Round(newValue / CurrentEditModeltax.So_luong, M_ROUND_GIA_NT);
                                CurrentEditModeltax.Gia1 = isVND ? CurrentEditModeltax.Gia_nt1 : Math.Round(CurrentEditModeltax.Tien1 / CurrentEditModeltax.So_luong, M_ROUND_GIA);
                            }
                            TinhTien2TAX();
                            break;
                        }
                    case "tien1":
                        {
                            CurrentEditModeltax.Tien1 = newValue;
                            if (CurrentEditModeltax.So_luong > 0)
                            {
                                CurrentEditModeltax.Gia1 = Math.Round(newValue / CurrentEditModeltax.So_luong, M_ROUND_GIA);
                            }
                            TinhTien2TAX();
                            break;
                        }
                    case "ts_nk":
                        {
                            CurrentEditModeltax.Ts_nk = newValue;
                            if (CurrentEditModeltax.Tien_nt1 * newValue != 0)
                            {
                                CurrentEditModeltax.Nk_nt = Math.Round(CurrentEditModeltax.Tien_nt1 * newValue / 100, M_ROUND_NT);
                                CurrentEditModeltax.Nk = isVND ? CurrentEditModeltax.Nk_nt : Math.Round(CurrentEditModeltax.Nk_nt * ty_gia_hq, M_ROUND);
                            }
                            TinhTien2TAX();
                            break;
                        }
                    case "nk_nt":
                        {
                            CurrentEditModeltax.Nk_nt = newValue;
                            CurrentEditModeltax.Nk = isVND ? newValue : Math.Round(newValue * ty_gia_hq, M_ROUND);
                            TinhTien2TAX();
                            break;
                        }
                    case "nk":
                        {
                            CurrentEditModeltax.Nk = newValue;
                            TinhTien2TAX();
                            break;
                        }
                    case "ts_db":
                        {
                            CurrentEditModeltax.Ts_db = newValue;
                            if ((CurrentEditModeltax.Tien_nt1 + CurrentEditModeltax.Nk_nt) * newValue != 0)
                            {
                                CurrentEditModeltax.Db_nt = Math.Round((CurrentEditModeltax.Tien_nt1 + CurrentEditModeltax.Nk_nt) * newValue / 100, M_ROUND_NT);
                                CurrentEditModeltax.Db = isVND ? CurrentEditModeltax.Db_nt : Math.Round(CurrentEditModeltax.Db_nt * ty_gia_hq, M_ROUND);
                            }
                            TinhTien2TAX();
                            break;
                        }
                    case "db_nt":
                        {
                            CurrentEditModeltax.Db_nt = newValue;
                            CurrentEditModeltax.Db = isVND ? newValue : Math.Round(newValue * ty_gia_hq, M_ROUND);
                            TinhTien2TAX();
                            break;
                        }
                    case "db":
                        {
                            CurrentEditModeltax.Db = newValue;
                            TinhTien2TAX();
                            break;
                        }
                }
            }
        }

        private void TinhTien2TAX()
        {
            CurrentEditModeltax.Tien_nt2 = CurrentEditModeltax.Tien_nt1 + CurrentEditModeltax.Nk_nt + CurrentEditModeltax.Db_nt;
            CurrentEditModeltax.Tien2 = CurrentEditModeltax.Tien1 + CurrentEditModeltax.Nk + CurrentEditModeltax.Db;
            if (CurrentEditModeltax.So_luong > 0)
            {
                CurrentEditModeltax.Gia_nt2 = Math.Round(CurrentEditModeltax.Tien_nt2 / CurrentEditModeltax.So_luong, M_ROUND_GIA_NT);
                CurrentEditModeltax.Gia2 = Math.Round(CurrentEditModeltax.Tien2 / CurrentEditModeltax.So_luong, M_ROUND_GIA);
            }


        }
        #endregion

        #region Calculation Methods
        private void TinhTienHang(Ct72 ct)
        {
            if (ct == null) return;
            
            ct.Tien_nt0 = Math.Round(ct.So_luong * ct.Gia_nt0, 0);
            ct.Tien0 = Math.Round(ct.Tien_nt0 * CurPh.Ty_gia, 0);
            
            ct.Tien_nt1 = ct.Tien_nt0;
            ct.Tien1 = ct.Tien0;
            ct.Gia_nt1 = ct.Gia_nt0;
            ct.Gia1 = ct.Gia0;
        }        
        private void TinhTong()
        {
            CurPh.T_so_luong = LSCt.Sum(x => x.So_luong);
            CurPh.T_tien_nt0 = LSCt.Sum(x => x.Tien_nt0);
            CurPh.T_tien0 = LSCt.Sum(x => x.Tien0);
            CurPh.T_tien_nt1 = LSCt.Sum(x => x.Tien_nt1);
            CurPh.T_tien1 = LSCt.Sum(x => x.Tien1);
            
            CurPh.T_tt_nt = CurPh.T_tien_nt1 + CurPh.T_cp_nt + CurPh.T_thue_nt;
            CurPh.T_tt = CurPh.T_tien1 + CurPh.T_cp + CurPh.T_thue;
            
            StateHasChanged();
        }
        
        private void TinhTienQuyDoi()
        {
            foreach (var ct in LSCt)
            {
                ct.Gia0 = Math.Round(ct.Gia_nt0 * CurPh.Ty_gia, M_ROUND_GIA);
                ct.Tien0 = Math.Round(ct.Tien_nt0 * CurPh.Ty_gia, M_ROUND);
                ct.Gia1 = Math.Round(ct.Gia_nt1 * CurPh.Ty_gia_hq, M_ROUND_GIA);
                ct.Tien1 = Math.Round(ct.Tien_nt1 * CurPh.Ty_gia_hq, M_ROUND);
                ct.Gia2 = Math.Round(ct.Gia_nt2 * CurPh.Ty_gia_hq, M_ROUND_GIA);
                ct.Tien2 = Math.Round(ct.Tien_nt2 * CurPh.Ty_gia_hq, M_ROUND);
                ct.Cp = Math.Round(ct.Cp_nt * CurPh.Ty_gia, M_ROUND);
                ct.Nk = Math.Round(ct.Nk_nt * CurPh.Ty_gia_hq, M_ROUND);
                ct.Db = Math.Round(ct.Db_nt * CurPh.Ty_gia_hq, M_ROUND);
            }
            
            TinhTong();
        }
        
        private void LoadTonKho(Ct72 ct)
        {
            if (string.IsNullOrEmpty(ct.Ma_vt) || string.IsNullOrEmpty(ct.Ma_kho_i))
                return;
            
            try
            {
                string filter = $"ma_vt = '{ct.Ma_vt}' AND ma_kho = '{ct.Ma_kho_i}'";
                
                if (!string.IsNullOrEmpty(ct.Ma_lo))
                {
                    filter += $" AND ma_lo = '{ct.Ma_lo}'";
                }
                
                string sql = $"SELECT SUM(ton_cuoi) as ton FROM dmton WHERE {filter}";
                var result = myDb.GetValue(sql);
                
                if (result != null && result != DBNull.Value)
                {
                    ct.Ton13 = Convert.ToDecimal(result);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error LoadTonKho: {ex.Message}");
            }
        }
        #endregion
        
        #region Buttons
        private async Task btnPhanBoNK_Click()
        {
            if (CurPh.T_nk == 0 && CurPh.T_nk_nt == 0)
            {
                myStateMN.SetNewThongbao(Lap["No import tax to allocate"]);
                return;
            }
            if (CurPh.T_nk == 0 && CurPh.T_nk_nt != 0)
            {
                CurPh.T_nk = Math.Round(CurPh.T_nk_nt * CurPh.Ty_gia_hq, M_ROUND);
            }

            decimal totalTien = LSCt.Sum(x => x.Tien1);
            if (totalTien == 0)
            {
                myStateMN.SetNewThongbao(Lap["No amount to allocate"]);
                return;
            }

            decimal sumNk_nt = 0;
            decimal sumNk = 0;
            Ct72 lastCt = null;

            foreach (var ct in LSCt)
            {
                if (ct.Tien1 > 0)
                {
                    decimal ratio = ct.Tien1 / totalTien;
                    ct.Nk_nt = Math.Round(CurPh.T_nk_nt * ratio, M_ROUND_NT);
                    ct.Nk = NotMaNt0Voucher ? Math.Round(CurPh.T_nk * ratio, M_ROUND) : ct.Nk_nt;
                    sumNk_nt += ct.Nk_nt;
                    sumNk += ct.Nk;
                    lastCt = ct;
                }
            }

            if (lastCt != null)
            {
                lastCt.Nk_nt += CurPh.T_nk_nt - sumNk_nt;
                lastCt.Nk += (NotMaNt0Voucher ? CurPh.T_nk : CurPh.T_nk_nt) - sumNk;
            }

            foreach (var ct in LSCt)
            {
                if (ct.Tien1 > 0)
                {
                    ct.Tien_nt2 = ct.Tien_nt1 + ct.Nk_nt + ct.Db_nt;
                    ct.Tien2 = ct.Tien1 + ct.Nk + ct.Db;
                    ct.Gia_nt2 = Math.Round((ct.So_luong > 0 ? ct.Tien_nt2 / ct.So_luong : 0), M_ROUND_GIA);
                    ct.Gia2 = Math.Round((ct.So_luong > 0 ? ct.Tien2 / ct.So_luong : 0), M_ROUND_GIA);
                }
            }
            
            StateHasChanged();
        }

        private async Task btnPhanBoDB_Click()
        {
            if (!SD_THUE_DB)
            {
                myStateMN.SetNewThongbao(Lap["Declared without Special Consumption Tax (SCT)"] + " !");
                return;
            }
            if (CurPh.T_db == 0 && CurPh.T_db_nt == 0)
            {
                myStateMN.SetNewThongbao(Lap["No SCT tax to allocate"]);
                return;
            }
            if (CurPh.T_db == 0 && CurPh.T_db_nt != 0)
            {
                CurPh.T_db = Math.Round(CurPh.T_db_nt * CurPh.Ty_gia_hq, M_ROUND);
            }
            decimal totalTien = LSCt.Sum(x => x.Tien_nt1 + x.Nk_nt);
            if (totalTien == 0)
            {
                myStateMN.SetNewThongbao(Lap["No amount to allocate"]);
                return;
            }

            decimal sumDb_nt = 0;
            decimal sumDb = 0;
            Ct72 lastCt = null;

            foreach (var ct in LSCt)
            {
                if (ct.Tien_nt1 + ct.Nk_nt > 0)
                {
                    decimal ratio = (ct.Tien_nt1 + ct.Nk_nt) / totalTien;
                    ct.Db_nt = Math.Round(CurPh.T_db_nt * ratio, M_ROUND_NT);
                    ct.Db = NotMaNt0Voucher ? Math.Round(CurPh.T_db * ratio, M_ROUND) : ct.Db_nt;
                    sumDb_nt += ct.Db_nt;
                    sumDb += ct.Db;
                    lastCt = ct;
                }
            }

            if (lastCt != null)
            {
                lastCt.Db_nt += CurPh.T_db_nt - sumDb_nt;
                lastCt.Db += (NotMaNt0Voucher ? CurPh.T_db : CurPh.T_db_nt) - sumDb;
            }

            foreach (var ct in LSCt)
            {
                if (ct.Tien_nt1 + ct.Nk_nt > 0)
                {
                    ct.Tien_nt2 = Math.Round(ct.Tien_nt1 + ct.Nk_nt + ct.Db_nt, M_ROUND_NT);
                    ct.Tien2 = Math.Round(ct.Tien1 + ct.Nk + ct.Db, M_ROUND);
                    ct.Gia_nt2 = Math.Round((ct.So_luong > 0 ? ct.Tien_nt2 / ct.So_luong : 0),M_ROUND_GIA_NT);
                    ct.Gia2 = Math.Round((ct.So_luong > 0 ? ct.Tien2 / ct.So_luong : 0), M_ROUND_GIA);
                }
            }

            StateHasChanged();
        }
        private async Task btnPhanbo_Click()
        {
            if (CurPh.T_cp_nt == 0 && CurPh.T_cp == 0)
            {
                myStateMN.SetNewThongbao(Lap["No charges to allocate"]);
                return;
            }
            if (CurPh.T_cp == 0 && CurPh.T_cp_nt != 0)
            {
                CurPh.T_cp = Math.Round(CurPh.T_cp_nt * CurPh.Ty_gia, M_ROUND);
            }
            
            decimal totalBase = 0;
            
            if (CurPh.Loai_pb == "1")
            {
                totalBase = LSCt.Sum(x => x.Tien_nt0);
            }
            else if (CurPh.Loai_pb == "2")
            {
                totalBase = LSCt.Sum(x => x.So_luong);
            }
            
            if (totalBase == 0)
            {
                myStateMN.SetNewThongbao(Lap["No base to allocate"]);
                return;
            }
            
            foreach (var ct in LSCt)
            {
                decimal baseValue = CurPh.Loai_pb == "1" ? ct.Tien_nt0 : ct.So_luong;
                decimal ratio = baseValue / totalBase;
                
                ct.Cp_nt = Math.Round(CurPh.T_cp_nt * ratio, M_ROUND_NT);
                ct.Cp = Math.Round(ct.Cp_nt * CurPh.Ty_gia, M_ROUND);
                
                if (ct.So_luong > 0)
                {
                    ct.Gia_nt = ct.Tien_nt0 + ct.Cp_nt / ct.So_luong;
                    ct.Gia = ct.Tien0 + ct.Cp / ct.So_luong;
                }
                
                ct.Tien_nt = ct.Tien_nt0 + ct.Cp_nt;
                ct.Tien = ct.Tien0 + ct.Cp;
            }
            
            StateHasChanged();
        }        
       
        private void ChkSua_tien_Click(bool newValue)
        {
            CurPh.Sua_tien = newValue ? 1 : 0;
            StateHasChanged();
        }
        
        private void ChkTao_phieu_Click(bool newValue)
        {
            CurPh.Tao_pc = newValue ? 1 : 0;
            StateHasChanged();
        }
        #endregion
        
        #region Validation & Save
        private bool ValidateVoucher()
        {
            if (string.IsNullOrEmpty(CurPh.Ma_kh))
            {
                myStateMN.SetNewThongbao(Lap["Supplier code is required"]);
                return false;
            }
            
            if (string.IsNullOrEmpty(CurPh.Ma_nx))
            {
                myStateMN.SetNewThongbao(Lap["Credit account is required"]);
                return false;
            }
            
            if (string.IsNullOrEmpty(CurPh.Ma_qs))
            {
                myStateMN.SetNewThongbao(Lap["Book code is required"]);
                return false;
            }
            
            if (LSCt.Count == 0)
            {
                myStateMN.SetNewThongbao(Lap["No detail data"]);
                return false;
            }
            
            return true;
        }
        string xmlpath = "";
        private async Task<bool> SaveVoucher()
        {
            if (!SisData.Lib.CheckValidNgayKs(mySysvar, myDb, CurPh.Ngay_ct, Ma_ct))
            {
                myStateMN.SetNewThongbao(Lap["Data is locked, cannot be edited!"]);
                return false;
            }

            if (!await CheckValid())
                return false;
            if (GridCT != null)
                await GridCT.SaveChangesAsync();
            if (GridCTGT != null)
                await GridCTGT?.SaveChangesAsync();

            if (LSCt.Count <= 0)
            {
                Message = Lap["No details entered"] + " !";
                return false;
            }
            if(Loai_tg == "1")
            {
                CurPh.Ty_giaf = CurPh.Ty_gia;
                CurPh.Ty_gia_hqf = CurPh.Ty_gia_hq;
            }
            UpdateTotalHT();
            ParentReload = true;
            CurPh.Date = DateTime.Now.Date;
            CurPh.Time = DateTime.Now.ToString("HH:mm:ss");
            if (curUser != null)
                CurPh.User_id = curUser.User_id.ToString();
            CurPh.Ma_dvcs = myStateMN.GetMa_Dvcs();
            foreach (var row in LSCt)
            {
                row.Ngay_ct = CurPh.Ngay_ct;
                row.So_ct = CurPh.So_ct;
                row.Ma_ct = CurPh.Ma_ct;

                decimal tien_nt0 = row.Tien_nt0;
                decimal cp_nt = row.Cp_nt;
                decimal tien0 = row.Tien0;
                decimal cp = row.Cp;

                decimal tien_nt = Math.Round(tien_nt0 + cp_nt + row.Nk_nt + row.Db_nt, M_ROUND_NT);
                decimal tien = Math.Round(tien0 + cp + row.Nk + row.Db, M_ROUND);

                row.Tien = tien;
                row.Tien_nt = tien_nt;
                row.Tien0 = tien0;
                row.Tien_nt0 = tien_nt0;

                decimal so_luong = row.So_luong;
                decimal gia_nt0 = row.Gia_nt0;
                decimal gia0 = row.Gia0;

                decimal gia_nt = 0m;
                decimal gia = 0m;

                if (so_luong > 0m)
                {
                    gia_nt = gia_nt0 + Math.Round(tien_nt / so_luong, M_ROUND_GIA_NT);
                    gia = gia0 + Math.Round(tien / so_luong, M_ROUND_GIA);
                }

                row.Gia = gia;
                row.Gia_nt = gia_nt;
                if (row.He_so1 != new decimal(0))
                {
                    row.So_luong1 = Converts.Round(row.So_luong * row.He_so1, M_IP_SL);
                }
                else
                {
                    row.So_luong1 = row.So_luong;
                }
            }
            // Xóa các dòng có So_ct0, Kh_mau_hd, So_seri0 null hoặc trống
            LSCtgt.RemoveAll(x => string.IsNullOrWhiteSpace(x.So_ct0) &&
                                  string.IsNullOrWhiteSpace(x.Kh_mau_hd) &&
                                  string.IsNullOrWhiteSpace(x.So_seri0) &&
                                  string.IsNullOrWhiteSpace(x.Ma_thue) &&
                                  x.T_thue == 0 &&
                                  x.T_thue_nt == 0);
            
            // Tìm item trong LSCtgt có T_thue lớn nhất và gán các giá trị vào CurPh
            if (LSCtgt.Count > 0)
            {
                var maxThueItem = LSCtgt.OrderByDescending(x => x.T_thue).FirstOrDefault();
                if (maxThueItem != null)
                {
                    CurPh.Han_tt = maxThueItem.Han_tt;
                    CurPh.So_ct0 = maxThueItem.So_ct0;
                    CurPh.Ngay_ct0 = maxThueItem.Ngay_ct0;
                    CurPh.So_seri0 = maxThueItem.So_seri0;
                }
                else
                {
                    CurPh.Han_tt = 0;
                    CurPh.So_ct0 = "";
                    CurPh.Ngay_ct0 = null;
                    CurPh.So_seri0 = "";                    
                }
            }
            //
            foreach (Ct72gt row in LSCtgt)
            {
                row.Ma_nt = CurPh.Ma_nt;
                row.Ty_gia = CurPh.Ty_gia;
                row.Ty_giaf = CurPh.Ty_giaf;
                row.Status = CurPh.Status;
            }
            if ((CurPh.Sua_tien == 0 || CurPh.Sua_tien == null) && CurPh.Ty_gia != 0m)
            {
                CanBangTien();
            }
            PhanBoThueInCT();
            if (curActionTask == ActionTask.New)
            {
                CurPh.Date0 = DateTime.Now.Date;
                CurPh.Time0 = DateTime.Now.ToString("HH:mm:ss");

                if (curUser != null)
                {
                    CurPh.User_id0 = curUser.User_id.ToString();
                }
                myDb.UpdateDataRowFromModel<Ph72>(DsTrans.Tables[0].Rows[0], CurPh);
                await myDb.UpdateRow(M_Phdbf, DsTrans.Tables[0].Rows[0], "stt_rec='" + CurPh.Stt_rec + "'", "stt_rec,row_id");


                if (LSCt.Count > 0)
                    await myDb.InsertManyRow<Ct72>(LSCt, M_Ctdbf, "stt_rec", CurPh.Stt_rec);
                if (LSCtgt.Count > 0)
                {
                    await myDb.InsertManyRow<Ct72gt>(LSCtgt, M_Ctgtdbf, "stt_rec", CurPh.Stt_rec);
                }
            }
            else if (curActionTask == ActionTask.Edit)
            {
                myDb.UpdateDataRowFromModel<Ph72>(DsTrans.Tables[0].Rows[0], CurPh);
                await myDb.UpdateRow(M_Phdbf, DsTrans.Tables[0].Rows[0], "stt_rec='" + CurPh.Stt_rec + "'", "stt_rec,row_id");


                string _saveXml = myOption.GetOptionsValue("M_SAVE_XML");
                if (LSCt.Count > 0)
                {
                    xmlpath = Path.Combine(myWebhost.WebRootPath, "upload_tmp", CurPh.Stt_rec);
                    await myDb.SaveDataTable<Ct72>(LSCt, M_Ctdbf, "stt_rec", CurPh.Stt_rec, "", xmlpath, _saveXml);

                }
                else
                {
                    string strDeleteCt = string.Format("Delete From {0} Where stt_rec='{1}'", M_Ctdbf, CurPh.Stt_rec);
                    await myDb.DeleteAsync(strDeleteCt);
                }
                if (LSCtgt.Count > 0)
                {
                    xmlpath = Path.Combine(myWebhost.WebRootPath, "upload_tmp", CurPh.Stt_rec + "CT");
                    await myDb.SaveDataTable<Ct72gt>(LSCtgt, M_Ctgtdbf, "stt_rec", CurPh.Stt_rec, "", xmlpath, _saveXml);
                }
                else
                {
                    string strDeleteCtgt = string.Format("Delete From {0} Where stt_rec='{1}'", M_Ctgtdbf, CurPh.Stt_rec);
                    await myDb.DeleteAsync(strDeleteCtgt);
                }

            }
            SaveLastPh(CurPh);
            dsCheckData = CheckData(curActionTask == ActionTask.Edit ? 0 : 1);
            bool validationFailed = false;
            if (this.dsCheckData.Tables.Count > 0)
            {
                validationFailed = false;

                foreach (DataRowView dataRowView in this.dsCheckData.Tables[0].DefaultView)
                {

                    switch (dataRowView[0].ToString())
                    {
                        case "PH01":
                            var obj = myDb.GetValue("Select chkso_ct from dmqs where ma_qs = '" + CurPh.Ma_qs.Trim() + "'");
                            M_trung_so = obj?.ToString().Trim() ?? "0";

                            if (M_trung_so.Equals("1"))
                            {
                                string _message = string.Format(Lap["The voucher number already exists, the last number is: {0}. Do you want to save this voucher"],
                                    this.GetLastSoct(this.CurPh.Ma_qs).Trim()) + " ?";

                                var confirm = await myDlSvc.ConfirmAsync(new MessageBoxOptions
                                {
                                    Text = _message,
                                    Title = Lap["Confirm"],
                                    OkButtonText = Lap["Yes"],
                                    CancelButtonText = Lap["No"],
                                    ShowIcon = true,
                                    RenderStyle = MessageBoxRenderStyle.Warning
                                });

                                if (!confirm)
                                {
                                    await JavacriptS.InvokeVoidAsync("eval", "document.getElementById('so_ct_input')?.focus()");
                                    validationFailed = true;
                                    break;

                                }
                            }
                            else if (M_trung_so.Equals("2"))
                            {
                                var confirm = await myDlSvc.ConfirmAsync(new MessageBoxOptions
                                {
                                    Text = Lap["The voucher number already exists"] + " !",
                                    Title = Lap["Notification"],
                                    OkButtonText = Lap["Yes"],
                                    ShowIcon = true,
                                    RenderStyle = MessageBoxRenderStyle.Danger
                                });
                                await JavacriptS.InvokeVoidAsync("eval", "document.getElementById('so_ct_input')?.focus()");
                                validationFailed = true;
                                break;

                            }
                            break;
                        case "PH02":
                            myStateMN.SetNewThongbao(Lap["Mã nx là tk tổng hợp, không lưu được!"]);
                            await JavacriptS.InvokeVoidAsync("eval", "document.getElementById('ma_nx_input')?.focus()");
                            validationFailed = true;
                            break;

                        // Các lỗi chi tiết (CTxx) - focus vào ô grid theo Stt_rec0
                        case "CT01":

                            {
                                int rowIndex = Convert.ToInt16(dataRowView[1]);
                                string message = dataRowView[0].ToString() switch
                                {
                                    "CT01" => Lap["Tk vật tư là tk tổng hợp, không lưu được!"],

                                    _ => "Tài khoản tổng hợp không lưu được!"
                                };

                                myStateMN.SetNewThongbao(message);

                                if (rowIndex >= 0 && rowIndex < LSCt.Count)
                                {
                                    string sttRec0 = LSCt[rowIndex].Stt_rec0;
                                    string field = dataRowView[0].ToString() switch
                                    {
                                        "CT01" => "Tk_vt_",

                                        _ => "Tk_vt_"
                                    };

                                    string js = $"var el = document.getElementById('{field}{sttRec0}'); " +
                                                $"if(el){{ var input = el.querySelector('input'); if(input) input.focus(); }}";

                                    await JavacriptS.InvokeVoidAsync("eval", js);
                                }
                                validationFailed = true;
                            }
                            break;


                    }
                }

            }

            if (validationFailed)
            {
                IsSaved = false;
                return false;
            }
            else
            {
                IsSaved = true;
            }
            if ((CurPh.Status.Equals("2") || CurPh.Status.Equals("1")) && !validationFailed && (curActionTask == ActionTask.New || curActionTask == ActionTask.Edit))
            {
                bool flag2 = false;
                string str2 = myOption.GetOptionsValue("M_TK_TK_VT");


                if (!string.IsNullOrEmpty(CurPh.Ma_nx))
                {
                    foreach (string str3 in str2.Split(','))
                    {
                        if (!string.IsNullOrWhiteSpace(str3.ToString()) && CurPh.Ma_nx.Trim().StartsWith(str3.ToString().Trim()))
                            flag2 = true;
                    }
                    if (!String.IsNullOrWhiteSpace(CurPh.Stt_rec_pc) && CurPh.Tao_pc == 0)
                    {
                        string sql = string.Format("Exec [DeleteVoucher] '{0}','{1}'", CurPh.Ma_ct_pc.Trim(), CurPh.Stt_rec_pc.Trim());
                        await myDb.DeleteAsync(sql);

                        CurPh.Stt_rec_pc = "";
                        CurPh.So_ct_pc = "";
                        CurPh.Ma_ct_pc = "";
                        CurPh.Ma_qs_pc = "";

                        myDb.UpdateDataRowFromModel<Ph72>(DsTrans.Tables[0].Rows[0], CurPh);
                        await myDb.UpdateRow(M_Phdbf, DsTrans.Tables[0].Rows[0], "stt_rec='" + CurPh.Stt_rec + "'", "stt_rec,row_id");
                    }

                    if (flag2 && CurPh.Tao_pc == 1) //&& String.IsNullOrWhiteSpace(CurPh.Stt_rec_pc) 
                    {
                        await LoadFormPC();
                    }
                    else
                    {
                        Post();
                    }

                }

            }
            return true;
        }

        private void Post()
        {
            string _poststore = (Post_store == null || Post_store.Length <= 2) ? "[Poctpnb-Post]" : Post_store[2];
            string format = string.Format("exec [dbo].{0} '{1}'", _poststore, CurPh.Stt_rec);
            myDb.ExecuteSqlQueryAsync(format);
        }
        async Task<bool> CheckValid()
        {
            bool flag = true;

            if (string.IsNullOrEmpty(CurPh.Ma_kh) && flag)
            {
                Message = Lap["Chưa vào Khách hàng!"];
                myStateMN.SetNewThongbao(Message);
                await JavacriptS.InvokeVoidAsync("eval", "document.getElementById('ma_kh_input')?.focus()");
                IsSaved = false;

                flag = false;
            }

            if (CurPh.Ngay_ct == default && flag)
            {
                Message = Lap["Chưa vào ngày hạch toán!"];
                myStateMN.SetNewThongbao(Message);
                await JavacriptS.InvokeVoidAsync("eval", "document.getElementById('ngay_ct_input')?.focus()");
                IsSaved = false;

                flag = false;
            }

            if (flag && M_ngay_lct == "1")
            {
                if (CurPh.Ngay_lct == default)
                {
                    Message = Lap["Chưa vào ngày lập px!"];
                    myStateMN.SetNewThongbao(Message);
                    await JavacriptS.InvokeVoidAsync("eval", "document.getElementById('ngay_lct_input')?.focus()");
                    IsSaved = false;

                    flag = false;
                }
            }

            if (string.IsNullOrEmpty(CurPh.Ma_qs) && flag)
            {
                Message = Lap["Chưa vào quyển chứng từ!"];
                myStateMN.SetNewThongbao(Message);
                await JavacriptS.InvokeVoidAsync("eval", "document.getElementById('ngay_lct_input')?.focus()");
                IsSaved = false;

                flag = false;
            }

            if (string.IsNullOrEmpty(CurPh.Ma_nx) && flag)
            {
                Message = Lap["Chưa vào tài khoản có!"];
                myStateMN.SetNewThongbao(Message);
                await JavacriptS.InvokeVoidAsync("eval", "document.getElementById('ma_nx_input')?.focus()");
                IsSaved = false;

                flag = false;
            }

            if (LSCt.Count == 0 && flag)
            {
                Message = Lap["Chưa vào chi tiết vật tư, không lưu được!"];
                IsSaved = false;

                myStateMN.SetNewThongbao(Message);
                flag = false;
            }
            if (LSCt.Count > 0 && flag)
            {
                decimal t_cp_nt = LSCt.Sum(a => a.Cp_nt);
                decimal t_cp = LSCt.Sum(a => a.Cp);

                if (t_cp_nt != CurPh.T_cp_nt || t_cp != CurPh.T_cp)
                {
                    Message = Lap["The total cost differs from the sum of material costs"];
                    myStateMN.SetNewThongbao(Message);
                    IsSaved = false;
                    flag = false;
                }

            }

            for (int i = 0; i < LSCt.Count && flag; i++)
            {
                var row = LSCt[i];


                if (string.IsNullOrEmpty(row.Ma_vt))
                {

                    Message = Lap["Chưa nhập mã vật tư!"];
                    myStateMN.SetNewThongbao(Message);
                    await JavacriptS.InvokeVoidAsync("eval", "document.getElementById('ma_vt_input')?.focus()");
                    IsSaved = false;

                    flag = false;
                }
                else if (string.IsNullOrEmpty(row.Ma_kho_i))
                {

                    Message = Lap["Chưa nhập mã kho!"];
                    myStateMN.SetNewThongbao(Message);
                    await JavacriptS.InvokeVoidAsync("eval", "document.getElementById('ma_kho_i_input')?.focus()");
                    IsSaved = false;

                    flag = false;
                }

                else if (string.IsNullOrEmpty(row.Tk_vt))
                {

                    Message = Lap["Chưa nhập tài khoản nợ!"];
                    myStateMN.SetNewThongbao(Message);
                    await JavacriptS.InvokeVoidAsync("eval", "document.getElementById('tk_vt_input')?.focus()");
                    IsSaved = false;

                    flag = false;
                }

            }
            
            return flag;
        }
        public void SaveLastPh(Ph72 _ph)
        {
            LastPh.Stt_rec = _ph.Stt_rec;
            LastPh.Ma_nt = _ph.Ma_nt;
            LastPh.Ma_qs = _ph.Ma_qs;
            LastPh.Ma_gd = _ph.Ma_gd;
            LastPh.Ngay_ct = _ph.Ngay_ct;
            LastPh.Status = _ph.Status;
        }
        private void CanBangTien()
        {
            if (CurPh == null) return;

            decimal ty_gia = CurPh.Ty_gia > 0 ? CurPh.Ty_gia : 1m;            
            decimal sum_tien_nt0_cp = LSCt.Where(x => x.Stt_rec == CurPh.Stt_rec).Sum(x => x.Tien_nt0);
            decimal sum_tien0_cp = LSCt.Where(x => x.Stt_rec == CurPh.Stt_rec).Sum(x => x.Tien0);            
            CurPh.T_tien0 = Math.Round(CurPh.T_tien_nt0 * ty_gia, M_ROUND);            
            decimal chenlech = CurPh.T_tien0 - sum_tien0_cp;

            var dongCanBu = LSCt.FirstOrDefault(x => x.Stt_rec == CurPh.Stt_rec && (x.Tien_nt0) != 0m);
            if (dongCanBu != null)
            {
                dongCanBu.Tien0 = (dongCanBu.Tien0) + chenlech;
                dongCanBu.Tien = (dongCanBu.Tien0) + dongCanBu.Cp + dongCanBu.Nk + dongCanBu.Db;
            }            
            CurPh.T_tt_nt = (CurPh.T_tien_nt0) + (CurPh.T_cp_nt) + (CurPh.T_thue_nt);
            CurPh.T_tt = (CurPh.T_tien0) + (CurPh.T_cp) + (CurPh.T_thue);            
            UpdateTotalHT();            
        }
        public DataSet CheckData(int mode)
        {
            string format = "exec [dbo].{0} @status,  @stt_rec;";
            string[] Check_Valid_Store = (curDmct == null ? "" : curDmct.Check_valid_store.Trim()).Split("|");
            string sqlcmd = (Check_Valid_Store == null || Check_Valid_Store.Length <= 8) ? string.Format(format, (object)"[Poctpnb-CheckData]") : string.Format(format, (object)Check_Valid_Store[8]);
            DataSet dsResult = myDb.LoadDataSet(sqlcmd, new Dictionary<string, object>
            {
                ["@status"] = CurPh.Status,
                ["@stt_rec"] = CurPh.Stt_rec
            });
            return dsResult;
        }

        private void PhanBoThueInCT()
        {
            if (!LSCt.Any(x => x.Stt_rec == CurPh.Stt_rec))
                return;

            string stt_rec = CurPh.Stt_rec;
            decimal ty_gia = CurPh.Ty_gia > 0 ? CurPh.Ty_gia : 1m;
            bool isVND = string.Equals(CurPh.Ma_nt?.Trim(), M_ma_nt0?.Trim(), StringComparison.OrdinalIgnoreCase);
            decimal tong_thue_nt;
            decimal tong_thue;
            tong_thue_nt = LSCtgt
                .Where(x => x.Stt_rec == stt_rec)
                .Sum(x => x.T_thue_nt);

            tong_thue = LSCtgt
                .Where(x => x.Stt_rec == stt_rec)
                .Sum(x => x.T_thue);

            decimal tong_co_so_nt = LSCt.Where(x => x.Stt_rec == stt_rec).Sum(x => (x.Tien_nt0 + x.Cp_nt));
            decimal tong_co_so = LSCt.Where(x => x.Stt_rec == stt_rec).Sum(x => (x.Tien0 + x.Cp));

            if (tong_co_so_nt == 0m && tong_co_so == 0m)
                return;

            decimal da_phanbo_nt = 0m;
            decimal da_phanbo = 0m;

            var listCp = LSCt.Where(x => x.Stt_rec == stt_rec).ToList();

            for (int i = 0; i < listCp.Count; i++)
            {
                var row = listCp[i];

                decimal tien_nt0 = row.Tien_nt0;
                decimal tien0 = row.Tien0;
                decimal cp_nt = row.Cp_nt;
                decimal cp = row.Cp;

                decimal co_so_nt = tien_nt0 + cp_nt;
                decimal co_so = tien0 + cp;

                decimal thue_nt;
                decimal thue;

                if (!isVND && co_so_nt == 0m)
                {
                    thue_nt = 0m;
                }
                else
                {
                    thue_nt = tong_co_so_nt == 0m
                        ? 0m
                        : Math.Round(co_so_nt / tong_co_so_nt * tong_thue_nt, M_ROUND_NT);
                }

                if (co_so == 0m)
                {
                    thue = 0m;
                }
                else
                {
                    thue = tong_co_so == 0m
                        ? 0m
                        : Math.Round(co_so / tong_co_so * tong_thue, M_ROUND);
                }

                row.Thue_nt = thue_nt;
                row.Thue = thue;

                da_phanbo_nt += thue_nt;
                da_phanbo += thue;
            }
            UpdateTotalHT();
            StateHasChanged();
        }

        public async Task LoadFormPC()
        {
            dialogTcs = new TaskCompletionSource<bool>();

            string format = string.Format("if(select count(1) from dmqs where ma_cts LIKE '%{0}%') = 1 " +
                  "select ma_qs from dmqs where ma_cts LIKE '%{1}%'", "PC1", "PC1");
            DataSet dataSet = myDb.LoadDataSet(format);
            if (dataSet != null && dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
            {
                CurPh.Ong_ba_pc = String.IsNullOrEmpty(CurPh.Ong_ba_pc) ? CurPh.Ong_ba : CurPh.Ong_ba_pc;
                CurPh.Dien_giai_pc = String.IsNullOrEmpty(CurPh.Dien_giai_pc) ? "Thanh toán tiền hóa dơn số " + CurPh.So_ct + ", " + ToDateString(CurPh.Ngay_ct) : CurPh.Dien_giai_pc;
            }
            StateHasChanged();

            VisiblePC = true;
        }
        public string ToDateString(DateTime date)
        {
            return $"ngày {date.Day} tháng {date.Month} năm {date.Year}";
        }
        #endregion

        #region Phiếu chi
        void OnAutoCompleteMactPCChanged(ExpandoObject value)
        {

            _filter_pt = String.Format("ma_cts = '' OR ma_cts like '%{0}%'", CurPh.Ma_ct_pc);
            _filter_gd = String.Format("[status] = '1' AND ma_ct = '{0}' AND ( ma_gd = '2' OR ma_gd = '9')", CurPh.Ma_ct_pc);
            StateHasChanged();
        }
        void OnAutoCompleteQuyensoPCChanged(ExpandoObject value)
        {
            if (value == null)
                return;
            string ma_qs_pt = value.Where(a => a.Key.Equals("ma_qs")).FirstOrDefault().Value?.ToString() ?? "";
            curQuyenso = GetNewSo_ct(ma_qs_pt);
            CurPh.So_ct_pc = string.Format(curQuyenso.Ma_qs, curQuyenso.So_ct);
            StateHasChanged();
        }
        async Task InsertPC(bool flag2)
        {
            if (string.IsNullOrEmpty(CurPh.Ma_qs_pc))
            {
                Message = Lap["Chưa vào mã quyển sổ phiếu chi"];
                myStateMN.SetNewThongbao(Message);
            }
            else if (string.IsNullOrEmpty(CurPh.So_ct_pc))
            {
                Message = Lap["Chưa vào số chứng từ phiếu chi"];
                myStateMN.SetNewThongbao(Message);
            }
            else
            {
                bool _createPT1;
                string format = "EXEC SetSoct '" + CurPh.Ma_qs.Trim() + "', '" + CurPh.So_ct.Trim() + "'";
                await myDb.DeleteAsync(format);
                string newstt_recPt1 = "";
                DataTable dt = new DataTable();
                dt.Columns.Add("ma_ct", typeof(string));
                dt.Columns.Add("stt_rec", typeof(string));
                dt.Columns.Add("stt_recPC", typeof(string));
                dt.Columns.Add("ma_qs", typeof(string));
                dt.Columns.Add("so_ct", typeof(string));
                dt.Columns.Add("ma_nt", typeof(string));
                dt.Columns.Add("ty_gia", typeof(Decimal));
                dt.Columns.Add("ty_giaf", typeof(Decimal));
                dt.Columns.Add("nguoinop", typeof(string));
                dt.Columns.Add("lydonop", typeof(string));
                dt.Columns.Add("ma_gd", typeof(string));
                _createPT1 = false;
                if (flag2)
                {
                    _createPT1 = true;
                    if (string.IsNullOrWhiteSpace(CurPh.Stt_rec_pc))
                    {
                        DataRow row = dt.NewRow();
                        dt.Rows.Add(row);
                        newstt_recPt1 = GetStt_rec(CurPh.Ma_ct_pc, M_WS_ID);
                        dt.Rows[0]["ma_ct"] = CurPh.Ma_ct_pc;
                        dt.Rows[0]["stt_rec"] = CurPh.Stt_rec;
                        dt.Rows[0]["stt_recPC"] = newstt_recPt1;
                        dt.Rows[0]["ma_qs"] = CurPh.Ma_qs_pc;
                        Dmqs _qs = GetNewSo_ct(CurPh.Ma_qs_pc);
                        await InsertCTHHD(newstt_recPt1, CurPh.Ma_qs_pc, CurPh.Ma_ct_pc, _qs.So_ct);
                        string so_ct_pt = string.Format(_qs.Ma_qs, _qs.So_ct);
                        dt.Rows[0]["so_ct"] = so_ct_pt;
                        dt.Rows[0]["ma_nt"] = CurPh.Ma_nt;
                        dt.Rows[0]["ty_gia"] = CurPh.Ma_nt.Trim().Equals(M_ma_nt0) ? 1 : CurPh.Ty_gia;
                        dt.Rows[0]["ty_giaf"] = CurPh.Ma_nt.Trim().Equals(M_ma_nt0) ? 1 : CurPh.Ty_giaf;
                        dt.Rows[0]["nguoinop"] = CurPh.Ong_ba_pc;
                        dt.Rows[0]["lydonop"] = CurPh.Dien_giai_pc;
                        dt.Rows[0]["ma_gd"] = CurPh.Ma_gd_pc;
                    }
                    else
                    {
                        newstt_recPt1 = CurPh.Stt_rec_pc;
                        string sMa_ct_pc = CurPh.Ma_ct_pc;                        
                        DataRow row = dt.NewRow();
                        dt.Rows.Add(row);

                        dt.Rows[0]["ma_ct"] = CurPh.Ma_ct_pc;
                        dt.Rows[0]["stt_rec"] = CurPh.Stt_rec;
                        dt.Rows[0]["stt_recPC"] = newstt_recPt1;
                        dt.Rows[0]["ma_qs"] = CurPh.Ma_qs_pc;
                        dt.Rows[0]["so_ct"] = CurPh.So_ct_pc;
                        dt.Rows[0]["ma_nt"] = CurPh.Ma_nt;
                        dt.Rows[0]["ty_gia"] = CurPh.Ma_nt.Trim().Equals(M_ma_nt0) ? 1 : CurPh.Ty_gia;
                        dt.Rows[0]["ty_giaf"] = CurPh.Ma_nt.Trim().Equals(M_ma_nt0) ? 1 : CurPh.Ty_giaf;
                        dt.Rows[0]["nguoinop"] = CurPh.Ong_ba_pc;
                        dt.Rows[0]["lydonop"] = CurPh.Dien_giai_pc;
                        dt.Rows[0]["ma_gd"] = CurPh.Ma_gd_pc;
                    }
                }

                Post();
                if (_createPT1)
                {
                    await CreatePC1(dt);
                    Message = Lap["Tạo phiếu chi thành công"];
                    myStateMN.SetNewThongbao(Message);
                }
                VisiblePC = false;

                dialogTcs.TrySetResult(true);

                StateHasChanged();
            }

        }
        public async void btnViewPC1_Click()
        {
            if (!String.IsNullOrWhiteSpace(CurPh.Stt_rec_pc) && !String.IsNullOrWhiteSpace(CurPh.Ma_ct_pc))
                TransBaseComponent.ViewCT(CurPh.Stt_rec_pc, CurPh.Ma_ct_pc, myStateMN, myDb, mySType, myLcStore, myModal, Lap);
        }

        async Task CreatePC1(DataTable dt)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(dt.Rows[0]["stt_recPC"].ToString()))
                {
                    CurPh.Stt_rec_pc = dt.Rows[0]["stt_recPC"].ToString();
                    myDb.UpdateDataRowFromModel<Ph72>(DsTrans.Tables[0].Rows[0], CurPh);
                    await myDb.UpdateRow(M_Phdbf, DsTrans.Tables[0].Rows[0], "stt_rec='" + CurPh.Stt_rec + "'", "stt_rec,row_id");
                }
                decimal tyGia = Convert.ToDecimal(dt.Rows[0]["ty_gia"]);
                decimal tyGiaF = Convert.ToDecimal(dt.Rows[0]["ty_giaf"]);
                string _sql = string.Format(CultureInfo.InvariantCulture,
                    "exec [dbo].[Poctpnb-CREATEPC] '{0}','{1}','{2}','{3}','{4}',{5},{6},N'{7}',N'{8}','{9}','{10}'",
                    dt.Rows[0]["stt_rec"].ToString(),
                    dt.Rows[0]["stt_recPC"].ToString(),
                    dt.Rows[0]["ma_qs"].ToString().Trim(),
                    dt.Rows[0]["so_ct"].ToString().Trim(),
                    dt.Rows[0]["ma_nt"].ToString().Trim(),
                    tyGia,
                    tyGiaF,
                    dt.Rows[0]["nguoinop"].ToString(),
                    dt.Rows[0]["lydonop"].ToString(),
                    dt.Rows[0]["ma_gd"].ToString(),
                    dt.Rows[0]["ma_ct"].ToString()
                );
                await myDb.ExecuteSqlQueryAsync(_sql);

            }
            catch (Exception ex)
            {

            }
        }
        void ExitPC()
        {
            CurPh.Ma_qs_pc = "";
            CurPh.So_ct_pc = "";
            CurPh.Ma_ct_pc = "";
            CurPh.Stt_rec_pc = "";
            VisiblePC = false;
            dialogTcs.TrySetResult(false);

            Post();
            StateHasChanged();

        }
        #endregion
        public override void Dispose()
        {
            Pageobj?.Dispose();
            base.Dispose();
        }
    }
}
