using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Bicks.Services
{
    public class FileTemplateService : IFileTemplateService
    {
        private readonly IConfiguration configuration;
        public FileTemplateService(IConfiguration IConfig)
        {
            configuration = IConfig;
        }

        public string getExampleEmailBody(string FirstName, string quoteTime, string problem, string totalCost, string solution, string refnumb, string url)
        {
            StreamReader str = new StreamReader(Directory.GetCurrentDirectory() + "\\wwwroot\\EmailTemplates\\ExampleEmailTemplate.html");
            string mailText = str.ReadToEnd();
            str.Close();

            mailText = mailText.Replace("[name]", FirstName);
            mailText = mailText.Replace("[quotedtime]", quoteTime);
            mailText = mailText.Replace("[problem]", problem);
            mailText = mailText.Replace("[price]", $"£{totalCost}");
            mailText = mailText.Replace("fix", solution);
            mailText = mailText.Replace("[referencenumber]", refnumb);
            mailText = mailText.Replace("[ResetLink]", url);

            return mailText;
        }
    }
}
