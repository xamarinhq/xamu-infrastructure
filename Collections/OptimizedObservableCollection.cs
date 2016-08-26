//
// OptimizedObservableCollection.cs
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
using System.ComponentModel;

namespace XamarinUniversity.Infrastructure
{
    /// <summary>
    /// ObservableCollection implementation which supports 
    /// turning off notifications for mass updates through 
    /// the <see cref="BeginMassUpdate"/> method.
    /// </summary>
    /// <example>
    /// <code>
    /// var coll = new OptimizedObservableCollection&lt;string&gt;();
    /// ...
    /// using (BeginMassUpdate ()) {
    ///    foreach (var value in names)
    ///       coll.Add (value);
    /// }
    /// </code>
    /// </example>
    public class OptimizedObservableCollection<T> : ObservableCollection<T>
    {
        bool shouldRaiseNotifications;

        /// <summary> 
        /// Init a new instance of the collection.
        /// </summary> 
        public OptimizedObservableCollection ()
        {
        }

        /// <summary>
        /// Initialize a new instance of the collection from an existing data set.
        /// </summary>
        /// <param name="collection">Collection.</param>
        public OptimizedObservableCollection (IEnumerable<T> collection)
            : base(collection)
        {
        }

        /// <summary>
        /// This method turns off notifications until the returned object
        /// is Disposed. At that point, the entire collection is invalidated.
        /// </summary>
        /// <returns>IDisposable</returns>
        public IDisposable BeginMassUpdate ()
        {
            return new MassUpdater (this);
        }

        /// <summary>
        /// Turn off the collection changed notification
        /// </summary>
        /// <param name="e">E.</param>
        protected override void OnCollectionChanged (NotifyCollectionChangedEventArgs e)
        {
            if (shouldRaiseNotifications)
                base.OnCollectionChanged (e);
        }

        /// <summary>
        /// Turn off the property changed notification
        /// </summary>
        /// <param name="e">E.</param>
        protected override void OnPropertyChanged (PropertyChangedEventArgs e)
        {
            if (shouldRaiseNotifications)
                base.OnPropertyChanged (e);
        }

        /// <summary>
        /// IDisposable class which turns off updating
        /// </summary>
        class MassUpdater : IDisposable
        {
            readonly OptimizedObservableCollection<T> parent;
            public MassUpdater (OptimizedObservableCollection<T> parent)
            {
                this.parent = parent;
                parent.shouldRaiseNotifications = false;
            }

            public void Dispose ()
            {
                parent.shouldRaiseNotifications = true;
                parent.OnPropertyChanged (new PropertyChangedEventArgs ("Count"));
                parent.OnPropertyChanged (new PropertyChangedEventArgs ("Item[]"));
                parent.OnCollectionChanged (new NotifyCollectionChangedEventArgs (NotifyCollectionChangedAction.Reset));
            }
        }
    }
}

