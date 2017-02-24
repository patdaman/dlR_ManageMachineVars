using CommonUtils.AppConfiguration;
using NDesk.Options;
using System;
using System.Linq;
using System.Deployment.Application;

namespace ManageConfigVariables
{
    public class ManageConfigVariables
    {
        private bool IsInteractive = false;
        protected bool ShowHelp = false;
        public OptionSet Options { get; set; }
        public string AppName { get; set; }
        public string LogFilePath { get; set; }
        public string EmailAddress { get; set; }
        public string KeyType { get; set; }

        static void Main(string[] args)
        {
            var app = new ManageConfigVariables();
            app.Start(args);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Starts the given arguments. </summary>
        ///
        /// <remarks>   Pdelosreyes, 2/23/2017. </remarks>
        ///
        /// <param name="args"> The arguments. </param>
        ///-------------------------------------------------------------------------------------------------
        private void Start(string[] args)
        {
            try
            {
                //Perform configuration (e.g., setting up processors, database conn strings, etc.)
                var config = new AppConfiguration();
                config.AddProvider(new ConfigFileConfigProvider());

                config.AddProvider(new EnvironmentVariableConfigProvider(EnvironmentVariableTarget.User));
                config.AddProvider(new EnvironmentVariableConfigProvider(EnvironmentVariableTarget.Machine));
                config.AddProvider(new EnvironmentVariableConfigProvider(EnvironmentVariableTarget.Process));
                string configuration = config.GetValue("Configuration") as string;
                ConfigureObjects(config);

                //Populate AdminArgs from AppConfiguration first 
                var adminArgs = new AdminArgs();
                SetupFromAppConfig(config, adminArgs);

                //Populate AdminArgs from Command line args 
                SetupFromCommandArgs(args, adminArgs);
                SetConfigType(adminArgs);

                //Run!                
                //Logger.Info($"Configuration is {configuration}");
                Run(adminArgs);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception Thrown:");
                Console.WriteLine(ex);
                Console.WriteLine();
                //this.Logger.Error(ExamineException.GetInnerExceptionAndStackTrackMessage(ex));
            }
#if DEBUG
        IsInteractive = true;
#endif
        if (IsInteractive)
            {
                Console.WriteLine("ENTER to exit.");
                Console.ReadLine();
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Sets up from command arguments. </summary>
        ///
        /// <remarks>   Pdelosreyes, 2/23/2017. </remarks>
        ///
        /// <param name="args">         The arguments. </param>
        /// <param name="adminArgs">    The admin arguments. </param>
        ///-------------------------------------------------------------------------------------------------
        private void SetupFromCommandArgs(string[] args, AdminArgs adminArgs)
        {
            Options = new OptionSet()
            {
                { "h|?|help", "show usage", v => ShowHelp = true },
                { "i|interactive", "Switch that puts app in interactive mode.", v => { IsInteractive = true; }},
                { "l|log=", "log file path",   v => LogFilePath = v },
                { "e|email=", "email address for errors", v=> EmailAddress = v },
                { "a|action=", "Action to perform (required)", v => { adminArgs.Action = v.ToLower(); }},
                { "t|keytype=", "Values: connstring|appsetting|user|machine|session", v => {adminArgs.KeyType = v; }},
                { "k|key=", "Key name", v => {adminArgs.Key = v; }},
                { "v|value=", "Key value", v => {adminArgs.Value = v; }},
                //{ "name|n=", "User name (user, role, etc. dependingon action) to operate with.", v => { adminArgs.UserName = v; }},
                //{ "password|pass=", "The password", v => { adminArgs.Password = v; }},
                { "p|path=", "Path to XML config file.", v => { adminArgs.Path = v; }},
                { "m|machine=", "Machine Name to target.", v => { adminArgs.MachineName = v; }},
            };
            this.Initialize(typeof(ManageConfigVariables), "ManageConfigVariables"); 
            ParseArgs(args); //Parse arguments                                           
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Initializes this object. </summary>
        ///
        /// <remarks>   Pdelosreyes, 2/10/2017. </remarks>
        ///
        /// <param name="applicationType">      Type of the application. </param>
        /// <param name="appName">              Name of the application. </param>
        /// <param name="additionalOptions">    (Optional) Options for controlling the additional. </param>
        /// <param name="defaultEmailAddress">  (Optional) The default email address. </param>
        /// <param name="logConfigFileName">    (Optional) Filename of the log configuration file. </param>
        /// <param name="emailSubject">         (Optional) The email subject. </param>
        /// <param name="configFacility">       (Optional) The configuration facility. </param>
        ///-------------------------------------------------------------------------------------------------
        public void Initialize(Type applicationType,
                                string appName,
                                string defaultEmailAddress = null,
                                string logConfigFileName = null,
                                string emailSubject = null
                                , string configFacility = null
                                )
        {
            AppName = appName;
            Console.WriteLine(AppName + " v " + AssemblyVersionString);
            //InitializeLogging(applicationType, defaultEmailAddress, logConfigFileName, emailSubject, configFacility);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Parse arguments. </summary>
        ///
        /// <remarks>   Pdelosreyes, 2/8/2017. </remarks>
        ///
        /// <param name="args"> The arguments. </param>
        ///-------------------------------------------------------------------------------------------------
        public void ParseArgs(string[] args)
        {
            Options.Parse(args);
            if (!args.Any())
            {
                ShowHelp = true;
            }
            if (ShowHelp)
            {
                ShowUsage(Options);
                Environment.Exit(0);
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the assembly version string. </summary>
        ///
        /// <value> The assembly version string. </value>
        ///-------------------------------------------------------------------------------------------------
        public static string AssemblyVersionString
        {
            get
            {
                string message;
                try
                {
                    Version ver = ApplicationDeployment.CurrentDeployment.CurrentVersion;
                    message = ver.Major.ToString() + "." + ver.Minor.ToString() + "." + ver.Build.ToString() + "." + ver.Revision.ToString();
                }
                catch (Exception ex)
                {
                    //Console.WriteLine("AssemblyVersionString error: " + ex.Message);
                    try
                    {
                        message = System.Reflection.Assembly.GetEntryAssembly().GetName().Version.ToString();
                    }
                    catch (Exception exx)
                    {
                        message = "Unknown Version";
                    }
                }
                return message;
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Sets up the admin arguments with additional default values. </summary>
        ///
        /// <remarks>   Pdelosreyes, 2/8/2017. </remarks>
        ///
        /// <param name="config">       The configuration. </param>
        /// <param name="adminArgs">    The admin arguments. </param>
        ///-------------------------------------------------------------------------------------------------
        private void SetupFromAppConfig(AppConfiguration config, AdminArgs adminArgs)
        {
            adminArgs.UserName = config.GetValue<string>("UserName");
            adminArgs.Password = config.GetValue<string>("Password");
            adminArgs.MachineName = config.GetValue<string>("MachineName");
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Configures the AppConfiguration object. </summary>
        ///
        /// <remarks>   Pdelosreyes, 2/8/2017. </remarks>
        ///
        /// <param name="config">   The configuration. </param>
        ///-------------------------------------------------------------------------------------------------
        private void ConfigureObjects(AppConfiguration config)
        {
            string connString = config.GetValue<string>("DevOpsEntities");
            //UserProcessor = null;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Runs the given admin arguments. </summary>
        ///
        /// <remarks>   Pdelosreyes, 2/8/2017. </remarks>
        ///
        /// <exception cref="Exception">    Thrown when an exception error condition occurs. </exception>
        ///
        /// <param name="adminArgs">    The admin arguments. </param>
        ///-------------------------------------------------------------------------------------------------
        private void Run(AdminArgs adminArgs)
        {
            switch (adminArgs.Action)
            {
                case AdminActions.AddAllEnvVariables:
                    Actions.AddAllEnvVariables(adminArgs);
                    break;
                case AdminActions.RemoveAllEnvVariables:
                    Actions.RemoveAllEnvVariables(adminArgs);
                    break;
                case AdminActions.ListAllEnvVariables:
                    Actions.ListAllEnvVariables(adminArgs);
                    break;
                case AdminActions.ListAllAppConfigVariablesFromDb:
                    Actions.ListAllDbConfigVariables(adminArgs);
                    break;
                case AdminActions.ListAllDiffAppConfigVariables:
                    Actions.ListAllDiffConfigVariables(adminArgs);
                    break;
                case AdminActions.AddEnvVariable:
                    Actions.AddEnvVariable(adminArgs);
                    break;
                case AdminActions.RemoveEnvVariable:
                    Actions.RemoveEnvVariable(adminArgs);
                    break;
                case AdminActions.GetEnvValue:
                    Actions.GetEnvValue(adminArgs);
                    break;
                case AdminActions.AddAllAppConfigVariables:
                    Actions.AddAllAppConfigVariables(adminArgs);
                    break;
                case AdminActions.RemoveAllAppConfigVariables:
                    Actions.RemoveAllAppConfigVariables(adminArgs);
                    break;
                case AdminActions.ListAllAppConfigVariables:
                    Actions.ListAllAppConfigVariables(adminArgs);
                    break;
                case AdminActions.AddAppConfigVariable:
                    Actions.AddAppConfigVariable(adminArgs);
                    break;
                case AdminActions.RemoveAppConfigVariable:
                    Actions.RemoveAppConfigVariable(adminArgs);
                    break;
                case AdminActions.GetAppConfigValue:
                    Actions.GetAppConfigValue(adminArgs);
                    break;
                default:
                    throw new Exception($"Action {adminArgs.Action} is unknown");
                    break;
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Sets configuration type. </summary>
        ///
        /// <remarks>   Pdelosreyes, 2/23/2017. </remarks>
        ///
        /// <param name="adminArgs">    The admin arguments. </param>
        ///-------------------------------------------------------------------------------------------------
        private void SetConfigType(AdminArgs adminArgs)
        {
            switch (adminArgs.KeyType)
            {
                case KeyTypes.AppSetting:
                    adminArgs.KeyType = "appSettings";
                    break;
                case KeyTypes.ConnectionString:
                    adminArgs.KeyType = "connectionStrings";
                    break;
                case KeyTypes.User:
                    adminArgs.KeyType = "User";
                    break;
                case KeyTypes.Machine:
                    adminArgs.KeyType = "Machine";
                    break;
                case KeyTypes.Session:
                    adminArgs.KeyType = "Session";
                    break;
                default:
                    adminArgs.KeyType = "appSettings";
                    break;
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Shows the usage. </summary>
        ///
        /// <remarks>   Pdelosreyes, 2/9/2017. </remarks>
        ///
        /// <param name="opts">         Options for controlling the operation. </param>
        /// <param name="AddTopLine">   (Optional) The add top line. </param>
        /// <param name="badcmd">       (Optional) True to badcmd. </param>
        ///-------------------------------------------------------------------------------------------------
        public void ShowUsage(OptionSet opts, String AddTopLine = null, bool badcmd = false)
        {
            if (AddTopLine != null)
                Console.WriteLine(AddTopLine + "\n");

            Console.WriteLine("Usage: " + AppName + " <options>");
            if (badcmd)
                Console.WriteLine("\n\nIncorrect commandline ... task complete\n\nPress any Key to Exit.");
            opts.WriteOptionDescriptions(Console.Out);
            string text = ",\tEnvironment Variable Actions:," +
                "\t\tget-allvars|get-allenvars|get-envalue," +
                "\t\tadd-allenvars|remove-allenvars," +
                "\t\tadd-envar|remove-envar,," +
                "\tApp Config Actions:," +
                "\t\tget-allappconfig|get-appconfigvalue|get-dbappconfig|get-diffappconfig," +
                "\t\tadd-allappconfig|remove-allappconfig," +
                "\t\tadd-appconfig|remove-appconfig";
            text = text.Replace(",", Environment.NewLine);
            Console.WriteLine(text);

#if DEBUG
            Console.Read();
#endif
        }
    }
}
