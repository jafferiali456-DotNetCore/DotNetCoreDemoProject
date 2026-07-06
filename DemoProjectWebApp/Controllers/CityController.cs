using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using DemoProjectWebApp.DAO;
using DemoProjectWebApp.Models;
using System.Net.Http.Headers;
using System.Text.Json;

namespace DemoProjectWebApp.Controllers
{
    public class CityController : Controller
    {
        public ClsAuthentication common = new ClsAuthentication();
        private readonly ApplicationDataService _appService;

        private readonly ILogger<CityController> _logger;
        public IConfiguration _configuration { get; }
        public CityController(ILogger<CityController> logger, IConfiguration configuration, ApplicationDataService appService)
        {
            _logger = logger;
            _configuration = configuration;
            _appService = appService;
        }
       public async Task<IActionResult> Index(int? id, string mode = "Create")
       {
            try
            {
                ViewBag.Mode = mode;
                Country country = new Country();
                if (HttpContext.Session.GetString("UserID") == null)
                {
                    return RedirectToAction("Login", "LoginModel");
                }
                if(mode == "Edit")
                {
                    return View(await GetCountryById(id));
                }
                if(mode == "View")
                {
                    return View(await GetCountryById(id));
                }

                return View();
            }
            catch (Exception ex)
            {
                return BadRequest();

            }
        }
        public IActionResult Main()
        {
            try
            {

                if (HttpContext.Session.GetString("UserID") == null)
                {
                    return RedirectToAction("Login", "LoginModel");
                }

                
                ViewBag.MainHeading = TempData["MainHeading"];
                ViewBag.DialogTitle = TempData["DialogTitle"];
                ViewBag.ShowPopup = TempData["ShowPopUp"];

                return View();
            }
            catch (Exception ex)
            {
                return BadRequest();

            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(City country, string mode)
        {
            try
            {
                if (HttpContext.Session.GetString("UserID") == null)
                {
                    return RedirectToAction("Login", "LoginModel");
                }

                if (mode == "Edit" && country.Id != null && country.Id != 0)
                {
                    if (ModelState.IsValid)
                    {
                        string endpoint = _configuration.GetSection("InsertCity").Value;
                        country.Mode = mode;
                        country.rec_modified_by = Convert.ToInt32(HttpContext.Session.GetString("UserID"));
                        string Result = await common.APIMethodPost(endpoint, "", country, "POST", common.GenerateToken());

                        if (Result == "Success")
                        {
                            
                                
                            TempData["ShowPopUp"] = true;
                            TempData["MainHeading"] = "Successfull !";
                            TempData["DialogTitle"] = "City Data has been Updated Succesfully";
                            return RedirectToAction("Main", "City");
                        }
                        else
                        {
                            ViewBag.ShowPopup = true;
                            ViewBag.MainHeading = "Failed !";
                            ViewBag.DialogTitle = "Data has No been Updated";
                            ModelState.AddModelError(string.Empty, "Update failed. " + Result);
                        }
                            
                    }
                    return View(country);
                }
                else if (mode == "Create")
                {
                    if (ModelState.IsValid)
                    {
                        string endpoint = _configuration.GetSection("InsertCity").Value;
                        country.Mode = mode;
                        country.rec_add_by = Convert.ToInt32(HttpContext.Session.GetString("UserID"));
                        string Result = await common.APIMethodPost(endpoint, "", country, "POST", common.GenerateToken());
                        
                        if (Result == "Success")
                        {
                            TempData["ShowPopUp"] = true;
                            TempData["MainHeading"] = "Successfull !";
                            TempData["DialogTitle"] = "City Data new request created Succesfully";
                            return RedirectToAction("Main", "City");
                        }
                        else
                        {
                            ViewBag.ShowPopup = true;
                            ViewBag.MainHeading = "Failed !";
                            ViewBag.DialogTitle = "Data has No been Updated";
                            ModelState.AddModelError(string.Empty, "Insertion failed. " + Result);
                        }
                            
                    }
                    return View(country);
                }
                else
                {
                    // Default return
                    return View(country);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error on method: Create controller: CountryController" + "|| User ID" + HttpContext.Session.GetString("UserID") + " Exception: " + ex.Message);
                return BadRequest();
            }
        }


        public async Task<ActionResult> DiscardRecord([FromBody]City city,string mode="Delete")
        {
            try
            {
                bool deleted = false;
                //city.Id = id;
                city.rec_modified_by = Convert.ToInt32(HttpContext.Session.GetString("UserID"));
                city.Mode = mode;
                string endpoint = _configuration.GetSection("InsertCity").Value;
                string result = await common.APIMethodPost(endpoint, "", city, "POST", common.GenerateToken());

                if (result == "Success")
                {
                    deleted = true;
                    return Json(new
                    {
                        success = true,
                        message = "Deleted Successfully"
                    });
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Discard failed. " + result);
                    return Json(new
                    {
                        success = false,
                        message = "Delete failed"
                    });
                    //return View(city);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error on GET DiscardRecord: " + ex.Message);
                return View();
            }
        }


        private async Task<City> GetCountryById(int? id)
        {
            if (id == null) return null;

            string endpoint = _configuration.GetSection("GetById").Value + "/" + id + "?spName=[SP_GetCityData]";
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", common.GenerateToken());
                using (HttpResponseMessage res = await client.GetAsync(endpoint))
                {
                    if (!res.IsSuccessStatusCode) return null;

                    var data = await res.Content.ReadAsStringAsync();
                    data = data.Replace("{}", "null");

                    var obj = JsonConvert.DeserializeObject<List<City>>(data);
                    return obj?.FirstOrDefault();
                }
            }
        }
        
        public async Task<ActionResult> View([DataSourceRequest] DataSourceRequest request)
        {
            try
            {
                IEnumerable<City> room = null;
                string endpoint = _configuration.GetSection("GetALLCity").Value;

                using (var client = new HttpClient())
                {
                    client.Timeout = TimeSpan.FromSeconds(30);
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", common.GenerateToken());
                    var response = await client.GetAsync(endpoint);

                    if (response.IsSuccessStatusCode)
                    {
                        var json = await response.Content.ReadAsStringAsync();
                        json = json.Replace("{}", "null");

                        room = System.Text.Json.JsonSerializer.Deserialize<IEnumerable<City>>(json,
                            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    }
                }

                if (room == null || !room.Any())
                {
                    ModelState.AddModelError("", "⚠ Data not found.");
                    
                    return Json(new
                    {
                        Errors = "Data Not Found."
                    });
                }

                return Json(room.ToDataSourceResult(request));
            }
            catch (Exception ex)
            {
               
                
                ModelState.AddModelError("", "❌ System Error: " + ex.Message);
                
                // Send JSON with error message
                return Json(new
                {
                    Errors = "Error in loading data ."
                });
            }

        }
    }
}
