//
// DependencyServiceExtension.cs
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
using System.Reflection;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace XamarinUniversity.Infrastructure
{
    /// <summary>
    /// This markup extension allows XAML to lookup dependencies 
    /// using the <see cref="DependencyService"/>.
    /// </summary>
    /// <example>
    /// <code>
    /// BindingContext="{DependencyService Type={x:Type someVMType}}"
    /// </code>
    /// </example>
    [ContentProperty ("Type")]
    public class DependencyServiceExtension : IMarkupExtension
    {
        /// <summary>
        /// Fetch target type for <seealso cref="DependencyService"/>
        /// </summary>
        /// <value>The fetch target.</value>
        public DependencyFetchTarget FetchTarget { get; set; }

        /// <summary>
        /// Type to retrieve (interface or class)
        /// </summary>
        /// <value>The type.</value>
        public Type Type { get; set; }

        /// <summary>
        /// Initializes the markup extension
        /// </summary>
        public DependencyServiceExtension()
        {
            // Default is a global instance.
            FetchTarget = DependencyFetchTarget.GlobalInstance;
        }

        /// <summary>
        /// Looks up the specified type and returns it to the XAML parser.
        /// </summary>
        /// <returns>Retrieved object</returns>
        /// <param name="serviceProvider">Service provider.</param>
        public object ProvideValue(IServiceProvider serviceProvider)
        {
            if (Type == null)
                throw new InvalidOperationException("Type argument mandatory for DependencyService extension");

            // DependencyService.Get<T>();
            var mi = typeof (DependencyService).GetTypeInfo().GetDeclaredMethod("Get");
            var cmi = mi.MakeGenericMethod(Type);
            return cmi.Invoke(null, new object[] { FetchTarget });
        }
    }
}
