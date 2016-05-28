using System;
using System.Web;
using GemBox.ExcelLite;
using System.Data;
using System.Drawing;

/// <summary>
/// Summary description for DataExtractBL
/// </summary>
public class DataExtractBL
{
    public static string GenerateExcelReport(DataTable dt)
    {
        string templateFile = HttpContext.Current.Server.MapPath(@"~/Docs/Templates/CamperDetailReport.xls");
        string workFileDir = HttpContext.Current.Server.MapPath(@"~/Docs");

        // Make a excel report
        ExcelLite.SetLicense("EL6N-Z669-AZZG-3LS7");
        ExcelFile excel = new ExcelFile();
        excel.LoadXls(templateFile);

        ExcelWorksheet ws = excel.Worksheets["Sheet1"];

        // The first row is decoration headers
        ws.Rows[1].Height = 2 * 256;

        CellStyle cs = new CellStyle();
        cs.FillPattern.SetSolid(Color.FromArgb(204, 153, 255));
        cs.Font.Weight = ExcelFont.BoldWeight;
        cs.WrapText = true;
        cs.HorizontalAlignment = HorizontalAlignmentStyle.Center;

        CellRange cr = ws.Cells.GetSubrangeAbsolute(0, 0, 0, 10);
        cr.Merged = true;
        cr.Value = "Program Information";
        cr.Style = cs;

        CellStyle cs2 = new CellStyle();
        cs2.FillPattern.SetSolid(Color.FromArgb(102, 255, 255));
        cs2.Font.Weight = ExcelFont.BoldWeight;
        cs2.WrapText = true;
        cs2.HorizontalAlignment = HorizontalAlignmentStyle.Center;

        CellRange cr2 = ws.Cells.GetSubrangeAbsolute(0, 11, 0, 15);
        cr2.Merged = true;
        cr2.Value = "Camp Information";
        cr2.Style = cs2;

        CellStyle cs3 = new CellStyle();
        cs3.FillPattern.SetSolid(Color.FromArgb(255, 153, 204));
        cs3.Font.Weight = ExcelFont.BoldWeight;
        cs3.WrapText = true;
        cs3.HorizontalAlignment = HorizontalAlignmentStyle.Center;

        CellRange cr3 = ws.Cells.GetSubrangeAbsolute(0, 16, 0, 23);
        cr3.Merged = true;
        cr3.Value = "Basic Camper Information";
        cr3.Style = cs3;

        CellStyle cs4 = new CellStyle();
        cs4.FillPattern.SetSolid(Color.FromArgb(204, 255, 204));
        cs4.Font.Weight = ExcelFont.BoldWeight;
        cs4.WrapText = true;
        cs4.HorizontalAlignment = HorizontalAlignmentStyle.Center;

        CellRange cr4 = ws.Cells.GetSubrangeAbsolute(0, 24, 0, 28);
        cr4.Merged = true;
        cr4.Value = "Camper Contact Information";
        cr4.Style = cs4;

        CellStyle cs5 = new CellStyle();
        cs5.FillPattern.SetSolid(Color.FromArgb(255, 255, 153));
        cs5.Font.Weight = ExcelFont.BoldWeight;
        cs5.WrapText = true;
        cs5.HorizontalAlignment = HorizontalAlignmentStyle.Center;

        CellRange cr5 = ws.Cells.GetSubrangeAbsolute(0, 29, 0, 42);
        cr5.Merged = true;
        cr5.Value = "Parent Contact Information";
        cr5.Style = cs5;

        CellStyle cs6 = new CellStyle();
        cs6.FillPattern.SetSolid(Color.FromArgb(150, 150, 150));
        cs6.Font.Weight = ExcelFont.BoldWeight;
        cs6.WrapText = true;
        cs6.HorizontalAlignment = HorizontalAlignmentStyle.Center;

        CellRange cr6 = ws.Cells.GetSubrangeAbsolute(0, 43, 0, 45);
        cr6.Merged = true;
        cr6.Value = "Application Information";
        cr6.Style = cs6;

        CellStyle cs7 = new CellStyle();
        cs7.FillPattern.SetSolid(Color.FromArgb(255, 204, 153));
        cs7.Font.Weight = ExcelFont.BoldWeight;
        cs7.WrapText = true;
        cs7.HorizontalAlignment = HorizontalAlignmentStyle.Center;

        CellRange cr7 = ws.Cells.GetSubrangeAbsolute(0, 46, 0, 52);
        cr7.Merged = true;
        cr7.Value = "Marketing Source";
        cr7.Style = cs7;

        CellStyle cs8 = new CellStyle();
        cs8.FillPattern.SetSolid(Color.FromArgb(153, 204, 255));
        cs8.Font.Weight = ExcelFont.BoldWeight;
        cs8.WrapText = true;
        cs8.HorizontalAlignment = HorizontalAlignmentStyle.Center;

        CellRange cr8 = ws.Cells.GetSubrangeAbsolute(0, 53, 0, 68);
        cr8.Merged = true;
        cr8.Value = "Demographic Information";
        cr8.Style = cs8;

        // this creats the real table
        ws.InsertDataTable(dt, 1, 0, true);

        CellStyle styleTableHeaderColumns = new CellStyle();
        styleTableHeaderColumns.Font.Weight = ExcelFont.BoldWeight;
        styleTableHeaderColumns.HorizontalAlignment = HorizontalAlignmentStyle.Center;
        styleTableHeaderColumns.VerticalAlignment = VerticalAlignmentStyle.Center;
        styleTableHeaderColumns.FillPattern.SetSolid(Color.LightGray);
        styleTableHeaderColumns.WrapText = true;

        ws.Columns[0].Width = 15 * 256;
        ws.Columns[1].Width = 35 * 256;
        ws.Columns[2].Width = 30 * 256;

        ws.Columns[11].Width = 35 * 256; // Camp Name
        ws.Columns[12].Width = 27 * 256;

        ws.Columns[16].Width = 30 * 256; // Last Name
        ws.Columns[17].Width = 30 * 256;

        ws.Rows[1].Style = styleTableHeaderColumns;
        ws.Rows[1].Height = 3 * 256;

        CellStyle styleContent = new CellStyle { HorizontalAlignment = HorizontalAlignmentStyle.Left, VerticalAlignment = VerticalAlignmentStyle.Center };

        for (int i = 2; i < dt.Rows.Count; i++)
        {
            ws.Rows[i].Style = styleContent;
        }

        styleContent.WrapText = true;

        excel.Worksheets.ActiveWorksheet = excel.Worksheets[0];

        // Save to a file on the local file system
        string filename = String.Format("\\{0}{1}{2}{3}DataExtract.xls", DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Millisecond);
        string newFile = workFileDir + filename;
        excel.SaveXls(newFile);


        string[] strFileParts = newFile.Split(new string[] { "\\" }, StringSplitOptions.None);

        return strFileParts[strFileParts.Length - 1];
    }
}