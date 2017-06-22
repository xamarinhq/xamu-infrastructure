//
// ColorExtensions.cs
//
// Author:
//       Mark Smith <mark.smith@xamarin.com>
//
// Copyright (c) 2017 Xamarin, Microsoft.
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

using Xamarin.Forms;
using System;
using System.Linq;
using System.Reflection;

namespace XamarinUniversity.Infrastructure
{
    /// <summary>
    /// Extension methods for the Xamarin.Forms <c>Color</c> type.
    /// </summary>
    public static class ColorExtensions
    {
        /// <summary>
        /// Get the name of a given color - either the property or Hex value.
        /// This is useful because the default ToString implementation reports
        /// the underlying field values.
        /// </summary>
        /// <returns>String name</returns>
        /// <param name="color">Color.</param>
        public static string GetName(this Color color)
        {
            return typeof(Color).GetTypeInfo().DeclaredFields
                    .FirstOrDefault(c => c.FieldType == typeof(Color)
                        && ((Color)c.GetValue(null)).Equals(color))?.Name
                        ?? ToHex(color);
        }

        /// <summary>
        /// Get the hex (#rrggbb) value for a color.
        /// </summary>
        /// <returns>String hex value</returns>
        /// <param name="color">Color.</param>
        public static string ToHex(this Color color)
        {
            return $"#{(int)Math.Floor(color.R * 255):X2}{(int)Math.Floor(color.G * 255):X2}{(int)Math.Floor(color.B * 255):X2}";
        }
    }
}

