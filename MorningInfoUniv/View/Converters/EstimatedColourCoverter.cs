using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace MorningInfoUniv.View.Converters
{
    public class EstimatedColourCoverter : IValueConverter
    {

        public Brush Red { get; set; }
        public Brush Green { get; set; }
        public Brush Amber { get; set; }


        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if(value is string)
            {
                switch (((string)value).ToLower())
                {
                    case "on time":
                        return Green;
                        
                    case "cancelled":
                        return Red;
                        
                    default:
                        return Amber;                        
                }                
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
