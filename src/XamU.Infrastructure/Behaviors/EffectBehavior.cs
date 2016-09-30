//
// EffectBehavior.cs
//
// Author:
//       David Britch <david.britch@xamarin.com>
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
	/// This behavior adds an Effect instance to a control when the behavior is attached to the control,
	/// and removes the Effect instance when the behavior is detached from the control.
	/// </summary>
	/// <example>
	/// <!--[CDATA[
	/// <Label Text = "Label Shadow Effect">
	/// 	<Label.Behaviors>
	///			<inf:EffectBehavior Group="Xamarin" Name="LabelShadowEffect" />
	/// 	</Label.Behaviors>
	/// </Label>
	/// ]]>-->
	/// </example>
	public class EffectBehavior : Behavior<View>
	{
		/// <summary>
		/// Bindable property for the ResolutionGroupName attribute of the Effect.
		/// </summary>
		public static readonly BindableProperty GroupProperty = BindableProperty.Create("Group", typeof(string), typeof(EffectBehavior), null);

		/// <summary>
		/// Bindable property for the ExportEffect attribute of the Effect.
		/// </summary>
		public static readonly BindableProperty NameProperty = BindableProperty.Create("Name", typeof(string), typeof(EffectBehavior), null);

		/// <summary>
		/// The group name of the Effect
		/// </summary>
		/// <value>The ResolutionGroupName value of the Effect.</value>
		public string Group
		{
			get { return (string)GetValue(GroupProperty); }
			set { SetValue(GroupProperty, value); }
		}

		/// <summary>
		/// The name of the Effect.
		/// </summary>
		/// <value>The ExportEffect value of the Effect.</value>
		public string Name
		{
			get { return (string)GetValue(NameProperty); }
			set { SetValue(NameProperty, value); }
		}

		/// <summary>
		/// Called when the behavior is attached to an element.
		/// </summary>
		/// <param name="bindable">The attached object.</param>
		protected override void OnAttachedTo(BindableObject bindable)
		{
			base.OnAttachedTo(bindable);
			AddEffect(bindable as View);
		}

		/// <summary>
		/// Called when the behavior is being removed from an element.
		/// </summary>
		/// <param name="bindable">The object being detached from.</param>
		protected override void OnDetachingFrom(BindableObject bindable)
		{
			RemoveEffect(bindable as View);
			base.OnDetachingFrom(bindable);
		}

		/// <summary>
		/// Adds the Effect to the element's Effects collection.
		/// </summary>
		/// <param name="view">The View to add the Effect to.</param>
		void AddEffect(View view)
		{
			var effect = GetEffect();
			if (effect != null)
			{
				view.Effects.Add(GetEffect());
			}
		}

		/// <summary>
		/// Removes the Effect from the element's Effects collection.
		/// </summary>
		/// <param name="view">The View to remove the Effect from.</param>
		void RemoveEffect(View view)
		{
			var effect = GetEffect();
			if (effect != null)
			{
				view.Effects.Remove(GetEffect());
			}
		}

		/// <summary>
		/// Resolves the Effect to be added to an element.
		/// </summary>
		/// <returns>The resolved Effect.</returns>
		Effect GetEffect()
		{
			if (!string.IsNullOrWhiteSpace(Group) && !string.IsNullOrWhiteSpace(Name))
			{
				return Effect.Resolve(string.Format("{0}.{1}", Group, Name));
			}
			return null;
		}
	}
}
