using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenAurora
{
	public class Editor
	{
		public static void Toggle()
		{
			if (Game.mode == Game.Mode.Editor)
				Disable();
			else
				Enable();
		}
		public static void Enable()
		{
			Game.mode = Game.Mode.Editor;
		}
		public static void Disable()
		{
			Game.mode = Game.Mode.Game;
		}
	}
}
