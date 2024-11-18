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
    private MainViewModel vm => (MainViewModel)DataContext;

    public MainPage()
    {
        this.InitializeComponent();
        this.Loaded += (s,e) => XamlRootService.Initialize(this.XamlRoot!);        
    }

    public async Task ShowMyDialog()
    {
        try
        {
         
            var dialog = new ContentDialog()
            {
                Title = "Hello!",
                Content = "No Message for you",
                PrimaryButtonText = "Okay",
                XamlRoot = this.XamlRoot,
            };
            await dialog.ShowAsync();
           
        }
        catch (Exception ex)
        {
            Console.WriteLine($"ShowMyDialog got exception: {ex.Message}");
        }
    }

}
