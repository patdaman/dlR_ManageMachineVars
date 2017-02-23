///-------------------------------------------------------------------------------------------------
// <summary>Implements the application configuration class</summary>
///-------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonUtils.AppConfiguration
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   An application configuration. </summary>
    ///
    /// <remarks>   Pdelosreyes, 2/8/2017. </remarks>
    ///-------------------------------------------------------------------------------------------------
    public class AppConfiguration
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the configuration providers. </summary>
        ///
        /// <value> The configuration providers. </value>
        ///-------------------------------------------------------------------------------------------------
        private List<IAppConfigurationProvider> ConfigProviders { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        /// Initializes a new instance of the CommonUtils.AppConfiguration.AppConfiguration class.
        /// </summary>
        ///
        /// <remarks>   Pdelosreyes, 2/8/2017. </remarks>
        ///-------------------------------------------------------------------------------------------------
        public AppConfiguration()
        {
            ConfigProviders = new List<IAppConfigurationProvider>();
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        /// Adds a provider. NOTE! Order matters providers are evaulated in reverse order meaning that
        /// the last provider in the chain takes precedence over previous ones.
        /// </summary>
        ///
        /// <remarks>   Pdelosreyes, 2/8/2017. </remarks>
        ///
        /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
        ///                                             null. </exception>
        ///
        /// <param name="provider"> The provider. </param>
        ///-------------------------------------------------------------------------------------------------
        public void AddProvider(IAppConfigurationProvider provider)
        {
            Debug.Assert(provider != null);
            if (provider == null)
                throw new ArgumentNullException("provider was null");

            ConfigProviders.Add(provider);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets a value. </summary>
        ///
        /// <remarks>   Pdelosreyes, 2/8/2017. </remarks>
        ///
        /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
        ///                                             null. </exception>
        /// <exception cref="Exception">                Thrown when an exception error condition occurs. </exception>
        ///
        /// <param name="key">  The key. </param>
        ///
        /// <returns>   The value. </returns>
        ///-------------------------------------------------------------------------------------------------
        public object GetValue(string key)
        {
            if (key == null)
                throw new ArgumentNullException();

            if (ConfigProviders.Count == 0)
                throw new Exception("No providers added to AppConfiguration.");

            object value = null;
            for (int iProvider = ConfigProviders.Count - 1; iProvider >= 0; iProvider--)
            {
                value = ConfigProviders[iProvider].GetValue(key);
                if (value != null)
                    break;
            }
            return value;
        }

        public T GetValue<T>(string key)
        {
            object value = GetValue(key);
            Type type = typeof(T);
            T returnvalue = (T)Convert.ChangeType(value, type);
            return returnvalue;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets value or null. </summary>
        ///
        /// <remarks>   Pdelosreyes, 2/8/2017. </remarks>
        ///
        /// <typeparam name="T">    Generic type parameter. </typeparam>
        /// <param name="key">  The key. </param>
        ///
        /// <returns>   The value or null. </returns>
        ///-------------------------------------------------------------------------------------------------
        public Nullable<T> GetValueOrNull<T>(string key) where T : struct
        {

            object value = GetValue(key);

            if (value != null)
                return (T)Convert.ChangeType(value, typeof(T));

            return null;
        }
    }
}
