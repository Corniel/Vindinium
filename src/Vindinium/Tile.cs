using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Vindinium
{
	[DebuggerDisplay("{DebugToString()}")]
	public class Tile
	{
		public Tile(int x, int y, TileType tp)
		{
			this.X = x;
			this.Y = y;
			this.Dimensions = ((ulong)x << Hero.PositionX | (ulong)y << Hero.PositionY);
			this.TileType = tp;
			this.Neighbors = m_Neighbors.Values.ToArray();
			this.MineIndex = -1;

			m_Neighbors[MoveDirection.x] = this;
		}

		public int X { get; protected set; }
		public int Y { get; protected set; }
		public ulong Dimensions { get; protected set; }

		public TileType TileType { get; protected set; }

		public bool IsMine()
		{
			return this.TileType >= TileType.GoldMine1 && this.TileType <= TileType.GoldMine;
		}
		
		/// <summary>Gets the index of the mine.</summary>
		public int MineIndex { get; internal set; }

		public Tile[] Neighbors { get; protected set; }
		public MoveDirection[] Directions { get; protected set; }
		public Tile this[MoveDirection dir] { get { return m_Neighbors[dir]; } }

		protected Dictionary<MoveDirection, Tile> m_Neighbors = new Dictionary<MoveDirection, Tile>()
		{
			{ MoveDirection.N, null },
			{ MoveDirection.E, null },
			{ MoveDirection.S, null },
			{ MoveDirection.W, null },
		};

		internal void SetNeighbor(Tile neighbor, MoveDirection dir)
		{
			m_Neighbors[dir] = neighbor;
			
			// All neighbors.
			this.Neighbors = m_Neighbors.Values
				.Where(n => n != null &&
					n != this).ToArray();

			// All possible directions including stay.
			this.Directions = m_Neighbors.Keys.Where(d => m_Neighbors[d] != null).ToArray();
		}

		[ExcludeFromCodeCoverage]
		public string DebugToString()
		{
			return string.Format("Tile[{0},{1}] {2}, Neighbors: {3}",
				X, Y, TileType, Neighbors == null ? 0 : Neighbors.Length);
		}
	}
}
