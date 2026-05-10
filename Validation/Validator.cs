using System;
using System.Reflection;
using System.Collections.Generic;

namespace FinanceManager.Validation
{
    public static class Validator
    {
        public static List<string> Validate<T>(T obj)
        {
            var errors = new List<string>();
            Type type = obj.GetType();
            PropertyInfo[] properties = type.GetProperties();

            foreach (var prop in properties)
            {
                var positiveAttr = prop.GetCustomAttribute<PositiveValueAttribute>();
                if (positiveAttr != null)
                {
                    var value = prop.GetValue(obj);
                    if (value is decimal decValue && decValue <= 0)
                    {
                        errors.Add($"{prop.Name}: {positiveAttr.ErrorMessage}");
                    }
                }

                var requiredAttr = prop.GetCustomAttribute<RequiredStringAttribute>();
                if (requiredAttr != null)
                {
                    var value = prop.GetValue(obj) as string;
                    if (string.IsNullOrWhiteSpace(value))
                    {
                        errors.Add($"{prop.Name}: {requiredAttr.ErrorMessage}");
                    }
                }
            }

            return errors;
        }
    }
}