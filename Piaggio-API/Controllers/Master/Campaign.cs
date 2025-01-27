using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Logger;
using AppConfig;
using Core.Module;
using BusinessLogic.Master;
using Core.Module.Master;
using Microsoft.AspNetCore.Authorization;

namespace ICICI_Dealer_API.Controllers.Master
{
    [Route("api/[controller]")]
    [ApiController]
    public class Campaign : ControllerBase
    {
        private readonly IConfigManager _configuration;
        private DbLogger dbLogger; BCampaign bCampaign;
        CommonIUD common = new CommonIUD(); CommonList objList;
        public Campaign(IConfigManager configuration)
        {
            this._configuration = configuration;
            dbLogger = new DbLogger(this._configuration);
            bCampaign = new BCampaign(this._configuration);
        }
        [HttpPost("PostCampaign")]
        [Authorize]
        public async Task<IActionResult> PostCampaign([FromBody] CampaignClass obj)
        {
            try
            {
                var t1 = Task.Run(() => bCampaign.PostCampaign(obj));
                await Task.WhenAll(t1);
                common = t1.Status == TaskStatus.RanToCompletion ? t1.Result : common;
                return Ok(Newtonsoft.Json.JsonConvert.SerializeObject(common));
            }
            catch (Exception ex)
            {
                dbLogger.PostErrorLog("Campaign", ex.Message.ToString(), "PostCampaign", 10001, "Admin", true);
                return Ok(Newtonsoft.Json.JsonConvert.SerializeObject(common));
            }
        }

        [HttpPost("GetCampaign")]
        [Authorize]
        public async Task<IActionResult> GetCampaign([FromBody] CampaignSearch obj)
        {
            bCampaign = new BCampaign(this._configuration);
            try
            {
                var t1 = Task.Run(() => bCampaign.GetCampaign(obj));
                await Task.WhenAll(t1);
                objList = t1.Status == TaskStatus.RanToCompletion ? t1.Result : objList;
                return Ok(Newtonsoft.Json.JsonConvert.SerializeObject(objList));
            }
            catch (Exception ex)
            {
                dbLogger.PostErrorLog("Campaign", ex.Message.ToString(), "GetCampaign", 10001, "Admin", true);
                return Ok(Newtonsoft.Json.JsonConvert.SerializeObject(objList));
            }
        }
    }
}
