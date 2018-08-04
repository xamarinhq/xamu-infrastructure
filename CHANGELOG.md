## XamU.Core [2.1.1]
- Fix bug in `ObservableDictionary` so it's property raising `PropertyChange` notifications on `Item[key]` to allow Forms to see it.

## XamU.Infrastructure [2.1.0]
- Allows `XamUInfrastructure.Init` to be called multiple times for native Android initialization in an Activity.
- Fixed a bug in `ObservableCollection` to ensure it always reports change notifications even when `MassUpdate` is not being used.
- Added unit tests to check new features.

## [2.0.0]
### Breaking changes
- New property on `IntegerToBoolean` which allows for a test of `One` vs. `Zero`.
- The `NavigateAsync` methods on `INavigationService` have been split into multiple methods instead of default parameters.
- There's a new interface `IViewModelNavigationInit` which is used to initialize a ViewModel post-creation.
- `FormsNavigationPageService` supports the new ViewModel initialization support.
- `SimpleViewModel` implements the new `IViewModelNavigationInit` interface and forwards it to a virtual method.
- `SimpleViewModel.RaisePropertyChanged` is now virtual and used to raise all property changes so it can be overridden.
- A new version of `IDependencyService.Get<T>` is available which takes a `DependencyScope` parameter to determine whether a new instance or global instance is returned.

## [1.6.0]
### Breaking changes
- Refined namespaces - might need to change some `using` statements.
- Removed `BindablePicker` as the support is now included in Xamarin.Forms. The source is still in the repo if anyone needs it.

### Other changes
- Simplified and cleaned up some code 
- Added a few new converters
- Added `WrapLayout` from Xamarin docs
- Added support to `ItemsControl` to replace the panel

## [1.5.0]
- Replaced PCL support with .NET Standard 1.0 and 2.0
- Added `Orientation` and `Spacing` properties to `ItemsControl`
- Fixed bug in `ItemsControl` when the `DataTemplate` is applied _after_ the `ItemsSource`

## [1.1.0]
- Added support for PCL and .NET Standard
- Added ability to reset navigation  stack

## [1.0.161128]
- Add class constraint to Register method of IDependencyService
        
## [1.0.161104]
- Unify versions

## [1.0.160930]
- Added new `Register` method to `IDependencyService` to allow instance registration.

## [1.0.160916]
- Initial release of Core - broken out from **XamU.Infrastructure.dll** with no dependencies on Xamarin.Forms.