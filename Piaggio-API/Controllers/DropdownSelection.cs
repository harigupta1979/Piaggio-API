using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Logger;
using AppConfig;
using Core.Module;
using BusinessLogic;
using Microsoft.AspNetCore.Authorization;

namespace Piaggio_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DropdownSelection : ControllerBase
    {
        private readonly IConfigManager _configuration;
        DbLogger dbLogger; BSelection bSelection;
        CommonList objList = new CommonList();
        public DropdownSelection(IConfigManager configuration)
        {
            this._configuration = configuration;
            dbLogger = new DbLogger(this._configuration);
            bSelection = new BSelection(this._configuration);
        }

        [HttpPost("GetSelection")]
        [Authorize]
        public async Task<IActionResult> GetSelection([FromBody] SelectionClass obj)
        {
            try
            {
                var t1 = Task.Run(() => bSelection.GetSelection(obj));
                await Task.WhenAll(t1);
                objList = t1.Status == TaskStatus.RanToCompletion ? t1.Result : objList;
                return Ok(Newtonsoft.Json.JsonConvert.SerializeObject(objList));
            }
            catch (Exception ex)
            {
                dbLogger.PostErrorLog("DropdownSelection", ex.Message.ToString(), "GetSelection", 10001, "Admin", true);
                return Ok(Newtonsoft.Json.JsonConvert.SerializeObject(objList));
            }
        }

        [HttpPost("GetLocationByPinCode")]
        //[Authorize]
        public async Task<IActionResult> GetLocationByPinCode([FromBody] PinCodeClass obj)
        {
            try
            {
                var t1 = Task.Run(() => bSelection.GetLocationByPinCode(obj));
                await Task.WhenAll(t1);
                objList = t1.Status == TaskStatus.RanToCompletion ? t1.Result : objList;
                return Ok(Newtonsoft.Json.JsonConvert.SerializeObject(objList));
            }
            catch (Exception ex)
            {
                dbLogger.PostErrorLog("DropdownSelection", ex.Message.ToString(), "GetLocationByPinCode", 10001, "Admin", true);
                return Ok(Newtonsoft.Json.JsonConvert.SerializeObject(objList));
            }
        }

        [HttpPost("GetRoleBaseAccessControl")]
        [Authorize]
        public async Task<IActionResult> GetRoleBaseAccessControl([FromBody] SelectionClass obj)
        {
            try
            {
                var t1 = Task.Run(() => bSelection.GetRoleBaseAccessControl(obj));
                await Task.WhenAll(t1);
                objList = t1.Status == TaskStatus.RanToCompletion ? t1.Result : objList;
                return Ok(Newtonsoft.Json.JsonConvert.SerializeObject(objList));
            }
            catch (Exception ex)
            {
                dbLogger.PostErrorLog("DropdownSelection", ex.Message.ToString(), "GetRoleBaseAccessControl", 10001, "Admin", true);
                return Ok(Newtonsoft.Json.JsonConvert.SerializeObject(objList));
            }
        }

        [HttpPost("GetRoleBaseAccessModule")]
        [Authorize]
        public async Task<IActionResult> GetRoleBaseAccessModule([FromBody] SelectionClass obj)
        {
            try
            {
                var t1 = Task.Run(() => bSelection.GetRoleBaseAccessModule(obj));
                await Task.WhenAll(t1);
                objList = t1.Status == TaskStatus.RanToCompletion ? t1.Result : objList;
                return Ok(Newtonsoft.Json.JsonConvert.SerializeObject(objList));
            }
            catch (Exception ex)
            {
                dbLogger.PostErrorLog("DropdownSelection", ex.Message.ToString(), "GetRoleBaseAccessModule", 10001, "Admin", true);
                return Ok(Newtonsoft.Json.JsonConvert.SerializeObject(objList));
            }
        }

        [HttpPost("GetDynemicmenu")]
        //[Authorize]
        public async Task<IActionResult> GetDynemicmenu([FromBody] SelectionClass obj)
        {
            try
            {
                var t1 = Task.Run(() => bSelection.GetDynemicmenu(obj));
                await Task.WhenAll(t1);
                objList = t1.Status == TaskStatus.RanToCompletion ? t1.Result : objList;
                return Ok(Newtonsoft.Json.JsonConvert.SerializeObject(objList));
            }
            catch (Exception ex)
            {
                dbLogger.PostErrorLog("DropdownSelection", ex.Message.ToString(), "GetDynemicmenu", 10001, "Admin", true);
                return Ok(Newtonsoft.Json.JsonConvert.SerializeObject(objList));
            }
        }
        [HttpPost("GetSelectionAutoCmt")]
        //[Authorize]
        public async Task<IActionResult> GetSelectionAutoCmt([FromBody] SelectionClassAuto obj)
        {
            try
            {
                var t1 = Task.Run(() => bSelection.GetSelectionAutoCmt(obj));
                await Task.WhenAll(t1);
                objList = t1.Status == TaskStatus.RanToCompletion ? t1.Result : objList;
                return Ok(Newtonsoft.Json.JsonConvert.SerializeObject(objList));
            }
            catch (Exception ex)
            {
                dbLogger.PostErrorLog("DropdownSelection", ex.Message.ToString(), "GetdbSelectionAutoCmt", 10001, "Admin", true);
                return Ok(Newtonsoft.Json.JsonConvert.SerializeObject(objList));
            }
        }

        [HttpPost("GetDropDownDtl")]
        public async Task<IActionResult> GetDropDownDtl([FromBody] DropDowncommon obj)
        {
            try
            {

                var t1 = Task.Run(() => bSelection.Get_Dropdown_Details(obj));
                await Task.WhenAll(t1);
                objList = t1.Status == TaskStatus.RanToCompletion ? t1.Result : objList;
                return Ok(Newtonsoft.Json.JsonConvert.SerializeObject(objList));
            }
            catch (Exception ex)
            {
                dbLogger.PostErrorLog("DropdownSelection", ex.Message.ToString(), "GetDropDownDtl", 10001, "Admin", true);
                return Ok(Newtonsoft.Json.JsonConvert.SerializeObject(objList));
            }

        }
    }
}
