using System;

namespace ApiColomiersVolley.BLL.Core.Tools.Exceptions
{
    public class MailTemplateNotFoundException : Exception
    {
        public MailTemplateNotFoundException(string path, string code) : base($"Can't find template : {code} at path : {path}") { }
    }
}
