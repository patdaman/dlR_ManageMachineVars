using System;
using System.Linq;
using System.Configuration;
using System.Web.Configuration;
using System.Collections.Generic;

namespace CommonUtils.IISAdmin
{
    public class DomainUserSection : ConfigurationSection
    {
        [ConfigurationProperty("DomainUsers", IsRequired = true)]
        public DomainUserElementCollection DomainUsers
        {
            get { return base["DomainUsers"] as DomainUserElementCollection; }
        }
    }

    [ConfigurationCollection(typeof(DomainUser), AddItemName = "DomainUser")]
    public class DomainUserElementCollection : ConfigurationElementCollection, IEnumerable<DomainUser>
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new DomainUser();
        }
        protected override object GetElementKey(ConfigurationElement element)
        {
            var l_configElement = element as DomainUser;
            if (l_configElement != null)
                return l_configElement.id;
            else
                return null;
        }
        public DomainUser this[int index]
        {
            get { return BaseGet(index) as DomainUser; }
        }
        #region IEnumerable<ConfigElement> Members
        IEnumerator<DomainUser> IEnumerable<DomainUser>.GetEnumerator()
        {
            return (from i in Enumerable.Range(0, this.Count)
                    select this[i])
                    .GetEnumerator();
        }
        #endregion
    }

    public class DomainUser : ConfigurationElement
    {
        [ConfigurationProperty("id", IsKey = true, IsRequired = true)]
        public string id
        {
            get { return base["id"] as string; }
            set { base["id"] = value; }
        }
        [ConfigurationProperty("uri")]
        public Uri uri { get { return (Uri)this["uri"]; } }
        [ConfigurationProperty("domain")]
        public Domain domain { get { return (Domain)this["domain"]; } }
        [ConfigurationProperty("environment")]
        public SiteEnvironment environment { get { return (SiteEnvironment)this["environment"]; } }
        [ConfigurationProperty("username")]
        public Username username { get { return (Username)this["username"]; } }
        [ConfigurationProperty("password")]
        public Password password { get { return (Password)this["password"]; } }
    }
    public class Uri : ConfigurationElement
    {
        [ConfigurationProperty("value", IsKey = true, IsRequired = true)]
        public string Value
        {
            get { return base["value"] as string; }
            set { base["value"] = value; }
        }
    }
    public class Domain : ConfigurationElement
    {
        [ConfigurationProperty("value", IsKey = true, IsRequired = true)]
        public string Value
        {
            get { return base["value"] as string; }
            set { base["value"] = value; }
        }
    }
    public class SiteEnvironment : ConfigurationElement
    {
        [ConfigurationProperty("value", IsKey = true, IsRequired = true)]
        public string Value
        {
            get { return base["value"] as string; }
            set { base["value"] = value; }
        }
    }
    public class Username : ConfigurationElement
    {
        [ConfigurationProperty("value", IsKey = true, IsRequired = true)]
        public string Value
        {
            get { return base["value"] as string; }
            set { base["value"] = value; }
        }
    }

    public class Password : ConfigurationElement
    {
        [ConfigurationProperty("value", IsKey = true, IsRequired = true)]
        public string Value
        {
            get { return base["value"] as string; }
            set { base["value"] = value; }
        }
    }
}
