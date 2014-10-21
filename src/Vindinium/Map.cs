using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text;

namespace Vindinium
{
	/// <summary>Represents a Vindinium map.</summary>
	public class Map : IEnumerable<Tile>
	{
		private static volatile object locker = new object();

		public static int GetManhattanDistance(Hero hero1, Hero hero2)
		{
			uint key = (uint)((hero1.Dimensions >> Hero.PositionX) | (hero2.Dimensions << (16 - Hero.PositionX)));
			byte distance;
			
			lock (locker)
			{
				if (!ManhattanDistance.TryGetValue(key, out distance))
				{
					distance = (byte)(Math.Abs(hero1.X - hero2.X) + Math.Abs(hero1.Y - hero2.Y));
					ManhattanDistance[key] = distance;
				}
			}
			return distance;
		}
		public static int GetManhattanDistance(Hero hero, Tile mine)
		{
			uint key = (uint)((hero.Dimensions >> Hero.PositionX) | (mine.Dimensions << (16 - Hero.PositionX)));
			
			byte distance;
			lock (locker)
			{
				if (!ManhattanDistance.TryGetValue(key, out distance))
				{
					distance = (byte)(Math.Abs(hero.X - mine.X) + Math.Abs(hero.Y - mine.Y));
					ManhattanDistance[key] = distance;
				}
			}
			return distance;
		}
		public static int GetManhattanDistance(Tile tile0, Tile tile1)
		{
			uint key = (uint)((tile0.Dimensions >> Hero.PositionX) | (tile1.Dimensions << (16 - Hero.PositionX)));

			byte distance;
			lock (locker)
			{
				if (!ManhattanDistance.TryGetValue(key, out distance))
				{
					distance = (byte)(Math.Abs(tile0.X - tile1.X) + Math.Abs(tile0.Y - tile1.Y));
					ManhattanDistance[key] = distance;
				}
			}
			return distance;
		}
		private static readonly Dictionary<uint, byte> ManhattanDistance = new Dictionary<uint, byte>();

		private Dictionary<Tile, Distance[,]> m_Distances = new Dictionary<Tile, Distance[,]>();
		private Tile[,] m_Tiles;
		private List<Tile> m_All;

		/// <summary>Gets a tile based on its coordinates.</summary>
		public Tile this[int x, int y]
		{
			get
			{
				return m_Tiles[x, y];
			}
		}

		/// <summary>Gets the tile of the hero.</summary>
		public Tile this[Hero hero]
		{
			get
			{
				return this[hero.X, hero.Y];
			}
		}

		/// <summary>Gets the number of passable tiles.</summary>
		public int Count { get { return m_All.Count; } }

		/// <summary>Gets the number of pre calculated distances to tiles.</summary>
		public int DistancesCount { get { return m_Distances.Count; } }

		public void ClearDistances() { m_Distances.Clear(); }

		/// <summary>Loops through all tiles.</summary>
		public IEnumerator<Tile> GetEnumerator()
		{
			return m_All.GetEnumerator();
		}
		[ExcludeFromCodeCoverage]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return m_All.GetEnumerator();
		}

		/// <summary>Gets the height of the map.</summary>
		public int Height { get { return m_Tiles.GetLength(1); } }

		/// <summary>Gets the width of the map.</summary>
		public int Width { get { return m_Tiles.GetLength(0); } }
		
		/// <summary>Gets the Tavernes of the map.</summary>
		public Tile[] Tavernes { get; protected set; }
		/// <summary>Gets the mines of the map.</summary>
		public Tile[] Mines { get; protected set; }

		/// <summary>Gets the (re)spawn tile for a player.</summary>
		public Tile GetSpawn(PlayerType player)
		{
			switch (player)
			{
				case PlayerType.Hero1: return Spawn1;
				case PlayerType.Hero2: return Spawn2;
				case PlayerType.Hero3: return Spawn3;
				case PlayerType.Hero4: return Spawn4;
				case PlayerType.None:
				default: return null;
			}

		}
		private Tile Spawn1;
		private Tile Spawn2;
		private Tile Spawn3;
		private Tile Spawn4;

		/// <summary>Updates the games based on the response of the request.</summary>
		public void Update(Serialization.Game game)
		{
			foreach (var hero in game.heroes)
			{
				var player = (PlayerType)hero.id;
				if (hero.crashed && !m_IsCrashed[player])
				{
					var spawn = GetSpawn(player);
					var current = this[hero.pos.y, hero.pos.x];
					if (spawn.IsPassable && current == spawn)
					{
						spawn.IsPassable = false;
					}
					// maybe a taverne is not longer available.
					this.DistanceToTaverne = GetDistances(this.Tavernes);
					
					// maybe the paths are not longer equal.
					ClearDistances();

					m_IsCrashed[player] = true;
				}
			}
		}
		private Dictionary<PlayerType, bool> m_IsCrashed = new Dictionary<PlayerType, bool>()
		{
			{ PlayerType.Hero1, false },
			{ PlayerType.Hero2, false },
			{ PlayerType.Hero3, false },
			{ PlayerType.Hero4, false },
		};

		/// <summary>Parses a map.</summary>
		public static Map Parse(string[] lines)
		{
			if (lines == null)
			{
				throw new ArgumentNullException("lines");
			}
			if (lines.Length == 0 || !lines.All(line => lines[0].Length == line.Length))
			{
				throw new ArgumentException("Invalid map");
			}

			var tiles = ParseTiles(lines);
			
			var map = new Map();
			map.AssignTiles(tiles);
			map.AssignNeighbors();
			map.AssignMines();
			map.AssignSpawns();
			map.AssignTavernes();

			map.LogMap(lines);
			return map;
		}
		/// <summary>Parses a map.</summary>
		public static Map Parse(string str)
		{
			var lines = str.Split(new string[] { Environment.NewLine, "\n" }, StringSplitOptions.RemoveEmptyEntries);

			return Parse(lines);
		}
		
		/// <summary>Logs the created map, to the map file.</summary>
		private void LogMap(string[] lines)
		{
			var logpath = ConfigurationManager.AppSettings["maps"];
			if (!string.IsNullOrEmpty(logpath))
			{
				var dir = new DirectoryInfo(logpath);
				if (!dir.Exists) { dir.Create(); }

				var hash = lines[0].GetHashCode();
				for(int i = 1; i < lines.Length;i++)
				{
					hash^= lines[i].GetHashCode();
				}

				var filename = string.Format("{0:000}x{1:000}_{2:00}m_{3}t_hash{4}.txt",
					this.Width, this.Height,
					this.Mines.Length,
					this.Count - this.Mines.Length - 4,
					hash);

				var file = new FileInfo(Path.Combine(dir.FullName, filename));
				if (!file.Exists)
				{
					using (var writer = new StreamWriter(file.FullName))
					{
						foreach (var line in lines)
						{
							writer.WriteLine(line);
						}
						writer.Flush();
					}
				}
			}
		}

		private static List<Tile> ParseTiles(string[] lines)
		{
			var tiles = new List<Tile>();
			var x = 0;
			var y = 0;
			for (int row = 0; row < lines.Length; row++)
			{
				x = 0;
				var line = lines[row];
				for (int col = 0; col < line.Length - 1; col += 2)
				{
					var t = line.Substring(col, 2);

					var type = TileTypes.TryParse(t);

					if (type == TileType.Unknown)
					{
						throw new ArgumentException("Invalid map");
					}

					var tile = new Tile(x, y, type);
					tiles.Add(tile);
					x++;
				}
				y++;
			}
			return tiles;
		}

		private void AssignTiles(List<Tile> tiles)
		{
			m_Tiles = new Tile[tiles.Max(t => t.X) + 1, tiles.Max(t => t.Y) + 1];
			m_All = tiles.Where(t => t.TileType != TileType.Impassable).ToList();
			foreach (var tile in m_All)
			{
				m_Tiles[tile.X, tile.Y] = tile;
			}
		}
		private void AssignMines()
		{
			this.Mines = m_All.Where(tile => tile.IsMine).ToArray();
			int mineIndex = 0;
			foreach (var mine in this.Mines)
			{
				mine.MineIndex = mineIndex++;
			}
		}
		private void AssignNeighbors()
		{
			foreach (var tile in this)
			{
				var x = tile.X;
				var y = tile.Y;

				var n = y > 0 ? m_Tiles[x, y - 1] : null;
				var s = y < Height - 1 ? m_Tiles[x, y + 1] : null;

				var w = x > 0 ? m_Tiles[x - 1, y] : null;
				var e = x < this.Width - 1 ? m_Tiles[x + 1, y] : null;

				if (n != null && n.TileType != TileType.Impassable)
				{
					tile.SetNeighbor(n, MoveDirection.N);
				}
				if (w != null && w.TileType != TileType.Impassable)
				{
					tile.SetNeighbor(w, MoveDirection.W);
				}
				if (s != null && s.TileType != TileType.Impassable)
				{
					tile.SetNeighbor(s, MoveDirection.S);
				}
				if (e != null && e.TileType != TileType.Impassable)
				{
					tile.SetNeighbor(e, MoveDirection.E);
				}
			}
		}
		private void AssignSpawns()
		{
			this.Spawn1 = m_All.First(tile => tile.TileType == TileType.Hero1);
			this.Spawn2 = m_All.First(tile => tile.TileType == TileType.Hero2);
			this.Spawn3 = m_All.First(tile => tile.TileType == TileType.Hero3);
			this.Spawn4 = m_All.First(tile => tile.TileType == TileType.Hero4);
		}
		private void AssignTavernes()
		{
			this.Tavernes = m_All.Where(tile => tile.TileType == TileType.Taverne).ToArray();
			this.DistanceToTaverne = GetDistances(this.Tavernes);
		}

		/// <summary>Gets a distances map for the specified target.</summary>
		/// <param name="target"></param>
		public Distance[,] GetDistances(Tile target)
		{
#if DEBUG
			if (target == null)
			{
				throw new ArgumentNullException("target");
			}
#endif
			lock (locker)
			{
				Distance[,] distances;

				if(!m_Distances.TryGetValue(target, out distances))
				{
					distances = Distances.Create(this);
					var queue = new Queue<Tile>();
					queue.Enqueue(target);
					distances.Set(target, Distance.Zero);
					
					SetDistances(distances, queue, Distance.One);
					m_Distances[target] = distances;
				}
				return distances;
			}
		}

		/// <summary>Gets a distances map for the specified targets.</summary>
		/// <param name="target"></param>
		public Distance[,] GetDistances(params Tile[] targets)
		{
#if DEBUG
			if (targets == null || targets.Length == 0)
			{
				throw new ArgumentNullException("targets");
			}
#endif
			if (targets.Length == 1)
			{
				return GetDistances(targets[0]);
			}
			var distances = Distances.Create(this);

			var queue = new Queue<Tile>();
			foreach (var tile in targets)
			{
				queue.Enqueue(tile);
				distances.Set(tile, Distance.Zero);
			}
			SetDistances(distances, queue, Distance.One);
			return distances;
		}

		/// <summary>Sets the distances.</summary>
		private void SetDistances(Distance[,] distances, Queue<Tile> queue, Distance dis)
		{
			while (queue.Count > 0)
			{
				int size = queue.Count;
				for (int i = 0; i < size; i++)
				{
					var t = queue.Dequeue();
					foreach (var n in t.Neighbors)
					{
						if (n.IsPassable && distances.Get(n) == Distance.Unknown)
						{
							queue.Enqueue(n);
							distances.Set(n, dis);
						}
					}
				}
				dis++;
			}
		}

		public Distance[,] GetDistances(IEnumerable<Tile> targets, IEnumerable<Tile> enermies)
		{
			var distances = Distances.Create(this.Height, this.Width);

			var dis = Distance.Zero;

			var qOppo = new Queue<Tile>();

			foreach (var tile in enermies.SelectMany(e => e.Targets).Where(t=> t.IsPassable))
			{
				qOppo.Enqueue(tile);
				distances.Set(tile, Distance.Blocked);
			}
			
			var queue = new Queue<Tile>();
			foreach (var tile in targets)
			{
				queue.Enqueue(tile);
				distances.Set(tile, dis);
			}
			dis++;

			while (queue.Count > 0)
			{
				int sOppo = qOppo.Count;
				for (int i = 0; i < sOppo; i++)
				{
					var t = qOppo.Dequeue();
					foreach (var n in t.Neighbors)
					{
						if (n.IsPassable && distances.Get(n) == Distance.Unknown)
						{
							qOppo.Enqueue(n);
							distances.Set(n, Distance.Blocked);
						}
					}
				}

				int size = queue.Count;
				for (int i = 0; i < size; i++)
				{
					var t = queue.Dequeue();
					foreach (var n in t.Neighbors)
					{
						if (n.IsPassable && distances.Get(n) == Distance.Unknown)
						{
							queue.Enqueue(n);
							distances.Set(n, dis);
						}
					}
				}
				dis++;
			}
			return distances;
		}

		public Distance GetDistanceToTaverne(Hero hero)
		{
			return DistanceToTaverne.Get(this[hero]);
		}
		public Distance GetDistanceToTaverne(Tile tile)
		{
			return DistanceToTaverne.Get(tile);
		}
		private Distance[,] DistanceToTaverne { get; set; }

		public override string ToString()
		{
			var sb = new StringBuilder();

			for (int y = 0; y < this.Height; y++)
			{
				sb.AppendLine();
				for (int x = 0; x < this.Width; x++)
				{
					var tile = this[x, y];
					if (tile == null)
					{
						sb.Append("##");
					}
					else if (tile.IsMine)
					{
						sb.Append("$-");
					}
					else if (tile.TileType == TileType.Taverne)
					{
						sb.Append("[]");
					}
					else if (tile.TileType == TileType.Hero1) { sb.Append("@1"); }
					else if (tile.TileType == TileType.Hero2) { sb.Append("@2"); }
					else if (tile.TileType == TileType.Hero3) { sb.Append("@3"); }
					else if (tile.TileType == TileType.Hero4) { sb.Append("@4"); }
					else
					{
						sb.Append("  ");
					}
				}
			}
			return sb.ToString();
		}

		public string ToUnitTestString(State state)
		{
			var sb = new StringBuilder();

			for (int y = 0; y < this.Height; y++)
			{
				sb.AppendLine();
				for (int x = 0; x < this.Width; x++)
				{
					var tile = this[x, y];
					if (tile == null)
					{
						sb.Append("##");
					}
					else if (tile.GetOccupied(state) != PlayerType.None)
					{
						var hero = tile.GetOccupied(state);
						switch (hero)
						{
							default:
							case PlayerType.None: sb.Append("@-"); break;
							case PlayerType.Hero1: sb.Append("@1"); break;
							case PlayerType.Hero2: sb.Append("@2"); break;
							case PlayerType.Hero3: sb.Append("@3"); break;
							case PlayerType.Hero4: sb.Append("@4"); break;
						}
					}
					else if (tile.IsMine)
					{
						var owner = state.Mines[tile.MineIndex];
						switch (owner)
						{
							default:
							case PlayerType.None: sb.Append("$-"); break;
							case PlayerType.Hero1: sb.Append("$1"); break;
							case PlayerType.Hero2: sb.Append("$2"); break;
							case PlayerType.Hero3: sb.Append("$3"); break;
							case PlayerType.Hero4: sb.Append("$4"); break;
						}
					}
					else if (tile.TileType == TileType.Taverne)
					{
						sb.Append("[]");
					}
					else
					{
						sb.Append("  ");
					}
				}
			}
			sb.AppendLine();
			return sb.ToString();
		}
	}
}
