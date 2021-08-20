using System;
using System.Collections.Generic;
using System.Text;

namespace Xamarin.CommunityToolkit.Sample.ViewModels.Behaviors
{
	public class PickerBoundToEnumBehaviorViewModel : BaseViewModel
	{
		ExampleEnum myEnum;

		public enum ExampleEnum
		{
			FIRST = 0,
			SECOND = 1,
			THIRD = 2,
			FOURTH = 3
		}

		public ExampleEnum MyEnum
		{
			get => myEnum;
			set
			{
				myEnum = value;
				OnPropertyChanged("MyEnum");
			}
		}
	}
}
