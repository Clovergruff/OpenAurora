using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

namespace OpenAurora
{
	class Game
	{
		public static GameWindow window;
		public static List<Entity> entities = new List<Entity>();
		public static List<Pawn> pawns = new List<Pawn>();
		
		public Game(GameWindow win)
		{
			window = win;

			window.Load += OnLoad;
			window.UpdateFrame += Update;
			window.RenderFrame += Render;

			window.MouseWheel += MouseWheel;
		}

		// Load resources
		void OnLoad(object sender, EventArgs e)
		{
			// GL.Enable(EnableCap.DepthTest);
			// GL.DepthFunc(DepthFunction.Lequal);

			GL.Enable(EnableCap.Texture2D);

			Resources.Load();
		}

		// Input
		void MouseWheel(object sender, MouseWheelEventArgs e)
		{
			// TODO: MouseWheel
		}

		// Update the game
		void Update(object sender, EventArgs e)
		{
			// Input
			MouseState mState = Mouse.GetCursorState();
			Input.mousePosition = window.PointToClient(new Point(mState.X, mState.Y));
			Input.state = Keyboard.GetState();

			// Update all entities
			foreach (var entity in entities)
			{
				entity.Update();
			}

			// 
			if (Input.GetKey(Key.AltLeft) && Input.GetKeyDown(Key.F4))
				window.Close();

			if (Input.GetKey(Key.AltLeft) && Input.GetKeyDown(Key.Enter))
				Screen.ToggleFullScreen();

			// Set the last keyboard state
			Input.lastState = Input.state;
		}

		// Rendering
		void Render(object sender, EventArgs e)
		{
			GL.ClearColor(0.2f, 0.2f, 0.25f, 1f);
			GL.Clear(ClearBufferMask.ColorBufferBit);

			RenderWorld();
			RenderUI();
			RenderConsole();

			window.SwapBuffers();
		}

		void RenderWorld()
		{
			foreach (var entity in entities)
			{
				entity.Render();
			}
		}

		void RenderUI()
		{
			//Draw.Triangle2D(new Vector2(0, 0), new Vector2(0, 1), new Vector2(1, 0.5f));
			Draw.Rect(new Vector2(0, 0), new Vector2(64, 64), Color.White, Resources.GetTexture("Icon"));

			// The UI is a seperate draw method in order to avoid game elements from bleeding into the UI
		}

		void RenderConsole()
		{
			// We have to draw the console above everything else
		}

	}
}
