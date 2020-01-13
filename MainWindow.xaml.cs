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
        //The WM_NCRBUTTONDOWN message is posted when the user presses the right mouse button while the cursor is within the nonclient area of a window.
        //This message is posted to the window that contains the cursor. If a window has captured the mouse, this message is not posted.
        private const uint WM_NCRBUTTONDOWN = 0xa4;

        //window message parameter for the hit test in the title bar
        private const uint HTCAPTION = 0x02;

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
            if ((msg == WM_NCRBUTTONDOWN) && (wParam.ToInt32() == HTCAPTION))
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
