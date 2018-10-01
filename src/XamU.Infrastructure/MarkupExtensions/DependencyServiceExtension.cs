//
// DependencyServiceExtension.cs
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
using System.Linq;
using System.Reflection;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XamarinUniversity.Services;

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
    [ContentProperty("Type")]
    public class DependencyServiceExtension : IMarkupExtension
    {
        /// <summary>
        /// Fetch target type for <seealso cref="DependencyService"/>
        /// </summary>
        /// <value>The fetch target.</value>
        public DependencyScope Scope { get; set; }

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
            Scope = DependencyScope.Global;
        }

        /// <summary>
        /// Looks up the specified type and returns it to the XAML parser.
        /// </summary>
        /// <returns>Retrieved object</returns>
        /// <param name="serviceProvider">Service provider.</param>
        public object ProvideValue(IServiceProvider serviceProvider)
        {
            if (Type == null)
                throw new InvalidOperationException("Type argument mandatory for DependencyService extension.");

            var ds = XamUInfrastructure.ServiceLocator;
            if (ds == null)
                throw new InvalidOperationException("DependencyService extension requires XamUInfrastructure.Init.");

            // ds.Get<T>(Scope);
            var mi = ds.GetType().GetTypeInfo().GetDeclaredMethods("Get").First(m => m.GetParameters().Length == 1);
            var cmi = mi.MakeGenericMethod(Type);
            var result = cmi.Invoke(ds, new object[] { Scope });

            return result;
        }
    }

}
