using System.Threading.Tasks;
using System.Windows.Input;
using ZenMvvmSampleApp.Models;
using ZenMvvm;
using ZenMvvm.Helpers;

namespace ZenMvvmSampleApp.ViewModels
{
    public class NewItemViewModel
    {
        public readonly INavigationService navigationService;
        public readonly ISafeMessagingCenter messagingCenter;

        public NewItemViewModel(INavigationService navigationService, ISafeMessagingCenter messagingCenter)
        {
            this.navigationService = navigationService;
            this.messagingCenter = messagingCenter;

            Item = new Item
            {
                Text = "Item name",
                Description = "This is an item description."
            };

        }

        public Item Item { get; set; }

        ICommand saveCommand;
        public ICommand SaveCommand => saveCommand ??= new SafeCommand(SaveAsync);
        async Task SaveAsync()
        {
            messagingCenter.Send(this, "AddItem", Item);
            await navigationService.PopAsync();
        }

        ICommand cancelCommand;
        public ICommand CancelCommand => cancelCommand ??= new SafeCommand(CancelAsync);
        async void CancelAsync() =>
            await navigationService.PopAsync();       
    }
}
