//
// ImageResourceConverter.cs
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
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace XamarinUniversity.Converters
{
    /// <summary>
    /// A custom IValueConverter which can be used with a {Binding} to convert
    /// a View Model string-based property into an ImageSource when the image
    /// is stored as an embedded resource. If you are not using a binding, then
    /// you can use the ImageResourceExtension to load an embedded resource.
    /// </summary>
    public class ImageResourceConverter : IValueConverter, IMarkupExtension
	{
        /// <summary>
        /// Prefix to prepend to the Resource ID (e.g. assembly + namespace + folder)
        /// Leave empty if the bound property specifies the full resource ID.
        /// </summary>
        public string Prefix { get; set; }

        /// <summary>
        /// A type in the same assembly as the image. This 
        /// is a required property set if you are in a UWP application
        /// and using .NET Native
        /// </summary>
        /// <value>The assembly.</value>
        public Type ResolvingType { get; set; }

        /// <summary>
        /// Convert a string-based value into an embedded resource
        /// </summary>
        /// <param name="value">Resource ID</param>
        /// <param name="targetType">ImageSource</param>
        /// <param name="parameter">Optional prefix</param>
        /// <param name="culture">Culture</param>
		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
            if (targetType != typeof (ImageSource))
                throw new ArgumentException ("ImageResourceConverter should only be used with Image.Source");

            string resourceId = (value ?? "").ToString ();
            if (string.IsNullOrEmpty (resourceId))
                return null;

            string prefix;
            prefix = parameter != null 
                ? parameter.ToString () 
                : Prefix != null 
                           ? Prefix : "";
            if (!string.IsNullOrEmpty (prefix) 
                    && !prefix.EndsWith (".", StringComparison.Ordinal))
                prefix += ".";

            return ResolvingType != null 
                ? ImageSource.FromResource (prefix + resourceId, ResolvingType) 
                : ImageSource.FromResource (prefix + resourceId);
        }

        /// <summary>
        /// Used to convert a value from target > source; not used for this converter.
        /// </summary>
        /// <returns>Converted value</returns>
        /// <param name="value">Value.</param>
        /// <param name="targetType">Target type.</param>
        /// <param name="parameter">Optional parameter</param>
        /// <param name="culture">Culture.</param>
		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			throw new NotSupportedException();
		}

        /// <summary>
        /// Allows the value converter to be created inline; note that it is not
        /// shared if you use this approach.
        /// </summary>
        /// <returns>The Value Converter</returns>
        /// <param name="serviceProvider">Service provider.</param>
        public object ProvideValue (IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}

