//
// RefreshingCollection.cs
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
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Threading.Tasks;

namespace XamarinUniversity.Infrastructure
{
    /// <summary>
    /// Provides an ObservableCollection which is backed by an asynchronous "fill"
    /// method. You can then "refresh" the data at any time and have the collection
    /// make callbacks when starting and completing the refresh.
    /// </summary>
    [DebuggerDisplay ("Count={Count}")]
    public class RefreshingCollection<T> : ObservableCollection<T>
    {
        private bool isRefreshing;
        readonly Func<Task<IEnumerable<T>>> refreshDataFunc;

        /// <summary>
        /// This delegate is called BEFORE a refresh is initated
        /// </summary>
        /// <value>The before refresh.</value>
        public Func<RefreshingCollection<T>, object> BeforeRefresh { get; set; }

        /// <summary>
        /// This delegate is called AFTER a refresh completes and the contents are replaced.
        /// </summary>
        /// <value>The after refresh.</value>
        public Action<RefreshingCollection<T>, object> AfterRefresh { get; set; }

        /// <summary>
        /// This delegate is called if a refresh throws an exception.
        /// </summary>
        /// <value>The refresh failed.</value>
        public Func<RefreshingCollection<T>, Exception, Task> RefreshFailed { get; set; }

        /// <summary>
        /// Create a new Refreshing Collection.
        /// </summary>
        /// <param name="refreshFunc">Method which returns the data for the collection</param>
        public RefreshingCollection(Func<Task<IEnumerable<T>>> refreshFunc)
        {
            if (refreshFunc == null)
                throw new ArgumentNullException("refreshFunc");
            this.refreshDataFunc = refreshFunc;
        }

        /// <summary>
        /// Called when the collection is being changed; we turn this off during
        /// full refresh events.
        /// </summary>
        /// <param name="e">E.</param>
        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            if (!isRefreshing)
                base.OnCollectionChanged(e);
        }

        /// <summary>
        /// Refreshes the data in the collection. The refresh method is invoked and
        /// this method will replace all the data in the collection with the data coming
        /// back from the refresh method.
        /// </summary>
        /// <returns>Awaitable task</returns>
        public async Task RefreshAsync(bool appendData = false)
        {
            object refreshParameter = null;
            Exception caughtException = null;
            isRefreshing = true;

            try
            {
                if (BeforeRefresh != null)
                    refreshParameter = BeforeRefresh.Invoke(this);

                var results = await refreshDataFunc();
                if (results != null)
                {
                    this.Clear();
                    foreach (var item in results)
                        this.Add(item);
                }
            }
            catch (Exception ex)
            {
                caughtException = ex;
            }

            if (caughtException != null)
            {
                if (RefreshFailed != null)
                    await RefreshFailed.Invoke (this, caughtException);  
            }
            else if (AfterRefresh != null)
                AfterRefresh.Invoke (this, refreshParameter);
            
            // Done refresh the world.
            isRefreshing = false;
            OnCollectionChanged(
                new NotifyCollectionChangedEventArgs(
                    NotifyCollectionChangedAction.Reset));
        }
    }
}
