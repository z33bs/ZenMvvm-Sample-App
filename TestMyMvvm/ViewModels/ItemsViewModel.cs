using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using TestMyMvvm.Models;
using ZenMvvm;
using System.Windows.Input;
using ZenMvvm.Helpers;
using TestMyMvvm.Services;

namespace TestMyMvvm.ViewModels
{
    public class ItemsViewModel : ViewModelBase, IOnViewAppearing
    {
        readonly INavigationService navigationService;
        readonly IDataStore<Item> dataStore;

        public ObservableCollection<Item> Items { get; set; }
        public ICommand LoadItemsCommand { get; set; }

        public ItemsViewModel(INavigationService navigationService, IDataStore<Item> dataStore)
        {
            this.navigationService = navigationService;
            this.dataStore = dataStore;
            Title = "Browse";
            Items = new ObservableCollection<Item>();
            LoadItemsCommand = new SafeCommand(ExecuteLoadItemsCommand, this); //todo Note to use OneWay in RefreshView

            SafeMessagingCenter.Subscribe<NewItemViewModel, Item>(this, "AddItem", async (obj, item) =>
            {
                var newItem = item as Item;
                Items.Add(newItem);
                await dataStore.AddItemAsync(newItem);
            });
        }

        async Task ExecuteLoadItemsCommand()
        {
            Items.Clear();
            var items = await dataStore.GetItemsAsync(true);
            foreach (var item in items)
            {
                Items.Add(item);
            }
        }

        ICommand addItemCommand;
        public ICommand AddItemCommand
            => addItemCommand ??= new SafeCommand(AddItemAsync);
        async Task AddItemAsync()
        {
            await navigationService.PushAsync<NewItemViewModel>();
        }

        ICommand onItemSelectedCommand;
        public ICommand OnItemSelectedCommand
            => onItemSelectedCommand ??= new SafeCommand<Item>(OnItemSelectedAsync);
        async Task OnItemSelectedAsync(Item item)
        {
            await navigationService.PushAsync<ItemDetailViewModel>(item);
        }

        public void OnViewAppearing(object sender, EventArgs e)
        {
            if (Items.Count == 0)
                LoadItemsCommand.Execute(null);
        }
    }
}