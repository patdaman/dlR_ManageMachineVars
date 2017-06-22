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
            get
            {
                return base["DomainUsers"] as DomainUserElementCollection;
            }
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
                return l_configElement.Key;
            else
                return null;
        }
        public DomainUser this[int index]
        {
            get
            {
                return BaseGet(index) as DomainUser;
            }
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
        [ConfigurationProperty("key", IsKey = true, IsRequired = true)]
        public string Key
        {
            get
            {
                return base["key"] as string;
            }
            set
            {
                base["key"] = value;
            }
        }
        [ConfigurationProperty("UserDetails")]
        public ConfigUserDetailsCollection UserDetails
        {
            get
            {
                return base["UserDetails"] as ConfigUserDetailsCollection;
            }
        }
    }

    [ConfigurationCollection(typeof(UserDetail), AddItemName = "UserDetail")]
    public class ConfigUserDetailsCollection : ConfigurationElementCollection, IEnumerable<UserDetail>
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new UserDetail();
        }
        protected override object GetElementKey(ConfigurationElement element)
        {
            var l_configElement = element as UserDetail;
            if (l_configElement != null)
                return l_configElement.Key;
            else
                return null;
        }
        public UserDetail this[int index]
        {
            get
            {
                return BaseGet(index) as UserDetail;
            }
        }

        #region IEnumerable<UserDetail> Members
        IEnumerator<UserDetail> IEnumerable<UserDetail>.GetEnumerator()
        {
            return (from i in Enumerable.Range(0, this.Count)
                    select this[i])
                    .GetEnumerator();
        }
        #endregion
    }

    public class UserDetail : ConfigurationElement
    {
        [ConfigurationProperty("key", IsKey = true, IsRequired = true)]
        public string Key
        {
            get
            {
                return base["key"] as string;
            }
            set
            {
                base["key"] = value;
            }
        }
        [ConfigurationProperty("value", IsKey = false, IsRequired = false)]
        public string Value
        {
            get
            {
                return base["value"] as string;
            }
            set
            {
                base["value"] = value;
            }
        }
    }
}
