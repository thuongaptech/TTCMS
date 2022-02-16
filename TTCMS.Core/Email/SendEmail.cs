using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;
using System.Threading;

namespace TTCMS.Core.Email
{
    public class SendEmail
    {
        static Thread SendMail_Thread;

        public static void StartThreadSendMail()
        {
            SendMail_Thread = new Thread(new ThreadStart(SendMailbyThread));
            SendMail_Thread.Start();
        }

        public class EmailQueue
        {
            public string Email { get; set; }
            public string Subject { get; set; }
            public string Body { get; set; }
            public bool is_finished { get; set; }

            public EmailQueue()
            {
                Email = "";
                Subject = "";
                Body = "";
                is_finished = false;
            }
        }

        public static bool SendMail(string emailto, string subject, string body, bool InsertToQueue, string email_account = "", string password_account = "", string host = "", int port = 0, bool enablessl = true, string displayname = "")
        {
            if (InsertToQueue)
            {
                EmailQueue emailQueue = new EmailQueue();
                emailQueue.Email = emailto;
                emailQueue.Subject = subject;
                emailQueue.Body = body;
                EmailQueueList.Add(emailQueue);
                return true;
            }
            else
            {
                SmtpClient SmtpServer = new SmtpClient();
                SmtpServer.Credentials = new System.Net.NetworkCredential(email_account, password_account);
                SmtpServer.Port = port;
                SmtpServer.Host = host;
                SmtpServer.EnableSsl = enablessl;
                MailMessage mail = new MailMessage();

                try
                {
                    mail.From = new MailAddress(email_account, displayname, System.Text.Encoding.UTF8);
                    mail.To.Add(emailto);
                    mail.Subject = subject;
                    mail.Body = body;
                    mail.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
                    mail.ReplyTo = new MailAddress(email_account);
                    mail.Priority = MailPriority.High;
                    mail.IsBodyHtml = true;
                    SmtpServer.Send(mail);
                    return true;
                }
                catch (Exception) { return false; }
            }
        }

        public static List<EmailQueue> EmailQueueList = new List<EmailQueue>();


        static void SendMailbyThread()
        {
            while (SendMail_Thread.IsAlive)
            {
                while (EmailQueueList.Count > 0)
                {
                    try
                    {
                        var item = EmailQueueList.Where(m => !m.is_finished).FirstOrDefault();
                        if (item == null)
                            break;

                        item.is_finished = true;
                        SendMail(item.Email, item.Subject, item.Body, false);
                        EmailQueueList.Remove(item);
                        Thread.Sleep(2500);
                    }
                    catch (Exception ex)
                    {

                    }
                }
                Thread.Sleep(2500);
            }

        }
    }
}
