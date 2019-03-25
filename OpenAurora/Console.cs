using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
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
		public class LogLine
		{
			public string text = "";
			public Color4 color;

			public LogLine(string newText, Color4 newCol)
			{
				text = newText;
				color = newCol;
			}
		}

		public static bool enabled = false;
		private static Mesh backRect = Primitives.CreateRectangle(new Vector3(0, 0, 0), new Vector3(1, 1, 0), new Color4(0, 0, 0, 0.5f));
		private static float height = 240;

		private static List<LogLine> historyLogs = new List<LogLine>();
		private static string inputText = "";
		private static int cursorPosition;

		private const int lineSpacing = 4;

		private static List<Commands.Command> commands = new List<Commands.Command>
		{
			new Commands.ToggleEditor(),
			new Commands.ClearHistory(),
			new Commands.Exit(),

			new Commands.Godmode(),
			new Commands.Noclip(),
		};

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

		public static void OnKeyPress(object sender, KeyPressEventArgs e)
		{
			if (!enabled)
				return;

			char c = e.KeyChar;

			if (c != '`')
			{
				inputText += e.KeyChar;
				MoveCursorForward();
			}
		}

		public static void Update()
		{
			if (!enabled)
				return;

			if (Input.GetKeyDown(Key.BackSpace))
			{
				if (inputText.Length > 0 && cursorPosition > 0)
				{
					inputText = inputText.Remove(cursorPosition - 1, 1);
					MoveCursorBack();
				}
			}

			if (Input.GetKeyDown(Key.Delete))
			{
				if (Input.GetKey(Key.LShift))
				{
					ClearInput();
				}
				else if (inputText.Length > 0 && cursorPosition < inputText.Length)
				{
					inputText = inputText.Remove(cursorPosition, 1);
				}
			}

			if (Input.GetKeyDown(Key.Left))
				MoveCursorBack();
			if (Input.GetKeyDown(Key.Right))
				MoveCursorForward();

			if (Input.GetKeyDown(Key.Enter) || Input.GetKeyDown(Key.KeypadEnter))
				ExecuteInput();

			int hightChangeSize = Resources.systemFont.size + lineSpacing;

			if (Input.GetKeyDown(Key.PageDown))
				height += Resources.systemFont.size + lineSpacing;
			if (Input.GetKeyDown(Key.PageUp) && height > hightChangeSize)
				height -= Resources.systemFont.size + lineSpacing;
		}

		private static void MoveCursorBack()
		{
			if (cursorPosition > 0)
				cursorPosition--;
		}

		private static void MoveCursorForward()
		{
			if (cursorPosition < inputText.Length)
				cursorPosition++;
		}

		public static void WriteLine(string txt, Color4 col)
		{
			historyLogs.Add(new LogLine(txt, col));
		}

		public static void Clear()
		{
			historyLogs = new List<LogLine>();
		}

		public static void ClearInput()
		{
			inputText = "";
			cursorPosition = 0;
		}

		private static void ExecuteInput()
		{
			bool commandExecuted = false;
			
			if (inputText != "")
			{
				foreach (var com in commands)
				{
					if (inputText.ToLower() == com.key.ToLower())
					{
						com.Execute();
						commandExecuted = true;
						break;
					}
				}
				if (!commandExecuted)
					WriteLine("\'" + inputText + "\' is not a valid command.", Color4.Red);

				ClearInput();
			}
		}

		public static void Render()
		{
			if (!enabled)
				return;

			int spacing = Resources.systemFont.charSpacing;
			int lineHeight = Resources.systemFont.size;
			int left = 2;
			int inputTextPos = (int)height - lineHeight;
			int spacedLineHeight = lineHeight + lineSpacing;

			Game.window.CursorVisible = true;

			Draw.ScreenMesh(backRect, new Vector3(Screen.width * 0.5f, height / 2, 0), Quaternion.Identity, new Vector3(Screen.width, height, 0));

			Draw.Text(new Vector2(left, inputTextPos), "/>" + inputText, Resources.systemFont, Color4.White);
			Draw.Text(new Vector2(left + spacing * 2 + cursorPosition * spacing, inputTextPos), "_", Resources.systemFont, Color4.White);

			// History
			for (int i = 0; i < historyLogs.Count; i++)
			{
				float yPos = inputTextPos - spacedLineHeight * (historyLogs.Count + 1) + spacedLineHeight * (i + 1);
				if (yPos > -spacedLineHeight)
				{
					var line = historyLogs[i];
					Draw.Text(new Vector2(left, yPos), line.text, Resources.systemFont, line.color);
				}
			}
		}
	}

	// Console commands
	public class Commands
	{
		public class Command
		{
			public string key = "";
			public virtual void Execute() { }
		}

		public class ToggleEditor : Command
		{
			public ToggleEditor() { key = "editor"; }
			public override void Execute()
			{
				Editor.Toggle();
				switch (Game.mode)
				{
					case Game.Mode.Game:
						Console.WriteLine("Game mode Enabled", Color4.Yellow);
						break;
					case Game.Mode.Editor:
						Console.WriteLine("Edit mode Enabled", Color4.Yellow);
						break;
				}
			}
		}

		public class ClearHistory : Command
		{
			public ClearHistory() { key = "cls"; }
			public override void Execute()
			{
				Console.Clear();
			}
		}

		public class Exit : Command
		{
			public Exit() { key = "exit"; }
			public override void Execute()
			{
				Game.Quit();
			}
		}

		public class SetTimeScale : Command
		{
			public SetTimeScale() { key = "time scale"; }
			public override void Execute()
			{
			}
		}

		#region Cheats
		public class Godmode : Command
		{
			public Godmode() { key = "god"; }
			public override void Execute()
			{
				Cheats.godmode = !Cheats.godmode;
				Console.WriteLine("God mode " + GetToggleModifier(Cheats.godmode), Color4.Yellow);
			}
		}

		public class Noclip : Command
		{
			public Noclip() { key = "noclip"; }
			public override void Execute()
			{
				Cheats.noclip = !Cheats.noclip;
				Console.WriteLine("Noclip " + GetToggleModifier(Cheats.noclip), Color4.Yellow);
			}
		}
		#endregion

		public static string GetToggleModifier(bool b)
		{
			if (b)
				return "Enabled";
			else
				return "Disabled";
		}
	}

	public class Cheats
	{
		public static bool godmode;
		public static bool noclip;
	}
}
