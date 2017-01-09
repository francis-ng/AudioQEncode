﻿using System.Windows.Input;

namespace AudioQEncode.Utils
{
	public static class Commands
	{
		public static readonly RoutedUICommand Exit = new RoutedUICommand
			(
				"Exit",
				"Exit",
				typeof(Commands),
				new InputGestureCollection()
				{
					new KeyGesture(Key.F4, ModifierKeys.Alt)
				}
			);
	}
}
