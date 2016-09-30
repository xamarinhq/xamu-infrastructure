//
// DelegateNavigationService.cs
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

namespace XamarinUniversity.Services
{
	/// <summary>
	/// Simple navigation service wrapper which relies solely on delegates
	/// to perform the navigation commands. This is useful when your UI does not
	/// have any NavigationPage in it, but is instead using tabs or MasterDetailPage 
	/// and you still want to drive it via a navigation service in the view model.
	/// </summary>
	/// <remarks>
	/// Note: you must register this service specifically by omitting the RegisterBehavior.Navigation
	/// flag from your call to XamUInfrastructure.Init and registering this service instead.
	/// </remarks>
	public class DelegateNavigationService : INavigationService
	{
		private static readonly Task TaskCompleted = Task.FromResult(0);
		private Dictionary<object, Action<object>> registeredNavRequests;

        /// <summary>
        /// Event raised when NavigateAsync is used
        /// </summary>
        public event EventHandler Navigated;

#pragma warning disable CS0067
        /// <summary>
        /// Event raised when a GoBackAsync operation occurs; not used in this implementation.
        /// </summary>
        public event EventHandler NavigatedBack;
#pragma warning restore CS0067

        /// <summary>
        /// Allows you to change how keys are compared.
        /// Must be called _before_ any navigation requests are registered.
        /// </summary>
        /// <value>The key comparer.</value>
        public IEqualityComparer<object> KeyComparer
		{
			get
			{
				return registeredNavRequests?.Comparer;
			}

			set
			{
				if (value == null)
					throw new ArgumentNullException(nameof(KeyComparer), "KeyComparer cannot be null.");
				if (registeredNavRequests != null)
					throw new InvalidOperationException("Cannot set KeyComparer once pages are added.");
				registeredNavRequests = new Dictionary<object, Action<object>>(value);
			}
		}

		/// <summary>
		/// Register a navigation request with a known key.
		/// </summary>
		/// <param name="key">key</param>
		/// <param name="navigationRequest">What to do</param>
		public void Register(object key, Action<object> navigationRequest)
		{
			if (key == null)
				throw new ArgumentNullException(nameof(key));
			if (navigationRequest == null)
				throw new ArgumentNullException(nameof(navigationRequest));

			if (registeredNavRequests == null)
				registeredNavRequests = new Dictionary<object, Action<object>>();

			registeredNavRequests.Add(key, navigationRequest);
		}

        /// <summary>
        /// Register a navigation request with a known key.
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="navigationRequest">What to do</param>
        public void Register(object key, Action navigationRequest)
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key));
            if (navigationRequest == null)
                throw new ArgumentNullException(nameof(navigationRequest));
            Register(key, () => navigationRequest());
        }

		/// <summary>
		/// Unregister a known page by key.
		/// </summary>
		/// <param name="key">Key to identify request</param>
		public void Unregister(object key)
		{
			if (key == null)
				throw new ArgumentNullException(nameof(key));
			if (registeredNavRequests != null)
				registeredNavRequests.Remove(key);
		}

        /// <summary>
        /// Locates the navigation work by key.
        /// </summary>
        /// <returns>The delegate to call.</returns>
        /// <param name="key">Key which identifies nav request</param>
        Action<object> GetWorkByKey(object key)
		{
			if (key == null)
				throw new ArgumentNullException(nameof(key));

			if (registeredNavRequests == null)
				return null;

            Action<object> work;
            return registeredNavRequests.TryGetValue(key, out work) ? work : null;
		}

		/// <summary>
		/// Executes a navigation request.
		/// </summary>
		/// <returns>Async task.</returns>
		/// <param name="key">Page key.</param>
		/// <param name="parameter">Parameter passed to request</param>
		public Task NavigateAsync(object key, object parameter = null)
		{
			if (key == null)
				throw new ArgumentNullException(nameof(key));

            GetWorkByKey(key)?.Invoke(parameter);
            Navigated?.Invoke(this, EventArgs.Empty);

            return TaskCompleted;
		}

		/// <summary>
		/// True if we can go backwards on the navigation stack.
		/// </summary>
		/// <value><c>true</c> if can go back; otherwise, <c>false</c>.</value>
		public bool CanGoBack
		{
			get
			{
                return false;
			}
		}

		/// <summary>
		/// Pops the last page off the stack.
		/// </summary>
		/// <returns>The back async.</returns>
		public Task GoBackAsync()
		{
            throw new NotSupportedException();
        }

		/// <summary>
		/// Pushes a new page modally onto the navigation stack.
		/// </summary>
		/// <returns>The modal async.</returns>
		/// <param name="pageKey">Page key.</param>
		/// <param name="viewModel">View model.</param>
		public Task PushModalAsync(object pageKey, object viewModel = null)
		{
            throw new NotSupportedException();
        }

        /// <summary>
        /// Pops a page off the modal stack.
        /// </summary>
        /// <returns>The modal async.</returns>
        public Task PopModalAsync()
		{
            throw new NotSupportedException();
		}
	}
}
