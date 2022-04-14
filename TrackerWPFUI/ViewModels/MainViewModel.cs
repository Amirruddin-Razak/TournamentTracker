using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Shell;
using TrackerWPFUI.Commands;
using TrackerWPFUI.Stores;
using TrackerWPFUI.ViewModels.Base;

namespace TrackerWPFUI.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private NavigationStore _navigationStore;

        public MainViewModel(Window window, NavigationStore navigationStore)
        {
            _navigationStore = navigationStore;

            _navigationStore.CurrentViewModelChanged += NavigationStore_CurrentViewModelChanged;


            #region Custom Window Settings
            _window = window;

            MinimizeWindowCommand = new RelayCommand(MinimizeWindow);
            MaximizeWindowCommand = new RelayCommand(MaximizeWindow);
            CloseWindowCommand = new RelayCommand(CloseWindow);
            WindowMenuCommand = new RelayCommand(WindowMenu);

            _window.StateChanged += Window_StateChanged;
            #endregion
        }

        public ViewModelBase CurrentViewModel => _navigationStore.CurrentViewModel;

        private void NavigationStore_CurrentViewModelChanged()
        {
            OnPropertyChanged(nameof(CurrentViewModel));
        }


        #region Custom Windo Settings
        private readonly Window _window;
        private readonly double _cornerRadius = 8;
        private readonly double _resizeBorder = 4;
        private readonly double _outerMargin = 6;
        private readonly double _titleBarHeight = SystemParameters.CaptionHeight;

        public WindowChrome WinChrome
        {
            get
            {
                WindowChrome windowChrome = new WindowChrome();

                windowChrome.ResizeBorderThickness = new Thickness(_resizeBorder + _outerMargin);
                windowChrome.CaptionHeight = _titleBarHeight - 2.5; // Minus 2.5 to correct for drag area inaccuracy
                windowChrome.CornerRadius = new CornerRadius(_cornerRadius * 2); // Corner Radius for drop shadow
                windowChrome.GlassFrameThickness = new Thickness(0);

                return windowChrome;
            }
        }
        public int MinWindowHeight { get; } = 350;
        public int MinWindowWidth { get; } = 350;
        public Thickness OuterMarginThickness => new Thickness(_window.WindowState == WindowState.Maximized ? 6 : _outerMargin);
        public CornerRadius WindowCornerRadius => new CornerRadius(_window.WindowState == WindowState.Maximized ? 0 : _cornerRadius);
        public CornerRadius OpacityMaskCornerRadius
        {
            get
            {
                if (_window.WindowState == WindowState.Maximized)
                {
                    return new CornerRadius(0);
                }
                else
                {
                    return new CornerRadius(_cornerRadius, _cornerRadius, 0, 0);
                }
            }
        }
        public GridLength TitleBarHeightGridLength => new GridLength(_titleBarHeight);

        public RelayCommand MinimizeWindowCommand { get; set; }
        public RelayCommand MaximizeWindowCommand { get; set; }
        public RelayCommand CloseWindowCommand { get; set; }
        public RelayCommand WindowMenuCommand { get; set; }

        private void Window_StateChanged(object sender, EventArgs e)
        {
            OnPropertyChanged(nameof(WindowCornerRadius));
            OnPropertyChanged(nameof(OpacityMaskCornerRadius));
            OnPropertyChanged(nameof(OuterMarginThickness));
        }

        private void CloseWindow(object parameter) => SystemCommands.CloseWindow(_window);
        private void MinimizeWindow(object parameter) => SystemCommands.MinimizeWindow(_window);
        private void MaximizeWindow(object parameter)
        {
            if (_window.WindowState == WindowState.Maximized)
            {
                SystemCommands.RestoreWindow(_window);
            }
            else
            {
                SystemCommands.MaximizeWindow(_window);
            }
        }
        private void WindowMenu(object parameter) => SystemCommands.ShowSystemMenu(_window, GetMousePosition());

        private Point GetMousePosition()
        {
            Point mousePosRelativeToWindow = Mouse.GetPosition(_window);
            return new Point(mousePosRelativeToWindow.X + _window.Left, mousePosRelativeToWindow.Y + _window.Top);
        }
        #endregion
    }
}
