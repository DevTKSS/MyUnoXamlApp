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
   // private MainViewModel vm => (MainViewModel)DataContext;
    private readonly HashSet<uint> activePointerIds = new HashSet<uint>();
    private bool isPointerCaptured = false;

    public MainPage()
    {
        this.InitializeComponent();
        this.Loaded += MainPage_Loaded;
        //this.Unloaded += MainPage_Unloaded;
                
    }

    //private void MainPage_Unloaded(object sender, RoutedEventArgs e)
    //{
    //    // Vorhandene Handler entfernen, um doppelte Abonnements zu vermeiden
    //    svControl.PointerPressed -= ScrollViewer_PointerPressed;
    //    Console.WriteLine("Removed PointerPressed Handler to svControl");
    //    svControl.PointerReleased -= OnPointerReleased;
    //    svControl.PointerCanceled -= OnPointerCanceled;
    //    svControl.PointerExited -= OnPointerExited;
    //    svControl.PointerCaptureLost -= OnPointerCaptureLost;
    //    tbName.KeyDown -= tbName_OnKeyDown;

    //}

    private void MainPage_Loaded(object sender, RoutedEventArgs e)
    {
        Console.WriteLine("Entered Main Page Loaded");
        XamlRootService.Initialize(this.XamlRoot!);
        
        var scrollContentPresenters = FindAllScrollContentPresenters(this);
        Console.WriteLine($"Found {scrollContentPresenters.Count} ScrollContentPresenter instance(s) in MainPage.");

        foreach (var presenter in scrollContentPresenters)
        {
            Console.WriteLine($"ScrollContentPresenter found: {presenter.GetType().Name}");

        }

        try
        {
            // Zuerst vorhandene Handler entfernen
            svControl.PointerPressed -= ScrollViewer_PointerPressed;
            svControl.PointerReleased -= ScrollViewer_OnPointerReleased;
            svControl.PointerCanceled -= ScrollViewer_OnPointerCanceled;
            svControl.PointerExited -= ScrollViewer_OnPointerExited;
            svControl.PointerCaptureLost -= ScrollViewer_OnPointerCaptureLost;
            tbName.KeyDown -= tbName_OnKeyDown;

            // Ereignishandler hinzufügen
            svControl.PointerPressed += ScrollViewer_PointerPressed;
            svControl.PointerReleased += ScrollViewer_OnPointerReleased;
            svControl.PointerCanceled += ScrollViewer_OnPointerCanceled;
            svControl.PointerExited += ScrollViewer_OnPointerExited;
            svControl.PointerCaptureLost += ScrollViewer_OnPointerCaptureLost;
            tbName.KeyDown += tbName_OnKeyDown;

            Console.WriteLine("Added Pointer Event Handlers to svControl");
        }
        catch (Exception ex)
        {
            string exceptionDetails = ExceptionHelper.HandleException(ex);
            Console.WriteLine("MainPage_Loaded caught Exception: " + exceptionDetails);
        }
    }

    private void SomeButton_Click(object sender, RoutedEventArgs e)
    {
        ShowMyDialog(sender, e);
    }

    private void tbName_OnKeyDown(object sender, KeyRoutedEventArgs e)
    {
        if (e.Key == Windows.System.VirtualKey.Enter)
        {
            tbName.FocusState = FocusState.Unfocused;
        }
    }

    private async void ShowMyDialog(object sender, RoutedEventArgs e)
    {
        try
        {
            string callerName = "";
            if (sender is Button clickedButton)
            {
                callerName = clickedButton.Name;
            }

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

#region "Pointer Interactions"
    private void ScrollViewer_OnPointerCaptureLost(object sender, PointerRoutedEventArgs e)
    {
        Console.WriteLine($"--- Pointer with ID:{e.Pointer.PointerId} did lost its capture!");
    }
    private void ScrollViewer_OnPointerReleased(object sender, PointerRoutedEventArgs e)
    {
        Console.WriteLine($"--- Pointer with ID:{e.Pointer.PointerId} was released");
        if (isPointerCaptured)
        {
            svControl.ReleasePointerCapture(e.Pointer);
            isPointerCaptured = false;
            Console.WriteLine($"--- PointerReleased and Pointer Capture Released: ID={e.Pointer.PointerId}");
        }

        activePointerIds.Remove(e.Pointer.PointerId); // Entferne den Pointer
        LogActivePointers(e);
    }

    private void ScrollViewer_OnPointerCanceled(object sender, PointerRoutedEventArgs e)
    {
        Console.WriteLine($"--- Pointer with ID:{e.Pointer.PointerId} was canceled");
        activePointerIds.Remove(e.Pointer.PointerId); // Entferne den Pointer
        LogActivePointers(e);
    }

    private void ScrollViewer_OnPointerExited(object sender, PointerRoutedEventArgs e)
    {
        Console.WriteLine($"--- Pointer with ID:{e.Pointer.PointerId} has Exited");
        activePointerIds.Remove(e.Pointer.PointerId); // Entferne den Pointer
        LogActivePointers(e);
        svControl.ReleasePointerCapture(e.Pointer);
    }

    private void LogActivePointers(PointerRoutedEventArgs e)
    {
       
        Console.WriteLine($"Active Pointers Count: {activePointerIds.Count}");
        Console.WriteLine("Active Pointer IDs: \n" + string.Join(",\n", activePointerIds));
        
        Console.WriteLine("Pointer Data:");
        var pointerPoint = e.GetCurrentPoint(this);
        Console.WriteLine($"Pointer ID's of e.GetCurrentPointer(this) and e.Pointer are {(pointerPoint.PointerId == e.Pointer.PointerId ? "the same" : "different!")}");

        Console.WriteLine($"Original Source: " + e.OriginalSource.ToString());
        Console.WriteLine($"Pointer ID: {e.Pointer.PointerId}");
        Console.WriteLine($"Timestamp: {pointerPoint.Timestamp}");
        Console.WriteLine($"Pointer Device Type: {e.Pointer.PointerDeviceType}");
        Console.WriteLine($"PointerUpdateKind: {pointerPoint.Properties.PointerUpdateKind}");
        Console.WriteLine($"Position: X={pointerPoint.Position.X}, Y={pointerPoint.Position.Y}");

        Console.WriteLine($"IsPrimary: {pointerPoint.Properties.IsPrimary}");
        bool isPressed = pointerPoint.Properties.IsLeftButtonPressed ||
                         pointerPoint.Properties.IsRightButtonPressed ||
                         pointerPoint.Properties.IsMiddleButtonPressed ||
                         pointerPoint.Properties.IsBarrelButtonPressed ||
                         pointerPoint.Properties.IsHorizontalMouseWheel;
        Console.WriteLine($"isPressed: {isPressed}");
        Console.WriteLine($"isInContact: {pointerPoint.IsInContact}");
        Console.WriteLine($"isEraser: {pointerPoint.Properties.IsEraser}");
        Console.WriteLine("-----------------------------------------\n");
    }

    private void ScrollViewer_PointerPressed(object sender, PointerRoutedEventArgs e)
    {
        try
        {
            Console.WriteLine($"--- PointerPressed Event Triggered");
            if (!isPointerCaptured)  
            {
                svControl.CapturePointer(e.Pointer);
                isPointerCaptured = true;
                Console.WriteLine($"+ Pointer Captured: ID={e.Pointer.PointerId}");
            }
            activePointerIds.Add(e.Pointer.PointerId); // Füge den Pointer hinzu
            
            LogActivePointers(e);
            Console.WriteLine();

        }
        catch (Exception ex) 
        {
            Console.WriteLine("ScrollView_PointerPressed got Exception: " + ExceptionHelper.HandleException(ex));
        }
    }
    #endregion

    private List<ScrollContentPresenter> FindAllScrollContentPresenters(DependencyObject parent)
    {
        var foundPresenters = new List<ScrollContentPresenter>();

        int childrenCount = VisualTreeHelper.GetChildrenCount(parent);
        for (int i = 0; i < childrenCount; i++)
        {
            var child = VisualTreeHelper.GetChild(parent, i);

            // Prüfen, ob das Kind ein ScrollContentPresenter ist und zur Liste hinzufügen
            if (child is ScrollContentPresenter scrollContentPresenter)
            {
                foundPresenters.Add(scrollContentPresenter);
            }

            // Rekursiver Aufruf, um nach weiteren ScrollContentPresentern in untergeordneten Elementen zu suchen
            foundPresenters.AddRange(FindAllScrollContentPresenters(child));
        }

        return foundPresenters;
    }
}
