using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;

namespace Utilities
{
    public class SendMail
    {
        Logger log;

        public SendMail(string _strConLogger)
        {

            log = new Logger(_strConLogger);
        }

        public void SendSMTP(string strFromAddress, string strFromDisplayName, string strTo, string strSubject, string strBody, string strCC, string strAttachments)
        {
            MailMessage insMail = new MailMessage(new MailAddress(strFromAddress, strFromDisplayName), new MailAddress(strTo));
            {
                insMail.Subject = strSubject;
                insMail.Body = strBody;
                insMail.IsBodyHtml = true;
                if (!string.IsNullOrEmpty(strCC))
                {
                    insMail.CC.Add(new MailAddress(strCC));
                }
                insMail.Bcc.Add(new MailAddress("mohamed.elmelegy@tedata.net"));
                insMail.Bcc.Add(new MailAddress("Ahmed.abdelmongy@tedata.net"));
                if (!strAttachments.Equals(string.Empty))
                {
                    string[] strAttach = strAttachments.Split(';');
                    foreach (string strFile in strAttach)
                    {
                        insMail.Attachments.Add(new Attachment(strFile.Trim()));
                    }
                }
            }

            SmtpClient smtp = new SmtpClient();
            smtp.Host = "212.103.160.61";
            smtp.Port = 25;
            smtp.Credentials = new System.Net.NetworkCredential("ahmed.abdelmongy","123456789");
                               
            try
            {
                smtp.Send(insMail);
                log.LogIt("Mail Of List of Newly Closed Exchanges has been sent", enumLOGType.INFORMATION.ToString());
            }
            catch (Exception ex)
            {
                log.LogIt(ex.ToString(), enumLOGType.ERROR.ToString());
            }
        }
    }
}
