//
// NamedDataTemplateSelector.cs
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

namespace XamarinUniversity.Infrastructure
{
    /// <summary>
    /// This is a simple DataTemplateSelector that matches resources by the typename of the
    /// object being data bound to the ListView.
    /// </summary>
    /// <remarks>
    /// To use it, add a copy into your resources and then assign it as the value for a
    /// <see cref="ListView"/> ItemTemplate. This will evaluate the bound object and, based
    /// on the typename, retrieve a resource (starting at that object and working up to App)
    /// by the name.
    /// </remarks>
    public class NamedDataTemplateSelector : DataTemplateSelector
    {
        /// <summary>
        /// True to strip off the namespace and only look for the base typename.
        /// </summary>
        /// <value><c>true</c> if strip namespace; otherwise, <c>false</c>.</value>
        public bool StripNamespace { get; set; }

        /// <summary>
        /// Retrieves the DataTemplate for a given object using the typename of the 
        /// object as the resource key. Throws an exception if the resource is not found.
        /// </summary>
        /// <returns>The select template.</returns>
        /// <param name="item">Item.</param>
        /// <param name="container">Container.</param>
        protected override DataTemplate OnSelectTemplate (object item, BindableObject container)
        {
            if (item == null)
                throw new Exception ("Cannot create template for null object.");

            Type itemType = item.GetType ();
            string typeName = (StripNamespace) ? itemType.Name : itemType.FullName;

            VisualElement ve = container as VisualElement;
            return ve.FindResource<DataTemplate> (typeName);
        }
   }
}

