using XamarinUniversity.Interfaces;
using System;
using System.Diagnostics;

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
        public static IDependencyService ServiceLocator => serviceLocator != null
                    ? serviceLocator
                    : (serviceLocator = new DependencyServiceWrapper ());

        /// <summary>
        /// Registers the known services with the ServiceLocator type.
        /// </summary>
        /// <returns>IDependencyService</returns>
        public static IDependencyService Init ()
        {
            return Init (null);
        }
            
        /// <summary>
        /// Registers the known services with the ServiceLocator type.
        /// </summary>
        /// <param name="defaultLocator">ServiceLocator, if null, DependencyService is used.</param>
        /// <returns>IDependencyService</returns>
        public static IDependencyService Init(IDependencyService defaultLocator)
        {
            // If the ServiceLocator has already been set, then something used it before
            // Init was called. This is not allowed if they are going to change the locator.
            if (defaultLocator != null 
                && serviceLocator != null)
                throw new InvalidOperationException (
                    "Must call XamUInfrastructure.Init before using any library features; " +
                    "ServiceLocator has already been set.");

            // Assign the locator; either use the supplied one, or the default
            // DependencyService version if not supplied.
            if (defaultLocator == null)
                defaultLocator = ServiceLocator;
            else {
                Debug.Assert (serviceLocator == null);
                serviceLocator = defaultLocator;
            }

            // Register the services
            defaultLocator.Register<IMessageVisualizerService, FormsMessageVisualizerService> ();
            defaultLocator.Register<INavigationService, FormsNavigationPageService> ();

            return defaultLocator;
        }
    }
}

