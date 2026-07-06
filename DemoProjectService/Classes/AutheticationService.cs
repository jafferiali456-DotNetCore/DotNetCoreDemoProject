using DemoProjectService.Classes;
using DemoProjectService.Models;
using DataAccess;
using MicroserviesWebApplication.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using DemoProjectService.Classes;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Sockets;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Xml.Linq;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace DemoProjectService.Classes
{
    public class AuthenticationService
    {
        //  private readonly IHttpContextAccessor _httpContextAccessor;
        clsDataAccessLayer objDataAccess = new clsDataAccessLayer();
        public AuthenticationService()//IHttpContextAccessor httpContextAccessor)
        {
            // _httpContextAccessor = httpContextAccessor;
        }

        // SQL Server Authentication (example)


        public async Task<string> FormFuncion(int Id)
        {
            //  bool bRt = false;
            var jsResult = "";
            try
            {

                DataTable dt = null;
                try
                {
                    string strQry = "[dbo].[FormsAction]";
                    SqlParamData[] objParams = new SqlParamData[2];

                    objParams[0] = new SqlParamData("@Id", Id, SqlDbType.BigInt, 100, ParameterDirection.Input);
                    dt = objDataAccess.ExecuteStoredProcedureAndReturnDataTable(strQry, objParams);
                    jsResult = new clsFunctions().ConvertDataTableToJson(dt);
                    return jsResult;

                }
                catch (Exception exp)
                {
                    //clsLogicErrorHandler objErr = new clsLogicErrorHandler();
                    //objErr.LogErrors(exp);
                    jsResult = "exception";
                }
                finally
                {
                    objDataAccess.CloseDataBase();
                    dt.Dispose();
                }
            }
            catch
            { }
            return "";

            //  return loginid == "sqlUser" && password == "sqlPassword"; // Simulated check
        }
        public async Task<string> SqlLogin(string loginid, string password)
        {
            //  bool bRt = false;
            var jsResult = "";
            try
            {
                bool invalidCount = CountInvalidAttempts();
                if (invalidCount)
                {
                    return Newtonsoft.Json.JsonConvert.SerializeObject(new
                    {
                        Status = "Blocked",
                        Message = "Your IP has been temporarily blocked due to multiple invalid login attempts."
                    });
                }

                DataTable dt = null;
                try
                {
                    string strQry = "[dbo].[SP_VerifyLoginCredentials]";
                    SqlParamData[] objParams = new SqlParamData[2];

                    objParams[0] = new SqlParamData("@LoginID", loginid, SqlDbType.VarChar, 100, ParameterDirection.Input);
                    objParams[1] = new SqlParamData("@Password", password, SqlDbType.VarChar, 100, ParameterDirection.Input);
                    dt = objDataAccess.ExecuteStoredProcedureAndReturnDataTable(strQry, objParams);
                    if (dt.Rows.Count > 0)
                    {
                        //       deleteinvalidattempts();

                        InsertIP(loginid);

                        jsResult = new clsFunctions().ConvertDataTableToJson(dt);
                        return jsResult;
                    }
                    else
                    {
                        InsertInvalidIP(loginid);
                        //int leftAttempts = 3 - (invalidCount + 1);
                        return Newtonsoft.Json.JsonConvert.SerializeObject(new
                        {
                            Status = "Invalid",
                            //Message = leftAttempts > 0
                            //? $"Invalid credentials. You have {leftAttempts} attempt(s) left."
                            //: "You have reached the maximum number of invalid attempts. Please try again later."
                        });
                    }


                }
                catch (Exception exp)
                {
                    //clsLogicErrorHandler objErr = new clsLogicErrorHandler();
                    //objErr.LogErrors(exp);
                    jsResult = JsonConvert.SerializeObject(new { Status = "Error", Message = exp.Message });
                    jsResult = "exception";
                }
                finally
                {
                    objDataAccess.CloseDataBase();
                    dt.Dispose();
                }
            }
            catch
            { }
            return "";

            //  return loginid == "sqlUser" && password == "sqlPassword"; // Simulated check
        }

        // LDAP Authentication (example)
        public async Task<string> LdapLogin(string loginid, string password)
        {
            ADServices cls_AD = new ADServices();// _httpContextAccessor);
            if (!cls_AD.IsUserADorBasic(loginid))//Is Ad User
            {

                if (cls_AD.ADUserVerify(loginid.Trim(), password.Trim()))
                {
                    var jsonResult = await cls_AD.VerifyUserAD(loginid);
                    return jsonResult;

                }
                // loginid == "ldapUser" && password == "ldapPassword"; // Simulated check
            }
            return "";
        }
        public string LocalIPAddress()
        {
            IPHostEntry host;
            string localIP = "";
            host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    localIP = ip.ToString();
                    break;
                }
            }
            return localIP;
        }
        public void InsertIP(string loginid)
        {
            clsDataAccessLayer objDataAccess = null; // Declare here to ensure it's in scope for finally block
            try
            {
                string strHostName = "";
                string IPAddress = "";
                strHostName = System.Net.Dns.GetHostName();

                // Initialize the data access object
                objDataAccess = new clsDataAccessLayer();

                IPHostEntry ipEntry = System.Net.Dns.GetHostEntry(strHostName);
                IPAddress[] addr = ipEntry.AddressList;

                // Use the IP address method
                IPAddress = LocalIPAddress();

                // Validate IP address
                if (string.IsNullOrEmpty(IPAddress))
                {
                    // Fallback to the first available IP from address list
                    IPAddress = addr.Length > 1 ? addr[1].ToString() : (addr.Length > 0 ? addr[0].ToString() : "Unknown");
                }

                // Insert into IpAddress Table
                string strQry = "[dbo].[SP_InsertIP]";
                SqlParamData[] objParams = new SqlParamData[2];

                objParams[0] = new SqlParamData("@LoginID", loginid, SqlDbType.VarChar, 100, ParameterDirection.Input);
                objParams[1] = new SqlParamData("@IPAddress", IPAddress, SqlDbType.VarChar, 100, ParameterDirection.Input);

                // Execute the stored procedure
                objDataAccess.ExecuteStoredProcedure(strQry, objParams);

                System.Threading.Thread.Sleep(200);
                // Optional: Add logging to verify execution
                System.Diagnostics.Debug.WriteLine($"IP inserted for {loginid}: {IPAddress}");

            }
            catch (Exception exp)
            {
                // Proper error logging - uncomment and implement your error handling
                // clsLogicErrorHandler objErr = new clsLogicErrorHandler();
                // objErr.LogErrors(exp);

                // At minimum, log to debug output
                System.Diagnostics.Debug.WriteLine($"Error in InsertIP: {exp.Message}");
                System.Diagnostics.Trace.TraceError($"InsertIP Error: {exp.ToString()}");
            }
            finally
            {
                // Safe disposal of data access object
                if (objDataAccess != null)
                {
                    objDataAccess.CloseDataBase();
                    // If clsDataAccessLayer implements IDisposable, also call Dispose()
                    // objDataAccess.Dispose();
                }
            }
        }




        protected bool CountInvalidAttempts()
        {
            bool returnvalue = false;
            try
            {
                string strHostName = "";
                string IPAddress = "";
                strHostName = System.Net.Dns.GetHostName();

                IPHostEntry ipEntry = System.Net.Dns.GetHostEntry(strHostName);
                IPAddress[] addr = ipEntry.AddressList;
                IPAddress = LocalIPAddress();
                //Insert into IpAddress Table

                clsDataAccessLayer objDataAccess = new clsDataAccessLayer();
                string strQry = "[dbo].[SP_GetCountofInvalidAttempts]";
                SqlParamData[] objParams = new SqlParamData[1];

                //objParams[0] = new SqlParamData("@IPAddress", addr[2].ToString(), SqlDbType.VarChar, 100, ParameterDirection.Input);
                objParams[0] = new SqlParamData("@IPAddress", IPAddress != null ? IPAddress : DBNull.Value, SqlDbType.VarChar, 100, ParameterDirection.Input);
                DataTable lrt = objDataAccess.ExecuteStoredProcedureAndReturnDataTable(strQry, objParams);
                if (lrt.Rows.Count > 0)
                {
                    returnvalue = Convert.ToBoolean(lrt.Rows[0]["counters"].ToString());
                }
                // objDataAccess.CloseDataBase();
            }
            catch (Exception exp)
            {
                //clsLogicErrorHandler objErr = new clsLogicErrorHandler();
                //objErr.LogErrors(exp);
            }
            finally
            {
                objDataAccess.CloseDataBase();
            }
            return returnvalue;
        }
        protected void deleteinvalidattempts()
        {
            try
            {
                string strHostName = "";
                string IPAddress = "";
                strHostName = System.Net.Dns.GetHostName();

                IPHostEntry ipEntry = System.Net.Dns.GetHostEntry(strHostName);
                IPAddress[] addr = ipEntry.AddressList;
                IPAddress = LocalIPAddress();
                //Insert into IpAddress Table

                //    clsDataAccessLayer objDataAccess = new clsDataAccessLayer();
                string strQry = "[dbo].[SP_UpdateInvalidIP]";
                SqlParamData[] objParams = new SqlParamData[1];

                objParams[0] = new SqlParamData("@IPAddress", addr[2].ToString(), SqlDbType.VarChar, 100, ParameterDirection.Input);
                long lrt = objDataAccess.ExecuteStoredProcedureLongSync(strQry, objParams);
                // objDataAccess.CloseDataBase();

            }
            catch (Exception exp)
            {
                //clsLogicErrorHandler objErr = new clsLogicErrorHandler();
                //objErr.LogErrors(exp);
            }
            finally
            {
                objDataAccess.CloseDataBase();
            }
        }

        protected void InsertInvalidIP(string LoginID)
        {
            clsDataAccessLayer objDataAccess = null; // Declare locally
            try
            {
                string strHostName = "";
                string IPAddress = "";
                strHostName = System.Net.Dns.GetHostName();

                IPHostEntry ipEntry = System.Net.Dns.GetHostEntry(strHostName);
                IPAddress = LocalIPAddress();

                // Initialize the data access object
                objDataAccess = new clsDataAccessLayer();

                string strQry = "[dbo].[SP_InsertInvalidIP]";
                SqlParamData[] objParams = new SqlParamData[2];

                objParams[0] = new SqlParamData("@IPAddress", IPAddress != null ? IPAddress : DBNull.Value, SqlDbType.VarChar, 100, ParameterDirection.Input);
                objParams[1] = new SqlParamData("@LoginID", LoginID != null ? LoginID : DBNull.Value, SqlDbType.VarChar, 255, ParameterDirection.Input);

                objDataAccess.ExecuteStoredProcedure(strQry, objParams);

                // Add debug output to verify execution
                System.Threading.Thread.Sleep(200);
                System.Diagnostics.Debug.WriteLine($"Invalid IP logged for {LoginID}: {IPAddress}");
            }
            catch (Exception exp)
            {
                // Log the error properly
                System.Diagnostics.Debug.WriteLine($"Error in InsertInvalidIP: {exp.Message}");
                System.Diagnostics.Trace.TraceError($"InsertInvalidIP Error: {exp.ToString()}");

                // If you have error logging, uncomment this:
                // clsLogicErrorHandler objErr = new clsLogicErrorHandler();
                // objErr.LogErrors(exp);
            }
            finally
            {
                // Safe cleanup
                objDataAccess?.CloseDataBase();

            }
        }



    }
}