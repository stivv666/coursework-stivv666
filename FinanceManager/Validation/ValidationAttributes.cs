using System;

namespace FinanceManager.Validation
{
    [AttributeUsage(AttributeTargets.Property)]
    public class PositiveValueAttribute : Attribute
    {
        public string ErrorMessage { get; }

        public PositiveValueAttribute(string errorMessage = "The sum must be greater than zero.")
        {
            ErrorMessage = errorMessage;
        }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class RequiredStringAttribute : Attribute
    {
        public string ErrorMessage { get; }

        public RequiredStringAttribute(string errorMessage = "This field cannot be empty.")
        {
            ErrorMessage = errorMessage;
        }
    }
}