using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;
using OpenTK.Input;

namespace OpenAurora
{
	public class Input
	{
		public static KeyboardState lastState;
		public static KeyboardState state;
		public static Point mousePosition;

		public static bool GetKey(Key key)
		{
			return state.IsKeyDown(key);
		}
		public static bool GetKeyDown(Key key)
		{
			return state.IsKeyDown(key) && lastState.IsKeyUp(key);
		}
		public static bool GetKeyUp(Key key)
		{
			return state.IsKeyUp(key) && lastState.IsKeyDown(key);
		}
	}
}
