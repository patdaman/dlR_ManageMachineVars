using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using ViewModel;
using static ViewModel.ConfigModels;

namespace CommonUtils.AppConfiguration
{
    public class AppConfigFunctions
    {
        public XDocument configFile { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Default constructor. </summary>
        ///
        /// <remarks>   Pdelosreyes, 2/9/2017. </remarks>
        ///-------------------------------------------------------------------------------------------------
        public AppConfigFunctions()
        {
            configFile = new XDocument();
        }

        public AppConfigFunctions(XDocument xmlDoc)
        {
            configFile = xmlDoc;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   List configuration variables. </summary>
        ///
        /// <remarks>   Pdelosreyes, 2/10/2017. </remarks>
        ///
        /// <param name="keys"> The keys. </param>
        ///
        /// <returns>   A List&lt;AttributeKeyValuePair&gt; </returns>
        ///-------------------------------------------------------------------------------------------------
        public List<AttributeKeyValuePair> ListConfigVariables(List<AttributeKeyValuePair> keys)
        {
            foreach (var element in configFile.Descendants())
            {
                foreach (var keyValue in keys)
                {
                    /// todo:
                    /// Figure out element / key pair that will match!
                    if (element.Name == keyValue.attribute && element.Name == keyValue.key)
                    {
                        keyValue.value = element.Value;
                    }
                }
            }
            return keys;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   List configuration variables. </summary>
        ///
        /// <remarks>   Pdelosreyes, 2/10/2017. </remarks>
        ///
        /// <returns>   A List&lt;AttributeKeyValuePair&gt; </returns>
        ///-------------------------------------------------------------------------------------------------
        public List<AttributeKeyValuePair> ListConfigVariables()
        {
            return ListConfigVariables(configFile);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   List configuration variables. </summary>
        ///
        /// <remarks>   Pdelosreyes, 2/10/2017. </remarks>
        ///
        /// <param name="xmlDoc">   The XML document. </param>
        ///
        /// <returns>   A List&lt;AttributeKeyValuePair&gt; </returns>
        ///-------------------------------------------------------------------------------------------------
        public List<AttributeKeyValuePair> ListConfigVariables(XDocument xmlDoc)
        {
            var keyValues = new List<AttributeKeyValuePair>();
            foreach (var element in configFile.Descendants())
            {
                if (element.Name == "add")
                {
                    if (element.FirstAttribute.Name == "key")
                    {
                        keyValues.Add(new AttributeKeyValuePair()
                        {
                            attribute = element.Name.ToString(),
                            key = element.FirstAttribute.Value.ToString(),
                            value = element.LastAttribute.Value.ToString()
                        });
                    }
                    if (element.Parent.Name.ToString().Contains("connectionString"))
                    {
                        keyValues.Add(new AttributeKeyValuePair()
                        {
                            attribute = "connectionString",
                            key = element.FirstAttribute.Value.ToString(),
                            value = element.LastAttribute.PreviousAttribute.Value.ToString()
                        });
                    }
                }
            }
            return keyValues;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets key value. </summary>
        ///
        /// <remarks>   Pdelosreyes, 2/10/2017. </remarks>
        ///
        /// <param name="attribute">    The attribute. </param>
        /// <param name="appKey">       The application key. </param>
        ///
        /// <returns>   The key value. </returns>
        ///-------------------------------------------------------------------------------------------------
        public AttributeKeyValuePair GetKeyValue(string attribute, string appKey)
        {
            var list = from appNode in configFile.Elements()
                   where appNode.Attribute(attribute).Value == appKey
                   select appNode;

            var e = list.FirstOrDefault();

            XElement element = configFile.Descendants(appKey).FirstOrDefault();
            return new AttributeKeyValuePair()
            {
                attribute = attribute,
                key = appKey,
                value = element.Value
            };
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Removes the key value. </summary>
        ///
        /// <remarks>   Pdelosreyes, 2/10/2017. </remarks>
        ///
        /// <param name="attribute">    The attribute. </param>
        /// <param name="appKey">       The application key. </param>
        /// <param name="element">      (Optional) The element. </param>
        ///
        /// <returns>   An Enums.ModifyResult. </returns>
        ///-------------------------------------------------------------------------------------------------
        public Enums.ModifyResult RemoveKeyValue(string attribute, string appKey, string element = null)
        {
            foreach (XElement x in configFile.Descendants())
            {
                if (string.IsNullOrEmpty(element) || x.Name == element)
                {
                    if (x.FirstAttribute.Name.ToString() == attribute &&
                        x.FirstAttribute.Value.ToString() == appKey)
                    {
                        try
                        {
                            x.Remove();
                            return Enums.ModifyResult.Removed;
                        }
                        catch (Exception ex)
                        {
                            return Enums.ModifyResult.Failed;
                            // this.Logger.Error(ExamineException.GetInnerExceptionAndStackTrackMessage(ex));
                        }
                    }
                }
            }
            return Enums.ModifyResult.NotFound;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Adds a key value to 'value'. </summary>
        ///
        /// <remarks>   Pdelosreyes, 2/10/2017. </remarks>
        ///
        /// <param name="appKey">   The application key. </param>
        /// <param name="value">    The value. </param>
        ///
        /// <returns>   An Enums.ModifyResult. </returns>
        ///-------------------------------------------------------------------------------------------------
        public Enums.ModifyResult AddKeyValue(string appKey, string value)
        {
            return UpdateOrCreateAppSetting(appKey, "value", value, "key", "appSettings");
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Adds a connection string to 'value'. </summary>
        ///
        /// <remarks>   Pdelosreyes, 2/10/2017. </remarks>
        ///
        /// <param name="appKey">   The application key. </param>
        /// <param name="value">    The value. </param>
        ///
        /// <returns>   An Enums.ModifyResult. </returns>
        ///-------------------------------------------------------------------------------------------------
        public Enums.ModifyResult AddConnectionString(string appKey, string value)
        {
            return UpdateOrCreateAppSetting(appKey, "connectionString", value, "name", "connectionStrings");
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Updates the or create application setting. </summary>
        ///
        /// <remarks>   Pdelosreyes, 2/10/2017. </remarks>
        ///
        /// <param name="appKey">       The application key. </param>
        /// <param name="valueName">    Name of the value. </param>
        /// <param name="value">        The value. </param>
        /// <param name="attribute">    The attribute. </param>
        /// <param name="element">      (Optional) The element. </param>
        ///
        /// <returns>   An Enums.ModifyResult. </returns>
        ///-------------------------------------------------------------------------------------------------
        public Enums.ModifyResult UpdateOrCreateAppSetting(string attribute, string appKey, string valueName, string value, string element = null)
        {
            string parent = string.Empty;
            string providerName = string.Empty;

            if (String.IsNullOrEmpty(element))
            {
                element = "add";
            }
            if (appKey == "key")
                parent = "appSettings";
            if (valueName == "connectionString")
            {
                parent = "connectionStrings";
                providerName = "System.Data.EntityClient";
            }

            var list = (from appNode in configFile.Descendants(parent)
                       select appNode)
                        .ToList();

            if (list == null)
            {
                try
                {
                    configFile.Root.Add(new XElement(parent));
                }
                catch (Exception ex)
                {
                    return Enums.ModifyResult.Failed;
                    //this.Logger.Error(ExamineException.GetInnerExceptionAndStackTrackMessage(ex));
                }
            }

            foreach (XElement y in configFile.Descendants(parent))
            {
                foreach (var x in y.Elements())
                {
                    if (x.Name == element)
                    {
                        if (x.FirstAttribute.Name.ToString() == attribute &&
                            x.FirstAttribute.Value.ToString() == appKey)
                        {
                            try
                            {
                                x.Attribute(valueName).Value = value;
                                return Enums.ModifyResult.Updated;
                            }
                            catch (Exception ex)
                            {
                                return Enums.ModifyResult.Failed;
                                // this.Logger.Error(ExamineException.GetInnerExceptionAndStackTrackMessage(ex));
                            }
                        }
                    }
                }
                try
                {
                    if (parent == "connectionStrings")
                    {
                        y.Add(new XElement("add"
                                , new XAttribute(attribute, appKey)
                                , new XAttribute(valueName, value)
                                , new XAttribute("providerName", providerName)));
                    }
                    else
                    {
                        y.Add(new XElement("add"
                                , new XAttribute(attribute, appKey)
                                , new XAttribute(valueName, value)));
                    }
                    return Enums.ModifyResult.Created;
                }
                catch (Exception ex)
                {
                    return Enums.ModifyResult.Failed;
                    //this.Logger.Error(ExamineException.GetInnerExceptionAndStackTrackMessage(ex));
                }
            }
            return Enums.ModifyResult.Unknown;
        }
    }
}
