//
// BooleanToImageSourceConverter.cs
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
using System.Globalization;
using System.Reflection;
using System.Text;
using Xamarin.Forms;

namespace XamarinUniversity.Converters
{
    /// <summary>
    /// Image source type (file, URL, or resource)
    /// </summary>
    public enum ImageSourceType
    {
        /// <summary>
        /// Image is a Filename
        /// </summary>
        File,
        /// <summary>
        /// Image is stored as an EmbeddedResource
        /// </summary>
        Resource,
        /// <summary>
        /// Image is a URL
        /// </summary>
        Uri
    }

    /// <summary>
    /// Converter to take a Boolean (true/false) value and turn it into an ImageSource
    /// </summary>
    public class BooleanToImageSourceConverter : IValueConverter
    {
        /// <summary>
        /// Source type (file, URL, or resource); defaults to FILE
        /// </summary>
        public ImageSourceType SourceType { get; set; }

        /// <summary>
        /// Image to use when value is TRUE
        /// </summary>
        public string TrueImageSource { get; set; }

        /// <summary>
        /// Image to use when value if FALSE
        /// </summary>
        public string FalseImageSource { get; set; }

        /// <summary>
        /// Prefix to prepend to the ImageSource id (e.g. assembly + namespace + folder)
        /// Leave empty if the bound property specifies the full resource ID/URL/filename
        /// </summary>
        public string Prefix { get; set; }

        /// <summary>
        /// A type in the same assembly as the image. This 
        /// is a required property if the type is a resource and
        /// you are in a UWP application and using .NET Native
        /// </summary>
        /// <value>The assembly.</value>
        public Type ResolvingType { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public BooleanToImageSourceConverter()
        {
            SourceType = ImageSourceType.File;
        }

        /// <summary>
        /// Convert a boolean into an ImageSource
        /// </summary>
        /// <param name="value">Boolean value</param>
        /// <param name="targetType">Returning image source</param>
        /// <param name="parameter">Parameter (not used)</param>
        /// <param name="culture">Culture (not used)</param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType != typeof(ImageSource))
                throw new Exception($"{nameof(BooleanToImageSourceConverter)} can only be used with an ImageSource target.");

            string id = (value as bool? ?? false) ? TrueImageSource : FalseImageSource;
            if (!string.IsNullOrEmpty(id))
            {
                if (!string.IsNullOrWhiteSpace(Prefix))
                {
                    id = Prefix + id;
                }

                if (SourceType == ImageSourceType.Uri)
                {
                    return ImageSource.FromUri(new Uri(id));
                }
                else if (SourceType == ImageSourceType.File)
                {
                    return ImageSource.FromFile(id);
                }
                else
                {
                    if (ResolvingType != null)
                    {
                        return ImageSource.FromResource(id, ResolvingType);
                    }
                    else
                    {
                        // Use the MainPage assembly.
                        return ImageSource.FromResource(id, Application.Current.MainPage?.GetType().GetTypeInfo().Assembly);
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// ConvertBack (not supported)
        /// </summary>
        /// <param name="value">ImageSource</param>
        /// <param name="targetType">Boolean</param>
        /// <param name="parameter">Optional parameter (not used)</param>
        /// <param name="culture">Culture (not used)</param>
        /// <returns></returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
