using DataAccess;
//using System.Reflection.PortableExecutable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.DirectoryServices;

using System.Data;
using Microsoft.AspNetCore.Http;
using MicroserviesWebApplication.Data;

namespace DemoProjectService.Classes
{
    public class ADServices
    {
       // private readonly IHttpContextAccessor _httpContextAccessor;
        private  string strDomain = ""; // Replace with your domain

        public IConfigurationRoot GetConfiguration()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            return builder.Build();
        }
        public ADServices()// IHttpContextAccessor httpContextAccessor)
        {
           // _httpContextAccessor = httpContextAccessor;
        }

        public bool IsUserADorBasic(string strLoginID)
        {
            Boolean bRt = false;
            try
            {
                clsDataAccessLayer objDataAccess = new clsDataAccessLayer();
                DataTable dt = null;
                try
                {
                    DataTable dtPass = null;
                    string strQryPass = "SELECT isnull(isbasicauth,0) as  isbasicauth FROM [dbo].[User] " +
                                       " where LoginID='" + strLoginID + "' and rec_status=1";

                    dtPass = objDataAccess.ExecuteDataTable(strQryPass);
                    if (dtPass.Rows.Count > 0)
                    {
                        if (Convert.ToBoolean(dtPass.Rows[0]["isbasicauth"].ToString()))
                        {
                            bRt = true;//its an basic auth
                        }
                        else
                        {
                            bRt = false;//its an AD user
                        }
                    }
                }
                catch (Exception exp)
                {
                    //clsLogicErrorHandler objErr = new clsLogicErrorHandler();
                    //objErr.LogErrors(exp);
                }
                finally
                {
                    objDataAccess.CloseDataBase();
                    dt.Dispose();
                }
            }
            catch
            { }
            return bRt;
        }
        public string TrimPNO(string value)
        {
            string a = "\\";
            int posA = value.LastIndexOf(a);
            if (posA == -1)
            {
                return value;
            }
            int adjustedPosA = posA + a.Length;
            if (adjustedPosA >= value.Length)
            {
                return value;
            }
            return value.Substring(adjustedPosA);
        }
        public string GetADNameByID(string AD_UserID)
        {
            string Name = "";
         
            var configuation = GetConfiguration();
           string CheckfromAD = (configuation.GetSection("CheckfromAD").Value).ToString();
            if (CheckfromAD.ToUpper() == "FALSE")
            {
                return "Spadmin";
            }
            string[] SearchResult = GetSingleDetails(AD_UserID);

            if (SearchResult.Length > 0)
            {
                Name = SearchResult[1] + " " + SearchResult[2];

            }
            return Name;

        }
        public Dictionary<string, object> GetAD_DatByID(string AD_UserID)
        {
            Dictionary<string, object> dicObjParams = new Dictionary<string, object>();
            string Name, Email, First_Name, Last_Name = "";
             var configuation = GetConfiguration();
            string CheckfromAD = (configuation.GetSection("CheckfromAD").Value).ToString();
            if (CheckfromAD.ToUpper() == "FALSE")
            {
                dicObjParams.Add("@Name", "Spadmin");
                dicObjParams.Add("@First_Name", "Sp");
                dicObjParams.Add("@Last_Name", "admin");
                dicObjParams.Add("@Email", "Spadmin@Email.com");
            }
            else
            {
                string[] SearchResult = GetSingleDetails(AD_UserID);

                if (SearchResult.Length > 0)
                {
                    Name = SearchResult[1] + " " + SearchResult[2];
                    dicObjParams.Add("@Name", Name);
                    First_Name = SearchResult[1].ToString();
                    dicObjParams.Add("@First_Name", First_Name);
                    Last_Name = SearchResult[2].ToString();
                    dicObjParams.Add("@Last_Name", Last_Name);
                    Email = SearchResult[3].ToString();
                    dicObjParams.Add("@Email", Email);

                }
            }
            return dicObjParams;

        }
        public bool ADUserVerify(string AD_UserID, string Password)
        {
            bool chk = true;
           
            var configuation = GetConfiguration();
            string CheckfromAD = (configuation.GetSection("CheckfromAD").Value).ToString();
            if (CheckfromAD.ToUpper() == "FALSE")
            {
                return true;
            }
            {
                chk = Authenticate(AD_UserID, Password);
            }
            return chk;

        }
        #region "Private Variables"
        string strUsername;
        string strPassword;
      
        #endregion

        #region "Consructor"
        public void Active_directory()
        {


            //String domain = System.Web.Configuration.WebConfigurationManager.AppSettings["DomainName"].ToString().ToUpper().Trim();
            //if (domain == "ENI.INTRANET/".ToUpper())
            //{
            var configuation = GetConfiguration();
            string CheckfromAD = (configuation.GetSection("CheckfromAD").Value).ToString();
            strDomain = (configuation.GetSection("LDAP_INTRANET").Value.ToString());
            //}
            //else if (domain == "Prime-Pakistan.com/".ToUpper())
            //{
            //    strDomain = System.Web.Configuration.WebConfigurationManager.AppSettings["LDAP_PRIM"];

            // }

        }

        #endregion
        private void writeMsg(string msg)
        {
            string filePath = @"E:\Error.txt";
            using (System.IO.StreamWriter writer = new System.IO.StreamWriter(filePath, true))
            {
                writer.WriteLine("-----------------------------------------------------------------------------");
                writer.WriteLine("Date : " + DateTime.Now.ToString());
                writer.WriteLine();


                writer.WriteLine(msg);



            }
        }
        #region "Authenticate User"

        public bool Authenticate(string Username, string Password)
        {
            Active_directory();
            //now create the directory entry to establish connection
             //  writeMsg("ConnectingLDAP");
            try
            {
                using (DirectoryEntry directoryEntry = new DirectoryEntry(strDomain, Username, Password))
                {
                    // Force a bind to the native object to authenticate
                    object nativeObject = directoryEntry.NativeObject;
                    clsFunctions func = new clsFunctions();
                    // Store the encrypted password in session (if needed)
                    ////var session = _httpContextAccessor.HttpContext.Session;
                    ////session.SetString("PassEncrypt",func.Encode(Password));

                    return true;
                }
            }
            catch
            {
                // writeMsg("error occured");
                return false;
            }
        }
        #endregion

        #region "Get Details"
        public string GetProperty(SearchResult SearchResult, string PropertyName)
        {
            if (SearchResult.Properties.Contains(PropertyName))
            {
                return SearchResult.Properties[PropertyName][0].ToString();
            }
            else
            {
                return string.Empty;
            }
        }
        public string[] GetSingleDetails(string AD_ID)
        {
            AD_ID = TrimPNO(AD_ID);
            string[] SearchResult = new string[4];

            Active_directory();

            DirectoryEntry DEntry = new DirectoryEntry(strDomain);
            DirectorySearcher DSearch = new DirectorySearcher(DEntry);
            DSearch.Filter = "(&(cn=" + AD_ID + ")(objectCategory=person))";
            //   DSearch.Filter = "(&(anr=" + SName + ")(objectCategory=person))";


            foreach (SearchResult sResultSet in DSearch.FindAll())
            {
                // Login Name
                SearchResult[0] = GetProperty(sResultSet, "cn");

                // First Name
                SearchResult[1] = GetProperty(sResultSet, "givenName");

                // Middle Initials
                SearchResult[2] = GetProperty(sResultSet, "sn");
                SearchResult[3] = GetProperty(sResultSet, "mail");
            }
            return SearchResult;
        }
        public string[] GetDetails(string AD_ID)
        {
            AD_ID = TrimPNO(AD_ID);
            string[] SearchResult = new string[41];

            Active_directory();

            DirectoryEntry DEntry = new DirectoryEntry(strDomain);
            DirectorySearcher DSearch = new DirectorySearcher(DEntry);
            DSearch.Filter = "(&(cn=" + AD_ID + ")(objectCategory=person))";
            //   DSearch.Filter = "(&(anr=" + SName + ")(objectCategory=person))";


            foreach (SearchResult sResultSet in DSearch.FindAll())
            {
                // Login Name
                SearchResult[0] = GetProperty(sResultSet, "cn");

                // First Name
                SearchResult[1] = GetProperty(sResultSet, "givenName");

                // Middle Initials
                SearchResult[2] = GetProperty(sResultSet, "sn");

                // Last Name
                SearchResult[3] = GetProperty(sResultSet, "physicalDeliveryOfficeName");

                // Address
                //  Dim tempAddress As String

                // tempAddress = GetProperty(sResultSet, "homePostalAddress");

                // If tempAddress <> String.Empty Then
                // string[] addressArray = tempAddress.Split(//;//);
                // string taddr1,taddr2;
                // taddr1=addressArray[0];
                // Console.Write(taddr1);
                // taddr2=addressArray[1];
                // Console.Write(taddr2);
                //End If

                SearchResult[4] = GetProperty(sResultSet, "description");

                //title
                SearchResult[5] = GetProperty(sResultSet, "title");

                //company
                SearchResult[6] = GetProperty(sResultSet, "department");

                //state
                SearchResult[7] = GetProperty(sResultSet, "st");

                //city
                SearchResult[8] = GetProperty(sResultSet, "l");

                //country
                SearchResult[9] = GetProperty(sResultSet, "co");

                //postalcode
                SearchResult[10] = GetProperty(sResultSet, "postalCode");

                //telephonenumber
                SearchResult[11] = GetProperty(sResultSet, "telephoneNumber");

                //extention
                SearchResult[12] = GetProperty(sResultSet, "otherTelephone");

                //fax
                SearchResult[13] = GetProperty(sResultSet, "facsimileTelephoneNumber");

                //email address
                SearchResult[14] = GetProperty(sResultSet, "mail");

                //Challenge Question
                SearchResult[15] = GetProperty(sResultSet, "extensionAttribute1");

                //Challenge Response
                SearchResult[16] = GetProperty(sResultSet, "extensionAttribute2");

                //Member Company
                SearchResult[17] = GetProperty(sResultSet, "extensionAttribute3");

                //Company Relation ship Exits
                SearchResult[18] = GetProperty(sResultSet, "extensionAttribute4");

                //status
                SearchResult[19] = GetProperty(sResultSet, "extensionAttribute5");

                //Assigned Sales Person
                SearchResult[20] = GetProperty(sResultSet, "extensionAttribute6");

                //Accept T and C

                SearchResult[21] = GetProperty(sResultSet, "extensionAttribute7");

                //jobs
                SearchResult[22] = GetProperty(sResultSet, "extensionAttribute8");

                SearchResult[23] = GetProperty(sResultSet, "extensionAttribute9");

                //email over night
                //    If (tEamil! = String.Empty) Then
                //   {
                //      string em1,em2,em3;
                //    string[] emailArray = tEmail.Split(';');
                //      em1=emailArray[0];
                //      em2=emailArray[1];
                //      em3=emailArray[2];
                //      Console.Write(em1+em2+em3);

                //   }

                //email daily emerging market
                SearchResult[25] = GetProperty(sResultSet, "extensionAttribute10");

                //email daily corporate market
                SearchResult[26] = GetProperty(sResultSet, "extensionAttribute11");

                //AssetMgt Range
                SearchResult[27] = GetProperty(sResultSet, "extensionAttribute12");

                //date of account created
                SearchResult[28] = GetProperty(sResultSet, "whenCreated");

                //date of account changed
                SearchResult[29] = GetProperty(sResultSet, "whenChanged");


                SearchResult[30] = GetProperty(sResultSet, "OU");
            }
            return SearchResult;
        }
        #endregion

        #region "Unlock"
        public void Unlock(string UserDN)
        {
            try
            {
                strUsername = UserDN;
                DirectoryEntry UEntry = new DirectoryEntry(strDomain, strUsername, strPassword);
                UEntry.Properties["LockOutTime"].Value = 0;
                UEntry.Close();
            }
            catch (System.DirectoryServices.DirectoryServicesCOMException ex)
            {
                throw ex;
            }
        }
        #endregion

        public async Task<string> VerifyUserAD(string loginid)
        {
            var jsonResult = "";
            try
            {
                clsDataAccessLayer objDataAccess = new clsDataAccessLayer();
                DataTable dt = null;
                try
                {
                    string strQry = "[dbo].[SP_VerifyLoginCredentials]";// "[SP_VerifyADLoginCredentials]";
                    SqlParamData[] objParams = new SqlParamData[2];

                    objParams[0] = new SqlParamData("@LoginID", loginid, SqlDbType.VarChar, 100, ParameterDirection.Input);
                    objParams[1] = new SqlParamData("@Password", DBNull.Value, SqlDbType.VarChar, 100, ParameterDirection.Input);
                    dt = objDataAccess.ExecuteStoredProcedureAndReturnDataTable(strQry, objParams);
                    if (dt.Rows.Count > 0)
                    {

                        dt.Rows[0]["UserName"] = new ADServices();//_httpContextAccessor).GetADNameByID(loginid);
                    }
                     jsonResult = new clsFunctions().ConvertDataTableToJson(dt);
                  //  return jsonResult;

                }
                catch (Exception exp)
                {
                    //clsLogicErrorHandler objErr = new clsLogicErrorHandler();
                    //objErr.LogErrors(exp);
                }
                finally
                {
                    objDataAccess.CloseDataBase();
                    dt.Dispose();
                }
            }
            catch
            { }

            return jsonResult;
            //  return loginid == "sqlUser" && password == "sqlPassword"; // Simulated check
        }
    }
}
