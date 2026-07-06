using DataAccess;
using MicroserviesWebApplication.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

using DemoProjectService.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MicroserviesWebApplication.Generic_Repo
{


    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync(string spName);
        Task<T> GetByIdAsync(int id, string spName);
        Task AddAsync(List<T> entities, string spName);
        Task<DataTable> AddAsync(List<T> entities, string spName,string returnDataTable);
        Task UpdateAsync(T entity, string spName);
        Task DeleteAsync(int id, [FromBody] T entity, string spName);
        Task<string> GetData(int? id, string spName);
        Task<string> GetData(int? id, string spName, string? RecAddBy, string? RequestType);
        Task<string> GetData(int? id, string spName, string? RecAddBy, string? RequestType, DateTime? StartDate, DateTime? EndDate);
    }

    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<T> _dbSet;
        private readonly ILogger<Repository<T>> _logger;


        public Repository(ApplicationDbContext context, ILogger<Repository<T>> logger)
        {
            _context = context;
            _dbSet = context.Set<T>();
            _logger = logger;
        }
        public async Task<IEnumerable<T>> GetAllAsync(string spName)
        {
            //  DataTable dt= CallGetSP(null,spName);
            ////  if (dt == null) return NotFound();
            //foreach(DataRow dr in dt.Rows)
            //      {

            //  }
            // return JsonSerializer.Serialize(dt).ToList<T>();
            //return dt.ToDataSourceResult(request));
            //return Ok(jsonData);
            return await _dbSet.ToListAsync();
        }
        public async Task<T> GetByIdAsync(int id, string spName) => await _dbSet.FindAsync(id);
        public async Task AddAsync(List<T> entities, string spName)
        {
            try
            {
                CreateJsonCallSP(entities, spName);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error on method: AddAsync Generic Repo: Repository" + " Exception is: ||" + ex.Message + " Trace: ||" + ex.StackTrace);
            }
            

            //=> await _dbSet.AddAsync(entity);
        }

        public async Task UpdateAsync(T entity, string spName)
        {
            //=> _dbSet.Update(entity);
            try
            {            
                CreateJsonCallSP(entity, spName);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error on method: UpdateAsync Generic Repo: Repository" + " Exception is: ||" + ex.Message + " Trace: ||" + ex.StackTrace);
            }

        }

        public async Task DeleteAsync(int id, [FromBody] T entity, string spName)
        {
            //var entity = await GetByIdAsync(id);
            //if (entity != null) _dbSet.Remove(entity);
            try
            {
                CreateJsonCallSP(entity, spName);
            }
            catch(Exception ex)
            {
                _logger.LogError("Error on method: DeleteAsync Generic Repo: Repository" + " Exception is: ||" + ex.Message + " Trace: ||" + ex.StackTrace);
            }
        }
        public async Task CreateJsonCallSP(T entity, string spName)
        {

            try
            {
                var jsonData = JsonSerializer.Serialize(entity);
                clsDataAccessLayer objDataAccess2 = new clsDataAccessLayer();
                SqlParamData[] objParams = new SqlParamData[1];
                objParams[0] = new SqlParamData("@json", jsonData, SqlDbType.NVarChar, 1000, ParameterDirection.Input);
                long lrt = await objDataAccess2.ExecuteStoredProcedureLong(spName, objParams);
            }
            catch(Exception ex)
            {
                _logger.LogError("Error on method: CreateJsonCallSP(T entity) Generic Repo: Repository" + " Exception is: ||" + ex.Message + " Trace: ||" + ex.StackTrace);
            }
            // return lrt;
        }
        public async Task CreateJsonCallSP(List<T> entities, string spName)
        {

            try
            {
                var jsonData = JsonSerializer.Serialize(entities);
                //jsonData = jsonData.Replace("[", "");
                //jsonData = jsonData.Replace("]", "");
                clsDataAccessLayer objDataAccess2 = new clsDataAccessLayer();
                SqlParamData[] objParams = new SqlParamData[1];
                objParams[0] = new SqlParamData("@json", jsonData, SqlDbType.NVarChar, 1000, ParameterDirection.Input);
                long lrt = await objDataAccess2.ExecuteStoredProcedureLong(spName, objParams);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error on method: CreateJsonCallSP(List<T>) Generic Repo: Repository" + " Exception is: ||" + ex.Message + " Trace: ||" + ex.StackTrace);
            }
            // return lrt;
        }

        public async Task<DataTable> CreateJsonCallSP(List<T> entities, string spName, string ReturnDataTable)
        {
            clsDataAccessLayer objDataAccess2 = new clsDataAccessLayer();
            try
            {
                var jsonData = Newtonsoft.Json.JsonConvert.SerializeObject(entities);
                SqlParamData[] objParams = new SqlParamData[1];
                objParams[0] = new SqlParamData("@json", jsonData, SqlDbType.NVarChar, -1, ParameterDirection.Input);
                DataTable dt = objDataAccess2.ExecuteStoredProcedureAndReturnDataTable(spName, objParams);
                return dt;
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    $"Error in CreateJsonCallSP2: {ex.Message} || Trace: {ex.StackTrace}"
                );
                throw; // rethrow for controller
            }
            finally
            {
                objDataAccess2.CloseDataBase();
            }
        }
        public async Task<string> CallGetSP(int? id, string spName, string? RecAddBy, string? requestType, DateTime? StartDate, DateTime? EndDate)
        {
            try
            {
                clsDataAccessLayer objDataAccess2 = new clsDataAccessLayer();
                SqlParamData[] objParams = null;

                //if (id != null)
                //{
                //    SqlParamData[] objParams = new SqlParamData[3];
                //    objParams[0] = new SqlParamData("@Id", id!=null?id:DBNull.Value, SqlDbType.Int, 1000, ParameterDirection.Input);
                //    objParams[1] = new SqlParamData("@RecAddBy", DBNull.Value, SqlDbType.Int, 1000, ParameterDirection.Input);
                //    objParams[2] = new SqlParamData("@RequestType", DBNull.Value, SqlDbType.VarChar, 255, ParameterDirection.Input);
                //    //objParams[3] = new SqlParamData("@StartDate", DBNull.Value, SqlDbType.DateTime, 255, ParameterDirection.Input);
                //    //objParams[4] = new SqlParamData("@EndDate", DBNull.Value, SqlDbType.DateTime, 255, ParameterDirection.Input);

                //}
                //else
                if (StartDate != null && EndDate != null)
                {
                    objParams = new SqlParamData[5];
                    objParams[0] = new SqlParamData("@Id", id != null ? id : DBNull.Value, SqlDbType.Int, 1000, ParameterDirection.Input);
                    objParams[1] = new SqlParamData("@RecAddBy", RecAddBy == null ? DBNull.Value : RecAddBy.Trim(), SqlDbType.Int, 1000, ParameterDirection.Input);
                    objParams[2] = new SqlParamData("@RequestType", requestType == null ? DBNull.Value : requestType.Trim(), SqlDbType.VarChar, 255, ParameterDirection.Input);
                    objParams[3] = new SqlParamData("@StartDate", StartDate == null ? DBNull.Value : StartDate, SqlDbType.DateTime, 255, ParameterDirection.Input);
                    objParams[4] = new SqlParamData("@EndDate", EndDate == null ? DBNull.Value : EndDate, SqlDbType.DateTime, 255, ParameterDirection.Input);

                }
                else
                {
                    objParams = new SqlParamData[3];

                    objParams[0] = new SqlParamData("@Id", id != null ? id : DBNull.Value, SqlDbType.Int, 1000, ParameterDirection.Input);
                    objParams[1] = new SqlParamData("@RecAddBy", RecAddBy == null ? DBNull.Value : RecAddBy.Trim(), SqlDbType.Int, 1000, ParameterDirection.Input);
                    objParams[2] = new SqlParamData("@RequestType", requestType == null ? DBNull.Value : requestType.Trim(), SqlDbType.VarChar, 255, ParameterDirection.Input);
                    //objParams[3] = new SqlParamData("@StartDate", StartDate == null ? DBNull.Value : StartDate, SqlDbType.DateTime, 255, ParameterDirection.Input);
                    //objParams[4] = new SqlParamData("@EndDate", EndDate == null ? DBNull.Value : EndDate, SqlDbType.DateTime, 255, ParameterDirection.Input);

                }

                DataTable dt = objDataAccess2.ExecuteStoredProcedureAndReturnDataTable(spName, objParams);
                //var jsonResult = await objDataAccess2.ExecuteStoredProcedureAndReturnJsonAsync(spName, objParams);
                var jsonResult = ConvertDataTableToJson(dt);
                return jsonResult;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error on method: CallGetSP(6) Generic Repo: Repository" + "SpName"+spName+" Exception is: ||" + ex.Message + " Trace: ||" + ex.StackTrace);
                return $"Error on method: CallGetSP(6) || Generic Repo:Repository ||  Error: {ex.Message}";
            }
        }

        public async Task<string> CallGetSP(int? id, string spName)
        {
            try
            {
                clsDataAccessLayer objDataAccess2 = new clsDataAccessLayer();

                SqlParamData[] objParams = new SqlParamData[1];
                if (id != null)
                {
                    objParams[0] = new SqlParamData("@Id", id, SqlDbType.Int, 1000, ParameterDirection.Input);
                }
                else
                {

                    objParams[0] = new SqlParamData("@Id", DBNull.Value, SqlDbType.Int, 1000, ParameterDirection.Input);
                }

                DataTable dt = objDataAccess2.ExecuteStoredProcedureAndReturnDataTable(spName, objParams);
                //var jsonResult = await objDataAccess2.ExecuteStoredProcedureAndReturnJsonAsync(spName, objParams);
                var jsonResult = ConvertDataTableToJson(dt);
                return jsonResult;

            }
            catch(Exception ex)
            {
                _logger.LogError("Error on method: CallGetSP(2) Generic Repo: Repository"+"spName"+ spName + " Exception is: ||" + ex.Message + " Trace: ||" + ex.StackTrace);
                return $"Error on method: CallGetSP(2) || Generic Repo:Repository ||  Error: {ex.Message}";
            }
        }
        private string ConvertDataTableToJson(DataTable dataTable)
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
        public async Task<string> GetData(int? id, string spName)
        {
            // return await _dbSet.ToListAsync();
            try
            {

                var result = await CallGetSP(id, spName);
                //var result = JsonSerializer.Deserialize<IEnumerable<MyDataModel>>(jsonResult);

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error on method: GetData(2) Generic Repo: Repository" + "SpName" + spName + " Exception is: ||" + ex.Message + " Trace: ||" + ex.StackTrace);
                return $"Error on method: GetData(2) || Generic Repo:Repository ||  Error: {ex.Message}";
            }
        }
        public async Task<string> GetData(int? id, string spName, string? RecAddBy, string? RequestType)
        {
            // return await _dbSet.ToListAsync();
            try
            {

                var result = await CallGetSP(id, spName, RecAddBy, RequestType, null, null);
                //var result = JsonSerializer.Deserialize<IEnumerable<MyDataModel>>(jsonResult);

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error on method: GetData  Generic Repo: Repository" + "SpName" + spName + " Exception is: ||" + ex.Message + " Trace: ||" + ex.StackTrace);
                return $"Error on method: GetData(4) || Generic Repo:Repository ||  Error: {ex.Message}";
            }
        }
        public async Task<DataTable> AddAsync(List<T> entities, string spName, string returnDataTable)
        {
            try
            {
                return await CreateJsonCallSP(entities, spName, returnDataTable);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error on method: AddAsync 3rd parameter Generic Repo: Repository" + " Exception is: ||" + ex.Message + " Trace: ||" + ex.StackTrace);
                throw;
            }


            //=> await _dbSet.AddAsync(entity);
        }
        private async Task<DataTable> CreateJsonCallSP_ReturnDT(List<T> entities, string spName)
        {
            clsDataAccessLayer objDataAccess = new clsDataAccessLayer();
            try
            {
                var jsonData = Newtonsoft.Json.JsonConvert.SerializeObject(entities);

                SqlParamData[] objParams =
                {
                    new SqlParamData("@json", jsonData, SqlDbType.NVarChar, -1, ParameterDirection.Input)
                };

                return objDataAccess.ExecuteStoredProcedureAndReturnDataTable(spName, objParams);
            }
            finally
            {
                objDataAccess.CloseDataBase();
            }
        }


        public async Task<DataTable> AddAndReturnAsync(List<T> entities, string spName)
        {
            try
            {
                return await CreateJsonCallSP_ReturnDT(entities, spName);
            }
            catch (Exception ex)
            {
                _logger.LogError("AddAndReturnAsync Error: " + ex.Message);
                throw;
            }
        }
        public async Task<string> GetData(int? id, string spName, string? RecAddBy, string? RequestType, DateTime? StartDate, DateTime? EndDate)
        {
            try
            {
                // return await _dbSet.ToListAsync();

                var result = await CallGetSP(id, spName, RecAddBy, RequestType, StartDate, EndDate);
                //var result = JsonSerializer.Deserialize<IEnumerable<MyDataModel>>(jsonResult);

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error on method: GetData(6)  Generic Repo: Repository" +"SpName"+ spName+ " Exception is: ||" + ex.Message + " Trace: ||" + ex.StackTrace);
                return $"Error on method: GetData(6) || Generic Repo:Repository ||  Error: {ex.Message}";
            }
        }

    }

}

