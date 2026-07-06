
using DemoProjectService.Classes;
using MicroserviesWebApplication.Generic_Repo;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Threading.Tasks;
using AuthenticationService = DemoProjectService.Classes.AuthenticationService;
namespace MicroserviesWebApplication.Controllers
{
   

    [Route("api/[controller]")]
    [ApiController]
    public class GenericController<T> : ControllerBase where T : class
    {
        private readonly IRepository<T> _repository;
        private AuthenticationService authservice = new AuthenticationService();
        public GenericController(IRepository<T> repository)
        {
            _repository = repository;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll(string spName, string? RecAddBy, string? RequestType)
        {
            var jsonResult = "";


            //return Ok(await _repository.GetAllAsync(""));
            if (RecAddBy == null || RequestType == null)
                jsonResult = await _repository.GetData(null, spName);
            else
                jsonResult = await _repository.GetData(null, spName, RecAddBy, RequestType);

            if (string.IsNullOrEmpty(jsonResult))
            {
                return NotFound();
            }
            return Ok(jsonResult);
            //  var result = JsonSerializer.Deserialize<IEnumerable<MyDataModel>>(jsonResult);
        }


        [HttpPost]
        public async Task<IActionResult> Create([FromBody] List<T> items, string spName,string? ReturnDataTable)
        {
            //            spName = "[dbo].[InsertJsonCountry]";
            if (ReturnDataTable != null)
            {
                // ? Capture the returned DataTable
                DataTable dt = await _repository.AddAsync(items, spName, ReturnDataTable);

                if (dt != null && dt.Rows.Count > 0)
                {
                    // Convert DataTable rows to a list of dictionaries for clean JSON response
                    var result = dt.Rows
                        .Cast<DataRow>()
                        .Select(row => dt.Columns
                            .Cast<DataColumn>()
                            .ToDictionary(col => col.ColumnName, col => row[col] == DBNull.Value ? null : row[col])
                        ).ToList();

                    return Ok(new { success = true, data = result });
                }

                return Ok(new { success = true, message = "Inserted, but no data returned." });
            }
            else
            {
                await _repository.AddAsync(items, spName);
                return Ok(new { success = true, message = "Records inserted successfully." });
            }
            //return CreatedAtAction(nameof(GetById), new { id = (items[0] as dynamic).Id }, items[0]);
            //return CreatedAtAction(nameof(GetById),new { id =   },new{Id = id,Message = message});
        }
        //[HttpPost]
        //public async Task<IActionResult> Create([FromBody] List<T> items, string spName,string ReturnDataTable)
        //{
        //    //            spName = "[dbo].[InsertJsonCountry]";
        //    await _repository.AddAsync(items, spName,ReturnDataTable);
        //    return CreatedAtAction(nameof(GetById), new { id = (items[0] as dynamic).Id }, items[0]);
        //}

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] T entity, string spName)
        {
            //            spName = "[dbo].[InsertJsonCountry]";
            if (id != (entity as dynamic).Id) return BadRequest();
            await _repository.UpdateAsync(entity, spName);
            return Ok("Updated Successfully");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, [FromBody] T entity, string spName)
        {
            //            spName = "[dbo].[InsertJsonCountry]";
            if (id != (entity as dynamic).Id) return BadRequest();

            await _repository.DeleteAsync(id, entity, spName);
            return Ok("Data has been deleted");
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id, string spName)
        {
            // string storedProcedureName = "GetMyData"; // Replace with your SP name

            var jsonResult = await _repository.GetData(id, spName);

            if (string.IsNullOrEmpty(jsonResult))
            {
                return NotFound();
            }

            //  var result = JsonSerializer.Deserialize<IEnumerable<MyDataModel>>(jsonResult);

            return Ok(jsonResult);
        }
    }
}
