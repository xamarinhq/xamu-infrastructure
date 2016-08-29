//
// SimpleViewModel.cs
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
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace XamarinUniversity.Infrastructure
{
    /// <summary>
    /// This is a simple base class for MVVM.
    /// </summary>
    public class SimpleViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Inform any bindings that ALL property values must be read.
        /// </summary>
        protected void RaiseAllPropertiesChanged()
        {
            // By convention, an empty string indicates all properties are invalid.
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(string.Empty));
        }

        /// <summary>
        /// Raises a specific property change event using an expression.
        /// </summary>
        /// <param name="propExpr">Property expr.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        protected void RaisePropertyChanged<T>(Expression<Func<T>> propExpr)
        {
            var prop = (PropertyInfo)((MemberExpression)propExpr.Body).Member;
            this.RaisePropertyChanged(prop.Name);
        }

        /// <summary>
        /// Raises a specific property change event using a string for the property name.
        /// </summary>
        /// <param name="propertyName">Property name.</param>
        protected void RaisePropertyChanged([CallerMemberName] string propertyName= "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Changes a field's value and raises property change notifications.
        /// </summary>
        /// <returns><c>true</c>, if property value was set, <c>false</c> otherwise.</returns>
        /// <param name="storageField">Storage field.</param>
        /// <param name="newValue">New value.</param>
        /// <param name="propExpr">Property expr.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        protected bool SetPropertyValue<T>(ref T storageField, T newValue, Expression<Func<T>> propExpr)
        {
            if (Equals(storageField, newValue))
                return false;

            storageField = newValue;
            var prop = (PropertyInfo)((MemberExpression)propExpr.Body).Member;
            this.RaisePropertyChanged(prop.Name);

            return true;
        }

        /// <summary>
        /// Changes a field's value and raises property change notifications.
        /// </summary>
        /// <returns><c>true</c>, if property value was set, <c>false</c> otherwise.</returns>
        /// <param name="storageField">Storage field.</param>
        /// <param name="newValue">New value.</param>
        /// <param name="propertyName">Property name.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        protected bool SetPropertyValue<T>(ref T storageField, T newValue, [CallerMemberName] string propertyName = "")
        {
            if (Equals(storageField, newValue))
                return false;

            storageField = newValue;
            this.RaisePropertyChanged(propertyName);

            return true;
        }
    }
}
