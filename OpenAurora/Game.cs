using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

namespace OpenAurora
{
	public class Game
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
			Var.game = this;

			window = win;
			window.Title = "Open Aurora";

			window.Load += OnLoad;
			window.Unload += UnLoad;
			window.UpdateFrame += Update;
			window.RenderFrame += Render;
			window.Resize += OnResize;

			window.MouseWheel += MouseWheel;

			Console.Enable();

			Input.CalculateMouse();
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

			Resources.LoadAssets();

			Console.Disable();
			Start(sender, e);

			// Enable Light 0 and set its parameters.
			/*GL.Light(LightName.Light0, LightParameter.Position, new float[] { 1.0f, 1.0f, -0.5f });
			GL.Light(LightName.Light0, LightParameter.Ambient, new float[] { 0.3f, 0.3f, 0.3f, 1.0f });
			GL.Light(LightName.Light0, LightParameter.Diffuse, new float[] { 1.0f, 1.0f, 1.0f, 1.0f });
			GL.Light(LightName.Light0, LightParameter.Specular, new float[] { 1.0f, 1.0f, 1.0f, 1.0f });
			GL.Light(LightName.Light0, LightParameter.SpotExponent, new float[] { 1.0f, 1.0f, 1.0f, 1.0f });
			GL.LightModel(LightModelParameter.LightModelAmbient, new float[] { 0.2f, 0.2f, 0.2f, 1.0f });
			GL.LightModel(LightModelParameter.LightModelTwoSide, 1);
			GL.LightModel(LightModelParameter.LightModelLocalViewer, 1);
			GL.Enable(EnableCap.Lighting);
			GL.Enable(EnableCap.Light0);

			// Use GL.Material to set your object's material parameters.
			GL.Material(MaterialFace.Front, MaterialParameter.Ambient, new float[] { 0.3f, 0.3f, 0.3f, 1.0f });
			GL.Material(MaterialFace.Front, MaterialParameter.Diffuse, new float[] { 1.0f, 1.0f, 1.0f, 1.0f });
			GL.Material(MaterialFace.Front, MaterialParameter.Specular, new float[] { 1.0f, 1.0f, 1.0f, 1.0f });
			GL.Material(MaterialFace.Front, MaterialParameter.Emission, new float[] { 0.0f, 0.0f, 0.0f, 1.0f });*/
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

			new Camera();
			new Player();

			CreateStaticObject(Vector3.Zero, Vector3.Zero, new Vector3(32, 0, 32), Primitives.plane, "BlueWallB");
			CreateStaticObject(new Vector3(5, 2, 6), new Vector3(0, 60, 0), new Vector3(2, 4, 2), Primitives.cube, "Troop");
			CreateStaticObject(new Vector3(8, 0, 0), new Vector3(45, 0, 0), new Vector3(8, 0, 8), Primitives.plane, "BlueWallB");
		}

		void CreateStaticObject(Vector3 pos, Vector3 rot, Vector3 scale, Mesh mesh, string Text)
		{
			var o = new Entity();
			o.SetModel(mesh, Text);
			o.SetTransform(pos, rot, scale);
		}

		// Update the game
		void Update(object sender, EventArgs e)
		{
			Time.GetDeltaTime((FrameEventArgs)e);

			// Input
			Input.CalculateMouse();
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
		public void Render(object sender, EventArgs e)
		{
			GL.ClearColor(0.2f, 0.2f, 0.25f, 1f);
			GL.ClearDepth(1);
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

			// Render the world
			RenderWorld();

			// Draw the UI and the Console using the same projection matrix!
			GL.MatrixMode(MatrixMode.Projection);
			Matrix4 uiProjMatrix = Matrix4.CreateOrthographicOffCenter(0, window.Width, window.Height, 0, -100, 100);
			GL.LoadMatrix(ref uiProjMatrix);

			RenderUI();
			Console.Render();

			if (Resources.loading)
				RenderLoadingScreen();

			window.SwapBuffers();
		}

		void RenderWorld()
		{
			if (Var.camera == null)
				return;

			Matrix4 projection = Var.camera.GetViewMatrix() * Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(Var.camera.fov), window.Width / (float)window.Height, 1.0f, 64.0f);
			GL.MatrixMode(MatrixMode.Projection);
			GL.LoadMatrix(ref projection);
			
			foreach (var entity in entities)
			{
				entity.Render();
			}
		}

		void RenderUI()
		{
			// Draw.Mesh(Primitives.rectangle, new Vector3(Input.mousePosition.X, Input.mousePosition.Y, 0), Quaternion.Identity, new Vector3(64, 64, 0), "Shade");
		}

		void RenderLoadingScreen()
		{
			Draw.Mesh(Primitives.CreateRectangle(Vector3.Zero, Vector3.One, new Color4(1, 0, 0, 1)),
				new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0), Quaternion.Identity, new Vector3(Screen.width, Screen.height, 0));
		}
	}
}
