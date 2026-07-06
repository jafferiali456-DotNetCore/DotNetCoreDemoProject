using DemoProjectWebApp.Controllers;
using DemoProjectWebApp.Models;
using Newtonsoft.Json;

namespace DemoProjectWebApp.DAO
{
    public class ApplicationDataService
    {
        public string AppName { get; } = "My ASP.NET Core App";
        private readonly IConfiguration _configuration;
        private readonly ClsAuthentication _common = new ClsAuthentication();
        private readonly IHttpContextAccessor _contextAccessor;
        public ILogger<ApplicationDataService> _logger;

        // Global shared data
        public List<DownTimeclass> DownTimeList { get; private set; } = new List<DownTimeclass>();
        //public DateTime LastLoadedTime { get; private set; } = DateTime.MinValue;
        public DateTime LastLoadedTime { get; set; } = DateTime.MinValue;

        public ApplicationDataService(IConfiguration configuration,ILogger<ApplicationDataService> logger, IHttpContextAccessor contextAccessor)
        {
            _configuration = configuration;
            _logger = logger;
            _contextAccessor = contextAccessor;
        }

        // ✅ Only first time load from API
        public async Task LoadDownTimeDataOnceAsync()
        {
            if (DownTimeList.Any()) return; // already loaded, skip further calls

            try
            {
                DownTimeclass request = new DownTimeclass
                {
                    StartDate = DateTime.Now.AddDays(-7),
                    EndDate = DateTime.Now
                };

                string endpoint = _configuration.GetSection("GetCallDownTimeAlert").Value;
                var response = await _common.APIMethodPostReturnResponse(endpoint, "", request, "POST", _common.GenerateToken());
                var responseContent = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var list = JsonConvert.DeserializeObject<List<DownTimeclass>>(responseContent);
                    if (list != null && list.Any())
                    {
                        DownTimeList = list;
                        LastLoadedTime = DateTime.Now;
                    }
                }
            }
            catch (Exception ex)
            {
                // Log error if needed
                var userId = _contextAccessor.HttpContext?.Session?.GetString("UserID") ?? "Unknown";

                _logger.LogError("Error on method: LoadDownTimeDataOnceAsync controller: IctChangeReqController"
                                 + " || User ID: " + userId
                                 + " || Exception: " + ex.Message
                                 + " || Trace: " + ex.StackTrace);

                Console.WriteLine($"[LoadDownTimeDataOnceAsync] Error: {ex.Message}");
            }
        }
    }
}
