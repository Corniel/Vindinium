using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using Vindinium.App;
using Vindinium.Logging;
using Vindinium.Viewer;
using System.Linq;
using Vindinium.Data;
using System.IO;
using System.Text.RegularExpressions;

namespace Vindinium.Viewer
{
	public class Program
	{
		public static readonly Regex GameSelectionPattern = new Regex("^[0-9]{1,8}$", RegexOptions.Compiled);
		public static void Main(string[] args)
		{
			Console.SetWindowSize(120, 40);

			var program = Program.Load();
			
			while (true)
			{
				if (program.SelectedGame != null)
				{
					program.RenderGameReplay();
				}
				else
				{
					program.RenderGamesOverview();
				}
			}
		}

		

		public Program() { }
		public Ratings PlayerRatings { get; protected set; }
		public List<Game> Games { get; protected set; }

		public int GameSelectionIndex { get; protected set; }
		public string GameFilter { get; protected set; }
		public Game SelectedGame { get; protected set; }

		private void LoadGames()
		{
			this.Games = new List<Game>();
			var i = 0;
			foreach (var game in Game.GetAll())
			{
				Console.Write("\rLoading game {0}.", ++i);
				this.Games.Add(game);
			}
			this.Games = this.Games.OrderByDescending(g => g.CreationTime).ToList();
			Console.WriteLine();
		}
		private void ProcessRatings()
		{
			var unprocessed = this.Games.Where(g => !g.Rated).ToList();
			var i = 0;
			foreach (var game in unprocessed)
			{
				Console.Write("\rProcess ratings: {0} out of {1}", ++i, unprocessed.Count);

				foreach (var hero in game.Heros)
				{
					var player = this.PlayerRatings.GetPlayer(hero.Name);
					if (player == null)
					{
						player = new Player() { Name = hero.Name, RatingPoints = new List<RatingPoint>() };
						this.PlayerRatings.Players.Add(player);
					}
					var rp = new RatingPoint() { Date = game.CreationTime, Elo = hero.Elo };
					if (!player.RatingPoints.Contains(rp))
					{
						player.RatingPoints.Insert(0, rp);
						player.RatingPoints.Sort();
					}
				}
				game.Rated = true;
				game.Save(game.FileLocation);
			}
			if (i > 0)
			{
				Console.WriteLine();

				foreach (var player in this.PlayerRatings.Players)
				{
					player.RatingPoints.Sort();
				}
				this.PlayerRatings.Players.Sort();
				this.PlayerRatings.Save();

				using (var writer = new StreamWriter(Path.Combine(Game.GamesDir.FullName, "ratings.txt")))
				{
					var pos = 1;
					foreach (var player in this.PlayerRatings.Players)
					{
						writer.WriteLine("{0,3}  {1}", pos++, player.DebuggerDisplay);
					}
				}
			}
		}

		private void RenderGameReplay()
		{
			if (this.SelectedGame == null) { return; }

			var turn = 0;
			var turnMax = this.SelectedGame.Turns.Count - 1;
			var rerender = true;
			var map = this.SelectedGame.Map.Mp;
	
			while (true)
			{
				if (rerender)
				{
					var viewer = new GameViewer();
					var state = this.SelectedGame.Turns[turn].State;

					Console.Clear();
					Console.WriteLine("Game: Turn {0} of {1}", this.SelectedGame.Turns[turn].Id, this.SelectedGame.Turns[turnMax].Id);
					RenderDoubleLine();
					foreach (var p in PlayerTypes.All.OrderByDescending(tp => state.GetHero(tp).Gold))
					{
						var player = this.SelectedGame.Heros.FirstOrDefault(h => h.Id == (int)p);
						var hero = state.GetHero(p);

						Console.Write(" ");
						Console.BackgroundColor = GameViewer.ToConsoleColor(p);
						Console.Write(" ");
						Console.BackgroundColor = ConsoleColor.Black;

						Console.WriteLine("  {5,-17} ({6,4}) {0} Health: {1,3}, Mines: {2,2}, Gold: {3,4}, Exp: {4}",
							p,
							hero.Health,
							hero.Mines,
							hero.Gold,
							hero.Gold + hero.Mines * ((1195 + (int)p - state.Turn) >> 2),
							player.Name.Substring(0, Math.Min(16, player.Name.Length)) + (hero.IsCrashed ? "*": ""),
							player.Elo
						);
					}
					RenderSingleLine();
					if (!String.IsNullOrEmpty(this.SelectedGame.Turns[turn].Evaluation))
					{
						Console.WriteLine(this.SelectedGame.Turns[turn].Evaluation);
						RenderSingleLine();
					}
					
					viewer.Render(map, state);
				}

				var input = Console.ReadKey(true);
				switch (input.Key)
				{
					case ConsoleKey.LeftArrow:
						if (turn > 0) { turn--; rerender = true; }
						else { rerender = false; }
						break;

					case ConsoleKey.RightArrow:
						if (turn < turnMax - 1) { turn++; rerender = true; }
						else { rerender = false; }
						break;

					case ConsoleKey.PageUp:
						if (turn > 0) { turn = Math.Max(0, turn - 10); rerender = true; }
						else { rerender = false; }
						break;

					case ConsoleKey.PageDown:
						if (turn < turnMax - 1) { turn = Math.Min(turnMax, turn + 10); rerender = true; }
						else { rerender = false; }
						break;

					case ConsoleKey.Q:
					case ConsoleKey.Delete:
						this.SelectedGame = null;
						return;

					default: rerender = false; break;
				}
			}
		}
		private void RenderGamesOverview()
		{
			var gamesPerPage = Console.WindowHeight - 6;

			var games = Filter().ToList();

			Console.Clear();
			Console.WriteLine("Games{0}: {1}-{2} from {3}",
				String.IsNullOrEmpty(this.GameFilter) ? "" : string.Format(" (filter: '{0}')", this.GameFilter),
				this.GameSelectionIndex,
				this.GameSelectionIndex + gamesPerPage - 1, games.Count);
			RenderDoubleLine();
			Console.WriteLine("  pos  date      size mines players");
			RenderSingleLine();

			int i = this.GameSelectionIndex;
			foreach (var game in games.Skip(this.GameSelectionIndex).Take(gamesPerPage))
			{
				Console.WriteLine("{0,5}  {1:yyyy-MM-dd}  {2,2}  {3,3}  {4}",
					i++,
					game.CreationTime,
					game.Map.Size,
					game.Map.Mines,
					String.Join(" ", game.Heros.OrderByDescending(h => h.Gold).Select(h => String.Format("{0} ({1})", h.Name, h.Elo))));
			}
			Console.WriteLine(new String('-', Console.WindowWidth - 1));

			var gameFilter = "";

			while (this.SelectedGame == null)
			{
				var input = Console.ReadKey(true);
				switch (input.Key)
				{
					case ConsoleKey.PageDown:
						this.GameSelectionIndex = Math.Min(games.Count - 1, this.GameSelectionIndex + gamesPerPage);
						RenderGamesOverview();
						break;

					case ConsoleKey.PageUp:
						this.GameSelectionIndex = Math.Max(0, this.GameSelectionIndex - gamesPerPage);
						RenderGamesOverview();
						break;
					case ConsoleKey.Delete:
						Console.Write("\r"+ new string(' ', gameFilter.Length));
						Console.CursorLeft = 0;
						gameFilter = string.Empty;
						break;
					case ConsoleKey.Backspace:
						gameFilter = gameFilter.Substring(0, Math.Max(1, gameFilter.Length - 1));
						Console.Write("\r");
						Console.Write(gameFilter + " ");
						Console.CursorLeft = Console.CursorLeft - 1;
						break;
					case ConsoleKey.Enter:
						int gameId;
						if (GameSelectionPattern.IsMatch(gameFilter) && int.TryParse(gameFilter, out gameId) && gameId < games.Count)
						{
							this.SelectedGame = games[gameId];
							break;
						}
						else
						{
							this.GameFilter = gameFilter;
							gameFilter = string.Empty;
							RenderGamesOverview();
							break;
						}
					default:
						gameFilter += input.KeyChar;
						Console.Write(input.KeyChar);
						break;
				}
			}
		}

		private static void RenderSingleLine()
		{
			Console.WriteLine(new String('-', Console.WindowWidth - 1));
		}

		private static void RenderDoubleLine()
		{
			Console.WriteLine(new String('=', Console.WindowWidth - 1));
		}

		public static Program Load()
		{
			var program = new Program();
			program.PlayerRatings = Ratings.Load();
			program.LoadGames();
			program.ProcessRatings();

			return program;
		}

		public IEnumerable<Game> Filter()
		{
			var filter = (this.GameFilter ?? String.Empty).Trim().ToLower();
			foreach (var game in this.Games)
			{
				if(String.IsNullOrEmpty(filter) || 
					game.Heros.Any(hero => hero.Name.ToLower().Contains(filter)))
				{
					yield return game;
				}
			}
		}
	}
}
