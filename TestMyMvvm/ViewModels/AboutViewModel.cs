using System;
using System.Windows.Input;
using Xamarin.Essentials;
//using Xamarin.Forms;
using XamarinFormsMvvmAdaptor.Helpers;
namespace TestMyMvvm.ViewModels
{
    public class AboutViewModel : ViewModelBase
    {
        public AboutViewModel()
        {
            Title = "About";
            OpenWebCommand = new SafeCommand(async () => await Browser.OpenAsync("https://xamarin.com"),mustRunOnCurrentSyncContext:true);
        }

        public ICommand OpenWebCommand { get; }
    }
}