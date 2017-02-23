using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace UpdateAppConfig_Compact
{
    public class Actions
    {
        XDocument xmlDoc { get; set; }
        string path { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Removes all application configuration variables. </summary>
        ///
        /// <remarks>   Pdelosreyes, 2/10/2017. </remarks>
        ///
        /// <returns>   A List&lt;ConfigModifyResult&gt; </returns>
        ///-------------------------------------------------------------------------------------------------
        public List<Models.ConfigModifyResult> RemoveAllAppConfigVariables()
        {
            var resultList = new List<Models.ConfigModifyResult>();
            var configKeys = new List<string[]>();

            //var EFconfigKeys = (from vars in DevOpsContext.Machines
            //                    where vars.machine_name == machineName
            //                    select vars.ConfigVariables).FirstOrDefault();

            foreach (string[] dbKey in configKeys)
            {
                resultList.Add(new Models.ConfigModifyResult() { key = dbKey[1], result = RemoveKeyValue(dbKey[0], dbKey[1]) });
            }
            xmlDoc.Save(path);
            return resultList;
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
        /// <returns>   A Models.ModifyResult. </returns>
        ///-------------------------------------------------------------------------------------------------
        public Models.ModifyResult RemoveKeyValue(string attribute, string appKey, string element = null)
        {
            if (String.IsNullOrEmpty(element))
            {
                if (!xmlDoc.Descendants(appKey).Any())
                    return Models.ModifyResult.NotFound;
            }
            else
            {
                if (!xmlDoc.Elements(element).Descendants(appKey).Any())
                    return Models.ModifyResult.NotFound;
            }

            try
            {
                if (String.IsNullOrEmpty(element))
                {
                    xmlDoc.Descendants(appKey).Remove();
                }
                else
                {
                    xmlDoc.Element(element).Descendants(appKey).Remove();
                }
                xmlDoc.Save(path);
                return Models.ModifyResult.Removed;
            }
            catch (Exception ex)
            {
                return Models.ModifyResult.Failed;
                // this.Logger.Error(ExamineException.GetInnerExceptionAndStackTrackMessage(ex));

            }
            return Models.ModifyResult.Unknown;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Adds all application configuration variables. </summary>
        ///
        /// <remarks>   Pdelosreyes, 2/10/2017. </remarks>
        ///
        /// <returns>   A List&lt;ConfigModifyResult&gt; </returns>
        ///-------------------------------------------------------------------------------------------------
        public List<Models.ConfigModifyResult> AddAllAppConfigVariables()
        {
            var resultList = new List<Models.ConfigModifyResult>();
            var configKeys = new List<string[]>();

            //var EFconfigKeys = (from vars in DevOpsContext.Machines
            //                    where vars.machine_name == machineName
            //                    select vars.ConfigVariables).FirstOrDefault();

            foreach (var dbKey in configKeys)
            {
                resultList.Add(new Models.ConfigModifyResult() { key = dbKey[1], result = UpdateOrCreateAppSetting("", dbKey[0], dbKey[1], "", "") });
                //resultList.Add(new Models.ConfigModifyResult() { key = dbKey[1], result = UpdateOrCreateAppSetting(attribute, dbKey[0], dbKey[1], value, element) });
            }
            xmlDoc.Save(path);
            return resultList;
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
        public Models.ModifyResult UpdateOrCreateAppSetting(string appKey, string valueName, string value, string attribute, string element)
        {
            var list = from appNode in xmlDoc.Descendants(element).Elements()
                       where appNode.Attribute(attribute).Value == appKey
                       select appNode;
            var e = list.FirstOrDefault();

            try
            {
                // If the element doesn't exist, create it
                if (e == null)
                {
                    xmlDoc.Root.Element(element)
                        .Add(new XElement("add"
                                , new XAttribute(attribute, appKey)
                                , new XAttribute(valueName, value)));
                    xmlDoc.Save(path);
                    return Models.ModifyResult.Created;
                }
                else
                {
                    e.Attribute(valueName).SetValue(value);
                    xmlDoc.Save(path);
                    return Models.ModifyResult.Updated;
                }
            }
            catch (Exception ex)
            {
                return Models.ModifyResult.Failed;
                //this.Logger.Error(ExamineException.GetInnerExceptionAndStackTrackMessage(ex));
            }
            return Models.ModifyResult.Unknown;
        }

    }
}
