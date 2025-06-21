using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Layout;

namespace Fenet.App;

public partial class MainWindow : Window
{
    private readonly TextBox _addressBar;
    private readonly Button _goButton;

    private readonly BrowserCanvas _browserCanvas = new();
    
    public MainWindow()
    {
        InitializeComponent();

        _addressBar = new TextBox
        {
            Watermark = "Enter URL",
            Margin = new Thickness(3d),
        };

        _goButton = new Button
        {
            Content = "Go"
        };
        _goButton.Click += GoButton_Click;

        var topPanel = new StackPanel
        {
            Orientation = Orientation.Horizontal,
            HorizontalAlignment = HorizontalAlignment.Center,
            Children =
            {
                _addressBar,
                _goButton,
            }
        };
        
        DockPanel.SetDock(topPanel, Dock.Top);

        Content = new DockPanel
        {
            Children =
            {
                topPanel,
                _browserCanvas
            }
        };
    }

    private void GoButton_Click(object? sender, RoutedEventArgs e)
    {
        if (_addressBar.Text == null)
        {
            return;
        }
            
        _browserCanvas.Navigate(_addressBar.Text);
    }
}