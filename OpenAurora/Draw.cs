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
		public static int width = 1024;
		public static int height = 600;
		public static bool fullscreen = false;

		public static void ToggleFullScreen()
		{
			if (fullscreen)
				DisableFullScreen();
			else
				EnableFullScreen();
		}
		public static void EnableFullScreen()
		{
			if (fullscreen)
				return;
			fullscreen = true;

			Game.window.WindowState = WindowState.Fullscreen;
		}
		public static void DisableFullScreen()
		{
			if (!fullscreen)
				return;
			fullscreen = false;

			Game.window.WindowState = WindowState.Normal;
		}
	}

	public static class Primitives
	{
		public static Mesh Rectangle(Vector3 pos, Vector3 size, Color col)
		{
			Vector3 halfSize = size * 0.5f;

			return new Mesh("Rectangle",
			  new Vertex[4]
			  {
				new Vertex(new Vector3(pos.X - halfSize.X,	pos.Y - halfSize.Y, pos.Z), new Vector2(0, 0), new Vector3(0, 0, -1), col),
				new Vertex(new Vector3(pos.X + halfSize.X,	pos.Y - halfSize.Y, pos.Z), new Vector2(1, 0), new Vector3(0, 0, -1), col),
				new Vertex(new Vector3(pos.X + halfSize.X,	pos.Y + halfSize.Y,	pos.Z), new Vector2(1, 1), new Vector3(0, 0, -1), col),
				new Vertex(new Vector3(pos.X - halfSize.X,	pos.Y + halfSize.Y,	pos.Z), new Vector2(0, 1), new Vector3(0, 0, -1), col),
			  },
			  new uint[6]
			  {
				0, 1, 2,
				0, 2, 3,
			  });
		}
	}
}
