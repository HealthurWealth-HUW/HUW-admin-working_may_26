using System;
using System.Linq;
using System.Net.Mail;
using System.Configuration;
using System.IO;
using System.Net;
using System.Web.Configuration;
using System.Threading.Tasks;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Collections.Generic;

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
            //From = "sudheerg38@gmail.com";

            //To = "info@healthurwealth.com";
            var msg = new System.Net.Mail.MailMessage();

            if (Body != null) msg.Body = Body;
            if (Cc != null)
            {
                msg.CC.Add(Cc);
                msg.Bcc.Add(WebConfigurationManager.AppSettings["AdminEmail"].ToString());
                //msg.Bcc.Add(WebConfigurationManager.AppSettings["AdminBCCEmail"].ToString());
                msg.Bcc.Add(WebConfigurationManager.AppSettings["ordersbccemail"].ToString());
                //msg.Bcc.Add(WebConfigurationManager.AppSettings["ravibccemail"].ToString());
            }
            else
            {
                msg.Bcc.Add(WebConfigurationManager.AppSettings["AdminEmail"].ToString());
                //msg.Bcc.Add(WebConfigurationManager.AppSettings["AdminBCCEmail"].ToString());
                msg.Bcc.Add(WebConfigurationManager.AppSettings["ordersbccemail"].ToString());
                //msg.Bcc.Add(WebConfigurationManager.AppSettings["ravibccemail"].ToString());
            }

            if (To != null)
            {
                msg.To.Add(To);
            }
            else
            {
                msg.To.Add(WebConfigurationManager.AppSettings["AdminEmail"].ToString());
                if (msg.Bcc != null)
                {
                    msg.Bcc.Remove(msg.Bcc[0]);
                }
            }
            if (From != null) msg.From = (new MailAddress(From));

            if (Bcc != null) msg.Bcc.Add(Bcc);
            if (Subject != null) msg.Subject = Subject;

            msg.IsBodyHtml = IsBodyHtml;
            //set attachments here

            if (Attachments != null)
            {
                var delim = new[] { ',' };

                foreach (var data in Attachments.Split(delim).Select(sattach => new System.Net.Mail.Attachment(GetNewFileNamePath() + @"\" + sattach)))
                {
                    msg.Attachments.Add(data);
                }
            }
            var client = new SmtpClient(MailServer)
            {
                UseDefaultCredentials = false,
                //Credentials = new System.Net.NetworkCredential("orders@healthurwealth.com", "Sonal@123"),
                //Host = "relay-hosting.secureserver.net",
                //Port = 25,
                Credentials = new System.Net.NetworkCredential("HealthUrWealth", "Sonal@123"),
                Host = "smtp.sendgrid.net",
                Port = 2525,
                EnableSsl = false
            };

            //client.Send(msg);
            //_ = Executesenmail(msg);


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