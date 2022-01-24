using ScreenResolutionChange.Properties;

namespace ScreenResolutionChange
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Application.Run(new AppContext());
        }
    }

    public class AppContext : ApplicationContext
    {
        private readonly NotifyIcon trayIcon;

        public AppContext()
        {
            // Initialize Tray Icon
            trayIcon = new NotifyIcon()
            {
                Icon = Resources.icon,
                Text = "Change Resolution",
                ContextMenuStrip = new ContextMenuStrip()
            };

            var resolution = Resolution.GetCurrentResolution();

            trayIcon.ContextMenuStrip.Items.Add("1920 x 1080 @ 144Hz", resolution == ResolutionEnum.FullHD ? Resources.check : null, OnFullHdClick);
            trayIcon.ContextMenuStrip.Items.Add("2560 x 1440 @ 144Hz", resolution == ResolutionEnum.UHD ? Resources.check : null, On2kClick);          
            //trayIcon.ContextMenuStrip.Items.Add("Supported Modes", null, OnSupportedModesClick);
            trayIcon.ContextMenuStrip.Items.Add("Exit", null, OnExit);
            SetContextButtons(resolution);
            trayIcon.Visible = true;
        }

        //private void OnSupportedModesClick(object? sender, EventArgs e)
        //{
        //    Resolution.GetDisplayNames();
        //    MessageBox.Show(string.Join(',',Resolution.GetDisplayNames()));
        //}

        private void On2kClick(object? sender, EventArgs e)
        {
            Resolution.ChangeDisplaySettings(2560, 1440, 144, 1);
            SetContextButtons(ResolutionEnum.UHD);
        }

        private void OnFullHdClick(object? sender, EventArgs e)
        {
            Resolution.ChangeDisplaySettings(1920, 1080, 144, 0);
            SetContextButtons(ResolutionEnum.FullHD);
        }

        private void OnExit(object? sender, EventArgs e)
        {
            trayIcon.Visible = false;
            Application.Exit();
        }

        private void SetContextButtons(ResolutionEnum resolution)
        {
            if (resolution == ResolutionEnum.UHD)
            {
                trayIcon.ContextMenuStrip.Items[1].Image = Resources.check;
                trayIcon.ContextMenuStrip.Items[1].Enabled = false;
                trayIcon.ContextMenuStrip.Items[0].Image = null;
                trayIcon.ContextMenuStrip.Items[0].Enabled = true;
            }
            else if (resolution == ResolutionEnum.FullHD)
            {
                trayIcon.ContextMenuStrip.Items[0].Image = Resources.check;
                trayIcon.ContextMenuStrip.Items[0].Enabled = false;
                trayIcon.ContextMenuStrip.Items[1].Image = null;
                trayIcon.ContextMenuStrip.Items[1].Enabled = true;
            }
            else
            {
                trayIcon.ContextMenuStrip.Items[1].Image = null;
                trayIcon.ContextMenuStrip.Items[1].Enabled = true;
                trayIcon.ContextMenuStrip.Items[0].Image = null;
                trayIcon.ContextMenuStrip.Items[0].Enabled = true;
            }
        }
    }
}