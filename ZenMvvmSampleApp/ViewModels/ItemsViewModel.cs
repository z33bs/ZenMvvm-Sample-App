//ZM: No dependency on Xamarin.Forms in the Viewmodel
// makes unit testing easier
using System;
using System.Threading.Tasks;
using ZenMvvmSampleApp.Models;
using ZenMvvm;
using System.Windows.Input;
using ZenMvvm.Helpers;
using ZenMvvmSampleApp.Services;

namespace ZenMvvmSampleApp.ViewModels
{
    //ZM: Extending ViewModelBase gives us the Observable properties
    // "Title" and "IsBusy" which are used in this VM
    //ZM: Implementing IOnViewAppearing runs code in this viewmodel
    //  when the attached Xamarin.Forms.Page.Appearing event is fired
    public class ItemsViewModel : ViewModelBase, IOnViewAppearing
    {
        //ZM: Saves code by providing Items.ReplaceRange
        public ObservableRangeCollection<Item> Items { get; }
            = new ObservableRangeCollection<Item>();

        public ICommand LoadItemsCommand { get; }
        public ICommand AddItemCommand { get; }
        public ICommand OnItemSelectedCommand { get; }

        //ZM: Built-in dependency injection will resolve these services
        public ItemsViewModel(
            INavigationService navigationService,
            IDataStore<Item> dataStore,
            ISafeMessagingCenter messagingCenter)
        {
            Title = "Browse";

            //ZM: Note how we can avoid async-void-lamdas (async () => ) by
            // using SafeMessagingCenter and SafeCommand
            // Unhandled exceptions are also caught and handled in app.cs
            messagingCenter.Subscribe<NewItemViewModel, Item>(this, "AddItem", SaveNewItemAsync);
            async Task SaveNewItemAsync(NewItemViewModel vm, Item item)
            {
                Items.Add(item);
                await dataStore.AddItemAsync(item);
            };

            //ZM: Because we're extending ViewModelBase, the IsBusy property
            // will be set to true while LoadItemsCommand is running.
            // Alot of code is saved avoiding writing a t-c-f block with
            // IsBusy condition
            LoadItemsCommand = new SafeCommand(
                LoadItemsAsync
                , viewModel: this); //NB to use OneWay Binding in RefreshView
            async Task LoadItemsAsync()
                => Items.ReplaceRange(await dataStore.GetItemsAsync(true));

            //ZM: An example of page navigation
            AddItemCommand = new SafeCommand(AddItemAsync);
            async Task AddItemAsync() =>
                await navigationService.PushAsync<NewItemViewModel>();

            //ZM: Passing the data Item to the navigated ViewModel is easy
            OnItemSelectedCommand = new SafeCommand<Item>(OnItemSelectedAsync);
            async Task OnItemSelectedAsync(Item item) =>
                await navigationService.PushAsync<ItemDetailViewModel, Item>(item);

        }

        //ZM: This code is run every time the attached View appears
        public void OnViewAppearing(object sender, EventArgs e)
        {
            if (Items.Count == 0)
                LoadItemsCommand.Execute(null);
        }
    }
}