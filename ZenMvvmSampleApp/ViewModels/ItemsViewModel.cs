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
        readonly INavigationService navigationService;

        public ObservableRangeCollection<Item> Items { get; } = new ObservableRangeCollection<Item>();

        public ItemsViewModel(INavigationService navigationService, IDataStore<Item> dataStore, ISafeMessagingCenter messagingCenter)
        {
            this.navigationService = navigationService;

            Title = "Browse";

            messagingCenter.Subscribe<NewItemViewModel, Item>(this, "AddItem", async (obj, item) =>
            {
                var newItem = item;
                Items.Add(newItem);
                await dataStore.AddItemAsync(newItem);
            });

            LoadItemsCommand = new SafeCommand(
                async () => Items.ReplaceRange(await dataStore.GetItemsAsync(true))
                , viewModel:this); //NB to use OneWay Binding in RefreshView
        }

        public ICommand LoadItemsCommand { get; } //set in ctor       

        ICommand addItemCommand;
        public ICommand AddItemCommand
            => addItemCommand ??= new SafeCommand(AddItemAsync);
        async Task AddItemAsync() =>
            await navigationService.PushAsync<NewItemViewModel>();        

        ICommand onItemSelectedCommand;
        public ICommand OnItemSelectedCommand
            => onItemSelectedCommand ??= new SafeCommand<Item>(OnItemSelectedAsync);
        async Task OnItemSelectedAsync(Item item) =>
            await navigationService.PushAsync<ItemDetailViewModel, Item>(item);

        public void OnViewAppearing(object sender, EventArgs e)
        {
            if (Items.Count == 0)
                LoadItemsCommand.Execute(null);
        }
    }
}