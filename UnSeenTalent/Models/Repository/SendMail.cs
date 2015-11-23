using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Text;
using System.Net.Mail;
using System.IO.Compression;
using System.IO;

namespace UnseentalentsApp.Models.Repository
{
    public class SendMail
    {


        public string Send(string emailto, string copyTo, string subject, string body, string pdfFileName, List<string> bcc)
        {
            string retVal = "";
            try
            {

                MailMessage message;
                message = new MailMessage();
                if (bcc != null && bcc.Count() > 1)
                {
                    for (int i = 1; i < bcc.Count(); i++)
                    {
                        message.Bcc.Add(bcc[i]);
                    }
                }
                message.To.Add(emailto);
                message.From = new MailAddress("support@unseentalents.com", "JustDelivr");
                message.Subject = subject;
                //message.Body = "Hi, <br/> This mail is generated from ShowingPad. <br/><br/> Thanks";
                message.Body = body;
                message.Priority = MailPriority.Normal;
                SmtpClient client = new SmtpClient();
                message.IsBodyHtml = true;


                if (!string.IsNullOrEmpty(pdfFileName))
                {
                    Attachment attachFile2 = new Attachment(pdfFileName);
                    message.Attachments.Add(attachFile2);
                }
                if (!string.IsNullOrEmpty(copyTo))
                {
                    message.CC.Add(copyTo);
                }
                client.Send(message);
                retVal = "Email sent successfully";
            }
            catch (Exception ex)
            {
                retVal = ex.Message;// +"\n\n" + ex.StackTrace;
            }
            return retVal;
        }

        public string SendEmail(string emailto, string from, string subject, string body,string attachement)
        {
            string retVal = "";
            try
            {
                MailMessage message;
                message = new MailMessage();
                message.To.Add(emailto);
                message.From = new MailAddress(from);
                if(!string.IsNullOrEmpty(attachement))
                {
                    System.Net.Mail.Attachment attachment; 
                    attachment = new System.Net.Mail.Attachment(attachement);
                    message.Attachments.Add(attachment);
                }
               
                message.Subject = subject;
                //message.Body = "Hi, <br/> This mail is generated from ShowingPad. <br/><br/> Thanks";
                message.Body = body;
                message.Priority = MailPriority.Normal;
                SmtpClient client = new SmtpClient();
                message.IsBodyHtml = true;
                client.Send(message);
                retVal = "Email sent successfully";
            }
            catch (Exception ex)
            {
                retVal = ex.Message;// +"\n\n" + ex.StackTrace;
            }
            return retVal;
        }
    }
}