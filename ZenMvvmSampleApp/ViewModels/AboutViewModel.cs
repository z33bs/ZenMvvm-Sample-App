using System.Windows.Input;
using Xamarin.Essentials;
using ZenMvvm.Helpers;

namespace ZenMvvmSampleApp.ViewModels
{
    //ZM: No need to extend ViewModelBase if unnecessary
    public class AboutViewModel
    {
        public ICommand TapCommand { get; }

        public AboutViewModel()
        {
            //ZM: mustRunOnCurrentSyncContext forces the command to
            // execute on the UI thread
            TapCommand = new SafeCommand<string>(
                Launcher.OpenAsync,
                mustRunOnCurrentSyncContext: true);
        }
    }
}