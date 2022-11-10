using ApiColomiersVolley.BLL.Core.Tools.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiColomiersVolley.BLL.Core.Tools.Interfaces
{
    public interface IServiceSendMail
    {
        MailOrder GetMailOrderFromTemplate(string companyCode, string templateCode);
        Task SendMailUser(MailOrder content, string mailParameters = null, List<MailFile> files = null);
        Task SendMailErreur(Exception ex, string message = "");
    }
}
