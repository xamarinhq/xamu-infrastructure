//
// ItemsControl.cs
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

using System.Collections;
using Xamarin.Forms;
using System.Collections.Specialized;
using System;

namespace XamarinUniversity.Controls
{
    /// <summary>
    /// Simple ItemsControl to render a list of things in a stacked view using
    /// either text labels, or an inflated data template. It also includes the ability
    /// to display a text placeholder if no items are present in the data bound collection.
    /// </summary>
    public class ItemsControl : ContentView
    {
        /// <summary>
        /// Bindable property for the placeholder text.
        /// </summary>
        public static readonly BindableProperty PlaceholderTextProperty = BindableProperty.Create (
            nameof(PlaceholderText), typeof (string), typeof (ItemsControl));

        /// <summary>
        /// Gets or sets the placeholder text.
        /// </summary>
        /// <value>The placeholder text.</value>
        public string PlaceholderText {
            get { return (string)GetValue (PlaceholderTextProperty); }
            set { SetValue (PlaceholderTextProperty, value); }
        }

        /// <summary>
        /// Bindable property for the orientation of the layout panel
        /// </summary>
        public static readonly BindableProperty OrientationProperty = BindableProperty.Create(
            nameof(Orientation), typeof(StackOrientation), typeof(ItemsControl), 
            defaultValue: StackOrientation.Vertical,
            propertyChanged: OnOrientationPropertyChanged);

        /// <summary>
        /// Gets or Sets the Orientation for the layout panel
        /// </summary>
        /// <value>Orientation value</value>
        public StackOrientation Orientation {
            get { return (StackOrientation)GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }

        /// <summary>
        /// Bindable property for the Spacing of the layout panel
        /// </summary>
        public static readonly BindableProperty SpacingProperty = BindableProperty.Create(
            nameof(Spacing), typeof(double), typeof(ItemsControl),
            defaultValue: 10.0,
            propertyChanged: OnSpacingPropertyChanged);

        /// <summary>
        /// Gets or Sets the Spacing for the layout panel
        /// </summary>
        /// <value>Spacing value</value>
        public double Spacing
        {
            get { return (double) GetValue(SpacingProperty); }
            set { SetValue(SpacingProperty, value); }
        }

        /// <summary>
        /// Bindable property for the Label style used for each item when there
        /// is no data template assigned.
        /// </summary>
        public static readonly BindableProperty ItemStyleProperty = BindableProperty.Create (
            nameof(ItemStyle), typeof (Style), typeof (ItemsControl), propertyChanged: OnItemStylePropertyChanged);

        /// <summary>
        /// Gets or sets the item style used for dynamically generated labels.
        /// </summary>
        /// <value>The item style.</value>
        public Style ItemStyle {
            get { return (Style)GetValue (ItemStyleProperty); }
            set { SetValue (ItemStyleProperty, value); }
        }

        /// <summary>
        /// Bindable property for the data source
        /// </summary>
        public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create (
            nameof(ItemsSource), typeof (IList), typeof (ItemsControl), propertyChanging: OnItemsSourceChanged);

        /// <summary>
        /// Gets or sets the items source - can be any collection of elements.
        /// </summary>
        /// <value>The items source.</value>
        public IList ItemsSource {
            get { return (IList)GetValue (ItemsSourceProperty); }
            set { SetValue (ItemsSourceProperty, value); }
        }

        /// <summary>
        /// Bindable property for the data template to visually represent each item.
        /// </summary>
        public static readonly BindableProperty ItemsTemplateProperty = BindableProperty.Create (
            nameof(ItemsTemplate), typeof (DataTemplate), typeof (ItemsControl));

        /// <summary>
        /// Gets or sets the item template used to generate the visuals for a single item.
        /// </summary>
        /// <value>The item template.</value>
        public DataTemplate ItemsTemplate {
            get { return (DataTemplate)GetValue (ItemsTemplateProperty); }
            set { SetValue (ItemsTemplateProperty, value); }
        }

        // Data
        StackLayout stack;
        Label noItemsLabel;

        /// <summary>
        /// Initializes an ItemsControl.
        /// </summary>
        public ItemsControl ()
        {
            Padding = new Thickness(5,0,5,5);

            stack = new StackLayout {
                Spacing = this.Spacing,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                Orientation = this.Orientation
            };

            noItemsLabel = new Label {
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center
            };
            noItemsLabel.SetBinding (Label.StyleProperty, new Binding(nameof(ItemStyle), source: this));
            noItemsLabel.SetBinding (Label.TextProperty, new Binding (nameof (PlaceholderText), source: this));

            Content = noItemsLabel;
        }

        /// <summary>
        /// This is called when the Orientation property is changed
        /// </summary>
        /// <param name="bindable">ItemsSource</param>
        /// <param name="oldValue">Old value.</param>
        /// <param name="newValue">New value.</param>
        static void OnOrientationPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            ((ItemsControl)bindable).OnOrientationPropertyChangedImpl((StackOrientation)oldValue, (StackOrientation)newValue);
        }

        /// <summary>
        /// Instance method used to change the current orientation of the layout panel.
        /// </summary>
        /// <param name="oldValue">Old value.</param>
        /// <param name="newValue">New value.</param>
        private void OnOrientationPropertyChangedImpl(StackOrientation oldValue, StackOrientation newValue)
        {
            if (oldValue != newValue)
            {
                stack.Orientation = newValue;
            }
        }

        /// <summary>
        /// This is called when the Spacing property is changed
        /// </summary>
        /// <param name="bindable">ItemsSource</param>
        /// <param name="oldValue">Old value.</param>
        /// <param name="newValue">New value.</param>
        static void OnSpacingPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            ((ItemsControl)bindable).OnSpacingPropertyChangedImpl((double)oldValue, (double)newValue);
        }

        /// <summary>
        /// Instance method used to change the current spacing of the layout panel.
        /// </summary>
        /// <param name="oldValue">Old value.</param>
        /// <param name="newValue">New value.</param>
        private void OnSpacingPropertyChangedImpl(double oldValue, double newValue)
        {
            if (oldValue != newValue)
            {
                stack.Spacing = newValue;
            }
        }


        /// <summary>
        /// This is called when the underlying data source is changed.
        /// </summary>
        /// <param name="bindable">ItemsSource</param>
        /// <param name="oldValue">Old value.</param>
        /// <param name="newValue">New value.</param>
        static void OnItemsSourceChanged (BindableObject bindable, object oldValue, object newValue)
        {
            ((ItemsControl)bindable).OnItemsSourceChangedImpl ((IList)oldValue, (IList)newValue);
        }

        /// <summary>
        /// Instance method called when the underlying data source is changed through the
        /// <see cref="ItemsSource"/> property. This re-generates the list based on the 
        /// new collection.
        /// </summary>
        /// <param name="oldValue">Old value.</param>
        /// <param name="newValue">New value.</param>
        void OnItemsSourceChangedImpl(IList oldValue, IList newValue)
        {
            // Unsubscribe from the old collection
            if (oldValue != null) {
                INotifyCollectionChanged ncc = oldValue as INotifyCollectionChanged;
                if (ncc != null)
                    ncc.CollectionChanged -= OnCollectionChanged;
            }

            if (newValue == null) {
                stack.Children.Clear ();
                Content = noItemsLabel;
            }
            else {
                Content = stack;
                FillContainer (newValue);
                INotifyCollectionChanged ncc = newValue as INotifyCollectionChanged;
                if (ncc != null)
                    ncc.CollectionChanged += OnCollectionChanged;
            }
        }

        /// <summary>
        /// Called when the Label style is changed.
        /// </summary>
        /// <param name="bindable">ItemsControl</param>
        /// <param name="oldValue">Old value.</param>
        /// <param name="newValue">New value.</param>
        static void OnItemStylePropertyChanged (BindableObject bindable, object oldValue, object newValue)
        {
            ((ItemsControl)bindable).OnItemStylePropertyChangedImpl (newValue as Style);
        }

        /// <summary>
        /// Instance method called when the label style is changed through the
        /// <see cref="ItemStyle"/> property. This applies the new style to all the labels.
        /// </summary>
        /// <param name="style">Style.</param>
        void OnItemStylePropertyChangedImpl (Style style)
        {
            // Ignore if we have a data template.
            if (ItemsTemplate != null)
                return;

            foreach (View view in stack.Children)
            {
                Label label = view as Label;
                if (label != null) {

                    if (style == null)
                        label.ClearValue (Label.StyleProperty);
                    else
                        label.Style = style;
                }
            }
        }

        /// <summary>
        /// This method takes our items source and generates visuals for
        /// each item in the collection; it can reuse visuals which were created
        /// previously and simply changes the binding context.
        /// </summary>
        /// <param name="newValue">New items to display</param>
        void FillContainer (IList newValue)
        {
            var itemStyle = ItemStyle;
            var template = ItemsTemplate;
            var visuals = stack.Children;
            
            for (int i = 0; i < this.stack.Children.Count; i++)
            {
                this.stack.Children[i].IsVisible = i < newValue.Count;
            }

            for (int i = 0; i < newValue.Count; i++) {
                var dataItem = newValue [i];

                if (visuals.Count > i)
                {
                    if (template != null)
                    {
                        var visualItem = visuals [i];
                        visualItem.BindingContext = dataItem;
                    }    
                    else
                    {
                        Label visualItem = (Label) visuals [i];
                        visualItem.Text = dataItem.ToString ();
                        if (itemStyle != null) {
                            visualItem.Style = itemStyle;
                        } else {
                            visualItem.ClearValue (Label.StyleProperty);
                        }
                    }
                }
                else
                {
                    if (template != null) {
                        InflateTemplate (template, dataItem);
                    } else {
                        var label = new Label { Text = dataItem.ToString () };
                        if (itemStyle != null) {
                            label.Style = itemStyle;
                        }
                        stack.Children.Add (label);
                    }
                }
            }

            Content = (stack.Children.Count == 0) ? (View) noItemsLabel : stack;
        }

        /// <summary>
        /// Inflates the visuals for a data template or template selector
        /// and adds it to our StackLayout.
        /// </summary>
        /// <param name="template">Template.</param>
        /// <param name="item">Item.</param>
        void InflateTemplate (DataTemplate template, object item)
        {
            // Pull real template from selector if necessary.
            var dSelector = template as DataTemplateSelector;
            if (dSelector != null)
                template = dSelector.SelectTemplate (item, this);

            var view = template.CreateContent () as View;
            if (view != null) {
                view.BindingContext = item;
                stack.Children.Add (view);
            }
        }

        /// <summary>
        /// This is called when the data source collection implements
        /// collection change notifications and the data has changed.
        /// This is not optimized - it simply replaces all the data.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        void OnCollectionChanged (object sender, NotifyCollectionChangedEventArgs e)
        {
            FillContainer ((IList)sender);
        }
   }
}

