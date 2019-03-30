using System;
using Avalonia;
using Avalonia.Logging.Serilog;
using Lingua.Demo.ViewModels;
using Lingua.Demo.Views;

namespace Lingua.Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            BuildAvaloniaApp().Start<MainWindow>(() => new MainWindowViewModel());
        }

        public static AppBuilder BuildAvaloniaApp()
            => AppBuilder.Configure<App>()
                .UsePlatformDetect()
                .UseReactiveUI()
                .LogToDebug();
    }
}
