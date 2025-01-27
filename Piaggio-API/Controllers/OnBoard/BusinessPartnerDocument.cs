using Core.Module.Onboard;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Logger;
using AppConfig;
using Core.Module;
using Microsoft.AspNetCore.Authorization;
using BusinessLogic.Onboard;

namespace Dealer_Main_API.Controllers.OnBoard
{
    [Route("api/[controller]")]
    [ApiController]
    public class BusinessPartnerDocument : ControllerBase
    {
        private readonly IConfigManager _configuration;
        DbLogger dbLogger; BOnboard bOnboard; BBusinessPartnerDocument bdocument;
        CommonIUD common = new CommonIUD(); CommonList objList;
        public BusinessPartnerDocument(IConfigManager configuration)
        {
            this._configuration = configuration;
            dbLogger = new DbLogger(this._configuration);
            bOnboard = new BOnboard(this._configuration);
            bdocument= new BBusinessPartnerDocument(this._configuration);
        }
        [HttpPost("BusinessPartnerDocumentDelete")]
        public async Task<IActionResult> BusinessPartnerDocumentDelete([FromBody] BusinessPartnerDocumentClass obj)
        {
            try
            {
                var t1 = Task.Run(() => bdocument.GetOnbaordDocumentDelete(obj));
                await Task.WhenAll(t1);
                common = t1.Status == TaskStatus.RanToCompletion ? t1.Result : common;
                return Ok(Newtonsoft.Json.JsonConvert.SerializeObject(common));
            }
            catch (Exception ex)
            {
                dbLogger.PostErrorLog("BusinessPartnerDocument", ex.Message.ToString(), "BusinessPartnerDocumentDelete", 10001, "Admin", true);
                return Ok(Newtonsoft.Json.JsonConvert.SerializeObject(common));
            }
        }
        [HttpPost("BusinessPartnerDocumentDownload")]
        public async Task<IActionResult> BusinessPartnerDocumentView([FromBody] BusinessPartnerDocumentClass obj)
        {
            try
            {
                CommonListMongodb commonListMongodb = new CommonListMongodb();
                var t1 = Task.Run(() => bdocument.GetOnbaordDocument(obj));
                await Task.WhenAll(t1);
                commonListMongodb = t1.Status == TaskStatus.RanToCompletion ? t1.Result : commonListMongodb;
                return Ok(Newtonsoft.Json.JsonConvert.SerializeObject(commonListMongodb));
            }
            catch (Exception ex)
            {
                dbLogger.PostErrorLog("BusinessPartnerDocument", ex.Message.ToString(), "BusinessPartnerDocumentDownload", 10001, "Admin", true);
                return Ok(Newtonsoft.Json.JsonConvert.SerializeObject(common));
            }
        }
        [HttpPost("BusinessPartnerDocumentUpload")]
        public async Task<IActionResult> BusinessPartnerDocumentUpload([FromBody] BusinessPartnerDocumentClass obj)
        {
            try
            {
                var t1 = Task.Run(() => bdocument.PostOnbaordDocument(obj));
                await Task.WhenAll(t1);
                common = t1.Status == TaskStatus.RanToCompletion ? t1.Result : common;
                return Ok(Newtonsoft.Json.JsonConvert.SerializeObject(common));
            }
            catch (Exception ex)
            {
                dbLogger.PostErrorLog("BusinessPartnerDocument", ex.Message.ToString(), "BusinessPartnerDocumentUpload", 10001, "Admin", true);
                return Ok(Newtonsoft.Json.JsonConvert.SerializeObject(common));
            }
        }
      
        [HttpPost("GetOnbaordDocument")]
        public async Task<IActionResult> GetOnbaordDocument([FromBody] OnboardSearch obj)
        {
            try
            {
                var t1 = Task.Run(() => bdocument.GetOnbaordDocumentList(obj));
                await Task.WhenAll(t1);
                objList = t1.Status == TaskStatus.RanToCompletion ? t1.Result : objList;
                return Ok(Newtonsoft.Json.JsonConvert.SerializeObject(objList));
            }
            catch (Exception ex)
            {
                dbLogger.PostErrorLog("BusinessPartnerDocument", ex.Message.ToString(), "GetOnbaordDocument", 10001, "Admin", true);
                return Ok(Newtonsoft.Json.JsonConvert.SerializeObject(common));
            }
        }

        [HttpPost("GetTDSDocument")]
        public async Task<IActionResult> GetTDSDocument([FromBody] TDSDocumentClass obj)
        {
            try
            {
                var t1 = Task.Run(() => bdocument.GetTDSDocumentlist(obj));
                await Task.WhenAll(t1);
                objList = t1.Status == TaskStatus.RanToCompletion ? t1.Result : objList;
                return Ok(Newtonsoft.Json.JsonConvert.SerializeObject(objList));
            }
            catch (Exception ex)
            {
                dbLogger.PostErrorLog("BusinessPartnerDocument", ex.Message.ToString(), "GetTDSDocument", 10001, "Admin", true);
                return Ok(Newtonsoft.Json.JsonConvert.SerializeObject(common));
            }
        }
        [HttpPost("TDSDocumentUpload")]
        public async Task<IActionResult> TDSDocumentUpload([FromBody] TDSDocumentClass obj)
        {
            try
            {
                var t1 = Task.Run(() => bdocument.PostTDSDocumentUpload(obj));
                await Task.WhenAll(t1);
                common = t1.Status == TaskStatus.RanToCompletion ? t1.Result : common;
                return Ok(Newtonsoft.Json.JsonConvert.SerializeObject(common));
            }
            catch (Exception ex)
            {
                dbLogger.PostErrorLog("BusinessPartnerDocument", ex.Message.ToString(), "TDSDocumentUpload", 10001, "Admin", true);
                return Ok(Newtonsoft.Json.JsonConvert.SerializeObject(common));
            }
        }
        [HttpPost("TDSDocumentDownload")]
        public async Task<IActionResult> TDSDocumentDownload([FromBody] TDSDocumentClass obj)
        {
            try
            {
                CommonListMongodb commonListMongodb = new CommonListMongodb();
                var t1 = Task.Run(() => bdocument.TDSDocumentDownload(obj));
                await Task.WhenAll(t1);
                commonListMongodb = t1.Status == TaskStatus.RanToCompletion ? t1.Result : commonListMongodb;
                return Ok(Newtonsoft.Json.JsonConvert.SerializeObject(commonListMongodb));
            }
            catch (Exception ex)
            {
                dbLogger.PostErrorLog("BusinessPartnerDocument", ex.Message.ToString(), "TDSDocumentDownload", 10001, "Admin", true);
                return Ok(Newtonsoft.Json.JsonConvert.SerializeObject(common));
            }
        }
        [HttpPost("TDSDocumentDelete")]
        public async Task<IActionResult> TDSDocumentDelete([FromBody] TDSDocumentClass obj)
        {
            try
            {
                var t1 = Task.Run(() => bdocument.TDSDocumentDelete(obj));
                await Task.WhenAll(t1);
                common = t1.Status == TaskStatus.RanToCompletion ? t1.Result : common;
                return Ok(Newtonsoft.Json.JsonConvert.SerializeObject(common));
            }
            catch (Exception ex)
            {
                dbLogger.PostErrorLog("BusinessPartnerDocument", ex.Message.ToString(), "TDSDocumentDelete", 10001, "Admin", true);
                return Ok(Newtonsoft.Json.JsonConvert.SerializeObject(common));
            }
        }
    }
}
