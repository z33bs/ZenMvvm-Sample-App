using System.Diagnostics;
using Xamarin.Forms;
using ZenMvvm.Helpers;

namespace ZenMvvmSampleApp
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            //ZM: SafeCommand setup -> Log unhandled exceptions,
            // preventing app crashes
            SafeExecutionHelpers.SetDefaultExceptionHandler(
                (ex) => Debug.WriteLine($"{ex.Source}: {ex.Message}"));

            //ZM: No need to regester dependencies because the
            // built-in injection engine will use smart-resolve by
            // default. This can be turned-off for enterprise apps

            //ZM: Wan't to use 3rd party dependecy injection instead?
            //... here are two examples
            //UseLightInject();
            //OR
            //UseAutofac();

            MainPage = new AppShell();
        }

        //___________ ALTERNATIVE DI/IOC CONTAINER IMPLEMENTATIONS

        //void UseLightInject()
        //{
        //    //LightInject (6.3.4)
        //    //One of the fastest Di engines
        //    //Use Nuget or add the following to the csproj file
        //    //<PackageReference Include = "LightInject" Version="6.3.4" />
        //    var container = new LightInject.ServiceContainer();
        //    container.Register<IDataStore<Item>, MockDataStore>();
        //    container.Register<INavigationService, NavigationService>();
        //    container.Register<ItemsViewModel>();
        //    container.Register<AboutViewModel>();
        //    container.Register<ItemDetailViewModel>();
        //    container.Register<NewItemViewModel>();
        //    ViewModelLocator.ContainerImplementation
        //        = new IIocAdapter(container, nameof(LightInject.ServiceContainer.GetInstance));
        //}

        //void UseAutofac()
        //{
        //    //Autofac (5.2.0)
        //    //One of the most flexible and feature rich Di engines
        //    //Use Nuget or add the following to the csproj file
        //    //<PackageReference Include = "Autofac" Version="5.2.0" />
        //    //add using Autofac; to the top of the class
        //    var containerBuilder = new Autofac.ContainerBuilder();
        //    containerBuilder.RegisterType<MockDataStore>().As<IDataStore<Item>>();
        //    containerBuilder.RegisterType<NavigationService>().As<INavigationService>();
        //    containerBuilder.RegisterType<ItemsViewModel>();
        //    containerBuilder.RegisterType<AboutViewModel>();
        //    containerBuilder.RegisterType<ItemDetailViewModel>();
        //    containerBuilder.RegisterType<NewItemViewModel>();
        //    ViewModelLocator.ContainerImplementation
        //        = new IIocAdapter(
        //            containerBuilder.Build(),
        //            typeof(ResolutionExtensions),
        //            nameof(ResolutionExtensions.Resolve));
        //}
    }
}
