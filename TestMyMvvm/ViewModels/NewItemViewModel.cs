using System.Threading.Tasks;
using System.Windows.Input;
using TestMyMvvm.Models;
using XamarinFormsMvvmAdaptor;
using XamarinFormsMvvmAdaptor.Helpers;

namespace TestMyMvvm.ViewModels
{
    public class NewItemViewModel
    {
        public readonly INavigationService navigationService;

        public NewItemViewModel(INavigationService navigationService)
        {
            this.navigationService = navigationService;

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
            SafeMessagingCenter.Send(this, "AddItem", Item);
            await navigationService.PopAsync();
        }

        ICommand cancelCommand;
        public ICommand CancelCommand => cancelCommand ??= new SafeCommand(CancelAsync);
        async void CancelAsync()
        {
            await navigationService.PopAsync();
        }

    }
}
