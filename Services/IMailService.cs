using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MimeKit;
using Bicks.Models;
using Bicks.Library.Mail;

namespace Bicks.Services
{
    public interface IMailService
    {
        void SendEmailNow(Mail mail);
        void SendEmailDelayMinutes(Mail mail, int delay);
        void SendEmailDelayHours(Mail mail, int delay);
        void SendEmailDelayDays(Mail mail, int delay);
        void SendEmailAtDateTime(Mail mail, DateTime dateTime);
    }
}

