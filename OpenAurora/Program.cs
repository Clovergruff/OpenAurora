using OpenTK;
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
			GameWindow window = new GameWindow(1024, 600, new OpenTK.Graphics.GraphicsMode(32, 8, 0, 4));
			window.VSync = VSyncMode.Off;

			Screen.scaling = window.Width / 1024;
			//System.Console.WriteLine("Scaling seems to be " + Screen.scaling);

			int halfScrWidth = DisplayDevice.Default.Width / 2;
			int halfScrHeight = DisplayDevice.Default.Height / 2;

			window.Location = new System.Drawing.Point(halfScrWidth - window.Width / 2,
														halfScrHeight - window.Height / 2);

			Game game = new Game(window);
			window.Run(60, 60);
		}
	}
}
