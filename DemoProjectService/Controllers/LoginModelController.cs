using DemoProjectService.Classes;
using DemoProjectService.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DemoProjectService.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class LoginModelController : Controller
    {

        //private readonly IHttpContextAccessor _httpContextAccessor;
        //  private readonly ILogger<LoginModelController> _logger;
        private readonly ILogger<LoginModelController> _logger;
        private AuthenticationService authservice = new AuthenticationService();
        public LoginModelController(ILogger<LoginModelController> logger)// ILogger<LoginModelController> logger)//IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            // _httpContextAccessor = httpContextAccessor;
        }
        public delegate Task<string> LoginMethod(string loginid, string password);
        //public IActionResult Index()
        //{

        //    return View();
        //}
        [HttpGet("api/ping")]
        public IActionResult Ping()
        {
            return Ok("Pong");
        }

        [HttpPost("api/Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginObj, string loginType)
        {
            //  LoginRequest loginObj = new LoginRequest { LoginID = "admin", Password = "Reckon9" };
            try
            {
                //string token = Request.Headers["Authorization"];
                //if (string.IsNullOrEmpty(token) || !token.StartsWith("Bearer "))
                //{
                //    return Unauthorized("Missing Authorization header.");
                //}
                //if (authservice.ValidateJWTToken(token))
                //{
                AuthenticationService authService = new AuthenticationService();// _httpContextAccessor);
                var Result = "";
                if (loginType == "SQL")
                {
                    // Create a delegate and point it to the SQL Login method
                    LoginMethod loginMethod = new LoginMethod(authService.SqlLogin);

                    // Use the delegate to perform the login
                    //  jsonResult = "poong";
                    Result = await loginMethod(loginObj.LoginID, loginObj.Password);
                }
                else if (loginType == "LDAP")
                {
                    // Now point the delegate to the LDAP Login method
                    LoginMethod loginMethod = new LoginMethod(authService.LdapLogin);

                    // Use the delegate to perform the login
                    Result = await loginMethod(loginObj.LoginID, loginObj.Password);
                }


                if (string.IsNullOrEmpty(Result))
                {
                    return NotFound();
                }

                //  var result = JsonSerializer.Deserialize<IEnumerable<MyDataModel>>(jsonResult);

                return Ok(Result);
                //}
                //else
                //    return Unauthorized("Invalid Authorization header.");
            }
            catch (Exception ex)
            {
                _logger.LogError("Error on method: Login controller: LoginModelController" + loginType + " Exception is: ||" + ex.Message + " Trace: ||" + ex.StackTrace);
                return StatusCode(500, new { message = "An error occurred while Login, LoginModelController the resource.", details = ex.Message });
            }
        }




    }

    public class LoginRequest
    {
        public string LoginID { get; set; }
        public string Password { get; set; }
        
    }

}
