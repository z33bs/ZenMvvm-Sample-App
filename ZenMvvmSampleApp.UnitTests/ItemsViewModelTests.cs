using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Xunit;
using ZenMvvm;
using ZenMvvm.Helpers;
using ZenMvvmSampleApp.Models;
using ZenMvvmSampleApp.Services;
using ZenMvvmSampleApp.ViewModels;

namespace ZenMvvmSampleApp.UnitTests
{
    public class ItemsViewModelTests
    {
        readonly Mock<IDataStore<Item>> mockDataStore;
        readonly Item item;

        public const int WAITING_TIME_FOR_ASYNC_MS = 1000;

        public ItemsViewModelTests()
        {
            item = new Item { Id = "1", Text = "Item1", Description = "This is Item1" };

            mockDataStore = new Mock<IDataStore<Item>>();
            mockDataStore.Setup(o => o.GetItemsAsync(true))
                .ReturnsAsync(new List<Item> { item });
        }

        [Fact]
        public void LoadItemsCommand_Constructed_IsNotNull()
        {
            var vm = new ItemsViewModel(
                new Mock<INavigationService>().Object,
                new Mock<IDataStore<Item>>().Object,
                new Mock<ISafeMessagingCenter>().Object);
            Assert.NotNull(vm.LoadItemsCommand);
        }

        [Fact]
        public void LoadItemsCommand_Executed_ItemsPropertyIsNotEmpty()
        {
            var vm = new ItemsViewModel(
                new Mock<INavigationService>().Object,
                mockDataStore.Object,
                new Mock<ISafeMessagingCenter>().Object);

            vm.LoadItemsCommand.Execute(null);
            Assert.Single(vm.Items);
        }

        [Fact]
        public void LoadItemsCommand_ExecutedWhenIsBusy_ItemsPropertyIsEmpty()
        {
            var vm = new ItemsViewModel(
                new Mock<INavigationService>().Object,
                mockDataStore.Object,
                new Mock<ISafeMessagingCenter>().Object)
            {
                IsBusy = true
            };

            vm.LoadItemsCommand.Execute(null);
            Assert.Empty(vm.Items);
        }

        [Fact]
        public void AddItemCommand_Constructed_IsNotNull()
        {
            var vm = new ItemsViewModel(
                new Mock<INavigationService>().Object,
                new Mock<IDataStore<Item>>().Object,
                new Mock<ISafeMessagingCenter>().Object);

            Assert.NotNull(vm.AddItemCommand);
        }

        [Fact]
        public void AddItemCommand_Executed_NavigatesToNewItemViewModel()
        {
            var mockNavigation = new Mock<INavigationService>();
            mockNavigation.Setup(
                o => o.PushAsync<NewItemViewModel>(true)).Verifiable();

            var vm = new ItemsViewModel(
                mockNavigation.Object,
                new Mock<IDataStore<Item>>().Object,
                new Mock<ISafeMessagingCenter>().Object);

            vm.AddItemCommand.Execute(null);
            Mock.Verify(new Mock[] { mockNavigation });
        }

        [Fact]
        public void OnItemSelectedCommand_Constructed_IsNotNull()
        {
            var vm = new ItemsViewModel(
                new Mock<INavigationService>().Object,
                new Mock<IDataStore<Item>>().Object,
                new Mock<ISafeMessagingCenter>().Object);

            Assert.NotNull(vm.OnItemSelectedCommand);
        }

        [Fact]
        public void OnItemSelectedCommand_Executed_NavigatesToItemDetailViewModel()
        {
            var mockNavigation = new Mock<INavigationService>();
            mockNavigation.Setup(
                o => o.PushAsync<ItemDetailViewModel>(item, true)).Verifiable();

            var vm = new ItemsViewModel(
                mockNavigation.Object,
                new Mock<IDataStore<Item>>().Object,
                new Mock<ISafeMessagingCenter>().Object);

            vm.OnItemSelectedCommand.Execute(item);
            Thread.Sleep(WAITING_TIME_FOR_ASYNC_MS);
            mockNavigation.Verify();
        }

        [Fact]
        public void ItemsProperty_Constructed_IsEmpty()
        {
            var vm = new ItemsViewModel(
                new Mock<INavigationService>().Object,
                new Mock<IDataStore<Item>>().Object,
                new Mock<ISafeMessagingCenter>().Object);

            Assert.Empty(vm.Items);
        }

        [Fact]
        public void ItemsProperty_AfterOnViewAppearing_IsNotEmpty()
        {
            var vm = new ItemsViewModel(
                new Mock<INavigationService>().Object,
                mockDataStore.Object,
                new Mock<ISafeMessagingCenter>().Object);

            vm.OnViewAppearing(this, new EventArgs());
            Thread.Sleep(WAITING_TIME_FOR_ASYNC_MS);
            Assert.NotEmpty(vm.Items);
        }

        [Fact]
        public void ItemsProperty_LoadItemsCommandExecuted_RaisesCollectionChanged()
        {
            bool invoked = false;

            var vm = new ItemsViewModel(
                new Mock<INavigationService>().Object,
                mockDataStore.Object,
                new Mock<ISafeMessagingCenter>().Object);

            vm.Items.CollectionChanged += (sender, e) =>
            {
                if (e.Action == NotifyCollectionChangedAction.Reset
                    && (((ObservableRangeCollection<Item>)sender).Count == 1))
                    invoked = true;
            };

            vm.LoadItemsCommand.Execute(null);
            Thread.Sleep(WAITING_TIME_FOR_ASYNC_MS);
            Assert.True(invoked);
        }

        [Fact]
        public void ItemsViewModel_Constructed_MessagingCenterSubscribeToAddItem()
        {
            var messagingCenter = new Mock<ISafeMessagingCenter>();
            messagingCenter.Setup(o => o.Subscribe < NewItemViewModel, Item>(
                    It.IsAny<ItemsViewModel>(),
                    "AddItem",
                    It.IsAny<Func<NewItemViewModel, Item, Task>>(),
                    null, null, null, true)
                ).Verifiable();

            var vm = new ItemsViewModel(
                new Mock<INavigationService>().Object,
                new Mock<IDataStore<Item>>().Object,
                messagingCenter.Object);

            messagingCenter.Verify();
        }
    }
}