using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using TestMyMvvm.Services;
using TestMyMvvm.Views;
using XamarinFormsMvvmAdaptor;

namespace TestMyMvvm
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();

            DependencyService.Register<MockDataStore>();
            ViewModelLocator.Ioc.Register<NavigationService>().As<INavigationService>();
            MainPage = new AppShell();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
