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

        static void Main(string[] args)
        {
            var app = new ManageConfigVariables();
            app.Start(args);
        }

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

        protected void InitializeOptions(OptionSet additionalOptions)
        {
            //Default options provided to all derived applications 
            Options = new OptionSet()
            {
                {"l|log=", "log file path",   v => LogFilePath = v },
                {"h|?|help", "show usage", v => ShowHelp = true },
                {"e|email=", "email address for errors (optional), set to null to disable", v=> EmailAddress = v }
            };
            //Combined all options 
            if (additionalOptions != null)
                foreach (Option opt in additionalOptions.ToArray())
                {
                    Options.Add(opt);
                }
        }

        private void SetupFromCommandArgs(string[] args, AdminArgs adminArgs)
        {
            string text = "Action to perform. |{add-allenvars|remove-allenvars|get-allenvars |add-envar|remove-envar|get-envalue |add-allappconfig|remove-allappconfig|get-allappconfig |add-appconfig|remove-appconfig|get-appconfigvalue}";
            //text = text.Replace(",", System.Environment.NewLine);
            var additionalOpts = new OptionSet()
                {
                    { "interactive|i", "Switch that puts app in interactive mode.", v =>
                        {
                            IsInteractive = true;
                        }
                    },

                    {
                        "action|a=", "Action to perform. {add-allenvars|remove-allenvars|get-allenvars add-envar|remove-envar|get-envalue add-allappconfig|remove-allappconfig|get-allappconfig add-appconfig|remove-appconfig|get-appconfigvalue}",
                        v =>
                        //"action|a=", text, v =>
                        {
                            adminArgs.Action = v.ToLower();
                        }
                    },

                    {
                        "name|n=", "Target name (user, role, etc. dependingon action) to operate on.", v =>
                        {
                            adminArgs.UserName = v;
                        }
                    },

                    {
                        "password|pass=", "The password", v =>
                        {
                            adminArgs.Password = v;
                        }
                    },
                    {
                        "configfilepath|path=", "Path to XML config file.", v =>
                        {
                            adminArgs.Path = v;
                        }
                    },
                    {
                        "machinename|machine=", "Machine Name to target.", v =>
                        {
                            adminArgs.MachineName = v;
                        }
                    },
                };
            this.Initialize(typeof(ManageConfigVariables), "ManageConfigVariables", additionalOpts); 
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
                                OptionSet additionalOptions = null,
                                string defaultEmailAddress = null,
                                string logConfigFileName = null,
                                string emailSubject = null
                                , string configFacility = null
                                )
        {
            AppName = appName;
            Console.WriteLine(AppName + " v " + AssemblyVersionString);
            //InitializeLogging(applicationType, defaultEmailAddress, logConfigFileName, emailSubject, configFacility);
            InitializeOptions(additionalOptions);
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
#if DEBUG
            Console.Read();
#endif
        }
    }
}
