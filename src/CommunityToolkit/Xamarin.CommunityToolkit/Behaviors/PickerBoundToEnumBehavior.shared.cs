using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.Behaviors.Internals;
using Xamarin.CommunityToolkit.Converters;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.Behaviors
{
	public class PickerBoundToEnumBehavior : BaseBehavior<Picker>
	{
		static readonly MethodInfo? getContextMethod
			= typeof(BindableObject).GetRuntimeMethods()?.FirstOrDefault(m => m.Name == "GetContext");

		static readonly FieldInfo? bindingField
			= getContextMethod?.ReturnType.GetRuntimeField("Binding");

		public static BindableProperty EnumSourceProperty = BindableProperty.Create(
		   propertyName: nameof(EnumSource),
		   returnType: typeof(Enum),
		   declaringType: typeof(PickerBoundToEnumBehavior),
		   defaultBindingMode: BindingMode.TwoWay);

		/// <summary>
		/// Enum Instance from which this Picker's ItemSource will be generated. The Picker and the Enum Instance will be bound
		/// </summary>
		public Enum EnumSource
		{
			get => (Enum)GetValue(EnumSourceProperty);
			set => SetValue(EnumSourceProperty, value);
		}

		protected override void OnAttachedTo(Picker bindable)
		{
			base.OnAttachedTo(bindable);
			bindable.BindingContextChanged += Bindable_BindingContextChanged;
		}

		protected override void OnDetachingFrom(Picker bindable)
		{
			base.OnDetachingFrom(bindable);
			bindable.BindingContextChanged -= Bindable_BindingContextChanged;
		}

		void Bindable_BindingContextChanged(object? sender, EventArgs? e)
		{
			if (sender != null)
			{
				var bindable = (Picker)sender;
				bindable.ItemsSource = Enum.GetNames(EnumSource.GetType());
				var context = getContextMethod?.Invoke(this, new object[] { EnumSourceProperty });
				var binding = (Binding?)bindingField?.GetValue(context);
				var viewModelPropertyName = binding?.Path;
				bindable.SetBinding(Picker.SelectedIndexProperty, viewModelPropertyName, BindingMode.Default, new EnumToIntConverter());
			}
		}
	}
}