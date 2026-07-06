using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Reflection;
using System.IO;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Text.Json;

namespace DemoProjectService.Classes
{
    /// <summary>
    /// Summary description for clsFunctions
    /// </summary>
    public class clsFunctions
    {
        public clsFunctions()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public string toSeperatorString(String[] strArray, string strSeperator, string strSuppress)
        {
            string rt = "";
            string tmp = "";
            for (int i = 0; i < strArray.Length; i++)
            {
                tmp = strArray[i];
                if (strSuppress.Length > 0)
                    tmp = tmp.Replace(strSuppress, "");
                //rt = rt + strSeperator + (i+1) + " AS [" + tmp + "]";
                rt = rt + strSeperator + " [" + tmp + "]";
            }
            if (rt.Length > 0)
                rt = rt.Substring(strSeperator.Length);
            return rt;
        }

        public int toInt(string strInt)
        {
            int rt = int.MinValue;
            try
            {
                rt = Convert.ToInt32(strInt);
            }
            catch (Exception ex)
            {
                rt = int.MinValue;
            }
            return rt;
        }

        public long toLong(string strLong)
        {
            long rt = long.MinValue;
            try
            {
                rt = Convert.ToInt64(strLong);
            }
            catch (Exception ex)
            {
                rt = long.MinValue;
            }
            return rt;
        }

        public decimal toDecimal(string strDecimal)
        {
            decimal rt = decimal.MinValue;
            try
            {
                rt = Convert.ToDecimal(strDecimal);
            }
            catch (Exception ex)
            {
                rt = decimal.MinValue;
            }
            return rt;
        }

        public DateTime toDate(string strDate, string strFormat)
        {
            strFormat = strFormat.Trim().Length > 0 ? strFormat.Trim() : "dd-MMM-yyyy";
            if (strDate.Trim().Length > 0)
                return DateTime.ParseExact(strDate, strFormat, null);
            else
                return DateTime.MinValue;
        }

        public string toDate(DateTime dtValue)
        {
            return dtValue.ToString("dd-MMM-yyyy");
        }

        public string toXMLString(string strXMLString)
        {
            return strXMLString.Replace("\r\n", "").Replace("\"", "&quot;").Replace("'", "&apos;").Replace("xml:space=&quot;preserve&quot;", "");
        }

        public string toXMLNodeString(string strXMLString)
        {
            return strXMLString.Replace("\r\n", "").Replace("&", "&amp;").Replace("\"", "&quot;").Replace("'", "&apos;").Replace("xml:space=&quot;preserve&quot;", "").Replace("<", "&lt;").Replace(">", "&gt;");
        }

        public long getComboValue(string strValue, string strDefault)
        {
            long rtVal = long.MinValue;
            strDefault = strDefault.Trim();
            if (strDefault.Length == 0 && strValue.Trim().Length == 0)
                rtVal = long.MinValue;
            else if (strDefault.Equals(strValue.Trim()))
                rtVal = long.MinValue;
            else
                rtVal = Convert.ToInt64(strValue);
            return rtVal;
        }
        public string setDisplayValue(object objVar, string defaultValue = "")
        {
            try
            {
                if (objVar.GetType() != typeof(DBNull) && objVar != null)
                {
                    if (objVar.GetType() == typeof(String) && objVar != String.Empty)
                        return objVar.ToString();
                    else if ((objVar.GetType() == typeof(Int32) && toInt(objVar.ToString()) == Int32.MinValue) ||
                        (objVar.GetType() == typeof(Int64) && toLong(objVar.ToString()) == long.MinValue) ||
                        (objVar.GetType() == typeof(long) && toLong(objVar.ToString()) == long.MinValue) ||
                        (objVar.GetType() == typeof(decimal) && toDecimal(objVar.ToString()) == decimal.MinValue))
                        return defaultValue;
                    else if (objVar.GetType() == typeof(DateTime))
                    {
                        if (toDate(objVar) == DateTime.MinValue)
                            return defaultValue;
                        else
                            return toDate(toDate(objVar));
                    }
                    else
                        return objVar.ToString();
                }
                else
                    return defaultValue;
            }
            catch (Exception ex)
            {
                return defaultValue;
            }
            return defaultValue;
        }
        public string getComboValueString(string strValue, string strDefault)
        {
            string rtVal = string.Empty;
            strDefault = strDefault.Trim();
            if (strDefault.Length == 0 && strValue.Trim().Length == 0)
                rtVal = string.Empty;
            else if (strDefault.Equals(strValue.Trim()))
                rtVal = string.Empty;
            else
                rtVal = strValue;
            return rtVal;
        }
        public DateTime toDate(object dtValue)
        {
            try
            {
                return Convert.ToDateTime(dtValue);
            }
            catch (Exception ex)
            {
            }
            return DateTime.MinValue;
        }
        public string getString(string strValue)
        {
            string rtVal = string.Empty;
            if (strValue.Trim().Length == 0)
                rtVal = string.Empty;
            else
                rtVal = strValue;
            return rtVal;
        }

     
       

      

      

        #region Binary Serializers
        public static System.IO.MemoryStream SerializeBinary(object request)
        {
            System.Runtime.Serialization.Formatters.Binary.BinaryFormatter serializer =
            new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            System.IO.MemoryStream memStream = new System.IO.MemoryStream();
            serializer.Serialize(memStream, request);
            return memStream;
        }

        public static object DeSerializeBinary(System.IO.MemoryStream memStream)
        {
            memStream.Position = 0;
            System.Runtime.Serialization.Formatters.Binary.BinaryFormatter deserializer =
            new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            object newobj = deserializer.Deserialize(memStream);
            memStream.Close();
            return newobj;
        }
        #endregion

        //public static void SaveEntity(StateBag viewstate, BusinessLogic.EntityClasses.clsEmployee objEntity)
        public static void SaveEntity(ITempDataDictionary viewstate, object objEntity)
        {
            Type theType = objEntity.GetType();
            PropertyInfo[] properties = theType.GetProperties();
            foreach (PropertyInfo property in properties)
            {
                viewstate.Add(property.Name, property.GetValue(objEntity, null));
            }
        }

        //public static BusinessLogic.EntityClasses.clsEmployee RetrieveEntity(StateBag viewstate, BusinessLogic.EntityClasses.clsEmployee objEntity)
        public static object RetrieveEntity(ITempDataDictionary viewstate, object objEntity)
        {
            Type theType = objEntity.GetType();
            PropertyInfo[] properties = theType.GetProperties();
            foreach (PropertyInfo property in properties)
            {
                property.SetValue(objEntity, viewstate[property.Name], null);
            }
            return objEntity;
        }

        //public CustomReportEngine.clsReportHandler GetReportDocument(BusinessLogic.EntityClasses.clsReportCaller objECls)
        //{
        //    CustomReportEngine.clsReportHandler objReportHandler = new CustomReportEngine.clsReportHandler();
        //    objReportHandler.ResourceName = objECls.FileName;
        //    objReportHandler.setConnectionInfo();

        //    foreach (KeyValuePair<string, object> param in objECls.Parameters)
        //        objReportHandler.SetParameterValue(param.Key, param.Value);
        //    foreach (KeyValuePair<string, object> formula in objECls.Formulas)
        //        objReportHandler.DataDefinition.FormulaFields[formula.Key].Text = formula.Value.ToString();

        //    objReportHandler.RecordSelectionFormula = objECls.SelectionFormula;
        //    return objReportHandler;
        //}

        //public void ExportReport(BusinessLogic.EntityClasses.clsReportCaller objECls, HttpResponse Response, string ExportFileName)
        //{
        //    CustomReportEngine.clsReportHandler objReportHandler = GetReportDocument(objECls);
        //    MemoryStream oStream = new MemoryStream();
        //    try
        //    {
        //        switch (objECls.ReportFormat)
        //        {
        //            case "4":
        //                oStream = (MemoryStream)objReportHandler.ExportToStream(CrystalDecisions.Shared.ExportFormatType.WordForWindows);
        //                Response.Clear();
        //                Response.AddHeader("content-disposition", "attachment;filename=" + ExportFileName + ".rtf");
        //                Response.Buffer = true;
        //                Response.ContentType = "application/rtf";
        //                break;
        //            case "1":
        //                oStream = (MemoryStream)objReportHandler.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
        //                Response.Clear();
        //                Response.AddHeader("content-disposition", "attachment;filename=" + ExportFileName + ".pdf");
        //                Response.Buffer = true;
        //                Response.ContentType = "application/pdf";
        //                break;
        //            case "2":
        //                oStream = (MemoryStream)objReportHandler.ExportToStream(CrystalDecisions.Shared.ExportFormatType.WordForWindows);
        //                Response.Clear();
        //                Response.AddHeader("content-disposition", "attachment;filename=" + ExportFileName + ".doc");
        //                Response.Buffer = true;
        //                Response.ContentType = "application/doc";
        //                break;
        //            case "3":
        //                oStream = (MemoryStream)objReportHandler.ExportToStream(CrystalDecisions.Shared.ExportFormatType.Excel);
        //                Response.Clear();
        //                Response.AddHeader("content-disposition", "attachment;filename=" + ExportFileName + ".xls");
        //                Response.Buffer = true;
        //                Response.ContentType = "application/vnd.ms-excel";
        //                break;
        //        }
        //        Response.BinaryWrite(oStream.ToArray());
        //        Response.End();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //        //Response.Write("<BR>");
        //        //Response.Write(err.Message.ToString());
        //    }
        //}

        public static string FormatGridDate(object date)
        {
            string strRt = "";
            try
            {
                if (date != null)
                {
                    if (Convert.ToDateTime(date) != DateTime.MinValue)
                    {
                        strRt = Convert.ToDateTime(date).ToString("dd-MMM-yyyy");
                    }
                }
            }
            catch (Exception ex)
            { }
            return strRt;
        }

        public string Encode(string data)
        {
            return Convert.ToString(HttpUtility.UrlEncode(Encrypt(data)));
           
            //return Convert.ToString(HttpUtility.UrlEncode(data));

            //    byte[] toEncodeAsBytes =
            //System.Text.Encoding.UTF8.GetBytes(data);
            //    string returnValue =
            //    System.Convert.ToBase64String(toEncodeAsBytes);
            //    return returnValue;
        }
        public string Decode(string data)
        {
            return Convert.ToString(Decrypt(HttpUtility.UrlDecode(data)));
            //return Convert.ToString(HttpUtility.UrlDecode(data));
            //     byte[] encodedDataAsBytes =
            //System.Convert.FromBase64String(data);
            //     string returnValue =
            //     System.Text.Encoding.UTF8.GetString(encodedDataAsBytes);
            //     return returnValue;
        }

        private string Encrypt(string clearText)
        {
            string EncryptionKey = "MAKV2SPBNI99212";
            byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    clearText = Convert.ToBase64String(ms.ToArray());
                }
            }
            return clearText;
        }
        private string Decrypt(string cipherText)
        {
            try
            {
                string EncryptionKey = "MAKV2SPBNI99212";
                cipherText = cipherText.Replace(" ", "+");
                byte[] cipherBytes = Convert.FromBase64String(cipherText);
                using (Aes encryptor = Aes.Create())
                {
                    Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                    encryptor.Key = pdb.GetBytes(32);
                    encryptor.IV = pdb.GetBytes(16);
                    using (MemoryStream ms = new MemoryStream())
                    {
                        using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                        {
                            cs.Write(cipherBytes, 0, cipherBytes.Length);
                            cs.Close();
                        }
                        cipherText = Encoding.Unicode.GetString(ms.ToArray());
                    }
                }
                return cipherText;


            }
            catch (Exception)
            {
                return null;
            }
        }

        public string ConvertDataTableToJson(DataTable dataTable)
        {
            // Extract rows into a list of dictionaries
            var rows = new List<Dictionary<string, object>>();

            foreach (DataRow row in dataTable.Rows)
            {
                var rowDict = new Dictionary<string, object>();
                foreach (DataColumn column in dataTable.Columns)
                {
                    rowDict[column.ColumnName] = row[column];
                }
                rows.Add(rowDict);
            }

            // Serialize the rows
            return JsonSerializer.Serialize(rows);
        }
    }
}
    

