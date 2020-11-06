using System;
using System.Threading.Tasks;
using ZenMvvmSampleApp.Models;
using ZenMvvm;
using System.Windows.Input;
using ZenMvvm.Helpers;
using ZenMvvmSampleApp.Services;

namespace ZenMvvmSampleApp.ViewModels
{
    public class ItemsViewModel : ViewModelBase, IOnViewAppearing
    {
        public ObservableRangeCollection<Item> Items { get; }
            = new ObservableRangeCollection<Item>();

        public ICommand LoadItemsCommand { get; }
        public ICommand AddItemCommand { get; }
        public ICommand OnItemSelectedCommand { get; }

        public ItemsViewModel(
            INavigationService navigationService,
            IDataStore<Item> dataStore,
            ISafeMessagingCenter messagingCenter)
        {
            Title = "Browse";

            messagingCenter.Subscribe<NewItemViewModel, Item>(
                this, "AddItem", async (obj, item) =>
            {
                var newItem = item;
                Items.Add(newItem);
                await dataStore.AddItemAsync(newItem);
            });

            LoadItemsCommand = new SafeCommand(
                LoadItemsAsync
                , viewModel:this); //NB to use OneWay Binding in RefreshView
            async Task LoadItemsAsync()
                => Items.ReplaceRange(await dataStore.GetItemsAsync(true));

            AddItemCommand = new SafeCommand(AddItemAsync);
            async Task AddItemAsync() =>
                await navigationService.PushAsync<NewItemViewModel>();

            OnItemSelectedCommand = new SafeCommand<Item>(OnItemSelectedAsync);
            async Task OnItemSelectedAsync(Item item) =>
                await navigationService.PushAsync<ItemDetailViewModel, Item>(item);

        }
        
        public void OnViewAppearing(object sender, EventArgs e)
        {
            if (Items.Count == 0)
                LoadItemsCommand.Execute(null);
        }
    }
}