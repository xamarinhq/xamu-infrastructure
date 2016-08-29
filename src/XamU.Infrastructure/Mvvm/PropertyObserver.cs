//
// PropertyObserver.cs
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
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace XamarinUniversity
{
    /// <summary>
    /// Monitors the PropertyChanged event of an object that implements INotifyPropertyChanged,
    /// and executes callback methods (i.e. handlers) registered for properties of that object
    /// using a fluid syntax.
    /// </summary>
    /// <remarks>
    /// The idea for this class was taken from a similar implementation in WPF.
    /// </remarks>
    /// <typeparam name="T">The type of object to monitor for property changes.</typeparam>
    public sealed class PropertyObserver<T> : IDisposable
        where T : class, INotifyPropertyChanged
    {
        private readonly Dictionary<string, Action<T>> pcToHandlerMap;
        private T source;

        /// <summary>
        /// Initializes a new instance of PropertyObserver, which
        /// observes the 'propertySource' object for property changes.
        /// </summary>
        /// <param name="propertySource">The object to monitor for property changes.</param>
        public PropertyObserver (T propertySource)
        {
            if (propertySource == null)
                throw new ArgumentNullException ("propertySource");

            source = propertySource;
            source.PropertyChanged += OnSourcePropertyChanged;
            pcToHandlerMap = new Dictionary<string, Action<T>> ();
        }

        /// <summary>
        /// Source object this observer is monitoring.
        /// </summary>
        /// <value>The source.</value>
        public T Source
        {
            get { return source; }
        }

        /// <summary>
        /// Called on each PropertyChange notification, forwards to handlers.
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">PropertyChangedEventArgs</param>
        void OnSourcePropertyChanged (object sender, PropertyChangedEventArgs e)
        {
            string propertyName = e.PropertyName;
            var propertySource = (T)sender;

            Debug.Assert (propertySource == source);

            // If there's no property, then notify ALL handlers.
            if (string.IsNullOrEmpty (propertyName)) {
                // Get a safe copy of the list
                List<Action<T>> entries = pcToHandlerMap.Values.ToList ();
                foreach (var entry in entries)
                    entry.Invoke (propertySource);
            }
            else
            {
                Action<T> action = null;
                if (pcToHandlerMap.TryGetValue (propertyName, out action))
                {
                    if (action != null)
                        action (propertySource);
                }
            }
        }

        /// <summary>
        /// Registers a callback to be invoked when the PropertyChanged event has been raised for the specified property.
        /// </summary>
        /// <param name="expression">A lambda expression like 'n => n.PropertyName'.</param>
        /// <param name="handler">The callback to invoke when the property has changed.</param>
        /// <returns>The object on which this method was invoked, to allow for multiple invocations chained together.</returns>
        public PropertyObserver<T> RegisterHandler (Expression<Func<T, object>> expression, Action<T> handler)
        {
            if (source == null)
                throw new ObjectDisposedException ("source");
            if (expression == null)
                throw new ArgumentNullException ("expression");

            string propertyName = GetPropertyName (expression);
            if (String.IsNullOrEmpty (propertyName))
                throw new ArgumentException ("'expression' did not provide a property name.");

            if (handler == null)
                throw new ArgumentNullException ("handler");

            pcToHandlerMap.Add (propertyName, handler);
            return this;
        }

        /// <summary>
        /// Removes the callback associated with the specified property.
        /// </summary>
        /// <param name="expression">A lambda expression like 'n => n.PropertyName'.</param>
        /// <returns>The object on which this method was invoked, to allow for multiple invocations chained together.</returns>
        public PropertyObserver<T> UnregisterHandler (Expression<Func<T, object>> expression)
        {
            if (source == null)
                throw new ObjectDisposedException ("source");
            if (expression == null)
                throw new ArgumentNullException ("expression");

            string propertyName = GetPropertyName (expression);
            if (String.IsNullOrEmpty (propertyName))
                throw new ArgumentException ("'expression' did not provide a property name.");

            pcToHandlerMap.Remove (propertyName);

            return this;
        }

        /// <summary>
        /// Retrieves the property name for a given expression.
        /// </summary>
        /// <param name="expression">Expression to evaluate</param>
        /// <returns>Property name</returns>
        private static string GetPropertyName (Expression<Func<T, object>> expression)
        {
            var lambda = expression as LambdaExpression;
            MemberExpression memberExpression;
            if (lambda.Body is UnaryExpression) {
                var unaryExpression = lambda.Body as UnaryExpression;
                memberExpression = unaryExpression.Operand as MemberExpression;
            } else {
                memberExpression = lambda.Body as MemberExpression;
            }

            Debug.Assert (memberExpression != null, "Please provide a lambda expression like 'n => n.PropertyName'");

            if (memberExpression != null) {
                var propertyInfo = memberExpression.Member as PropertyInfo;
                if (propertyInfo != null)
                    return propertyInfo.Name;
            }

            return null;
        }

        #region IDisposable Members

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose ()
        {
            source.PropertyChanged -= OnSourcePropertyChanged;
            pcToHandlerMap.Clear ();
            source = null;
        }

        #endregion
    }
}
