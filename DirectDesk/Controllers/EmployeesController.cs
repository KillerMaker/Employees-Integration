using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using DirectDesk.Models;
using System.Threading.Tasks;
using System.Data.SqlClient;
#nullable enable

namespace DirectDesk.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        public EmployeesController()
        {
                
        }
        [HttpPost("Post")]
        public async Task<ActionResult>PostData( IEnumerable<Employee>employees)
        {
            try
            {
                List<string> querys = new List<string>();
                List<SqlParameter[]> parameters = new List<SqlParameter[]>();

                foreach (Employee employee in employees)
                {
                    querys.Add(employee.GetInsertInsertString());
                    parameters.Add(employee.GetParameters().ToArray());
                }

                await QueryBuilder.ExecuteQuery(querys.ToArray(), parameters);

                return Ok("Insertados correctamente");
            }
            catch(Exception e)
            {
                throw new Exception(e.Message);
            }


        }
    }
}
