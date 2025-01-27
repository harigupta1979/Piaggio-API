using AppConfig;
using BusinessLogic.Healper;
using Core.Module;
using Core.Module.Dealer;
using Core.Module.Onboard;
using BusinessLogic.Dealer;
using Logger;
using System;
using System.Data;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BusinessLogic.Onboard;
using System.IO;
using ExcelDataReader;
using System.Net.Http;

namespace Dealer_Main_API.Controllers.OnBoard
{
    [Route("api/[controller]")]
    [ApiController]
    public class OnBoard : ControllerBase
    {
        private readonly IConfigManager _configuration;
        DbLogger dbLogger; BOnboard bOnboard;
        CommonIUD common = new CommonIUD(); CommonList objList;
        public OnBoard(IConfigManager configuration)
        {
            this._configuration = configuration;
            dbLogger = new DbLogger(this._configuration);
            bOnboard = new BOnboard(this._configuration);
        }
        //[HttpPost("PostOnboardApprove")]
        //[Authorize]
        //public async Task<IActionResult> PostOnboardApprove([FromBody] OnboardClass obj)
        //{
        //    try
        //    {
        //        var t1 = Task.Run(() => bOnboard.PostOnboardApproval(obj));
        //        await Task.WhenAll(t1);
        //        common = t1.Status == TaskStatus.RanToCompletion ? t1.Result : common;
        //        return Ok(Newtonsoft.Json.JsonConvert.SerializeObject(common));
        //    }
        //    catch (Exception ex)
        //    {
        //        dbLogger.PostErrorLog("Dealer", ex.Message.ToString(), "PostDealer", 10001, "Admin", true);
        //        return Ok(Newtonsoft.Json.JsonConvert.SerializeObject(common));
        //    }
        //}
        [HttpPost("GetOnbaord")]
        public async Task<IActionResult> GetOnbaordDealer([FromBody] OnboardSearch obj)
        {
            try
            {
                var t1 = Task.Run(() => bOnboard.GetOnbaord(obj));
                await Task.WhenAll(t1);
                objList = t1.Status == TaskStatus.RanToCompletion ? t1.Result : objList;
                return Ok(Newtonsoft.Json.JsonConvert.SerializeObject(objList));
            }
            catch (Exception ex)
            {
                dbLogger.PostErrorLog("OnBoard", ex.Message.ToString(), "GetOnbaord", 10001, "Admin", true);
                return Ok(Newtonsoft.Json.JsonConvert.SerializeObject(common));
            }
        }
        [HttpPost("PostOnboard")]
        //[Authorize]
        public async Task<IActionResult> PostOnboard([FromBody] OnboardClass obj)
        {
            try
            {
                var t1 = Task.Run(() => bOnboard.PostOnboard(obj));
                await Task.WhenAll(t1);
                common = t1.Status == TaskStatus.RanToCompletion ? t1.Result : common;
                return Ok(Newtonsoft.Json.JsonConvert.SerializeObject(common));
            }
            catch (Exception ex)
            {
                dbLogger.PostErrorLog("OnBoard", ex.Message.ToString(), "PostOnboard", 10001, "Admin", true);
                return Ok(Newtonsoft.Json.JsonConvert.SerializeObject(common));
            }
        }

        [HttpPost("GenerateOtp")]
        public async Task<IActionResult> GenerateOtp([FromBody] OnboardOtp obj)
        {
            try
            {
                var t1 = Task.Run(() => bOnboard.GenerateOtp(obj));
                await Task.WhenAll(t1);
                objList = t1.Status == TaskStatus.RanToCompletion ? t1.Result : objList;
                return Ok(Newtonsoft.Json.JsonConvert.SerializeObject(objList));
            }
            catch (Exception ex)
            {
                dbLogger.PostErrorLog("OnBoard", ex.Message.ToString(), "GenerateOtp", 10001, "Admin", true);
                return Ok(Newtonsoft.Json.JsonConvert.SerializeObject(objList));
            }
        }
        [HttpPost("VerifyOTP")]
        public async Task<IActionResult> VerifyOTP([FromBody] OnboardOtp obj)
        {
            try
            {
                var t1 = Task.Run(() => bOnboard.VerifyOTP(obj));
                await Task.WhenAll(t1);
                objList = t1.Status == TaskStatus.RanToCompletion ? t1.Result : objList;
                return Ok(Newtonsoft.Json.JsonConvert.SerializeObject(objList));
            }
            catch (Exception ex)
            {
                dbLogger.PostErrorLog("OnBoard", ex.Message.ToString(), "VerifyOTP", 10001, "CLIENT", true);
                return Ok(Newtonsoft.Json.JsonConvert.SerializeObject(objList));
            }
        }

        [HttpPost("GetOnboardApprovalList")]
        public async Task<IActionResult> GetOnboardApprovalList([FromBody] OnboardSearch obj)
        {
            try
            {
                var t1 = Task.Run(() => bOnboard.GetOnboardApprovalList(obj));
                await Task.WhenAll(t1);
                objList = t1.Status == TaskStatus.RanToCompletion ? t1.Result : objList;
                return Ok(Newtonsoft.Json.JsonConvert.SerializeObject(objList));
            }
            catch (Exception ex)
            {
                dbLogger.PostErrorLog("OnBoard", ex.Message.ToString(), "GetOnboardApprovalList", 10001, "Admin", true);
                return Ok(Newtonsoft.Json.JsonConvert.SerializeObject(objList));
            }
        }
        [HttpGet("GetdownloadTemplate")]
        public async Task<IActionResult> GetdownloadTemplate()
        {
            try
            {
                string templetPath = "OnboardTemplate.xlsx";
                var folderName = Path.Combine("Template", templetPath);
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
                return File(memory, contentType, fileName);
            }
            catch (Exception ex)
            {
                dbLogger.PostErrorLog("OnBoard", ex.Message.ToString(), "GetdownloadTemplate", 10001, "Admin", true);
                return Ok();
            }
        }
        [HttpGet("GetdownloadDataTemplate")]
        public async Task<IActionResult> GetdownloadDataTemplate()
        {
            try
            {
                string templetPath = "OnboardDataMigrattionTemplate.xlsx";
                var folderName = Path.Combine("Template", templetPath);
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
                return File(memory, contentType, fileName);
            }
            catch (Exception ex)
            {
                dbLogger.PostErrorLog("OnBoard", ex.Message.ToString(), "GetdownloadDataTemplate", 10001, "Admin", true);
                return Ok();
            }
        }
        [HttpPost("UploadOnboard")]
        public async Task<Object> UploadOnboard()
        {
            DataSet _ds = new DataSet();
            CommonListDataSet _objList = new CommonListDataSet();
            var httpRequest = HttpContext.Request;
            if (httpRequest.Form.Files.Count > 0)
            {
                try
                {
                    System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
                    Stream stream = httpRequest.Form.Files[0].OpenReadStream();
                    string filename = httpRequest.Form.Files[0].FileName.ToString();
                    IExcelDataReader reader = null;
                    string ExcelFileName = filename;
                    Int64 CreatedBy = Convert.ToInt64(Request.Form["CreatedBy"].ToString());
                    if (filename.EndsWith(".xls"))
                    {
                        reader = ExcelReaderFactory.CreateBinaryReader(stream);
                    }
                    else if (filename.EndsWith(".xlsx"))
                    {
                        reader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                    }
                    else
                    {
                        _objList.Message = "This file format is not supported";
                    }
                    var conf = new ExcelDataSetConfiguration
                    {
                        ConfigureDataTable = _ => new ExcelDataTableConfiguration
                        {
                            UseHeaderRow = true
                        }
                    };
                    DataSet excelRecords = reader.AsDataSet(conf);

                    reader.Close();
                    if (excelRecords.Tables.Count > 0 && excelRecords.Tables[0].Rows.Count > 0)
                    {
                        var t1 = Task.Run(() => bOnboard.PostBulkOnboard(ExcelFileName, CreatedBy, "", "", excelRecords.Tables[0]));
                        await Task.WhenAll(t1);
                        _objList = t1.Status == TaskStatus.RanToCompletion ? t1.Result : _objList;
                        return Ok(Newtonsoft.Json.JsonConvert.SerializeObject(_objList));
                    }
                }
                catch (Exception ex)
                {
                    dbLogger.PostErrorLog("OnBoard", ex.Message.ToString(), "UploadOnboard", 10001, "Admin", true);
                    return Ok();
                }

            }
            else
            {
                return Ok(Newtonsoft.Json.JsonConvert.SerializeObject(_objList));
            }
            return Ok(Newtonsoft.Json.JsonConvert.SerializeObject(_objList));
        }
        [HttpPost("UploadOnboardDataMigration")]
        public async Task<Object> UploadOnboardDataMigration()
        {
            DataSet _ds = new DataSet();
            CommonListDataSet _objList = new CommonListDataSet();
            var httpRequest = HttpContext.Request;
            if (httpRequest.Form.Files.Count > 0)
            {
                try
                {
                    System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
                    Stream stream = httpRequest.Form.Files[0].OpenReadStream();
                    string filename = httpRequest.Form.Files[0].FileName.ToString();
                    IExcelDataReader reader = null;
                    string ExcelFileName = filename;
                    Int64 CreatedBy = Convert.ToInt64(Request.Form["CreatedBy"].ToString());
                    if (filename.EndsWith(".xls"))
                    {
                        reader = ExcelReaderFactory.CreateBinaryReader(stream);
                    }
                    else if (filename.EndsWith(".xlsx"))
                    {
                        reader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                    }
                    else
                    {
                        _objList.Message = "This file format is not supported";
                    }
                    var conf = new ExcelDataSetConfiguration
                    {
                        ConfigureDataTable = _ => new ExcelDataTableConfiguration
                        {
                            UseHeaderRow = true
                        }
                    };
                    DataSet excelRecords = reader.AsDataSet(conf);

                    reader.Close();
                    if (excelRecords.Tables.Count > 0 && excelRecords.Tables[0].Rows.Count > 0)
                    {
                        var t1 = Task.Run(() => bOnboard.PostOnboadDataMigration(ExcelFileName, CreatedBy, "", "", excelRecords.Tables[0]));
                        await Task.WhenAll(t1);
                        _objList = t1.Status == TaskStatus.RanToCompletion ? t1.Result : _objList;
                        return Ok(Newtonsoft.Json.JsonConvert.SerializeObject(_objList));
                    }
                }
                catch (Exception ex)
                {
                    dbLogger.PostErrorLog("OnBoard", ex.Message.ToString(), "UploadOnboardDataMigration", 10001, "Admin", true);
                    return Ok();
                }

            }
            else
            {
                return Ok(Newtonsoft.Json.JsonConvert.SerializeObject(_objList));
            }
            return Ok(Newtonsoft.Json.JsonConvert.SerializeObject(_objList));
        }
        [HttpPost("SendDealerAgreement")]
        [Authorize]
        public async Task<IActionResult> SendDealerAgreement([FromBody] DealerClass obj)
        {
            try
            {

                var t1 = Task.Run(() => bOnboard.SendDealerAgreement(obj.DealerId, "DealerId"));
                await Task.WhenAll(t1);
                common = t1.Status == TaskStatus.RanToCompletion ? t1.Result : common;
                return Ok(Newtonsoft.Json.JsonConvert.SerializeObject(common));
            }
            catch (Exception ex)
            {
                dbLogger.PostErrorLog("Dealer", ex.Message.ToString(), "SendDealerAgreement", 10001, "Admin", true);
                return Ok(Newtonsoft.Json.JsonConvert.SerializeObject(common));
            }
        }
    }
}
