using DemoProjectWebApp.DAO;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using DemoProjectWebApp.DAO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Net.Http.Headers;

namespace DemoProjectWebApp.Controllers

{

    public class LoginModelController : Controller
    {
        public IConfiguration _configuration { get; }
        public ClsAuthentication common = new ClsAuthentication();
        public ILogger<LoginModelController> _logger;
        //  private readonly IDataAccessService _dataAccessService;

        public LoginModelController(IConfiguration configuration, ILogger<LoginModelController> logger)//, IDataAccessService dataAccessService)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _logger = logger;
            //  _dataAccessService = dataAccessService;

        }

        public IActionResult Image()
        {
            var random = new Random();
            var captchaCode = random.Next(1000, 9999).ToString();

            HttpContext.Session.SetString("CaptchaCode", captchaCode);

            using (var bitmap = new Bitmap(150, 50))
            using (var graphics = Graphics.FromImage(bitmap))
            using (var ms = new MemoryStream())
            {
                // background gradient
                var rect = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
                using (var brush = new System.Drawing.Drawing2D.LinearGradientBrush(
                           rect,
                           Color.LightGray,
                           Color.White,
                           45f))
                {
                    graphics.FillRectangle(brush, rect);
                }

                // random font
                using (var font = new Font("Arial", 24, FontStyle.Bold))
                {
                    // draw some noise lines
                    for (int i = 0; i < 5; i++)
                    {
                        var pen = new Pen(Color.FromArgb(random.Next(50, 200),
                                                         random.Next(50, 200),
                                                         random.Next(50, 200)), 1);
                        graphics.DrawLine(pen,
                            random.Next(0, bitmap.Width), random.Next(0, bitmap.Height),
                            random.Next(0, bitmap.Width), random.Next(0, bitmap.Height));
                    }

                    // draw captcha text with slight rotation
                    var matrix = new System.Drawing.Drawing2D.Matrix();
                    matrix.RotateAt(random.Next(-10, 10), new PointF(bitmap.Width / 2, bitmap.Height / 2));
                    graphics.Transform = matrix;

                    var textBrush = new SolidBrush(Color.FromArgb(
                        random.Next(0, 150),
                        random.Next(0, 150),
                        random.Next(0, 150)));

                    var textSize = graphics.MeasureString(captchaCode, font);
                    graphics.DrawString(
                        captchaCode,
                        font,
                        textBrush,
                        (bitmap.Width - textSize.Width) / 2,
                        (bitmap.Height - textSize.Height) / 2
                    );

                    graphics.ResetTransform();
                }

                // save as PNG
                bitmap.Save(ms, ImageFormat.Png);
                ms.Seek(0, SeekOrigin.Begin);
                return File(ms.ToArray(), "image/png");
            }
        }

        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            if (TempData["ErrorMessage"] != null)
            {
                TempData["ErrorMessage"] = "Your session has expired. Please log in again.";
                ModelState.AddModelError(string.Empty, TempData["ErrorMessage"].ToString());
                if (returnUrl != null)
                    TempData["ReturnUrl"] = returnUrl;
                return View();
            }
            //    TempData["CaptchaError"] = null; // force clear
            TempData["ReturnUrl"] = returnUrl;
            return View();
        }
        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "LoginModel");

        }

        public async Task<List<PageAction>> GetActionValue(int id)
        {
            ActionRequest wf = new ActionRequest();
            try
            {
                string endpoint = _configuration.GetSection("GetActionId").Value + "/" + "Action?Id=" + 10;
                using (var client = new HttpClient())
                {
                    client.Timeout = TimeSpan.FromSeconds(30);
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", common.GenerateToken());
                    using (HttpResponseMessage res = await client.GetAsync(endpoint))
                    {
                        if (res.IsSuccessStatusCode)
                        {
                            var data = await res.Content.ReadAsStringAsync();

                            var obj = JsonConvert.DeserializeObject<List<PageAction>>(data);
                            //string jsonString = JsonConvert.SerializeObject(obj);
                            HttpContext.Session.SetString("ActionValues", data);
                            //var newobjetc = HttpContext.Session.GetString("ActionValues");
                            //Console.Write(newobjetc);
                            return obj;
                        }
                        else
                        {
                            ViewBag.ErrorMessage = "Error fetching data from the server";
                            return null;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error on method: Edit(id) controller: IctChangeReqController" + "|| User ID" + HttpContext.Session.GetString("UserID") + " for requestID: " + " Exception is: ||" + ex.Message + " Trace: ||" + ex.StackTrace);
            }
            return null;
        }

        public async Task<List<PageAction>> GetActionsFromApi(int id)
        {
            List<PageAction> actions = new List<PageAction>();

            try
            {
                // Construct the API endpoint
                string endpoint = _configuration.GetSection("GetActionId").Value + "/Action?Id=" + id;

                using (var client = new HttpClient())
                {
                    client.Timeout = TimeSpan.FromSeconds(30);
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", common.GenerateToken());
                    using (HttpResponseMessage res = await client.GetAsync(endpoint))
                    {
                        if (res.IsSuccessStatusCode)
                        {
                            var data = await res.Content.ReadAsStringAsync();
                            //data.Replace("[", "}");
                            //data.Replace("[", "}");
                            // Deserialize the data
                            actions = JsonConvert.DeserializeObject<List<PageAction>>(data);

                            // Store in session
                            HttpContext.Session.SetString("ActionValues", data); // Storing raw JSON in session
                        }
                        else
                        {
                            throw new Exception("Error fetching data from the server.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error fetching actions for ID {id}: {ex.Message}");
                throw; // Rethrow the exception to be handled by the calling function
            }

            return actions; // Return the actions
        }




        [HttpPost]
        // [Authorize("Authorization")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginRequest model)//, string returnUrl )
        {

            //if (HttpContext.Session.GetString("UserID") == null)
            //{
            //    TempData["ErrorMessage"] = "Session Ended, Login again.";
            //    return RedirectToAction("Login", "LoginModel");
            //}
            //var sessionCaptcha = HttpContext.Session.GetString("CaptchaCode");
            ////string storedCode = HttpContext.Session.GetString("CaptchaCode");
            //var captchaInput = Request.Form["CaptchaInput"];

            //if (model.CaptchaInput != sessionCaptcha)
            //{
            //    ModelState.AddModelError("CaptchaInput", "Invalid captcha code!");
            //    TempData["CaptchaError"] = "Invalid Captcha";
            //    return RedirectToAction("Login", "LoginModel");
            //}
            //else
            //{
            //    TempData["CaptchaError"] = null; // force clear
            //}

            string returnUrl = TempData["ReturnUrl"] == null ? "" : TempData["ReturnUrl"].ToString();
            try
            {
                if (ModelState.IsValid)
                {

                    #region Login

                    
                    //if (!string.IsNullOrEmpty(token))
                    //{
                    string endpoint = _configuration.GetSection("GatewayLoginUrl").Value;
                    //   endpoint += "?spName=WyF_SP_Create_2_Repository";

                    // var EmployeeCode = "PN000125";
                    var response = await common.APIMethodPostReturnResponse(endpoint, "", model, "POST", common.GenerateToken());
                    var res = response.Content.ReadAsStringAsync();
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {

                        string resStr = res.Result.Replace("{}", "null");
                        resStr = resStr.Replace("[", "");
                        resStr = resStr.Replace("]", "");
                        var obj = JsonConvert.DeserializeObject<LoginResponse>(resStr);

                        if (res.Result.Equals("[]"))
                        { //return Unauthorized();
                            ModelState.AddModelError(string.Empty, "Login failed. Incorrect User Name or Password ");
                            return View();
                        }
                        //else if (!string.IsNullOrEmpty(obj.Status) && obj.Status.Equals("Blocked", StringComparison.OrdinalIgnoreCase))
                        //{
                        //    ModelState.AddModelError(string.Empty, "Your account is temporarily blocked. Please try again later.");
                        //    return View();
                        //}
                        else if (!string.IsNullOrEmpty(obj.Status))
                        {

                            // Agar message me 'Invalid' ya 'Blocked' ya 'Attempt' jaisa word hai to show karo
                            if (obj.Status.Contains("Invalid", StringComparison.OrdinalIgnoreCase))
                            {

                                ModelState.AddModelError(string.Empty, "Invalid Attempt. Please try again later.");
                                return View();
                            }
                            else if (obj.Status.Contains("Blocked", StringComparison.OrdinalIgnoreCase))
                            {
                                ModelState.AddModelError(string.Empty, "Your account is temporarily blocked. Please try again later.");
                                return View();
                            }
                            else if (obj.Status.Contains("attempt", StringComparison.OrdinalIgnoreCase))
                            {
                                ModelState.AddModelError(string.Empty, obj.Status);
                                return View();
                            }

                        }
                        else
                        {
                            //string resStr = res.Result.Replace("{}", "null");
                            //resStr = resStr.Replace("[", "");
                            //resStr = resStr.Replace("]", "");
                            //var obj = JsonConvert.DeserializeObject<LoginResponse>(resStr);
                            // var singleItem = obj.FirstOrDefault();
                            // var session = _httpContextAccessor.HttpContext.Session;
                            int userid = Convert.ToInt32(obj.UserID);

                            var actions = await GetActionsFromApi(userid);
                            // var actions = await GetActionValue(Convert.ToInt32(obj.UserID));
                            Console.Write(actions);
                            //HttpContext.Session.SetString("ActionValues", actions);
                            //var newobjetc = HttpContext.Session.GetString("ActionValues");
                            //Console.Write(newobjetc);

                            HttpContext.Session.SetString("Name", obj.UserName);
                            HttpContext.Session.SetString("UserID", obj.UserID.ToString().Trim().ToLower());
                            HttpContext.Session.SetString("AD_UserID", obj.UserID.ToString().Trim().ToLower());
                            HttpContext.Session.SetString("RoleID", obj.RoleID.ToString().Trim());
                            HttpContext.Session.SetString("LoginID", model.LoginID);
                            HttpContext.Session.SetString("EmployeeCode", obj.EmployeeCode.ToString().Trim());
                            HttpContext.Session.SetString("RoleCode", obj.RoleCode.ToString().Trim());
                            HttpContext.Session.SetString("UserRoleCode", obj.RoleCode.ToString());
                            var menuJson = await GetMenu(userid);

                            //	HttpContext.Session.SetString("MenuJson",menuJson.ToString().Trim());//GetXMLMenu();
                            //to add menu
                            ///System.Web.HttpContext.Current.Session["Menu"] = (new BusinessLogic.LogicClasses.clsUser()).getMenu(lDBID);
                            ///
                            if (!string.IsNullOrEmpty(returnUrl))
                            {
                                return LocalRedirect(returnUrl);
                            }
                            else
                            {


                                if (obj.RoleCode == "ADM001")
                                    return RedirectToAction("Index", "Home");
                                if (obj.RoleID == 2)
                                    return RedirectToAction("Index", "Home");
                                if (obj.RoleID == 4)
                                    return RedirectToAction("Index5", "Home");
                                

                            }
                            // return RedirectToAction("Index", "Home");
                        }
                    }
                    else //return response.StatusCode + "|" + response.ReasonPhrase;
                    {
                        ModelState.AddModelError(string.Empty, "Login failed. " + response.StatusCode);
                        return Unauthorized();
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        private string GetXMLMenu()
        {
            return "<Menus>  <Menu>    <MenuID>140</MenuID>    <Text>AA Bank Payment Voucher</Text>    <Description>AA Bank Payment Voucher</Description>    <Navigate>GL/BankPaymentVoucherList.aspx?CompID=04 </Navigate>    <ParentID>140</ParentID>  </Menu>  <Menu>    <MenuID>141</MenuID>    <Text>AA Bank Payment Voucher Post</Text>    <Description>AA Bank Payment Voucher Post</Description>    <Navigate>GL/BankPaymentVoucherPostList.aspx?CompID=04 </Navigate>    <ParentID>140</ParentID>  </Menu>  <Menu>    <MenuID>145</MenuID>    <Text>AA Bank Receipt Voucher Posted</Text>    <Description>AA Bank Receipt Voucher Posted</Description>    <Navigate>GL/BankReceiptVoucherPostedList.aspx?CompID=04 </Navigate>    <ParentID>141</ParentID>  </Menu>  <Menu>    <MenuID>162</MenuID>    <Text>AA Advance Posted List</Text>    <Description>AA Advance Posted List</Description>    <Navigate>AR/AdvancePostedList.aspx?CompID=01 </Navigate>    <ParentID>145</ParentID>  </Menu>  <Menu>    <MenuID>259</MenuID>    <Text>AP IPS Bill</Text>    <Description>AP IPS Bill</Description>    <Navigate>#</Navigate>    <ParentID>162</ParentID>  </Menu>  <Menu>    <MenuID>260</MenuID>    <Text>AP IPS Settlement </Text>    <Description>AP IPS Settlement </Description>    <Navigate>#</Navigate>    <ParentID>162</ParentID>  </Menu>  <Menu>    <MenuID>247</MenuID>    <Text>AP AAG  Bill Posted List</Text>    <Description>AP AAG  Bill Posted List</Description>    <Navigate>AP/InvoicePostedList.aspx?CompID=05 </Navigate>    <ParentID>245</ParentID>  </Menu>  <Menu>    <MenuID>245</MenuID>    <Text>AP AAG Bill</Text>    <Description>AP AAG Bill</Description>    <Navigate>AP/InvoiceList.aspx?CompID=05 </Navigate>    <ParentID>245</ParentID>  </Menu>  <Menu>    <MenuID>251</MenuID>    <Text>AP IPS  Bill</Text>    <Description>AP IPS  Bill</Description>    <Navigate>AP/InvoiceList.aspx?CompID=06 </Navigate>    <ParentID>247</ParentID>  </Menu>  <Menu>    <MenuID>323</MenuID>    <Text>Collection Analysis</Text>    <Navigate>CollectionAnalysisPivot.aspx</Navigate>    <ParentID>251</ParentID>  </Menu>  <Menu>    <MenuID>277</MenuID>    <Text>SHA Invoice Posted List</Text>    <Description>SHA Invoice Posted List</Description>    <Navigate>AR/InvoicePostedList.aspx?CompID=07 </Navigate>    <ParentID>259</ParentID>  </Menu>  <Menu>    <MenuID>280</MenuID>    <Text>SHA Invoice Settlement Posted List</Text>    <Description>SHA Invoice Settlement Posted List</Description>    <Navigate>AR/InvoiceSettlementPostedList.aspx?CompID=07 </Navigate>    <ParentID>260</ParentID>  </Menu>  <Menu>    <MenuID>296</MenuID>    <Text>SHA Bank Payment Voucher</Text>    <Description>SHA Bank Payment Voucher</Description>    <Navigate>GL/BankPaymentVoucherList.aspx?CompID=08</Navigate>    <ParentID>277</ParentID>  </Menu>  <Menu>    <MenuID>306</MenuID>    <Text>SHA Cash Receipt Voucher Post</Text>    <Description>SHA Cash Receipt Voucher Post</Description>    <Navigate>GL/CashReceiptVoucherPostList.aspx?CompID=08</Navigate>    <ParentID>280</ParentID>  </Menu>  <Menu>    <MenuID>309</MenuID>    <Text>AP SHA  Bill</Text>    <Description>AP SHA  Bill</Description>    <Navigate>#</Navigate>    <ParentID>296</ParentID>  </Menu>  <Menu>    <MenuID>347</MenuID>    <Text>AAD Advance Posted List</Text>    <Description>AAD Advance Posted List</Description>    <Navigate>AR/AdvancePostedList.aspx?CompID=09 </Navigate>    <ParentID>323</ParentID>  </Menu>  <Menu>    <MenuID>367</MenuID>    <Text>AAD Bank Payment Voucher Post</Text>    <Description>AAD Bank Payment Voucher Post</Description>    <Navigate>GL/BankPaymentVoucherPostList.aspx?CompID=10</Navigate>    <ParentID>347</ParentID>  </Menu></Menus>";
        }
        public async Task<List<MenuItem>> GetMenu(int id)
        {
            List<MenuItem> actions = new List<MenuItem>();

            try
            {
                // Construct the API endpoint
                string endpoint = _configuration.GetSection("GetActionId").Value + "/Menu?Id=" + id;

                using (var client = new HttpClient())
                {
                    client.Timeout = TimeSpan.FromSeconds(30);
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", common.GenerateToken());
                    using (HttpResponseMessage res = await client.GetAsync(endpoint))
                    {
                        if (res.IsSuccessStatusCode)
                        {
                            var data = await res.Content.ReadAsStringAsync();

                            //actions = JsonConvert.DeserializeObject<List<MenuItem>>(data,);
                            //var settings = new JsonSerializerSettings
                            //{
                            //	NullValueHandling = NullValueHandling.Ignore,
                            //	Formatting = Formatting.None
                            //};

                            // Store in session
                            string fixedJson = data.Replace("\"ParentID\":{}", "\"ParentID\":null");
                            actions = JsonConvert.DeserializeObject<List<MenuItem>>(fixedJson);
                            //actions = JsonConvert.DeserializeObject<List<MenuItem>>(data);
                            //var menuJson = Json(data, settings);
                            HttpContext.Session.SetString("MenuJson", fixedJson);

                            //HttpContext.Session.SetString("ActionValues", menuJson); // Storing raw JSON in session
                        }
                        else
                        {
                            throw new Exception("Error fetching data from the server.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error fetching actions for ID {id}: {ex.Message}");
                throw; // Rethrow the exception to be handled by the calling function
            }

            return actions; // Return the actions
        }






    }
    public class PageAction
    {
        public string Text { get; set; }
        public string Navigate { get; set; }
    }

    public class ActionRequest
    {
        public int Id { get; set; }
        public string? Descritpion { get; set; }
    }
    public class LoginRequest
    {
        public string LoginID { get; set; }
        public string Password { get; set; }
        public string? CaptchaInput { get; set; }

        
    }
    public class LoginResponse
    {
        public string UserName { get; set; }
        public string? Status { get; set; }
        public string LoginID { get; set; }
        public long RoleID { get; set; }
        public long UserID { get; set; }
        public string RoleCode { get; set; }
        public string EmployeeCode { get; set; }
    }

}
