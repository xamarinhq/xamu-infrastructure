//
// FormsNavigationPageService.cs
//
// Author:
//       Mark Smith <smmark@microsoft.com>
//
// Copyright (c) 2016-2018 Xamarin, Microsoft.
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Xamarin.Forms;
using XamarinUniversity.Infrastructure;

namespace XamarinUniversity.Services
{
    /// <summary>
    /// Service to manage the Xamarin.Forms Stack navigation system.
    /// This understands both <c>NavigationPage</c> and <c>MasterDetailPage</c> with
    /// an embedded navigation page.
    /// </summary>
    public class FormsNavigationPageService : INavigationPageService
    {
#if NETSTANDARD1_0
        private static readonly Task TaskCompleted = Task.FromResult(0);
#endif
        private INavigation navigation;
        private NavigationPage currentNavigationPage;
        private Dictionary<object, Func<Page>> registeredPages;
        private Dictionary<object, Func<object, Page>> registeredStatePages;
        private Dictionary<object, Action<object>> registeredActions;

        /// <summary>
        /// Event raised when NavigateAsync is used.
        /// </summary>
        public event EventHandler Navigated;

        /// <summary>
        /// Event raised when a GoBackAsync operation occurs.
        /// </summary>
        public event EventHandler NavigatedBack;

        /// <summary>
        /// Constructor
        /// </summary>
        public FormsNavigationPageService ()
        {
            // If we are using MasterDetailPage as the MainPage, then
            // hide master page when we navigate on phones since we only look at
            // the detail page for the navigation root.
            HideMasterPageOnNavigation = Device.Idiom == TargetIdiom.Phone;
        }

        /// <summary>
        /// Allows you to change how keys are compared.
        /// Must be called _before_ any pages are registered.
        /// </summary>
        /// <value>The key comparer.</value>
        public IEqualityComparer<object> KeyComparer
        {
            get => registeredPages?.Comparer;

            set
            {
                if (value == null)
                    throw new ArgumentNullException (nameof(KeyComparer), "KeyComparer cannot be null.");

                if (registeredPages != null)
                {
                    throw new InvalidOperationException ("Cannot set KeyComparer once pages are added.");
                }

                registeredPages = new Dictionary<object, Func<Page>> (value);
                registeredStatePages = new Dictionary<object, Func<object, Page>>(value);
            }
        }

        /// <summary>
        /// This flag determines whether to hide the master page when a NavigateAsync
        /// occurs. The default is TRUE for phones, but you can set this flag to FALSE 
        /// to turn off this behavior.
        /// </summary>
        /// <value><c>true</c> if hide master page on navigation; otherwise, <c>false</c>.</value>
        public bool HideMasterPageOnNavigation
        {
            get; set;
        }

        /// <summary>
        /// Register a page with a known key. When the page is
        /// navigated to, the optional state will be set as the BindingContext.
        /// </summary>
        /// <param name="key">Page key.</param>
        /// <param name="creator">Creator.</param>
	    public void RegisterPage(object key, Func<Page> creator)
	    {
            if (key == null)
                throw new ArgumentNullException(nameof(key));
	        if (creator == null)
	            throw new ArgumentNullException(nameof (creator));

            if (registeredPages == null)
            {
                registeredPages = new Dictionary<object, Func<Page>> ();
            }

            registeredPages.Add(key, creator);
	    }

        /// <summary>
        /// Register a page with a known key that is created with a state element.
        /// When the page is created/navigated to, the state may be used to initialize
        /// the page. It will not be set as the BindingContext.
        /// </summary>
        /// <param name="key">Page key.</param>
        /// <param name="creator">Creator.</param>
	    public void RegisterPage(object key, Func<object,Page> creator)
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key));
            if (creator == null)
                throw new ArgumentNullException(nameof(creator));

            if (registeredStatePages == null)
            {
                registeredStatePages = new Dictionary<object, Func<object, Page>>();
            }

            registeredStatePages.Add(key, creator);
        }

        /// <summary>
        /// Registers an action in response to a navigation request.
        /// </summary>
        /// <param name="key">Key</param>
        /// <param name="action">Action to perform, gets passed the viewModel parameter.</param>
        public void RegisterAction(object key, Action<object> action)
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key));
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            if (registeredActions == null)
            {
                registeredActions = new Dictionary<object, Action<object>>();
            }

            registeredActions.Add(key, action);
        }

        /// <summary>
        /// Registers an action in response to a navigation request.
        /// </summary>
        /// <param name="key">Key</param>
        /// <param name="action">Action to perform</param>
        public void RegisterAction(object key, Action action)
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key));
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            if (registeredActions == null)
            {
                registeredActions = new Dictionary<object, Action<object>>();
            }

            registeredActions.Add(key, unused => action());
        }

        /// <summary>
        /// Unregister a known page/action by key.
        /// </summary>
        /// <param name="key">Page key.</param>
        public void Unregister(object key)
	    {
            if (key == null)
                throw new ArgumentNullException(nameof (key));

            registeredPages?.Remove(key);
            registeredStatePages?.Remove(key);
	        registeredActions?.Remove(key);
	    }

        /// <summary>
        /// Locates a page creator by key.
        /// </summary>
        /// <returns>The page by key.</returns>
        /// <param name="pageKey">Page key.</param>
        /// <param name="state">Optional state passed to function</param>
        /// <param name="usedState">True/False whether state was used by page</param>
        Page GetPageByKey(object pageKey, object state, out bool usedState)
        {
            if (pageKey == null)
                throw new ArgumentNullException (nameof (pageKey));

            // Try the state pages first.
            Func<object, Page> stateCreator = null;
            if (registeredStatePages?.TryGetValue(pageKey, out stateCreator) == true
                && stateCreator != null)
            {
                usedState = true;
                return stateCreator.Invoke(state);
            }

            usedState = false;
            Func<Page> creator = null;
            return registeredPages?.TryGetValue(pageKey, out creator) == true && creator != null ? creator.Invoke() : null;
        }

        /// <summary>
        /// Method used to locate the NavigationPage - looks either on the 
        /// MainPage or, in the case of a MasterDetail setup, on the Details page.
        /// </summary>
        /// <returns>The navigation page.</returns>
        NavigationPage FindNavigationPage()
        {
            // Most of the time this is good.
            var navPage = Application.Current.MainPage as NavigationPage;
            if (navPage == null)
            {
                // Special case for Master/Detail page.
                if (Application.Current.MainPage is MasterDetailPage mdPage)
                {
                    // Should always have a NavigationPage as the Detail
                    navPage = mdPage.Detail as NavigationPage;
                }
            }
            return navPage;
        }

        /// <summary>
        /// Returns the underlying Navigation interface implemented by the
        /// Forms page system.
        /// </summary>
        /// <value>The INavigation implementation to use for navigation.</value>
        public INavigation Navigation
        {
            get
            {
                // Locate the navigation page.
                var navPage = FindNavigationPage();
                if (navPage == null)
                    throw new Exception($"Failed to locate required {nameof(NavigationPage)} from App.MainPage.");

                if (navPage != currentNavigationPage)
                {
                    navigation = null;

                    // Unwire from the old object to let it go away
                    if (currentNavigationPage != null)
                    {
                        currentNavigationPage.Pushed -= OnPagePushed;
                        currentNavigationPage.Popped -= OnPagePopped;
                        currentNavigationPage.PoppedToRoot -= OnPagePopped;
                    }

                    // Wire into the events.
                    currentNavigationPage = navPage;
                    currentNavigationPage.Pushed += OnPagePushed;
                    currentNavigationPage.Popped += OnPagePopped;
                    currentNavigationPage.PoppedToRoot += OnPagePopped;
                }

                return navigation ?? (navigation = navPage.Navigation);
            }

            set => throw new NotSupportedException($"{nameof(FormsNavigationPageService)} doesn't support setting the Navigation property.");
        }
        
        /// <summary>
        /// Method called when a page is pushed onto the Navigation stack.
        /// </summary>
        /// <param name="sender">NavigationPage</param>
        /// <param name="e">Details</param>
        void OnPagePushed (object sender, NavigationEventArgs e)
        {
            Navigated?.Invoke (this, EventArgs.Empty);
        }

        /// <summary>
        /// Method called when a page is popped off the Navigation stack,
        /// or when we pop to root.
        /// </summary>
        /// <param name="sender">NavigationPage</param>
        /// <param name="e">Details</param>
        void OnPagePopped (object sender, NavigationEventArgs e)
        {
            NavigatedBack?.Invoke (this, EventArgs.Empty);
        }

        /// <summary>
        /// Navigate to a page using the known key.
        /// </summary>
        /// <param name="key">Page key</param>
        /// <returns>Task</returns>
        public Task NavigateAsync(object key)
        {
            return NavigateAsync(key, null);
        }

        /// <summary>
        /// Navigate to a page using the passed key. This also assigns the
        /// BindingContext if a ViewModel is passed.
        /// </summary>
        /// <returns>Task representing the navigation</returns>
        /// <param name="key">Page key.</param>
        /// <param name="state">State (can be ViewModel, or state passed to IViewModelNavigationInit)</param>
        public async Task NavigateAsync(object key, object state)
        {
            if (key == null)
                throw new ArgumentNullException (nameof (key));

            // On a phone, always hide master page when we navigate since we
            // will be using the Detail page.
            if (HideMasterPageOnNavigation)
            {
                if (Application.Current.MainPage is MasterDetailPage mdPage)
                {
                    mdPage.IsPresented = false;
                }
            }

            // Look for a registered page first. If that's not available, look for an action.
            var page = GetPageByKey(key, state, out bool usedState);
            if (page == null)
            {
                if (registeredActions != null)
                {
                    if (registeredActions.TryGetValue(key, out var action))
                    {
                        action.Invoke(state);
                    }
                }
                return;
            }

            if (page == null)
                throw new ArgumentException("Cannot navigate to unregistered page", nameof(key));

            await InitializeViewModel(page, usedState ? null : state);
            await Navigation.PushAsync(page).ConfigureAwait(false);
        }

        /// <summary>
        /// True if we can go backwards on the navigation stack.
        /// </summary>
        /// <value><c>true</c> if can go back; otherwise, <c>false</c>.</value>
        public bool CanGoBack => Navigation.NavigationStack.Count > 1;

        /// <summary>
        /// Pops the last page off the stack.
        /// </summary>
        /// <returns>Task representing the navigation event.</returns>
        public Task GoBackAsync()
        {
            return !CanGoBack
#if NETSTANDARD1_0
                ? TaskCompleted : Navigation.PopAsync();
#else
                ? Task.CompletedTask : Navigation.PopAsync();
#endif
        }

        /// <summary>
        /// Pushes a new page modally onto the navigation stack.
        /// </summary>
        /// <returns>Task representing the modal navigation.</returns>
        /// <param name="key">Page key.</param>
        public Task PushModalAsync(object key)
        {
            return PushModalAsync(key, null);
        }

        /// <summary>
        /// Pushes a new page modally onto the navigation stack.
        /// </summary>
        /// <returns>Task representing the modal navigation.</returns>
        /// <param name="key">Page key.</param>
        /// <param name="state">State (can be ViewModel, or state passed to IViewModelNavigationInit)</param>
        public async Task PushModalAsync(object key, object state)
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key));

            var page = GetPageByKey(key, state, out var usedState);
            if (page == null)
            {
                throw new ArgumentException("Cannot navigate to unregistered page", nameof(key));
            }

            await InitializeViewModel(page, usedState ? null : state);
            await Navigation.PushModalAsync(page).ConfigureAwait(false);
        }

        /// <summary>
        /// This method binds a newly created Page with a ViewModel and then
        /// optionally initializes the ViewModel through the IViewModelNavigationInit interface
        /// </summary>
        /// <param name="page">New page</param>
        /// <param name="state">State to initialize with</param>
        /// <returns>Task</returns>
        private static async Task InitializeViewModel(Page page, object state)
        {
            if (page == null)
                throw new ArgumentNullException(nameof(page));

            // If no view model was assigned by the View as part of creation, 
            // we will assume that we either have one to assign, or we should use the "state" as our VM.
            if (page.BindingContext == null && state != null)
            {
                page.BindingContext = state;
                state = null;
            }

            // See if our VM allows for initialization via our interface.
            if (page.BindingContext is IViewModelNavigationInit vmInit)
            {
                await vmInit.IntializeAsync(state);
            }
        }

        /// <summary>
        /// Pops a page off the modal stack.
        /// </summary>
        /// <returns>Task representing the navigation.</returns>
        public Task PopModalAsync()
        {
            return Navigation.PopModalAsync();
        }

        /// <summary>
        /// Pops all pages up to the final root page
        /// </summary>
        /// <returns>Task representing the navigation</returns>
        public Task PopToRootAsync()
        {
            return Navigation.PopToRootAsync();
        }
    }
}

