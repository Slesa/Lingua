using System.Reactive;
using ReactiveUI;
using System;

namespace Lingua.Demo.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public MainWindowViewModel()
        {
            OnFileExitCommand = ReactiveCommand.Create(OnFileExit);
            TraceViewModel = new TraceViewModel();
            RunViewModel = new RunViewModel();
        }

        public RunViewModel RunViewModel { get; }
        public TraceViewModel TraceViewModel { get; }
        public ReactiveCommand<Unit, Unit> OnFileExitCommand { get; }

        void OnFileExit()
        {
            Environment.Exit(0);
        }
    }
}
