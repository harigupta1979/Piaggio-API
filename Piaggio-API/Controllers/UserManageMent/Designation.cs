using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Logger;
using AppConfig;
using Core.Module;
using BusinessLogic.UserManagement;
using Core.Module.UserManagement;
using Microsoft.AspNetCore.Authorization;

namespace Piaggio_API.Controllers.UserManagement
{
    [Route("api/[controller]")]
    [ApiController]
    public class Designation : ControllerBase
    {
        private readonly IConfigManager _configuration;
        DbLogger dbLogger; BDesignation bDesignation;
        CommonIUD common = new CommonIUD(); CommonList objList;
        public Designation(IConfigManager configuration)
        {
            this._configuration = configuration;
            dbLogger = new DbLogger(this._configuration);
            bDesignation = new BDesignation(this._configuration);
        }
        [HttpPost("PostDesignation")]
        [Authorize]
        public async Task<IActionResult> PostDesignation([FromBody] DesignationClass obj)
        {
            try
            {
                var t1 = Task.Run(() => bDesignation.PostDesignation(obj));
                await Task.WhenAll(t1);
                common = t1.Status == TaskStatus.RanToCompletion ? t1.Result : common;
                return Ok(Newtonsoft.Json.JsonConvert.SerializeObject(common));
            }
            catch (Exception ex)
            {
                dbLogger.PostErrorLog("Designation", ex.Message.ToString(), "PostDesignation", 10001, "Admin", true);
                return Ok(Newtonsoft.Json.JsonConvert.SerializeObject(common));
            }
        }

        [HttpPost("GetDesignation")]
        [Authorize]
        public async Task<IActionResult> GetDesignation([FromBody] DesignationSearch obj)
        {
            try
            {
                var t1 = Task.Run(() => bDesignation.GetDesignation(obj));
                await Task.WhenAll(t1);
                objList = t1.Status == TaskStatus.RanToCompletion ? t1.Result : objList;
                return Ok(Newtonsoft.Json.JsonConvert.SerializeObject(objList));
            }
            catch (Exception ex)
            {
                dbLogger.PostErrorLog("Designation", ex.Message.ToString(), "GetDesignation", 10001, "Admin", true);
                return Ok(Newtonsoft.Json.JsonConvert.SerializeObject(objList));
            }
        }
    }
}
