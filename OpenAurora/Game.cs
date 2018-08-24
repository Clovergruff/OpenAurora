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
		public enum Mode
		{
			Game,
			Editor,
		}

		public static GameWindow window;
		public static List<Entity> entities = new List<Entity>();
		public static List<Pawn> pawns = new List<Pawn>();
		public static Mode mode;

		public Game(GameWindow win)
		{
			window = win;
			window.Title = "Open Aurora";

			window.Load += OnLoad;
			window.Unload += UnLoad;
			window.UpdateFrame += Update;
			window.RenderFrame += Render;
			window.Resize += OnResize;

			window.MouseWheel += MouseWheel;

			Console.Enable();
		}

		void OnResize(object sender, EventArgs e)
		{
			GL.Viewport(0, 0, Screen.width, Screen.height);
		}

		// Load resources
		void OnLoad(object sender, EventArgs e)
		{
			GL.Enable(EnableCap.Blend);
			GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

			GL.Enable(EnableCap.DepthTest);
			GL.DepthFunc(DepthFunction.Lequal);

			GL.Enable(EnableCap.Texture2D);

			Resources.Load();

			Console.Disable();

			Start(sender, e);
		}

		void UnLoad(object sender, EventArgs e)
		{
			Resources.Clear();
		}

		// Input
		void MouseWheel(object sender, MouseWheelEventArgs e)
		{
			// TODO: MouseWheel
		}

		// Start of the game
		void Start(object sender, EventArgs e)
		{
			// Let's create some first objects

			// Var.camera = (Camera)Entity.Create();
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

			// Base input
			if (Input.GetKeyDown(Key.Tilde))
				Console.Toggle();

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
			GL.ClearDepth(1);
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

			// Render the world
			RenderWorld();

			// Draw the UI and the Console using the same projection matrix!
			Matrix4 uiProjMatrix = Matrix4.CreateOrthographicOffCenter(0, window.Width, window.Height, 0, -100, 100);
			GL.MatrixMode(MatrixMode.Projection);
			GL.LoadMatrix(ref uiProjMatrix);
			RenderUI();

			Console.Render();

			window.SwapBuffers();
		}

		void RenderWorld()
		{
			Matrix4 projection = Matrix4.CreatePerspectiveFieldOfView((float)Math.PI / 4, window.Width / (float)window.Height, 1.0f, 64.0f);
			GL.MatrixMode(MatrixMode.Projection);
			GL.LoadMatrix(ref projection);

			Draw.Mesh(Primitives.cube, new Vector3(0, 0, 0), Quaternion.FromEulerAngles(0, MathHelper.DegreesToRadians(Input.mousePosition.X), 0), new Vector3(0.5f, 0.5f, 0.5f));

			foreach (var entity in entities)
			{
				entity.Render();
			}
		}

		void RenderUI()
		{
			Draw.Mesh(Primitives.rectangle, new Vector3(Input.mousePosition.X, Input.mousePosition.Y, 0), Quaternion.Identity, new Vector3(64, 64, 0),
				Resources.GetTexture("Shade"));
		}
	}
}
