namespace SisFx.Pages.Report.FXTHECC.V
{
    partial class FXTHECC1_SUB1
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
            this.topMarginBand1 = new DevExpress.XtraReports.UI.TopMarginBand();
            this.detailBand1 = new DevExpress.XtraReports.UI.DetailBand();
            this.table3 = new DevExpress.XtraReports.UI.XRTable();
            this.tableRow3 = new DevExpress.XtraReports.UI.XRTableRow();
            this.tableCell8 = new DevExpress.XtraReports.UI.XRTableCell();
            this.tableCell13 = new DevExpress.XtraReports.UI.XRTableCell();
            this.tableCell16 = new DevExpress.XtraReports.UI.XRTableCell();
            this.tableCell9 = new DevExpress.XtraReports.UI.XRTableCell();
            this.tableCell20 = new DevExpress.XtraReports.UI.XRTableCell();
            this.bottomMarginBand1 = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.ReportHeader = new DevExpress.XtraReports.UI.ReportHeaderBand();
            this.lbltitle4444 = new DevExpress.XtraReports.UI.XRLabel();
            this.PageHeader = new DevExpress.XtraReports.UI.PageHeaderBand();
            this.table2 = new DevExpress.XtraReports.UI.XRTable();
            this.tableRow2 = new DevExpress.XtraReports.UI.XRTableRow();
            this.tableCell10 = new DevExpress.XtraReports.UI.XRTableCell();
            this.tableCell3 = new DevExpress.XtraReports.UI.XRTableCell();
            this.tableCell4 = new DevExpress.XtraReports.UI.XRTableCell();
            this.tableCell5 = new DevExpress.XtraReports.UI.XRTableCell();
            this.tableCell14 = new DevExpress.XtraReports.UI.XRTableCell();
            this.table1 = new DevExpress.XtraReports.UI.XRTable();
            this.tableRow1 = new DevExpress.XtraReports.UI.XRTableRow();
            this.tableCell2 = new DevExpress.XtraReports.UI.XRTableCell();
            this.tableCell11 = new DevExpress.XtraReports.UI.XRTableCell();
            this.tableCell12 = new DevExpress.XtraReports.UI.XRTableCell();
            this.tableCell1 = new DevExpress.XtraReports.UI.XRTableCell();
            this.tableCell17 = new DevExpress.XtraReports.UI.XRTableCell();
            this.ReportFooter = new DevExpress.XtraReports.UI.ReportFooterBand();
            this.FieldFormatVatTu = new DevExpress.XtraReports.UI.CalculatedField();
            this.FieldFormatKho = new DevExpress.XtraReports.UI.CalculatedField();
            this.FieldStartEndDate = new DevExpress.XtraReports.UI.CalculatedField();
            this.objectDataSource1 = new DevExpress.DataAccess.ObjectBinding.ObjectDataSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.table3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.table2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.table1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.objectDataSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // topMarginBand1
            // 
            this.topMarginBand1.HeightF = 28.04166F;
            this.topMarginBand1.Name = "topMarginBand1";
            // 
            // detailBand1
            // 
            this.detailBand1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.table3});
            this.detailBand1.HeightF = 20F;
            this.detailBand1.MultiColumn.Layout = DevExpress.XtraPrinting.ColumnLayout.AcrossThenDown;
            this.detailBand1.MultiColumn.Mode = DevExpress.XtraReports.UI.MultiColumnMode.UseColumnCount;
            this.detailBand1.Name = "detailBand1";
            // 
            // table3
            // 
            this.table3.BorderColor = System.Drawing.Color.Gray;
            this.table3.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.table3.LocationFloat = new DevExpress.Utils.PointFloat(6.357829E-05F, 0F);
            this.table3.Name = "table3";
            this.table3.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 2, 2, 100F);
            this.table3.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.tableRow3});
            this.table3.SizeF = new System.Drawing.SizeF(759.9999F, 20F);
            this.table3.StylePriority.UseBorderColor = false;
            this.table3.StylePriority.UseBorders = false;
            this.table3.StylePriority.UsePadding = false;
            // 
            // tableRow3
            // 
            this.tableRow3.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.tableCell8,
            this.tableCell13,
            this.tableCell16,
            this.tableCell9,
            this.tableCell20});
            this.tableRow3.Name = "tableRow3";
            this.tableRow3.Weight = 0.8D;
            // 
            // tableCell8
            // 
            this.tableCell8.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Accessories].[Stt]")});
            this.tableCell8.Font = new DevExpress.Drawing.DXFont("Arial", 9F);
            this.tableCell8.ForeColor = System.Drawing.Color.Black;
            this.tableCell8.Name = "tableCell8";
            this.tableCell8.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 96F);
            this.tableCell8.StylePriority.UseFont = false;
            this.tableCell8.StylePriority.UseTextAlignment = false;
            this.tableCell8.Text = "tableCell8";
            this.tableCell8.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.tableCell8.Weight = 0.30040583606732363D;
            // 
            // tableCell13
            // 
            this.tableCell13.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Accessories].[Ten_ptkt]")});
            this.tableCell13.Font = new DevExpress.Drawing.DXFont("Arial", 9F);
            this.tableCell13.ForeColor = System.Drawing.Color.Black;
            this.tableCell13.Name = "tableCell13";
            this.tableCell13.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 96F);
            this.tableCell13.StylePriority.UseFont = false;
            this.tableCell13.StylePriority.UseTextAlignment = false;
            this.tableCell13.Text = "tableCell13";
            this.tableCell13.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.tableCell13.Weight = 3.0040583079018939D;
            // 
            // tableCell16
            // 
            this.tableCell16.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Accessories].[Dvt]")});
            this.tableCell16.Font = new DevExpress.Drawing.DXFont("Arial", 9F);
            this.tableCell16.ForeColor = System.Drawing.Color.Black;
            this.tableCell16.Name = "tableCell16";
            this.tableCell16.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 96F);
            this.tableCell16.StylePriority.UseFont = false;
            this.tableCell16.StylePriority.UseTextAlignment = false;
            this.tableCell16.Text = "tableCell16";
            this.tableCell16.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.tableCell16.Weight = 0.40054111290298466D;
            // 
            // tableCell9
            // 
            this.tableCell9.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Accessories].[So_luong]")});
            this.tableCell9.Font = new DevExpress.Drawing.DXFont("Arial", 9F);
            this.tableCell9.ForeColor = System.Drawing.Color.Black;
            this.tableCell9.Name = "tableCell9";
            this.tableCell9.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 96F);
            this.tableCell9.StylePriority.UseFont = false;
            this.tableCell9.StylePriority.UseTextAlignment = false;
            this.tableCell9.Tag = "I0";
            this.tableCell9.Text = "tableCell9";
            this.tableCell9.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.tableCell9.TextFormatString = "{0:#,0.000;(#,0.000);#}";
            this.tableCell9.Weight = 0.53405482686651062D;
            // 
            // tableCell20
            // 
            this.tableCell20.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Accessories].[Gia_tri]")});
            this.tableCell20.Font = new DevExpress.Drawing.DXFont("Arial", 9F);
            this.tableCell20.ForeColor = System.Drawing.Color.Black;
            this.tableCell20.Name = "tableCell20";
            this.tableCell20.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 96F);
            this.tableCell20.StylePriority.UseFont = false;
            this.tableCell20.StylePriority.UsePadding = false;
            this.tableCell20.StylePriority.UseTextAlignment = false;
            this.tableCell20.Tag = "I0";
            this.tableCell20.Text = "tableCell20";
            this.tableCell20.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.tableCell20.TextFormatString = "{0:#,0;(#,0);#}";
            this.tableCell20.Weight = 0.834460143643464D;
            // 
            // bottomMarginBand1
            // 
            this.bottomMarginBand1.HeightF = 0F;
            this.bottomMarginBand1.Name = "bottomMarginBand1";
            // 
            // ReportHeader
            // 
            this.ReportHeader.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.lbltitle4444});
            this.ReportHeader.HeightF = 65.41666F;
            this.ReportHeader.Name = "ReportHeader";
            // 
            // lbltitle4444
            // 
            this.lbltitle4444.Font = new DevExpress.Drawing.DXFont("Arial", 14F, DevExpress.Drawing.DXFontStyle.Bold);
            this.lbltitle4444.ForeColor = System.Drawing.Color.Black;
            this.lbltitle4444.LocationFloat = new DevExpress.Utils.PointFloat(0F, 26.41665F);
            this.lbltitle4444.Name = "lbltitle4444";
            this.lbltitle4444.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lbltitle4444.SizeF = new System.Drawing.SizeF(760F, 20F);
            this.lbltitle4444.Tag = "NS";
            this.lbltitle4444.Text = "DỤNG CỤ PHỤ TÙNG KÈM THEO";
            this.lbltitle4444.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // PageHeader
            // 
            this.PageHeader.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.table2,
            this.table1});
            this.PageHeader.HeightF = 44.79167F;
            this.PageHeader.Name = "PageHeader";
            // 
            // table2
            // 
            this.table2.AnchorVertical = DevExpress.XtraReports.UI.VerticalAnchorStyles.Bottom;
            this.table2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(242)))), ((int)(((byte)(224)))));
            this.table2.BorderColor = System.Drawing.Color.Gray;
            this.table2.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.table2.LocationFloat = new DevExpress.Utils.PointFloat(0F, 24.79167F);
            this.table2.Name = "table2";
            this.table2.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.tableRow2});
            this.table2.SizeF = new System.Drawing.SizeF(760F, 20F);
            this.table2.StylePriority.UseBackColor = false;
            this.table2.StylePriority.UseBorderColor = false;
            this.table2.StylePriority.UseBorders = false;
            this.table2.StylePriority.UseTextAlignment = false;
            this.table2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // tableRow2
            // 
            this.tableRow2.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.tableCell10,
            this.tableCell3,
            this.tableCell4,
            this.tableCell5,
            this.tableCell14});
            this.tableRow2.Name = "tableRow2";
            this.tableRow2.Weight = 1.1294116803633361D;
            // 
            // tableCell10
            // 
            this.tableCell10.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.tableCell10.CanGrow = false;
            this.tableCell10.Font = new DevExpress.Drawing.DXFont("Arial", 9F, DevExpress.Drawing.DXFontStyle.Bold);
            this.tableCell10.ForeColor = System.Drawing.Color.Black;
            this.tableCell10.Name = "tableCell10";
            this.tableCell10.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 96F);
            this.tableCell10.StylePriority.UseBackColor = false;
            this.tableCell10.StylePriority.UseBorders = false;
            this.tableCell10.StylePriority.UseFont = false;
            this.tableCell10.StylePriority.UsePadding = false;
            this.tableCell10.StylePriority.UseTextAlignment = false;
            this.tableCell10.Text = "A";
            this.tableCell10.Weight = 0.14171454046694357D;
            // 
            // tableCell3
            // 
            this.tableCell3.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.tableCell3.CanGrow = false;
            this.tableCell3.Font = new DevExpress.Drawing.DXFont("Arial", 9F, DevExpress.Drawing.DXFontStyle.Bold);
            this.tableCell3.ForeColor = System.Drawing.Color.Black;
            this.tableCell3.Name = "tableCell3";
            this.tableCell3.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 96F);
            this.tableCell3.StylePriority.UseBackColor = false;
            this.tableCell3.StylePriority.UseBorders = false;
            this.tableCell3.StylePriority.UseFont = false;
            this.tableCell3.StylePriority.UsePadding = false;
            this.tableCell3.StylePriority.UseTextAlignment = false;
            this.tableCell3.Text = "B";
            this.tableCell3.Weight = 1.4171454399350676D;
            // 
            // tableCell4
            // 
            this.tableCell4.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.tableCell4.CanGrow = false;
            this.tableCell4.Font = new DevExpress.Drawing.DXFont("Arial", 9F, DevExpress.Drawing.DXFontStyle.Bold);
            this.tableCell4.ForeColor = System.Drawing.Color.Black;
            this.tableCell4.Name = "tableCell4";
            this.tableCell4.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 96F);
            this.tableCell4.StylePriority.UseBackColor = false;
            this.tableCell4.StylePriority.UseBorders = false;
            this.tableCell4.StylePriority.UseFont = false;
            this.tableCell4.StylePriority.UsePadding = false;
            this.tableCell4.StylePriority.UseTextAlignment = false;
            this.tableCell4.Text = "C";
            this.tableCell4.Weight = 0.18895271919271456D;
            // 
            // tableCell5
            // 
            this.tableCell5.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.tableCell5.CanGrow = false;
            this.tableCell5.Font = new DevExpress.Drawing.DXFont("Arial", 9F, DevExpress.Drawing.DXFontStyle.Bold);
            this.tableCell5.ForeColor = System.Drawing.Color.Black;
            this.tableCell5.Name = "tableCell5";
            this.tableCell5.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 96F);
            this.tableCell5.StylePriority.UseBackColor = false;
            this.tableCell5.StylePriority.UseBorders = false;
            this.tableCell5.StylePriority.UseFont = false;
            this.tableCell5.StylePriority.UsePadding = false;
            this.tableCell5.StylePriority.UseTextAlignment = false;
            this.tableCell5.Text = "1";
            this.tableCell5.Weight = 0.25193695957329543D;
            // 
            // tableCell14
            // 
            this.tableCell14.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.tableCell14.CanGrow = false;
            this.tableCell14.Font = new DevExpress.Drawing.DXFont("Arial", 9F, DevExpress.Drawing.DXFontStyle.Bold);
            this.tableCell14.ForeColor = System.Drawing.Color.Black;
            this.tableCell14.Name = "tableCell14";
            this.tableCell14.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 96F);
            this.tableCell14.StylePriority.UseBackColor = false;
            this.tableCell14.StylePriority.UseBorders = false;
            this.tableCell14.StylePriority.UseFont = false;
            this.tableCell14.StylePriority.UsePadding = false;
            this.tableCell14.StylePriority.UseTextAlignment = false;
            this.tableCell14.Text = "2";
            this.tableCell14.Weight = 0.39365146642319038D;
            // 
            // table1
            // 
            this.table1.AnchorVertical = DevExpress.XtraReports.UI.VerticalAnchorStyles.Bottom;
            this.table1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(242)))), ((int)(((byte)(224)))));
            this.table1.BorderColor = System.Drawing.Color.Gray;
            this.table1.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.table1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 4.791668F);
            this.table1.Name = "table1";
            this.table1.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.tableRow1});
            this.table1.SizeF = new System.Drawing.SizeF(760F, 20F);
            this.table1.StylePriority.UseBackColor = false;
            this.table1.StylePriority.UseBorderColor = false;
            this.table1.StylePriority.UseBorders = false;
            this.table1.StylePriority.UseTextAlignment = false;
            this.table1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // tableRow1
            // 
            this.tableRow1.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.tableCell2,
            this.tableCell11,
            this.tableCell12,
            this.tableCell1,
            this.tableCell17});
            this.tableRow1.Name = "tableRow1";
            this.tableRow1.Weight = 0.68571421097736784D;
            // 
            // tableCell2
            // 
            this.tableCell2.CanGrow = false;
            this.tableCell2.Font = new DevExpress.Drawing.DXFont("Arial", 9F, DevExpress.Drawing.DXFontStyle.Bold);
            this.tableCell2.ForeColor = System.Drawing.Color.Black;
            this.tableCell2.Name = "tableCell2";
            this.tableCell2.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 96F);
            this.tableCell2.StylePriority.UseBackColor = false;
            this.tableCell2.StylePriority.UseFont = false;
            this.tableCell2.StylePriority.UsePadding = false;
            this.tableCell2.StylePriority.UseTextAlignment = false;
            this.tableCell2.Text = "STT";
            this.tableCell2.Weight = 0.29367619367253606D;
            // 
            // tableCell11
            // 
            this.tableCell11.CanGrow = false;
            this.tableCell11.Font = new DevExpress.Drawing.DXFont("Arial", 9F, DevExpress.Drawing.DXFontStyle.Bold);
            this.tableCell11.ForeColor = System.Drawing.Color.Black;
            this.tableCell11.Name = "tableCell11";
            this.tableCell11.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 96F);
            this.tableCell11.StylePriority.UseBackColor = false;
            this.tableCell11.Text = "TÊN, QUY CÁCH DỤNG CỤ, PHỤ TÙNG";
            this.tableCell11.Weight = 2.9367618110323335D;
            // 
            // tableCell12
            // 
            this.tableCell12.CanGrow = false;
            this.tableCell12.Font = new DevExpress.Drawing.DXFont("Arial", 9F, DevExpress.Drawing.DXFontStyle.Bold);
            this.tableCell12.ForeColor = System.Drawing.Color.Black;
            this.tableCell12.Name = "tableCell12";
            this.tableCell12.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 96F);
            this.tableCell12.StylePriority.UseBackColor = false;
            this.tableCell12.StylePriority.UseFont = false;
            this.tableCell12.StylePriority.UsePadding = false;
            this.tableCell12.StylePriority.UseTextAlignment = false;
            this.tableCell12.Text = "ĐVT";
            this.tableCell12.Weight = 0.3915682571815281D;
            // 
            // tableCell1
            // 
            this.tableCell1.CanGrow = false;
            this.tableCell1.Font = new DevExpress.Drawing.DXFont("Arial", 9F, DevExpress.Drawing.DXFontStyle.Bold);
            this.tableCell1.ForeColor = System.Drawing.Color.Black;
            this.tableCell1.Name = "tableCell1";
            this.tableCell1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 96F);
            this.tableCell1.StylePriority.UseBackColor = false;
            this.tableCell1.StylePriority.UseFont = false;
            this.tableCell1.StylePriority.UsePadding = false;
            this.tableCell1.StylePriority.UseTextAlignment = false;
            this.tableCell1.Text = "SỐ LƯỢNG";
            this.tableCell1.Weight = 0.5220909858162075D;
            // 
            // tableCell17
            // 
            this.tableCell17.CanGrow = false;
            this.tableCell17.Font = new DevExpress.Drawing.DXFont("Arial", 9F, DevExpress.Drawing.DXFontStyle.Bold);
            this.tableCell17.ForeColor = System.Drawing.Color.Black;
            this.tableCell17.Name = "tableCell17";
            this.tableCell17.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 96F);
            this.tableCell17.StylePriority.UseBackColor = false;
            this.tableCell17.StylePriority.UseFont = false;
            this.tableCell17.StylePriority.UsePadding = false;
            this.tableCell17.StylePriority.UseTextAlignment = false;
            this.tableCell17.Text = "GIÁ TRỊ";
            this.tableCell17.Weight = 0.815767255871296D;
            // 
            // ReportFooter
            // 
            this.ReportFooter.HeightF = 0F;
            this.ReportFooter.Name = "ReportFooter";
            // 
            // FieldFormatVatTu
            // 
            this.FieldFormatVatTu.DataMember = "tbInfo";
            this.FieldFormatVatTu.Expression = "Upper(\'Vật tư: \'+[ma_vt]+\'-\'+[ten_vt]+\', Đvt:\'+[dvt])";
            this.FieldFormatVatTu.Name = "FieldFormatVatTu";
            // 
            // FieldFormatKho
            // 
            this.FieldFormatKho.DataMember = "tbInfo";
            this.FieldFormatKho.Expression = "Upper(\'Kho: \'+ [ten_kho] +\'- \'+[ma_kho])";
            this.FieldFormatKho.Name = "FieldFormatKho";
            // 
            // FieldStartEndDate
            // 
            this.FieldStartEndDate.DataMember = "tbInfo";
            this.FieldStartEndDate.Expression = "Upper(\'Từ ngày: \'+[StartDateTime]+\' đến ngày: \'+[EndDateTime])";
            this.FieldStartEndDate.Name = "FieldStartEndDate";
            // 
            // objectDataSource1
            // 
            this.objectDataSource1.DataSource = typeof(global::SisFx.Pages.Report.FXTHECC.FXTHECCModel);
            this.objectDataSource1.Name = "objectDataSource1";
            // 
            // FXTHECC1_SUB1
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.topMarginBand1,
            this.detailBand1,
            this.bottomMarginBand1,
            this.ReportHeader,
            this.PageHeader,
            this.ReportFooter});
            this.CalculatedFields.AddRange(new DevExpress.XtraReports.UI.CalculatedField[] {
            this.FieldFormatVatTu,
            this.FieldFormatKho,
            this.FieldStartEndDate});
            this.ComponentStorage.AddRange(new System.ComponentModel.IComponent[] {
            this.objectDataSource1});
            this.DataMember = "Accessories";
            this.DataSource = this.objectDataSource1;
            this.Margins = new DevExpress.Drawing.DXMargins(28F, 39F, 28.04166F, 0F);
            this.PageHeight = 1169;
            this.PageWidth = 827;
            this.PaperKind = DevExpress.Drawing.Printing.DXPaperKind.A4;
            this.ScriptReferencesString = "SmLib.dll\r\nSasFormBrowes.dll";
            this.Version = "24.2";
            ((System.ComponentModel.ISupportInitialize)(this.table3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.table2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.table1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.objectDataSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        #endregion

        private DevExpress.XtraReports.UI.TopMarginBand topMarginBand1;
        private DevExpress.XtraReports.UI.DetailBand detailBand1;
        private DevExpress.XtraReports.UI.XRTable table3;
        private DevExpress.XtraReports.UI.XRTableRow tableRow3;
        private DevExpress.XtraReports.UI.XRTableCell tableCell8;
        private DevExpress.XtraReports.UI.XRTableCell tableCell13;
        private DevExpress.XtraReports.UI.XRTableCell tableCell16;
        private DevExpress.XtraReports.UI.XRTableCell tableCell9;
        private DevExpress.XtraReports.UI.XRTableCell tableCell20;
        private DevExpress.XtraReports.UI.BottomMarginBand bottomMarginBand1;
        private DevExpress.XtraReports.UI.ReportHeaderBand ReportHeader;
        private DevExpress.XtraReports.UI.XRLabel lbltitle4444;
        private DevExpress.XtraReports.UI.PageHeaderBand PageHeader;
        private DevExpress.XtraReports.UI.XRTable table2;
        private DevExpress.XtraReports.UI.XRTableRow tableRow2;
        private DevExpress.XtraReports.UI.XRTableCell tableCell10;
        private DevExpress.XtraReports.UI.XRTableCell tableCell3;
        private DevExpress.XtraReports.UI.XRTableCell tableCell4;
        private DevExpress.XtraReports.UI.XRTableCell tableCell5;
        private DevExpress.XtraReports.UI.XRTableCell tableCell14;
        private DevExpress.XtraReports.UI.XRTable table1;
        private DevExpress.XtraReports.UI.XRTableRow tableRow1;
        private DevExpress.XtraReports.UI.XRTableCell tableCell2;
        private DevExpress.XtraReports.UI.XRTableCell tableCell11;
        private DevExpress.XtraReports.UI.XRTableCell tableCell12;
        private DevExpress.XtraReports.UI.XRTableCell tableCell1;
        private DevExpress.XtraReports.UI.XRTableCell tableCell17;
        private DevExpress.XtraReports.UI.ReportFooterBand ReportFooter;
        private DevExpress.XtraReports.UI.CalculatedField FieldFormatVatTu;
        private DevExpress.XtraReports.UI.CalculatedField FieldFormatKho;
        private DevExpress.XtraReports.UI.CalculatedField FieldStartEndDate;
        private DevExpress.DataAccess.ObjectBinding.ObjectDataSource objectDataSource1;
    }
}
