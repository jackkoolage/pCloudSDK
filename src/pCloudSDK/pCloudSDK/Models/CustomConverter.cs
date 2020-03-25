using System;
using System.ComponentModel;

namespace pCloudSDK
{
    public class CustomConverter : ExpandableObjectConverter
    {

        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string))
            {
                return "(Browse)";
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }


    }
}
