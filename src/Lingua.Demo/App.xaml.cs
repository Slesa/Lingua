using Avalonia;
using Avalonia.Markup.Xaml;

namespace Lingua.Demo
{
    public class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
