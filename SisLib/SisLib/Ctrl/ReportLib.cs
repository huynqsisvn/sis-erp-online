using DevExpress.XtraGauges.Core.Model;
using DevExpress.XtraPrinting;
using DevExpress.XtraReports.UI;
using SisData.Data;
using SisData.Model;
using SisLib.Ctrl;
using SisLib.Lib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows;

namespace SisLib
{
    public class ReportLib
    {
        // Optimized: cache option values and arrays once; reduce repeated calls and duplicated branches;
        // extracted small local helpers to keep behaviour identical while improving readability and perf.
        public void SetControlsFontFamily(object parent_control, string titleReport, IOptionsManager OptionsManager, string refReport, bool isFirst, string lang)
        {
            string optRFontName = OptionsManager.GetOptionsValue("M_RFONTNAME");
            string optRTFont = OptionsManager.GetOptionsValue("M_RTFONT");
            string optRSFont = OptionsManager.GetOptionsValue("M_RSFONT");
            string optRtfFontOption = OptionsManager.GetOptionsValue("M_RTFONT");
            string optIsDemo = OptionsManager.GetOptionsValue("M_IS_DEMO") ?? "";
            string optUpperCoRep = OptionsManager.GetOptionsValue("M_UPPER_CO_REP") ?? "0";
            string optUpperAddRep = OptionsManager.GetOptionsValue("M_UPPER_ADD_REP") ?? "0";

            string[] strArray1 = !String.IsNullOrEmpty(optRFontName) ? optRFontName.Split(';') : null;
            string[] strArray4 = !String.IsNullOrEmpty(optRTFont) ? optRTFont.Split(';') : null;
            string[] strArray6 = !String.IsNullOrEmpty(optRSFont) ? optRSFont.Split(';') : null;

            // Frequently referenced label names set
            var titleLabels = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                "lbltitle","lblten_ktt","lblten_ktt2","lblten_gd","lblten_gd2",
                "lblten_thu_kho","lblten_thu_kho2","lblten_thu_quy","lblten_thu_quy2"
            };

            // Local helper to set font safely
            void SetControlFont(XRControl ctl, string fontName, float size, FontStyle style)
            {
                if (ctl == null) return;
                if (!ctl.GetType().Equals(typeof(XRRichText)))
                    ctl.Font = new Font(new FontFamily(fontName), size, style);
                else
                    (ctl as XRRichText).Rtf = (ctl as XRRichText).Rtf?.Replace(ctl.Font.Name, fontName);
            }

            // Local helper to safely get option value
            string GetOpt(string name) => OptionsManager.GetOptionsValue(name) ?? "";

            foreach (XRControl control in (CollectionBase)((XRControl)parent_control).Controls)
            {
                if (control.Controls.Count > 0)
                    SetControlsFontFamily(control, titleReport, OptionsManager, refReport, false, lang);

                if (control.GetType().Equals(typeof(XRLabel)) || control.GetType().Equals(typeof(XRTableCell)) || control.GetType().Equals(typeof(XRCheckBox)) || control.GetType().Equals(typeof(XRRichText)))
                {

                    bool nf = false, ns = false, nc = false, i0 = false, np = false;
                    string tagName = "";

                    if (control.Tag != null)
                    {
                        foreach (var t in control.Tag.ToString().Split(';'))
                        {
                            var s = t.Trim();
                            switch (s.ToUpperInvariant())
                            {
                                case "NF": nf = true; break;
                                case "NS": ns = true; break;
                                case "NC": nc = true; break;
                                case "I0": i0 = true; break;
                                case "NP": np = true; break;
                                default:
                                    tagName = s;
                                    break;
                            }
                        }
                    }

                    if (!np)
                        control.Padding = new PaddingInfo(2, 2, 0, 0);
                    if (!nc)
                        control.ForeColor = Color.Black;

                    string ctrlName = (control.Name ?? "").Trim();
                    string ctrlNameLower = ctrlName.ToLowerInvariant();
                    string tagNameLower = (tagName ?? "").ToLowerInvariant();                    
                    if (!titleLabels.Contains(ctrlNameLower) && !titleLabels.Contains(tagNameLower))
                    {
                        if (!String.IsNullOrEmpty(optRFontName))
                        {
                            if (!nf)
                            {
                                if (!control.GetType().Equals(typeof(XRRichText)))
                                {
                                    // Use cached first font name if available; fall back to Arial
                                    var fontName = strArray1 != null && strArray1.Length > 0 ? strArray1[0] : "Arial";
                                    SetControlFont(control, fontName, control.Font.Size, (FontStyle)control.Font.Style);
                                }
                                else
                                {
                                    (control as XRRichText).Rtf = (control as XRRichText).Rtf?.Replace(control.Font.Name, "Arial");
                                }
                            }
                        }
                        else if (nf || ns)
                        {
                            if (strArray1 != null && strArray1.Length > 1)
                            {
                                float parsedSize = -1f;
                                float.TryParse(strArray1[1], out parsedSize);
                                if (parsedSize > 0)
                                {
                                    if (nf && ns)
                                    {
                                        // keep current font name, only adjust size
                                        if (!control.GetType().Equals(typeof(XRRichText)))
                                            control.Font = new Font(new FontFamily(control.Font.Name), control.Font.Size, (FontStyle)control.Font.Style);
                                    }
                                    else if (nf)
                                    {
                                        if (!control.GetType().Equals(typeof(XRRichText)))
                                            control.Font = new Font(control.Font.Name, parsedSize, (FontStyle)control.Font.Style);
                                    }
                                    else // ns
                                    {
                                        string fn = strArray1[0];
                                        if (!control.GetType().Equals(typeof(XRRichText)))
                                            control.Font = new Font(fn, control.Font.Size, (FontStyle)control.Font.Style);
                                        else
                                            (control as XRRichText).Rtf = (control as XRRichText).Rtf?.Replace(control.Font.Name, fn);
                                    }
                                }
                            }
                        }
                        else if (strArray1 != null && strArray1.Length > 1)
                        {
                            float parsedSize = -1f;
                            float.TryParse(strArray1[1], out parsedSize);
                            if (parsedSize > 0)
                            {
                                string fn = strArray1[0];
                                SetControlFont(control, fn, parsedSize, (FontStyle)control.Font.Style);
                            }
                        }

                        // Special-case company/title label texts
                        if (ctrlNameLower.Equals("lblten_cty") || tagNameLower.Equals("lblten_cty"))
                        {
                            control.Text = "PHẦN MỀM CHƯA ĐĂNG KÝ BẢN QUYỀN!";
                            if (!optIsDemo.Trim().Equals("TRUE", StringComparison.OrdinalIgnoreCase))
                            {
                                if (optUpperCoRep == "1")
                                    control.Text = GetOpt("M_TEN_CTY").Trim();
                                else if (optUpperCoRep == "2")
                                    control.Text = GetOpt("M_TEN_CTY").Trim().ToUpperInvariant();
                                else if (optUpperCoRep == "3")
                                {
                                    control.Text = GetOpt("M_TEN_CTY").Trim();
                                    control.Font = new Font(control.Font.Name, control.Font.Size, System.Drawing.FontStyle.Bold);
                                }
                                else
                                {
                                    control.Text = GetOpt("M_TEN_CTY").Trim().ToUpperInvariant();
                                    control.Font = new Font(control.Font.Name, control.Font.Size, System.Drawing.FontStyle.Bold);
                                }
                            }
                        }
                        else if (ctrlNameLower.Equals("lblten_cty2") || tagNameLower.Equals("lblten_cty2"))
                        {
                            control.Text = "PHẦN MỀM CHƯA ĐĂNG KÝ BẢN QUYỀN!";
                            if (!optIsDemo.Trim().Equals("TRUE", StringComparison.OrdinalIgnoreCase))
                            {
                                if (optUpperCoRep == "1")
                                    control.Text = GetOpt("M_TEN_CTY2").Trim();
                                else if (optUpperCoRep == "2")
                                    control.Text = GetOpt("M_TEN_CTY2").Trim().ToUpperInvariant();
                                else if (optUpperCoRep == "3")
                                {
                                    control.Text = GetOpt("M_TEN_CTY2").Trim();
                                    control.Font = new Font(control.Font.Name, control.Font.Size, System.Drawing.FontStyle.Bold);
                                }
                                else
                                {
                                    control.Text = GetOpt("M_TEN_CTY2").Trim().ToUpperInvariant();
                                    control.Font = new Font(control.Font.Name, control.Font.Size, System.Drawing.FontStyle.Bold);
                                }
                            }
                        }
                        else if (ctrlNameLower.Equals("lbldia_chi") || tagNameLower.Equals("lbldia_chi"))
                        {
                            control.Text = "LIÊN HỆ VỚI SIS VIỆT NAM ĐỂ BIẾT THÊM CHI TIẾT";
                            if (!optIsDemo.Trim().Equals("TRUE", StringComparison.OrdinalIgnoreCase))
                            {
                                if (optUpperAddRep == "1")
                                {
                                    control.Text = GetOpt("M_DIA_CHI").Trim();
                                }
                                else if (optUpperAddRep == "2")
                                {
                                    control.Text = GetOpt("M_DIA_CHI").Trim().ToUpperInvariant();
                                }
                                else if (optUpperAddRep == "3")
                                {
                                    control.Text = GetOpt("M_DIA_CHI").Trim();
                                    control.Font = new Font(control.Font.Name, control.Font.Size, System.Drawing.FontStyle.Bold);
                                }
                                else
                                {
                                    control.Text = GetOpt("M_DIA_CHI").Trim().ToUpperInvariant();
                                    control.Font = new Font(control.Font.Name, control.Font.Size, System.Drawing.FontStyle.Bold);
                                }
                            }
                        }
                        else if (ctrlNameLower.Equals("lbldia_chi2") || tagNameLower.Equals("lbldia_chi2"))
                        {
                            control.Text = "LIÊN HỆ VỚI SIS VIỆT NAM ĐỂ BIẾT THÊM CHI TIẾT";
                            if (!optIsDemo.Trim().Equals("TRUE", StringComparison.OrdinalIgnoreCase))
                            {
                                if (optUpperAddRep == "1")
                                {
                                    control.Text = GetOpt("M_DIA_CHI2").Trim();
                                    if (!lang.ToLowerInvariant().Trim().Equals("việt - anh"))
                                        control.Font = new Font(control.Font.Name, control.Font.Size, System.Drawing.FontStyle.Regular);
                                }
                                else if (optUpperAddRep == "2")
                                {
                                    control.Text = GetOpt("M_DIA_CHI2").Trim().ToUpperInvariant();
                                    if (!lang.ToLowerInvariant().Trim().Equals("việt - anh"))
                                        control.Font = new Font(control.Font.Name, control.Font.Size, System.Drawing.FontStyle.Regular);
                                }
                                else if (optUpperAddRep == "3")
                                {
                                    control.Text = GetOpt("M_DIA_CHI2").Trim();
                                    if (!lang.ToLowerInvariant().Trim().Equals("việt - anh"))
                                        control.Font = new Font(control.Font.Name, control.Font.Size, System.Drawing.FontStyle.Bold);
                                }
                                else
                                {
                                    control.Text = GetOpt("M_DIA_CHI2").Trim().ToUpperInvariant();
                                    if (!lang.ToLowerInvariant().Trim().Equals("việt - anh"))
                                        control.Font = new Font(control.Font.Name, control.Font.Size, System.Drawing.FontStyle.Bold);
                                }
                            }
                        }
                        else if (ctrlNameLower.Equals("lblmst") || tagNameLower.Equals("lblmst"))
                        {
                            control.Text = String.IsNullOrEmpty(GetOpt("M_MA_THUE")) ? "" : GetOpt("M_MA_THUE");
                        }
                        else if (ctrlNameLower.Equals("lblma_thue") || tagNameLower.Equals("lblma_thue"))
                        {
                            control.Text = "Mã số thuế: " + (String.IsNullOrEmpty(GetOpt("M_MA_THUE")) ? "" : GetOpt("M_MA_THUE"));
                            var option = OptionsManager.GetOptionsValue("M_MA_THUE_CK");
                            if (option != null && option.ToString().Trim().Equals("0"))
                                control.Visible = false;
                        }
                        else if (ctrlNameLower.Equals("lblma_thue2") || tagNameLower.Equals("lblma_thue2"))
                        {
                            control.Text = "Tax code: " + (GetOpt("M_MA_THUE"));
                            var option = OptionsManager.GetOptionsValue("M_MA_THUE_CK");
                            if (option != null && option.ToString().Trim().Equals("0"))
                                control.Visible = false;
                        }
                        else if (ctrlNameLower.Equals("lblcuc_thue") || tagNameLower.Equals("lblcuc_thue"))
                        {
                            control.Text = GetOpt("M_CUC_THUE") == null ? "" : GetOpt("M_CUC_THUE").ToString().ToUpperInvariant();
                        }
                        else if (ctrlNameLower.Equals("lbltk") || tagNameLower.Equals("lbltk"))
                            control.Text = GetOpt("M_TK_NH");
                        else if (ctrlNameLower.Equals("lbldien_thoai") || tagNameLower.Equals("lbldien_thoai"))
                            control.Text = GetOpt("M_PHONE");
                        else if (ctrlNameLower.Equals("lblnh") || tagNameLower.Equals("lblnh"))
                            control.Text = GetOpt("M_NH");
                        else if (ctrlNameLower.Equals("lblweb") || tagNameLower.Equals("lblweb"))
                            control.Text = GetOpt("M_WEB");
                        else if (ctrlNameLower.Equals("lblemail") || tagNameLower.Equals("lblemail"))
                            control.Text = GetOpt("M_EMAIL");
                        else if (control.Text != null && control.Text.Contains("lblten_goi_gd"))
                            control.Text = control.Text.Replace("lblten_goi_gd", GetOpt("M_TEN_GOI_GD"));
                        else if (control.Text != null && control.Text.Contains("lblten_goi_ktt"))
                            control.Text = control.Text.Replace("lblten_goi_ktt", GetOpt("M_TEN_GOI_KTT"));
                        else if (ctrlNameLower.Equals("lblpageinfo") || tagNameLower.Equals("lblpageinfo"))
                        {
                            control.Text = "";
                            control.PrintOnPage += new PrintOnPageEventHandler((sender, e) =>
                            {
                                string s = "";
                                string pageOpt = GetOpt("M_PAGE_F_REP").Trim();
                                if (pageOpt.Equals("1"))
                                    s = "Trang: " + (e.PageIndex + 1).ToString();
                                else if (pageOpt.Equals("2"))
                                    s = "Trang: " + (e.PageIndex + 1).ToString() + ", " + DateTime.Now.Date.ToString("dd-MM-yyyy");
                                else if (pageOpt.Equals("3"))
                                    s = $"Trang: {(e.PageIndex + 1)}, {DateTime.Now.Date:dd-MM-yyyy}, {DateTime.Now:HH:mm:ss}";
                                (sender as XRLabel).Text = s;
                            });
                        }
                        else if (ctrlNameLower.Equals("lblpageinfo2") || tagNameLower.Equals("lblpageinfo2"))
                        {
                            control.Text = "";
                            control.PrintOnPage += new PrintOnPageEventHandler((sender, e) =>
                            {
                                string s = "";
                                string pageOpt = GetOpt("M_PAGE_F_REP").Trim();
                                if (pageOpt.Equals("1"))
                                    s = "Page: " + (e.PageIndex + 1).ToString();
                                else if (pageOpt.Equals("2"))
                                    s = "Page: " + (e.PageIndex + 1).ToString() + ", " + DateTime.Now.Date.ToString("dd-MM-yyyy");
                                else if (pageOpt.Equals("3"))
                                    s = $"Page: {(e.PageIndex + 1)}, {DateTime.Now.Date:dd-MM-yyyy}, {DateTime.Now:HH:mm:ss}";
                                (sender as XRLabel).Text = s;
                            });
                        }
                        else if (ctrlNameLower.Equals("lblpageinfo3") || tagNameLower.Equals("lblpageinfo3"))
                        {
                            if (control.GetType().Equals(typeof(XRRichText)))
                            {
                                float result = -1f;
                                if (strArray4 != null && strArray4.Length > 1)
                                    float.TryParse(strArray4[1], out result);
                                control.Text = "";
                                if (result > 0)
                                    control.Font = new Font(strArray4[0], result, (FontStyle)control.Font.Style);

                                control.PrintOnPage += new PrintOnPageEventHandler((sender, e) =>
                                {
                                    string s = "";
                                    string pageOpt = GetOpt("M_PAGE_F_REP").Trim();
                                    if (pageOpt.Equals("1"))
                                        s = "<div style=\"font-size:15px\"><b>Trang</b><i> / Page:</i>" + (e.PageIndex + 1).ToString() + "</div>";
                                    else if (pageOpt.Equals("2"))
                                        s = $"<div style=\"font-size:15px\"><b>Trang</b><i> / Page:</i>{(e.PageIndex + 1)}, {DateTime.Now.Date:dd-MM-yyyy}</div>";
                                    else if (pageOpt.Equals("3"))
                                        s = $"<div style=\"font-size:15px\"><b>Trang</b><i> / Page:</i>{(e.PageIndex + 1)}, {DateTime.Now.Date:dd-MM-yyyy}, {DateTime.Now:HH:mm:ss}</div>";
                                    (sender as XRRichText).Html = s;
                                });
                            }
                        }
                        else
                        {
                            // Handle data bindings formatting for XRLabel / XRTableCell (numeric/date formatting)
                            if (control is XRLabel xrLabel)
                            {
                                if (!String.IsNullOrEmpty(control.TextFormatString))
                                {
                                    var fmt = control.TextFormatString.Trim();
                                    if (!string.IsNullOrEmpty(fmt))
                                    {
                                        // map common codes to options
                                        if (fmt.Contains("N0"))
                                        {
                                            string f = GetOpt("M_IP_TIEN").ToString();
                                            if (GetOpt("M_KIEU_SO_AM").ToString().Equals("2"))
                                                f = NormalizeOptionNegativeFormat(f);
                                            control.TextFormatString = !i0 ? "{0:" + f + "}" : "{0:" + f + ";#}";
                                        }
                                        else if (fmt.Equals("N1"))
                                        {
                                            string f = GetOpt("M_IP_TIEN_NT").ToString();
                                            if (GetOpt("M_KIEU_SO_AM").ToString().Equals("2"))
                                                f = NormalizeOptionNegativeFormat(f);
                                            control.TextFormatString = !i0 ? "{0:" + f + "}" : "{0:" + f + ";#}";
                                        }
                                        else if (fmt.Equals("P0"))
                                        {
                                            string f = GetOpt("M_IP_GIA").ToString();
                                            if (GetOpt("M_KIEU_SO_AM").ToString().Equals("2"))
                                                f = NormalizeOptionNegativeFormat(f);
                                            control.TextFormatString = !i0 ? "{0:" + f + "}" : "{0:" + f + ";#}";
                                        }
                                        else if (fmt.Equals("P1"))
                                        {
                                            string f = GetOpt("M_IP_GIA_NT").ToString();
                                            if (GetOpt("M_KIEU_SO_AM").ToString().Equals("2"))
                                                f = NormalizeOptionNegativeFormat(f);
                                            control.TextFormatString = !i0 ? "{0:" + f + "}" : "{0:" + f + ";#}";
                                        }
                                        else if (fmt.Equals("Q"))
                                        {
                                            string f = GetOpt("M_IP_SL").ToString();
                                            if (GetOpt("M_KIEU_SO_AM").ToString().Equals("2"))
                                                f = NormalizeOptionNegativeFormat(f);
                                            control.TextFormatString = !i0 ? "{0:" + f + "}" : "{0:" + f + ";#}";
                                        }
                                        else if (fmt.Equals("R"))
                                        {
                                            control.TextFormatString = !i0 ? "{0:" + GetOpt("M_IP_TY_GIA") + "}" : "{0:" + GetOpt("M_IP_TY_GIA").ToString() + ";#}";
                                        }
                                        else if (fmt.Contains("x"))
                                        {
                                            // date handling script injection (preserve original behaviour)
                                            string dataMember = control.DataBindings["Text"].DataMember;
                                            string[] parts = dataMember.Split('.');
                                            string table = parts[0];
                                            string column = parts[1];
                                            string script = "private void " + control.Name + "_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e) {\r\n if((((XRLabel)sender).RootReport.DataSource as System.Data.DataSet).Tables[\"" + table + "\"].Rows[0][\"" + column + "\"] == DBNull.Value)\r\n   return;\r\n   DateTime currentDateTime= new DateTime();\r\n   if(GetCurrentColumnValue(\"" + column + "\") == null)\r\n     currentDateTime = Convert.ToDateTime((((XRLabel)sender).RootReport.DataSource as System.Data.DataSet).Tables[\"" + table + "\"].Rows[0][\"" + column + "\"]);\r\n   else \r\n     currentDateTime = Convert.ToDateTime(GetCurrentColumnValue(\"" + column + "\"));\r\n  ((XRLabel)sender).Text = SasFormBrowes.SysFunc.GetDateString(currentDateTime,\"" + control.TextFormatString + "\");\r\n}";
                                            if (!control.RootReport.ScriptsSource.Contains(script))
                                                control.RootReport.ScriptsSource += script;
                                            control.Scripts.OnBeforePrint = control.Name + "_BeforePrint";
                                        }
                                    }
                                    else
                                    {
                                        // fallback to TextFormatString behavior for XRLabel
                                        if (xrLabel.TextFormatString != null)
                                        {
                                            ApplyLabelTextFormat(xrLabel, i0);
                                        }
                                    }
                                }

                                if (xrLabel.Summary != null)
                                {
                                    ApplySummaryFormat(xrLabel.Summary, i0);
                                }
                            }
                            else if (control is XRTableCell xrCell)
                            {
                                if (control.DataBindings["Text"] != null)
                                {
                                    var fmt = control.TextFormatString;
                                    if (!string.IsNullOrEmpty(fmt))
                                    {
                                        if (fmt.Contains("N0"))
                                        {
                                            string f = GetOpt("M_IP_TIEN").ToString();
                                            if (GetOpt("M_KIEU_SO_AM").ToString().Equals("2"))
                                                f = NormalizeOptionNegativeFormat(f);
                                            control.TextFormatString = !i0 ? "{0:" + f + "}" : "{0:" + f + ";#}";
                                        }
                                        else if (fmt.Equals("N1"))
                                        {
                                            string f = GetOpt("M_IP_TIEN_NT").ToString();
                                            if (GetOpt("M_KIEU_SO_AM").ToString().Equals("2"))
                                                f = NormalizeOptionNegativeFormat(f);
                                            control.TextFormatString = !i0 ? "{0:" + f + "}" : "{0:" + f + ";#}";
                                        }
                                        else if (fmt.Equals("P0"))
                                        {
                                            string f = GetOpt("M_IP_GIA").ToString();
                                            if (GetOpt("M_KIEU_SO_AM").ToString().Equals("2"))
                                                f = NormalizeOptionNegativeFormat(f);
                                            control.TextFormatString = !i0 ? "{0:" + f + "}" : "{0:" + f + ";#}";
                                        }
                                        else if (fmt.Equals("P1"))
                                        {
                                            string f = GetOpt("M_IP_GIA_NT").ToString();
                                            if (GetOpt("M_KIEU_SO_AM").ToString().Equals("2"))
                                                f = NormalizeOptionNegativeFormat(f);
                                            control.TextFormatString = !i0 ? "{0:" + f + "}" : "{0:" + f + ";#}";
                                        }
                                        else if (fmt.Equals("Q"))
                                        {
                                            string f = GetOpt("M_IP_SL").ToString();
                                            if (GetOpt("M_KIEU_SO_AM").ToString().Equals("2"))
                                                f = NormalizeOptionNegativeFormat(f);
                                            control.TextFormatString = !i0 ? "{0:" + f + "}" : "{0:" + f + ";#}";
                                        }
                                        else if (fmt.Equals("R"))
                                        {
                                            control.TextFormatString = !i0 ? "{0:" + GetOpt("M_IP_TY_GIA") + "}" : "{0:" + GetOpt("M_IP_TY_GIA").ToString() + ";#}";
                                        }
                                        else if (fmt.Contains("x"))
                                        {
                                            string dataMember = control.DataBindings["Text"].DataMember;
                                            string[] parts = dataMember.Split('.');
                                            string table = parts[0];
                                            string column = parts[1];
                                            string script = "private void " + control.Name + "_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e) {\r\n DateTime currentDateTime= new DateTime();\r\n   if(GetCurrentColumnValue(\"" + column + "\") == null)\r\n     currentDateTime = Convert.ToDateTime((((XRTableCell)sender).RootReport.DataSource as System.Data.DataSet).Tables[\"" + table + "\"].Rows[0][\"" + column + "\"]);\r\n   else \r\n     currentDateTime = Convert.ToDateTime(GetCurrentColumnValue(\"" + column + "\"));\r\n  ((XRTableCell)sender).Text = SasFormBrowes.SysFunc.GetDateString(currentDateTime,\"" + control.TextFormatString + "\");\r\n}";
                                            if (!control.RootReport.ScriptsSource.Contains(script))
                                                control.RootReport.ScriptsSource += script;
                                            control.Scripts.OnBeforePrint = control.Name + "_BeforePrint";
                                        }
                                    }
                                    else
                                    {
                                        if (xrCell.TextFormatString != null)
                                        {
                                            ApplyTableCellTextFormat(xrCell, i0);
                                        }
                                    }
                                }

                                if (xrCell.Summary != null)
                                {
                                    ApplySummaryFormat(xrCell.Summary, i0);
                                }
                            }
                        }
                    }
                    else if (control.Name.Trim().ToLower().Equals("lbltitle") || tagName.ToLower().Equals("lbltitle"))
                    {
                        control.Text = titleReport.ToUpper();
                        if (GetOpt("M_RTFONT").Equals((object)DBNull.Value))
                        {
                            if (!nf)
                                control.Font = new Font(new FontFamily("Arial"), control.Font.Size, (FontStyle)control.Font.Style);
                        }
                        else if (strArray4.Length > 1)
                        {
                            float result = -1f;
                            float.TryParse(strArray4[1], out result);
                            if ((double)result > 0.0)
                                control.Font = new Font(strArray4[0], result, (FontStyle)(control.Font.Style));
                        }
                    }
                    else
                    {
                        if (GetOpt("M_RSFONT").Equals((object)DBNull.Value))
                        {
                            if (!nf)
                                control.Font = new Font(new FontFamily("Arial"), control.Font.Size, (FontStyle)control.Font.Style);
                        }
                        else if (strArray6.Length > 1)
                        {
                            float result = -1f;
                            float.TryParse(strArray6[1], out result);
                            if ((double)result > 0.0)
                                control.Font = new Font(new FontFamily(strArray6[0]), result, (FontStyle)control.Font.Style);
                        }
                        if (control.Name.Trim().ToLower().Equals("lblten_ktt") || tagName.ToLower().Equals("lblten_ktt"))
                            control.Text = GetOpt("M_TEN_KTT") == null ? "" : GetOpt("M_TEN_KTT").ToString();
                        else if (control.Name.Trim().ToLower().Equals("lblten_ktt2") || tagName.ToLower().Equals("lblten_ktt2"))
                            control.Text = GetOpt("M_TEN_KTT2") == null ? "" : GetOpt("M_TEN_KTT2").ToString();
                        else if (control.Name.Trim().ToLower().Equals("lblten_gd") || tagName.ToLower().Equals("lblten_gd"))
                            control.Text = GetOpt("M_TEN_GD") == null ? "" : GetOpt("M_TEN_GD").ToString();
                        else if (control.Name.Trim().ToLower().Equals("lblten_gd2") || tagName.ToLower().Equals("lblten_gd2"))
                            control.Text = GetOpt("M_TEN_GD2") == null ? "" : GetOpt("M_TEN_GD2").ToString();
                        else if (control.Name.Trim().ToLower().Equals("lblten_thu_kho") || tagName.ToLower().Equals("lblten_thu_kho"))
                            control.Text = GetOpt("M_THU_KHO") == null ? "" : GetOpt("M_THU_KHO").ToString();
                        else if (control.Name.Trim().ToLower().Equals("lblten_thu_kho2") || tagName.ToLower().Equals("lblten_thu_kho2"))
                            control.Text = GetOpt("M_THU_KHO2") == null ? "" : GetOpt("M_THU_KHO2").ToString();
                        else if (control.Name.Trim().ToLower().Equals("lblten_thu_quy") || tagName.ToLower().Equals("lblten_thu_quy"))
                            control.Text = GetOpt("M_THU_QUY") == null ? "" : GetOpt("M_THU_QUY").ToString();
                        else if (control.Name.Trim().ToLower().Equals("lblten_thu_quy2") || tagName.ToLower().Equals("lblten_thu_quy2"))
                            control.Text = GetOpt("M_THU_QUY2") == null ? "" : GetOpt("M_THU_QUY2").ToString();
                    }
                } 
                else if (control is XRPivotGrid pivot)
                {
                    // Apply pivot fonts if RS font options present
                    if (strArray1 != null && strArray1.Length > 1)
                    {
                        float result = -1f;
                        float.TryParse(strArray1[1], out result);
                        if (result > 0)
                        {
                            XRControlStyle xrControlStyle = new XRControlStyle
                            {
                                Padding = new PaddingInfo(2, 2, 0, 0)
                            };
                            pivot.Styles.CellStyle = xrControlStyle;
                            pivot.Appearance.Cell.Font = new Font(strArray1[0], result, (FontStyle)pivot.Appearance.Cell.Font.Style);
                            pivot.Appearance.FieldHeader.Font = new Font(strArray1[0], result, (FontStyle)pivot.Appearance.FieldHeader.Font.Style);
                            pivot.Appearance.CustomTotalCell.Font = new Font(strArray1[0], result, (FontStyle)pivot.Appearance.CustomTotalCell.Font.Style);
                            pivot.Appearance.FieldValue.Font = new Font(strArray1[0], result, (FontStyle)pivot.Appearance.FieldValue.Font.Style);
                            pivot.Appearance.FieldValueGrandTotal.Font = new Font(strArray1[0], result, (FontStyle)pivot.Appearance.FieldValueGrandTotal.Font.Style);
                            pivot.Appearance.FieldValueTotal.Font = new Font(strArray1[0], result, (FontStyle)pivot.Appearance.FieldValueTotal.Font.Style);
                            pivot.Appearance.FilterSeparator.Font = new Font(strArray1[0], result, (FontStyle)pivot.Appearance.FilterSeparator.Font.Style);
                            pivot.Appearance.GrandTotalCell.Font = new Font(strArray1[0], result, (FontStyle)pivot.Appearance.GrandTotalCell.Font.Style);
                            pivot.Appearance.HeaderGroupLine.Font = new Font(strArray1[0], result, (FontStyle)pivot.Appearance.HeaderGroupLine.Font.Style);
                            pivot.Appearance.Lines.Font = new Font(strArray1[0], result, (FontStyle)pivot.Appearance.Lines.Font.Style);
                            pivot.Appearance.TotalCell.Font = new Font(strArray1[0], result, (FontStyle)pivot.Appearance.TotalCell.Font.Style);
                        }
                    }
                }
            }

            // Local helper functions below (keeps original formatting logic centralized)
            static string NormalizeOptionNegativeFormat(string s)
            {
                if (s == null) return s;
                var parts = s.Split(';');
                if (parts.Length > 1 && parts[1].StartsWith("-"))
                {
                    parts[1] = "(" + parts[1].Substring(1).Trim() + ")";
                    return string.Join(";", parts);
                }
                return s;
            }

            void ApplyLabelTextFormat(XRLabel xrLabel, bool i0flag)
            {
                if (xrLabel.TextFormatString != null)
                {
                    string tf = xrLabel.TextFormatString;
                    string k = GetOpt("M_KIEU_SO_AM");
                    // Delegate to central formatting logic by simulating FormatString handling
                    if (tf.Contains("N0"))
                    {
                        string f = GetOpt("M_IP_TIEN");
                        if (k.Equals("2")) f = NormalizeOptionNegativeFormat(f);
                        xrLabel.TextFormatString = !i0flag ? "{0:" + f + "}" : "{0:" + f + ";#}";
                    }
                    else if (tf.Equals("N1"))
                    {
                        string f = GetOpt("M_IP_TIEN_NT");
                        if (k.Equals("2")) f = NormalizeOptionNegativeFormat(f);
                        xrLabel.TextFormatString = !i0flag ? "{0:" + f + "}" : "{0:" + f + ";#}";
                    }
                    else if (tf.Equals("P0"))
                    {
                        string f = GetOpt("M_IP_GIA");
                        if (k.Equals("2")) f = NormalizeOptionNegativeFormat(f);
                        xrLabel.TextFormatString = !i0flag ? "{0:" + f + "}" : "{0:" + f + ";#}";
                    }
                    else if (tf.Equals("P1"))
                    {
                        string f = GetOpt("M_IP_GIA_NT");
                        if (k.Equals("2")) f = NormalizeOptionNegativeFormat(f);
                        xrLabel.TextFormatString = !i0flag ? "{0:" + f + "}" : "{0:" + f + ";#}";
                    }
                    else if (tf.Equals("Q"))
                    {
                        string f = GetOpt("M_IP_SL");
                        if (k.Equals("2")) f = NormalizeOptionNegativeFormat(f);
                        xrLabel.TextFormatString = !i0flag ? "{0:" + f + "}" : "{0:" + f + ";#}";
                    }
                    else if (tf.Equals("R"))
                    {
                        xrLabel.TextFormatString = !i0flag ? "{0:" + GetOpt("M_IP_TY_GIA") + "}" : "{0:" + GetOpt("M_IP_TY_GIA").ToString() + ";#}";
                    }
                }
            }

            void ApplyTableCellTextFormat(XRTableCell xrCell, bool i0flag)
            {
                if (xrCell.TextFormatString != null)
                {
                    string tf = xrCell.TextFormatString;
                    string k = GetOpt("M_KIEU_SO_AM");
                    if (tf.Contains("N0"))
                    {
                        string f = GetOpt("M_IP_TIEN");
                        if (k.Equals("2")) f = NormalizeOptionNegativeFormat(f);
                        xrCell.TextFormatString = !i0flag ? "{0:" + f + "}" : "{0:" + f + ";#}";
                    }
                    else if (tf.Equals("N1"))
                    {
                        string f = GetOpt("M_IP_TIEN_NT");
                        if (k.Equals("2")) f = NormalizeOptionNegativeFormat(f);
                        xrCell.TextFormatString = !i0flag ? "{0:" + f + "}" : "{0:" + f + ";#}";
                    }
                    else if (tf.Equals("P0"))
                    {
                        string f = GetOpt("M_IP_GIA");
                        if (k.Equals("2")) f = NormalizeOptionNegativeFormat(f);
                        xrCell.TextFormatString = !i0flag ? "{0:" + f + "}" : "{0:" + f + ";#}";
                    }
                    else if (tf.Equals("P1"))
                    {
                        string f = GetOpt("M_IP_GIA_NT");
                        if (k.Equals("2")) f = NormalizeOptionNegativeFormat(f);
                        xrCell.TextFormatString = !i0flag ? "{0:" + f + "}" : "{0:" + f + ";#}";
                    }
                    else if (tf.Equals("Q"))
                    {
                        string f = GetOpt("M_IP_SL");
                        if (k.Equals("2")) f = NormalizeOptionNegativeFormat(f);
                        xrCell.TextFormatString = !i0flag ? "{0:" + f + "}" : "{0:" + f + ";#}";
                    }
                    else if (tf.Equals("R"))
                    {
                        xrCell.TextFormatString = !i0flag ? "{0:" + GetOpt("M_IP_TY_GIA") + "}" : "{0:" + GetOpt("M_IP_TY_GIA").ToString() + ";#}";
                    }
                }
            }

            void ApplySummaryFormat(XRSummary summary, bool i0flag)
            {
                if (summary != null && !string.IsNullOrEmpty(summary.FormatString))
                {
                    string fs = summary.FormatString;
                    string k = GetOpt("M_KIEU_SO_AM");
                    if (fs.Equals("N0"))
                    {
                        string f = GetOpt("M_IP_TIEN");
                        if (k.Equals("2")) f = NormalizeOptionNegativeFormat(f);
                        summary.FormatString = !i0flag ? "{0:" + f + "}" : "{0:" + f + ";#}";
                    }
                    else if (fs.Equals("N1"))
                    {
                        string f = GetOpt("M_IP_TIEN_NT");
                        if (k.Equals("2")) f = NormalizeOptionNegativeFormat(f);
                        summary.FormatString = !i0flag ? "{0:" + f + "}" : "{0:" + f + ";#}";
                    }
                    else if (fs.Equals("P0"))
                    {
                        string f = GetOpt("M_IP_GIA");
                        if (k.Equals("2")) f = NormalizeOptionNegativeFormat(f);
                        summary.FormatString = !i0flag ? "{0:" + f + "}" : "{0:" + f + ";#}";
                    }
                    else if (fs.Equals("P1"))
                    {
                        string f = GetOpt("M_IP_GIA_NT");
                        if (k.Equals("2")) f = NormalizeOptionNegativeFormat(f);
                        summary.FormatString = !i0flag ? "{0:" + f + "}" : "{0:" + f + ";#}";
                    }
                    else if (fs.Equals("Q"))
                    {
                        string f = GetOpt("M_IP_SL");
                        if (k.Equals("2")) f = NormalizeOptionNegativeFormat(f);
                        summary.FormatString = !i0flag ? "{0:" + f + "}" : "{0:" + f + ";#}";
                    }
                    else if (fs.Equals("R"))
                    {
                        summary.FormatString = !i0flag ? "{0:" + GetOpt("M_IP_TY_GIA") + "}" : "{0:" + GetOpt("M_IP_TY_GIA").ToString() + ";#}";
                    }
                }
            }
        }

        public static void BrowTable(IDxWindowService myModal, DataTable TblData, string StrBrowse, string Title = "", string CancelCaption = "")
        {
            if (TblData == null)
            {
                return;
            }
            Dictionary<string, object> Par = new Dictionary<string, object>();
            Par.Add("TblData", (object)TblData);
            Par.Add("StrBrowse", (object)StrBrowse);
            Par.Add("Title", (object)Title);

            var aty = typeof(BrowTable);
            DxWindowModel model = new DxWindowModel(aty, Par, true, "", Title, CancelCaption);
            model.nWitdh = "75vw";
            myModal.Show(model);
        }
    }
}