# ![Logo](https://raw.githubusercontent.com/z33bs/Zenimals/master/Zenimals.iOS/Resources/zenmvvm_icon.png)ZenMvvm Sample App
A refactored version of the Xamarin Forms Startup project, "Shell Forms App"., 
demonstrating **ViewModel-First** navigation with [ZenMvvm](https://github.com/z33bs/zenmvvm#readme). ZenMvvm is a Lightweight **ViewModel-First MVVM** framework for Xamarin.Forms.

>*Tip: Search for `//ZM` in the solution to quickly jump to code comments explaining ZenMvvm => [click here to do this now](https://github.com/z33bs/ZenMvvm-Sample-App/search?q=%2F%2FZM%3A)*

When browsing the code, look for:
* Simple. Almost nothing new to learn. Design pages as you normally would using Xamarin Forms.
* Cleaner, easier to understand architecture
* Nothing in the code-behind files
* ViewModels are easily testable. This example uses xUnit and Moq to test the ItemsViewModel class.ViewModels don't have any dependency on Xamarin.Forms
* Buit-in dependency injection uses the convenient "[Smart Resolve](https://github.com/z33bs/SmartDi/wiki/Resolution#smart-resolve)" feature meaning you don't have to register dependenices (perfect for rapid prototyping).
* Passing data to another viewmodel is  easy. Navigation from ItemsViewModel that passes an Item object to the NewItemViewModel:
```c#
await navigationService.PushAsync<ItemDetailViewModel>(item)
```

* Running async code after page navigation has completed is achieved with `IOnNavigated`



For another sample app, see [Zenimals](https://github.com/z33bs/zenimals-sample-app#readme) which showcases more features, including route navigation with `GotoAsync()`.



<img src="https://raw.githubusercontent.com/z33bs/ZenMvvm-Sample-App/master/screenshot_droid.png" width="300" /> 

