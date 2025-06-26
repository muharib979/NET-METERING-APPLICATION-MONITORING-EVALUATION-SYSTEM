using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Common.Mailing
{
    public interface IMailService
    {
       public Task<bool> SendEmail(MailRequest request);
    }
}
