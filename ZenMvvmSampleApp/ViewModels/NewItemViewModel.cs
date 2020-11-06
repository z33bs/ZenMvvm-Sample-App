using System.Threading.Tasks;
using System.Windows.Input;
using ZenMvvmSampleApp.Models;
using ZenMvvm;
using ZenMvvm.Helpers;

namespace ZenMvvmSampleApp.ViewModels
{
    public class NewItemViewModel
    {
        public Item Item { get; set; }

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        public NewItemViewModel(
            INavigationService navigationService,
            ISafeMessagingCenter messagingCenter)
        {
            Item = new Item
            {
                Text = "Item name",
                Description = "This is an item description."
            };

            SaveCommand = new SafeCommand(SaveAsync);
            async Task SaveAsync()
            {
                messagingCenter.Send(this, "AddItem", Item);
                await navigationService.PopAsync();
            };

            CancelCommand = new SafeCommand(CancelAsync);
            async void CancelAsync() =>
                await navigationService.PopAsync();
        }
    }
}