using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Services.Attribute
{
    public class MinAgeAttribute : ValidationAttribute
    {
        private readonly int _minAge;
        public MinAgeAttribute(int minAge)
        {
            _minAge = minAge;
        }

        public override bool IsValid(object value)
        {
            if (value == null)
            {
                return true;
            }

            DateTime date = Convert.ToDateTime(value);
            long ticks = DateTime.Now.Ticks - date.Ticks;
            int years = new DateTime(ticks).Year;
            return years >= _minAge;
        }
        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule();
            rule.ErrorMessage = ErrorMessage;
            rule.ValidationType = "minage";
            rule.ValidationParameters["min"] = _minAge;
            yield return rule;
        }
    }
}
