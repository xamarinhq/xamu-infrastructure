# Xamarin University Infrastructure Library

![Build Status](https://xamu-labs.visualstudio.com/_apis/public/build/definitions/454c2b9a-92a4-4467-a3ea-f1a914619cf3/3/badge)

This is a set of useful classes for Xamarin and Xamarin.Forms development which are used in a varity of labs in Xamarin University.

You can use it in source form by cloning this repository (or just grabbing the pieces you want to use), or as a binary through two NuGet packages: [Core](https://www.nuget.org/packages/XamarinUniversity.Core/) and [Infrastructure](https://www.nuget.org/packages/XamarinUniversity.Infrastructure/) which also adds Core. 

The library includes examples of behaviors, MVVM, custom controls, binding value converters, type extensions, XAML markup extensions and helpful collections for data binding.

## Add the NuGet packages to your project
To add the library to your project as an updatable binary, you can use NuGet with the following package:

```
PM > Install-Package XamarinUniversity.Infrastructure
```

This will also add the **Core** library which provides the bare minimum for MVVM development - with no Xamarin.Forms dependencies. You can add this library on it's own if you prefer - this is useful if you segregate your ViewModels out to a separate assembly and don't want to take a dependency on Xamarin.Forms:

```
PM > Install-Package XamarinUniversity.Core
```

## Helpful links

* [NuGet package](https://www.nuget.org/packages/XamarinUniversity.Infrastructure/)

* [Documentation](https://github.com/xamarinhq/xamu-infrastructure/wiki)

* [License](https://github.com/xamarinhq/xamu-infrastructure/blob/master/LICENSE)

* [Contribution Guide](https://github.com/xamarinhq/xamu-infrastructure/blob/master/CONTRIBUTING.md)

Copyright (C) 2016-2018 Xamarin University, Microsoft
