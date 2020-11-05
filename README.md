# ![Logo](https://raw.githubusercontent.com/z33bs/Zenimals/master/Zenimals.iOS/Resources/zenmvvm_icon.png)ZenMvvm Sample App
A refactored version of the Xamarin Forms Startup project, "Shell Forms App"., 
demonstrating **ViewModel-First** navigation with [ZenMvvm](https://github.com/z33bs/zenmvvm#readme). ZenMvvm is a Lightweight **ViewModel-First MVVM** framework for Xamarin.Forms.

>*Tip: Search for `//ZM` in the solution to quickly jump to code comments explaining ZenMvvm => [click here to do this now](https://github.com/z33bs/ZenMvvm-Sample-App/search?q=%2F%2FZM)*

When browsing the code, look for:
* Simpler, easier to understand architecture compared with Xaminals.
* ViewModels don't have a dependency on Xamarin.Forms
* Dependency injection uses the convenient "Smart Resolve" feature meaning you don't have to register dependenices (perfect for rapid prototyping).
* Views Bind to their corresponding ViewModels simply by adding:
```c#
mvvm:ViewModelLocator.AutoWireViewModel="True"
```
* Navigation from ItemsViewModel that passes an Item object to the NewItemViewModel:
```c#
await navigationService.PushAsync<ItemDetailViewModel>(item)
```

See [Zenimals](https://github.com/z33bs/zenimals-sample-app#readme) for an example that showcases more features, including route navigation with `GotoAsync()`.



<img src="https://raw.githubusercontent.com/z33bs/ZenMvvm-Sample-App/master/screenshot_droid.png" width="300" /> 

