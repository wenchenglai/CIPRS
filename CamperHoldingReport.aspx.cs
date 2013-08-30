using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Drawing;
using GemBox.ExcelLite;

public partial class CamperHoldingReport : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void btnReport_Click(object sender, EventArgs e)
    {
        DataTable dt = CamperHoldingDA.GetData(Int32.Parse(Application["CampYear"].ToString()));

        string templateFile = Server.MapPath(@"~/Docs/Templates/CamperDetailReport.xls");
        string workFileDir = Server.MapPath(@"~/Docs");

        // Make a excel report
        ExcelLite.SetLicense("EL6N-Z669-AZZG-3LS7");
        ExcelFile excel = new ExcelFile();
        excel.LoadXls(templateFile);

        ExcelWorksheet ws = excel.Worksheets["Sheet1"];
        ws.Name = "Campers Holding";

        ws.InsertDataTable(dt, 0, 0, true);

        CellStyle tableHeaderStyle = new CellStyle();
        tableHeaderStyle.Font.Weight = ExcelFont.BoldWeight;
        tableHeaderStyle.FillPattern.SetSolid(Color.DarkGray);

        ws.Rows[0].Style = tableHeaderStyle;
        ws.Columns[0].Width = 20 * 256;
        ws.Columns[1].Width = 20 * 256;
        ws.Columns[2].Width = 30 * 256;
        ws.Columns[4].Width = 17 * 256;
        ws.Columns[5].Width = 42 * 256;
        ws.Columns[6].Width = 40 * 256;
        ws.Columns[7].Width = 16 * 256;
        ws.Columns[8].Width = 16 * 256;

        // Save to a file on the local file system
        string filename = String.Format("\\{0}{1}{2}{3}CamperHoldingReport.xls", DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Millisecond);
        string newFile = workFileDir + filename;
        excel.SaveXls(newFile);

        string[] strFileParts = newFile.Split(new string[] { "\\" }, StringSplitOptions.None);

        //Display excel spreadsheet
        this.Response.Clear();
        this.Response.Buffer = true;
        this.Response.AddHeader("Content-Disposition", "attachment; filename=" + strFileParts[strFileParts.Length - 1]);
        this.Response.ContentType = "application/vnd.ms-excel";
        this.Response.Charset = "";

        if (newFile.Length == 0)
            this.Response.Write("Error encountered - no spreadsheet to display");
        else
            this.Response.WriteFile(newFile);

        Response.End();	  
    }
}