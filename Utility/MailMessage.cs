using BAL;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;
using System.Web.Mail;

namespace Utility
{

    public class MailMessage
    {
        private string _mailServer;

        #region mail class accessors

        public string Cc { get; set; }

        public string Bcc { get; set; }

        public string Body { get; set; }

        public bool IsBodyHtml { get; set; }


        public string Subject { get; set; }

        public string MailServer
        {
            get { return _mailServer ?? (_mailServer = "smtp.gmail.com"); }
            set { _mailServer = value; }
        }

        public string From { get; set; }

        public string To { get; set; }

        public string MailFormat { get; set; }

        public string Attachments { get; set; }

        public Exception MailException { get; set; }

        #endregion


        public static string SendSms(string Url, string User, string password, decimal Mobile_Number, string Message, string MType,string templateid)
        {
            try
            {
                string stringpost = Url + "?username=" + User + "&apikey=" + password + "&senderid=SNLHUW&templateid=" + templateid + "&mobile=" + Mobile_Number.ToString() + "&message=" + Message;

                //string stringpost = Url + "?username=" + User + "&password=" + password + "&to=" + Mobile_Number.ToString() + "&from=SNLHUW&message=" + Message;
                WebRequest request = HttpWebRequest.Create(stringpost);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream s = (Stream)response.GetResponseStream();
                StreamReader readStream = new StreamReader(s);
                string dataString = readStream.ReadToEnd();
                response.Close();
                s.Close();
                readStream.Close();

                return "";
            }
            catch
            {
                return "";
            }
        }


        public bool SendMail()
        {
            From = Shared.GetFromMailId(Shared.fromMailId.info);

            // Build BCC list — AdminEmail and ordersbccemail always get a copy
            var bccList = new List<string>
            {
                WebConfigurationManager.AppSettings["adminemail"].ToString(),
                WebConfigurationManager.AppSettings["ordersbccemail"].ToString()
            };

            string toEmail;
            if (To != null)
            {
                toEmail = To;
            }
            else
            {
                // When no To is specified, send to AdminEmail and remove it from BCC to avoid duplicate
                toEmail = WebConfigurationManager.AppSettings["adminemail"].ToString();
                bccList.RemoveAt(0);
            }

            if (Bcc != null)
                bccList.Add(Bcc);

            var attachmentList = new List<object>();
            if (Attachments != null)
            {
                var delim = new[] { ',' };
                foreach (var sattach in Attachments.Split(delim))
                {
                    var filePath = GetNewFileNamePath() + @"\" + sattach;
                    if (File.Exists(filePath))
                    {
                        attachmentList.Add(new
                        {
                            content = Convert.ToBase64String(File.ReadAllBytes(filePath)),
                            name = sattach
                        });
                    }
                }
            }

            var bodyObj = new
            {
                sender = new { email = From, name = "Healthurwealth" },
                to = new[] { new { email = toEmail } },
                cc = Cc != null ? new[] { new { email = Cc } } : null,
                subject = Subject,
                htmlContent = IsBodyHtml ? Body : (string)null,
                textContent = IsBodyHtml ? (string)null : Body,
                bcc = bccList.Select(b => new { email = b.Trim() }).ToArray(),
                attachment = attachmentList.Count > 0 ? attachmentList.ToArray() : (object[])null
            };

            var json = Newtonsoft.Json.JsonConvert.SerializeObject(bodyObj,
                new Newtonsoft.Json.JsonSerializerSettings { NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore });

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11;

            using (var httpClient = new HttpClient())
            {
                var apiKey = WebConfigurationManager.AppSettings["BrevoApiKey"];
                httpClient.DefaultRequestHeaders.Add("api-key", apiKey);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = httpClient.PostAsync("https://api.brevo.com/v3/smtp/email", content).GetAwaiter().GetResult();
                if (!response.IsSuccessStatusCode)
                {
                    var error = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                    throw new Exception("Brevo email failed: " + error);
                }
            }

            return true;
        }

        static async Task<bool> Executesenmail(System.Net.Mail.MailMessage msg)
        {
            var apiKeyAS = Environment.GetEnvironmentVariable("Healthurwealth");
            var client = new SendGridClient("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");
            var from = new EmailAddress("orders@healthurwealth.com", "Healthurwealth");
            var subject = msg.Subject;
            var tos = new List<EmailAddress>();
            var to = new EmailAddress(msg.To[0].ToString(), "");
            var Bcc = new EmailAddress("orders@healthurwealth.com", "");
            tos.Add(to);
            tos.Add(Bcc);
            var plainTextContent = "";
            var htmlContent = msg;
            var msgs = MailHelper.CreateSingleEmailToMultipleRecipients(from, tos, subject, plainTextContent, msg.Body);
            await client.SendEmailAsync(msgs);
            return true;
        }

        private string GetNewFileNamePath()
        {
#pragma warning disable 612,618
            {
                var filePath = ConfigurationSettings.AppSettings["MailFilePath"];
                // ReSharper disable NotResolvedInText
                if (filePath == null) throw new ArgumentNullException("filePath");
                // ReSharper restore NotResolvedInText
                return filePath;
            }
        }

        public bool DeleteSentFile()
        {
            if (Attachments != null)
            {
                var delim = new[] { ',' };
                foreach (var fi in from sattach in Attachments.Split(delim) where File.Exists(GetNewFileNamePath() + @"\" + sattach) select new FileInfo(GetNewFileNamePath() + @"\" + sattach))
                {
                    fi.Delete();
                }
            }
            return true;
        }
        
    }
}