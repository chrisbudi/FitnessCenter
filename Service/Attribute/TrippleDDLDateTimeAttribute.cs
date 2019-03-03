using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Services.Attribute
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class TrippleDDLDateTimeAttribute : ValidationAttribute, IMetadataAware, IClientValidatable
    {
        public void OnMetadataCreated(ModelMetadata metadata)
        {
            metadata.TemplateHint = "TrippleDDLDateTime";
        }

        public override bool IsValid(object value)
        {
            // It's the custom model binder that is responsible for validating
            return true;
        }
        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule
            {
                ErrorMessage = ErrorMessage,
                ValidationType = "trippleddldate"
            };

            yield return rule;
        }
    }
}
