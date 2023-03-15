using System;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace USPS.AddressApi.Extensions
{
    internal static class EnumExtensions
    {
        internal static bool TryGetEnumFromChildElement<T>(this XElement element, string childName, out T obj) where T : Enum
        {
            var elm = element.Element(childName);

            if(string.IsNullOrEmpty(elm?.Value))
            {
                obj = default;
                return false;
            }

            return elm.Value.Trim().TryGetEnumFromName<T>(out obj);
        }

        internal static bool TryGetEnumFromName<T>(this string name, out T obj) where T : Enum
        {
            foreach(var field in typeof(T).GetFields())
            {
                if(Attribute.GetCustomAttribute(field, typeof(DisplayAttribute)) is DisplayAttribute attribute)
                {
                    if(attribute.Name == name)
                    {
                        obj = (T)field.GetValue(null);
                        return true;
                    }
                }

                if(field.Name == name)
                {
                    obj = (T)field.GetValue(null);
                    return true;
                }
            }

            obj = default;
            return false;
        }

        internal static bool TryGetChildString(this XElement element, string childName, out string str)
        {
            str = default;

            if(!string.IsNullOrEmpty(element.Element(childName)?.Value))
            {
                str = element.Element(childName).Value;
                return true;
            }

            return false;
        }
    }
}