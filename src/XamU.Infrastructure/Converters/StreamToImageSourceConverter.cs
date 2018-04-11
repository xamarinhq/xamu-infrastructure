//
// StreamToImageSourceConverter.cs
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
using System.IO;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace XamarinUniversity.Converters
{
    /// <summary>
    /// Converter that takes an IO.Stream and turns it into an ImageSource.
    /// </summary>
    public class StreamToImageSourceConverter : IValueConverter, IMarkupExtension
    {
        /// <summary>
        /// Convert Stream input to an ImageSource output
        /// </summary>
        /// <param name="value">Stream</param>
        /// <param name="targetType">ImageSource type</param>
        /// <param name="parameter">Optional parameter (not used)</param>
        /// <param name="culture">Culture (not used)</param>
        /// <returns>ImageSource object wrapped around stream</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var stm = value as Stream;
            return stm != null ? ImageSource.FromStream(() => stm) : null;
        }

        /// <summary>
        /// Convert ImageSource back to stream (not supported)
        /// </summary>
        /// <param name="value">ImageSource</param>
        /// <param name="targetType">Stream type</param>
        /// <param name="parameter">Optional parameter (not used)</param>
        /// <param name="culture">Culture (not used)</param>
        /// <returns>ImageSource object wrapped around stream</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Allows value converter to be used inline.
        /// </summary>
        /// <param name="serviceProvider">Service Provider interface</param>
        /// <returns>Value Converter</returns>
        public object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}
