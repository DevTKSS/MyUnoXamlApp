namespace MyUnoXamlApp.Presentation;
[Bindable]
public sealed partial class SecondPage : Page
{
    public SecondPage()
    {
        this.InitializeComponent();
    }

    private async void ShowDialogAsync(object sender, RoutedEventArgs e)
    {
        try
        {
            string callerName = "";
            if (sender is Button clickedButton)
            {
                callerName = clickedButton.Name;
            }

            string dialogContent = callerName switch
            {
                "GoLeft" => "You went left and nothing was there",
                "GoRight" => "You went right and found a bucket of gold at the end of the rainbow!",
                _ => ""
            };
            var dialog = new ContentDialog()
            {
                Title = "Hello!",
                Content = dialogContent,
                PrimaryButtonText = "Yes",
                SecondaryButtonText = "No",
                CloseButtonText = "Cancel",
                XamlRoot = this.XamlRoot,
            };
            var result = await dialog.ShowAsync();
            if (this.FindName("DialogResult") is TextBlock DialogResult)
            {
                if (result == ContentDialogResult.Primary)
                {
                    DialogResult.Text = "User selected Yes.";
                }
                else if (result == ContentDialogResult.Secondary)
                {
                    DialogResult.Text = "User selected No.";
                }
                else
                {
                    DialogResult.Text = "User cancelled the Dialog";
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"ShowDialogAsync got exception: {ex.Message}");
        }
    }
}

