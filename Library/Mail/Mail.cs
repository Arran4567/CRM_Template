using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.FileProviders;

namespace Bicks.Library.Mail
{
    
    public class Mail
    {
        public Mail() { }

        public Mail(string ToEmail, string Subject, string Body, List<IFileInfo> Attachments)
        {
            this.ToEmail = ToEmail;
            this.Subject = Subject;
            this.Body = Body;
            this.Attachments = Attachments;
        }

        public string ToEmail { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public List<IFileInfo> Attachments { get; set; }
    }
}
