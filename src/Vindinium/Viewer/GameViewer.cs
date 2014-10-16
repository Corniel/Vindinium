using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vindinium.Viewer
{
	public class GameViewer
	{
		public void Render(Map map, State state)
		{
			using (new ConsoleScope())
			{
				WriteLine(new String(' ', map.Width * 2 + 4), background: ConsoleColor.DarkBlue);
				
				for (int y = 0; y < map.Height; y++)
				{
					Write("  ", background: ConsoleColor.DarkBlue);
					for (int x = 0; x < map.Width; x++)
					{
						var tile = map[x, y];
						if (tile == null)
						{
							Write("##", ConsoleColor.Green, ConsoleColor.DarkGreen);
						}
						else if (tile.IsMine)
						{
							var mine = string.Format("${0}", (int)state.Mines[tile.MineIndex]).Replace("0", "-");
							Write(mine, ToConsoleColor(state.Mines[tile.MineIndex]));
							
						}
						else if (tile.TileType == TileType.Taverne)
						{
							Write("[]", ConsoleColor.Yellow, ConsoleColor.Black);
						}
						else
						{
							var bg = ConsoleColor.Black;

							var t = "{}";
							var f = ConsoleColor.Gray;

							switch (tile.TileType)
							{
								case TileType.Hero1: f = ToConsoleColor(PlayerType.Hero1); break;
								case TileType.Hero2: f = ToConsoleColor(PlayerType.Hero2); break;
								case TileType.Hero3: f = ToConsoleColor(PlayerType.Hero3); break;
								case TileType.Hero4: f = ToConsoleColor(PlayerType.Hero4); break;
								default: t = "  "; break;
									  
							}

							
							foreach (var player in PlayerTypes.All)
							{
								if (map[state.GetHero(player)] == tile)
								{
									t = String.Format("@{0}", (int)player);
									f = ToConsoleColor(player);
									break;
								}
								
							}
							Write(t, f, bg);
						}
					}
					Write("  ", background: ConsoleColor.DarkBlue);
					WriteLine();
				}
				WriteLine(new String(' ', map.Width * 2 + 4), background: ConsoleColor.DarkBlue);
			}
		}

		private void Write(string value, ConsoleColor foreground = ConsoleColor.White, ConsoleColor background = ConsoleColor.Black)
		{
			Console.ForegroundColor = foreground;
			Console.BackgroundColor = background;

			Console.Write(value);
		}

		private void WriteLine(string value = "", ConsoleColor foreground = ConsoleColor.White, ConsoleColor background = ConsoleColor.Black)
		{
			Write(value, foreground, background);
			Console.WriteLine();
		}

		private ConsoleColor ToConsoleColor(PlayerType tp)
		{
			switch (tp)
			{
				default:
				case PlayerType.None: return ConsoleColor.Gray;
				case PlayerType.Hero1: return ConsoleColor.Red;
				case PlayerType.Hero2: return ConsoleColor.Magenta;
				case PlayerType.Hero3: return ConsoleColor.Blue;
				case PlayerType.Hero4: return ConsoleColor.Cyan;
			}
		}
	}
}
