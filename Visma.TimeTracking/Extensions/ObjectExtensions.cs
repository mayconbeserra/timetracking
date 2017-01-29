using System.Collections.Generic;
using System.Dynamic;
using System.Reflection;

namespace Visma.TimeTracking.Extensions
{
    public static class ObjectExtensions
    {
        public static dynamic ToDynamic(this object value)
        {
            IDictionary<string, object> expando = new ExpandoObject();

            foreach (var property in value.GetType().GetProperties())
            {
                expando.Add(property.Name, property.GetValue(value));
            }

            return (ExpandoObject) expando;
        }
    }
}