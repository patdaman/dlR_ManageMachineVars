using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Deployment.Application;

namespace CommonUtils.Build
{
    public class AssemblyUtils
    {
        public const string NoEntryAssemblyFound = "NoEntryAssemblyFound";

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the application deployment version. Note, this will throw an exception when 
        ///             running in debug mode because the application will not be installed.</summary>
        ///
        /// <value> The assembly version. </value>
        ///-------------------------------------------------------------------------------------------------

        private static Version ApplicationDeploymentVersion
        {
            get
            {
                try
                {
                    return ApplicationDeployment.CurrentDeployment.CurrentVersion;
                }
                catch(Exception)
                {
                    //This method return errors in unit tests since there is no entry assembly.
                    return new Version(0, 0, 0, 0);
                }
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the deployment or assembly version string. If the assembly version is returned 
        ///             the string is post-fixed with a 'd'</summary>
        ///
        /// <value> The deployment or assembly version string. </value>
        ///-------------------------------------------------------------------------------------------------

        public static String EntryDeploymentOrAssemblyVersionString
        {
            get
            {
                try
                {
                    Version ver = ApplicationDeploymentVersion;
                    return ver.Major.ToString() + "." + ver.Minor.ToString() + "." + ver.Build.ToString() + "." + ver.Revision.ToString();
                }
                catch (Exception)
                {
                    return System.Reflection.Assembly.GetEntryAssembly().GetName().Version.ToString();
                }
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the name of the entry assembly. </summary>
        ///
        /// <value> The name of the entry assembly. </value>
        ///-------------------------------------------------------------------------------------------------

        public static string EntryAssemblyName
        {
            get
            {
                try
                {
                    return System.Reflection.Assembly.GetEntryAssembly().GetName().Name;
                }
                //This method return errors in unit tests since there is no entry assembly.
                catch( NullReferenceException ex)
                {
                    return NoEntryAssemblyFound;
                }
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the name of the machine. </summary>
        ///
        /// <value> The name of the machine. </value>
        ///-------------------------------------------------------------------------------------------------

        public static string MachineName
        {
            get
            {
                return System.Environment.MachineName; ;
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        /// Gets object of class type. Given an assembly, finds a class with the required name and class
        /// type and creates and returns an instance of that class using the default constructor.
        /// </summary>
        ///
        /// <remarks>   Ssur, 20160114. </remarks>
        ///
        /// <param name="assy">         The assembly. </param>
        /// <param name="classname">    The classname. </param>
        /// <param name="classtype">    The classtype. </param>
        ///
        /// <returns>   The object of class type. </returns>
        ///-------------------------------------------------------------------------------------------------

        public static Object GetObjectOfClassType(Assembly assy, string classname, Type classtype)
        {
            foreach (Type type in assy.GetTypes())
            {
                if (type.IsClass == true)
                    if (type.FullName.EndsWith("." + classname))
                        if (type == classtype)
                        {
                            return Activator.CreateInstance(type);
                        }
            }

            return null;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        /// Gets object of class type. Given an assembly, finds a class with the required name, checks if
        /// it is derived from the required baseclass, and creates and returns an instance of that class
        /// using the default constructor.
        /// </summary>
        ///
        /// <remarks>   Pdelosreyes, 4/25/2017. </remarks>
        ///
        /// <param name="assy">         The assembly. </param>
        /// <param name="classname">    The classname. </param>
        /// <param name="basetype">     The basetype. </param>
        ///
        /// <returns>   The object of class with base type. </returns>
        ///-------------------------------------------------------------------------------------------------
        public static Object GetObjectOfClassWithBaseType(Assembly assy, string classname, Type basetype)
        {
            foreach (Type type in assy.GetTypes())
            {
                if (type.IsClass == true)
                    if (type.FullName.EndsWith("." + classname))
                        if (type.IsSubclassOf(basetype))
                        {
                            return Activator.CreateInstance(type);
                        }
            }

            return null;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets object of class implementation. </summary>
        ///
        /// <remarks>   Pdelosreyes, 4/25/2017. </remarks>
        ///
        /// <param name="assy">         The assembly. </param>
        /// <param name="classname">    The classname. </param>
        /// <param name="impltype">     The impltype. </param>
        ///
        /// <returns>   The object of class implementation. </returns>
        ///-------------------------------------------------------------------------------------------------
        public static Object GetObjectOfClassImplementation(Assembly assy, string classname, Type impltype)
        {
            foreach (Type type in assy.GetTypes())
            {
                if (type.IsClass == true)
                    if (type.FullName.EndsWith("." + classname))
                        foreach (Type imty in type.GetInterfaces())
                            if (imty == impltype)
                            {
                                return Activator.CreateInstance(type);
                            }
            }

            return null;
        }
    }
}
