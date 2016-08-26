//
// GroupedObservableCollection.cs
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

using System.ComponentModel;
using XamarinUniversity.Infrastructure;
using System.Collections.Generic;

namespace XamarinUniversity.Collections
{
    /// <summary>
    /// This is a simple observable collection which has a GroupBy key which can
    /// be used to populate a ListView with grouping turned on
    /// </summary>
    /// <typeparam name="TKey">The type to use for the grouping key<typeparam name="TKey"/>
    /// <typeparam name="TValue">The type to use for the items<typeparam name="TKey"/>
    public class GroupedObservableCollection<TKey,TValue> 
        : OptimizedObservableCollection<TValue>
    {
        // Data
        bool hasItems;
        readonly TKey key;

        /// <summary>
        /// The read-only grouping key.
        /// </summary>
        /// <value>The group title.</value>
        public TKey Key { get; }

        /// <summary>
        /// Simple property to allow us to collapse a group when it has no items.
        /// </summary>
        /// <value><c>true</c> if has items; otherwise, <c>false</c>.</value>
        public bool HasItems {
            get {
                return hasItems;
            }

            set {
                if (hasItems != value) {
                    hasItems = value;
                    OnPropertyChanged (new PropertyChangedEventArgs (nameof (HasItems)));
                }
            }
        }

        /// <summary>
        /// Initializes a grouped collection.
        /// </summary>
        public GroupedObservableCollection (TKey key)
        {
            this.key = key;
        }

        /// <summary>
        /// Initializes the grouped collection with a set of items.
        /// </summary>
        /// <param name="key">Grouping key value</param>
        /// <param name="items">Set of items for this group</param>
        public GroupedObservableCollection (TKey key, IEnumerable<TValue> items)
            : base(items)
        {
            this.key = key;
        }

        /// <summary>
        /// Handles the PropertyChanged notification. We use this to catch changes
        /// to the Count and then update the <see cref="HasItems"/> property.
        /// </summary>
        /// <param name="e">EventArgs</param>
        protected override void OnPropertyChanged (PropertyChangedEventArgs e)
        {
            base.OnPropertyChanged (e);
            if (e.PropertyName == nameof (Count)) {
                HasItems = Count > 0;
            }
        }
    }
}

