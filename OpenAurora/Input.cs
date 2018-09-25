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
		public static char currentKeyChar;
		public static bool cursorLocked = true;
		public static bool cursorVisible = true;

		public static Point oldMousePos = Point.Empty;

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
			mousePosition = GetMousePosition();
			mouseDelta = new Vector2(oldMousePos.X - mousePosition.X, oldMousePos.Y - mousePosition.Y);
			oldMousePos = mousePosition;

			if (cursorLocked && !Console.enabled)
			{
				Point winPos = Game.window.Bounds.Location;
				Size winSize = Game.window.Bounds.Size;
				Mouse.SetPosition(winPos.X + winSize.Width / 2, winPos.Y + winSize.Height / 2);
			}
		}

		public static Point GetMousePosition()
		{
			MouseState mState = Mouse.GetState();
			return Game.window.PointToClient(new Point(mState.X, mState.Y));
		}
	}
}
