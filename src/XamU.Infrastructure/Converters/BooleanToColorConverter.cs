//
// BooleanToColorConverter.cs
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
using System.Globalization;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace XamarinUniversity.Converters
{
    /// <summary>
    /// This converter converts a boolean (true/false) value into a specific
    /// color.
    /// </summary>
    public class BooleanToColorConverter : IValueConverter, IMarkupExtension
    {
        /// <summary>
        /// The color to use for TRUE
        /// </summary>
        public Color TrueColor { get; set; }

        /// <summary>
        /// The color to use for FALSE
        /// </summary>
        public Color FalseColor { get; set; }

        /// <summary>
        /// Constructor - sets the initial colors.
        /// </summary>
        public BooleanToColorConverter()
        {
            TrueColor = Color.Black;
            FalseColor = Color.LightGray;
        }

        /// <summary>
        /// Convert a boolean into a Color value
        /// </summary>
        /// <param name="value">Boolean value</param>
        /// <param name="targetType">Returning color</param>
        /// <param name="parameter">Parameter (not used)</param>
        /// <param name="culture">Culture (not used)</param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType != typeof(Color))
                throw new Exception($"{nameof(BooleanToColorConverter)} can only be used with a Color target.");

            return (value as bool? ?? false) ? TrueColor : FalseColor;
        }

        /// <summary>
        /// ConvertBack (not supported)
        /// </summary>
        /// <param name="value">Color</param>
        /// <param name="targetType">Boolean</param>
        /// <param name="parameter">Optional parameter (not used)</param>
        /// <param name="culture">Culture (not used)</param>
        /// <returns></returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Color color = (Color)value;
            return (color == TrueColor) ? true : false;
        }

        /// <summary>
        /// Returns an instance of the ValueConverter as an inline element.
        /// </summary>
        /// <param name="serviceProvider">Service Provider for XAML services</param>
        /// <returns>Value Converter</returns>
        public object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}
