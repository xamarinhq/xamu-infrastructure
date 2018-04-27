//
// IntegerToBooleanConverter.cs
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
    /// Converts an integer value into a boolean true/false
    /// </summary>
    public class IntegerToBooleanConverter : IMarkupExtension, IValueConverter
    {
        /// <summary>
        /// Boolean value for zero; defaults to false.
        /// </summary>
        public bool ZeroOrNull { get; set; }

        /// <summary>
        /// Boolean value for one; defaults to true.
        /// </summary>
        public bool One { get; set; }

        /// <summary>
        /// Boolean value for non-zero; defaults to true.
        /// </summary>
        public bool Positive { get; set; }

        /// <summary>
        /// Boolean value for negative treqtment; defaults to false.
        /// </summary>
        /// <value><c>true</c> if negative treatment; otherwise, <c>false</c>.</value>
        public bool Negative { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public IntegerToBooleanConverter()
        {
            Positive = true;
            One = true;
            ZeroOrNull = false;
            Negative = false;
        }

        #region IValueConverter Members

        /// <summary>
        /// Converts a value. 
        /// </summary>
        /// <returns>
        /// A converted value. If the method returns null, the valid null value is used.
        /// </returns>
        /// <param name="value">The value produced by the binding source.</param><param name="targetType">The type of the binding target property.</param><param name="parameter">The converter parameter to use.</param><param name="culture">The culture to use in the converter.</param>
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return ZeroOrNull;

            int result = System.Convert.ToInt32 (value, culture);

            return result < 0 ? Negative 
                : result == 0 ? ZeroOrNull 
                    : result == 1 ? One : Positive;
        }

        /// <summary>
        /// Converts a value. 
        /// </summary>
        /// <returns>
        /// A converted value. If the method returns null, the valid null value is used.
        /// </returns>
        /// <param name="value">The value that is produced by the binding target.</param><param name="targetType">The type to convert to.</param><param name="parameter">The converter parameter to use.</param><param name="culture">The culture to use in the converter.</param>
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion

        /// <summary>
        /// Returns the converter
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <returns></returns>
        public object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}
