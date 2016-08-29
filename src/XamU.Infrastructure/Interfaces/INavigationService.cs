//
// INavigationService.cs
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
using System.Threading.Tasks;
using Xamarin.Forms;

namespace XamarinUniversity.Interfaces
{
    /// <summary>
    /// Interface to manage navigation in the application.
    /// </summary>
    public interface INavigationService
    {
        /// <summary>
        /// Register a Forms page with a key.
        /// </summary>
        /// <param name="pageKey">Page key (string).</param>
        /// <param name="creator">Creator function to return a Page.</param>
        void RegisterPage(string pageKey, Func<Page> creator);

        /// <summary>
        /// Unregister a known page from the navigation system.
        /// </summary>
        /// <param name="pageKey">Page key.</param>
        void UnregisterPage(string pageKey);

        /// <summary>
        /// Navigate to a page using the known key.
        /// </summary>
        /// <returns>The async.</returns>
        /// <param name="pageKey">Page key.</param>
        /// <param name="viewModel">View model.</param>
        Task NavigateAsync(string pageKey, object viewModel = null);

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
        /// <param name="pageKey">Page key.</param>
        /// <param name="viewModel">View model.</param>
        Task PushModalAsync(string pageKey, object viewModel = null);

        /// <summary>
        /// Pops the last page off the modal stack
        /// </summary>
        /// <returns>Async response</returns>
        Task PopModalAsync();
    }
}