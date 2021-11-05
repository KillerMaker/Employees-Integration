using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gestion_Usuarios_Service_Desk.Middlewares
{
    public static class MiddlewaresExtensionMethods
    {
        public static IApplicationBuilder UseLogSendDataToServiceDeskResponses(this IApplicationBuilder app) =>
            app.UseMiddleware<LogResponses>("/api/RRHH/SendDataToServiceDesk");
        public static IApplicationBuilder UseLogTest(this IApplicationBuilder app) =>
            app.UseMiddleware<LogResponses>("/api/RRHH/test");
    }
}
