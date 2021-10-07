using System;
using System.Diagnostics;
using System.Net.Mail;
using System.Threading;

namespace GRunAs
{
    class Program
    {
        static string emailUser = "";
        static string emailPass = "";
        static string toEmail = "";
        static string runUser = "";
        static string runPass = "";
        static string cmd = "C:\\Windows\\regedit.exe";

        static void Main(string[] args)
        {                        
            var res = RunAs.StartProcess(new RunAs.StartProcessInfo(runUser, runPass),cmd);
            SendEmail("starting program", "attempting to start " + cmd);
            if (!res.isStarted)
            {
                Console.WriteLine("failed to start process");
                return;
            }
            try
            {
                Process proc = res.getProcess();
                proc.EnableRaisingEvents = true;
                proc.Exited += new EventHandler((e, e1) =>
                {
                    Console.WriteLine("done");
                });
            } catch (Exception err)
            {
                Console.WriteLine(err);
            }
        }

        static void SendEmail(string subject, string body)
        {            
            SmtpClient smtpClient = new SmtpClient("smtp.live.com");
            var mail = new MailMessage();
            mail.From = new MailAddress(emailUser);
            mail.To.Add(toEmail);
            mail.Subject = subject;
            mail.IsBodyHtml = true;
            string htmlBody;
            htmlBody = body;
            mail.Body = htmlBody;
            smtpClient.Port = 587;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new System.Net.NetworkCredential(emailUser, emailPass);
            smtpClient.EnableSsl = true;
            smtpClient.SendCompleted += (sender, completedEvent) =>
            {
                if (completedEvent.Error!= null)
                {
                    Console.WriteLine("Error smtp ");
                    Console.WriteLine(completedEvent.Error);
                }
                if (completedEvent.Cancelled) Console.WriteLine("Cancelled");
                Console.WriteLine("smtp done");
            };

            smtpClient.Send(mail);
        }
        
    }
}
