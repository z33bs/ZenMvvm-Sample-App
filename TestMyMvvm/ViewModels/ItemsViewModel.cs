using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using TestMyMvvm.Models;
using ZenMvvm;
using System.Windows.Input;
using ZenMvvm.Helpers;
using TestMyMvvm.Services;
using System.Diagnostics;
using Xamarin.Forms;

namespace TestMyMvvm.ViewModels
{
    public class ItemsViewModel : ViewModelBase, IOnViewAppearing
    {
        readonly INavigationService navigationService;

        public ObservableRangeCollection<Item> Items { get; } = new ObservableRangeCollection<Item>();
        public ICommand LoadItemsCommand { get; set; }

        public ItemsViewModel(INavigationService navigationService, IDataStore<Item> dataStore, ISafeMessagingCenter messagingCenter)
        {
            this.navigationService = navigationService;
            Title = "Browse";
            LoadItemsCommand = new SafeCommand(
                async () => Items.ReplaceRange(await dataStore.GetItemsAsync(true))
                    , this); //todo Note to use OneWay in RefreshView

            messagingCenter.Subscribe<NewItemViewModel, Item>(this, "AddItem", async (obj, item) =>
            {
                var newItem = item as Item;
                Items.Add(newItem);
                await dataStore.AddItemAsync(newItem);
            });
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