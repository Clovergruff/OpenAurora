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
		public static Vector2 mouseDelta;
		private static Point oldMousePos = Point.Empty;

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

		public static void CalculateMouse()
		{
			MouseState mState = Mouse.GetState();
			mousePosition = Game.window.PointToClient(new Point(mState.X, mState.Y));

			mouseDelta = new Vector2(oldMousePos.X - mousePosition.X, oldMousePos.Y - mousePosition.Y);

			oldMousePos = mousePosition;
		}
	}
}
