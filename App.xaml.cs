using Microsoft.Extensions.DependencyInjection;

namespace PM2E124580053
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            MainPage = new NavigationPage(new MainPage());
        }
    }
}