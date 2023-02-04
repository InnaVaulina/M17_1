using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Data;

namespace M17_Task31
{
    public class MyTemplateSelector : DataTemplateSelector
    {
        
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            FrameworkElement element = container as FrameworkElement;

            if (element != null && item != null)
            {
                string type = item.GetType().ToString();
                switch (type)
                {

                    case "M17_Task31.VM.Obj.DBObject.BuyersShowTableAVM":
                        return element.FindResource("BuyersTemplate") as DataTemplate;
                            
                    case "M17_Task31.VM.Obj.DBObject.ProductsShowTableAVM":
                        return element.FindResource("ProductsTemplate") as DataTemplate;
                    default: return null;
                }
            }
            return null;
        }
    }

}
