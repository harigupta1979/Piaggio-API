using AppConfig;
using Logger;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Threading.Tasks;
//using Core.Module.Report;
using Microsoft.Reporting.NETCore;
using System.IO;
using ClosedXML.Excel;

namespace ICICI_Dealer_API.Controllers.Report
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly IConfigManager _configuration;
        DbLogger dbLogger;

        public ReportController(IConfigManager configuration)
        {
            this._configuration = configuration;
            dbLogger = new DbLogger(this._configuration);
        }
      /*  [HttpPost("GetExcelReport")]
        public async Task<IActionResult> GetExelReport([FromBody] ExelReportClass obj)
        {
            try
            {
                string ReportURL = Convert.ToString(this._configuration.AppKey("ReportUrl"));
                string ReportUserName = Convert.ToString(this._configuration.AppKey("ReportUserName"));
                string ReportPassword = Convert.ToString(this._configuration.AppKey("ReportPassword"));
                string ReportHost = Convert.ToString(this._configuration.AppKey("ReportHost"));
                string ReportFolderPath = Convert.ToString(this._configuration.AppKey("ReportFolder"));
                byte[] pdfByte = null;
                ServerReport report = new ServerReport();
                report.ReportServerCredentials.NetworkCredentials = new NetworkCredential(ReportUserName, ReportPassword, ReportHost);
                report.ReportServerUrl = new Uri(ReportURL);
                report.ReportPath = ReportFolderPath + obj.ReportName;
                if (obj.ReportName.ToLower() == "customer")
                {
                    report.SetParameters(new[] { new ReportParameter("CustomerId", obj.CustomerId == null ? null : obj.CustomerId.ToString()) });
                    report.SetParameters(new[] { new ReportParameter("BusinessPartnerId", Convert.ToString(obj.BusinessPartnerId)) });
                }
                if (obj.ReportName.ToLower() == "item" || obj.ReportName.ToLower() == "performance" || obj.ReportName.ToLower() == "monthlyperformance"
                    || obj.ReportName.ToLower() == "source"|| obj.ReportName.ToLower() == "leadexecutive"|| obj.ReportName.ToLower() == "salesexecutive"
                    || obj.ReportName.ToLower() == "stockdetails" || obj.ReportName.ToLower() =="user" || obj.ReportName.ToLower() == "subproduct"
                    || obj.ReportName.ToLower() == "dealer" || obj.ReportName.ToLower() == "sourcemaster" || obj.ReportName.ToLower() == "colormaster"
                    || obj.ReportName.ToLower() == "campaign" || obj.ReportName.ToLower() == "publisher" || obj.ReportName.ToLower() == "financer"
                    || obj.ReportName.ToLower() == "fueltype" || obj.ReportName.ToLower() == "department" || obj.ReportName.ToLower() == "language"
                    || obj.ReportName.ToLower() == "dealer" || obj.ReportName.ToLower() == "sourcemaster" || obj.ReportName.ToLower() == "colormaster"
                    || obj.ReportName.ToLower() == "vendertype" || obj.ReportName.ToLower() == "venderonboard" || obj.ReportName.ToLower() == "taxes"
                    || obj.ReportName.ToLower() == "partcategory" || obj.ReportName.ToLower() == "paymentterm" || obj.ReportName.ToLower() == "warrenty" 
                    || obj.ReportName.ToLower() == "fueltype" || obj.ReportName.ToLower() == "department" || obj.ReportName.ToLower() == "language"
                    || obj.ReportName.ToLower() == "designation" || obj.ReportName.ToLower() == "dealerbranch" || obj.ReportName.ToLower() == "product"
                    || obj.ReportName.ToLower() == "make" || obj.ReportName.ToLower() == "model" || obj.ReportName.ToLower() == "variant")
                {
                    report.SetParameters(new[] { new ReportParameter("BusinessPartnerId", Convert.ToString(obj.BusinessPartnerId)) });
                }
                if(obj.ReportName.ToLower() == "rto") { }
                string SaveLocation = @"tempreport\";
                string timestampfilename;
                timestampfilename = obj.ReportName.ToUpper() + "_" + DateTime.Now.ToString() + ".xlsx";
                timestampfilename = timestampfilename.Replace(" ", "");
                timestampfilename = timestampfilename.Replace("/", "");
                timestampfilename = timestampfilename.Replace(":", "");
                //DataSet dsGen = SqlHelper.ExecuteDataset(System.Configuration.ConfigurationSettings.AppSettings["connectionstring"].ToString(), System.Data.CommandType.Text, "SELECT breducedreport,brptpwdprotected,vrptpassword FROM defgeneral where nofficeid=" + officeid);
                //if (dsGen.Tables[0].Rows.Count > 0)
                //{
                //    if (dsGen.Tables[0].Rows[0]["breducedreport"].ToString().ToLower() == "true" && ViewState["reportname"].ToString().ToLower() != "monthlyservicereport_solved.rpt" && ViewState["reportname"].ToString().ToLower() != "" && ViewState["reportname"].ToString().ToLower() != "responsetimereport.rpt" && ViewState["reportname"].ToString().ToLower() != "routewiseanalysis.rpt" && ViewState["reportname"].ToString().ToLower() != "routewisecallbackanalysis.rpt")
                //    {
                       timestampfilename = "b_" + timestampfilename;
                //    }
                //}

                string mimeType;
                string encoding;
                string extension;
                string[] streams;
                Warning[] warnings;
                var t1 = Task.Run(() =>  report.Render("EXCELOPENXML", string.Empty, out mimeType, out encoding, out extension, out streams, out warnings));
                await Task.WhenAll(t1);
                pdfByte = t1.Status == TaskStatus.RanToCompletion ? t1.Result : pdfByte;
                SaveLocation += timestampfilename;
                if (System.IO.File.Exists(SaveLocation))
                {
                    System.IO.File.Delete(SaveLocation);
                }
                System.IO.FileStream fs = new System.IO.FileStream(SaveLocation, System.IO.FileMode.Create);
                fs.Write(pdfByte, 0, pdfByte.Length);
                fs.Close();
                fs.Dispose();
                string newstrExportFile = FormatExcel(timestampfilename, obj.ReportName.ToLower());

                var folderName = Path.Combine(SaveLocation, newstrExportFile);
                string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                var fileName = Path.GetFileName(filePath);
                if (!System.IO.File.Exists(filePath))
                    return Ok();

                var memory = new MemoryStream();
                await using (var stream = new FileStream(filePath, FileMode.Open))
                {
                    await stream.CopyToAsync(memory);
                }
                memory.Position = 0;
                if (System.IO.File.Exists(newstrExportFile))
                {
                    System.IO.File.Delete(newstrExportFile);
                }
                GC.WaitForPendingFinalizers();
                return File(memory, contentType, fileName);
            }
            catch (Exception ex)
            {
                dbLogger.PostErrorLog("ReportController", ex.Message.ToString(), "GetExcelReport", 10001, "Admin", true);
                return Ok();
            }
        }*/

        static string FormatExcel(string fileName,string WorksheetName)
        {
            var strExportFile = Path.Combine(Directory.GetCurrentDirectory(), "tempreport/" + fileName);
            string newfilename = fileName.Remove(0, 2);
            string filetosave = Path.Combine(Directory.GetCurrentDirectory(), "tempreport/" + newfilename);
            try
            {
                using var wbook = new XLWorkbook(strExportFile);
                var ws = wbook.Worksheet(WorksheetName);
                //ws.Row(1).InsertRowsAbove(4);
                ws.Rows().AdjustToContents();
                ws.Columns().AdjustToContents();
                ws.ShowGridLines = true;
                ws.Range("A5", "BZ5").SetAutoFilter();
                ws.Rows().Style.Font.FontSize = 8;
                ws.Rows().Style.Font.FontName = "ARIAL";
                ws.Range("A5", "BZ5").Style.Fill.BackgroundColor = XLColor.Orange;
                ws.Range("A5", "BZ5").Style.Font.Bold = true;
                ws.Range("A5", "BZ5").Style.Font.Underline = XLFontUnderlineValues.Single;
                ws.SheetView.Freeze(5, 0);
                wbook.SaveAs(filetosave);
            }
            catch(Exception ex)
            {
                if (System.IO.File.Exists(filetosave))
                {
                    System.IO.File.Delete(strExportFile);
                    return newfilename;
                }
                else
                {
                    return strExportFile;
                }
            }
            finally
            {
                if (System.IO.File.Exists(filetosave))
                {
                    System.IO.File.Delete(strExportFile);
                }
                GC.WaitForPendingFinalizers();
            }
            return filetosave;
        }
        [HttpPost("GetPDFReport")]
        public async Task<IActionResult> GetReport([FromBody] Core.Module.ReportClass obj)
        {
            try
            {
                string ReportURL = Convert.ToString(this._configuration.AppKey("ReportUrl"));
                string ReportUserName = Convert.ToString(this._configuration.AppKey("ReportUserName"));
                string ReportPassword = Convert.ToString(this._configuration.AppKey("ReportPassword"));
                string ReportHost = Convert.ToString(this._configuration.AppKey("ReportHost"));
                string ReportFolderPath = Convert.ToString(this._configuration.AppKey("ReportFolder"));
                byte[] pdfByte = null;
                ServerReport report = new ServerReport();
                report.ReportServerCredentials.NetworkCredentials = new NetworkCredential(ReportUserName, ReportPassword, ReportHost);
                report.ReportServerUrl = new Uri(ReportURL);
                report.ReportPath = ReportFolderPath + obj.ReportName;
                if (obj.ReportName.ToLower() == "invoice")
                {
                    report.SetParameters(new[] { new ReportParameter("InvoiceId", Convert.ToString(obj.TrnsId)) });
                    report.SetParameters(new[] { new ReportParameter("DealerId", Convert.ToString(obj.DealerId)) });
                }
                var t1 = Task.Run(() => report.Render("PDF"));
                await Task.WhenAll(t1);
                pdfByte = t1.Status == TaskStatus.RanToCompletion ? t1.Result : pdfByte;
                return File(pdfByte, contentType: "application/pdf", obj.ReportName.ToLower()+".pdf");
            }
            catch (Exception ex)
            {
                dbLogger.PostErrorLog("ReportController", ex.Message.ToString(), "GetPDFReport", 10001, "Admin", true);
                return Ok();
            }

        }

    }
}
