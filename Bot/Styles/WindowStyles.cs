using Avalonia.Controls;
using Avalonia.Media;

namespace SelfBotDiscordUI.Bot.Styles
{
    public static class WindowStyles
    {
        public static void MainWindowStyle(Window window)
        {
            window.CanResize = false;
            window.Background = new SolidColorBrush(Color.FromArgb(255, 0, 120, 215));
            window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }
    }
}
