using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using DataObjects.Entities;
using Services.Attribute;

namespace FitnessCenter.Binder
{
    public class DateModelBinder : DefaultModelBinder
    {
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var displayFormat = bindingContext.ModelMetadata.DisplayFormatString;
            var metadata = bindingContext.ModelMetadata;
            var datevalue = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
            DateTime date;
            if (metadata.ContainerType != null)
            {
                var type = metadata.ContainerType;


                var metadataType = type.GetCustomAttributes(typeof(MetadataTypeAttribute), true)
                    .OfType<MetadataTypeAttribute>().FirstOrDefault();

                var metaData = (metadataType != null)
                    ? ModelMetadataProviders.Current.GetMetadataForType(null, metadataType.MetadataClassType)
                    : ModelMetadataProviders.Current.GetMetadataForType(null, type);

                if (metaData.ModelType.GetProperty(metadata.PropertyName)
                                   .GetCustomAttributes(typeof(TrippleDDLDateTimeAttribute), true)
                                   .FirstOrDefault() is TrippleDDLDateTimeAttribute)
                {

                    var trippleDdl =
                          metaData.ModelType.GetProperty(metadata.PropertyName)
                                   .GetCustomAttributes(typeof(TrippleDDLDateTimeAttribute), true)
                                   .FirstOrDefault() as TrippleDDLDateTimeAttribute;

                    var prefix = bindingContext.ModelName;
                    var value = bindingContext.ValueProvider.GetValue(prefix);
                    var parts = value.RawValue as string[];
                    if (parts.All(string.IsNullOrEmpty))
                    {
                        return null;
                    }

                    bindingContext.ModelState.SetModelValue(prefix, value);

                    var dateStr = string.Format("{0}-{1}-{2}", parts[2], parts[1], parts[0]);

                    if (DateTime.TryParseExact(dateStr, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None,
                        out date))
                    {
                        return date;
                    }

                    bindingContext.ModelState.AddModelError(prefix, trippleDdl.ErrorMessage);

                    return null;
                }
                //                throw new Exception();
                if (string.IsNullOrEmpty(displayFormat) || datevalue == null)
                    return base.BindModel(controllerContext, bindingContext);

                displayFormat = displayFormat.Replace("{0:", string.Empty).Replace("}", string.Empty);
                if (DateTime.TryParseExact(datevalue.AttemptedValue, displayFormat, CultureInfo.InvariantCulture,
                    DateTimeStyles.None, out date))
                {
                    return date;
                }
                bindingContext.ModelState.AddModelError(
                    bindingContext.ModelName,
                    string.Format("{0} is an invalid date format", datevalue.AttemptedValue));

                return base.BindModel(controllerContext, bindingContext);
            }
            else
            {
                if (!string.IsNullOrEmpty(displayFormat) && datevalue != null)
                {
                    displayFormat = displayFormat.Replace("{0:", string.Empty).Replace("}", string.Empty);
                    if (DateTime.TryParseExact(datevalue.AttemptedValue, displayFormat, CultureInfo.InvariantCulture,
                        DateTimeStyles.None, out date))
                    {
                        return date;
                    }
                    bindingContext.ModelState.AddModelError(
                        bindingContext.ModelName,
                        string.Format("{0} is an invalid date format", datevalue.AttemptedValue));
                }

                return base.BindModel(controllerContext, bindingContext);
            }
        }
    }
}
