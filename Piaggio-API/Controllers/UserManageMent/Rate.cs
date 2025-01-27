using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Logger;
using AppConfig;
using Core.Module;
using Microsoft.AspNetCore.Authorization;
using BusinessLogic.UserManageMent;
using Core.Module.UserManageMent;

namespace ICICI_Dealer_API.Controllers.UserManageMent
{
    [Route("api/[controller]")]
    [ApiController]
    public class Rate : ControllerBase
    {
        private readonly IConfigManager _configuration;
        DbLogger dbLogger; BRate bRate;
        CommonIUD common = new CommonIUD(); CommonList objList;
        public Rate(IConfigManager configuration)
        {
            this._configuration = configuration;
            dbLogger = new DbLogger(this._configuration);
            bRate = new BRate(this._configuration);
        }
        [HttpPost("PostRate")]
        [Authorize]
        public async Task<IActionResult> PostRate([FromBody] RateClass obj)
        {
            try
            {
                var t1 = Task.Run(() => bRate.PostRate(obj));
                await Task.WhenAll(t1);
                common = t1.Status == TaskStatus.RanToCompletion ? t1.Result : common;
                return Ok(Newtonsoft.Json.JsonConvert.SerializeObject(common));
            }
            catch (Exception ex)
            {
                dbLogger.PostErrorLog("Rate", ex.Message.ToString(), "PostRate", 10001, "Admin", true);
                return Ok(Newtonsoft.Json.JsonConvert.SerializeObject(common));
            }
        }

        [HttpPost("GetRate")]
        [Authorize]
        public async Task<IActionResult> GetRate([FromBody] RateSearch obj)
        {
            try
            {
                var t1 = Task.Run(() => bRate.GetRate(obj));
                await Task.WhenAll(t1);
                objList = t1.Status == TaskStatus.RanToCompletion ? t1.Result : objList;
                return Ok(Newtonsoft.Json.JsonConvert.SerializeObject(objList));
            }
            catch (Exception ex)
            {
                dbLogger.PostErrorLog("Rate", ex.Message.ToString(), "GetRate", 10001, "Admin", true);
                return Ok(Newtonsoft.Json.JsonConvert.SerializeObject(objList));
            }
        }
    }
}
