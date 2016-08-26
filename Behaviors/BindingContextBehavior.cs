//
// BindingContextBehavior.cs
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
using System.Diagnostics;

namespace XamarinUniversity.Infrastructure
{
    /// <summary>
    /// Provides a Behavior(Of T) which forwards the associated object's binding
    /// context to the behavior. This is NOT done by default because behaviors can
    /// be shared if they are applied using a Style. In that case, there is not
    /// a 1:1 relationship. This enforces the 1:1 relationship but disallows the 
    /// application via a shared resource.
    /// </summary>
    public class BindingContextBehavior<T> : Behavior<T> where T : BindableObject
    {
        /// <summary>
        /// True if the binding context is being forwarded.
        /// </summary>
        bool bindingContextForwarded;

        /// <summary>
        /// The single object this behavior is bound to.
        /// </summary>
        /// <value>The associated object.</value>
        protected T AssociatedObject { get; set; }

        /// <summary>
        /// Called when the behavior is attached to an object.
        /// </summary>
        /// <param name="bindable">Bindable.</param>
        protected override void OnAttachedTo (T bindable)
        {
            // Disallow sharing of the behavior since we are associating 
            // to a single object and it's binding context.
            if (AssociatedObject != null) {
                throw new Exception (GetType () + " behaviors cannot be shared or used in a Style setter.");
            }

            base.OnAttachedTo (bindable);
            AssociatedObject = bindable;

            if (BindingContext == null) {
                bindingContextForwarded = true;
                BindingContext = bindable.BindingContext;
                bindable.BindingContextChanged += OnAssociatedBindingContextChanged;
            }
        }

        /// <summary>
        /// Called when this behavior is being detached from a bindable object.
        /// </summary>
        /// <param name="bindable">Bindable.</param>
        protected override void OnDetachingFrom (T bindable)
        {
            Debug.Assert (AssociatedObject == bindable);
            base.OnDetachingFrom (bindable);
            AssociatedObject = null;
            if (bindingContextForwarded) {
                bindable.BindingContextChanged -= OnAssociatedBindingContextChanged;
                BindingContext = null;
            }
        }

        /// <summary>
        /// Raised when our associated object's BindingContext changes.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        void OnAssociatedBindingContextChanged (object sender, EventArgs e)
        {
            Debug.Assert (AssociatedObject == sender);
            Debug.Assert (bindingContextForwarded == true);
            this.BindingContext = ((BindableObject)sender).BindingContext;
        }
   }
}

