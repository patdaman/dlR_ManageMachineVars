// <summary>Implements the log 4 net helper class</summary>

using CommonUtils.DataStructures;
using CommonUtils.Reflection;
using log4net.Appender;
using log4net.Repository.Hierarchy;
///-------------------------------------------------------------------------------------------------
///-------------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;

using RSysFac = log4net.Appender.RemoteSyslogAppender.SyslogFacility;

namespace CommonUtils.Logging
{
    public class AppenderPropertyValue
    {
        public string AppenderName { get; set; }
        public string PropertyName { get; set; }
        public string ValueString { get; set; }
    }

    public class Log4NetHelper
    {
        public static IDictionary<string, System.Net.Mail.MailPriority> MailPriorityBiDictionary = new BiDictionary<string, System.Net.Mail.MailPriority>()
        {
            {"High", System.Net.Mail.MailPriority.High },
            {"Normal", System.Net.Mail.MailPriority.Normal },
            {"Low", System.Net.Mail.MailPriority.Low }
        };

        public static IDictionary<String, RSysFac> SyslogFacilityBiDictionary = new BiDictionary<string, RSysFac>()
        {
            {"Kernel", RSysFac.Kernel },
            {"User", RSysFac.User },
            {"Mail", RSysFac.Mail },
            {"Daemons", RSysFac.Daemons },
            {"Authorization", RSysFac.Authorization },
            {"Authorization2", RSysFac.Authorization2 },
            {"Syslog", RSysFac.Syslog },
            {"Printer", RSysFac.Printer },
            {"News", RSysFac.News },
            {"Uucp", RSysFac.Uucp },
            {"Clock", RSysFac.Clock },
            {"Clock2", RSysFac.Clock2 },
            {"Ftp", RSysFac.Ftp },
            {"Ntp", RSysFac.Ntp },
            {"Audit", RSysFac.Audit },
            {"Alert", RSysFac.Alert },
            {"Local0", RSysFac.Local0 },
            {"Local1", RSysFac.Local1 },
            {"Local2", RSysFac.Local2 },
            {"Local3", RSysFac.Local3 },
            {"Local4", RSysFac.Local4 },
            {"Local5", RSysFac.Local5 },
            {"Local6", RSysFac.Local6 },
            {"Local7", RSysFac.Local7 }
        };
        public static void SetAppenderProperties(List<AppenderPropertyValue> pvlist)
        {
            Hierarchy hier = log4net.LogManager.GetRepository() as Hierarchy;

            List<String> dApp = pvlist.Select(x => x.AppenderName).Distinct().ToList();

            foreach (string apstr in dApp)
            {
                var applist = hier.GetAppenders();
                var appender = hier.GetAppenders().
                    Where(app => app.Name.Equals(apstr, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();

                if (appender != null)
                {
                    Dictionary<String, String> aProp = pvlist.Where(x => x.AppenderName == apstr).ToDictionary(v => v.PropertyName, v => v.ValueString);
                    foreach (KeyValuePair<String, String> kvp in aProp)
                    {
                        PropertyInfo api = appender.GetType().GetProperty(kvp.Key);
                        if (api == null)
                            throw new Exception("Property " + kvp.Key + " not found in " + appender.GetType().ToString());
                            //api.SetValue(appender, Convert.ChangeType(kvp.Value, api.PropertyType));

                            if (api.PropertyType == typeof(System.Net.IPAddress))
                            {
                                IPAddress[] adds = Dns.GetHostAddresses(kvp.Value);
                                if (adds.Length > 0)
                                    //System.Net.IPAddress ipa = System.Net.IPAddress.Parse(kvp.Value);
                                    //appender.SetPropertyValueByName(kvp.Key, ipa);
                                    api.SetValue(appender, adds[0]);
                            }
                            else if (api.PropertyType == typeof(System.Net.Mail.MailAddress))
                            {
                                System.Net.Mail.MailAddress ma = new System.Net.Mail.MailAddress(kvp.Value);
                                appender.SetPropertyValueByName(kvp.Key, ma);
                            }
                            else if (api.PropertyType == typeof(System.Net.Mail.MailPriority))
                            {
                                System.Net.Mail.MailPriority mp = MailPriorityBiDictionary[kvp.Value];
                                appender.SetPropertyValueByName(kvp.Key, mp);
                            }
                            else if (api.PropertyType == typeof(log4net.Appender.RemoteSyslogAppender.SyslogFacility))
                            {
                                appender.SetPropertyValueByName(kvp.Key, SyslogFacilityBiDictionary[kvp.Value]);
                            }
                            else if (api.PropertyType == typeof(log4net.Layout.PatternLayout))
                            {
                                appender.SetPropertyValueByName(kvp.Key, new log4net.Layout.PatternLayout(kvp.Value));
                            }
                            else
                            {
                                appender.SetPropertyValueByName(
                                    kvp.Key,
                                    Convert.ChangeType(kvp.Value, api.PropertyType)
                                    );
                            }
                    }
                    if (appender is UdpAppender)
                        (appender as UdpAppender).ActivateOptions();
                }
            }
        }

        private static IAppender getAppender(String appenderName)
        {
            Hierarchy hier = log4net.LogManager.GetRepository() as Hierarchy;
            return hier.GetAppenders().
                    Where(app => app.Name.Equals(appenderName, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
        }

        public static void CloseAppender(String appenderName)
        {
            var appender = getAppender(appenderName);
            if (appender != null)
                appender.Close();

        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Sets log file path. </summary>
        ///
        /// <remarks>   Pdelosreyes, 4/24/2017. </remarks>
        ///
        /// <param name="logFilePath">          Full pathname of the log file. </param>
        /// <param name="logConfigFilePath">    Full pathname of the log configuration file. </param>
        ///-------------------------------------------------------------------------------------------------
        public static void SetLogFilePath(string logFilePath, string logConfigFilePath)
        {

            Path.GetFullPath(logFilePath); // check if path is valid. Throw exception otherwise
            log4net.GlobalContext.Properties["LogFileName"] = Path.GetFullPath(logFilePath);
            log4net.Config.XmlConfigurator.ConfigureAndWatch(new FileInfo(logConfigFilePath));
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Sets log email subject and priority. </summary>
        ///
        /// <remarks>   Pdelosreyes, 4/24/2017. </remarks>
        ///
        /// <param name="subject">      The subject. </param>
        /// <param name="priority">     (Optional) The priority. </param>
        /// <param name="smtpAppender"> (Optional) The SMTP appender. </param>
        ///-------------------------------------------------------------------------------------------------
        public static void SetLogEmailSubjectPriority(string subject, System.Net.Mail.MailPriority priority = System.Net.Mail.MailPriority.Normal, string smtpAppender = "SmtpAppender")
        {
            List<AppenderPropertyValue> apvlist = new List<AppenderPropertyValue>()
            {
                new AppenderPropertyValue {AppenderName=smtpAppender, PropertyName="Subject", ValueString=subject },
                new AppenderPropertyValue {AppenderName=smtpAppender, PropertyName="Priority", ValueString=priority.ToString() }
            };
            Log4NetHelper.SetAppenderProperties(apvlist);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Sets log email priority. </summary>
        ///
        /// <remarks>   Pdelosreyes, 4/24/2017. </remarks>
        ///
        /// <param name="priority">     (Optional) The priority. </param>
        /// <param name="smtpAppender"> (Optional) The SMTP appender. </param>
        ///-------------------------------------------------------------------------------------------------
        public static void SetLogEmailPriority(System.Net.Mail.MailPriority priority = System.Net.Mail.MailPriority.Normal, string smtpAppender = "SmtpAppender")
        {
            List<AppenderPropertyValue> apvlist = new List<AppenderPropertyValue>()
            {
                 new AppenderPropertyValue {AppenderName=smtpAppender, PropertyName="Priority", ValueString=priority.ToString() }
            };
            Log4NetHelper.SetAppenderProperties(apvlist);
        }
    }
}