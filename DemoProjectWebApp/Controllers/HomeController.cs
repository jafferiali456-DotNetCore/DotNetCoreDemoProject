using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using DemoProjectWebApp.DAO;
using DemoProjectWebApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Text.Json;
using System.Xml;
using System.Xml.Linq;


namespace DemoProjectWebApp.Controllers
{
    public class HomeController : Controller
    {
        private static string reqType = "";
        public ClsAuthentication common = new ClsAuthentication();
        private readonly ApplicationDataService _appService;
        private readonly ILogger<HomeController> _logger;
        public IConfiguration _configuration { get; }
        public HomeController(ILogger<HomeController> logger, IConfiguration configuration, ApplicationDataService appService)
        {
            _logger = logger;
            _configuration = configuration;
            _appService = appService;
        }



        public async Task<ActionResult> Index(string rType)
        {
            // Default model in case something goes wrong
            var menuJson = HttpContext.Session.GetString("MenuJson");
            var menuList = JsonConvert.DeserializeObject<List<MenuItem>>(menuJson);
            var model = new SetupTileModel
            {
                  Operation = menuList
                 .Where(x => x.ParentID?.ToString() == "3")
                     .Select(x => new SetupItem
                     {
                         Name = x.Text,
                         Url = x.Navigate
                     }).ToList(),

                  SetUpForms = menuList
                 .Where(x => x.ParentID?.ToString() == "11")
                     .Select(x => new SetupItem
                     {
                         Name = x.Text,
                         Url = x.Navigate
                     }).ToList()
            };
         
            //var model = new SetupTileModel
            //{
            //    SetupItems = new List<SetupItem>
            //    {
            //        new SetupItem { Name = "Pay Statement", Url = "/pay-statement" },
            //        new SetupItem { Name = "Salary Taxation", Url = "/salary-taxation" },
            //        new SetupItem { Name = "Salary Taxation", Url = "/salary-taxation" },
            //        new SetupItem { Name = "Salary Taxation", Url = "/salary-taxation" },
            //        new SetupItem { Name = "Salary Taxation", Url = "/salary-taxation" },
            //        new SetupItem { Name = "Salary Taxation", Url = "/salary-taxation" },
            //        new SetupItem { Name = "Salary Taxation", Url = "/salary-taxation" },
            //        new SetupItem { Name = "Salary Taxation", Url = "/salary-taxation" },
            //        new SetupItem { Name = "Comp Statement", Url = "/comp-statement" }
            //    }
            //};

            try
            {
                reqType = string.IsNullOrEmpty(rType) ? reqType : rType;

                await _appService.LoadDownTimeDataOnceAsync();

                ViewBag.AppName = _appService.AppName;
                ViewBag.DownTimeList = _appService.DownTimeList;
                ViewBag.LastLoaded = _appService.LastLoadedTime;

                string endpoint = _configuration.GetSection("GatewayWFGetTotalRequestsURL").Value;
                using (var client = new HttpClient())
                {
                    client.Timeout = TimeSpan.FromSeconds(30);
                    var responseTask = client.GetAsync(endpoint + "?EmpCode=" + HttpContext?.Session?.GetString("EmployeeCode") ?? "");
                    responseTask.Wait();

                    ViewBag.RoleCode = (HttpContext.Session.GetString("RoleCode") != null &&
                                        HttpContext.Session.GetString("RoleCode").ToString() == "ADM001");

                    var result = responseTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        var content = await result.Content.ReadAsStringAsync();
                        var keyValueCollection = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, string>>(content);

                        foreach (var kvp in keyValueCollection)
                        {
                            if (kvp.Key.Equals("EmployeesRequestsApproved"))
                                ViewBag.TotApprovedReqs = kvp.Value;
                            else if (kvp.Key.Equals("EmployeesRequestsNotApproved"))
                                ViewBag.TotPendingReqs = kvp.Value;
                            else if (kvp.Key.Equals("EmployeesPendingRequestsAssigend"))
                                ViewBag.TotPendingApprovals = kvp.Value;
                        }
                    }
                }

                return View(model); // Model hamesha pass kar rahe hain
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(model); // <-- Yahan bhi model pass karna zaruri
            }
        }

        public class SetupItem
        {
            public string Name { get; set; }
            public string Url { get; set; }
        }
        public class SetupTileModel
        {
            public List<SetupItem> Operation { get; set; }
            public List<SetupItem> SetUpForms { get; set; }
        }

        [HttpGet]
		public IActionResult View(string requestType)
		{
			reqType = requestType;
			return View();
		}

        public IActionResult ViewProfile()
        {
            return View();  
        }
        public IActionResult DemoForm()
        {
            return View();  
        }

        [HttpGet]
        public IActionResult DashBoard(string requestType)
        {
            reqType = requestType;
            return View();
        }

        [HttpGet]
		public IActionResult GetMenu()
		{
			//var xmlData = HttpContext.Session.GetString("MenuXml");
			var MenuJson = HttpContext.Session.GetString("MenuJson");
			////XmlDocument xmlDoc = new XmlDocument();
			////xmlDoc.LoadXml(xmlData);
			var menus = JsonConvert.DeserializeObject<List<MenuItem>>(MenuJson);
			//var jsonData = JsonConvert.SerializeXmlNode(xmlDoc, Newtonsoft.Json.Formatting.None, true);
			//var allItems = JsonConvert.DeserializeObject<List<MenuItem>>(serializedData);
			var items = new List<MenuItem>
			{
				new MenuItem() { MenuID = 1, ParentID = null, Text = "Dashboard", Navigate = "/Home/Index" },
				new MenuItem() { MenuID = 2, ParentID = null, Text = "Requests", Navigate = "#" },
				new MenuItem() { MenuID = 3, ParentID = 2, Text = "DemoProjectWebApp Request", Navigate = "/DemoProjectWebAppRequest/Index" },
				new MenuItem() { MenuID = 4, ParentID = 2, Text = "Transport Request", Navigate = "/TransportRequest/Index" }
				// Add more as needed
			};

			//return Json(items);
			return Json(menus);// Content(MenuJson, "application/json"); //Json( MenuJson); // Content(jsonData, "application/json");
		}


	}


	public class MenuItem
	{
		public int MenuID { get; set; }

		public int? ParentID { get; set; }

		public string? Text { get; set; }
		public string? Navigate { get; set; }
		public string? Description { get; set; }
	}
	public class GridColumns
	{
		public string ViewURL { get; set; }
		public string Token { get; set; }
		public long WF_RequestKeyID { get; set; }
		public long WF_WorkFlowID { get; set; }
		public long WF_WorkFlowDetailID { get; set; }

		public long WF_WorkFlowStateID { get; set; }
		public string StatusText { get; set; }
		public string Status { get; set; }
		public string WF_TransactionCode { get; set; }
		public string RequestType { get; set; }
		public string WF_WorkflowCode { get; set; }
		public string AssignTo { get; set; }
		public string Title { get; set; }
		public string ModuleType { get; set; }
		public string RequestedBy { get; set; }

		public string RequestFor { get; set; }

		public string Department { get; set; }
	}
	public class ChangeAssignTo
	{

		public long WorkflowStateID { get; set; }

		public string AssignTo { get; set; }
		public long RecModifiedBy { get; set; }
	}
}
