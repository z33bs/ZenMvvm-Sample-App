using System;
using Xamarin.Forms;
using ZenMvvm.Helpers;

namespace TestMyMvvm
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            //todo writeup the important setup of SafeExecutionHelpers or break if not set            
            SafeExecutionHelpers.SetDefaultExceptionHandler(
                (ex) =>
                {
                    //todo put this example somewhere... nice
                    System.Diagnostics.Debug.WriteLine(ex.Source + ": " + ex.Message);
                    Exception innerException = ex.InnerException;
                    int count = 0;
                    while (innerException != null)
                    {
                        count++;
                        for (int i = 0; i < count; i++)
                            System.Diagnostics.Debug.Write(" ");

                        System.Diagnostics.Debug.WriteLine("> " + innerException.Source + ": " + innerException.Message);
                        innerException = innerException.InnerException;
                    }
                });



            //Uncomment code below to try external 3rd party containers
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
        //    var container = new LightInject.ServiceContainer();
        //    container.Register<IDataStore<Item>, MockDataStore>();
        //    container.Register<INavigationService, NavigationService>();
        //    container.Register<ItemsViewModel>();
        //    container.Register<AboutViewModel>();
        //    container.Register<ItemDetailViewModel>();
        //    container.Register<NewItemViewModel>();

        //    ViewModelLocator.ContainerImplementation = new IIocAdapter(container, nameof(LightInject.ServiceContainer.GetInstance));
        //}

        //void UseAutofac()
        //{
        //    //Autofac (5.2.0)
        //    //One of the most flexible and feature rich Di engines
        //    //add using Autofac; to the top of the class
        //    var containerBuilder = new Autofac.ContainerBuilder();
        //    containerBuilder.RegisterType<MockDataStore>().As<IDataStore<Item>>();
        //    containerBuilder.RegisterType<NavigationService>().As<INavigationService>();
        //    containerBuilder.RegisterType<ItemsViewModel>();
        //    containerBuilder.RegisterType<AboutViewModel>();
        //    containerBuilder.RegisterType<ItemDetailViewModel>();
        //    containerBuilder.RegisterType<NewItemViewModel>();

        //    ViewModelLocator.ContainerImplementation = new IIocAdapter(containerBuilder.Build(), typeof(ResolutionExtensions), nameof(ResolutionExtensions.Resolve));
        //}
    }
}
