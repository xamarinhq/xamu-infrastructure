//
// RefreshingCollection.cs
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
using System.Diagnostics;
using System.Threading.Tasks;

namespace XamarinUniversity.Infrastructure
{
    /// <summary>
    /// Provides an ObservableCollection which is backed by an asynchronous "fill"
    /// method. You can then "refresh" the data at any time and have the collection
    /// make callbacks when starting and completing the refresh.
    /// </summary>
    /// <typeparam name="T">Object type for the collection</typeparam>
    [DebuggerDisplay("Count={Count}")]
    public class RefreshingCollection<T> : OptimizedObservableCollection<T>
    {
        private bool isRefreshing;
        private readonly Func<Task<IEnumerable<T>>> refreshDataFunc;

        /// <summary>
        /// True when the collection is refreshing.
        /// Can databind this to a ListView.IsRefreshing property to control
        /// when it's refreshing.
        /// </summary>
        public bool IsRefreshing
        {
            get { return isRefreshing; }
            protected set
            {
                if (isRefreshing != value)
                {
                    isRefreshing = value;
                    OnPropertyChanged(new PropertyChangedEventArgs(nameof(IsRefreshing)));
                }
            }
        }

        /// <summary>
        /// This delegate is called BEFORE a refresh is initated
        /// </summary>
        /// <value>The before refresh.</value>
        public Func<RefreshingCollection<T>, object> BeforeRefresh { get; set; }

        /// <summary>
        /// This delegate is called during a refresh to possibly merge the collection.
        /// If this is not implemented, then the collection is replaced.
        /// </summary>
        public Action<RefreshingCollection<T>, IEnumerable<T>> Merge { get; set; }

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
            this.refreshDataFunc = refreshFunc ?? throw new ArgumentNullException(nameof(refreshFunc));
        }

        /// <summary>
        /// Create a new Refreshing Collection.
        /// </summary>
        /// <param name="refreshFunc">Method which returns the data for the collection</param>
        /// <param name="initialData">Initial data to fill collection with</param>
        public RefreshingCollection(Func<Task<IEnumerable<T>>> refreshFunc, IEnumerable<T> initialData)
            : base(initialData)
        {
            this.refreshDataFunc = refreshFunc ?? throw new ArgumentNullException(nameof(refreshFunc));
        }

        /// <summary>
        /// Refreshes the data in the collection. The refresh method is invoked and
        /// this method will replace all the data in the collection with the data coming
        /// back from the refresh method.
        /// </summary>
        /// <returns>Awaitable task</returns>
        public async Task RefreshAsync()
        {
            object refreshParameter = null;
            IsRefreshing = true;

            try
            {
                if (BeforeRefresh != null)
                {
                    refreshParameter = BeforeRefresh.Invoke(this);
                }

                var results = await refreshDataFunc();
                if (results != null)
                {
                    using (base.BeginMassUpdate())
                    {
                        if (Merge == null) // replace the entire collection
                        {
                            base.Clear();
                            foreach (var item in results)
                            {
                                base.Add(item);
                            }
                        }
                        else
                        {
                            Merge.Invoke(this, results);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (RefreshFailed != null)
                {
                    await RefreshFailed.Invoke(this, ex);
                }
            }
            finally
            {
                IsRefreshing = false;
                AfterRefresh?.Invoke(this, refreshParameter);
            }
        }
    }
}
