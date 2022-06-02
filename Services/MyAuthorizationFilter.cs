using Hangfire.Dashboard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bicks.Services
{
    public class MyAuthorizationFilter : IDashboardAuthorizationFilter
    {
        public bool Authorize(DashboardContext context)
        {
            var httpContext = context.GetHttpContext();

            bool result = httpContext.User.IsInRole("Superadmin");
            // Allow all authenticated users to see the Dashboard (potentially dangerous).
            return result;
        }
    }
}
