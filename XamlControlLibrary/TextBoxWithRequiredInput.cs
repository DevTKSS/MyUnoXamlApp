using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Extensions.Localization;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Documents;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Uno.Extensions.Localization;

// To learn about building custom controls see 
// https://learn.microsoft.com/windows/apps/winui/winui3/xaml-templated-controls-csharp-winui-3

namespace tks_controllibrary;
public sealed partial class TextBoxWithRequiredInput : TextBox
{
    public TextBoxWithRequiredInput()
    {
        this.DefaultStyleKey = typeof(TextBoxWithRequiredInput);
        //this.Loaded += (s, e) =>
        //{
        //    InputTextBox.Text = ResourceLoader.GetForViewIndependentUse().GetString("TextBoxWithRequiredInput_ErrorMessage")
        //};
    }
}
//public static class Converters
//{
//    public static IValueConverter StringToBool = new StringToBoolConverter();

//    private object StringToBoolConverter()
//    {
//        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
//        {
//            if (value is string str)
//            {
//                return !string.IsNullOrEmpty(str);
//            };
//            return false;
//        };
//        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
//        {
//            throw new NotImplementedException();
//        };
//    }
//}
