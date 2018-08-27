using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace OpenAurora
{
	public struct Vertex
	{
		public static int SizeInBytes
		{
			get
			{
				return Vector2.SizeInBytes + Vector3.SizeInBytes * 2 + Vector4.SizeInBytes;
			}
		}

		public Vector3 position;
		public Vector2 texCoord;
		public Vector3 normal;
		public Vector4 color;

		public Color Color
		{
			get
			{
				return Draw.Vector4ToColor(color);
			}
			set
			{
				this.color = Draw.ColorToVector4(value);
			}
		}

		public Vertex(Vector3 position, Vector2 texCoord, Vector3 normal, Vector4 color)
		{
			this.position = position;
			this.texCoord = texCoord;
			this.normal = normal;
			this.color = color;
		}
		public Vertex(Vector3 position, Vector2 texCoord, Vector3 normal, Color color)
		{
			this.position = position;
			this.texCoord = texCoord;
			this.normal = normal;
			this.color = Draw.ColorToVector4(color);
		}
	}

	public class Material
	{
		public Texture2D texture;
	}
	public class Texture2D
	{
		public string name;
		public int id;
		public int width, height;

		public Texture2D(int id, int width, int height)
		{
			this.id = id;
			this.width = width;
			this.height = height;
		}
	}
	public class Mesh
	{
		public Mesh(string newName, Vertex[] newVerts, uint[] newIds)
		{
			name = newName;
			vertices = newVerts;
			indices = newIds;

			Init();
		}

		public string name;

		public int VBO, IBO;
		public Vertex[] vertices;
		public uint[] indices;

		public void Init()
		{
			VBO = GL.GenBuffer();
			GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);
			GL.BufferData<Vertex>(BufferTarget.ArrayBuffer, (IntPtr)(Vertex.SizeInBytes * vertices.Length),
				vertices, BufferUsageHint.StaticDraw);

			IBO = GL.GenBuffer();
			GL.BindBuffer(BufferTarget.ElementArrayBuffer, IBO);
			GL.BufferData<uint>(BufferTarget.ElementArrayBuffer, (IntPtr)(sizeof(uint) * indices.Length),
				indices, BufferUsageHint.StaticDraw);
		}
	}

	public class Draw
	{
		public static Vector4 ColorToVector4(Color color)
		{
			return new Vector4(color.R / 255f, color.G / 255f, color.B / 255f, color.A / 255f);
		}
		public static Color Vector4ToColor(Vector4 vector)
		{
			return Color.FromArgb((int)(vector.W * 255), (int)(vector.X * 255), (int)(vector.Y * 255), (int)(vector.Z * 255));
		}

		public static void Rect(Vector2 pos, Vector2 size, float depth, Color col, Texture2D tex = null)
		{
			
		}

		public static void Mesh(Mesh mesh, Vector3 pos, Quaternion rot, Vector3 scale, string texName)
		{
			Mesh(mesh, pos, rot, scale, Resources.GetTexture(texName));
		}
		public static void Mesh(Mesh mesh, Vector3 pos, Quaternion rot, Vector3 scale, Texture2D tex = null)
		{
			int texId = 0;
			if (tex != null)
				texId = tex.id;
			GL.BindTexture(TextureTarget.Texture2D, texId);

			Matrix4 modelViewMatrix =
				Matrix4.CreateScale(scale) *
				Matrix4.CreateFromQuaternion(rot) *
				Matrix4.CreateTranslation(pos);
			GL.MatrixMode(MatrixMode.Modelview);
			GL.LoadMatrix(ref modelViewMatrix);

			GL.EnableClientState(ArrayCap.VertexArray);
			GL.EnableClientState(ArrayCap.TextureCoordArray);
			GL.EnableClientState(ArrayCap.ColorArray);
			GL.EnableClientState(ArrayCap.NormalArray);
			GL.EnableClientState(ArrayCap.IndexArray);

			GL.VertexPointer(3, VertexPointerType.Float, Vertex.SizeInBytes, 0);
			GL.TexCoordPointer(2, TexCoordPointerType.Float, Vertex.SizeInBytes, Vector3.SizeInBytes);
			GL.NormalPointer(NormalPointerType.Float, Vertex.SizeInBytes, Vector3.SizeInBytes + Vector2.SizeInBytes);
			GL.ColorPointer(4, ColorPointerType.Float, Vertex.SizeInBytes, Vector3.SizeInBytes * 2 + Vector2.SizeInBytes);

			GL.BindBuffer(BufferTarget.ArrayBuffer, mesh.VBO);
			GL.BindBuffer(BufferTarget.ElementArrayBuffer, mesh.IBO);
			GL.DrawElements(PrimitiveType.Triangles, mesh.indices.Length, DrawElementsType.UnsignedInt, 0);
		}
	}

	public class Screen
	{
		public static int width
		{
			get	{ return Game.window.Width; }
			set	{ Game.window.Width = value; }
		}
		public static int height
		{
			get	{ return Game.window.Height; }
			set	{ Game.window.Height = value; }
		}
		public static bool fullscreen
		{
			get { return Game.window.WindowState == WindowState.Fullscreen; }
			set
			{
				if (value == true)
					Game.window.WindowState = WindowState.Fullscreen;
				else
					Game.window.WindowState = WindowState.Normal;
			}
		}

		public static void ToggleFullScreen()
		{
			fullscreen = !fullscreen;
		}
	}

	public static class Primitives
	{
		public static Mesh rectangle = CreateRectangle(new Vector3(0, 0, 0), new Vector3(1, 1, 0), Color.White);
		public static Mesh cube = CreateCube(new Vector3(0, 0, 0), new Vector3(1, 1, 1), Color.White);
		public static Mesh plane = CreatePlane(new Vector3(0, 0, 0), new Vector3(1, 0, 1), Color.White);

		public static Mesh CreateRectangle(Vector3 pos, Vector3 size, Color col)
		{
			Vector3 hs = size * 0.5f;

			return new Mesh("Rectangle",
			  new Vertex[4]
			  {
				new Vertex(pos + new Vector3( -hs.X, -hs.Y, 0), new Vector2(0, 0), new Vector3(0, 0, -1), col),
				new Vertex(pos + new Vector3(  hs.X, -hs.Y, 0), new Vector2(1, 0), new Vector3(0, 0, -1), col),
				new Vertex(pos + new Vector3(  hs.X,  hs.Y, 0), new Vector2(1, 1), new Vector3(0, 0, -1), col),
				new Vertex(pos + new Vector3( -hs.X,  hs.Y, 0), new Vector2(0, 1), new Vector3(0, 0, -1), col),
			  },
			  new uint[6]
			  {
				0, 1, 2,
				0, 2, 3,
			  });
		}

		public static Mesh CreatePlane(Vector3 pos, Vector3 size, Color col)
		{
			Vector3 hs = size * 0.5f;

			return new Mesh("Plane",
			  new Vertex[4]
			  {
				new Vertex(pos + new Vector3( -hs.X, 0, -hs.Z), new Vector2(0, 0), new Vector3(0, 1, 0), col),
				new Vertex(pos + new Vector3(  hs.X, 0, -hs.Z), new Vector2(1, 0), new Vector3(0, 1, 0), col),
				new Vertex(pos + new Vector3(  hs.X, 0,  hs.Z), new Vector2(1, 1), new Vector3(0, 1, 0), col),
				new Vertex(pos + new Vector3( -hs.X, 0,  hs.Z), new Vector2(0, 1), new Vector3(0, 1, 0), col),
			  },
			  new uint[6]
			  {
				0, 1, 2,
				0, 2, 3,
			  });
		}

		public static Mesh CreateCube(Vector3 pos, Vector3 size, Color col)
		{
			Vector3 hs = size * 0.5f;

			return new Mesh("Cube",
				new Vertex[]
				{
					new Vertex(new Vector3( -hs.X, -hs.Y, -hs.Z), new Vector2(0, 0), new Vector3(0, 0, 1), col),
					new Vertex(new Vector3(  hs.X, -hs.Y, -hs.Z), new Vector2(1, 0), new Vector3(0, 0, 1), col),
					new Vertex(new Vector3(  hs.X,  hs.Y, -hs.Z), new Vector2(1, 1), new Vector3(0, 0, 1), col),
					new Vertex(new Vector3( -hs.X,  hs.Y, -hs.Z), new Vector2(0, 1), new Vector3(0, 0, 1), col),
					new Vertex(new Vector3( -hs.X, -hs.Y,  hs.Z), new Vector2(0, 0), new Vector3(0, 0, 1), col),
					new Vertex(new Vector3(  hs.X, -hs.Y,  hs.Z), new Vector2(1, 0), new Vector3(0, 0, 1), col),
					new Vertex(new Vector3(  hs.X,  hs.Y,  hs.Z), new Vector2(1, 1), new Vector3(0, 0, 1), col),
					new Vertex(new Vector3( -hs.X,  hs.Y,  hs.Z), new Vector2(0, 1), new Vector3(0, 0, 1), col),
			  },
			  new uint[]
			  {
				  //front
					0, 7, 3,
					0, 4, 7,
					//back
					1, 2, 6,
					6, 5, 1,
					//left
					0, 2, 1,
					0, 3, 2,
					//right
					4, 5, 6,
					6, 7, 4,
					//top
					2, 3, 6,
					6, 3, 7,
					//bottom
					0, 1, 5,
					0, 5, 4
			  });
		}
	}
}
