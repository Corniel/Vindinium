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
			this.Neighbors = m_Targets.Values.ToArray();
			this.MineIndex = -1;

			m_Targets[MoveDirection.x] = this;
			this.Targets = m_Targets.Values.ToArray();

			this.IsMine = tp >= TileType.GoldMine1 && tp <= TileType.GoldMine;
			this.IsTaverne = tp == TileType.Taverne;
			this.IsPassable = tp == TileType.Empty || (tp >= TileType.Hero1 && tp <= TileType.Hero4);
		}

		public int X { get; protected set; }
		public int Y { get; protected set; }
		public ulong Dimensions { get; protected set; }

		public TileType TileType { get; protected set; }

		public bool IsMine { get; protected set; }
		public bool IsTaverne { get; protected set; }
		public bool IsPassable { get; internal set; }

		/// <summary>Gets the player type of the hero who occupies the tile.</summary>
		/// <remarks>
		/// Returns none if the tile is not occupied.
		/// </remarks>
		public PlayerType GetOccupied(State state)
		{
			foreach (var player in PlayerTypes.All)
			{
				if (state.GetHero(player).Dimensions == this.Dimensions)
				{
					return player;
				}
			}
			return PlayerType.None;
		}

		/// <summary>Gets the index of the mine.</summary>
		public int MineIndex { get; internal set; }
		

		public Tile[] Neighbors { get; protected set; }
		public Tile[] Targets { get; protected set; }
		public MoveDirection[] Directions { get; protected set; }
		public Tile this[MoveDirection dir] { get { return m_Targets[dir]; } }

		protected Dictionary<MoveDirection, Tile> m_Targets = new Dictionary<MoveDirection, Tile>()
		{
			{ MoveDirection.N, null },
			{ MoveDirection.E, null },
			{ MoveDirection.S, null },
			{ MoveDirection.W, null },
		};

		internal void SetNeighbor(Tile neighbor, MoveDirection dir)
		{
			m_Targets[dir] = neighbor;

			// All neighbors.
			this.Neighbors = m_Targets.Values
				.Where(n => n != null &&
					n != this).ToArray();

			// All targets.
			this.Targets = m_Targets.Values
				.Where(n => n != null).ToArray();

			// All possible directions including stay.
			this.Directions = m_Targets.Keys.Where(d => m_Targets[d] != null).ToArray();
		}

		[ExcludeFromCodeCoverage]
		public string DebugToString()
		{
			return string.Format("Tile[{0},{1}] {2}, Neighbors: {3}",
				X, Y, TileType, Neighbors == null ? 0 : Neighbors.Length);
		}
	}
}
