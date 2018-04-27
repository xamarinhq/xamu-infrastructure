//
// INavigationService.cs
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
using System.Threading.Tasks;

namespace XamarinUniversity.Infrastructure
{
    /// <summary>
    /// Interface to manage navigation in the application.
    /// </summary>
    public interface INavigationService
    {
        /// <summary>
        /// Key comparer for navigation keys. Change this if you
        /// use a complex key which doesn't support direct equality.
        /// </summary>
        IEqualityComparer<object> KeyComparer { get; set; }

        /// <summary>
        /// Event raised when NavigateAsync is used.
        /// </summary>
        event EventHandler Navigated;

        /// <summary>
        /// Event raised when a GoBackAsync operation occurs.
        /// </summary>
        event EventHandler NavigatedBack;

        /// <summary>
        /// Pops all pages off the stack up to the first one.
        /// </summary>
        Task PopToRootAsync();

        /// <summary>
        /// Navigate to a page using the known key.
        /// </summary>
        /// <returns>The async.</returns>
        /// <param name="key">Navigation key.</param>
        Task NavigateAsync(object key);

        /// <summary>
        /// Navigate to a page using the known key.
        /// The state object (if not null) will be assigned as the BindingContext
        /// if the View doesn't assign it as part of construction. If there _is_ a BindingContext,
        /// then the state will be passed to the IViewModelNavigationInit.IntializeAsync implementation.
        /// </summary>
        /// <returns>Task</returns>
        /// <param name="key">Navigation key.</param>
        /// <param name="state">State (can be ViewModel, or state passed to IViewModelNavigationInit)</param>
        Task NavigateAsync(object key, object state);

        /// <summary>
        /// Returns true/false whether we can go backwards on the Nav Stack.
        /// </summary>
        /// <value><c>true</c> if can go back; otherwise, <c>false</c>.</value>
        bool CanGoBack { get; }

        /// <summary>
        /// Pops the last page off the stack and navigates to it.
        /// </summary>
        /// <returns>Async response</returns>
        Task GoBackAsync();

        /// <summary>
        /// Push a page onto the modal stack.
        /// </summary>
        /// <returns>Async response</returns>
        /// <param name="key">Navigation key.</param>
        Task PushModalAsync(object key);

        /// <summary>
        /// Push a page onto the modal stack.
        /// </summary>
        /// <returns>Async response</returns>
        /// <param name="key">Navigation key.</param>
        /// <param name="state">State (can be ViewModel, or state passed to IViewModelNavigationInit)</param>
        Task PushModalAsync(object key, object state);

        /// <summary>
        /// Pops the last page off the modal stack
        /// </summary>
        /// <returns>Async response</returns>
        Task PopModalAsync();

        /// <summary>
        /// Register an action to take when a specific navigation is requested.
        /// </summary>
        /// <param name="key">Navigation Key</param>
        /// <param name="action">Action to perform</param>
        void RegisterAction(object key, Action action);

        /// <summary>
        /// Register an action to take when a specific navigation is requested.
        /// </summary>
        /// <param name="key">Navigation Key</param>
        /// <param name="action">Action to perform</param>
        void RegisterAction(object key, Action<object> action);

        /// <summary>
        /// Unregister a specific key
        /// </summary>
        /// <param name="key">Navigation Key</param>
        void Unregister(object key);
    }
}