﻿using System;
using System.IO;
using System.Linq;
using System.Net;
using Vindinium.Net;

namespace Vindinium.App
{
	public abstract class Program<T> where T : Program<T>
	{
		protected Program() { }

		/// <summary>The client parameters.</summary>
		public ClientParameters Parameters { get; protected set; }

		/// <summary>The client.</summary>
		public Client Client { get; protected set; }

		protected Logging.Game Game { get; set; }
		protected State State { get; set; }

		public void Run()
		{
			int left = this.Parameters.Runs;
			int run = 1;
			while (left == -1 || left-- > 0)
			{
				Console.WriteLine("Run {0} of {1}", run++, left == -1 ? "oo" : this.Parameters.Runs.ToString());

				this.Client = new Client(this.Parameters);
				this.Client.CreateGame();

				CreateLogGame();

				if (this.Client.IsCrashed) { continue; }

				using (var writer = new StreamWriter("replay.html", true))
				{
					writer.WriteLine("<a href='{0}'>{0}</a><br />", this.Client.Response.viewUrl);
				}

				CreateGame();

				while (!this.Client.IsFinished && !this.Client.IsCrashed)
				{
					try
					{
						Console.Write("{0,4}\r", this.Client.Response.game.turn);
						var move = GetMove();

						LogTurn();
						this.Client.Move(move);
						LogMove(move);
					}
					catch (WebException x)
					{
						Console.WriteLine(x.Message);
						this.Client.IsCrashed = true;
						LogCrashed(x.Message);
					}
					catch (Exception x)
					{
						Console.WriteLine(x);
						this.Client.IsCrashed = true;
						LogCrashed(x.ToString());
					}
				}
				Console.WriteLine(this.Client.Response.viewUrl);
			}
		}

		private void LogCrashed(string message)
		{
			if (Logging.Game.GamesDir != null)
			{
				this.Game.Heros[(int)this.Client.Player - 1].IsCrashed = true;
				this.Game.Heros[(int)this.Client.Player - 1].Message = message;
				this.Game.Save();
			}
		}

		private void LogMove(MoveDirection move)
		{
			if (Logging.Game.GamesDir != null)
			{
				this.Game.Turns.Last().Move = move.ToString();

				foreach (var p in PlayerTypes.All)
				{
					var hero = this.State.GetHero(p);
					this.Game.Heros[(int)p - 1].Gold = hero.Gold;
				}

				var rank = this.Game.Heros.OrderBy(h => h.Gold).ToList();

				for (var i = 0; i < 4; i++)
				{
					var id = rank[i].Id - 1;
					var hero = this.Game.Heros[id];
					hero.Rank = i + 1;
				}
				this.Game.Save();
			}
		}

		private void LogTurn()
		{
			if (Logging.Game.GamesDir != null)
			{
				this.State = this.State.Update(this.Client.Response.game);
				this.Game.Turns.Add(Logging.Turn.Create(this.State));
				this.Game.Save();
			}
		}

		private void CreateLogGame()
		{
			if (Logging.Game.GamesDir != null)
			{
				this.Game = Logging.Game.Create(this.Client.Response.game);
				this.State = State.Create(Map.Parse(this.Client.Response.game.board.ToRows()));
				this.Game.Save();
			}
		}

		protected virtual void CreateGame() { }

		protected abstract MoveDirection GetMove();

		public static void DoMain(string[] args)
		{
			var program = (T)Activator.CreateInstance(typeof(T));
			program.Parameters = ClientParameters.FromConfig();

			program.Run();
		}
	}
}
