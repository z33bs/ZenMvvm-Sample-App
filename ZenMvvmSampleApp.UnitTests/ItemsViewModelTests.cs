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
        readonly Item item
            = new Item { Id = "1", Text = "Item1", Description = "This is Item1" };

        public const int WAITING_TIME_FOR_ASYNC_MS = 1000;

        //ZM: Dependency injection being used with mocks.
        // At no point is ZenMvvm being tested = true unit testing
        [Fact]
        public void LoadItemsCommand_Constructed_IsNotNull()
        {
            //Act
            var vm = new ItemsViewModel(
                new Mock<INavigationService>().Object,
                new Mock<IDataStore<Item>>().Object,
                new Mock<ISafeMessagingCenter>().Object);

            Assert.NotNull(vm.LoadItemsCommand);
        }

        [Fact]
        public void LoadItemsCommand_Executed_ItemsPropertyIsNotEmpty()
        {
            var mockDataStore = new Mock<IDataStore<Item>>();
            mockDataStore.Setup(o => o.GetItemsAsync(true))
                .ReturnsAsync(new List<Item> { item });

            var vm = new ItemsViewModel(
                new Mock<INavigationService>().Object,
                mockDataStore.Object,
                new Mock<ISafeMessagingCenter>().Object);

            //Act
            vm.LoadItemsCommand.Execute(null);
            Thread.Sleep(WAITING_TIME_FOR_ASYNC_MS);

            Assert.Single(vm.Items);
        }

        [Fact]
        public void LoadItemsCommand_ExecutedWhenIsBusy_DoestRun()
        {
            var mockDataStore = new Mock<IDataStore<Item>>();
            mockDataStore.Setup(o => o.GetItemsAsync(true))
                .ReturnsAsync(new List<Item> { item }).Verifiable();

            var vm = new ItemsViewModel(
                new Mock<INavigationService>().Object,
                mockDataStore.Object,
                new Mock<ISafeMessagingCenter>().Object);

            //LoadItems calls mockDataStore
            vm.LoadItemsCommand.Execute(null);
            Thread.Sleep(WAITING_TIME_FOR_ASYNC_MS);
            mockDataStore.Verify();
            Assert.Single(mockDataStore.Invocations);

            //Setup
            vm.IsBusy = true;

            //Act
            vm.LoadItemsCommand.Execute(null);
            Thread.Sleep(WAITING_TIME_FOR_ASYNC_MS);
            // mockDataStore not called a second time
            mockDataStore.VerifyNoOtherCalls();
            Assert.Single(mockDataStore.Invocations);
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

        //ZM: Test that navigation was called
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
            Thread.Sleep(WAITING_TIME_FOR_ASYNC_MS);
            mockNavigation.Verify();
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
                o => o.PushAsync<ItemDetailViewModel, Item>(item, true)).Verifiable();

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
            var mockDataStore = new Mock<IDataStore<Item>>();
            mockDataStore.Setup(o => o.GetItemsAsync(true))
                .ReturnsAsync(new List<Item> { item });

            var vm = new ItemsViewModel(
                new Mock<INavigationService>().Object,
                mockDataStore.Object,
                new Mock<ISafeMessagingCenter>().Object);

            vm.OnViewAppearing(this, new EventArgs());
            Thread.Sleep(WAITING_TIME_FOR_ASYNC_MS);
            Assert.NotEmpty(vm.Items);
        }

        //ZM: Test that LoadItemsCommand results in a CollectionChanged event on Items
        [Fact]
        public void ItemsProperty_LoadItemsCommandExecuted_RaisesCollectionChanged()
        {
            bool invoked = false;

            var mockDataStore = new Mock<IDataStore<Item>>();
            mockDataStore.Setup(o => o.GetItemsAsync(true))
                .ReturnsAsync(new List<Item> { item });

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

        //ZM: Test messaging center subscription
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