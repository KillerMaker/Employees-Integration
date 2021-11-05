using Gestion_Usuarios_Service_Desk.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Gestion_Usuarios_Service_Desk.Middlewares
{
    public class LogResponses
    {
        private readonly RequestDelegate next;
        private readonly string path;
        private readonly ILogger<LogResponses> logger;

        public LogResponses(RequestDelegate next, string path,ILogger<LogResponses> logger)
        {
            this.next = next;
            this.path = path;
            this.logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            Stream originalBody = context.Response.Body;

            try
            {
                 if (context.Request.Path.StartsWithSegments(path))
                 {
                    using (var memStream = new MemoryStream())
                    {
                        context.Response.Body = memStream;

                        await next(context);

                        memStream.Position = 0;
                        logger.LogInformation(await new StreamReader(memStream).ReadToEndAsync());
                        memStream.Position = 0;

                        await memStream.CopyToAsync(originalBody);
                    }
                }
                else
                    await next.Invoke(context);
                    
               
            }
            finally
            {
                context.Response.Body = originalBody;
            }

        }
    }
}
