using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;

using Xamarin.Forms;

using TestMyMvvm.Models;
using TestMyMvvm.Views;
using XamarinFormsMvvmAdaptor;
using System.Windows.Input;
using XamarinFormsMvvmAdaptor.Helpers;
using TestMyMvvm.Services;

namespace TestMyMvvm.ViewModels
{
    public class ItemsViewModel : ViewModelBase, IOnViewAppearing
    {
        readonly INavigationService navigationService;
        readonly IDataStore<Item> dataStore;

        public ObservableCollection<Item> Items { get; set; }
        public Command LoadItemsCommand { get; set; }

        public ItemsViewModel(INavigationService navigationService, IDataStore<Item> dataStore)
        {
            this.navigationService = navigationService;
            this.dataStore = dataStore;
            Title = "Browse";
            Items = new ObservableCollection<Item>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());

            MessagingCenter.Subscribe<NewItemViewModel, Item>(this, "AddItem", async (obj, item) =>
            {
                var newItem = item as Item;
                Items.Add(newItem);
                await dataStore.AddItemAsync(newItem);
            });
        }

        async Task ExecuteLoadItemsCommand()
        {
            IsBusy = true;

            try
            {
                Items.Clear();
                var items = await dataStore.GetItemsAsync(true);
                foreach (var item in items)
                {
                    Items.Add(item);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        ICommand addItemCommand;
        public ICommand AddItemCommand => addItemCommand ??= new SafeCommand(AddItemAsync);
        async Task AddItemAsync()
        {
            //todo PushModalAsync gives option to wrap in NavigationPage
            await navigationService.PushAsync<NewItemViewModel>();
        }

        ICommand onItemSelectedCommand;
        public ICommand OnItemSelectedCommand => onItemSelectedCommand ??= new SafeCommand<Item>(OnItemSelectedAsync);
        async Task OnItemSelectedAsync(Item item)
        {
            await navigationService.PushAsync<ItemDetailViewModel>(item);
        }

        public void OnViewAppearing(object sender, EventArgs e)
        {
            if (Items.Count == 0)
                IsBusy = true;
        }
    }
}