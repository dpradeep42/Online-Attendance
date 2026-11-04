using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace OnlineAttendance.Models
{
    public partial class zclass
    {
        public List<zclass> zclasses { get; set; }
    }
    public partial class faculty
    {
        public List<faculty> faculties { get; set; }
        public String type { get; set; }
        public List<zclass> zclasses { get; set; }
    }
    public partial class student
    {
        public List<student> students { get; set; }
        public String type { get; set; }
        public List<zclass> zclasses { get; set; }
    }
    public class GMailer
    {
        public static string GmailUsername { get; set; }
        public static string GmailPassword { get; set; }
        public static string GmailHost { get; set; }
        public static int GmailPort { get; set; }
        public static bool GmailSSL { get; set; }

        public string ToEmail { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public bool IsHtml { get; set; }

        static GMailer()
        {
            LoadSettings(ConfigurationManager.AppSettings, Environment.GetEnvironmentVariable);
        }

        // Environment variables override AppSettings.secrets.config / Web.config.
        // Host, port and SSL keep the previous Gmail defaults when unset.
        public static void LoadSettings(NameValueCollection appSettings, Func<string, string> getEnvironmentVariable)
        {
            GmailUsername = ConfigValue(appSettings, getEnvironmentVariable, "SmtpUser");
            GmailPassword = ConfigValue(appSettings, getEnvironmentVariable, "SmtpPassword");
            GmailHost = ConfigValue(appSettings, getEnvironmentVariable, "SmtpHost") ?? "smtp.gmail.com";

            string portText = ConfigValue(appSettings, getEnvironmentVariable, "SmtpPort");
            if (string.IsNullOrEmpty(portText))
            {
                GmailPort = 587;
            }
            else
            {
                int port;
                if (!int.TryParse(portText, NumberStyles.Integer, CultureInfo.InvariantCulture, out port) || port < 1 || port > 65535)
                    throw new InvalidOperationException("SmtpPort must be an integer from 1 to 65535.");
                GmailPort = port;
            }

            string sslText = ConfigValue(appSettings, getEnvironmentVariable, "SmtpEnableSsl");
            if (string.IsNullOrEmpty(sslText))
            {
                GmailSSL = true;
            }
            else
            {
                bool ssl;
                if (!bool.TryParse(sslText, out ssl))
                    throw new InvalidOperationException("SmtpEnableSsl must be true or false.");
                GmailSSL = ssl;
            }
        }

        public void Send()
        {
            if (string.IsNullOrWhiteSpace(GmailUsername) || string.IsNullOrWhiteSpace(GmailPassword))
                throw new InvalidOperationException("SMTP credentials are not configured. Set SmtpUser and SmtpPassword in AppSettings.secrets.config or as environment variables.");

            SmtpClient smtp = new SmtpClient();
            smtp.Host = GmailHost;
            smtp.Port = GmailPort;
            smtp.EnableSsl = GmailSSL;
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new NetworkCredential(GmailUsername, GmailPassword);

            using (var message = new MailMessage(GmailUsername, ToEmail))
            {
                message.Subject = Subject;
                message.Body = Body;
                message.IsBodyHtml = IsHtml;
                smtp.Send(message);
            }
        }

        private static string ConfigValue(NameValueCollection appSettings, Func<string, string> getEnvironmentVariable, string key)
        {
            if (getEnvironmentVariable != null)
            {
                string fromEnvironment = getEnvironmentVariable(key);
                if (!string.IsNullOrWhiteSpace(fromEnvironment))
                    return fromEnvironment.Trim();

                string alias = EnvironmentAlias(key);
                if (alias != null)
                {
                    fromEnvironment = getEnvironmentVariable(alias);
                    if (!string.IsNullOrWhiteSpace(fromEnvironment))
                        return fromEnvironment.Trim();
                }
            }

            if (appSettings != null)
            {
                string fromFile = appSettings[key];
                if (!string.IsNullOrWhiteSpace(fromFile))
                    return fromFile.Trim();
            }

            return null;
        }

        private static string EnvironmentAlias(string key)
        {
            switch (key)
            {
                case "SmtpUser": return "SMTP_USER";
                case "SmtpPassword": return "SMTP_PASSWORD";
                case "SmtpHost": return "SMTP_HOST";
                case "SmtpPort": return "SMTP_PORT";
                case "SmtpEnableSsl": return "SMTP_ENABLE_SSL";
                default: return null;
            }
        }
    }
}