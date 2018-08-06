using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Drawing;
using System.Drawing.Imaging;
using OpenTK.Graphics.OpenGL;
using System.IO;

namespace OpenAurora
{
	public class Resources
	{
		public static List<Texture2D> textures = new List<Texture2D>();

		public static void Load()
		{
			textures.Add(LoadTexture("Data/Textures/Icon.png"));
		}

		public static Texture2D LoadTexture(string filePath)
		{
			Bitmap bitmap = new Bitmap(filePath);
			int id = GL.GenTexture();

			BitmapData bmpData = bitmap.LockBits(
				new Rectangle(0, 0, bitmap.Width, bitmap.Height),
					ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

			GL.BindTexture(TextureTarget.Texture2D, id);

			GL.TexImage2D(TextureTarget.Texture2D, 0,
				PixelInternalFormat.Rgba, bitmap.Width, bitmap.Height, 0,
					OpenTK.Graphics.OpenGL.PixelFormat.Bgra,
						PixelType.UnsignedByte, bmpData.Scan0);

			bitmap.UnlockBits(bmpData);

			GL.TexParameter(TextureTarget.Texture2D,
				TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
			GL.TexParameter(TextureTarget.Texture2D,
				TextureParameterName.TextureMagFilter, (int)TextureMinFilter.Linear);

			Texture2D tex = new Texture2D(id, bitmap.Width, bitmap.Height);
			tex.name = Path.GetFileNameWithoutExtension(filePath);

			Console.WriteLine(filePath + " loaded, called " + tex.name);

			return tex;
		}

		public static Texture2D GetTexture(string texName)
		{
			foreach (var tex in textures)
			{
				if (tex.name == texName)
					return tex;
			}

			return null;
		}
	}
}
