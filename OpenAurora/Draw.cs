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
		public Vertex(Vector3 position, Vector2 texCoord, Vector3 normal, Color4 color)
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
		public Mesh(string newName, Vertex[] verts, uint[] ids)
		{
			name = newName;
			vertices = verts;
			indices = ids;

			Generate();
		}

		public string name;

		public int VBO, IBO;
		public Vertex[] vertices;
		public uint[] indices;

		public void Generate()
		{
			GL.DeleteBuffer(VBO);
			VBO = GL.GenBuffer();
			GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);
			GL.BufferData<Vertex>(BufferTarget.ArrayBuffer, (IntPtr)(Vertex.SizeInBytes * vertices.Length),
				vertices, BufferUsageHint.StaticDraw);

			GL.DeleteBuffer(IBO);
			IBO = GL.GenBuffer();
			GL.BindBuffer(BufferTarget.ElementArrayBuffer, IBO);
			GL.BufferData<uint>(BufferTarget.ElementArrayBuffer, (IntPtr)(sizeof(uint) * indices.Length),
				indices, BufferUsageHint.StaticDraw);
		}

		public void Bind()
		{
			GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);
			GL.BindBuffer(BufferTarget.ElementArrayBuffer, IBO);
		}
	}

	public class Draw
	{
		public static Mesh textMesh = new Mesh("TextMesh",
					new Vertex[4]
					{
						new Vertex(new Vector3( 0, 0, 0),
							new Vector2(0, 0),
							new Vector3(0, 0, -1), Color.White),
						new Vertex(new Vector3( 1, 0, 0),
							new Vector2(1, 0),
							new Vector3(0, 0, -1), Color.White),
						new Vertex(new Vector3( 1, 1, 0),
							new Vector2(1, 1),
							new Vector3(0, 0, -1), Color.White),
						new Vertex(new Vector3( 0, 1, 0),
							new Vector2(0, 1),
							new Vector3(0, 0, -1), Color.White),
					},
					new uint[6]
					{
					0, 1, 2,
					0, 2, 3,
					});

		public static Vector4 ColorToVector4(Color4 color)
		{
			return new Vector4(color.R, color.G, color.B, color.A);
		}
		public static Color Vector4ToColor(Vector4 vector)
		{
			return Color.FromArgb((int)(vector.W), (int)(vector.X), (int)(vector.Y), (int)(vector.Z));
		}

		public static void Rect(Vector2 pos, Vector2 size, float depth, Color col, Texture2D tex = null)
		{
			
		}

		public static void ScreenMesh(Mesh mesh, Vector3 pos, Quaternion rot, Vector3 scale, Texture2D tex = null)
		{
			Mesh(mesh, pos * Screen.scaling, rot, scale * Screen.scaling, tex);
		}

		public static void Mesh(Mesh mesh, Vector3 pos, Quaternion rot, Vector3 scale, string texName)
		{
			Mesh(mesh, pos, rot, scale, Resources.GetTexture(texName));
		}

		public static void Mesh(Mesh mesh, Vector3 pos, Quaternion rot, Vector3 scale, Texture2D tex = null)
		{
			if (mesh == null)
				return;

			int texId = 0;
			if (tex != null)
				texId = tex.id;

			GL.BindTexture(TextureTarget.Texture2D, texId);

			mesh.Bind();

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

			int offset = 0;
			GL.VertexPointer(3, VertexPointerType.Float, Vertex.SizeInBytes, offset);
			offset += Vector3.SizeInBytes;
			GL.TexCoordPointer(3, TexCoordPointerType.Float, Vertex.SizeInBytes, offset);
			offset += Vector2.SizeInBytes;
			GL.NormalPointer(NormalPointerType.Float, Vertex.SizeInBytes, offset);
			offset += Vector3.SizeInBytes;
			GL.ColorPointer(4, ColorPointerType.Float, Vertex.SizeInBytes, offset);

			GL.BindBuffer(BufferTarget.ArrayBuffer, mesh.VBO);
			GL.BindBuffer(BufferTarget.ElementArrayBuffer, mesh.IBO);
			GL.DrawElements(PrimitiveType.Triangles, mesh.indices.Length, DrawElementsType.UnsignedInt, 0);
		}

		public static void Text(Vector2 pos, string text, BitmapFont font, Color4 col)
		{
			float u_step = ((float)font.glyphWidth / font.atlas.width) * Screen.scaling;
			float v_step = ((float)font.glyphHeight / font.atlas.height) * Screen.scaling;
			int xOffset = 0;

			for (int n = 0; n < text.Length; n++)
			{
				char idx = text[n];
				float u = (idx % font.glyphsPerLine) * u_step;
				float v = (idx / font.glyphsPerLine) * v_step;

				float width = font.glyphWidth * Screen.scaling;
				float height = font.glyphHeight * Screen.scaling;

				Vector2[] letterUvs = new Vector2[4];

				letterUvs[0] = new Vector2(u, v);
				letterUvs[1] = new Vector2(u + u_step, v);
				letterUvs[2] = new Vector2(u + u_step, v + v_step);
				letterUvs[3] = new Vector2(u, v + v_step);

				for (int i = 0; i < 4; i++)
				{
					textMesh.vertices[i].texCoord = letterUvs[i];
					textMesh.vertices[i].color = ColorToVector4(col);
				}

				textMesh.Generate();

				Draw.Mesh(textMesh, new Vector3(pos.X * Screen.scaling + xOffset, pos.Y * Screen.scaling, 0), Quaternion.Identity,
					new Vector3(width, height, 0), font.atlas);

				xOffset += (int)(font.charSpacing * Screen.scaling);
			}
		}
	}

	public class Screen
	{
		public static float scaling;

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
		public static int scaledWidth
		{
			get { return (int)(Game.window.Width * scaling); }
			//set { Game.window.Width = (int)(value / scaling); }
		}
		public static int scaledHeight
		{
			get { return (int)(Game.window.Height * scaling); }
			//set { Game.window.Height = (int)(value / scaling); }
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

		public static Mesh CreateRectangle(Vector3 pos, Vector3 size, Color4 col)
		{
			Vector3 hs = size * 0.5f;

			return new Mesh("Rectangle",
			  new Vertex[4]
			  {
				new Vertex(pos + new Vector3( -hs.X, -hs.Y, 0), new Vector2(0, 1), new Vector3(0, 0, -1), col),
				new Vertex(pos + new Vector3(  hs.X, -hs.Y, 0), new Vector2(1, 1), new Vector3(0, 0, -1), col),
				new Vertex(pos + new Vector3(  hs.X,  hs.Y, 0), new Vector2(1, 0), new Vector3(0, 0, -1), col),
				new Vertex(pos + new Vector3( -hs.X,  hs.Y, 0), new Vector2(0, 0), new Vector3(0, 0, -1), col),
			  },
			  new uint[6]
			  {
				0, 1, 2,
				0, 2, 3,
			  });
		}

		public static Mesh CreatePlane(Vector3 pos, Vector3 size, Color4 col)
		{
			Vector3 hs = size * 0.5f;

			return new Mesh("Plane",
			  new Vertex[4]
			  {
				new Vertex(pos + new Vector3( -hs.X, 0, -hs.Z), new Vector2(0, 1), new Vector3(0, 1, 0), col),
				new Vertex(pos + new Vector3(  hs.X, 0, -hs.Z), new Vector2(1, 1), new Vector3(0, 1, 0), col),
				new Vertex(pos + new Vector3(  hs.X, 0,  hs.Z), new Vector2(1, 0), new Vector3(0, 1, 0), col),
				new Vertex(pos + new Vector3( -hs.X, 0,  hs.Z), new Vector2(0, 0), new Vector3(0, 1, 0), col),
			  },
			  new uint[6]
			  {
				0, 1, 2,
				0, 2, 3,
			  });
		}

		public static Mesh CreateCube(Vector3 pos, Vector3 size, Color4 col)
		{
			Vector3 hs = size * 0.5f;

			return new Mesh("Cube",
				new Vertex[]
				{
					new Vertex(new Vector3( -hs.X, -hs.Y, -hs.Z), new Vector2(0, 1), new Vector3(-1, -1, -1), col), // Left		Front	Down
					new Vertex(new Vector3(  hs.X, -hs.Y, -hs.Z), new Vector2(1, 1), new Vector3( 1, -1, -1), col), // Right	Front	Down
					new Vertex(new Vector3(  hs.X,  hs.Y, -hs.Z), new Vector2(1, 0), new Vector3( 1,  1,  1), col), // Right	Front	Up
					new Vertex(new Vector3( -hs.X,  hs.Y, -hs.Z), new Vector2(0, 0), new Vector3(-1,  1,  1), col), // Left		Front	Up
					new Vertex(new Vector3( -hs.X, -hs.Y,  hs.Z), new Vector2(0, 1), new Vector3(-1, -1, -1), col), // Left		Back	Down
					new Vertex(new Vector3(  hs.X, -hs.Y,  hs.Z), new Vector2(1, 1), new Vector3( 1, -1, -1), col), // Right	Back	Down
					new Vertex(new Vector3(  hs.X,  hs.Y,  hs.Z), new Vector2(1, 0), new Vector3( 1,  1,  1), col), // Right	Back	Up
					new Vertex(new Vector3( -hs.X,  hs.Y,  hs.Z), new Vector2(0, 0), new Vector3(-1,  1,  1), col), // Left		Back	Up
			  },
			  new uint[]
			  {
				// Front
				0, 7, 3,
				0, 4, 7,
				// Back
				1, 2, 6,
				6, 5, 1,
				// Left
				0, 2, 1,
				0, 3, 2,
				// Right
				4, 5, 6,
				6, 7, 4,
				// Top
				2, 3, 6,
				6, 3, 7,
				// Bottom
				0, 1, 5,
				0, 5, 4
			  });
		}
	}
}
