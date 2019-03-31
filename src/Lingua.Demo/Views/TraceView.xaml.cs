using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Lingua.Demo.Views
{
    public class TraceView : UserControl
    {
        public TraceView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
