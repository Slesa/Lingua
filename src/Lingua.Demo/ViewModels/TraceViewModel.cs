using System.Reactive;
using ReactiveUI;
using System;

namespace Lingua.Demo.ViewModels
{
    public class TraceViewModel : ViewModelBase
    {
        public TraceViewModel()
        {
            OnClearCommand = ReactiveCommand.Create(OnClear);
            LinguaTrace.TraceSource.Listeners.Add(new LinguaTraceListener(null, WriteTraceLine));
        }

        string _traceText;
        public string TraceText { 
            get { return _traceText; } 
            private set { this.RaiseAndSetIfChanged(ref _traceText, value); }
        }
        public ReactiveCommand<Unit, Unit> OnClearCommand { get; }

        void WriteTraceLine(string text)
        {
            TraceText += text + Environment.NewLine;
        }

        void OnClear()
        {
            TraceText = "";
        }
    }
}