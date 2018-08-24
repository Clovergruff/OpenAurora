using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenAurora
{
	class Program
	{
		static void Main(string[] args)
		{
			OpenTK.GameWindow window = new OpenTK.GameWindow(1024, 600, new OpenTK.Graphics.GraphicsMode(32, 8, 0, 0));

			Game game = new Game(window);
			window.Run(1d / 60d);
		}
	}
}
