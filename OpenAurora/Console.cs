using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenAurora
{
	public class Console
	{
		public static bool enabled = false;

		public static void Toggle()
		{
			if (enabled)
				Disable();
			else
				Enable();
		}
		public static void Enable()
		{
			if (enabled)
				return;

			enabled = true;
		}
		public static void Disable()
		{
			if (!enabled)
				return;

			enabled = false;
		}

		public static void Update()
		{
			if (!enabled)
				return;
		}

		public static void Render()
		{
			if (!enabled)
				return;

			GL.BlendColor(1, 0, 1, 1);
			Draw.Mesh(Primitives.rectangle, new Vector3(Screen.width * 0.5f, 64, 0), Quaternion.Identity, new Vector3(Screen.width, 128, 0));
		}
	}
}
