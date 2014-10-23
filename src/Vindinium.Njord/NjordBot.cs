using System;
using System.Diagnostics;
using System.Linq;
using Vindinium.App;

namespace Vindinium.Njord
{
	/// <summary>A bot that gets his moves based on a heatmap.</summary>
	/// <remarks>In Norse mythology, Njörðr (Njord) is a god that calms both sea and fire.</remarks>
	public class NjordBot : Program<NjordBot>
	{
		public static void Main(string[] args) { NjordBot.DoMain(args); }

		protected bool[,] Blocked { get; set; }
		protected double[,] HeatMap { get; set; }

		protected override void CreateGame()
		{
			base.CreateGame();

			this.Blocked = new bool[this.Map.Width, this.Map.Height];
			this.HeatMap = new double[this.Map.Width, this.Map.Height];
		}

		public override Move GetMove()
		{
			var sw = new Stopwatch();
			sw.Start();

			UpdateState();
			UpdateBlocked();
			UpdateHeatMap();

			var location = this.Map[this.State.GetHero(this.Player)];
			var values = location.Directions.ToDictionary(d => d, d=> HeatMap.Get(location[d]));
			var direction = values.OrderByDescending(kvp => kvp.Value).First().Key;

			var evaluation = String.Format("{0,4} {1} {2,6}µs", this.State.Turn, direction, (1000.0 * sw.Elapsed.TotalMilliseconds).ToString("#,##0"));
			Console.WriteLine(evaluation);
			sw.Stop();
			return new Move(direction, evaluation);
		}

		private void UpdateBlocked()
		{
			this.Blocked.Clear();

			foreach (var hero in PlayerTypes.All.Select(p => this.State.GetHero(p)))
			{
				this.Blocked[hero.X, hero.Y] = true;
			}

			for (var x = 0; x < this.Map.Width; x++)
			{
				for (var y = 0; y < this.Map.Height; y++)
				{
					var tile = this.Map[x, y];
					if (tile == null || !tile.IsPassable)
					{
						this.Blocked[x, y] = true;
					}
				}
			}
		}

		private void UpdateHeatMap()
		{
			this.HeatMap.Clear();

			var hero = this.State.GetHero(this.Player);

			foreach (var player in PlayerTypes.Other[this.Player])
			{
				//var spawn = this.Map.GetSpawn(player);
				//// spawns.
				//var score = this.HeatMap.Get(spawn);
				//this.HeatMap.Set(spawn, score - 20);

				var oppo = this.State.GetHero(player);
				var score = this.HeatMap.Get(this.Map[oppo]);
				double healthdif = hero.Health - oppo.Health;

				if (healthdif > Hero.HealthBattle)
				{
					score += (oppo.Mines - 1) * 6000;
				}
				else
				{
					score -= hero.Mines * 1200;
				}
				this.HeatMap.Set(this.Map[oppo], score);
			}

			// Set mines scores.
			foreach (var mine in this.Map.Mines)
			{
				var owner = this.State.Mines[mine.MineIndex];
				var dis = Map.GetManhattanDistance(hero, mine);

				// Only mines we can reach.
				if (owner != this.Player && dis < hero.Health - 2 * Hero.HealthBattle)
				{
					var score = HeatMap.Get(mine);
					score += owner == PlayerType.None ? 10000 : 9000;
					this.HeatMap.Set(mine, score);
				}
			}

			if (hero.Health < Hero.HealthMax - Hero.HealthBattle)
			{
				// Set taverne scores.
				foreach (var taverne in this.Map.Tavernes)
				{
					var score = this.HeatMap.Get(taverne);
					score += 0.01 * (100 - hero.Health);
					this.HeatMap.Set(taverne, score);
				}
			}
			var iterations = 1 + System.Math.Min((1200 - this.State.Turn) >> 2, 19);
			var neighborValue = 0.10 / iterations;
			CalculateHeatMap(iterations, 0.80, neighborValue);
		}

		private void CalculateHeatMap(int iterations, double ownValue, double neighborValue)
		{
			for (var i = 0; i < iterations; i++)
			{
				var temp = new double[this.Map.Width, this.Map.Height];

				for (var x = 0; x < this.Map.Width; x++)
				{
					for (var y = 0; y < this.Map.Height; y++)
					{
						var tile = this.Map[x, y];
						if (tile != null)
						{
							var score = this.HeatMap[x, y];

							foreach (var n in tile.Neighbors.Where(t => !this.Blocked.Get(t)))
							{
								temp[n.X, n.Y] += score * neighborValue;
							}
							if (this.Blocked.Get(tile))
							{
								temp[x, y] += score;
							}
							else
							{
								temp[x, y] += score * ownValue;
							}
						}
					}
				}
				this.HeatMap = temp;
			}
		}
	}
}
