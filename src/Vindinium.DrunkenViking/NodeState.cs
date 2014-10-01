using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vindinium.DrunkenViking
{
	[DebuggerDisplay("{DebuggerDisplay}")]
	public class NodeState
	{
		private TileClaim[,] tiles;
		private bool[,] triggers;

		private Dictionary<PlayerType, Queue<Tile>> queue = new Dictionary<PlayerType, Queue<Tile>>();

		/// <summary>Gets the height of the map.</summary>
		public int Height { get { return tiles.GetLength(1); } }

		public State Initial { get; protected set; }
		public PlayerType PlayerToMove { get; protected set; }

		/// <summary>Gets the width of the map.</summary>
		public int Width { get { return tiles.GetLength(0); } }
		public int Depth { get; protected set; }

		public TileClaim DecisionFor { get; protected set; }

		public TileClaim Process()
		{
			while (true)
			{
				foreach (var player in queue.Keys)
				{
					this.Depth++;

					var claim = player.ToTileClaim();
					var q = queue[player];
					var count = q.Count;

					for (int i = 0; i < count; i++)
					{
						var tile = q.Dequeue();

						foreach (var neighor in tile.Neighbors)
						{
							var val = tiles.Get(neighor);

							if (val == claim) { }
							else if (val == TileClaim.None)
							{
								tiles.Set(neighor, claim);
								q.Enqueue(neighor);
							}
							else if (val == TileClaim.Mine)
							{
								// Do mine stuff.
								return claim | val;
							}
							else if (val == TileClaim.Taverne)
							{
								// Do taverne stuff.
								return claim | val;
							}
							else
							{
								// Do battle stuff.
								return claim | val;
							}
						}
					}
				}
			}
		}

		private string DebuggerDisplay
		{
			get
			{
				return string.Format("Depth: {0}", this.Depth);
			}
		}

		/// <summary>Represents the distance array as unit test string.</summary>
		public string ToUnitTestString()
		{
			var sb = new StringBuilder();
			sb.AppendLine();
			sb.AppendLine(new string('-', this.Width * 3 + 1));

			for (var y = 0; y < this.Height; y++)
			{
				sb.Append('|');
				for (var x = 0; x < this.Width; x++)
				{
					sb.AppendFormat("{0,2}|", tiles[x, y].ToUnitTestString());
				}
				sb.AppendLine();
				sb.AppendLine(new string('-', tiles.GetLength(0) * 3 + 1));
			}
			return sb.ToString();
		}

		public static NodeState Create(Map map, State state, PlayerType playerToMove)
		{
			var node = new NodeState()
			{
				tiles = new TileClaim[map.Width, map.Height],
				triggers = new bool[map.Width, map.Height],
				Initial = state,
				Depth = state.Turn,
				PlayerToMove = playerToMove,
			};

			foreach (var mine in map.Mines)
			{
				node.tiles.Set(mine, TileClaim.Mine);
			}
			foreach (var taverne in map.Tavernes)
			{
				node.tiles.Set(taverne, TileClaim.Taverne);
			}

			var hero = state.GetHero(playerToMove);
			var tile = map[hero];
			node.tiles.Set(tile, playerToMove.ToTileClaim());
			node.queue[playerToMove] = new Queue<Tile>();
			if (!hero.IsCrashed)
			{
				node.queue[playerToMove].Enqueue(tile);
			}

			foreach (var player in PlayerTypes.Other[playerToMove])
			{
				hero = state.GetHero(player);
				tile = map[hero];
				node.tiles.Set(tile, player.ToTileClaim());

				node.queue[player] = new Queue<Tile>();

				if (!hero.IsCrashed)
				{
					node.queue[player].Enqueue(tile);
				}
			}
			return node;
		}
	}
}
