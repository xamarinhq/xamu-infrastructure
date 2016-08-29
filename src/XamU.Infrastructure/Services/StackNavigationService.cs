//
// StackNavigationService.cs
//
// Author:
//       Mark Smith <mark.smith@xamarin.com>
//
// Copyright (c) 2016 Xamarin, Microsoft.
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
using System.Threading.Tasks;
using XamarinUniversity.Interfaces;
using Xamarin.Forms;

namespace XamarinUniversity.Services
{
    /// <summary>
    /// Service to manage the Xamarin.Forms Stack navigation system.
    /// This understands both <c>NavigationPage</c> and <c>MasterDetailPage</c> with
    /// an embedded navigation page.
    /// </summary>
    public class StackNavigationService : INavigationService
    {
        private static readonly Task TaskCompleted = Task.FromResult(0);
        private INavigation navigation;
        readonly Dictionary<string, Func<Page>> registeredPages = new Dictionary<string, Func<Page>>();

        /// <summary>
        /// Register a page with a known key.
        /// </summary>
        /// <param name="pageKey">Page key.</param>
        /// <param name="creator">Creator.</param>
	    public void RegisterPage(string pageKey, Func<Page> creator)
	    {
            if (string.IsNullOrEmpty(pageKey))
                throw new ArgumentNullException("pageKey");
	        if (creator == null)
	            throw new ArgumentNullException("creator");
	        registeredPages.Add(pageKey, creator);
	    }

        /// <summary>
        /// Unregister a known page by key.
        /// </summary>
        /// <param name="pageKey">Page key.</param>
	    public void UnregisterPage(string pageKey)
	    {
            if (string.IsNullOrEmpty(pageKey))
                throw new ArgumentNullException("pageKey");
            registeredPages.Remove(pageKey);
	    }

        /// <summary>
        /// Locates a page creator by key.
        /// </summary>
        /// <returns>The page by key.</returns>
        /// <param name="pageKey">Page key.</param>
        Page GetPageByKey(string pageKey)
        {
            if (string.IsNullOrEmpty(pageKey))
                throw new ArgumentNullException("key");

            Func<Page> creator;
            return registeredPages.TryGetValue(pageKey, out creator) ? creator.Invoke() : null;
        }

        /// <summary>
        /// Returns the underlying Navigation interface implemented by the
        /// Forms page system.
        /// </summary>
        /// <value>The navigation.</value>
        INavigation Navigation
        {
            get
            {
                if (navigation == null)
                {
                    // Most of the time this is good.
                    var main = Application.Current.MainPage;
                    if (main is NavigationPage)
                        navigation = main.Navigation;

                    // Special case for Master/Detail page.
                    MasterDetailPage mdPage = main as MasterDetailPage;
                    if (mdPage != null)
                    {
                        if (mdPage.Master is NavigationPage)
                            navigation = mdPage.Master.Navigation;
                        if (mdPage.Detail is NavigationPage)
                            navigation = mdPage.Detail.Navigation;
                    }

                    if (navigation == null)
                        throw new Exception("Failed to locate navigation");
                }
                return navigation;
            }
        }

        /// <summary>
        /// Navigate to a page using the passed key. This also assigns the
        /// BindingContext if a ViewModel is passed.
        /// </summary>
        /// <returns>The async.</returns>
        /// <param name="pageKey">Page key.</param>
        /// <param name="viewModel">View model.</param>
        public Task NavigateAsync(string pageKey, object viewModel = null)
        {
            if (Device.Idiom == TargetIdiom.Phone)
            {
                var mdPage = Application.Current.MainPage as MasterDetailPage;
                if (mdPage != null)
                {
                    mdPage.IsPresented = false;
                }
            }

            var page = GetPageByKey(pageKey);
            if (page == null)
                return TaskCompleted;

            if (viewModel != null)
                page.BindingContext = viewModel;

            return Navigation.PushAsync(page);
        }

        /// <summary>
        /// True if we can go backwards on the navigation stack.
        /// </summary>
        /// <value><c>true</c> if can go back; otherwise, <c>false</c>.</value>
        public bool CanGoBack
        {
            get
            {
                return Navigation.NavigationStack.Count > 1;
            }
        }

        /// <summary>
        /// Pops the last page off the stack.
        /// </summary>
        /// <returns>The back async.</returns>
        public Task GoBackAsync()
        {
            return !CanGoBack ? TaskCompleted : Navigation.PopAsync();
        }

        /// <summary>
        /// Pushes a new page modally onto the navigation stack.
        /// </summary>
        /// <returns>The modal async.</returns>
        /// <param name="pageKey">Page key.</param>
        /// <param name="viewModel">View model.</param>
        public Task PushModalAsync(string pageKey, object viewModel = null)
        {
            var page = GetPageByKey(pageKey);
            if (page == null)
                throw new ArgumentException("Cannot navigate to unregistered page", "pageKey");

            if (viewModel != null)
                page.BindingContext = viewModel;

            return Navigation.PushModalAsync(page);
        }

        /// <summary>
        /// Pops a page off the modal stack.
        /// </summary>
        /// <returns>The modal async.</returns>
        public Task PopModalAsync()
        {
            return Navigation.PopModalAsync();
        }
    }
}

