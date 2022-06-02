using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bicks.Services
{
    public interface IFileTemplateService
    {
        string getExampleEmailBody(string FirstName, string quoteTime, string problem, string totalCost, string solution, string refnumb, string url);
    }
}
