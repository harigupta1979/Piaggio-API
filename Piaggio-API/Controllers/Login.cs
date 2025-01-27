using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Logger;
using AppConfig;
using Core.Module;
using BusinessLogic;
using Piaggio_API.JWT;

namespace Piaggio_API.Controllers
{
    //[Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class Login : ControllerBase
    {
        
        private readonly IConfigManager _configuration; private readonly IJwtAuth jwtAuth;
        DbLogger dbLogger; BLogin dLogin;
        CommonIUD common = new CommonIUD(); CommonList objList = new CommonList();
        public Login(IConfigManager configuration, IJwtAuth jwtAuth)
        {
            this._configuration = configuration;
            dbLogger=new DbLogger(this._configuration);
            dLogin = new BLogin(this._configuration);
            this.jwtAuth = jwtAuth;
        }

        [HttpPost("GetLogin")]
        public async Task<IActionResult> GetLogin([FromBody] Users obj)
        {
            
            try
            {
                var t1 = Task.Run(() => dLogin.GetLogin(obj));
                await Task.WhenAll(t1);
                objList = t1.Status == TaskStatus.RanToCompletion ? t1.Result : objList;
                if (objList.FinalMode == "DataFound")
                {
                    var token = jwtAuth.AuthenticationToken(obj.Username, "admin", objList);
                    objList.AdditionalParameter = token;
                }
               
                return Ok(Newtonsoft.Json.JsonConvert.SerializeObject(objList));
            }
            catch (Exception ex)
            {
                dbLogger.PostErrorLog("Login", ex.Message.ToString(), "GetLogin", 10001, "Admin", true);
                return Ok(Newtonsoft.Json.JsonConvert.SerializeObject(objList));
            }
        }

        [HttpPost("GetResetPassword")]
        public async Task<IActionResult> GetResetPassword([FromBody] UserResetPwd obj)
        {
            try
            {
                var t1 = Task.Run(() => dLogin.GetResetPassword(obj));
                await Task.WhenAll(t1);
                objList = t1.Status == TaskStatus.RanToCompletion ? t1.Result : objList;
                return Ok(Newtonsoft.Json.JsonConvert.SerializeObject(objList));
            }
            catch (Exception ex)
            {
                dbLogger.PostErrorLog("Login", ex.Message.ToString(), "GetResetPassword", 10002, obj.Username, true);
                return Ok(Newtonsoft.Json.JsonConvert.SerializeObject(objList));
            }
        }

        [HttpPost("CheckUserLogin")]
        public async Task<IActionResult> CheckUserLogin([FromBody] Users obj)
        {
            try
            {
                var t1 = Task.Run(() => dLogin.CheckUserLogin(obj));
                await Task.WhenAll(t1);
                objList = t1.Status == TaskStatus.RanToCompletion ? t1.Result : objList;
                return Ok(Newtonsoft.Json.JsonConvert.SerializeObject(objList));
            }
            catch (Exception ex)
            {
                dbLogger.PostErrorLog("Login", ex.Message.ToString(), "CheckUserLogin", 10002, obj.Username, true);
                return Ok(Newtonsoft.Json.JsonConvert.SerializeObject(objList));
            }
        }
       


     /*   [HttpPost("UpdateAccountLockedStatus")]
        public async Task<IActionResult> UpdateAccountLockedStatus([FromBody] Users obj)
        {
            try
            {
                var t1 = Task.Run(() => dLogin.UpdateAccountLockedStatus(obj));
                await Task.WhenAll(t1);
                objList = t1.Status == TaskStatus.RanToCompletion ? t1.Result : objList;
                return Ok(Newtonsoft.Json.JsonConvert.SerializeObject(objList));
            }
            catch (Exception ex)
            {
                dbLogger.PostErrorLog("Login", ex.Message.ToString(), "UpdateAccountLockedStatus", 10002, obj.Username, true);
                return Ok(Newtonsoft.Json.JsonConvert.SerializeObject(objList));
            }
        }*/
        [HttpPost("GetUserActivityLog")]
        public async Task<IActionResult> GetUserActivityLog([FromBody] UserActivity obj)
        {
            try
            {
                var t1 = Task.Run(() => dLogin.GetUserActivityLog(obj));
                await Task.WhenAll(t1);
                objList = t1.Status == TaskStatus.RanToCompletion ? t1.Result : objList;
                return Ok(Newtonsoft.Json.JsonConvert.SerializeObject(objList));
            }
            catch (Exception ex)
            {
                dbLogger.PostErrorLog("Login", ex.Message.ToString(), "GetUserActivityLog", 10001, "Admin", true);
                return Ok(Newtonsoft.Json.JsonConvert.SerializeObject(common));
            }
        }

        [HttpPost("GenerateUserOTP")]
        public async Task<IActionResult> GenerateUserOTP([FromBody] UserOTP obj)
        {
            try
            {
                var t1 = Task.Run(() => dLogin.GenerateUserOTP(obj));
                await Task.WhenAll(t1);
                objList = t1.Status == TaskStatus.RanToCompletion ? t1.Result : objList;
                return Ok(Newtonsoft.Json.JsonConvert.SerializeObject(objList));
            }
            catch (Exception ex)
            {
                dbLogger.PostErrorLog("Login", ex.Message.ToString(), "GenerateUserOTP", 10001, "Admin", true);
                return Ok(Newtonsoft.Json.JsonConvert.SerializeObject(objList));
            }
        }

        [HttpPost("VerifyUserOTP")]
        public async Task<IActionResult> VerifyUserOTP([FromBody] UserOTP obj)
        {
            try
            {
                var t1 = Task.Run(() => dLogin.VerifyUserOTP(obj));
                await Task.WhenAll(t1);
                objList = t1.Status == TaskStatus.RanToCompletion ? t1.Result : objList;
                return Ok(Newtonsoft.Json.JsonConvert.SerializeObject(objList));
            }
            catch (Exception ex)
            {
                dbLogger.PostErrorLog("Login", ex.Message.ToString(), "VerifyUserOTP", 10001, "Admin", true);
                return Ok(Newtonsoft.Json.JsonConvert.SerializeObject(objList));
            }
        }

        [HttpPost("UpdateUserPassword")]
        public async Task<IActionResult> UpdateUserPassword([FromBody] UserOTP obj)
        {
            try
            {
                var t1 = Task.Run(() => dLogin.UpdateUserPassword(obj));
                await Task.WhenAll(t1);
                objList = t1.Status == TaskStatus.RanToCompletion ? t1.Result : objList;
                return Ok(Newtonsoft.Json.JsonConvert.SerializeObject(objList));
            }
            catch (Exception ex)
            {
                dbLogger.PostErrorLog("Login", ex.Message.ToString(), "UpdateUserPassword", 10001, "Admin", true);
                return Ok(Newtonsoft.Json.JsonConvert.SerializeObject(objList));
            }
        }

        [HttpPost("GetUserInfo")]
        public async Task<IActionResult> GetUserInfo([FromBody] UserInfo obj)
        {
            try
            {
                var t1 = Task.Run(() => dLogin.GetUserInfo(obj));
                await Task.WhenAll(t1);
                objList = t1.Status == TaskStatus.RanToCompletion ? t1.Result : objList;
                return Ok(Newtonsoft.Json.JsonConvert.SerializeObject(objList));
            }
            catch (Exception ex)
            {
                dbLogger.PostErrorLog("Login", ex.Message.ToString(), "GetUserInfo", 10002, obj.UserId.ToString(), true);
                return Ok(Newtonsoft.Json.JsonConvert.SerializeObject(objList));
            }
        }
    }
}

    

