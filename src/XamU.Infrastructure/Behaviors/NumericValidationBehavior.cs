//
// NumericValidationBehavior.cs
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

using Xamarin.Forms;

namespace XamarinUniversity.Infrastructure
{
    /// <summary>
    /// A custom behavior for the Xamarin.Forms Entry control to 
    /// restrict the input to be numeric only in the form of a double or integer.
    /// </summary>
    public class NumericValidationBehavior : Behavior<Entry>
    {
        #region AllowDecimalProperty
        /// <summary>
        /// Backing storage for the boolean flag which decides between
        /// integer vs. double validation.
        /// </summary>
        public static BindableProperty AllowDecimalProperty =
            BindableProperty.Create ("AllowDecimal",
                typeof (bool), typeof (NumericValidationBehavior),
                true, BindingMode.OneWay);

        /// <summary>
        /// Bindable property to hold the boolean flag which decides
        /// whether we test for integer vs. double values.
        /// </summary>
        /// <value>The selected item.</value>
        public bool AllowDecimal {
            get { return (bool)base.GetValue (AllowDecimalProperty); }
            set { base.SetValue (AllowDecimalProperty, value); }
        }
        #endregion


        #region InvalidColorProperty
        /// <summary>
        /// Backing storage for the color used when the
        /// Entry has invalid data (non-numeric).
        /// </summary>
        public static BindableProperty InvalidColorProperty =
            BindableProperty.Create ("InvalidColor",
                typeof (Color), typeof (NumericValidationBehavior),
                Color.Red, BindingMode.OneWay);

        /// <summary>
        /// Bindable property to hold the color used when the
        /// Entry has invalid data (non-numeric).
        /// </summary>
        /// <value>The selected item.</value>
        public Color InvalidColor {
            get { return (Color) base.GetValue (InvalidColorProperty); }
            set { base.SetValue (InvalidColorProperty, value); }
        }
        #endregion

        /// <summary>
        /// Called when this behavior is attached to a visual.
        /// </summary>
        /// <param name="bindable">Visual owner</param>
        protected override void OnAttachedTo(Entry bindable)
        {
            base.OnAttachedTo(bindable);
            bindable.TextChanged += OnEntryTextChanged;
        }

        /// <summary>
        /// Called when this behavior is detached from a visual
        /// </summary>
        /// <param name="bindable">Visual owner</param>
        protected override void OnDetachingFrom(Entry bindable)
        {
            base.OnDetachingFrom(bindable);
            bindable.TextChanged -= OnEntryTextChanged;
        }

        /// <summary>
        /// Called when the associated Entry has new text.
        /// This changes the text color to reflect whether the data
        /// is valid.
        /// </summary>
        /// <param name="sender">Entry control</param>
        /// <param name="args">TextChanged event arguments</param>
        void OnEntryTextChanged(object sender, TextChangedEventArgs args)
        {
            bool isValid = false;
            if (AllowDecimal)
            {
                double result;
                isValid = double.TryParse (args.NewTextValue, out result);
            }
            else
            {
                long result;
                isValid = long.TryParse (args.NewTextValue, out result);
            }

            ((Entry)sender).TextColor = isValid ? Color.Default : InvalidColor;
        }
    }}

