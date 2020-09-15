using System.Threading.Tasks;
using TestMyMvvm.Models;
using XamarinFormsMvvmAdaptor;
using XamarinFormsMvvmAdaptor.Helpers;

namespace TestMyMvvm.ViewModels
{
    public class ItemDetailViewModel : ViewModelBase, IOnViewNavigated
    {
        Item item;
        public Item Item
        {
            get => item;
            set => SetProperty(ref item, value);
        }

        public Task OnViewNavigatedAsync(object navigationData)
        {
            var item = navigationData as Item;
            Title = item?.Text;
            Item = item;

            return Task.CompletedTask;
        }
    }
}
