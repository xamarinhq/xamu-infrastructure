//
// CollectionExtensions.cs
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
using System.Collections;
using System.Linq;
using System.Collections.Generic;

namespace XamarinUniversity.Infrastructure
{
    /// <summary>
    /// A set of <c>IList</c>, <c>IEnumerable</c>, and <c>ICollection</c> extension methods
    /// </summary>
    public static class CollectionExtensions
    {
        /// <summary>
        /// Enumerates a collection and executes a predicate against each item
        /// </summary>
        /// <typeparam name="T">Type of elements</typeparam>
        /// <param name="collection">Collection</param>
        /// <param name="action">Action to execute on each element</param>
        public static void ForEach<T> (this IEnumerable<T> collection, Action<T> action)
        {
            if (action == null)
                throw new ArgumentNullException ("action", "Missing Action<T> to execute.");
            if (collection != null)
                foreach (var o in collection) action (o);
        }

        /// <summary>
        /// Perform a sort of the items in a collection. This is useful
        /// if the underlying collection does not support sorting. 
        /// </summary>
        /// <param name="collection">Underlying collection to sort</param>
        /// <param name="comparer">Comparer delegate</param>
        /// <param name="reverse">True to reverse the collection</param>
        public static void BubbleSort<T> (this IList<T> collection, Func<T, T, int> comparer, bool reverse = false)
        {
            for (int index = collection.Count - 1; index >= 0; index--) {
                for (int child = 1; child <= index; child++) {
                    T d1 = collection [child - 1];
                    T d2 = collection [child];

                    int result = (!reverse) ? comparer (d1, d2) : comparer (d2, d1);
                    if (result > 0) {
                        collection.Remove (d1);
                        collection.Insert (child, d1);
                    }
                }
            }
        }

        /// <summary>
        /// Perform a sort of the items in a collection. This is useful
        /// if the underlying collection does not support sorting. Note that
        /// the object type must be comparable.
        /// </summary>
        /// <param name="collection">Underlying collection to sort</param>
        /// <param name="comparer">Comparer interface</param>
        /// <param name="reverse">True to reverse the collection</param>
        public static void BubbleSort (this IList collection, IComparer comparer, bool reverse = false)
        {
            for (int index = collection.Count - 1; index >= 0; index--) {
                for (int child = 1; child <= index; child++) {
                    object d1 = collection [child - 1];
                    object d2 = collection [child];

                    int result = (!reverse)
                        ? comparer.Compare (d1, d2)
                        : comparer.Compare (d2, d1);

                    if (result > 0) {
                        collection.Remove (d1);
                        collection.Insert (child, d1);
                    }
                }
            }
        }

        /// <summary>
        /// This is used to compare two collections.
        /// </summary>
        /// <typeparam name="T">Collection type</typeparam>
        /// <param name="collection">Collection Source</param>
        /// <param name="other">Collection to compare to</param>
        /// <param name="sameOrderRequired">Require same-order elements (exact match)</param>
        /// <returns></returns>
        public static bool Compare<T> (this ICollection<T> collection, ICollection<T> other, bool sameOrderRequired = false)
        {
            if (!ReferenceEquals (collection, other)) {
                if (other == null)
                    throw new ArgumentNullException ("other");

                // Not the same number of elements.  No match
                if (collection.Count != other.Count)
                    return false;

                // Require same-order; just defer to existing LINQ match
                if (sameOrderRequired)
                    return collection.SequenceEqual (other);

                // Otherwise allow it to be any order, but require same count of each item type.
                var comparer = EqualityComparer<T>.Default;
                return !(from item in collection
                         let thisItem = item
                         where !other.Contains (item, comparer) || collection.Count (check => comparer.Equals (thisItem, check)) != other.Count (check => comparer.Equals (thisItem, check))
                         select item).Any ();
            }

            return true;
        }

        /// <summary>
        /// Add a range of IEnumerable collection to an existing Collection.
        /// </summary>
        ///<typeparam name="T">Type of collection</typeparam>
        ///<param name="collection">Collection</param>
        /// <param name="items">Items to add</param>
        public static void AddRange<T> (this ICollection<T> collection, IEnumerable<T> items)
        {
            if (collection == null)
                throw new ArgumentNullException ("collection");
            if (items == null)
                throw new ArgumentNullException ("items");

            foreach (var item in items)
                collection.Add (item);
        }

        /// <summary>
        /// Removes a set of items from the collection.
        /// </summary>
        /// <param name="collection">Collection.</param>
        public static void RemoveRange<T>(this ICollection<T> collection, IEnumerable<T> items)
        {
            if (collection == null)
                throw new ArgumentNullException ("collection");
            if (items == null)
                throw new ArgumentNullException ("items");

            foreach (var item in items)
                collection.Remove (item);
        }

        ///<summary>
        /// This method tests an enumerable sequence and returns the index of the first item that
        /// passes the test.
        ///</summary>
        ///<typeparam name="T">Type of collection</typeparam>
        ///<param name="collection">Collection</param>
        ///<param name="test">Predicate test</param>
        ///<returns>Index (zero based) of first element that passed test, -1 if none did</returns>
        public static int IndexOf<T> (this IEnumerable<T> collection, Predicate<T> test)
        {
            int pos = 0;
            foreach (var item in collection) {
                if (test (item))
                    return pos;
                pos++;
            }
            return -1;
        }

        /// <summary>
        /// Swap a value in the collection
        /// </summary>
        /// <typeparam name="T">Type of collection</typeparam>
        /// <param name="collection">Source collection</param>
        /// <param name="sourceIndex">Index</param>
        /// <param name="destIndex">Dest index</param>
        public static void Swap<T> (this IList<T> collection, int sourceIndex, int destIndex)
        {
            // Simple parameter checking
            if (sourceIndex < 0 || sourceIndex >= collection.Count)
                throw new ArgumentOutOfRangeException ("sourceIndex");
            if (destIndex < 0 || destIndex >= collection.Count)
                throw new ArgumentOutOfRangeException ("destIndex");

            // Ignore if same index
            if (sourceIndex == destIndex)
                return;

            T temp = collection [sourceIndex];
            collection [sourceIndex] = collection [destIndex];
            collection [destIndex] = temp;
        }

        /// <summary>
        /// This method moves a range of values in the collection
        /// </summary>
        /// <typeparam name="T">Type of collection</typeparam>
        /// <param name="collection">Source collection</param>
        /// <param name="startingIndex">Index</param>
        /// <param name="count">Count of items</param>
        /// <param name="destIndex">Dest index</param>
        public static void MoveRange<T> (this IList<T> collection, int startingIndex, int count, int destIndex)
        {
            // Simple parameter checking
            if (startingIndex < 0 || startingIndex >= collection.Count)
                throw new ArgumentOutOfRangeException ("startingIndex");
            if (destIndex < 0 || destIndex >= collection.Count)
                throw new ArgumentOutOfRangeException ("destIndex");
            if (startingIndex + count > collection.Count)
                throw new ArgumentOutOfRangeException ("count");
            if (count < 0)
                throw new ArgumentOutOfRangeException ("count");

            // Ignore if same index or count is zero
            if (startingIndex == destIndex || count == 0)
                return;

            // Make sure we can modify this directly
            if (collection.GetType ().IsArray)
                throw new NotSupportedException ("Collection is fixed-size and items cannot be efficiently moved.");

            // Go through the collection element-by-element
            var range = Enumerable.Range (0, count);
            if (startingIndex < destIndex)
                range = range.Reverse ();

            foreach (var i in range) {
                int start = startingIndex + i;
                int dest = destIndex + i;

                T item = collection [start];
                collection.RemoveAt (start);
                collection.Insert (dest, item);
            }
        }
    }
}