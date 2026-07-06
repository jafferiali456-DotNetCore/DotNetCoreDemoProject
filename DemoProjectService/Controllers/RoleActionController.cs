using DataAccess;
using DemoProjectService.Classes;
using Google.Protobuf.WellKnownTypes;
using MicroserviesWebApplication.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using MySqlX.XDevAPI.Common;
using System.Collections;
using System.Data;
using System.Text.Json;
using static DemoProjectService.Controllers.LoginModelController;
using DemoProjectService.Entity;
using Newtonsoft.Json;

namespace DemoProjectService.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class RoleActionController : ControllerBase
    {
        clsDataAccessLayer objAccess = new clsDataAccessLayer();
        private readonly ILogger<RoleActionController> _logger;
        private AuthenticationService authservice = new AuthenticationService();
        public RoleActionController(ILogger<RoleActionController> logger)
        {
            _logger = logger;
        }

        [HttpGet("api/Menu")]
        public string GetMenu(int Id)
        {
            string strRt = "";
            try
            {

                string strQry = "[dbo].[GetMenu]"; //DataSet ds = new DataSet();
                SqlParamData[] objParams = new SqlParamData[1];

                objParams[0] = new SqlParamData("@Id", Id, SqlDbType.Int, 100, ParameterDirection.Input);
                DataTable dt = objAccess.ExecuteStoredProcedureAndReturnDataTable(strQry, objParams);

                //SqlCommand cmd = new SqlCommand(strQry);
                //cmd.CommandType = CommandType.StoredProcedure;
                //cmd.Parameters.Add("@Id", SqlDbType.Int, 100).Value = Id;

                //DataSet dd = objAccess.ExecuteDataSet(cmd);

                //dd.Tables[0].TableName = "Menu";
                //DataRelation relation = new DataRelation("ParentChild", dd.Tables["Menu"].Columns["MenuID"], dd.Tables["Menu"].Columns["ParentID"], false);
                //relation.Nested = true;
                //dd.Relations.Add(relation);

                //DataSet cleanDs = new DataSet("Menus");
                // cleanDs.Tables.Add(dd.Tables["Menu"].Copy());

                var jsResult = new clsFunctions().ConvertDataTableToJson(dt);
                return jsResult;

                //				strRt = cleanDs.GetXml();
                //dd.Dispose();
                //cleanDs.Dispose();
                return jsResult;
                //xmlDataSource.Data = ds.GetXml();
            }
            catch (Exception ex)
            {
                _logger.LogError("Error on method: GetMenu controller: RoleActionController" + " Exception is: ||" + ex.Message + " Trace: ||" + ex.StackTrace);
                //return StatusCode(500, new { message = "An error occurred while GetMenu, GetMenuController the resource.", details = ex.Message });
            }
            return strRt;
        }

        [HttpPost("api/CallDownTimeAlert")]
        public async Task<ActionResult<IEnumerable<DownTimeclass>>> CallDownTimeAlert([FromBody] DownTimeclass downTimeclass)  //,string? statusMode, int? EmpCode, string? AccountNumber, string? NatureOfAccount, string? SearchType)
        {

            clsDataAccessLayer cls = new clsDataAccessLayer();
            try
            {
                string strQry = "[dbo].[SP_Get_DownTimeAlert]";
                DateTime StartDate = downTimeclass.StartDate;
                DateTime EndDate = downTimeclass.EndDate;
                SqlParamData[] objParams = new SqlParamData[2];
                objParams[0] = new SqlParamData("@StartDate", StartDate == null ? DBNull.Value : StartDate, SqlDbType.DateTime, 255, ParameterDirection.Input);
                objParams[1] = new SqlParamData("@EndDate", EndDate == null ? DBNull.Value : EndDate, SqlDbType.DateTime, 255, ParameterDirection.Input);
                DataTable dt = cls.ExecuteStoredProcedureAndReturnDataTable(strQry, objParams);

                if (dt == null || dt.Rows.Count == 0)
                {
                    return NotFound();  // Return a 404 if no data is found
                }

                return Ok(JsonConvert.SerializeObject(dt));
            }
            catch (Exception ex)
            {
                // Handle any exceptions and log them (optional)
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
            finally
            { cls.CloseDataBase(); }
            //return "";
        }


        [HttpPost("api/InsertEmployee")]
        public async Task<ActionResult<IEnumerable<Employee>>> InsertEmployee([FromBody] EmployeeMaster entities)  //,string? statusMode, int? EmpCode, string? AccountNumber, string? NatureOfAccount, string? SearchType)
        {
            int employeeId = 0;
            clsDataAccessLayer cls = new clsDataAccessLayer();
            try
            {
                string strQry = "[dbo].[SP_InsertEmployee]";
                List<EmployeeMaster> employeeMasters = new List<EmployeeMaster>();
                employeeMasters.Add(entities);
                var jsonData = System.Text.Json.JsonSerializer.Serialize(employeeMasters);
                clsDataAccessLayer objDataAccess2 = new clsDataAccessLayer();
                SqlParamData[] objParams = new SqlParamData[1];
                objParams[0] = new SqlParamData("@json", jsonData, SqlDbType.NVarChar, 1000, ParameterDirection.Input);
                DataTable dt = cls.ExecuteStoredProcedureAndReturnDataTable(strQry, objParams);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataColumn col in dt.Columns)
                    {
                        // Yahan check karein ke 'Id' naam ka column list mein hai ya nahi
                        Console.WriteLine("Column Name: " + col.ColumnName);
                    }
                    var id = dt.Rows[0]["@NextID"].ToString();

                    employeeId = Convert.ToInt32(id);
                    if (dt.Columns.Contains("@NextID"))
                    {
                        employeeId = Convert.ToInt32(dt.Rows[0]["@NextID"]);
                    }
                }
                if (dt == null || dt.Rows.Count == 0)
                {
                    return NotFound();  // Return a 404 if no data is found
                }

                return Ok(new { id = employeeId });
            }
            catch (Exception ex)
            {
                // Handle any exceptions and log them (optional)
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
            finally
            { cls.CloseDataBase(); }
            //return "";
        }


        [HttpPost("api/GetGridData")]
        public async Task<ActionResult<IEnumerable<DynamicField>>> GetGridData([FromBody] DynamicField dynamicField)  //,string? statusMode, int? EmpCode, string? AccountNumber, string? NatureOfAccount, string? SearchType)
        {

            clsDataAccessLayer cls = new clsDataAccessLayer();
            try
            {
                string strQry = "[dbo].[SP_GetDynamicGridData]";
                string FormCode = dynamicField.FormCode;
                SqlParamData[] objParams = new SqlParamData[1];
                objParams[0] = new SqlParamData("@FormCode", FormCode != null ? FormCode : DBNull.Value, SqlDbType.VarChar, 255, ParameterDirection.Input);
                DataTable dt = cls.ExecuteStoredProcedureAndReturnDataTable(strQry, objParams);

                if (dt == null || dt.Rows.Count == 0)
                {
                    return NotFound();  // Return a 404 if no data is found
                }

                return Ok(JsonConvert.SerializeObject(dt));
            }
            catch (Exception ex)
            {
                // Handle any exceptions and log them (optional)
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
            finally
            { cls.CloseDataBase(); }
            //return "";
        }

        //[HttpPost("api/FormAction")]
        //public async Task<IActionResult> FormAction([FromBody] ActionRequest loginObj)
        //{
        //    try
        //    {
        //        AuthenticationService authService = new AuthenticationService();
        //        if (loginObj == null || loginObj.Id <= 0)
        //        {
        //            return BadRequest("Invalid input.");
        //        }

        //        // ✅ Call static method from another class
        //        var result = "";// await Task.Run(() => authService.FormFuncion(loginObj.Id));

        //        if (string.IsNullOrEmpty(result) || result == "exception")
        //        {
        //            return StatusCode(500, new { message = "Failed to retrieve form data." });
        //        }

        //        return Content(result, "application/json");
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError("Error in FormAction: " + ex.Message + " || Trace: " + ex.StackTrace);
        //        return StatusCode(500, new { message = "An error occurred while processing the request.", details = ex.Message });
        //    }
        //}




        [HttpGet("api/Action")]
        public string Actions(int Id)
        {
            //  ArrayList arr = new ArrayList();
            try
            {
                DataTable dt = null;
                string strQry = "[dbo].[FormsAction]";

                SqlParamData[] objParams = new SqlParamData[1];

                objParams[0] = new SqlParamData("@Id", Id, SqlDbType.BigInt, 100, ParameterDirection.Input);
                dt = objAccess.ExecuteStoredProcedureAndReturnDataTable(strQry, objParams);

                var jsResult = new clsFunctions().ConvertDataTableToJson(dt);
                return jsResult;

            }
            catch (Exception ex)
            {
                // Always log exceptions for debugging
                // arr.Add("ERROR: " + ex.Message);
            }
            return "";
        }

    }


    public class ActionRequest
    {
        public int Id { get; set; }
    }

    public class MenuRequest
    {
        public int Id { get; set; }
        public string? Description { get; set; }
        public string? Navigate { get; set; }

    }


}