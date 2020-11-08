using System.Threading.Tasks;
using ZenMvvmSampleApp.Models;
using ZenMvvm;
using ZenMvvm.Helpers;

namespace ZenMvvmSampleApp.ViewModels
{
    //ZM: Implements IOnViewNavigated so that this ViewModel can receive data
    public class ItemDetailViewModel : ViewModelBase, IOnViewNavigated<Item>
    {
        Item item;
        public Item Item
        {
            get => item;
            //ZM: SetProperty is provided in the ViewModelBase
            set => SetProperty(ref item, value);
        }

        //ZM: Triggered once the attached view has complete navigation
        // Asynchronous code welcome.
        public Task OnViewNavigatedAsync(Item item)
        {
            Title = item?.Text;
            Item = item;

            return Task.CompletedTask;
        }
    }
}
