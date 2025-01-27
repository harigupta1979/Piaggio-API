using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Logger;
using AppConfig;
using Core.Module;
using BusinessLogic.UserManagement;
using Core.Module.UserManagement;
using Microsoft.AspNetCore.Authorization;
using Core.Module.UserManageMent;

namespace ICICI_Dealer_API.Controllers.UserManagement
{
    [Route("api/[controller]")]
    [ApiController]
    public class Role : ControllerBase
    {
        private readonly IConfigManager _configuration;
        DbLogger dbLogger; BRole bRole;
        CommonIUD common = new CommonIUD(); CommonList objList;
        public Role(IConfigManager configuration)
        {
            this._configuration = configuration;
            dbLogger = new DbLogger(this._configuration);
            bRole = new BRole(this._configuration);
        }
        [HttpPost("PostRole")]
        [Authorize]
        public async Task<IActionResult> PostRole([FromBody] RoleClass obj)
        {
            try
            {
                var t1 = Task.Run(() => bRole.PostRole(obj));
                await Task.WhenAll(t1);
                common = t1.Status == TaskStatus.RanToCompletion ? t1.Result : common;
                return Ok(Newtonsoft.Json.JsonConvert.SerializeObject(common));
            }
            catch (Exception ex)
            {
                dbLogger.PostErrorLog("Role", ex.Message.ToString(), "PostRole", 10001, "Admin", true);
                return Ok(Newtonsoft.Json.JsonConvert.SerializeObject(common));
            }
        }

        [HttpPost("GetRole")]
        [Authorize]
        public async Task<IActionResult> GetRole([FromBody] RoleSearch obj)
        {
            try
            {
                var t1 = Task.Run(() => bRole.GetRole(obj));
                await Task.WhenAll(t1);
                objList = t1.Status == TaskStatus.RanToCompletion ? t1.Result : objList;
                return Ok(Newtonsoft.Json.JsonConvert.SerializeObject(objList));
            }
            catch (Exception ex)
            {
                dbLogger.PostErrorLog("Role", ex.Message.ToString(), "GetRole", 10001, "Admin", true);
                return Ok(Newtonsoft.Json.JsonConvert.SerializeObject(objList));
            }
        }
       
        [HttpPost("GetRoleMenu")]
        [Authorize]
        public async Task<IActionResult> GetRoleMenu([FromBody] RoleMenu obj)
        {
            try
            {
                var t1 = Task.Run(() => bRole.GetRoleMenu(obj));
                await Task.WhenAll(t1);
                objList = t1.Status == TaskStatus.RanToCompletion ? t1.Result : objList;
                return Ok(Newtonsoft.Json.JsonConvert.SerializeObject(objList));
            }
            catch (Exception ex)
            {
                dbLogger.PostErrorLog("Role", ex.Message.ToString(), "GetRoleMenu", 10001, "Admin", true);
                return Ok(Newtonsoft.Json.JsonConvert.SerializeObject(objList));
            }
        }
    }
}
