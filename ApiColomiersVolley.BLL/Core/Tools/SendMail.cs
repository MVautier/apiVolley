using ApiColomiersVolley.BLL.Core.Tools.Exceptions;
using ApiColomiersVolley.BLL.Core.Tools.Interfaces;
using ApiColomiersVolley.BLL.Core.Tools.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace ApiColomiersVolley.BLL.Core.Tools
{
    public class SendMail : IServiceSendMail
    {
        private readonly IConfiguration _config;

        public SendMail(IConfiguration config)
        {
            _config = config;
        }

        public MailOrder GetMailOrderFromTemplate(string companyCode, string templateCode)
        {
            var path = _config.GetSection("Resources").GetValue<string>("Path") + companyCode;
            var filePath = path + "/mail-template.json";
            if (!File.Exists(filePath))
            {
                throw new MailTemplateNotFoundException(filePath, templateCode);
            }

            var companyTemplates = JsonConvert.DeserializeObject<List<MailTemplate>>(File.ReadAllText(filePath));
            var template = companyTemplates.FirstOrDefault(t => t.Code == templateCode);
            if (template == null)
            {
                throw new MailTemplateNotFoundException(filePath, templateCode);
            }

            template.Files?.ForEach(f => f.Filename = path + "/" + _config["MailSettings:DataTemplate"] + "/" + f.Filename);
            if (template.Files == null)
            {
                template.Files = new List<MailAttachementTemplate>();
            }

            return new MailOrder
            {
                Subject = template.Subject,
                HtmlContent = template.HTML,
                TextContent = template.Text,
                Files = template.Files,
            };
        }

        public async Task SendMailErreur(Exception ex, string message = "")
        {
            var mailConf = _config.GetSection("MailSettings");
            var from = new MailAddress(mailConf.GetValue<string>("EmailFrom"));
            var to = new List<string> { mailConf.GetValue<string>("EmailErreurTo") };
            var subject = "[" + mailConf.GetValue<string>("ModeTravail") + "] [BUG] " + mailConf.GetValue<string>("TypeRobot");
            var body = CreateErrorMessage(ex, message);
            await Send(from, to, null, body, subject);
        }

        public async Task SendMailUser(MailOrder content, string mailParameters = null, List<MailFile> files = null)
        {
            var parameters = PrepareConfig(mailParameters);
            var from = new MailAddress(parameters.mail_sender, parameters.name_sender, Encoding.UTF8);
            bool enableRealMail = _config.GetSection("FeatureActivation").GetValue<bool>("enableRealMail");
            if (enableRealMail)
            {
                await Send(from, content.ToMails != null && content.ToMails.Any() ? content.ToMails : new List<string> { content.ToMail }, null, content.HtmlContent, content.Subject, content.TextContent, content.Files, parameters, files);
            }
            else
            {
                var mailConf = _config.GetSection("MailSettings");
                from = new MailAddress(mailConf.GetValue<string>("FromMail"), mailConf.GetValue<string>("FromName"), Encoding.UTF8);
                await Send(from, mailConf.GetValue<string>("EmailTo")?.Split(',').ToList() ?? new List<string>(), null, content.HtmlContent, content.Subject, content.TextContent, content.Files, parameters, files);
            }
        }

        private async Task Send(MailAddress from, List<string> to, List<string> hiddenTo, string htmlBody, string subject, string textBody = "", List<MailAttachementTemplate> attachements = null, MailConfig parameters = null, List<MailFile> files = null)
        {
            var mailConf = _config.GetSection("MailSettings");
            var message = new MailMessage
            {
                From = from
            };

            foreach (var t in to)
            {
                message.To.Add(t);
            }

            if (hiddenTo != null)
            {
                foreach (var t in hiddenTo)
                {
                    message.Bcc.Add(t);
                }
            }

            message.Subject = subject + new string(new char[] { '\u2000', '\u2000' });
            message.SubjectEncoding = Encoding.UTF8;
            var htmlView = AlternateView.CreateAlternateViewFromString(htmlBody, Encoding.UTF8, "text/html");
            if (attachements != null)
            {
                foreach (var r in PrepareResources(attachements.Where(a => a.Type == "include").ToList()))
                {
                    htmlView.LinkedResources.Add(r);
                }

                foreach (var r in PrepareAttachments(attachements.Where(a => a.Type == "joint").ToList()))
                {
                    message.Attachments.Add(r);
                }
            }

            if (files != null)
            {
                files.ForEach(f => message.Attachments.Add(new Attachment(new MemoryStream(f.Content), f.Name)));
            }

            message.AlternateViews.Add(htmlView);
            if (!string.IsNullOrEmpty(textBody))
            {
                var textView = AlternateView.CreateAlternateViewFromString(textBody, Encoding.UTF8, "text/plain");
                message.AlternateViews.Add(textView);
            }


            SmtpClient smtpClient;
            if (parameters == null)
            {
                smtpClient = new SmtpClient(mailConf.GetValue<string>("SmtpHost"));
            }
            else
            {
                smtpClient = new SmtpClient(parameters.srv_mail);
                if (parameters.port_mail.HasValue)
                {
                    smtpClient.Port = parameters.port_mail.Value;
                    smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                    if (parameters.port_mail != 25)
                    {
                        smtpClient.EnableSsl = true;
                        smtpClient.TargetName = "STARTTLS/" + parameters.srv_mail;
                    }
                }

                if (!string.IsNullOrEmpty(parameters.user_mail) && !string.IsNullOrEmpty(parameters.pass_mail))
                {
                    smtpClient.Credentials = new NetworkCredential(parameters.user_mail, parameters.pass_mail);
                }
            }

            await smtpClient.SendMailAsync(message);
        }

        private static List<LinkedResource> PrepareResources(List<MailAttachementTemplate> attachements)
        {
            return attachements.Select(a => new LinkedResource(a.Filename)
            {
                ContentId = a.Name,
                ContentType = new ContentType(a.ContentType),
                TransferEncoding = ConvertEncoding(a.TransferEncoding)
            }).ToList();
        }

        private static List<Attachment> PrepareAttachments(List<MailAttachementTemplate> attachments)
        {
            return attachments.Select(a => new Attachment(a.Filename)
            {
                Name = a.Name,
                ContentType = new ContentType(a.ContentType),
                TransferEncoding = ConvertEncoding(a.TransferEncoding)
            }).ToList();
        }

        private static TransferEncoding ConvertEncoding(string encoding)
        {
            if (encoding.ToLower() == "eightbit")
            {
                return TransferEncoding.EightBit;
            }

            if (encoding.ToLower() == "sevenbit")
            {
                return TransferEncoding.SevenBit;
            }

            if (encoding.ToLower() == "quotedprintable")
            {
                return TransferEncoding.QuotedPrintable;
            }

            return TransferEncoding.Base64;
        }

        private string CreateErrorMessage(Exception ex, string message = "")
        {
            var body = "<html><body>";
            if (!string.IsNullOrWhiteSpace(message))
            {
                body += "<h2>Erreur</h2><p>" + message + "</p>";
            }

            body += RecurciveInnerException(ex) + "</body></html>";
            return body;
        }

        private string RecurciveInnerException(Exception ex)
        {
            var message = "<h2>Erreur</h2><p>{0}</p><h2>Méthode</h2><p>{1}</p><h2>Pile</h2><p>{2}</p>";
            if (ex == null)
            {
                return string.Empty;
            }

            return string.Format(message, ex.Message, ex.TargetSite, (ex.StackTrace ?? string.Empty).Replace(Environment.NewLine, "<br/>")) + "<br/>" + RecurciveInnerException(ex.InnerException);
        }

        private MailConfig PrepareConfig(string mailParameters)
        {
            if (!_config.GetSection("FeatureActivation").GetChildren().Any(c => c.Key == "enableRealMail")
                || _config.GetSection("FeatureActivation").GetValue<bool>("enableRealMail"))
            {
                return JsonConvert.DeserializeObject<MailConfig>(mailParameters);
            }

            var mailConf = _config.GetSection("MailSettings");
            return new MailConfig
            {
                srv_mail = mailConf.GetValue<string>("SmtpHost"),
                mail_sender = mailConf.GetValue<string>("FromMail"),
                name_sender = mailConf.GetValue<string>("FromName")
            };
        }
    }
}
