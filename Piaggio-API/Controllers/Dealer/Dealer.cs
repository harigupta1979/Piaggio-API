using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Logger;
using AppConfig;
using Core.Module;
using BusinessLogic.Dealer;
using Core.Module.Dealer;
using Microsoft.AspNetCore.Authorization;

namespace ICICI_Dealer_API.Controllers.Dealer
{
    [Route("api/[controller]")]
    [ApiController]
    public class Dealer : ControllerBase
    {
        private readonly IConfigManager _configuration;
        DbLogger dbLogger; BDealer bDealer;
        CommonIUD common = new CommonIUD(); CommonList objList;
        public Dealer(IConfigManager configuration)
        {
            this._configuration = configuration;
            dbLogger = new DbLogger(this._configuration);
            bDealer = new BDealer(this._configuration);
        }
        [HttpPost("PostDealer")]
        [Authorize]
        public async Task<IActionResult> PostDealer([FromBody] DealerClass obj)
        {
            try
            {
                var t1 = Task.Run(() => bDealer.PostDealer(obj));
                await Task.WhenAll(t1);
                common = t1.Status == TaskStatus.RanToCompletion ? t1.Result : common;
                return Ok(Newtonsoft.Json.JsonConvert.SerializeObject(common));
            }
            catch (Exception ex)
            {
                dbLogger.PostErrorLog("Dealer", ex.Message.ToString(), "PostDealer", 10001, "Admin", true);
                return Ok(Newtonsoft.Json.JsonConvert.SerializeObject(common));
            }
        }

        [HttpPost("GetDealer")]
        [Authorize]
        public async Task<IActionResult> GetDealer([FromBody] DealerSearch obj)
        {
            try
            {
                var t1 = Task.Run(() => bDealer.GetDealer(obj));
                await Task.WhenAll(t1);
                objList = t1.Status == TaskStatus.RanToCompletion ? t1.Result : objList;
                return Ok(Newtonsoft.Json.JsonConvert.SerializeObject(objList));
            }
            catch (Exception ex)
            {
                dbLogger.PostErrorLog("Dealer", ex.Message.ToString(), "GetDealer", 10001, "Admin", true);
                return Ok(Newtonsoft.Json.JsonConvert.SerializeObject(objList));
            }
        }

        [HttpPost("PostDealerUploadDocument")]
        [Authorize]
        public async Task<IActionResult> PostDealerUploadDocument([FromBody] DealerImage obj)
        {
            try
            {
                var t1 = Task.Run(() => bDealer.PostDealerImage(obj));
                await Task.WhenAll(t1);
                common = t1.Status == TaskStatus.RanToCompletion ? t1.Result : common;
                return Ok(Newtonsoft.Json.JsonConvert.SerializeObject(common));
            }
            catch (Exception ex)
            {
                dbLogger.PostErrorLog("Dealer", ex.Message.ToString(), "PostDealerUploadDocument", 10001, "Admin", true);
                return Ok(Newtonsoft.Json.JsonConvert.SerializeObject(common));
            }
        }
        [HttpPost("GetDealerDocument")]
        [Authorize]
        public async Task<IActionResult> GetDealerDocument([FromBody] DealerDocSearch obj)
        {
            try
            {
                var t1 = Task.Run(() => bDealer.GetDealerDocument(obj));
                await Task.WhenAll(t1);
                objList = t1.Status == TaskStatus.RanToCompletion ? t1.Result : objList;
                return Ok(Newtonsoft.Json.JsonConvert.SerializeObject(objList));
            }
            catch (Exception ex)
            {
                dbLogger.PostErrorLog("Dealer", ex.Message.ToString(), "GetDealerDocument", 10001, "Admin", true);
                return Ok(Newtonsoft.Json.JsonConvert.SerializeObject(common));
            }
        }

        [HttpPost("DealerDocumentDelete")]
        [Authorize]
        public async Task<IActionResult> DealerDocumentDelete([FromBody] DealerDocSearch obj)
        {
            try
            {
                var t1 = Task.Run(() => bDealer.DealerDocumentDelete(obj));
                await Task.WhenAll(t1);
                objList = t1.Status == TaskStatus.RanToCompletion ? t1.Result : objList;
                return Ok(Newtonsoft.Json.JsonConvert.SerializeObject(objList));
            }
            catch (Exception ex)
            {
                dbLogger.PostErrorLog("Dealer", ex.Message.ToString(), "DealerDocumentDelete", 10001, "Admin", true);
                return Ok(Newtonsoft.Json.JsonConvert.SerializeObject(common));
            }
        }
        [HttpPost("GenerateOtp")]
        public async Task<IActionResult> GenerateOtp([FromBody] DealerAgreement obj)
        {
            try
            {
                var t1 = Task.Run(() => bDealer.GenerateOtp(obj));
                await Task.WhenAll(t1);
                objList = t1.Status == TaskStatus.RanToCompletion ? t1.Result : objList;
                return Ok(Newtonsoft.Json.JsonConvert.SerializeObject(objList));
            }
            catch (Exception ex)
            {
                dbLogger.PostErrorLog("Dealer", ex.Message.ToString(), "GenerateOtp", 10001, "Admin", true);
                return Ok(Newtonsoft.Json.JsonConvert.SerializeObject(common));
            }
        }

        [HttpPost("VerifyOTP")]
        public async Task<IActionResult> VerifyOTP([FromBody] DealerAgreement obj)
        {
            try
            {
                var t1 = Task.Run(() => bDealer.VerifyOTP(obj));
                await Task.WhenAll(t1);
                objList = t1.Status == TaskStatus.RanToCompletion ? t1.Result : objList;
                return Ok(Newtonsoft.Json.JsonConvert.SerializeObject(objList));
            }
            catch (Exception ex)
            {
                dbLogger.PostErrorLog("Dealer", ex.Message.ToString(), "VerifyOTP", 10001, "Dealer", true);
                return Ok(Newtonsoft.Json.JsonConvert.SerializeObject(objList));
            }
        }

        [HttpPost("AcceptDealerAgreement")]
        public async Task<IActionResult> AcceptDealerAgreement([FromBody] DealerAgreement obj)
        {
            try
            {
                var t1 = Task.Run(() => bDealer.AcceptDealerAgreement(obj));
                await Task.WhenAll(t1);
                common = t1.Status == TaskStatus.RanToCompletion ? t1.Result : common;
                return Ok(Newtonsoft.Json.JsonConvert.SerializeObject(common));
            }
            catch (Exception ex)
            {
                dbLogger.PostErrorLog("Dealer", ex.Message.ToString(), "AcceptDealerAgreement", 10001, "Admin", true);
                return Ok(Newtonsoft.Json.JsonConvert.SerializeObject(common));
            }
        }
        [HttpPost("GetAgreementFile")]
        public async Task<IActionResult> GetAgreementFile([FromBody] DealerAgreement obj)
        {
            try
            {
                var t1 = Task.Run(() => bDealer.GetAgreementFile(obj));
                await Task.WhenAll(t1);
                common = t1.Status == TaskStatus.RanToCompletion ? t1.Result : common;
                return Ok(Newtonsoft.Json.JsonConvert.SerializeObject(common));
            }
            catch (Exception ex)
            {
                dbLogger.PostErrorLog("Dealer", ex.Message.ToString(), "GetAgreementFile", 10001, "Admin", true);
                return Ok(Newtonsoft.Json.JsonConvert.SerializeObject(common));
            }
        }
        [HttpPost("GetOtherAgreementDocuments")]
        public async Task<IActionResult> GetOtherAgreementDocuments([FromBody] AgreementDoc agreementDoc)
        {
            try
            {
                var t1 = Task.Run(() => bDealer.GetOtherAgreementDocuments(agreementDoc));
                await Task.WhenAll(t1);
                common = t1.Status == TaskStatus.RanToCompletion ? t1.Result : common;
                return Ok(Newtonsoft.Json.JsonConvert.SerializeObject(common));
            }
            catch (Exception ex)
            {
                dbLogger.PostErrorLog("Dealer", ex.Message.ToString(), "GetAgreementFile", 10001, "Admin", true);
                return Ok(Newtonsoft.Json.JsonConvert.SerializeObject(common));
            }
        }
        [HttpPost("DownloadAgreementDocuments")]
        public async Task<IActionResult> DownloadAgreementDocuments([FromBody] AgreementDoc agreementDoc)
        {
            try
            {
                var t1 = Task.Run(() => bDealer.DownloadAgreementDocuments(agreementDoc));
                await Task.WhenAll(t1);
                common = t1.Status == TaskStatus.RanToCompletion ? t1.Result : common;
                return Ok(Newtonsoft.Json.JsonConvert.SerializeObject(common));
            }
            catch (Exception ex)
            {
                dbLogger.PostErrorLog("Dealer", ex.Message.ToString(), "GetAgreementFile", 10001, "Admin", true);
                return Ok(Newtonsoft.Json.JsonConvert.SerializeObject(common));
            }
        }
    }
}
