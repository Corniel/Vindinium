using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Vindinium
{
	/// <summary>Represents a Vindinium map.</summary>
	public class Map : IEnumerable<Tile>
	{
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

		/// <summary>Gerts the (re)spawn tile for a player.</summary>
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

			return map;
		}
		/// <summary>Parses a map.</summary>
		public static Map Parse(string str)
		{
			var lines = str.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

			return Parse(lines);
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
		}

		public Distance[,] GetDistances(params Tile[] tiles)
		{
			var distances = Distances.Create(this.Height, this.Width);

			var dis = Distance.Zero;
			var queue = new Queue<Tile>();
			foreach (var tile in tiles)
			{
				queue.Enqueue(tile);
				distances.Set(tile, dis);
			}
			dis++;

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
			return distances;
		}
	}
}
