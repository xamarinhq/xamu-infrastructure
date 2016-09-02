using XamarinUniversity.Interfaces;
using System;

namespace XamarinUniversity.Services
{
    /// <summary>
    /// Static class to initialize the library
    /// </summary>
    public static class XamUInfrastructure
    {
        static IDependencyService serviceLocator;

        /// <summary>
        /// This allows you to retrieve and customize the global dependency service
        /// used by the library (and app).
        /// </summary>
        /// <value>The service locator.</value>
        public static IDependencyService ServiceLocator
        {
            get
            {
                return serviceLocator != null
                    ? serviceLocator
                    : (serviceLocator = new DependencyServiceWrapper ());
            }

            set
            {
                if (serviceLocator != null)
                    throw new InvalidOperationException ("Cannot set ServiceLocator property once Init is called.");
                if (value == null)
                    throw new ArgumentNullException (nameof(value), "ServiceLocator property cannot be null.");
                serviceLocator = value;
            }
        }


        /// <summary>
        /// Registers the known services with the ServiceLocator type.
        /// </summary>
        /// <returns>IDependencyService</returns>
        public static IDependencyService Init()
        {
            // Get our default locator
            var locator = ServiceLocator;

            // Register the services
            locator.Register<IMessageVisualizerService, FormsMessageVisualizerService> ();
            locator.Register<INavigationService, FormsNavigationPageService> ();

            return locator;
        }
    }
}

