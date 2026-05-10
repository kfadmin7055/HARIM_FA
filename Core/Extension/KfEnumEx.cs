using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Core.Extension
{
    public static class KfEnumEx
    {
        public static string GetDesc(this System.Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());
            if (fi == null) return value.ToString();

            var attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
            return attributes.Length > 0 ? attributes[0].Description : value.ToString();
        }

        //public static T? TryGetEnumByDescription<T>(string description) where T : struct, Enum
        //{
        //    foreach (var field in typeof(T).GetFields())
        //    {
        //        var attr = (DescriptionAttribute)Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute));
        //        if (attr != null && attr.Description == description)
        //        {
        //            return (T)field.GetValue(null);
        //        }
        //    }
        //    return null;
        //}

    }
}
