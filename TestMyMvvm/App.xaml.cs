using Xamarin.Forms;
using TestMyMvvm.Services;
using XamarinFormsMvvmAdaptor;
using TestMyMvvm.Models;
using XamarinFormsMvvmAdaptor.Helpers;

namespace TestMyMvvm
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            //todo writeup the important setup of SafeExecutionHelpers or break if not set
            SafeExecutionHelpers.SetDefaultExceptionHandler(
                (ex) => System.Diagnostics.Debug.WriteLine("SafeHelpers: " + ex.Message));
            //todo iesolate Ioc container
            ViewModelLocator.Ioc.Register<MockDataStore>().As<IDataStore<Item>>();
            ViewModelLocator.Ioc.Register<NavigationService>().As<INavigationService>();
            MainPage = new AppShell();
        }
    }
}
