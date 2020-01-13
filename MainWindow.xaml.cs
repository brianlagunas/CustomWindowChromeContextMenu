using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;

namespace CustomWindowChromeContextMenu
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //window message ID for the system menu
        private const uint WM_SYSTEMMENU = 0xa4;

        //window message parameter for the system menu
        private const uint WP_SYSTEMMENU = 0x02;

        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            IntPtr windowhandle = new WindowInteropHelper(this).Handle;
            HwndSource hwndSource = HwndSource.FromHwnd(windowhandle);
            hwndSource.AddHook(new HwndSourceHook(WndProc));
        }

        private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            //Message for the System Menu
            if ((msg == WM_SYSTEMMENU) && (wParam.ToInt32() == WP_SYSTEMMENU))
            {
                //Show our context menu
                ShowContextMenu();

                //prevent default context menu from appearing
                handled = true;
            }

            return IntPtr.Zero;
        }

        private void ShowContextMenu()
        {
            var contextMenu = Resources["contextMenu"] as ContextMenu;
            contextMenu.IsOpen = true;
        }

        private void ContextMenu_Click(object sender, RoutedEventArgs e)
        {
            var item = e.OriginalSource as MenuItem;
            MessageBox.Show($"{item.Header} was clicked");
        }
    }
}
