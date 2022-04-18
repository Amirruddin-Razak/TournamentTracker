using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Shell;

namespace TrackerWPFUI.CustomControls
{
    public class CustomWindow : Window
    {
        private const int _windowEdgeMargin = 6; // Used to correct for oversized window when maximized
        private TextBlock _title;
        private Border _titleBar;
        private Border _dropShadowBorder;
        private Border _windowBorder;
        private Button _minimizeButton;
        private Button _maximizeButton;
        private Button _closeButton;
        private Button _windowIconButton;

        public static readonly DependencyProperty ResizeBorderProperty =
            DependencyProperty.Register("ResizeBorder", typeof(double), typeof(CustomWindow), new PropertyMetadata(4d));
        public static readonly DependencyProperty OuterMarginProperty =
            DependencyProperty.Register("OuterMargin", typeof(double), typeof(CustomWindow), new PropertyMetadata(20d));
        public static readonly DependencyProperty TitleBarHeightProperty =
            DependencyProperty.Register("TitleBarHeight", typeof(double), typeof(CustomWindow), new PropertyMetadata(23d));
        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register("CornerRadius", typeof(double), typeof(CustomWindow), new PropertyMetadata(8d));
        public static readonly DependencyProperty DropShadowDepthProperty =
            DependencyProperty.Register("DropShadowDepth", typeof(double), typeof(CustomWindow), new PropertyMetadata(10d));
        public static readonly DependencyProperty DropShadowOpacityProperty =
            DependencyProperty.Register("DropShadowOpacity", typeof(double), typeof(CustomWindow), new PropertyMetadata(0.2d));
        public static readonly DependencyProperty DropShadowBlurRadiusProperty =
            DependencyProperty.Register("DropShadowBlurRadius", typeof(double), typeof(CustomWindow), new PropertyMetadata(10d));
        public static readonly DependencyProperty DropShadowColorProperty =
            DependencyProperty.Register("DropShadowColor", typeof(Color), typeof(CustomWindow), new PropertyMetadata(Colors.Black));

        public static readonly DependencyProperty TitleBarBackgroundProperty =
            DependencyProperty.Register("TitleBarBackground", typeof(Color), typeof(CustomWindow), new PropertyMetadata(Colors.Purple));
        public static readonly DependencyProperty TitleBarForegroundProperty =
            DependencyProperty.Register("TitleBarForeground", typeof(Color), typeof(CustomWindow), new PropertyMetadata(Colors.AntiqueWhite));
        public static readonly DependencyProperty WindowButtonHoverBackgroundProperty =
            DependencyProperty.Register("WindowButtonHoverBackground", typeof(Color), typeof(CustomWindow), new PropertyMetadata(Colors.LightGray));
        public static readonly DependencyProperty WindowButtonHoverForegroundProperty =
            DependencyProperty.Register("WindowButtonHoverForeground", typeof(Color), typeof(CustomWindow), new PropertyMetadata(Colors.Black));
        public static readonly DependencyProperty CloseButtonHoverBackgroundProperty =
            DependencyProperty.Register("CloseButtonHoverBackground", typeof(Color), typeof(CustomWindow), new PropertyMetadata(Colors.Red));
        public static readonly DependencyProperty CloseButtonHoverForegroundProperty =
            DependencyProperty.Register("CloseButtonHoverForeground", typeof(Color), typeof(CustomWindow), new PropertyMetadata(Colors.White));

        static CustomWindow()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CustomWindow), new FrameworkPropertyMetadata(typeof(CustomWindow)));
        }

        public override void BeginInit()
        {
            base.BeginInit();
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            WindowChrome winChrome = new WindowChrome
            {
                ResizeBorderThickness = new Thickness(ResizeBorder + OuterMargin),
                CaptionHeight = TitleBarHeight - 2.5, // Minus 2.5 to correct for drag area inaccuracy
                CornerRadius = new CornerRadius(CornerRadius * 2), // Corner Radius for drop shadow
                GlassFrameThickness = new Thickness(0)
            };
            WindowChrome.SetWindowChrome(this, winChrome);

            _dropShadowBorder = (Border)GetTemplateChild("PART_DropShadowBorder");
            _dropShadowBorder.Padding = new Thickness(OuterMargin);

            _windowBorder = (Border)GetTemplateChild("PART_WindowBorder");
            _windowBorder.CornerRadius = new CornerRadius(CornerRadius);

            _titleBar = (Border)GetTemplateChild("PART_TitleBar");
            _titleBar.CornerRadius = new CornerRadius(CornerRadius, CornerRadius, 0, 0);

            _title = (TextBlock)GetTemplateChild("PART_Title");

            GetTemplateChild("PART_TitleBarHeight").SetValue(RowDefinition.HeightProperty, new GridLength(TitleBarHeight));
            GetTemplateChild("PART_DropShadow").SetValue(DropShadowEffect.ShadowDepthProperty, DropShadowDepth);
            GetTemplateChild("PART_DropShadow").SetValue(DropShadowEffect.OpacityProperty, DropShadowOpacity);
            GetTemplateChild("PART_DropShadow").SetValue(DropShadowEffect.BlurRadiusProperty, DropShadowBlurRadius);
            GetTemplateChild("PART_DropShadow").SetValue(DropShadowEffect.ColorProperty, DropShadowColor);

            ConfigureWindowButton();
            ConfigureWindowColor();

            StateChanged += CustomWindow_StateChanged;
        }


        public Color TitleBarBackground
        {
            get => (Color)GetValue(TitleBarBackgroundProperty);
            set => SetValue(TitleBarBackgroundProperty, value);
        }
        public Color TitleBarForeground
        {
            get => (Color)GetValue(TitleBarForegroundProperty);
            set => SetValue(TitleBarForegroundProperty, value);
        }
        public Color WindowButtonHoverBackground
        {
            get => (Color)GetValue(WindowButtonHoverBackgroundProperty);
            set => SetValue(WindowButtonHoverBackgroundProperty, value);
        }
        public Color WindowButtonHoverForeground
        {
            get => (Color)GetValue(WindowButtonHoverForegroundProperty);
            set => SetValue(WindowButtonHoverForegroundProperty, value);
        }
        public Color CloseButtonHoverBackground
        {
            get => (Color)GetValue(CloseButtonHoverBackgroundProperty);
            set => SetValue(CloseButtonHoverBackgroundProperty, value);
        }
        public Color CloseButtonHoverForeground
        {
            get => (Color)GetValue(CloseButtonHoverForegroundProperty);
            set => SetValue(CloseButtonHoverForegroundProperty, value);
        }


        public double ResizeBorder
        {
            get => (double)GetValue(ResizeBorderProperty);
            set => SetValue(ResizeBorderProperty, value);
        }
        public double OuterMargin
        {
            get => (double)GetValue(OuterMarginProperty);
            set => SetValue(OuterMarginProperty, value);
        }
        public double TitleBarHeight
        {
            get => (double)GetValue(TitleBarHeightProperty);
            set => SetValue(TitleBarHeightProperty, value);
        }
        public double CornerRadius
        {
            get => (double)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }
        public double DropShadowDepth
        {
            get => (double)GetValue(DropShadowDepthProperty);
            set => SetValue(DropShadowDepthProperty, value);
        }
        public double DropShadowOpacity
        {
            get => (double)GetValue(DropShadowOpacityProperty);
            set => SetValue(DropShadowOpacityProperty, value);
        }
        public double DropShadowBlurRadius
        {
            get => (double)GetValue(DropShadowBlurRadiusProperty);
            set => SetValue(DropShadowBlurRadiusProperty, value);
        }
        public Color DropShadowColor
        {
            get => (Color)GetValue(DropShadowColorProperty);
            set => SetValue(DropShadowColorProperty, value);
        }


        private void ConfigureWindowButton()
        {
            _windowIconButton = (Button)GetTemplateChild("PART_WindowIconButton");
            _windowIconButton.Background = new ImageBrush(Icon);
            _windowIconButton.Height = TitleBarHeight;
            _windowIconButton.Width = TitleBarHeight;
            _windowIconButton.Click += WindowIcon_Click;

            _minimizeButton = (Button)GetTemplateChild("PART_MinimizeButton");
            _minimizeButton.MouseEnter += MinimizeButton_MouseEnter;
            _minimizeButton.MouseLeave += MinimizeButton_MouseLeave;
            _minimizeButton.Click += MinimizeButton_Click;

            _maximizeButton = (Button)GetTemplateChild("PART_MaximizeButton");
            _maximizeButton.MouseEnter += MaximizeButton_MouseEnter;
            _maximizeButton.MouseLeave += MaximizeButton_MouseLeave;
            _maximizeButton.Click += MaximizeButton_Click;

            _closeButton = (Button)GetTemplateChild("PART_CloseButton");
            _closeButton.MouseEnter += CloseButton_MouseEnter;
            _closeButton.MouseLeave += CloseButton_MouseLeave;
            _closeButton.Click += CloseButton_Click;
        }

        private void WindowIcon_Click(object sender, RoutedEventArgs e)
        {
            SystemCommands.ShowSystemMenu(this, GetMousePosition());
        }
        private Point GetMousePosition()
        {
            Point mousePosRelativeToWindow = Mouse.GetPosition(this);
            if (WindowState == WindowState.Maximized)
            {
                return new Point(mousePosRelativeToWindow.X - _windowEdgeMargin, mousePosRelativeToWindow.Y - _windowEdgeMargin);
            }
            else
            {
                return new Point(mousePosRelativeToWindow.X + Left, mousePosRelativeToWindow.Y + Top);
            }
        }

        private void MinimizeButton_MouseEnter(object sender, MouseEventArgs e)
        {
            _minimizeButton.SetValue(BackgroundProperty, new SolidColorBrush(WindowButtonHoverBackground));
            _minimizeButton.SetValue(ForegroundProperty, new SolidColorBrush(WindowButtonHoverForeground));
        }
        private void MinimizeButton_MouseLeave(object sender, MouseEventArgs e)
        {
            _minimizeButton.SetValue(BackgroundProperty, new SolidColorBrush(Colors.Transparent));
            _minimizeButton.SetValue(ForegroundProperty, new SolidColorBrush(TitleBarForeground));
        }
        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            SystemCommands.MinimizeWindow(this);
        }

        private void MaximizeButton_MouseEnter(object sender, MouseEventArgs e)
        {
            _maximizeButton.SetValue(BackgroundProperty, new SolidColorBrush(WindowButtonHoverBackground));
            _maximizeButton.SetValue(ForegroundProperty, new SolidColorBrush(WindowButtonHoverForeground));
        }
        private void MaximizeButton_MouseLeave(object sender, MouseEventArgs e)
        {
            _maximizeButton.SetValue(BackgroundProperty, new SolidColorBrush(Colors.Transparent));
            _maximizeButton.SetValue(ForegroundProperty, new SolidColorBrush(TitleBarForeground));
        }
        private void MaximizeButton_Click(object sender, RoutedEventArgs e)
        {
            if (WindowState == WindowState.Maximized)
            {
                SystemCommands.RestoreWindow(this);
            }
            else
            {
                SystemCommands.MaximizeWindow(this);
            }
        }

        private void CloseButton_MouseEnter(object sender, MouseEventArgs e)
        {
            _closeButton.SetValue(BackgroundProperty, new SolidColorBrush(CloseButtonHoverBackground));
            _closeButton.SetValue(ForegroundProperty, new SolidColorBrush(CloseButtonHoverForeground));
        }
        private void CloseButton_MouseLeave(object sender, MouseEventArgs e)
        {
            _closeButton.SetValue(BackgroundProperty, new SolidColorBrush(Colors.Transparent));
            _closeButton.SetValue(ForegroundProperty, new SolidColorBrush(TitleBarForeground));
        }
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            SystemCommands.CloseWindow(this);
        }


        private void ConfigureWindowColor()
        {
            _titleBar.Background = new SolidColorBrush(TitleBarBackground);

            _title.Foreground = new SolidColorBrush(TitleBarForeground);
            _minimizeButton.Foreground = new SolidColorBrush(TitleBarForeground);
            _maximizeButton.Foreground = new SolidColorBrush(TitleBarForeground);
            _closeButton.Foreground = new SolidColorBrush(TitleBarForeground);
            _windowIconButton.Foreground = new SolidColorBrush(TitleBarForeground);
        }


        private void CustomWindow_StateChanged(object sender, EventArgs e)
        {
            if (WindowState == WindowState.Maximized)
            {
                _titleBar.CornerRadius = new CornerRadius(0);
                _windowBorder.CornerRadius = new CornerRadius(0);
                _dropShadowBorder.Padding = new Thickness(_windowEdgeMargin); // Hack to correct for window being too big

                WindowChrome winChrome = new WindowChrome
                {
                    ResizeBorderThickness = new Thickness(ResizeBorder + 2),
                    CaptionHeight = TitleBarHeight - 2.5, // Minus 2.5 to correct for drag area inaccuracy
                    CornerRadius = new CornerRadius(CornerRadius * 2), // Corner Radius for drop shadow
                    GlassFrameThickness = new Thickness(0)
                };
                WindowChrome.SetWindowChrome(this, winChrome);
            }
            else if (WindowState == WindowState.Normal)
            {
                _titleBar.CornerRadius = new CornerRadius(CornerRadius, CornerRadius, 0, 0);
                _windowBorder.CornerRadius = new CornerRadius(CornerRadius);
                _dropShadowBorder.Padding = new Thickness(OuterMargin);

                WindowChrome winChrome = new WindowChrome
                {
                    ResizeBorderThickness = new Thickness(ResizeBorder + OuterMargin),
                    CaptionHeight = TitleBarHeight - 2.5, // Minus 2.5 to correct for drag area inaccuracy
                    CornerRadius = new CornerRadius(CornerRadius * 2), // Corner Radius for drop shadow
                    GlassFrameThickness = new Thickness(0)
                };
                WindowChrome.SetWindowChrome(this, winChrome);
            }
        }
    }
}
