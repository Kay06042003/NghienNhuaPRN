using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Nghien_Nhua.Models;

namespace Nghien_Nhua.Services
{
    public interface IVnPayServices
    {
        string CreateRequestUrl(HttpContext context, VnPayResqestModel vnPayment);
        VnPaymentResponseModel PaymentExcute(IQueryCollection collection);

    }
}