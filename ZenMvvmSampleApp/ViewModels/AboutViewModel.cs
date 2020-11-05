using System.Windows.Input;
using Xamarin.Essentials;
using ZenMvvm.Helpers;

namespace ZenMvvmSampleApp.ViewModels
{
    public class AboutViewModel : ViewModelBase
    {
        public AboutViewModel()
        {
            Title = "About";
        }

        public ICommand TapCommand
            => new SafeCommand<string>(Launcher.OpenAsync, mustRunOnCurrentSyncContext: true);
    }
}