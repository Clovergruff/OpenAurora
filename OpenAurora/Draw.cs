using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace OpenAurora
{
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
		public static void Triangle(Vector3 p1, Vector3 p2, Vector3 p3, Color col)
		{
			GL.Begin(PrimitiveType.Triangles);
			GL.Color4(col);
			GL.Vertex3(p1.X, p1.Y, p1.Z);
			GL.Vertex3(p2.X, p2.Y, p2.Z);
			GL.Vertex3(p3.X, p3.Y, p3.Z);
			GL.End();
		}

		public static void Triangle2D(Vector2 p1, Vector2 p2, Vector2 p3, Color col)
		{
			GL.Begin(PrimitiveType.Triangles);
			GL.Color4(col);
			GL.Vertex2(p1.X, p1.Y);
			GL.Vertex2(p2.X, p2.Y);
			GL.Vertex2(p3.X, p3.Y);
			GL.End();
		}
		public static void Rect(Vector2 pos, Vector2 size, Color col, Texture2D tex = null)
		{
			pos = PixelToPerc(pos);
			size = PixelToPerc(size);

			GL.Begin(PrimitiveType.Triangles);

			if (tex != null)
				GL.BindTexture(TextureTarget.Texture2D, tex.id);

			GL.Color4(col);
			GL.TexCoord2(0, 0); GL.Vertex2(pos.X,			pos.Y + size.Y);
			GL.TexCoord2(1, 1); GL.Vertex2(pos.X + size.X,	pos.Y);
			GL.TexCoord2(0, 1); GL.Vertex2(pos.X,			pos.Y);

			GL.TexCoord2(0, 0); GL.Vertex2(pos.X,			pos.Y + size.Y);
			GL.TexCoord2(1, 0); GL.Vertex2(pos.X + size.X,	pos.Y + size.Y);
			GL.TexCoord2(1, 1); GL.Vertex2(pos.X + size.X,	pos.Y);

			GL.End();
		}

		public static Vector2 PixelToPerc(Vector2 vec)
		{
			return new Vector2((vec.X / (Screen.width * 0.5f)),
							   (vec.Y / (Screen.height * 0.5f)));
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
