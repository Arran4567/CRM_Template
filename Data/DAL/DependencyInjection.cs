using Microsoft.Extensions.DependencyInjection;
using Bicks.Areas.Invoicing.Data.DAL;
using Bicks.Areas.ProductManagement.Data.DAL;
using Bicks.Areas.Sales.Data.DAL;
using Bicks.Areas.ClientManagement.Data.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bicks.Data.DAL
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddWorkUnits(this IServiceCollection services)
        {
            //services.AddTransient<ExampleWorkUnit>();
            services.AddTransient<InvoicingWorkUnit>();
            services.AddTransient<ProductManagementWorkUnit>();
            services.AddTransient<SalesWorkUnit>();
            services.AddTransient<ClientManagementWorkUnit>();
            return services;
        }
    }
}
