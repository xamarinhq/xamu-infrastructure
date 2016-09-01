using Xamarin.Forms;
using XamarinUniversity.Interfaces;

namespace XamarinUniversity.Services
{
    /// <summary>
    /// Static class to initialize the library
    /// </summary>
    public static class XamUInfrastructure
    {
        /// <summary>
        /// Registers the known services with Xamarin.Forms DependencyService.
        /// </summary>
        /// <returns>IDependencyService</returns>
        public static IDependencyService Init()
        {
            // Register the services
            DependencyService.Register<IDependencyService, DependencyServiceWrapper> ();
            DependencyService.Register<IMessageVisualizerService, FormsMessageVisualizerService> ();
            DependencyService.Register<INavigationService, FormsNavigationPageService> ();

            return DependencyService.Get<IDependencyService>();
        }
    }
}

