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
		public static int assetCount = 0;
		public static bool loading = true;

		public static List<Texture2D> textures = new List<Texture2D>();
		public static List<Mesh> meshes = new List<Mesh>();
		//public static List<Font> fonts = new List<Font>();
		//public static List<Font> soundClips = new List<Font>();
		public static List<AnimationClip> animations = new List<AnimationClip>();

		// Text
		public static Font systemFont;
		private const string textChars = @"qwertyuiopasdfghjklzxcvbnmQWERTYUIOPASDFGHJKLZXCVBNM0123456789µ§½!""#¤%&/()=?^*@£€${[]}\~¨'-_.:,;<>|°©®±¥";

		public class BitmapFont
		{
			public string name;
			public Bitmap bitmap;
			public Font sourceFont;
			public int size = 16;
			public Size charSize;

			public BitmapFont(string newName, int newFontSize)
			{
				name = newName;
				size = newFontSize;
				Generate(size, name, out charSize);
			}

			public Bitmap Generate(int fontSize, string fontName, out Size charSize)
			{
				var characters = new List<Bitmap>();
				using (var font = new Font(fontName, fontSize))
				{
					for (int i = 0; i < textChars.Length; i++)
					{
						var charBmp = GenerateChar(font, textChars[i]);
						characters.Add(charBmp);
					}
					charSize = new Size(characters.Max(x => x.Width), characters.Max(x => x.Height));
					var charMap = new Bitmap(charSize.Width * characters.Count, charSize.Height);
					using (var gfx = Graphics.FromImage(charMap))
					{
						gfx.FillRectangle(Brushes.Black, 0, 0, charMap.Width, charMap.Height);
						for (int i = 0; i < characters.Count; i++)
						{
							var c = characters[i];
							gfx.DrawImageUnscaled(c, i * charSize.Width, 0);

							c.Dispose();
						}
					}
					return charMap;
				}
			}

			private Bitmap GenerateChar(Font font, char c)
			{
				var size = GetCharSize(font, c);
				var bmp = new Bitmap((int)size.Width, (int)size.Height);
				using (var gfx = Graphics.FromImage(bmp))
				{
					gfx.FillRectangle(Brushes.Black, 0, 0, bmp.Width, bmp.Height);
					gfx.DrawString(c.ToString(), font, Brushes.White, 0, 0);
				}
				return bmp;
			}
			private SizeF GetCharSize(Font font, char c)
			{
				using (var bmp = new Bitmap(512, 512))
				{
					using (var gfx = Graphics.FromImage(bmp))
					{
						return gfx.MeasureString(c.ToString(), font);
					}
				}
			}
		}

		public static void LoadAssets()
		{
			loading = true;
			// string[] fontPaths = Directory.GetFiles("Data/Fonts", "*.ttf", SearchOption.AllDirectories);
			string[] texPaths = Directory.GetFiles("Data/Textures", "*.png", SearchOption.AllDirectories);

			assetCount = texPaths.Length;

			// Load all fonts
			//foreach (var path in fontPaths)
			LoadFont("Data/Fonts/Console.ttf");

			// Load all textures
			foreach (var path in texPaths)
			{
				LoadTexture(path);
				Var.game.Render(null, null);
			}

			loading = false;
		}

		public static void Clear()
		{
			textures.Clear();
			meshes.Clear();
			//fonts.Clear();
			//soundClips.Clear();
		}

		#region Textures
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

			GL.BindTexture(TextureTarget.Texture2D, 0);

			Texture2D tex = new Texture2D(id, bitmap.Width, bitmap.Height);
			tex.name = Path.GetFileNameWithoutExtension(filePath);

			textures.Add(tex);

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
		public static void RemoveTexture(string texName)
		{
			foreach (var tex in textures)
			{
				if (tex.name == texName)
				{
					textures.Remove(tex);
					break;
				}
			}
		}
		#endregion
		#region Meshes
		public static Mesh LoadMesh(string filePath)
		{
			return null;
		}
		public Mesh GetMesh()
		{
			return null;
		}
		public void RemoveMesh()
		{

		}
		#endregion
		#region Fonts
		public static Font LoadFont(string filePath)
		{
			return null;
		}
		public Mesh GetFont()
		{
			return null;
		}
		public void RemoveFont(Font targetFont)
		{
			
		}
		#endregion
		#region Audio
		#endregion
		#region Animations
		public static AnimationClip LoadAnimation(string filePath)
		{
			return null;
		}
		public AnimationClip GetAnimation()
		{
			return null;
		}
		public void RemoveAnimation()
		{

		}
		#endregion
	}
}
