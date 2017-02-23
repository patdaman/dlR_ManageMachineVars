using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CommonUtils.Reflection
{
    /// <summary>
    /// Custom attribute. This allows a user to select properties that should be 
    /// skipped when using the Copy Property methods below. 
    /// Usage: Simply decorate the property in the source object, that should be skiped.
    /// </summary>
    [System.AttributeUsage(System.AttributeTargets.Property)]    
    public class ReflectionUtils_SkipCopyProperty : System.Attribute
    {        
    }

    public class PropertyDifferenceInfo
    {
        public PropertyInfo PropertyInfo { get; set; }
        public object LeftValue { get; set; }
        public object RightValue { get; set; }

        public string PropertyName { get { return PropertyInfo?.Name; } }
    }

    public static class ReflectionUtils
    {
        /// <summary>
        /// Copy all properties by name from an expando object. 
        /// </summary>
        /// <param name="srcObject"></param>
        /// <param name="expandoObject"></param>
        public static void CopyProperties(object srcObject, ExpandoObject expandoObject)
        {
            if (srcObject == null || expandoObject == null) throw new ArgumentNullException();
            var expando = expandoObject as IDictionary<string, Object>;
            PropertyInfo[] properties = srcObject.GetType().GetProperties();
            foreach (var property in properties)
            {
                ReflectionUtils_SkipCopyProperty attribute = property.GetCustomAttribute<ReflectionUtils_SkipCopyProperty>();
                if (attribute == null)
                {
                    string propName = property.Name;
                    object propValue = property.GetValue(srcObject);
                    expando.Add(propName, propValue);
                }
            }
        }

        /// <summary>
        /// Copy all properties by name from one object to another.
        /// </summary>
        /// <remarks>
        /// Properties must be formal c# properties (i.e. defined with a get).
        /// THis performs a shallow copy.
        /// </remarks>
        /// <param name="srcObject"></param>
        /// <param name="destObject"></param>
        public static void CopyPropertiesTo(this object srcObject, object destObject)
        {
            if (srcObject == null || destObject == null) throw new ArgumentNullException();
            PropertyInfo[] properties = srcObject.GetType().GetProperties();
            foreach (var property in properties)
            {
                ReflectionUtils_SkipCopyProperty attribute = property.GetCustomAttribute<ReflectionUtils_SkipCopyProperty>();
                if (attribute == null)
                {
                    string propName = property.Name;
                    object propValue = property.GetValue(srcObject);
                    SetPropertyValueByName(destObject, propName, propValue);
                }
            }
        }

        /// <summary>
        /// Copy all properties by name from one object to another.
        /// </summary>
        /// <remarks>
        /// Properties must be formal c# properties (i.e. defined with a get).
        /// THis performs a shallow copy.
        /// </remarks>
        /// <param name="srcObject"></param>
        /// <param name="destObject"></param>
        public static void CopyPropertiesFrom(this object destObject, object srcObject)
        {
            if (srcObject == null || destObject == null) throw new ArgumentNullException();
            PropertyInfo[] properties = destObject.GetType().GetProperties();
            foreach (var property in properties)
            {   
                var attribute = srcObject.GetType().GetProperty(property.Name).GetCustomAttribute<ReflectionUtils_SkipCopyProperty>();
                if (attribute == null)
                {
                    string propName = property.Name;
                    object propValue = srcObject.GetPropertyValueByName(propName);
                    SetPropertyValueByName(destObject, propName, propValue);
                }
            }
        }


        /// <summary>
        /// Get property based on name given as string.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="propName"></param>
        /// <returns></returns>
        public static object GetPropertyValueByName(this object obj, string propName)
        {
            
            if (obj == null) throw new ArgumentNullException();
            try
            {
                return obj.GetType().GetProperty(propName).GetValue(obj);
            }
            catch(Exception ex)
            {
                throw new Exception($"ReflectionUtils: Failed to get property {propName}. See inner exception as well.", ex);
            }
        }


        /// <summary>
        /// Get property based on name given as string.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="propName"></param>
        /// <returns></returns>
        public static object TryGetPropertyValueByName(this object obj, string propName)
        {

            if (obj == null) throw new ArgumentNullException();
            try
            {
                return obj.GetType().GetProperty(propName).GetValue(obj);
            }
            catch (Exception)
            {
                return null;
            }
        }


        public static void CopyProperties(this object srcObject, object destObject, PropertyInfo[] properties)
        {
            if (srcObject == null || destObject == null) throw new ArgumentNullException();            
            foreach (var property in properties)
            {
                string propName = property.Name;
                object propValue = property.GetValue(srcObject);
                SetPropertyValueByName(destObject, propName, propValue);
            }
        }

        /// <summary>
        /// Set property given proporty name as string and an object value.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="propName"></param>
        /// <param name="value"></param>
        public static void SetPropertyValueByName(this object obj, string propName, object value)
        {
            obj.GetType().GetProperty(propName).SetValue(obj, value, null);
        }

        public static TType CreateNewObjectAndCopyProperties<TType>(this object srcObject) where TType : class, new() 
        {
            if (srcObject == null) throw new ArgumentNullException(); 

            if (srcObject == null) return null;
            var newObject = new TType();
            CopyPropertiesTo(srcObject, newObject);
            return newObject;
        }

        /// <summary>
        /// Returns a list of properties that do not match using the default == operator.
        /// </summary>
        /// <remarks>
        /// This should be used on classes that have basic types. If you are comparing properties that are objects 
        /// the equality operator should probably be overridden.
        /// </remarks>
        /// <param name="srcObject"></param>
        /// <param name=""></param>
        /// <returns></returns>
        public static List<PropertyDifferenceInfo> DiffProperties(this object srcObject, object targetObject)
        {
            if (srcObject == null || targetObject == null) throw new ArgumentNullException();

            var list = new List<PropertyDifferenceInfo>();
            PropertyInfo[] properties = srcObject.GetType().GetProperties();
            foreach (var property in properties)
            {
                object lhs = srcObject.GetPropertyValueByName(property.Name);
                object rhs = targetObject.GetPropertyValueByName(property.Name);


                if ( lhs == null && rhs != null ||
                     lhs != null && rhs == null ||                    
                     (lhs != null && (lhs.Equals(rhs) == false))) //compare the properties
                {
                    list.Add(
                        new PropertyDifferenceInfo
                        {
                            PropertyInfo = property,
                            LeftValue = property.GetValue(srcObject),
                            RightValue = property.GetValue(targetObject)
                        });
                }
            }
            return list;
        }



    }
}
