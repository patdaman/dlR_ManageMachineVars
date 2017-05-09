using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using ViewModel;

namespace CommonUtils.AppConfiguration
{
    public class AppConfigFunctions
    {
        public string path { get; set; }
        public XDocument configFile { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Default constructor. </summary>
        ///
        /// <remarks>   Pdelosreyes, 2/9/2017. </remarks>
        ///-------------------------------------------------------------------------------------------------
        public AppConfigFunctions()
        {
            if (!string.IsNullOrWhiteSpace(path))
                configFile = XDocument.Load(path);
            else
                configFile = new XDocument();
        }

        public AppConfigFunctions(XDocument xmlDoc)
        {
            configFile = xmlDoc;
        }

        public AppConfigFunctions(string configPath)
        {
            configFile = XDocument.Load(configPath);
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
                    if (element.Name == keyValue.element && element.Name == keyValue.key)
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
                string parentName = string.Empty;
                string parentFullElement = string.Empty;
                if (element.Parent != null)
                {
                    parentName = element.Parent.Name.ToString() ?? "";
                    XElement res = new XElement(parentName);
                    res.Add(element.Parent.Attributes().ToArray());
                    parentFullElement = res.ToString();
                }
                XElement selfRes = new XElement(element.Name);
                selfRes.Add(element.Attributes().ToArray());
                string selfFullElement = selfRes.ToString();
                if (element.HasAttributes)
                {
                    if (element.FirstAttribute == element.LastAttribute)
                    {
                        keyValues.Add(new AttributeKeyValuePair()
                        {
                            //parentElement = parentName,
                            parentElement = parentFullElement,
                            fullElement = selfFullElement,
                            element = element.Name.ToString(),
                            attribute = element.FirstAttribute.Name.ToString(),
                            key = element.FirstAttribute.Name.ToString(),
                            valueName = "",
                            value = element.FirstAttribute.Value.ToString(),
                            Result = new ConfigModifyResult()
                        });
                    }
                    else
                    {
                        string keys = string.Empty;
                        string values = string.Empty;
                        string longAttributeKeys;
                        string longAttributeString;
                        IEnumerable<XAttribute> longAttributes;
                        if (element.Attributes().ToList().Count() > 2)
                        {
                            longAttributeKeys = string.Empty;
                            longAttributeString = string.Empty;
                            longAttributes = element.Attributes();
                            foreach (var att in longAttributes.Skip(1))
                            {
                                longAttributeKeys = longAttributeKeys + att.Name.ToString() + ",";
                                longAttributeString = longAttributeString + att.Name.ToString() + "=" + att.Value.ToString();
                            }
                            keys = longAttributeKeys;
                            values = longAttributeString;
                        }
                        else
                        {
                            keys = element.FirstAttribute.Value.ToString();
                            values = element.LastAttribute.Value.ToString();
                        }
                        keyValues.Add(new AttributeKeyValuePair()
                        {
                            //parentElement = parentName,
                            parentElement = parentFullElement,
                            fullElement = selfFullElement,
                            element = element.Name.ToString(),
                            attribute = element.FirstAttribute.Name.ToString(),
                            key = keys,
                            valueName = element.LastAttribute.Name.ToString(),
                            value = values,
                            Result = new ConfigModifyResult()
                        });
                    }
                }
                else if (!element.HasElements
                    // && element.FirstNode == element.LastNode
                    && element.Value != null)
                {
                    keyValues.Add(new AttributeKeyValuePair()
                    {
                        //parentElement = parentName,
                        parentElement = parentFullElement,
                        fullElement = selfFullElement,
                        element = element.Name.ToString(),
                        //attribute = element.FirstNode.ToString(),
                        attribute = string.Empty,
                        key = element.Name.ToString(),
                        valueName = "",
                        value = element.Value.ToString(),
                        Result = new ConfigModifyResult()
                    });
                }
                else
                {
                    var keyValue = new AttributeKeyValuePair()
                    {
                        //parentElement = parentName,
                        parentElement = parentFullElement,
                        fullElement = selfFullElement,
                        element = element.Name.ToString(),
                        attribute = "",
                        key = "",
                        valueName = "",
                        value = "",
                        Result = new ConfigModifyResult()
                    };
                    keyValues.Add(keyValue);
                }
            }
            return keyValues;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets application configuration value. </summary>
        ///
        /// <remarks>   Pdelosreyes, 3/7/2017. </remarks>
        ///
        /// <param name="key">      The key. </param>
        /// <param name="parent">   (Optional) The parent. </param>
        ///
        /// <returns>   The application configuration value. </returns>
        ///-------------------------------------------------------------------------------------------------
        public AttributeKeyValuePair GetAppConfigValue(string key, string parent = null)
        {
            if (parent == "connectionStrings" || parent == "connstring" || parent == "name")
                return GetKeyValue(key, "connectionStrings");
            if (parent == "appsetting" || parent == "key")
                return GetKeyValue(key, "appSettings");
            return GetKeyValue(key, parent);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Adds an application configuration variable. </summary>
        ///
        /// <remarks>   Pdelosreyes, 3/3/2017. </remarks>
        ///
        /// <param name="key">              The key. </param>
        /// <param name="value">            The value. </param>
        /// <param name="parent_element">   (Optional) The parent element. </param>
        ///
        /// <returns>   A ConfigModifyResult. </returns>
        ///-------------------------------------------------------------------------------------------------
        public ConfigModifyResult AddAppConfigVariable(string key, string value, string parent_element = null)
        {
            var result = new ConfigModifyResult()
            {
                key = key,
                result = AddKeyValue(key, value, parent_element)
            };
            return result;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Removes the application configuration variable. </summary>
        ///
        /// <remarks>   Pdelosreyes, 3/3/2017. </remarks>
        ///
        /// <param name="attribute">    (Optional) The element. </param>
        /// <param name="key">          The key. </param>
        ///
        /// <returns>   A ConfigModifyResult. </returns>
        ///-------------------------------------------------------------------------------------------------
        public ConfigModifyResult RemoveAppConfigVariable(string attribute, string key)
        {
            var result = new ConfigModifyResult()
            {
                key = key,
                result = RemoveKeyValue(attribute, key)
            };
            //configFile.Save(path);
            return result;
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
        public AttributeKeyValuePair GetKeyValue(string appKey, string parent_element = null)
        {
            string parentElementName = string.Empty;
            if (!string.IsNullOrWhiteSpace(parent_element))
            {
                char[] chars = { ' ', '=' };
                int index = parent_element.IndexOfAny(chars);
                if (index > 0)
                    parentElementName = parent_element.Substring(0, index).Replace("<", "").Replace(">", "");
                else
                    parentElementName = parent_element.Replace("<", "").Replace(">", "");
                foreach (XElement y in configFile.Descendants(parent_element))
                {
                    foreach (var x in y.Elements())
                    {
                        if (x.FirstAttribute.Value.ToString() == null)
                            return new AttributeKeyValuePair()
                            {
                                //parentElement = x.Parent.Name.ToString(),
                                parentElement = x.Parent.ToString(),
                                fullElement = x.ToString(),
                                element = x.Name.ToString(),
                                attribute = "",
                                key = x.Name.ToString(),
                                valueName = "",
                                value = x.Value.ToString()
                            };
                        if (x.FirstAttribute.Value.ToString() == appKey)
                            return new AttributeKeyValuePair()
                            {
                                //parentElement = x.Parent.Name.ToString(),
                                parentElement = x.Parent.ToString(),
                                fullElement = x.ToString(),
                                element = x.Name.ToString(),
                                attribute = x.FirstAttribute.Name.ToString(),
                                key = x.FirstAttribute.Value.ToString(),
                                valueName = x.FirstAttribute.NextAttribute.Value.ToString(),
                                value = x.FirstAttribute.NextAttribute.Value.ToString()
                            };
                    }
                }
            }
            else
            {
                foreach (XElement y in configFile.Descendants())
                {
                    foreach (var x in y.Elements())
                    {
                        string parentElement = string.Empty;
                        if (x.Parent != null)
                            parentElement = x.Parent.ToString();
                        if (x.FirstAttribute.Value.ToString() == appKey)
                            return new AttributeKeyValuePair()
                            {
                                //parentElement = x.Parent.Name.ToString(),
                                parentElement = parentElement,
                                fullElement = x.ToString(),
                                element = x.Name.ToString(),
                                attribute = "",
                                key = "",
                                valueName = x.FirstAttribute.NextAttribute.Value.ToString(),
                                value = x.FirstAttribute.NextAttribute.Value.ToString()
                            };
                    }
                }
            }
            return new AttributeKeyValuePair()
            {
                element = "",
                key = appKey,
                value = "KEY NOT FOUND"
            };
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Removes the key value. </summary>
        ///
        /// <remarks>   Pdelosreyes, 3/24/2017. </remarks>
        ///
        /// <param name="keyName">      The attribute. </param>
        /// <param name="appKey">       The application key. </param>
        /// <param name="forceDelete">  True to force delete. </param>
        ///
        /// <returns>   An Enums.ModifyResult. </returns>
        ///-------------------------------------------------------------------------------------------------
        public Enums.ModifyResult RemoveKeyValue(string keyName, string appKey, bool forceDelete)
        {
            return RemoveKeyValue(keyName, appKey, String.Empty, forceDelete);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Removes the key value. </summary>
        ///
        /// <remarks>   Pdelosreyes, 2/10/2017. </remarks>
        ///
        /// <param name="keyName">      The attribute. </param>
        /// <param name="appKey">       The application key. </param>
        /// <param name="element">      (Optional) The element. </param>
        /// <param name="forceDelete">  (Optional)
        ///                             True to force delete. </param>
        ///
        /// <returns>   An Enums.ModifyResult. </returns>
        ///-------------------------------------------------------------------------------------------------
        public Enums.ModifyResult RemoveKeyValue(string keyName, string appKey, string element = null, bool forceDelete = false)
        {
            foreach (XElement x in configFile.Descendants())
            {
                if (string.IsNullOrEmpty(element) || x.Name == element)
                {
                    if (x.FirstAttribute.Name.ToString() == keyName &&
                        x.FirstAttribute.Value.ToString() == appKey)
                    {
                        try
                        {
                            if (forceDelete)
                            {
                                x.Remove();
                                return Enums.ModifyResult.Removed;
                            }
                            else
                            {
                                x.ReplaceWith(new XComment(x.ToString()));
                                return Enums.ModifyResult.Commented;
                            }
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
        /// <param name="appKey">           The application key. </param>
        /// <param name="value">            The value. </param>
        /// <param name="parent_element">   (Optional) The parent element. </param>
        ///
        /// <returns>   An Enums.ModifyResult. </returns>
        ///-------------------------------------------------------------------------------------------------
        public Enums.ModifyResult AddKeyValue(string appKey, string value, string parent_element = null)
        {
            if (string.IsNullOrWhiteSpace(parent_element))
            {
                parent_element = "appSettings";
            }
            string full_element = "<add key=\"" + appKey + "\" value=\"" + value + "\">";
            return AddKeyValue("add", appKey, "value", value, "key", full_element, parent_element);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Updates the or create application setting. </summary>
        ///
        /// <remarks>   Pdelosreyes, 2/10/2017. </remarks>
        ///
        /// <param name="appKey">       The application key. </param>
        /// <param name="valueName">    Name of the value. </param>
        /// <param name="value">        The value. </param>
        /// <param name="keyName">    The attribute. </param>
        /// <param name="attribute">      (Optional) The element. </param>
        ///
        /// <returns>   An Enums.ModifyResult. </returns>
        ///-------------------------------------------------------------------------------------------------
        public List<Enums.ModifyResult> AddKeyValue(List<AttributeKeyValuePair> keyValuePairs)
        {
            List<Enums.ModifyResult> results = new List<Enums.ModifyResult>();
            foreach (var valuePair in keyValuePairs)
            {
                results.Add(AddKeyValue(
                    valuePair.attribute,
                    valuePair.key,
                    valuePair.valueName,
                    valuePair.value,
                    valuePair.element,
                    valuePair.fullElement,
                    valuePair.parentElement
                    ));
            }
            var orphans = keyValuePairs.Where(x => x.attribute == "").ToList();
            var incompleteParents = keyValuePairs.Where(x => orphans.Contains(x)).ToList();
            return results;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Adds a key value. </summary>
        ///
        /// <remarks>   Pdelosreyes, 3/31/2017. </remarks>
        ///
        /// <param name="attribute">          The attribute. </param>
        /// <param name="appKey">           The application key. </param>
        /// <param name="valueName">        Name of the value. </param>
        /// <param name="value">            The value. </param>
        /// <param name="parent_element">   (Optional) The parent element. </param>
        /// <param name="attribute">        (Optional) The element. </param>
        ///
        /// <returns>   An Enums.ModifyResult. </returns>
        ///-------------------------------------------------------------------------------------------------
        public Enums.ModifyResult AddKeyValue(string attribute, string appKey, string valueName, string value, string element, string full_element, string parent_element = null)
        {
            string parentElementName = string.Empty;
            if (string.IsNullOrWhiteSpace(parent_element))
            {
                var rootElement = configFile.Elements().Where(x => x.Name == element).FirstOrDefault();
                if (rootElement == null)
                {
                    if (string.IsNullOrWhiteSpace(attribute))
                    {
                        //if (string.IsNullOrWhiteSpace(valueName))
                        if (string.IsNullOrWhiteSpace(value))
                            configFile.Add(new XElement(element, string.Empty));
                        else
                            configFile.Add(new XElement(element, value));
                        return Enums.ModifyResult.Created;
                    }
                    else
                    {
                        if (string.IsNullOrWhiteSpace(valueName))
                            configFile.Add(new XElement(element,
                                new XAttribute(attribute, appKey)));
                        else
                            configFile.Add(new XElement(element,
                                  new XAttribute(attribute, appKey),
                                  new XAttribute(valueName, value)));
                        return Enums.ModifyResult.Created;
                    }
                }
            }

            List<XElement> parentElement = new List<XElement>();
            if (!string.IsNullOrWhiteSpace(parent_element))
            {
                char[] chars = { ' ', '=' };
                int index = parent_element.IndexOfAny(chars);
                if (index > 0)
                    parentElementName = parent_element.Substring(0, index).Replace("<", "").Replace(">", "");
                else
                    parentElementName = parent_element.Replace("<", "").Replace(">", "");
                if (configFile.Descendants(parentElementName) != null)
                {
                    parentElement.AddRange(configFile.Descendants(parentElementName).ToList());
                    parentElement.RemoveAll(x => new XElement(x.Name, x.Attributes().ToArray()).ToString() != parent_element);
                }
                if (parentElement == null || parentElement.Count == 0)
                {
                    configFile.Root.Add(XElement.Parse(parent_element));
                }
            }

            if (string.IsNullOrWhiteSpace(attribute))
            {
                var namedElement = parentElement.Elements().Where(x => x.Name == element).FirstOrDefault();
                if (namedElement == null)
                {
                    if (string.IsNullOrWhiteSpace(value))
                        parentElement.FirstOrDefault().Add(new XElement(element, string.Empty));
                    else
                        parentElement.FirstOrDefault().Add(new XElement(element, value));
                    return Enums.ModifyResult.Created;
                }
                else
                {
                    parentElement.Elements().Where(x => x.Name == element).FirstOrDefault().Value = value;
                    return Enums.ModifyResult.Updated;
                }
            }

            foreach (XElement y in parentElement)
            {
                foreach (var x in y.Elements())
                {
                    if (x.Name == element)
                    {
                        if (x.HasAttributes)
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
                                    // this.Logger.Error(ExamineException.GetInnerExceptionAndStackTrackMessage(ex));
                                    return Enums.ModifyResult.Failed;
                                }
                            }
                        }
                    }
                }
            }
            if (!string.IsNullOrWhiteSpace(attribute))
            {
                if (string.IsNullOrWhiteSpace(valueName))
                    if (attribute == value)
                        parentElement.FirstOrDefault().Add(new XElement(element, value));
                    else
                        parentElement.FirstOrDefault().Add(new XElement(element,
                                                new XAttribute(attribute, value)));
                else
                    parentElement.FirstOrDefault().Add(new XElement(element,
                                          new XAttribute(attribute, appKey),
                                          new XAttribute(valueName, value)));
            }
            else
            {
                if (string.IsNullOrWhiteSpace(valueName))
                    parentElement.FirstOrDefault().Add(new XElement(element, string.Empty));
                else
                    parentElement.FirstOrDefault().Add(new XElement(element, value));
            }
            return Enums.ModifyResult.Created;
            return Enums.ModifyResult.Failed;
        }
    }
}
