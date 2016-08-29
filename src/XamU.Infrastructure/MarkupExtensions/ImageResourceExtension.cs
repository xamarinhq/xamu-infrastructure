//
// ImageResourceExtension.cs
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
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Reflection;

namespace XamarinUniversity.Infrastructure
{
    /// <summary>
    /// XAML markup extension to load an image from embedded resources.
    /// </summary>
    [ContentProperty ("Source")]
    public class ImageResourceExtension : IMarkupExtension<ImageSource>
    {
        /// <summary>
        /// Optional System.Type used to identify the assembly where
        /// the resources are located
        /// </summary>
        /// <value>The type of the assembly resolver.</value>
        public Type AssemblyResolverType { get; set; }

        /// <summary>
        /// Resource ID which identifies the image
        /// </summary>
        /// <value>The source.</value>
        public string Source { get; set; }

        /// <summary>
        /// Returns the image
        /// </summary>
        /// <returns>The value.</returns>
        /// <param name="serviceProvider">Service provider.</param>
        object IMarkupExtension.ProvideValue (IServiceProvider serviceProvider)
        {
            return (this as IMarkupExtension<ImageSource>).ProvideValue (serviceProvider);
        }

        /// <summary>
        /// Returns the image
        /// </summary>
        /// <returns>The value.</returns>
        /// <param name="serviceProvider">Service provider.</param>
        public ImageSource ProvideValue (IServiceProvider serviceProvider)
        {
            Assembly assembly = null;
            if (AssemblyResolverType != null) {
                assembly = AssemblyResolverType.GetTypeInfo ().Assembly;
            } 
            else {
                var app = Application.Current;
                if (app != null) {
                    assembly = app.GetType ().GetTypeInfo ().Assembly;
                }
            }

            return Source == null ? null : ImageSource.FromResource (Source, assembly);        
        }
    }
}

