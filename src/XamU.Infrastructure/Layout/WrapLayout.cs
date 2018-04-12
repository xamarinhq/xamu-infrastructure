//
// WrapLayout.cs
//
// Author:
//       Documentation team - https://developer.xamarin.com/samples/xamarin-forms/UserInterface/CustomLayout/WrapLayout/
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
using Xamarin.Forms;

namespace XamarinUniversity.Layout
{
    /// <summary>
    /// This class provides a wrapping layout container for Xamarin.Forms
    /// </summary>
    public class WrapLayout : Layout<View>
    {
        /// <summary>
        /// Internal structure to hold data about the structure of the panel.
        /// </summary>
        struct LayoutData
        {
            public int VisibleChildCount;
            public Size CellSize;
            public int Rows;
            public int Columns;

            public LayoutData(int visibleChildCount, Size cellSize, int rows, int columns)
            {
                VisibleChildCount = visibleChildCount;
                CellSize = cellSize;
                Rows = rows;
                Columns = columns;
            }
        }

        /// <summary>
        /// Bindable property definition for the ColumnSpacing property
        /// </summary>
        public static readonly BindableProperty ColumnSpacingProperty = BindableProperty.Create(
            nameof(ColumnSpacing), typeof(double), typeof(WrapLayout), 5.0,
            propertyChanged: (bindable, oldvalue, newvalue) => ((WrapLayout)bindable).InvalidateLayout());

        /// <summary>
        /// This controls the spacing between columns
        /// </summary>
        public double ColumnSpacing
        {
            set => SetValue(ColumnSpacingProperty, value);
            get => (double)GetValue(ColumnSpacingProperty);
        }

        /// <summary>
        /// Bindable property definition for the ColumnSpacing property
        /// </summary>
        public static readonly BindableProperty RowSpacingProperty = BindableProperty.Create(
                nameof(RowSpacing), typeof(double), typeof(WrapLayout), 5.0,
                propertyChanged: (bindable, oldvalue, newvalue) => ((WrapLayout)bindable).InvalidateLayout());

        /// <summary>
        /// This controls the spacing between rows
        /// </summary>
        public double RowSpacing
        {
            set { SetValue(RowSpacingProperty, value); }
            get { return (double)GetValue(RowSpacingProperty); }
        }

        /// <summary>
        /// Initial size calculation performed to determine how much space is required to 
        /// display the child elements
        /// </summary>
        /// <param name="widthConstraint">The width constraint to request.</param>
        /// <param name="heightConstraint">The height constraint to request.</param>
        /// <summary>Method that is called when a layout measurement happens.</summary>
        /// <returns>To be added.</returns>
        /// <remarks>To be added.</remarks>
        protected override SizeRequest OnMeasure(double widthConstraint, double heightConstraint)
        {
            LayoutData layoutData = GetLayoutData(widthConstraint, heightConstraint);
            if (layoutData.VisibleChildCount == 0)
            {
                return new SizeRequest();
            }

            Size totalSize = new Size((layoutData.CellSize.Width * layoutData.Columns) + ColumnSpacing * (layoutData.Columns - 1),
                                      (layoutData.CellSize.Height * layoutData.Rows) + RowSpacing * (layoutData.Rows - 1));
            return new SizeRequest(totalSize);
        }

        /// <summary>
        /// Second pass which performs the actual layout of children based on the available
        /// size given to the layout panel.
        /// </summary>
        /// <param name="x">X coordinate</param>
        /// <param name="y">Y coordinate</param>
        /// <param name="width">Width available</param>
        /// <param name="height">Height available</param>
        protected override void LayoutChildren(double x, double y, double width, double height)
        {
            LayoutData layoutData = GetLayoutData(width, height);
            if (layoutData.VisibleChildCount == 0)
            {
                return;
            }

            double xChild = x;
            double yChild = y;
            int row = 0;
            int column = 0;

            foreach (View child in Children)
            {
                if (!child.IsVisible)
                {
                    continue;
                }

                LayoutChildIntoBoundingRegion(child, new Rectangle(new Point(xChild, yChild), layoutData.CellSize));

                if (++column == layoutData.Columns)
                {
                    column = 0;
                    row++;
                    xChild = x;
                    yChild += RowSpacing + layoutData.CellSize.Height;
                }
                else
                {
                    xChild += ColumnSpacing + layoutData.CellSize.Width;
                }
            }
        }

        /// <summary>
        /// Calculate the rows/columns to use for the given width/height and cache it off.
        /// Ideally, we will only calculate this twice for most cases (orientations), however
        /// desktop apps can resize at will.
        /// </summary>
        /// <param name="width">Available width</param>
        /// <param name="height">Available height</param>
        /// <returns></returns>
        LayoutData GetLayoutData(double width, double height)
        {
            Size size = new Size(width, height);

            // Check if cached information is available.
            if (layoutDataCache.ContainsKey(size))
            {
                return layoutDataCache[size];
            }

            int visibleChildCount = 0;
            Size maxChildSize = new Size();
            int rows = 0;
            int columns = 0;
            LayoutData layoutData = new LayoutData();

            // Enumerate through all the children.
            foreach (View child in Children)
            {
                // Skip invisible children.
                if (!child.IsVisible)
                    continue;

                // Count the visible children.
                visibleChildCount++;

                // Get the child's requested size.
                SizeRequest childSizeRequest = child.Measure(Double.PositiveInfinity, Double.PositiveInfinity);

                // Accumulate the maximum child size.
                maxChildSize.Width = Math.Max(maxChildSize.Width, childSizeRequest.Request.Width);
                maxChildSize.Height = Math.Max(maxChildSize.Height, childSizeRequest.Request.Height);
            }

            if (visibleChildCount != 0)
            {
                // Calculate the number of rows and columns.
                if (Double.IsPositiveInfinity(width))
                {
                    columns = visibleChildCount;
                    rows = 1;
                }
                else
                {
                    columns = (int)((width + ColumnSpacing) / (maxChildSize.Width + ColumnSpacing));
                    columns = Math.Max(1, columns);
                    rows = (visibleChildCount + columns - 1) / columns;
                }

                // Now maximize the cell size based on the layout size.
                Size cellSize = new Size();

                if (Double.IsPositiveInfinity(width))
                {
                    cellSize.Width = maxChildSize.Width;
                }
                else
                {
                    cellSize.Width = (width - (ColumnSpacing * (columns - 1))) / columns;
                }

                if (Double.IsPositiveInfinity(height))
                {
                    cellSize.Height = maxChildSize.Height;
                }
                else
                {
                    cellSize.Height = (height - (RowSpacing * (rows - 1))) / rows;
                }

                layoutData = new LayoutData(visibleChildCount, cellSize, rows, columns);
            }

            // We don't save off the layout data for desktop because it can have too
            // many variations.
            if (Device.Idiom != TargetIdiom.Desktop)
            {
                layoutDataCache.Add(size, layoutData);
            }

            return layoutData;
        }

        /// <summary>
        /// Invalidates the current layout.
        /// </summary>
        /// <remarks>
        /// Calling this method will invalidate the measure and triggers a new layout cycle.
        /// </remarks>
        protected override void InvalidateLayout()
        {
            base.InvalidateLayout();

            // Discard all layout information for children added or removed.
            layoutDataCache.Clear();
        }

        /// <summary>
        /// Invoked whenever a child of the layout has emitted <see cref="E:Xamarin.Forms.VisualElement.MeaureInvalidated" />. 
        /// Implement this method to add class handling for this event.
        /// </summary>
        protected override void OnChildMeasureInvalidated()
        {
            base.OnChildMeasureInvalidated();

            // Discard all layout information for child size changed.
            layoutDataCache.Clear();
        }

        readonly Dictionary<Size, LayoutData> layoutDataCache = new Dictionary<Size, LayoutData>();
    }
}