using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Logger;
using AppConfig;
using Core.Module;
using BusinessLogic.Master;
using Core.Module.Master;
using Microsoft.AspNetCore.Authorization;
using System.IO;
using System.Data;
using System.Net.Http;
using ExcelDataReader;
using System.Net.Http.Headers;

namespace Piaggio_API.Controllers.Master
{
    [Route("api/[controller]")]
    [ApiController]
    public class CampaignUpload : ControllerBase
    {
        private readonly IConfigManager _configuration;
        DbLogger dbLogger; BCampaignUpload bCampaignUpload;
        CommonIUD common = new CommonIUD(); CommonList objList;
        public CampaignUpload(IConfigManager configuration)
        {
            this._configuration = configuration;
            dbLogger = new DbLogger(this._configuration);
            bCampaignUpload = new BCampaignUpload(this._configuration);
        }
        [HttpGet("GetdownloadTemplate")]
        public async Task<IActionResult> GetdownloadTemplate()
        {
            try
            {
                string templetPath = "CampaignUpload.xlsx";
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
                dbLogger.PostErrorLog("CampaignUpload", ex.Message.ToString(), "GetdownloadTemplate", 10001, "Admin", true);
                return Ok();
            }
        }
        [HttpPost("GetCampaignUpload")]
        public async Task<IActionResult> GetCampaignUpload([FromBody] CampaignUploadSearch obj)
        {
            try
            {
                var t1 = Task.Run(() => bCampaignUpload.GetCampaignUpload(obj));
                await Task.WhenAll(t1);
                objList = t1.Status == TaskStatus.RanToCompletion ? t1.Result : objList;
                return Ok(Newtonsoft.Json.JsonConvert.SerializeObject(objList));
            }
            catch (Exception ex)
            {
                dbLogger.PostErrorLog("CampaignUpload", ex.Message.ToString(), "GetCampaignUpload", 10001, "Admin", true);
                return Ok(Newtonsoft.Json.JsonConvert.SerializeObject(common));
            }
        }
        [HttpPost("PostCampaignUploadedit")]
        [Authorize]
        public async Task<IActionResult> PostCampaignUploadedit([FromBody] CampaignUploadClass obj)
        {
            try
            {
                var t1 = Task.Run(() => bCampaignUpload.CampaignUploadedit(obj));
                await Task.WhenAll(t1);
                common = t1.Status == TaskStatus.RanToCompletion ? t1.Result : common;
                return Ok(Newtonsoft.Json.JsonConvert.SerializeObject(common));
            }
            catch (Exception ex)
            {
                dbLogger.PostErrorLog("CampaignUpload", ex.Message.ToString(), "PostCampaignUploadedit", 10001, "Admin", true);
                return Ok(Newtonsoft.Json.JsonConvert.SerializeObject(common));
            }
        }
        [HttpPost("PostCampaignUpload")]
        public async Task<Object> PostCampaignUpload()
        {
            string message = ""; DataSet _ds = new DataSet();
            HttpResponseMessage result = null;
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
                    CampaignUploadClass obj = new CampaignUploadClass();
                    obj.CampaignUploadId = Request.Form["CampaignUploadId"].ToString() == "null" ? 0 : Convert.ToInt32(Request.Form["CampaignUploadId"].ToString());
                    obj.TempletName = Convert.ToString(Request.Form["TempletName"].ToString());//Templet name
                    obj.FromDate = Request.Form["FromDate"].ToString() == "null" ? null : Convert.ToDateTime(Request.Form["FromDate"].ToString());
                    obj.ToDate = Request.Form["ToDate"].ToString() == "null" ? null : Convert.ToDateTime(Request.Form["ToDate"].ToString());
                    obj.CreatedBy = Convert.ToInt64(Request.Form["CreatedBy"].ToString());
                    obj.UpdatedBy = Convert.ToInt64(Request.Form["UpdatedBy"].ToString());
                    obj.IsActive = Convert.ToBoolean(Request.Form["IsActive"].ToString());
                    obj.Action = Convert.ToString(Request.Form["TempTableName"].ToString());//Temp Table Name
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
                        message = "This file format is not supported";
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
                        var t1 = Task.Run(() => bCampaignUpload.CampaignUpload(obj, "", "", excelRecords.Tables[0]));
                        await Task.WhenAll(t1);
                        _objList = t1.Status == TaskStatus.RanToCompletion ? t1.Result : _objList;
                        return Ok(Newtonsoft.Json.JsonConvert.SerializeObject(_objList));
                    }
                }
                catch (Exception ex)
                {
                    dbLogger.PostErrorLog("CampaignUpload", ex.Message.ToString(), "PostCampaignUpload", 10001, "Admin", true);
                    return Ok(Newtonsoft.Json.JsonConvert.SerializeObject(objList));
                }

            }
            else
            {
                result = null; //Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            return message;
        }
        [HttpPost("PostImages")]
        public async Task<IActionResult> PostImages([FromBody] CampaignImagesClass obj)
        {
            try
            {
                var t1 = Task.Run(() => bCampaignUpload.PostImages(obj));
                await Task.WhenAll(t1);
                common = t1.Status == TaskStatus.RanToCompletion ? t1.Result : common;
                return Ok(Newtonsoft.Json.JsonConvert.SerializeObject(common));
            }
            catch (Exception ex)
            {
                dbLogger.PostErrorLog("CampaignUpload", ex.Message.ToString(), "PostImages", 10001, "Admin", true);
                return Ok(Newtonsoft.Json.JsonConvert.SerializeObject(common));
            }
        }
        [HttpPost("PostUploadVedio")]
        public async Task<Object> PostUploadVedio([FromForm] CampaignImagesClass obj)
        {
            try
            {
                obj.FileName = "";
                var httpRequest = HttpContext.Request;
                if (httpRequest.Form.Files.Count > 0)
                {
                    var file = httpRequest.Form.Files[0];
                    var folderName = Path.Combine("wwwroot", "CampaignConatiner");
                    var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                    if (!Directory.Exists(folderName))
                    {
                        Directory.CreateDirectory(folderName);
                    }
                    int num = new Random().Next(1000, 9999);
                    obj.FileName = obj.CampaignUploadNo.Replace("-", "_") + "_" + obj.Action + "_" + num + '.' + obj.FileExtension;
                    string[] allfiles = Directory.GetFiles(pathToSave, file.FileName);
                    //var extname = Path.GetExtension(file.FileName);
                    if (file.Length > 0)
                    {
                        //var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                        var fullPath = Path.Combine(pathToSave, obj.FileName);
                        var dbPath = Path.Combine(folderName, obj.FileName);
                        
                        using (var stream = new FileStream(fullPath, FileMode.Create))
                        {
                            file.CopyTo(stream);
                        }
                        obj.FilePath = fullPath;
                        var t1 = Task.Run(() => bCampaignUpload.PostUploadVedio(obj));
                        await Task.WhenAll(t1);
                        common = t1.Status == TaskStatus.RanToCompletion ? t1.Result : common;
                    }
                }
                return Ok(Newtonsoft.Json.JsonConvert.SerializeObject(common));
            }
            catch (Exception ex)
            {
                dbLogger.PostErrorLog("CampaignUpload", ex.Message.ToString(), "PostUploadVedio", 10001, "Admin", true);
                return Ok(Newtonsoft.Json.JsonConvert.SerializeObject(common));
            }
        }
    }
}
