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

	public class Draw
	{
		public static int VBO, IBO;
		public static Vertex[] vertices;
		public static uint[] indices;

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
			vertices = new Vertex[4]
			{
				new Vertex(new Vector3(pos.X,			pos.Y,			depth), new Vector2(0, 0), new Vector3(0, 0, -1), col),
				new Vertex(new Vector3(pos.X + size.X,	pos.Y,			depth), new Vector2(1, 0), new Vector3(0, 0, -1), col),
				new Vertex(new Vector3(pos.X + size.X,	pos.Y + size.Y, depth), new Vector2(1, 1), new Vector3(0, 0, -1), col),
				new Vertex(new Vector3(pos.X,           pos.Y + size.Y, depth), new Vector2(0, 1), new Vector3(0, 0, -1), col),
			};

			indices = new uint[6]
			{
				0, 1, 2,
				0, 2, 3,
			};

			/*GL.Begin(PrimitiveType.Triangles);

			if (tex != null)
				GL.BindTexture(TextureTarget.Texture2D, tex.id);

			GL.Color4(col);

			GL.TexCoord2(0, 0); GL.Vertex3(pos.X,			pos.Y,			depth);
			GL.TexCoord2(1, 0); GL.Vertex3(pos.X + size.X,	pos.Y,			depth);
			GL.TexCoord2(0, 1); GL.Vertex3(pos.X,			pos.Y + size.Y, depth);					  

			GL.TexCoord2(1, 0); GL.Vertex3(pos.X + size.X,	pos.Y,			depth);
			GL.TexCoord2(1, 1); GL.Vertex3(pos.X + size.X,	pos.Y + size.Y, depth);
			GL.TexCoord2(0, 1); GL.Vertex3(pos.X,			pos.Y + size.Y, depth);							

			GL.End();*/
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
}
