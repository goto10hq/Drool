using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Hosting;
using Drool.Configurations;
using Drool.Tokens;
using Drool.Tools;
using HtmlAgilityPack;
using Microsoft.Azure;
using Sushi;

namespace Drool
{
    public class Mailer
    {
        private IConfiguration Configuration { get; set; }
        private static readonly Lazy<string> _smtpServer = new Lazy<string>(() => CloudConfigurationManager.GetSetting("Drool.SmtpServer"));
        private static readonly Lazy<int> _smtpPort = new Lazy<int>(() => CloudConfigurationManager.GetSetting("Drool.SmtpPort").ToInt32() ?? 25);
        private static readonly Lazy<string> _smtpLogin = new Lazy<string>(() => CloudConfigurationManager.GetSetting("Drool.SmtpLogin"));
        private static readonly Lazy<string> _smtpPassword = new Lazy<string>(() => CloudConfigurationManager.GetSetting("Drool.SmtpPassword"));                
        private static readonly Lazy<SmtpClient> _smtp = new Lazy<SmtpClient>(() => new SmtpClient(_smtpServer.Value, _smtpPort.Value) { DeliveryMethod = SmtpDeliveryMethod.Network });
        private readonly EmailTemplate _template;      

        /// <summary>
        /// Ctor.
        /// </summary>        
        public Mailer(string fullPath, IConfiguration configuration = null)
        {            
            if (fullPath == null)
                throw new ArgumentNullException(nameof(fullPath));

            Configuration = configuration;

            if (!string.IsNullOrEmpty(_smtpLogin.Value) &&
                !string.IsNullOrEmpty(_smtpPassword.Value))
                _smtp.Value.Credentials = new NetworkCredential(_smtpLogin.Value, _smtpPassword.Value);

            if (HostingEnvironment.IsHosted)
                fullPath = HostingEnvironment.MapPath(fullPath);

            if (fullPath == null)                                    
                throw new Exception("Email template not found.");                

            string template;

            using (var sr = new StreamReader(fullPath, Encoding.UTF8))
            {
                template = sr.ReadToEnd();
            }

            template = template.Replace("charset=utf-8", "charset=iso-8859-2");

            var html = new HtmlDocument();
            html.LoadHtml(template);

            var allImages = new List<EmailImage>();

            var images = html.DocumentNode.SelectNodes("//img");

            if (images != null)
            {
                foreach (var i in images)
                {
                    var src = i.Attributes["src"].Value;

                    // don't add duplicates
                    if (allImages.Any(ix => ix.OriginalNameWithPath.Equals(src, StringComparison.OrdinalIgnoreCase)))
                        continue;

                    var c = allImages.Count + 1;

                    var it = new EmailImage(Path.Combine(Path.GetDirectoryName(fullPath), src), src, src.Substring(src.LastIndexOf("/", StringComparison.Ordinal) + 1), $"{c}@DROOL");

                    allImages.Add(it);
                }

                template = allImages.Aggregate(template, (current, it) => current.Replace(it.OriginalNameWithPath, $"cid:{it.ImageNameInHtml}"));
            }

            _template = new EmailTemplate(template, allImages);
        }

        /// <summary>
        /// Send email.
        /// </summary>
        public void Send(MailAddress from, MailAddressCollection tos, string subject, Dictionary<string, object> replacements = null, MailAddressCollection replyTos = null, MailAddressCollection ccs = null, IEnumerable<Attachment> attachments = null)
        {
            AsyncTools.RunSync(() => SendHelperAsync(from, tos, subject, replacements, replyTos, ccs, attachments));
        }

        /// <summary>
        /// Send email.
        /// </summary>
        public void Send(string from, string to, string subject, Dictionary<string, object> replacements = null)
        {
            AsyncTools.RunSync(() => SendHelperAsync(new MailAddress(from), new MailAddressCollection { new MailAddress(to) }, subject, replacements));
        }

        /// <summary>
        /// Send email.
        /// </summary>
        public async Task SendAsync(MailAddress from, MailAddressCollection tos, string subject, Dictionary<string, object> replacements = null, MailAddressCollection replyTos = null, MailAddressCollection ccs = null, IEnumerable<Attachment> attachments = null)
        {
            await SendHelperAsync(from, tos, subject, replacements, replyTos, ccs, attachments);
        }
        
        /// <summary>
        /// Send email.
        /// </summary>
        public async Task SendAsync(string from, string to, string subject, Dictionary<string, object> replacements = null)
        {
            await SendHelperAsync(new MailAddress(from), new MailAddressCollection { new MailAddress(to) }, subject, replacements);
        }

        private async Task SendHelperAsync(MailAddress from, MailAddressCollection tos, string subject, Dictionary<string, object> replacements = null, MailAddressCollection replyTos = null, MailAddressCollection ccs = null, IEnumerable<Attachment> attachments = null)
        {
            if (@from == null)
                throw new ArgumentNullException(nameof(@from));

            if (tos == null)
                throw new ArgumentNullException(nameof(tos));

            if (subject == null)
                throw new ArgumentNullException(nameof(subject));

            if (!Regex.IsMatch(@from.Address, RegexPatterns.Email))
                throw new Exception($"From address '{@from.Address} is not valid e-mail.");

            var content = _template.Content;                

            var mail = new MailMessage
                       {
                           From = @from,
                           Subject = subject.ToIsoString(),
                           SubjectEncoding = Encoding.GetEncoding("ISO-8859-2"),                                                            
                       };

            foreach (var to in tos)
            {
                if (!Regex.IsMatch(to.Address, RegexPatterns.Email))
                    throw new Exception($"To address '{to.Address} is not valid e-mail.");

                mail.To.Add(to);
            }

            if (replyTos != null &&
                replyTos.Any())
            {
                foreach (var rto in replyTos)
                {
                    if (!Regex.IsMatch(rto.Address, RegexPatterns.Email))
                        throw new Exception($"Reply to address '{rto.Address} is not valid e-mail.");

                    mail.ReplyToList.Add(rto);
                }
            }

            if (ccs != null &&
                ccs.Any())
            {
                foreach (var cc in ccs)
                {
                    if (!Regex.IsMatch(cc.Address, RegexPatterns.Email))
                        throw new Exception($"CC address '{cc.Address} is not valid e-mail.");

                    mail.CC.Add(cc);
                }
            }

            if (replacements != null &&
                replacements.Count > 0)
            {
                content = replacements.Aggregate(content, (current, r) => current.Replace("{" + r.Key + "}", r.Value.ToString()));
            }

            var htmlView = AlternateView.CreateAlternateViewFromString(content.ToIsoString(), Encoding.GetEncoding("ISO-8859-2"), "text/html");
            htmlView.TransferEncoding = TransferEncoding.QuotedPrintable;

            if (_template.Images != null &&
                _template.Images.Any())
            {
                foreach (var it in _template.Images)
                {
                    var lr = new LinkedResource(it.OriginalNameWithFullPath, it.OriginalName.ToEncodingType()) { ContentId = it.ImageNameInHtml };
                    htmlView.LinkedResources.Add(lr);                    
                }
            }

            mail.AlternateViews.Add(htmlView);
            
            if (attachments != null)
            {
                var attachmentList = attachments as IList<Attachment> ?? attachments.ToList();

                foreach (var attachment in attachmentList)
                {
                    mail.Attachments.Add(attachment);
                }
            }

            if (Configuration != null)
            {
                mail.Headers.Add(Configuration.HeaderValues);
            } 
                
            await _smtp.Value.SendMailAsync(mail);
        }
    }
}
