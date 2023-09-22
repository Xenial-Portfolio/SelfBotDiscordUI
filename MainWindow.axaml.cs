using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using System;
using SelfBotDiscordUI.Bot.Styles;
using DSharpPlus;
using SelfBotDiscordUI.Bot.Logs;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Threading;
using System.Collections.ObjectModel;

namespace SelfBotDiscordUI
{
    public partial class MainWindow : Window
    {
        public static Button? Button1, Button2, Button3, Button4, Button5, Button6, Button7, Button8, Button9, Button10;
        public static CheckBox? CheckBox1, CheckBox2, CheckBox3, CheckBox4, EnableSelfBotToggle, EnableAdvancedLoggerToggle;
        public static ListBox? MessageBoxCollection;
        public static TextBlock? LogDateLabel;

        public static bool IsSelfBotEnabled = true, IsAdvancedLoggingEnabled = true, IsDisconnected;

        public static ObservableCollection<string> Messages = new ObservableCollection<string>();
        public static DiscordClient DiscordClient;

        private Thread _discordThread, _checkTogglesThread;

        private const string Token = "TokenHere";

        public MainWindow()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
            WindowStyles.MainWindowStyle(this);

            ClientSize = new Size(802, 425);
            DrawControls();

            _discordThread = new Thread(StartDiscordBot);
            _discordThread.Start();

            _checkTogglesThread = new Thread(CheckToggles);
            _checkTogglesThread.Start();
        }

        private void CheckToggles()
        {
            for (;;)
            {
                void ToggleHandler()
                {
                    IsSelfBotEnabled = EnableSelfBotToggle.IsChecked.Value;
                    IsAdvancedLoggingEnabled = EnableAdvancedLoggerToggle.IsChecked.Value;

                    if (!IsSelfBotEnabled)
                    {
                        DiscordClient.DisconnectAsync().ConfigureAwait(false);
                        IsDisconnected = true;
                    }
                    if (IsSelfBotEnabled && IsDisconnected)
                    {
                        DiscordClient.ReconnectAsync(true);
                        DiscordClient.ConnectAsync();
                        IsDisconnected = false;
                    }
                }
                Dispatcher.UIThread.InvokeAsync(ToggleHandler).ConfigureAwait(true);
                Thread.Sleep(15);
            }
        }

        private void StartDiscordBot()
        {
            AdvancedLogger.LoadAdvancedLoggerConfig();
            StartAsync().ConfigureAwait(false).GetAwaiter().GetResult();
        }

        private static async Task StartAsync()
        {
            try
            {
                var clientConfig = new DiscordConfiguration
                {
                    LogLevel = LogLevel.Critical, // makes message logging to critical so you can collect all the info
                    Token = Token, // sets the token from config.json
                    TokenType = TokenType.User, // sets the bots token type to user if your using a user account
                    UseInternalLogHandler = true // this is set to true so we can use our own message logger
                };

                DiscordClient = new DiscordClient(clientConfig); // makes a new discord client and uses our client config
                await DiscordClient.ConnectAsync().ConfigureAwait(false); // await the connection from the discord client
                await AdvancedLogger.MessageLoggerAsync().ConfigureAwait(false); // Logs the new messages in a log file
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                System.Threading.Thread.Sleep(5000);
                Environment.Exit(0);
            }

            await Task.Delay(-1).ConfigureAwait(false);
        }

        private void DrawControls()
        {
            var panelColor = new SolidColorBrush(Color.FromArgb(255, 0, 102,204));
            var settingsPanel = this.FindControl<Panel>("SettingsPanel");
            settingsPanel.Background = panelColor;
            
            var togglesPanel = this.FindControl<Panel>("TogglesPanel");
            togglesPanel.Background = panelColor;

            Button1 = this.FindControl<Button>("Button1");
            Button1.Content = "button1";
            Button2 = this.FindControl<Button>("Button2");
            Button2.Content = "button2";
            Button3 = this.FindControl<Button>("Button3");
            Button3.Content = "button3";
            Button4 = this.FindControl<Button>("Button4");
            Button4.Content = "button4";
            Button5 = this.FindControl<Button>("Button5");
            Button5.Content = "button5";
            Button6 = this.FindControl<Button>("Button6");
            Button6.Content = "button6";
            Button7 = this.FindControl<Button>("Button7");
            Button7.Content = "button7";
            Button8 = this.FindControl<Button>("Button8");
            Button8.Content = "button8";
            Button9 = this.FindControl<Button>("Button9");
            Button9.Content = "button9";
            Button10 = this.FindControl<Button>("Button10");
            Button10.Content = "button10";

            CheckBox1 = this.FindControl<CheckBox>("CheckBox1");
            CheckBox1.Content = "checkBox1";
            CheckBox2 = this.FindControl<CheckBox>("CheckBox2");
            CheckBox2.Content = "checkBox2";
            CheckBox3 = this.FindControl<CheckBox>("CheckBox3");
            CheckBox3.Content = "checkBox3";
            CheckBox4 = this.FindControl<CheckBox>("CheckBox4");
            CheckBox4.Content = "checkBox4";

            MessageBoxCollection = this.FindControl<ListBox>("MessageBoxCollection");
            LogDateLabel = this.FindControl<TextBlock>("LogDateLabel");

            EnableSelfBotToggle = this.FindControl<CheckBox>("EnableSelfBotToggle");
            EnableAdvancedLoggerToggle = this.FindControl<CheckBox>("EnableAdvancedLoggerToggle");
        }
    }
}
