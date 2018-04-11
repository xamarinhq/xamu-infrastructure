//
// RelativeBindingContext.cs
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
using System.Diagnostics;

namespace XamarinUniversity.Infrastructure
{
    /// <summary>
    /// This markup extension will locate the given element by name, grab the 
    /// associated BindingContext and return it. This allows you to forward a 
    /// BindingContext from some other element with a simpler syntax.
    /// </summary>
    [ContentProperty ("Name")]
    public class RelativeBindingContext : IMarkupExtension
    {
        BindableObject associatedObject;

        /// <summary>
        /// The name of the Element in the XAML file to grab the 
        /// BindingContext from.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }

        /// <summary>
        /// True to track binding changes and apply them; this is the 
        /// default. Set this property to FALSE if you are not applying 
        /// the value to the BindingContext.
        /// </summary>
        /// <value><c>true</c> if track binding changes; otherwise, <c>false</c>.</value>
        public bool TrackBindingChanges { get; set; }

        /// <summary>
        /// Construct a new RelativeBindinContext.
        /// </summary>
        public RelativeBindingContext ()
        {
            TrackBindingChanges = true;
        }

        /// <summary>
        /// Retrieves the BindingContext from the named element.
        /// </summary>
        /// <returns>BindingContext value or null</returns>
        /// <param name="serviceProvider">Service provider.</param>
        public object ProvideValue (IServiceProvider serviceProvider)
        {
            Debug.Assert (serviceProvider != null);
            if (string.IsNullOrEmpty (Name))
                throw new ArgumentNullException ("RelativeBindingContext: Name must be provided.");

            var pvt = serviceProvider.GetService (typeof (IProvideValueTarget)) as IProvideValueTarget;
            var rootProvider = serviceProvider.GetService (typeof(IRootObjectProvider)) as IRootObjectProvider;
            Debug.Assert (pvt != null && rootProvider != null);

            // BUGBUG: unfortunately, Forms currently does not implement the 'TargetProperty'
            // value from IProvideValueTarget. That means we cannot tell what property is
            // being assigned here; so we assume BindingContext but allow you to manually
            // switch that off through the TrackBindingChanges property for this extension.

            var root = rootProvider.RootObject as Element;
            if (root != null) {
                var namedElement = root.FindByName<Element> (Name);
                if (namedElement != null) {
                    if (TrackBindingChanges) {
                        associatedObject = pvt.TargetObject as BindableObject;
                        if (associatedObject != null) {
                            associatedObject.BindingContext = namedElement.BindingContext;
                            namedElement.BindingContextChanged += OnBindingContextChanged;
                        }
                    }
                    return namedElement.BindingContext;
                }
            }

            return null;
        }

        /// <summary>
        /// This is called when the named element's binding context has changed.
        /// We forward this back to our associated object.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        void OnBindingContextChanged (object sender, EventArgs e)
        {
            associatedObject.BindingContext = ((BindableObject)sender).BindingContext;
        }
   }
}

