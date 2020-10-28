using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;

namespace RPN.Helpers
{
    internal static class ObjectParseHelper
    {
        internal static object GetCollection(object collection)
        {
            return (collection is IDictionary ? (IEnumerable)((IDictionary)collection).Values : (IEnumerable)collection).Cast<object>().ToArray();
        }
        internal static object GetPropertyValue(string propertyName, object inputObject)
        {
            if (inputObject == null)
                return null;

            dynamic objectClone = inputObject;
            var propertySplit = propertyName.Replace("[", ".").Replace("]", "").Trim('.').Split('.', count: 2);
            var specificName = propertySplit[0];
            var isRecursive = propertySplit.Length > 1;

            if (objectClone is ExpandoObject)
            {
                return ((IDictionary<string, object>)objectClone)[specificName];
            }
            else if (objectClone is IEnumerable)
            {
                if (objectClone is IDictionary)
                {
                    var dictionary = (IDictionary)objectClone;
                    var property = dictionary.GetType().GetProperty(specificName);

                    if (property == null)
                    {
                        var entryValue = dictionary[specificName];

                        return isRecursive ? GetPropertyValue(propertySplit[1], entryValue) : entryValue;
                    }
                    return property.GetValue(dictionary, null);
                }
                else //is array
                {
                    var array = ((IEnumerable)objectClone).Cast<object>().ToArray();

                    var property = array.GetType().GetProperty(specificName); //check if the field is a property or an item

                    if (property == null)
                    {
                        var idx = int.Parse(specificName);
                        var arrayItem = array[idx];

                        return isRecursive ? GetPropertyValue(propertySplit[1], arrayItem) : arrayItem;
                    }
                    return property.GetValue(array, null);
                }
            }
            else
            {
                var property = objectClone.GetType().GetProperty(specificName);
                var propertyValue = property.GetValue(objectClone);

                return isRecursive ? GetPropertyValue(propertySplit[1], propertyValue) : propertyValue;
            }
        }
    }
}
