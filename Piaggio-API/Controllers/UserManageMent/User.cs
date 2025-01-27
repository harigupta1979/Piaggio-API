using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Logger;
using AppConfig;
using Core.Module;
using BusinessLogic.UserManagement;
using Core.Module.UserManagement;
using Microsoft.AspNetCore.Authorization;

namespace Piaggio_API.Controllers.UserManageMent
{
    [Route("api/[controller]")]
    [ApiController]
    public class User: ControllerBase
    {
        private readonly IConfigManager _configuration;
        DbLogger dbLogger; BUser buser;
        CommonIUD common = new CommonIUD(); CommonList objList;
        public User(IConfigManager configuration)
        {
            this._configuration = configuration;
            dbLogger = new DbLogger(this._configuration);
            buser = new BUser(this._configuration);
        }
        [HttpPost("PostUser")]
        [Authorize]
        public async Task<IActionResult> PostUser([FromBody] UserClass obj)
        {
            try
            {
                var t1 = Task.Run(() => buser.PostUser(obj));
                await Task.WhenAll(t1);
                common = t1.Status == TaskStatus.RanToCompletion ? t1.Result : common;
                return Ok(Newtonsoft.Json.JsonConvert.SerializeObject(common));
            }
            catch (Exception ex)
            {
                dbLogger.PostErrorLog("User", ex.Message.ToString(), "PostUser", 10001, "Admin", true);
                return Ok(Newtonsoft.Json.JsonConvert.SerializeObject(common));
            }
        }

        [HttpPost("GetUser")]
        [Authorize]
        public async Task<IActionResult> GetUser([FromBody] UserSearch obj)
        {
            buser = new BUser(this._configuration);
            try
            {
                var t1 = Task.Run(() => buser.GetUser(obj));
                await Task.WhenAll(t1);
                objList = t1.Status == TaskStatus.RanToCompletion ? t1.Result : objList;
                return Ok(Newtonsoft.Json.JsonConvert.SerializeObject(objList));
            }
            catch (Exception ex)
            {
                dbLogger.PostErrorLog("User", ex.Message.ToString(), "GetUser", 10001, "Admin", true);
                return Ok(Newtonsoft.Json.JsonConvert.SerializeObject(objList));
            }
        }

        [HttpPost("GetUserProfileDetails")]
        public async Task<IActionResult> GetUserProfileDetails([FromBody] UserProfileClass obj)
        {
            buser = new BUser(this._configuration);
            try
            {
                var t1 = Task.Run(() => buser.GetUserProfileDetails(obj));
                await Task.WhenAll(t1);
                objList = t1.Status == TaskStatus.RanToCompletion ? t1.Result : objList;
                return Ok(Newtonsoft.Json.JsonConvert.SerializeObject(objList));
            }
            catch (Exception ex)
            {
                dbLogger.PostErrorLog("User", ex.Message.ToString(), "GetUserProfileDetails", 10001, "Admin", true);
                return Ok(Newtonsoft.Json.JsonConvert.SerializeObject(objList));
            }
        }
    }
}
