using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using DemoProjectWebApp.DAO;
using DemoProjectWebApp.Models;
using System.Net.Http.Headers;
using System.Text.Json;


namespace DemoProjectWebApp.Controllers
{
    public class GroupMasterController : Controller
    {
        public ClsAuthentication common = new ClsAuthentication();
        private readonly ILogger<GroupMasterController> _logger;
        public IConfiguration _configuration { get; }

        public GroupMasterController(ILogger<GroupMasterController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public async Task<JsonResult> GetDropDownValues(string DropDownParametre)
        {
            #region Get All DropDownValue

            //string DropDownParametre = "GNL_Currency";
            IEnumerable<ValueSetDetail> dropd = null;
            string endpoint = _configuration.GetSection("GetDropDownValues").Value;
            using (var client = new HttpClient())
            {



                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", common.GenerateToken());
                //var responseTask = client.GetAsync(endpoint);
                var responseTask = client.GetAsync(endpoint + "&RecAddBy=" + 2 + "&RequestType=" + DropDownParametre);
                responseTask.Wait()
                    ;

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var json = await result.Content.ReadAsStringAsync();

                    json = json.Replace("{}", "null");

                    dropd = System.Text.Json.JsonSerializer.Deserialize<IEnumerable<ValueSetDetail>>(json, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                }
            }


            if (dropd == null)
            {
                return Json(new ValueSetDetail());
            }

            #endregion

            return Json(dropd.ToList());
        }

        public async Task<List<ValueSetDetail>> GetDropDownValuesListWithParam(string DropDownParametre)
        {
            try
            {
                IEnumerable<ValueSetDetail> dropd = null;

                string endpoint = _configuration.GetSection("GetDropDownValues").Value;

                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue("Bearer", common.GenerateToken());

                    var result = await client.GetAsync(
                        endpoint + "&RecAddBy=2&RequestType=" + DropDownParametre
                    );

                    if (result.IsSuccessStatusCode)
                    {
                        var json = await result.Content.ReadAsStringAsync();

                        json = json.Replace("{}", "null");

                        dropd = System.Text.Json.JsonSerializer.Deserialize<List<ValueSetDetail>>(
                            json,
                            new JsonSerializerOptions
                            {
                                PropertyNameCaseInsensitive = true
                            }
                        );
                    }
                }

                return dropd?.ToList() ?? new List<ValueSetDetail>();
            }
            catch (Exception ex)
            {
                // optional: log error
                _logger.LogError("GetDropDownValues Error: {0}", ex.Message);

                return new List<ValueSetDetail>();
            }
        }

        public async Task<List<COA>> GetDropDownCOA(string DropDownParametre)
        {
            try
            {
                IEnumerable<COA> dropd = null;

                string endpoint = _configuration.GetSection("GetDropDownValuesCOA").Value;

                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue("Bearer", common.GenerateToken());

                    var result = await client.GetAsync(
                        endpoint + "&RecAddBy=2&RequestType=" + DropDownParametre
                    );

                    if (result.IsSuccessStatusCode)
                    {
                        var json = await result.Content.ReadAsStringAsync();

                        json = json.Replace("{}", "null");

                        dropd = System.Text.Json.JsonSerializer.Deserialize<List<COA>>(
                            json,
                            new JsonSerializerOptions
                            {
                                PropertyNameCaseInsensitive = true
                            }
                        );
                    }
                }

                return dropd?.ToList() ?? new List<COA>();
            }
            catch (Exception ex)
            {
                // optional: log error
                _logger.LogError("GetDropDownValues Error: {0}", ex.Message);

                return new List<COA>();
            }
        }
        public async Task<IActionResult> Index(int? id, string mode = "Create")
        {
            try
            {
                ViewBag.Mode = mode;
                    
                if (HttpContext.Session.GetString("UserID") == null)
                {
                    return RedirectToAction("Login", "LoginModel");
                }

                List<ValueSetDetail> accountRequests = await GetDropDownValuesListWithParam("Account Type");

                List<ValueSetDetail> accountRequestsLedger = await GetDropDownValuesListWithParam("Ledger");

                List<ValueSetDetail> accountRequestsSourceBaseVal = await GetDropDownValuesListWithParam("Source Base Value");

                List<ValueSetDetail> accountRequestsCalcType = await GetDropDownValuesListWithParam("CalcType");

                List<ValueSetDetail> accountRequestsProrate = await GetDropDownValuesListWithParam("Pro-rate");

                List<ValueSetDetail> accountRequestsCalcImpact = await GetDropDownValuesListWithParam("Calc-Impact");

                List<COA> coalist = await GetDropDownCOA("");

                // Map accountRequests to dropdown items
                ViewData["actions"] = accountRequests?
                        .Select(x => new SelectListItem
                        {
                            Value = x.ValueSetItemCode.ToString(),
                            Text = x.RefText1.ToString(),
                        })
                        .ToList() ?? new List<SelectListItem>();

                ViewData["Ledger"] = accountRequestsLedger?
                        .Select(x => new SelectListItem
                        {
                            Value = x.ValueSetItemCode.ToString(),
                            Text = x.RefText1.ToString(),
                        })
                        .ToList() ?? new List<SelectListItem>();

                ViewData["SourceBaseVal"] = accountRequestsSourceBaseVal?
                        .Select(x => new SelectListItem
                        {
                            Value = x.ValueSetItemCode.ToString(),
                            Text = x.RefText1.ToString(),
                        })
                        .ToList() ?? new List<SelectListItem>();

                ViewData["CalcTypeVal"] = accountRequestsCalcType?
                        .Select(x => new SelectListItem
                        {
                            Value = x.ValueSetItemCode.ToString(),
                            Text = x.RefText1.ToString(),
                        })
                        .ToList() ?? new List<SelectListItem>();

                ViewData["ProrateVal"] = accountRequestsProrate?
                        .Select(x => new SelectListItem
                        {
                            Value = x.ValueSetItemCode.ToString(),
                            Text = x.RefText1.ToString(),
                        })
                        .ToList() ?? new List<SelectListItem>();

                ViewData["CalcImpactVal"] = accountRequestsCalcImpact?
                        .Select(x => new SelectListItem
                        {
                            Value = x.ValueSetItemCode.ToString(),
                            Text = x.RefText1.ToString(),
                        })
                        .ToList() ?? new List<SelectListItem>();

                ViewData["coalistVal"] = coalist?
                        .Select(x => new SelectListItem
                        {
                            Value = x.AccountCode.ToString(),
                            Text = x.AccountDesc.ToString(),
                        })
                        .ToList() ?? new List<SelectListItem>();

                if (mode == "Edit" || mode == "View")
                {
                    return View(await GetRecordById(id));
                }
             

                GroupMaster groupmaster = new GroupMaster();

                return View(groupmaster);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(GroupMaster model, string mode)
        {
            try
            {
                var userIdStr = HttpContext.Session.GetString("UserID");

                if (string.IsNullOrEmpty(userIdStr))
                    return RedirectToAction("Login", "LoginModel");

                int userId = Convert.ToInt32(userIdStr);

                List<ValueSetDetail> accountRequests = await GetDropDownValuesListWithParam("Account Type");

                List<ValueSetDetail> accountRequestsLedger = await GetDropDownValuesListWithParam("Ledger");

                List<ValueSetDetail> accountRequestsSourceBaseVal = await GetDropDownValuesListWithParam("Source Base Value");

                List<ValueSetDetail> accountRequestsCalcType = await GetDropDownValuesListWithParam("CalcType");

                List<ValueSetDetail> accountRequestsProrate = await GetDropDownValuesListWithParam("Pro-rate");

                List<ValueSetDetail> accountRequestsCalcImpact = await GetDropDownValuesListWithParam("Calc-Impact");

                List<COA> coalist = await GetDropDownCOA("");

                // Map accountRequests to dropdown items
                ViewData["actions"] = accountRequests?
                        .Select(x => new SelectListItem
                        {
                            Value = x.ValueSetItemCode.ToString(),
                            Text = x.RefText1.ToString(),
                        })
                        .ToList() ?? new List<SelectListItem>();

                ViewData["Ledger"] = accountRequestsLedger?
                        .Select(x => new SelectListItem
                        {
                            Value = x.ValueSetItemCode.ToString(),
                            Text = x.RefText1.ToString(),
                        })
                        .ToList() ?? new List<SelectListItem>();

                ViewData["SourceBaseVal"] = accountRequestsSourceBaseVal?
                        .Select(x => new SelectListItem
                        {
                            Value = x.ValueSetItemCode.ToString(),
                            Text = x.RefText1.ToString(),
                        })
                        .ToList() ?? new List<SelectListItem>();

                ViewData["CalcTypeVal"] = accountRequestsCalcType?
                        .Select(x => new SelectListItem
                        {
                            Value = x.ValueSetItemCode.ToString(),
                            Text = x.RefText1.ToString(),
                        })
                        .ToList() ?? new List<SelectListItem>();

                ViewData["ProrateVal"] = accountRequestsProrate?
                        .Select(x => new SelectListItem
                        {
                            Value = x.ValueSetItemCode.ToString(),
                            Text = x.RefText1.ToString(),
                        })
                        .ToList() ?? new List<SelectListItem>();

                ViewData["CalcImpactVal"] = accountRequestsCalcImpact?
                        .Select(x => new SelectListItem
                        {
                            Value = x.ValueSetItemCode.ToString(),
                            Text = x.RefText1.ToString(),
                        })
                        .ToList() ?? new List<SelectListItem>();

                ViewData["coalistVal"] = coalist?
                        .Select(x => new SelectListItem
                        {
                            Value = x.AccountCode.ToString(),
                            Text = x.AccountDesc.ToString(),
                        })
                        .ToList() ?? new List<SelectListItem>();
                // ? Ensure list exists
                //if (model.DetailList == null)
                //    model.DetailList = new List<GroupDetail>();

                // ? Set user fields safely
                if (model.DetailList == null)
                {
                    ViewBag.ShowPopup = true;
                    ViewBag.MainHeading = "Error Message ?";
                    ViewBag.DialogTitle = "Kindly Filled Group Detail";
                    return View(model);
                }
                if (mode == "Create")
                {
                    foreach (var item in model.DetailList)
                    {
                        item.rec_add_by = userId;
                        item.Mode = mode;
                    }

                    model.rec_add_by = userId;
                }
                else if (mode == "Edit")
                {
                    foreach (var item in model.DetailList)
                    {
                        item.rec_modified_by = userId;
                    }

                    model.rec_modified_by = userId;
                }
                model.Mode = mode;

                
                if (ModelState.IsValid)
                {
                    string endpoint = _configuration.GetSection("InsertMasterGroupMaster").Value;

                    if (mode == "Edit")
                        model.rec_modified_by = Convert.ToInt32(HttpContext.Session.GetString("UserID"));
                    else
                        model.rec_add_by = Convert.ToInt32(HttpContext.Session.GetString("UserID"));

                    

                    string Result = await common.APIMethodPost(endpoint, "", model, "POST", common.GenerateToken());

                    if (Result == "Success")
                    {


                        TempData["ShowPopUp"] = true;
                        TempData["MainHeading"] = "Successfull !";
                        TempData["DialogTitle"] = $" Group Data has been {mode} Succesfully";
                        return RedirectToAction("Main", "GroupMaster");
                    }
                    else
                    {
                        ViewBag.ShowPopup = true;
                        ViewBag.MainHeading = "Failed !";
                        ViewBag.DialogTitle = $" Group has No been {mode}";
                        ModelState.AddModelError(string.Empty, "Update failed. " + Result);
                    }

                    //if (Result == "Success")
                    //    return RedirectToAction("Main", "GroupMaster");
                    //else
                    //    ModelState.AddModelError(string.Empty, "Operation failed. " + Result);
                }

                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in GroupMasterController: " + ex.Message);
                return BadRequest();
            }
        }

        public IActionResult Main()
        {
            if (HttpContext.Session.GetString("UserID") == null)
                return RedirectToAction("Login", "LoginModel");

            ViewBag.MainHeading = TempData["MainHeading"];
            ViewBag.DialogTitle = TempData["DialogTitle"];
            ViewBag.ShowPopup = Convert.ToBoolean(TempData["ShowPopUp"] ?? false);
            

            TempData.Clear();

            return View();
        }


        private async Task<List<GroupDetail>> GetRuleDetailsAsync(int Id)
        {
            var details = new List<GroupDetail>();
            string endpoint = _configuration.GetSection("GetByIdGroupDetail").Value + "/" + Id + "?spName=[SP_GetGroupDetailData]";
            using (var client = new HttpClient())
            {
                client.Timeout = TimeSpan.FromSeconds(30);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", common.GenerateToken());
                var res = await client.GetAsync(endpoint);
                if (res.IsSuccessStatusCode)
                {
                    var data = await res.Content.ReadAsStringAsync();
                    data = data.Replace("{}", "null");
                    details = JsonConvert.DeserializeObject<List<GroupDetail>>(data);
                }
            }
            return details;
        }




        public async Task<ActionResult> DiscardRecord(int id, string mode)
        {
            try
            {
                GroupMaster req = new GroupMaster();
                req.Id = id;
                req.Mode = mode;
                req.rec_modified_by = Convert.ToInt32(HttpContext.Session.GetString("UserID"));
                req.DetailList = new List<GroupDetail>();   
                string endpoint = _configuration.GetSection("InsertMasterGroupMaster").Value;
                string result = await common.APIMethodPost(endpoint, "", req, "POST", common.GenerateToken());

                if (result == "Success")
                    return RedirectToAction("Main", "GroupMaster");

                ModelState.AddModelError("", "Discard failed. " + result);
                return View(req);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in DiscardRecord: " + ex.Message);
                return View();
            }
        }

        private async Task<GroupMaster> GetRecordById(int? id)
        {
            if (id == null) return null;

            string endpoint = _configuration.GetSection("GetByIdGroupMaster").Value + "/" + id + "?spName=[SP_GetGroupMasterData]";

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = 
                    new AuthenticationHeaderValue("Bearer", common.GenerateToken());

                var res = await client.GetAsync(endpoint);

                if (!res.IsSuccessStatusCode)
                    return null;

                var data = await res.Content.ReadAsStringAsync();
                data = data.Replace("{{}}", "null");
                var obj = JsonConvert.DeserializeObject<List<GroupMaster>>(data);
                var singleItem = obj.FirstOrDefault();
                if (singleItem != null)
                {
                    singleItem.DetailList = await GetRuleDetailsAsync(singleItem.Id);
                    return singleItem;
                }
                return singleItem;
                //return obj.FirstOrDefault();
            }
        }

        public async Task<ActionResult> View([DataSourceRequest] DataSourceRequest request)
        {
            try
            {
                IEnumerable<GroupMaster> dataList = null;
                string endpoint = _configuration.GetSection("GetALLMasterGroupMaster").Value;

                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue("Bearer", common.GenerateToken());

                    var response = await client.GetAsync(endpoint);

                    if (response.IsSuccessStatusCode)
                    {
                        var json = await response.Content.ReadAsStringAsync();
                        json = json.Replace("{{}}", "null");
                        dataList = System.Text.Json.JsonSerializer.Deserialize<IEnumerable<GroupMaster>>(json);
                    }
                }

                return Json(dataList.ToDataSourceResult(request));
            }
            catch (Exception ex)
            {
                return Json(new { Errors = "Error loading data." });
            }
        }
    }
}