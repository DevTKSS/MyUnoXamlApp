
using System.Xml.Linq;
using MyUnoXamlApp.Services.XamlRootProvider;

namespace MyUnoXamlApp.Presentation;
[Bindable]
public partial record MainModel
{
    private INavigator _navigator;

    public MainModel(
        IOptions<AppConfig> appInfo,
        INavigator navigator)
    {
        _navigator = navigator;
        Title = "Main";
        Title += $" - {appInfo?.Value?.Environment}";
    }

    public string? Title { get; }

    public IState<string> Name => State<string>.Value(this, () => string.Empty);

    public IState<string> DialogAnswer => State<string>.Value(this, () => string.Empty);

    public async Task GoToStart()
    {
        var name = await Name;
        await _navigator.NavigateViewModelAsync<SecondModel>(this, data: new Entity(name!));
    }


    public async Task NavigateMyDialog()
    {
        var result = await _navigator.NavigateViewModelAsync<ContentDialogs.CoolContentDialog>(this);
        await this.DialogAnswer.SetAsync(result?.ToString() ?? string.Empty);
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

//    public async Task GoLeft()
//    {
//        var dialog = new ContentDialog()
//        {
//            Title = "Hello!",
//            Content = "I'am a Dialog",
//            PrimaryButtonText = "Yes",
//            SecondaryButtonText = "No",
//            CloseButtonText = "Cancel",
//        };
//        var result = await dialog.ShowAsync();
//        if (result == ContentDialogResult.Primary)
//        {
//            await Result.SetAsync(dialog.PrimaryButtonText);
//        }
//        else if (result == ContentDialogResult.Secondary)
//        {
//            await Result.SetAsync( dialog.SecondaryButtonText);
//        }
//        else
//        {
//            await Result.SetAsync(dialog.CloseButtonText);
//        }
//    }

//    public async Task ShowMyDialog(string caption,
//                                   string content,
//                                   string primaryButtonText,
//                                   string? secondaryButtonText = null)
//    {
//        var dialog = new ContentDialog()
//        {
//            Title = caption,
//            Content = content,
//            PrimaryButtonText = primaryButtonText
//        };
//        if (secondaryButtonText is not null)
//        {
//            dialog.SecondaryButtonText = secondaryButtonText;
//        }

//        await dialog.ShowAsync();
//    }
//}
