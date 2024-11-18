using System.Diagnostics;
using System.Drawing;
using System.Reflection;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml.Input;
using MyUnoXamlApp.Services.XamlRootProvider;
using Uno.Extensions.Specialized;

namespace MyUnoXamlApp.Presentation;

[Bindable]
public sealed partial class MainPage : Page
{
    public MainViewModel? ViewModel => DataContext as MainViewModel;

    public MainPage()
    {
        this.InitializeComponent();
        this.Loaded += (s,e) => XamlRootService.Initialize(this.XamlRoot!);        
    }

    public async void JustShowThisDialog(object sender, RoutedEventArgs e)
    {
        try
        {
            var dialog = new ContentDialog()
            {
                Title = "Hello!",
                Content = "No Message for you",
                PrimaryButtonText = "Okay",
                XamlRoot = XamlRootService.GetXamlRoot(),
            };
            await dialog.ShowAsync();

        }
        catch (Exception ex)
        {
            Console.WriteLine($"ShowMyDialog got exception: {ex.Message}");
        }
    }
}
