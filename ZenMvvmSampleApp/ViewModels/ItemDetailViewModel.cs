using System.Threading.Tasks;
using ZenMvvmSampleApp.Models;
using ZenMvvm;
using ZenMvvm.Helpers;

namespace ZenMvvmSampleApp.ViewModels
{
    public class ItemDetailViewModel : ViewModelBase, IOnViewNavigated<Item>
    {
        Item item;
        public Item Item
        {
            get => item;
            set => SetProperty(ref item, value);
        }

        public Task OnViewNavigatedAsync(Item item)
        {
            Title = item?.Text;
            Item = item;

            return Task.CompletedTask;
        }
    }
}
