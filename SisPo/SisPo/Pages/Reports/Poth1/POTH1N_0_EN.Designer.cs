namespace SisPo.Pages.Reports.Poth1
{
    partial class POTH1N_0_EN
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
            DevExpress.XtraReports.UI.XRSummary xrSummary1 = new DevExpress.XtraReports.UI.XRSummary();
            this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
            this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.Detail = new DevExpress.XtraReports.UI.DetailBand();
            this.objectDataSource1 = new DevExpress.DataAccess.ObjectBinding.ObjectDataSource(this.components);
            this.ReportHeader = new DevExpress.XtraReports.UI.ReportHeaderBand();
            this.xrLabel3 = new DevExpress.XtraReports.UI.XRLabel();
            this.PageHeader = new DevExpress.XtraReports.UI.PageHeaderBand();
            this.table1 = new DevExpress.XtraReports.UI.XRTable();
            this.tableRow1 = new DevExpress.XtraReports.UI.XRTableRow();
            this.tableCell4 = new DevExpress.XtraReports.UI.XRTableCell();
            this.tableCell1 = new DevExpress.XtraReports.UI.XRTableCell();
            this.tableCell5 = new DevExpress.XtraReports.UI.XRTableCell();
            this.tableCell2 = new DevExpress.XtraReports.UI.XRTableCell();
            this.tableCell6 = new DevExpress.XtraReports.UI.XRTableCell();
            this.tableCell3 = new DevExpress.XtraReports.UI.XRTableCell();
            this.DetailReport = new DevExpress.XtraReports.UI.DetailReportBand();
            this.Detail1 = new DevExpress.XtraReports.UI.DetailBand();
            this.table3 = new DevExpress.XtraReports.UI.XRTable();
            this.tableRow3 = new DevExpress.XtraReports.UI.XRTableRow();
            this.tableCell10 = new DevExpress.XtraReports.UI.XRTableCell();
            this.tableCell7 = new DevExpress.XtraReports.UI.XRTableCell();
            this.tableCell11 = new DevExpress.XtraReports.UI.XRTableCell();
            this.tableCell8 = new DevExpress.XtraReports.UI.XRTableCell();
            this.tableCell12 = new DevExpress.XtraReports.UI.XRTableCell();
            this.tableCell9 = new DevExpress.XtraReports.UI.XRTableCell();
            this.ReportFooter = new DevExpress.XtraReports.UI.ReportFooterBand();
            this.panel1 = new DevExpress.XtraReports.UI.XRPanel();
            this.lblten_ktt2 = new DevExpress.XtraReports.UI.XRLabel();
            this.label2 = new DevExpress.XtraReports.UI.XRLabel();
            this.table4 = new DevExpress.XtraReports.UI.XRTable();
            this.tableRow4 = new DevExpress.XtraReports.UI.XRTableRow();
            this.tableCell17 = new DevExpress.XtraReports.UI.XRTableCell();
            this.tableRow5 = new DevExpress.XtraReports.UI.XRTableRow();
            this.tableCell14 = new DevExpress.XtraReports.UI.XRTableCell();
            this.tableRow6 = new DevExpress.XtraReports.UI.XRTableRow();
            this.tableCell16 = new DevExpress.XtraReports.UI.XRTableCell();
            this.table2 = new DevExpress.XtraReports.UI.XRTable();
            this.tableRow2 = new DevExpress.XtraReports.UI.XRTableRow();
            this.tableCell13 = new DevExpress.XtraReports.UI.XRTableCell();
            this.tableCell15 = new DevExpress.XtraReports.UI.XRTableCell();
            this.PageFooter = new DevExpress.XtraReports.UI.PageFooterBand();
            this.lblpageinfo2 = new DevExpress.XtraReports.UI.XRLabel();
            this.line1 = new DevExpress.XtraReports.UI.XRLine();
            this.lbltitle = new DevExpress.XtraReports.UI.XRLabel();
            ((System.ComponentModel.ISupportInitialize)(this.objectDataSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.table1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.table3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.table4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.table2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // TopMargin
            // 
            this.TopMargin.HeightF = 30F;
            this.TopMargin.Name = "TopMargin";
            // 
            // BottomMargin
            // 
            this.BottomMargin.HeightF = 7.208379F;
            this.BottomMargin.Name = "BottomMargin";
            // 
            // Detail
            // 
            this.Detail.HeightF = 0F;
            this.Detail.Name = "Detail";
            // 
            // objectDataSource1
            // 
            this.objectDataSource1.DataSource = typeof(global::SisPo.Pages.Reports.Poth1.Poth1Model);
            this.objectDataSource1.Name = "objectDataSource1";
            // 
            // ReportHeader
            // 
            this.ReportHeader.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.lbltitle,
            this.xrLabel3});
            this.ReportHeader.Name = "ReportHeader";
            // 
            // xrLabel3
            // 
            this.xrLabel3.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "\'FROM DATE: \'+ FormatString(\'{0:dd-MM-yyyy}\', [Startdate]) + \' TO DATE: \' + Forma" +
                    "tString(\'{0:dd-MM-yyyy}\', [Enddate])")});
            this.xrLabel3.Font = new DevExpress.Drawing.DXFont("Arial", 10F);
            this.xrLabel3.LocationFloat = new DevExpress.Utils.PointFloat(78.20832F, 53.33333F);
            this.xrLabel3.Name = "xrLabel3";
            this.xrLabel3.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel3.SizeF = new System.Drawing.SizeF(623.2916F, 20F);
            this.xrLabel3.StylePriority.UseFont = false;
            this.xrLabel3.StylePriority.UseTextAlignment = false;
            this.xrLabel3.Tag = "NS";
            this.xrLabel3.Text = "lblNgaydenngay";
            this.xrLabel3.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // PageHeader
            // 
            this.PageHeader.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.table1});
            this.PageHeader.HeightF = 20.00001F;
            this.PageHeader.Name = "PageHeader";
            // 
            // table1
            // 
            this.table1.AnchorVertical = DevExpress.XtraReports.UI.VerticalAnchorStyles.Bottom;
            this.table1.BackColor = System.Drawing.Color.Turquoise;
            this.table1.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.table1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.table1.Name = "table1";
            this.table1.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.tableRow1});
            this.table1.SizeF = new System.Drawing.SizeF(767.9999F, 20F);
            this.table1.StylePriority.UseBackColor = false;
            this.table1.StylePriority.UseBorders = false;
            this.table1.StylePriority.UseTextAlignment = false;
            this.table1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // tableRow1
            // 
            this.tableRow1.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.tableCell4,
            this.tableCell1,
            this.tableCell5,
            this.tableCell2,
            this.tableCell6,
            this.tableCell3});
            this.tableRow1.Name = "tableRow1";
            this.tableRow1.Weight = 0.8D;
            // 
            // tableCell4
            // 
            this.tableCell4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(242)))), ((int)(((byte)(224)))));
            this.tableCell4.BorderColor = System.Drawing.Color.Gray;
            this.tableCell4.CanGrow = false;
            this.tableCell4.Font = new DevExpress.Drawing.DXFont("Arial", 9F, DevExpress.Drawing.DXFontStyle.Bold);
            this.tableCell4.Name = "tableCell4";
            this.tableCell4.StylePriority.UseBackColor = false;
            this.tableCell4.StylePriority.UseBorderColor = false;
            this.tableCell4.StylePriority.UseFont = false;
            this.tableCell4.Text = "NO.";
            this.tableCell4.Weight = 0.16551723640374655D;
            // 
            // tableCell1
            // 
            this.tableCell1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(242)))), ((int)(((byte)(224)))));
            this.tableCell1.BorderColor = System.Drawing.Color.Gray;
            this.tableCell1.CanGrow = false;
            this.tableCell1.Font = new DevExpress.Drawing.DXFont("Arial", 9F, DevExpress.Drawing.DXFontStyle.Bold);
            this.tableCell1.Name = "tableCell1";
            this.tableCell1.StylePriority.UseBackColor = false;
            this.tableCell1.StylePriority.UseBorderColor = false;
            this.tableCell1.StylePriority.UseFont = false;
            this.tableCell1.Text = "ITEM CODE";
            this.tableCell1.Weight = 0.41379309037801426D;
            // 
            // tableCell5
            // 
            this.tableCell5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(242)))), ((int)(((byte)(224)))));
            this.tableCell5.BorderColor = System.Drawing.Color.Gray;
            this.tableCell5.CanGrow = false;
            this.tableCell5.Font = new DevExpress.Drawing.DXFont("Arial", 9F, DevExpress.Drawing.DXFontStyle.Bold);
            this.tableCell5.Name = "tableCell5";
            this.tableCell5.StylePriority.UseBackColor = false;
            this.tableCell5.StylePriority.UseBorderColor = false;
            this.tableCell5.StylePriority.UseFont = false;
            this.tableCell5.Text = "ITEM NAME";
            this.tableCell5.Weight = 1.6055172680461636D;
            // 
            // tableCell2
            // 
            this.tableCell2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(242)))), ((int)(((byte)(224)))));
            this.tableCell2.BorderColor = System.Drawing.Color.Gray;
            this.tableCell2.CanGrow = false;
            this.tableCell2.Font = new DevExpress.Drawing.DXFont("Arial", 9F, DevExpress.Drawing.DXFontStyle.Bold);
            this.tableCell2.Name = "tableCell2";
            this.tableCell2.StylePriority.UseBackColor = false;
            this.tableCell2.StylePriority.UseBorderColor = false;
            this.tableCell2.StylePriority.UseFont = false;
            this.tableCell2.Text = "UOM";
            this.tableCell2.Weight = 0.24827585870958896D;
            // 
            // tableCell6
            // 
            this.tableCell6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(242)))), ((int)(((byte)(224)))));
            this.tableCell6.BorderColor = System.Drawing.Color.Gray;
            this.tableCell6.CanGrow = false;
            this.tableCell6.Font = new DevExpress.Drawing.DXFont("Arial", 9F, DevExpress.Drawing.DXFontStyle.Bold);
            this.tableCell6.Name = "tableCell6";
            this.tableCell6.StylePriority.UseBackColor = false;
            this.tableCell6.StylePriority.UseBorderColor = false;
            this.tableCell6.StylePriority.UseFont = false;
            this.tableCell6.Text = "QUANTITY";
            this.tableCell6.Weight = 0.3310344880202718D;
            // 
            // tableCell3
            // 
            this.tableCell3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(242)))), ((int)(((byte)(224)))));
            this.tableCell3.BorderColor = System.Drawing.Color.Gray;
            this.tableCell3.CanGrow = false;
            this.tableCell3.Font = new DevExpress.Drawing.DXFont("Arial", 9F, DevExpress.Drawing.DXFontStyle.Bold);
            this.tableCell3.Name = "tableCell3";
            this.tableCell3.StylePriority.UseBackColor = false;
            this.tableCell3.StylePriority.UseBorderColor = false;
            this.tableCell3.StylePriority.UseFont = false;
            this.tableCell3.Text = "VALUE";
            this.tableCell3.Weight = 0.41379284036570618D;
            // 
            // DetailReport
            // 
            this.DetailReport.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail1});
            this.DetailReport.DataMember = "LsData";
            this.DetailReport.DataSource = this.objectDataSource1;
            this.DetailReport.Level = 0;
            this.DetailReport.Name = "DetailReport";
            // 
            // Detail1
            // 
            this.Detail1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.table3});
            this.Detail1.HeightF = 20.83333F;
            this.Detail1.Name = "Detail1";
            // 
            // table3
            // 
            this.table3.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.table3.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.table3.Name = "table3";
            this.table3.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 2, 2, 100F);
            this.table3.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.tableRow3});
            this.table3.SizeF = new System.Drawing.SizeF(767.9999F, 20F);
            this.table3.StylePriority.UseBorders = false;
            this.table3.StylePriority.UsePadding = false;
            // 
            // tableRow3
            // 
            this.tableRow3.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.tableCell10,
            this.tableCell7,
            this.tableCell11,
            this.tableCell8,
            this.tableCell12,
            this.tableCell9});
            this.tableRow3.Name = "tableRow3";
            this.tableRow3.Weight = 0.8D;
            // 
            // tableCell10
            // 
            this.tableCell10.BorderColor = System.Drawing.Color.Gray;
            this.tableCell10.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Stt]")});
            this.tableCell10.Font = new DevExpress.Drawing.DXFont("Arial", 9F);
            this.tableCell10.Name = "tableCell10";
            this.tableCell10.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.tableCell10.Scripts.OnBeforePrint = "tableCell7_BeforePrint";
            this.tableCell10.StylePriority.UseBorderColor = false;
            this.tableCell10.StylePriority.UseFont = false;
            this.tableCell10.StylePriority.UsePadding = false;
            this.tableCell10.StylePriority.UseTextAlignment = false;
            this.tableCell10.Text = "tableCell10";
            this.tableCell10.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.tableCell10.Weight = 0.1667881254138342D;
            // 
            // tableCell7
            // 
            this.tableCell7.BorderColor = System.Drawing.Color.Gray;
            this.tableCell7.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Ma_vt]")});
            this.tableCell7.Font = new DevExpress.Drawing.DXFont("Arial", 9F);
            this.tableCell7.Name = "tableCell7";
            this.tableCell7.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.tableCell7.Scripts.OnBeforePrint = "tableCell7_BeforePrint";
            this.tableCell7.StylePriority.UseBorderColor = false;
            this.tableCell7.StylePriority.UseFont = false;
            this.tableCell7.StylePriority.UsePadding = false;
            this.tableCell7.StylePriority.UseTextAlignment = false;
            this.tableCell7.Text = "tableCell7";
            this.tableCell7.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.tableCell7.Weight = 0.41697049834865907D;
            // 
            // tableCell11
            // 
            this.tableCell11.BorderColor = System.Drawing.Color.Gray;
            this.tableCell11.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Ten_vt]")});
            this.tableCell11.Font = new DevExpress.Drawing.DXFont("Arial", 9F);
            this.tableCell11.Name = "tableCell11";
            this.tableCell11.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.tableCell11.Scripts.OnBeforePrint = "tableCell7_BeforePrint";
            this.tableCell11.StylePriority.UseBorderColor = false;
            this.tableCell11.StylePriority.UseFont = false;
            this.tableCell11.StylePriority.UsePadding = false;
            this.tableCell11.StylePriority.UseTextAlignment = false;
            this.tableCell11.Text = "tableCell11";
            this.tableCell11.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.tableCell11.Weight = 1.6178448986444489D;
            // 
            // tableCell8
            // 
            this.tableCell8.BorderColor = System.Drawing.Color.Gray;
            this.tableCell8.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Dvt]")});
            this.tableCell8.Font = new DevExpress.Drawing.DXFont("Arial", 9F);
            this.tableCell8.Name = "tableCell8";
            this.tableCell8.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.tableCell8.Scripts.OnBeforePrint = "tableCell7_BeforePrint";
            this.tableCell8.StylePriority.UseBorderColor = false;
            this.tableCell8.StylePriority.UseFont = false;
            this.tableCell8.StylePriority.UsePadding = false;
            this.tableCell8.StylePriority.UseTextAlignment = false;
            this.tableCell8.Text = "tableCell8";
            this.tableCell8.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.tableCell8.Weight = 0.25018224097516006D;
            // 
            // tableCell12
            // 
            this.tableCell12.BorderColor = System.Drawing.Color.Gray;
            this.tableCell12.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Sl_nhap]")});
            this.tableCell12.Font = new DevExpress.Drawing.DXFont("Arial", 9F);
            this.tableCell12.Name = "tableCell12";
            this.tableCell12.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.tableCell12.Scripts.OnBeforePrint = "tableCell7_BeforePrint";
            this.tableCell12.StylePriority.UseBorderColor = false;
            this.tableCell12.StylePriority.UseFont = false;
            this.tableCell12.StylePriority.UsePadding = false;
            this.tableCell12.StylePriority.UseTextAlignment = false;
            this.tableCell12.Tag = "I0";
            this.tableCell12.Text = "tableCell12";
            this.tableCell12.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.tableCell12.TextFormatString = "{0:N0}";
            this.tableCell12.Weight = 0.33357633657007985D;
            // 
            // tableCell9
            // 
            this.tableCell9.BorderColor = System.Drawing.Color.Gray;
            this.tableCell9.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Tien_nhap]")});
            this.tableCell9.Font = new DevExpress.Drawing.DXFont("Arial", 9F);
            this.tableCell9.Name = "tableCell9";
            this.tableCell9.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.tableCell9.Scripts.OnBeforePrint = "tableCell7_BeforePrint";
            this.tableCell9.StylePriority.UseBorderColor = false;
            this.tableCell9.StylePriority.UseFont = false;
            this.tableCell9.StylePriority.UsePadding = false;
            this.tableCell9.StylePriority.UseTextAlignment = false;
            this.tableCell9.Tag = "I0";
            this.tableCell9.Text = "tableCell9";
            this.tableCell9.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.tableCell9.TextFormatString = "{0:N0}";
            this.tableCell9.Weight = 0.41697014472542526D;
            // 
            // ReportFooter
            // 
            this.ReportFooter.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.panel1});
            this.ReportFooter.HeightF = 202.0834F;
            this.ReportFooter.Name = "ReportFooter";
            // 
            // panel1
            // 
            this.panel1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.lblten_ktt2,
            this.label2,
            this.table4,
            this.table2});
            this.panel1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.panel1.Name = "panel1";
            this.panel1.SizeF = new System.Drawing.SizeF(767.9999F, 202.0834F);
            // 
            // lblten_ktt2
            // 
            this.lblten_ktt2.Font = new DevExpress.Drawing.DXFont("Arial", 10F, DevExpress.Drawing.DXFontStyle.Bold);
            this.lblten_ktt2.LocationFloat = new DevExpress.Utils.PointFloat(403.0002F, 182.0834F);
            this.lblten_ktt2.Name = "lblten_ktt2";
            this.lblten_ktt2.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblten_ktt2.SizeF = new System.Drawing.SizeF(364.9998F, 20.00002F);
            this.lblten_ktt2.StylePriority.UseFont = false;
            this.lblten_ktt2.StylePriority.UseTextAlignment = false;
            this.lblten_ktt2.Text = "lblten_ktt";
            this.lblten_ktt2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.LocationFloat = new DevExpress.Utils.PointFloat(0F, 63.00001F);
            this.label2.Multiline = true;
            this.label2.Name = "label2";
            this.label2.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.label2.SizeF = new System.Drawing.SizeF(365F, 40F);
            this.label2.StylePriority.UseTextAlignment = false;
            this.label2.Tag = "NS";
            this.label2.Text = "PREPARED BY\r\n(Signature, full name)";
            this.label2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // table4
            // 
            this.table4.LocationFloat = new DevExpress.Utils.PointFloat(403.0002F, 43F);
            this.table4.Name = "table4";
            this.table4.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.tableRow4,
            this.tableRow5,
            this.tableRow6});
            this.table4.SizeF = new System.Drawing.SizeF(364.4997F, 60.00001F);
            // 
            // tableRow4
            // 
            this.tableRow4.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.tableCell17});
            this.tableRow4.Name = "tableRow4";
            this.tableRow4.Weight = 0.8D;
            // 
            // tableCell17
            // 
            this.tableCell17.Font = new DevExpress.Drawing.DXFont("Arial", 10F);
            this.tableCell17.Name = "tableCell17";
            this.tableCell17.StylePriority.UseFont = false;
            this.tableCell17.StylePriority.UseTextAlignment = false;
            this.tableCell17.Tag = "NS";
            this.tableCell17.Text = "Date: ..............................";
            this.tableCell17.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.tableCell17.Weight = 3D;
            // 
            // tableRow5
            // 
            this.tableRow5.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.tableCell14});
            this.tableRow5.Name = "tableRow5";
            this.tableRow5.Weight = 0.79999999999999993D;
            // 
            // tableCell14
            // 
            this.tableCell14.Multiline = true;
            this.tableCell14.Name = "tableCell14";
            this.tableCell14.StylePriority.UseTextAlignment = false;
            this.tableCell14.Tag = "NS";
            this.tableCell14.Text = "CHIEF ACCOUNTANT";
            this.tableCell14.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.tableCell14.Weight = 3D;
            // 
            // tableRow6
            // 
            this.tableRow6.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.tableCell16});
            this.tableRow6.Name = "tableRow6";
            this.tableRow6.Weight = 0.79999999999999993D;
            // 
            // tableCell16
            // 
            this.tableCell16.Name = "tableCell16";
            this.tableCell16.StylePriority.UseTextAlignment = false;
            this.tableCell16.Tag = "NS";
            this.tableCell16.Text = "(Signature, full name)";
            this.tableCell16.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.tableCell16.Weight = 3D;
            // 
            // table2
            // 
            this.table2.BorderColor = System.Drawing.Color.Gray;
            this.table2.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Right | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.table2.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.table2.Name = "table2";
            this.table2.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 2, 2, 100F);
            this.table2.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.tableRow2});
            this.table2.SizeF = new System.Drawing.SizeF(767.9999F, 20F);
            this.table2.StylePriority.UseBorderColor = false;
            this.table2.StylePriority.UseBorders = false;
            this.table2.StylePriority.UsePadding = false;
            // 
            // tableRow2
            // 
            this.tableRow2.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.tableCell13,
            this.tableCell15});
            this.tableRow2.Name = "tableRow2";
            this.tableRow2.Weight = 0.8D;
            // 
            // tableCell13
            // 
            this.tableCell13.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.tableCell13.Font = new DevExpress.Drawing.DXFont("Arial", 9F, DevExpress.Drawing.DXFontStyle.Bold);
            this.tableCell13.Name = "tableCell13";
            this.tableCell13.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.tableCell13.StylePriority.UseBorders = false;
            this.tableCell13.StylePriority.UseFont = false;
            this.tableCell13.StylePriority.UsePadding = false;
            this.tableCell13.StylePriority.UseTextAlignment = false;
            this.tableCell13.Text = "TOTAL:";
            this.tableCell13.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.tableCell13.Weight = 3.29424682852713D;
            // 
            // tableCell15
            // 
            this.tableCell15.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "sumSum([LsData].[Tien_nhap])")});
            this.tableCell15.Font = new DevExpress.Drawing.DXFont("Arial", 9F, DevExpress.Drawing.DXFontStyle.Bold);
            this.tableCell15.Name = "tableCell15";
            this.tableCell15.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.tableCell15.StylePriority.UseFont = false;
            this.tableCell15.StylePriority.UsePadding = false;
            this.tableCell15.StylePriority.UseTextAlignment = false;
            xrSummary1.FormatString = "N0";
            xrSummary1.IgnoreNullValues = true;
            xrSummary1.Running = DevExpress.XtraReports.UI.SummaryRunning.Report;
            this.tableCell15.Summary = xrSummary1;
            this.tableCell15.Text = "tableCell15";
            this.tableCell15.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.tableCell15.Weight = 0.49315066517980743D;
            // 
            // PageFooter
            // 
            this.PageFooter.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.lblpageinfo2,
            this.line1});
            this.PageFooter.HeightF = 45.83327F;
            this.PageFooter.Name = "PageFooter";
            // 
            // lblpageinfo2
            // 
            this.lblpageinfo2.Font = new DevExpress.Drawing.DXFont("Arial", 9F);
            this.lblpageinfo2.LocationFloat = new DevExpress.Utils.PointFloat(0F, 9.999974F);
            this.lblpageinfo2.Name = "lblpageinfo2";
            this.lblpageinfo2.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblpageinfo2.SizeF = new System.Drawing.SizeF(515.8334F, 25F);
            this.lblpageinfo2.StylePriority.UseTextAlignment = false;
            this.lblpageinfo2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // line1
            // 
            this.line1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 6.958326F);
            this.line1.Name = "line1";
            this.line1.SizeF = new System.Drawing.SizeF(767.9999F, 3.041649F);
            // 
            // lbltitle
            // 
            this.lbltitle.Font = new DevExpress.Drawing.DXFont("Microsoft Sans Serif", 14.25F, DevExpress.Drawing.DXFontStyle.Bold, DevExpress.Drawing.DXGraphicsUnit.Point, new DevExpress.Drawing.DXFontAdditionalProperty[] {
            new DevExpress.Drawing.DXFontAdditionalProperty("GdiCharSet", ((byte)(0)))});
            this.lbltitle.LocationFloat = new DevExpress.Utils.PointFloat(0F, 28.33333F);
            this.lbltitle.Name = "lbltitle";
            this.lbltitle.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lbltitle.SizeF = new System.Drawing.SizeF(771F, 25F);
            this.lbltitle.StylePriority.UseFont = false;
            this.lbltitle.StylePriority.UseTextAlignment = false;
            this.lbltitle.Tag = "NS";
            this.lbltitle.Text = "lbltitle";
            this.lbltitle.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // POTH1N_0_EN
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.TopMargin,
            this.BottomMargin,
            this.Detail,
            this.ReportHeader,
            this.PageHeader,
            this.DetailReport,
            this.ReportFooter,
            this.PageFooter});
            this.ComponentStorage.AddRange(new System.ComponentModel.IComponent[] {
            this.objectDataSource1});
            this.DataSource = this.objectDataSource1;
            this.Font = new DevExpress.Drawing.DXFont("Arial", 9.75F);
            this.Margins = new DevExpress.Drawing.DXMargins(27F, 29F, 30F, 7.208379F);
            this.PageHeight = 1169;
            this.PageWidth = 827;
            this.PaperKind = DevExpress.Drawing.Printing.DXPaperKind.A4;
            this.Version = "24.2";
            ((System.ComponentModel.ISupportInitialize)(this.objectDataSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.table1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.table3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.table4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.table2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        #endregion

        private DevExpress.XtraReports.UI.TopMarginBand TopMargin;
        private DevExpress.XtraReports.UI.BottomMarginBand BottomMargin;
        private DevExpress.XtraReports.UI.DetailBand Detail;
        private DevExpress.DataAccess.ObjectBinding.ObjectDataSource objectDataSource1;
        private DevExpress.XtraReports.UI.ReportHeaderBand ReportHeader;
        private DevExpress.XtraReports.UI.PageHeaderBand PageHeader;
        private DevExpress.XtraReports.UI.XRTable table1;
        private DevExpress.XtraReports.UI.XRTableRow tableRow1;
        private DevExpress.XtraReports.UI.XRTableCell tableCell4;
        private DevExpress.XtraReports.UI.XRTableCell tableCell1;
        private DevExpress.XtraReports.UI.XRTableCell tableCell5;
        private DevExpress.XtraReports.UI.XRTableCell tableCell2;
        private DevExpress.XtraReports.UI.XRTableCell tableCell6;
        private DevExpress.XtraReports.UI.XRTableCell tableCell3;
        private DevExpress.XtraReports.UI.DetailReportBand DetailReport;
        private DevExpress.XtraReports.UI.DetailBand Detail1;
        private DevExpress.XtraReports.UI.XRTable table3;
        private DevExpress.XtraReports.UI.XRTableRow tableRow3;
        private DevExpress.XtraReports.UI.XRTableCell tableCell10;
        private DevExpress.XtraReports.UI.XRTableCell tableCell7;
        private DevExpress.XtraReports.UI.XRTableCell tableCell11;
        private DevExpress.XtraReports.UI.XRTableCell tableCell8;
        private DevExpress.XtraReports.UI.XRTableCell tableCell12;
        private DevExpress.XtraReports.UI.XRTableCell tableCell9;
        private DevExpress.XtraReports.UI.ReportFooterBand ReportFooter;
        private DevExpress.XtraReports.UI.XRPanel panel1;
        private DevExpress.XtraReports.UI.XRLabel lblten_ktt2;
        private DevExpress.XtraReports.UI.XRLabel label2;
        private DevExpress.XtraReports.UI.XRTable table4;
        private DevExpress.XtraReports.UI.XRTableRow tableRow4;
        private DevExpress.XtraReports.UI.XRTableCell tableCell17;
        private DevExpress.XtraReports.UI.XRTableRow tableRow5;
        private DevExpress.XtraReports.UI.XRTableCell tableCell14;
        private DevExpress.XtraReports.UI.XRTableRow tableRow6;
        private DevExpress.XtraReports.UI.XRTableCell tableCell16;
        private DevExpress.XtraReports.UI.XRTable table2;
        private DevExpress.XtraReports.UI.XRTableRow tableRow2;
        private DevExpress.XtraReports.UI.XRTableCell tableCell13;
        private DevExpress.XtraReports.UI.XRTableCell tableCell15;
        private DevExpress.XtraReports.UI.PageFooterBand PageFooter;
        private DevExpress.XtraReports.UI.XRLabel lblpageinfo2;
        private DevExpress.XtraReports.UI.XRLine line1;
        private DevExpress.XtraReports.UI.XRLabel xrLabel3;
        private DevExpress.XtraReports.UI.XRLabel lbltitle;
    }
}
