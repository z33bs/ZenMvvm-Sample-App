using System.Windows.Input;
using Xamarin.Essentials;
using ZenMvvm.Helpers;

namespace ZenMvvmSampleApp.ViewModels
{
    public class AboutViewModel : ViewModelBase
    {
        public ICommand TapCommand { get; }

        public AboutViewModel()
        {
            Title = "About";
            TapCommand = new SafeCommand<string>(
                Launcher.OpenAsync,
                mustRunOnCurrentSyncContext: true);
        }
    }
}