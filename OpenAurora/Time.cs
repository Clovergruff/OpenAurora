using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenAurora
{
	public class Time
	{
		public static double time;
		public static float timeScale = 1;
		public static float deltaTime;
		public static float unscaledDeltaTime;

		private static double previousTime;

		public static void GetDeltaTime(FrameEventArgs e)
		{
			time += e.Time;
			deltaTime = (float)(time - previousTime);
			previousTime = time;
		}
	}
}
