using CommunityToolkit.Maui.Alerts;

namespace BrokenMeasureInvalidationSample;

public partial class MainPage : ContentPage
{
    private bool _headless;
    private VerticalStackLayout _layout;

    public MainPage()
    {
        InitializeComponent();
        MainVerticalStackLayout[3] = _layout = CreateLayout();
    }

    private VerticalStackLayout CreateLayout()
    {
        var vsl = new VerticalStackLayout { BackgroundColor = Colors.LightGreen };
        if (_headless)
        {
            vsl.BackgroundColor = Colors.PaleVioletRed; // This should not be visible due to the headless layout
            CompressedLayout.SetIsHeadless(vsl, true);
        }

        vsl.MeasureInvalidated += (sender, args) =>
        {
            Toast.Make("Measure invalidated").Show();
        };

        return vsl;
    }

    private void ToggleHeadless(object? sender, EventArgs e)
    {
        _headless = !_headless;
        
        var children = _layout.Children.ToList();
        _layout.Children.Clear();

        _layout = CreateLayout();
        foreach (var child in children)
        {
            _layout.Add(child);
        }

        MainVerticalStackLayout[3] = _layout;
    }

    private void ToggleLabel(object? sender, EventArgs e)
    {
        if (_layout.Count == 0)
        {
            _layout.Add(new Label
            {
                Text = "Height: 100",
                HeightRequest = 100,
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center
            });
        }
        else
        {
            _layout.Clear();
        }
    }

    private void ResizeLabel(object? sender, EventArgs e)
    {
        if (_layout.Count == 1 && _layout[0] is Label label)
        {
            if (label.HeightRequest == 100)
            {
                label.HeightRequest = 200;
                label.Text = "Height: 200";
            }
            else
            {
                label.HeightRequest = 100;
                label.Text = "Height: 100";
            }
        }
    }
}