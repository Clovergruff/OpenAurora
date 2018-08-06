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
			window.Title = "Open Aurora";

			window.Load += OnLoad;
			window.Unload += UnLoad;
			window.UpdateFrame += Update;
			window.RenderFrame += Render;

			window.MouseWheel += MouseWheel;
		}

		// Load resources
		void OnLoad(object sender, EventArgs e)
		{
			Draw.Rect(new Vector2(32, 32), new Vector2(64, 64), 0, Color.White);

			Draw.VBO = GL.GenBuffer();
			GL.BindBuffer(BufferTarget.ArrayBuffer, Draw.VBO);
			GL.BufferData<Vertex>(BufferTarget.ArrayBuffer, (IntPtr)(Vertex.SizeInBytes * Draw.vertices.Length),
				Draw.vertices, BufferUsageHint.StaticDraw);

			Draw.IBO = GL.GenBuffer();
			GL.BindBuffer(BufferTarget.ElementArrayBuffer, Draw.IBO);
			GL.BufferData<uint>(BufferTarget.ElementArrayBuffer, (IntPtr)(sizeof(uint) * Draw.indices.Length),
				Draw.indices, BufferUsageHint.StaticDraw);

			GL.Enable(EnableCap.Blend);
			GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

			GL.Enable(EnableCap.DepthTest);
			GL.DepthFunc(DepthFunction.Lequal);

			GL.Enable(EnableCap.Texture2D);

			Resources.Load();
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
			GL.ClearDepth(1);
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

			// Render the world
			//Matrix4 worldProjMatrix = Matrix4.CreatePerspectiveFieldOfView()
			RenderWorld();

			// Render the UI
			Matrix4 uiProjMatrix = Matrix4.CreateOrthographicOffCenter(0, window.Width, window.Height, 0, -100, 100);
			GL.MatrixMode(MatrixMode.Projection);
			GL.LoadMatrix(ref uiProjMatrix);

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
			//GL.BindTexture(TextureTarget.Texture2D, Resources.GetTexture("Icon").id);

			GL.EnableClientState(ArrayCap.VertexArray);
			GL.EnableClientState(ArrayCap.TextureCoordArray);
			GL.EnableClientState(ArrayCap.ColorArray);
			GL.EnableClientState(ArrayCap.NormalArray);
			GL.EnableClientState(ArrayCap.IndexArray);

			GL.VertexPointer(	3, VertexPointerType.Float,		Vertex.SizeInBytes, 0);
			GL.TexCoordPointer(	2, TexCoordPointerType.Float,	Vertex.SizeInBytes, Vector3.SizeInBytes);
			GL.NormalPointer(	   NormalPointerType.Float,		Vertex.SizeInBytes, Vector3.SizeInBytes + Vector2.SizeInBytes);
			GL.ColorPointer(	4, ColorPointerType.Float,		Vertex.SizeInBytes, Vector3.SizeInBytes * 2 + Vector2.SizeInBytes);

			GL.BindBuffer(BufferTarget.ArrayBuffer, Draw.VBO);
			GL.BindBuffer(BufferTarget.ElementArrayBuffer, Draw.IBO);
			GL.DrawElements(PrimitiveType.Triangles, Draw.indices.Length, DrawElementsType.UnsignedInt, 0);
			//GL.DrawArrays(PrimitiveType.Triangles, 0, Draw.vertices.Length);

			//Draw.Triangle2D(new Vector2(0, 0), new Vector2(0, 1), new Vector2(1, 0.5f), Color.White);
			//Draw.Rect(new Vector2(64, 64), new Vector2(128, 128), 0, Color.White, Resources.GetTexture("Icon"));
			//Draw.Rect(new Vector2(0, 0), new Vector2(128, 128), 0.5f, Color.White, Resources.GetTexture("Icon"));

			// The UI is a seperate draw method in order to avoid game elements from bleeding into the UI
		}

		void RenderConsole()
		{
			// We have to draw the console above everything else
		}

	}
}
