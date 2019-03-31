using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Lingua.Demo.Views
{
    public class RunView : UserControl
    {
        public RunView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
