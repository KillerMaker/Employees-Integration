using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Gestion_Usuarios_Service_Desk.Models;
using System.Net.Http;
using Newtonsoft.Json;
using System.Net.Http.Json;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;

namespace Gestion_Usuarios_Service_Desk.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RRHHController : ControllerBase
    {
        private readonly SqlQueryBuilder queryBuilder;
        private readonly ILogger<RRHHController> logger;

        public RRHHController(SqlQueryBuilder queryBuilder,ILogger<RRHHController>logger)
        {
            this.queryBuilder = queryBuilder;
            this.logger = logger;
        }

        [HttpPost("SendDataToServiceDesk")]
        public async Task<ActionResult> SendDataToWebService([FromQuery]string modDate=null)
        {
            try
            {
                using (HttpClient client= new HttpClient())
                {
                    List<EmployeeView>employees=(List<EmployeeView>) await EmployeeView.Select(queryBuilder);

                    HttpResponseMessage responseMessage =
                        await client.PostAsJsonAsync("https://localhost:44334/api/Employees/Post", employees);

                    HttpResponseMessage confirm= responseMessage.EnsureSuccessStatusCode();

                    return Ok(new { Message = await confirm.Content.ReadAsStringAsync(), Date = DateTime.Now, Affected = employees.Count });
                }
            }
            catch (Exception e)
            {
                return NotFound(new { Message= e.Message,Date= DateTime.Now, Affected= 0 });
            }
        }

        [HttpGet("test")]
        public ActionResult test(string message)
        {
            return Ok(message);
        }
    }
}
