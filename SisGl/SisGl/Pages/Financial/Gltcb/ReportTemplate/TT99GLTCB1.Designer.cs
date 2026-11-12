namespace SisGl.Pages.Reports.ReportTemplate
{
    partial class TT99GLTCB1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
            this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.pageInfo2 = new DevExpress.XtraReports.UI.XRPageInfo();
            this.ReportHeader = new DevExpress.XtraReports.UI.ReportHeaderBand();
            this.xrLabel3 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel1 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel6 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel5 = new DevExpress.XtraReports.UI.XRLabel();
            this.label1 = new DevExpress.XtraReports.UI.XRLabel();
            this.label5 = new DevExpress.XtraReports.UI.XRLabel();
            this.label6 = new DevExpress.XtraReports.UI.XRLabel();
            this.lblDia_chi = new DevExpress.XtraReports.UI.XRLabel();
            this.lblTen_cty = new DevExpress.XtraReports.UI.XRLabel();
            this.lblma_thue = new DevExpress.XtraReports.UI.XRLabel();
            this.table2 = new DevExpress.XtraReports.UI.XRTable();
            this.tableRow3 = new DevExpress.XtraReports.UI.XRTableRow();
            this.tableCell9 = new DevExpress.XtraReports.UI.XRTableCell();
            this.tableCell11 = new DevExpress.XtraReports.UI.XRTableCell();
            this.tableCell12 = new DevExpress.XtraReports.UI.XRTableCell();
            this.tableCell23 = new DevExpress.XtraReports.UI.XRTableCell();
            this.tableCell24 = new DevExpress.XtraReports.UI.XRTableCell();
            this.lbltitle = new DevExpress.XtraReports.UI.XRLabel();
            this.Detail = new DevExpress.XtraReports.UI.DetailBand();
            this.DetailReport = new DevExpress.XtraReports.UI.DetailReportBand();
            this.Detail1 = new DevExpress.XtraReports.UI.DetailBand();
            this.table3 = new DevExpress.XtraReports.UI.XRTable();
            this.tableRow4 = new DevExpress.XtraReports.UI.XRTableRow();
            this.tableCell26 = new DevExpress.XtraReports.UI.XRTableCell();
            this.tableCell28 = new DevExpress.XtraReports.UI.XRTableCell();
            this.tableCell29 = new DevExpress.XtraReports.UI.XRTableCell();
            this.tableCell40 = new DevExpress.XtraReports.UI.XRTableCell();
            this.tableCell41 = new DevExpress.XtraReports.UI.XRTableCell();
            this.objectDataSource1 = new DevExpress.DataAccess.ObjectBinding.ObjectDataSource(this.components);
            this.Title = new DevExpress.XtraReports.UI.XRControlStyle();
            this.DetailCaption1 = new DevExpress.XtraReports.UI.XRControlStyle();
            this.DetailData1 = new DevExpress.XtraReports.UI.XRControlStyle();
            this.DetailCaption2 = new DevExpress.XtraReports.UI.XRControlStyle();
            this.DetailData2 = new DevExpress.XtraReports.UI.XRControlStyle();
            this.DetailData3_Odd = new DevExpress.XtraReports.UI.XRControlStyle();
            this.PageInfo = new DevExpress.XtraReports.UI.XRControlStyle();
            ((System.ComponentModel.ISupportInitialize)(this.table2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.table3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.objectDataSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // TopMargin
            // 
            this.TopMargin.HeightF = 50.69444F;
            this.TopMargin.Name = "TopMargin";
            // 
            // BottomMargin
            // 
            this.BottomMargin.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.pageInfo2});
            this.BottomMargin.HeightF = 52.73146F;
            this.BottomMargin.Name = "BottomMargin";
            // 
            // pageInfo2
            // 
            this.pageInfo2.LocationFloat = new DevExpress.Utils.PointFloat(325F, 0F);
            this.pageInfo2.Name = "pageInfo2";
            this.pageInfo2.SizeF = new System.Drawing.SizeF(425F, 23F);
            this.pageInfo2.StyleName = "PageInfo";
            this.pageInfo2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            this.pageInfo2.TextFormatString = "Trang {0} / {1}";
            // 
            // ReportHeader
            // 
            this.ReportHeader.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel3,
            this.xrLabel1,
            this.xrLabel6,
            this.xrLabel5,
            this.label1,
            this.label5,
            this.label6,
            this.lblDia_chi,
            this.lblTen_cty,
            this.lblma_thue,
            this.table2,
            this.lbltitle});
            this.ReportHeader.HeightF = 199.2985F;
            this.ReportHeader.Name = "ReportHeader";
            // 
            // xrLabel3
            // 
            this.xrLabel3.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Tu_ngay_Den_ngay]")});
            this.xrLabel3.Font = new DevExpress.Drawing.DXFont("Arial", 9F);
            this.xrLabel3.LocationFloat = new DevExpress.Utils.PointFloat(298.7799F, 129.0554F);
            this.xrLabel3.Name = "xrLabel3";
            this.xrLabel3.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel3.SizeF = new System.Drawing.SizeF(152.1879F, 20F);
            this.xrLabel3.StylePriority.UseFont = false;
            this.xrLabel3.StylePriority.UseTextAlignment = false;
            this.xrLabel3.Text = "Từ ngày:... đến ngày.....";
            this.xrLabel3.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // xrLabel1
            // 
            this.xrLabel1.Font = new DevExpress.Drawing.DXFont("Arial", 9F, DevExpress.Drawing.DXFontStyle.Bold);
            this.xrLabel1.LocationFloat = new DevExpress.Utils.PointFloat(325F, 109.0555F);
            this.xrLabel1.Name = "xrLabel1";
            this.xrLabel1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel1.SizeF = new System.Drawing.SizeF(105.3128F, 19.99999F);
            this.xrLabel1.StylePriority.UseFont = false;
            this.xrLabel1.StylePriority.UseTextAlignment = false;
            this.xrLabel1.Text = "(Dạng đầy đủ)";
            this.xrLabel1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // xrLabel6
            // 
            this.xrLabel6.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Dvt]")});
            this.xrLabel6.Font = new DevExpress.Drawing.DXFont("Arial", 9F, DevExpress.Drawing.DXFontStyle.Bold);
            this.xrLabel6.LocationFloat = new DevExpress.Utils.PointFloat(673.3331F, 152.78F);
            this.xrLabel6.Name = "xrLabel6";
            this.xrLabel6.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel6.SizeF = new System.Drawing.SizeF(74.30792F, 20F);
            this.xrLabel6.StylePriority.UseFont = false;
            this.xrLabel6.StylePriority.UseTextAlignment = false;
            this.xrLabel6.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // xrLabel5
            // 
            this.xrLabel5.Font = new DevExpress.Drawing.DXFont("Arial", 9F);
            this.xrLabel5.LocationFloat = new DevExpress.Utils.PointFloat(585.8306F, 152.78F);
            this.xrLabel5.Name = "xrLabel5";
            this.xrLabel5.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel5.SizeF = new System.Drawing.SizeF(87.5025F, 20.00002F);
            this.xrLabel5.StylePriority.UseFont = false;
            this.xrLabel5.StylePriority.UseTextAlignment = false;
            this.xrLabel5.Text = "Đơn vị tính:";
            this.xrLabel5.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // label1
            // 
            this.label1.Font = new DevExpress.Drawing.DXFont("Arial", 9F, DevExpress.Drawing.DXFontStyle.Bold);
            this.label1.LocationFloat = new DevExpress.Utils.PointFloat(417.7779F, 19.99999F);
            this.label1.Name = "label1";
            this.label1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.label1.SizeF = new System.Drawing.SizeF(329.8631F, 20F);
            this.label1.StylePriority.UseFont = false;
            this.label1.StylePriority.UseTextAlignment = false;
            this.label1.Text = "Mẫu số: B 01a - DN";
            this.label1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // label5
            // 
            this.label5.Font = new DevExpress.Drawing.DXFont("Arial", 9F);
            this.label5.LocationFloat = new DevExpress.Utils.PointFloat(417.7778F, 39.99998F);
            this.label5.Name = "label5";
            this.label5.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.label5.SizeF = new System.Drawing.SizeF(329.8632F, 24.86115F);
            this.label5.StylePriority.UseFont = false;
            this.label5.StylePriority.UseTextAlignment = false;
            this.label5.Text = "(Ban hành theo Thông tư số 200/2014/TT-BTC";
            this.label5.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // label6
            // 
            this.label6.Font = new DevExpress.Drawing.DXFont("Arial", 9F);
            this.label6.LocationFloat = new DevExpress.Utils.PointFloat(417.7778F, 64.86114F);
            this.label6.Multiline = true;
            this.label6.Name = "label6";
            this.label6.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.label6.SizeF = new System.Drawing.SizeF(329.8636F, 19.99998F);
            this.label6.StylePriority.UseFont = false;
            this.label6.StylePriority.UseTextAlignment = false;
            this.label6.Text = "Ngày 22/12/2014 của Bộ Tài chính)";
            this.label6.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // lblDia_chi
            // 
            this.lblDia_chi.Font = new DevExpress.Drawing.DXFont("Arial", 9F);
            this.lblDia_chi.LocationFloat = new DevExpress.Utils.PointFloat(0F, 40.00003F);
            this.lblDia_chi.Multiline = true;
            this.lblDia_chi.Name = "lblDia_chi";
            this.lblDia_chi.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblDia_chi.SizeF = new System.Drawing.SizeF(417.7779F, 20F);
            this.lblDia_chi.StylePriority.UseFont = false;
            this.lblDia_chi.StylePriority.UseTextAlignment = false;
            this.lblDia_chi.Text = "Địa chỉ công ty";
            this.lblDia_chi.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // lblTen_cty
            // 
            this.lblTen_cty.Font = new DevExpress.Drawing.DXFont("Arial", 9F);
            this.lblTen_cty.LocationFloat = new DevExpress.Utils.PointFloat(0F, 19.99999F);
            this.lblTen_cty.Multiline = true;
            this.lblTen_cty.Name = "lblTen_cty";
            this.lblTen_cty.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblTen_cty.SizeF = new System.Drawing.SizeF(417.7779F, 20F);
            this.lblTen_cty.StylePriority.UseFont = false;
            this.lblTen_cty.StylePriority.UseTextAlignment = false;
            this.lblTen_cty.Text = "Tên công ty";
            this.lblTen_cty.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // lblma_thue
            // 
            this.lblma_thue.BorderColor = System.Drawing.Color.Gray;
            this.lblma_thue.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.lblma_thue.Font = new DevExpress.Drawing.DXFont("Arial", 9F);
            this.lblma_thue.LocationFloat = new DevExpress.Utils.PointFloat(417.7779F, 0F);
            this.lblma_thue.Name = "lblma_thue";
            this.lblma_thue.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblma_thue.SizeF = new System.Drawing.SizeF(329.8631F, 20F);
            this.lblma_thue.StylePriority.UseBorderColor = false;
            this.lblma_thue.StylePriority.UseBorders = false;
            this.lblma_thue.StylePriority.UseFont = false;
            this.lblma_thue.StylePriority.UseTextAlignment = false;
            this.lblma_thue.Text = "lblma_thue";
            this.lblma_thue.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // table2
            // 
            this.table2.LocationFloat = new DevExpress.Utils.PointFloat(6.357829E-05F, 172.78F);
            this.table2.Name = "table2";
            this.table2.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.tableRow3});
            this.table2.SizeF = new System.Drawing.SizeF(750.0001F, 26.51852F);
            // 
            // tableRow3
            // 
            this.tableRow3.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.tableCell9,
            this.tableCell11,
            this.tableCell12,
            this.tableCell23,
            this.tableCell24});
            this.tableRow3.Name = "tableRow3";
            this.tableRow3.Weight = 1D;
            // 
            // tableCell9
            // 
            this.tableCell9.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(242)))), ((int)(((byte)(224)))));
            this.tableCell9.BorderColor = System.Drawing.Color.Silver;
            this.tableCell9.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.tableCell9.ForeColor = System.Drawing.Color.Black;
            this.tableCell9.Name = "tableCell9";
            this.tableCell9.StyleName = "DetailCaption2";
            this.tableCell9.StylePriority.UseBackColor = false;
            this.tableCell9.StylePriority.UseBorderColor = false;
            this.tableCell9.StylePriority.UseBorders = false;
            this.tableCell9.StylePriority.UseForeColor = false;
            this.tableCell9.StylePriority.UseTextAlignment = false;
            this.tableCell9.Text = "Chỉ tiêu\t";
            this.tableCell9.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.tableCell9.Weight = 0.0816404208479247D;
            // 
            // tableCell11
            // 
            this.tableCell11.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(242)))), ((int)(((byte)(224)))));
            this.tableCell11.BorderColor = System.Drawing.Color.Silver;
            this.tableCell11.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.tableCell11.ForeColor = System.Drawing.Color.Black;
            this.tableCell11.Name = "tableCell11";
            this.tableCell11.StyleName = "DetailCaption2";
            this.tableCell11.StylePriority.UseBackColor = false;
            this.tableCell11.StylePriority.UseBorderColor = false;
            this.tableCell11.StylePriority.UseBorders = false;
            this.tableCell11.StylePriority.UseForeColor = false;
            this.tableCell11.StylePriority.UseTextAlignment = false;
            this.tableCell11.Text = "Mã số";
            this.tableCell11.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.tableCell11.Weight = 0.030216952374577884D;
            // 
            // tableCell12
            // 
            this.tableCell12.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(242)))), ((int)(((byte)(224)))));
            this.tableCell12.BorderColor = System.Drawing.Color.Silver;
            this.tableCell12.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.tableCell12.ForeColor = System.Drawing.Color.Black;
            this.tableCell12.Name = "tableCell12";
            this.tableCell12.StyleName = "DetailCaption2";
            this.tableCell12.StylePriority.UseBackColor = false;
            this.tableCell12.StylePriority.UseBorderColor = false;
            this.tableCell12.StylePriority.UseBorders = false;
            this.tableCell12.StylePriority.UseForeColor = false;
            this.tableCell12.StylePriority.UseTextAlignment = false;
            this.tableCell12.Text = "Thuyết minh";
            this.tableCell12.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.tableCell12.Weight = 0.060003149419139777D;
            // 
            // tableCell23
            // 
            this.tableCell23.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(242)))), ((int)(((byte)(224)))));
            this.tableCell23.BorderColor = System.Drawing.Color.Silver;
            this.tableCell23.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.tableCell23.ForeColor = System.Drawing.Color.Black;
            this.tableCell23.Name = "tableCell23";
            this.tableCell23.StyleName = "DetailCaption2";
            this.tableCell23.StylePriority.UseBackColor = false;
            this.tableCell23.StylePriority.UseBorderColor = false;
            this.tableCell23.StylePriority.UseBorders = false;
            this.tableCell23.StylePriority.UseForeColor = false;
            this.tableCell23.StylePriority.UseTextAlignment = false;
            this.tableCell23.Text = "Số cuối quý";
            this.tableCell23.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.tableCell23.Weight = 0.043644135642116791D;
            // 
            // tableCell24
            // 
            this.tableCell24.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(242)))), ((int)(((byte)(224)))));
            this.tableCell24.BorderColor = System.Drawing.Color.Silver;
            this.tableCell24.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.tableCell24.ForeColor = System.Drawing.Color.Black;
            this.tableCell24.Name = "tableCell24";
            this.tableCell24.StyleName = "DetailCaption2";
            this.tableCell24.StylePriority.UseBackColor = false;
            this.tableCell24.StylePriority.UseBorderColor = false;
            this.tableCell24.StylePriority.UseBorders = false;
            this.tableCell24.StylePriority.UseForeColor = false;
            this.tableCell24.StylePriority.UseTextAlignment = false;
            this.tableCell24.Text = "Số đầu năm";
            this.tableCell24.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.tableCell24.Weight = 0.0426277979822039D;
            // 
            // lbltitle
            // 
            this.lbltitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.lbltitle.LocationFloat = new DevExpress.Utils.PointFloat(6.357829E-05F, 84.86112F);
            this.lbltitle.Name = "lbltitle";
            this.lbltitle.SizeF = new System.Drawing.SizeF(747.6409F, 24.19433F);
            this.lbltitle.StyleName = "Title";
            this.lbltitle.StylePriority.UseForeColor = false;
            this.lbltitle.StylePriority.UseTextAlignment = false;
            this.lbltitle.Text = "Report Title";
            this.lbltitle.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // Detail
            // 
            this.Detail.Expanded = false;
            this.Detail.HeightF = 56F;
            this.Detail.KeepTogether = true;
            this.Detail.Name = "Detail";
            // 
            // DetailReport
            // 
            this.DetailReport.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail1});
            this.DetailReport.DataMember = "LsData";
            this.DetailReport.DataSource = this.objectDataSource1;
            this.DetailReport.Level = 0;
            this.DetailReport.Name = "DetailReport";
            this.DetailReport.Scripts.OnBeforePrint = "DetailReport_BeforePrint";
            // 
            // Detail1
            // 
            this.Detail1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.table3});
            this.Detail1.HeightF = 25F;
            this.Detail1.Name = "Detail1";
            this.Detail1.Scripts.OnBeforePrint = "Detail1_BeforePrint";
            // 
            // table3
            // 
            this.table3.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.table3.Name = "table3";
            this.table3.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.tableRow4});
            this.table3.SizeF = new System.Drawing.SizeF(750F, 25F);
            // 
            // tableRow4
            // 
            this.tableRow4.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.tableCell26,
            this.tableCell28,
            this.tableCell29,
            this.tableCell40,
            this.tableCell41});
            this.tableRow4.Name = "tableRow4";
            this.tableRow4.Weight = 11.5D;
            // 
            // tableCell26
            // 
            this.tableCell26.BorderColor = System.Drawing.Color.Silver;
            this.tableCell26.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.tableCell26.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Chi_tieu]"),
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "BackColor", "Iif([Bold] == \'1\',\'Silver\',\'Transparent\')"),
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Font.Bold", "Iif([Bold] == \'1\',1,0)")});
            this.tableCell26.ForeColor = System.Drawing.Color.Black;
            this.tableCell26.Name = "tableCell26";
            this.tableCell26.Scripts.OnBeforePrint = "tableCell26_BeforePrint";
            this.tableCell26.StyleName = "DetailData2";
            this.tableCell26.StylePriority.UseBorderColor = false;
            this.tableCell26.StylePriority.UseBorders = false;
            this.tableCell26.StylePriority.UseForeColor = false;
            this.tableCell26.Weight = 0.11931780847908559D;
            // 
            // tableCell28
            // 
            this.tableCell28.BorderColor = System.Drawing.Color.Silver;
            this.tableCell28.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.tableCell28.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Ma_so]"),
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Font.Bold", "Iif([Bold] == \'1\',True,False)"),
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "BackColor", "Iif([Bold] == \'1\',\'Silver\',\'Transparent\')")});
            this.tableCell28.Name = "tableCell28";
            this.tableCell28.Scripts.OnBeforePrint = "tableCell28_BeforePrint";
            this.tableCell28.StyleName = "DetailData2";
            this.tableCell28.StylePriority.UseBorderColor = false;
            this.tableCell28.StylePriority.UseBorders = false;
            this.tableCell28.Weight = 0.044162209503596284D;
            // 
            // tableCell29
            // 
            this.tableCell29.BorderColor = System.Drawing.Color.Silver;
            this.tableCell29.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.tableCell29.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Th_minh]"),
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "BackColor", "Iif([Bold] == \'1\',\'Silver\',\'Transparent\')")});
            this.tableCell29.Name = "tableCell29";
            this.tableCell29.StyleName = "DetailData2";
            this.tableCell29.StylePriority.UseBorderColor = false;
            this.tableCell29.StylePriority.UseBorders = false;
            this.tableCell29.Weight = 0.0876948701151194D;
            // 
            // tableCell40
            // 
            this.tableCell40.BorderColor = System.Drawing.Color.Silver;
            this.tableCell40.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.tableCell40.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Tien]"),
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Font.Bold", "Iif([Bold] == \'1\')"),
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "BackColor", "Iif([Bold] == \'1\',\'Silver\',\'Transparent\')")});
            this.tableCell40.Name = "tableCell40";
            this.tableCell40.StyleName = "DetailData2";
            this.tableCell40.StylePriority.UseBorderColor = false;
            this.tableCell40.StylePriority.UseBorders = false;
            this.tableCell40.StylePriority.UseTextAlignment = false;
            this.tableCell40.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.tableCell40.TextFormatString = "{0:N0}";
            this.tableCell40.Weight = 0.063786020167352075D;
            // 
            // tableCell41
            // 
            this.tableCell41.BorderColor = System.Drawing.Color.Silver;
            this.tableCell41.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.tableCell41.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Tien0]"),
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "BackColor", "Iif([Bold] == \'1\',\'Silver\',\'Transparent\')")});
            this.tableCell41.Name = "tableCell41";
            this.tableCell41.StyleName = "DetailData2";
            this.tableCell41.StylePriority.UseBorderColor = false;
            this.tableCell41.StylePriority.UseBorders = false;
            this.tableCell41.StylePriority.UseTextAlignment = false;
            this.tableCell41.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.tableCell41.TextFormatString = "{0:N0}";
            this.tableCell41.Weight = 0.062300672927422585D;
            // 
            // objectDataSource1
            // 
            this.objectDataSource1.DataSource = typeof(global::SisGl.Pages.Financial.Gltcb.Gltcb200Model);
            this.objectDataSource1.Name = "objectDataSource1";
            // 
            // Title
            // 
            this.Title.BackColor = System.Drawing.Color.Transparent;
            this.Title.BorderColor = System.Drawing.Color.Black;
            this.Title.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.Title.BorderWidth = 1F;
            this.Title.Font = new DevExpress.Drawing.DXFont("Arial", 14.25F);
            this.Title.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(75)))), ((int)(((byte)(75)))));
            this.Title.Name = "Title";
            this.Title.Padding = new DevExpress.XtraPrinting.PaddingInfo(6, 6, 0, 0, 100F);
            // 
            // DetailCaption1
            // 
            this.DetailCaption1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(75)))), ((int)(((byte)(75)))));
            this.DetailCaption1.BorderColor = System.Drawing.Color.White;
            this.DetailCaption1.Borders = DevExpress.XtraPrinting.BorderSide.Left;
            this.DetailCaption1.BorderWidth = 2F;
            this.DetailCaption1.Font = new DevExpress.Drawing.DXFont("Arial", 8.25F, DevExpress.Drawing.DXFontStyle.Bold);
            this.DetailCaption1.ForeColor = System.Drawing.Color.White;
            this.DetailCaption1.Name = "DetailCaption1";
            this.DetailCaption1.Padding = new DevExpress.XtraPrinting.PaddingInfo(6, 6, 0, 0, 100F);
            this.DetailCaption1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // DetailData1
            // 
            this.DetailData1.BorderColor = System.Drawing.Color.Transparent;
            this.DetailData1.Borders = DevExpress.XtraPrinting.BorderSide.Left;
            this.DetailData1.BorderWidth = 2F;
            this.DetailData1.Font = new DevExpress.Drawing.DXFont("Arial", 8.25F);
            this.DetailData1.ForeColor = System.Drawing.Color.Black;
            this.DetailData1.Name = "DetailData1";
            this.DetailData1.Padding = new DevExpress.XtraPrinting.PaddingInfo(6, 6, 0, 0, 100F);
            this.DetailData1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // DetailCaption2
            // 
            this.DetailCaption2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(111)))), ((int)(((byte)(111)))), ((int)(((byte)(111)))));
            this.DetailCaption2.BorderColor = System.Drawing.Color.White;
            this.DetailCaption2.Borders = DevExpress.XtraPrinting.BorderSide.Left;
            this.DetailCaption2.BorderWidth = 2F;
            this.DetailCaption2.Font = new DevExpress.Drawing.DXFont("Arial", 8.25F, DevExpress.Drawing.DXFontStyle.Bold);
            this.DetailCaption2.ForeColor = System.Drawing.Color.White;
            this.DetailCaption2.Name = "DetailCaption2";
            this.DetailCaption2.Padding = new DevExpress.XtraPrinting.PaddingInfo(6, 6, 0, 0, 100F);
            this.DetailCaption2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // DetailData2
            // 
            this.DetailData2.BorderColor = System.Drawing.Color.Transparent;
            this.DetailData2.Borders = DevExpress.XtraPrinting.BorderSide.Left;
            this.DetailData2.BorderWidth = 2F;
            this.DetailData2.Font = new DevExpress.Drawing.DXFont("Arial", 8.25F);
            this.DetailData2.ForeColor = System.Drawing.Color.Black;
            this.DetailData2.Name = "DetailData2";
            this.DetailData2.Padding = new DevExpress.XtraPrinting.PaddingInfo(6, 6, 0, 0, 100F);
            this.DetailData2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // DetailData3_Odd
            // 
            this.DetailData3_Odd.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(231)))), ((int)(((byte)(231)))));
            this.DetailData3_Odd.BorderColor = System.Drawing.Color.Transparent;
            this.DetailData3_Odd.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.DetailData3_Odd.BorderWidth = 1F;
            this.DetailData3_Odd.Font = new DevExpress.Drawing.DXFont("Arial", 8.25F);
            this.DetailData3_Odd.ForeColor = System.Drawing.Color.Black;
            this.DetailData3_Odd.Name = "DetailData3_Odd";
            this.DetailData3_Odd.Padding = new DevExpress.XtraPrinting.PaddingInfo(6, 6, 0, 0, 100F);
            this.DetailData3_Odd.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // PageInfo
            // 
            this.PageInfo.Font = new DevExpress.Drawing.DXFont("Arial", 8.25F, DevExpress.Drawing.DXFontStyle.Bold);
            this.PageInfo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(75)))), ((int)(((byte)(75)))));
            this.PageInfo.Name = "PageInfo";
            this.PageInfo.Padding = new DevExpress.XtraPrinting.PaddingInfo(6, 6, 0, 0, 100F);
            // 
            // TT99GLTCB1
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.TopMargin,
            this.BottomMargin,
            this.ReportHeader,
            this.Detail,
            this.DetailReport});
            this.ComponentStorage.AddRange(new System.ComponentModel.IComponent[] {
            this.objectDataSource1});
            this.DataSource = this.objectDataSource1;
            this.Font = new DevExpress.Drawing.DXFont("Arial", 9.75F);
            this.Margins = new DevExpress.Drawing.DXMargins(50F, 50F, 50.69444F, 52.73146F);
            this.StyleSheet.AddRange(new DevExpress.XtraReports.UI.XRControlStyle[] {
            this.Title,
            this.DetailCaption1,
            this.DetailData1,
            this.DetailCaption2,
            this.DetailData2,
            this.DetailData3_Odd,
            this.PageInfo});
            this.Version = "24.2";
            ((System.ComponentModel.ISupportInitialize)(this.table2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.table3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.objectDataSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        #endregion

        private DevExpress.XtraReports.UI.TopMarginBand TopMargin;
        private DevExpress.XtraReports.UI.BottomMarginBand BottomMargin;
        private DevExpress.XtraReports.UI.XRPageInfo pageInfo2;
        private DevExpress.XtraReports.UI.ReportHeaderBand ReportHeader;
        private DevExpress.XtraReports.UI.XRLabel lbltitle;
        private DevExpress.XtraReports.UI.DetailBand Detail;
        private DevExpress.XtraReports.UI.DetailReportBand DetailReport;
        private DevExpress.XtraReports.UI.XRTable table2;
        private DevExpress.XtraReports.UI.XRTableRow tableRow3;
        private DevExpress.XtraReports.UI.XRTableCell tableCell9;
        private DevExpress.XtraReports.UI.XRTableCell tableCell11;
        private DevExpress.XtraReports.UI.XRTableCell tableCell12;
        private DevExpress.XtraReports.UI.XRTableCell tableCell23;
        private DevExpress.XtraReports.UI.XRTableCell tableCell24;
        private DevExpress.XtraReports.UI.DetailBand Detail1;
        private DevExpress.XtraReports.UI.XRTable table3;
        private DevExpress.XtraReports.UI.XRTableRow tableRow4;
        private DevExpress.XtraReports.UI.XRTableCell tableCell26;
        private DevExpress.XtraReports.UI.XRTableCell tableCell28;
        private DevExpress.XtraReports.UI.XRTableCell tableCell29;
        private DevExpress.XtraReports.UI.XRTableCell tableCell40;
        private DevExpress.XtraReports.UI.XRTableCell tableCell41;
        private DevExpress.DataAccess.ObjectBinding.ObjectDataSource objectDataSource1;
        private DevExpress.XtraReports.UI.XRControlStyle Title;
        private DevExpress.XtraReports.UI.XRControlStyle DetailCaption1;
        private DevExpress.XtraReports.UI.XRControlStyle DetailData1;
        private DevExpress.XtraReports.UI.XRControlStyle DetailCaption2;
        private DevExpress.XtraReports.UI.XRControlStyle DetailData2;
        private DevExpress.XtraReports.UI.XRControlStyle DetailData3_Odd;
        private DevExpress.XtraReports.UI.XRControlStyle PageInfo;
        private DevExpress.XtraReports.UI.XRLabel label1;
        private DevExpress.XtraReports.UI.XRLabel label5;
        private DevExpress.XtraReports.UI.XRLabel label6;
        private DevExpress.XtraReports.UI.XRLabel lblDia_chi;
        private DevExpress.XtraReports.UI.XRLabel lblTen_cty;
        private DevExpress.XtraReports.UI.XRLabel lblma_thue;
        private DevExpress.XtraReports.UI.XRLabel xrLabel5;
        private DevExpress.XtraReports.UI.XRLabel xrLabel6;
        private DevExpress.XtraReports.UI.XRLabel xrLabel3;
        private DevExpress.XtraReports.UI.XRLabel xrLabel1;
    }
}
