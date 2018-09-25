using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Drawing;
using System.Drawing.Imaging;
using OpenTK.Graphics.OpenGL;
using System.IO;
using System.Drawing.Drawing2D;
using System.Drawing.Text;

namespace OpenAurora
{
	public class BitmapFont
	{
		public string name;
		public Font font;
		public Texture2D atlas;

		public int atlasOffsetX = -3, atlassOffsetY = -1;
		public int size = 12;
		public bool bitmapFont = false;
		public int glyphsPerLine = 16;
		public int glyphLineCount = 16;
		public int glyphWidth = 12;
		public int glyphHeight = 16;
		public int charSpacing = 8;

		private string fontImageFileName;

		public BitmapFont(Font newFont, string newName)
		{
			name = newName;
			font = newFont;
			fontImageFileName = name + ".png";

			Generate();
		}

		public void Generate()
		{
			int bitmapWidth = glyphsPerLine * glyphWidth;
			int bitmapHeight = glyphLineCount * glyphHeight;

			using (Bitmap bitmap = new Bitmap(bitmapWidth, bitmapHeight, System.Drawing.Imaging.PixelFormat.Format32bppArgb))
			{
				using (var g = Graphics.FromImage(bitmap))
				{
					g.SmoothingMode = SmoothingMode.None;
					g.TextRenderingHint = TextRenderingHint.SingleBitPerPixel;
					// g.SmoothingMode = SmoothingMode.HighQuality;
					// g.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
					//g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

					for (int p = 0; p < glyphLineCount; p++)
					{
						for (int n = 0; n < glyphsPerLine; n++)
						{
							char c = (char)(n + p * glyphsPerLine);
							g.DrawString(c.ToString(), font, Brushes.White,
								n * glyphWidth + atlasOffsetX, p * glyphHeight + atlassOffsetY);
						}
					}
				}
				string atlasPath = "Data/Fonts/" + fontImageFileName;
				bitmap.Save(atlasPath);

				atlas = Resources.LoadTexture(atlasPath);
			}
			//Process.Start(fontImageFileName);
		}
	}

	public class Resources
	{
		public static int assetCount = 0;
		public static bool loading = true;

		public static List<Texture2D> textures = new List<Texture2D>();
		public static List<Mesh> meshes = new List<Mesh>();
		public static List<Font> fonts = new List<Font>();
		// public static List<Font> soundClips = new List<Font>();
		public static List<AnimationClip> animations = new List<AnimationClip>();

		// Text
		public static BitmapFont systemFont;
		// private const string textChars = @"qwertyuiopasdfghjklzxcvbnmQWERTYUIOPASDFGHJKLZXCVBNM0123456789µ§½!""#¤%&/()=?^*@£€${[]}\~¨'-_.:,;<>|°©®±¥";

		public static void LoadAssets()
		{
			loading = true;
			string[] fontPaths = Directory.GetFiles("Data/Fonts", "*.ttf", SearchOption.AllDirectories);
			string[] texPaths = Directory.GetFiles("Data/Textures", "*.png", SearchOption.AllDirectories);

			assetCount = texPaths.Length;

			// Load all fonts
			//foreach (var path in fontPaths)
			//LoadFont(path);

			systemFont = LoadFont("Data/Fonts/Console.ttf");

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

			// GL.BindTexture(TextureTarget.Texture2D, 0);

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
		public static BitmapFont LoadFont(string filePath)
		{
			string fontName = Path.GetFileNameWithoutExtension(filePath);

			var collection = new PrivateFontCollection();
			collection.AddFontFile(filePath);

			var fontFamily = new FontFamily(collection.Families[0].Name, collection);
			Font newFont = new Font(fontFamily, 12);

			return new BitmapFont(newFont, fontName);
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
