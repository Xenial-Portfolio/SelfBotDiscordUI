using DSharpPlus.EventArgs;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Reactive.Subjects;
using Avalonia;
using Avalonia.Controls;

namespace SelfBotDiscordUI.Bot.Logs
{
    public class AdvancedLogger
    {
        private static string _path = @$"/home/admin/Documents/Logs/LogOf_{DateTime.Now.Year}-{DateTime.Now.Month}-{DateTime.Now.Day}.txt"; // Path of the log file
        public static Subject<ObservableCollection<string>> Source = new Subject<ObservableCollection<string>>();

        public static void LoadAdvancedLoggerConfig()
        {
            try
            {
                _path = @$"/home/admin/Documents/Logs/LogOf_{DateTime.Now.Year}-{DateTime.Now.Month}-{DateTime.Now.Day}.txt"; // Path of the log file
                if (File.Exists(_path)) return;
                
                Directory.CreateDirectory(@$"/home/admin/Documents/Logs"); // Creates a new Directory for the log files
                File.WriteAllText(_path, ""); // Makes a new log file
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public static Task MessageLoggerAsync()
        {
            if (File.Exists(_path)) MainWindow.DiscordClient.MessageCreated += OnDiscordClientOnMessageCreatedAsync;
            return Task.CompletedTask;
        }

        private static Task OnDiscordClientOnMessageCreatedAsync(MessageCreateEventArgs e)
        {
            LoadAdvancedLoggerConfig();

            void UpdateList()
            {
                MainWindow.MessageBoxCollection.Bind(ItemsControl.ItemsProperty, Source);
                MainWindow.Messages.Add(MessageLog(e));

                Source.OnNext(MainWindow.Messages);
                MainWindow.LogDateLabel.Text = $"Date: {DateTime.Now.Year}-{DateTime.Now.Month}-{DateTime.Now.Day}";
                
                if (MainWindow.IsAdvancedLoggingEnabled)
                    File.AppendAllText(_path, MessageLog(e)); // writes the message to the file
            }

            Avalonia.Threading.Dispatcher.UIThread.InvokeAsync(UpdateList);
            return Task.CompletedTask;
        }

        public static string MessageLog(MessageCreateEventArgs e)
        {
            return $"[{DateTime.Now.ToLongDateString()} : {DateTime.Now.ToShortTimeString()}] {e.Author.Username}#{e.Author.Discriminator} " +
                   $"| Message: \"{e.Message.Content}\" \n"; // Template for our logged message
        }
    }
}