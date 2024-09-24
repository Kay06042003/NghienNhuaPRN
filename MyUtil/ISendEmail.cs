using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nghien_Nhua.MyUtil
{
    public interface ISendEmail
    {
        Task SendEmailAsync(string email, string subject, int code, string name);
    }
}