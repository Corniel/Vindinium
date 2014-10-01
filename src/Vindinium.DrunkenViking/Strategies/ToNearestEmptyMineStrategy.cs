using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vindinium.DrunkenViking.Strategies
{
	public class ToNearestEmptyMineStrategy : Strategy
	{
		public ToNearestEmptyMineStrategy(Map map) : base(map) 
		{
			this.DistanceToMine = new Dictionary<Tile, Distance[,]>();
		}

		public override void Initializes()
		{
			foreach (var tile in this.Map.Mines)
			{
				this.DistanceToMine[tile] = this.Map.GetDistances(tile);
			}
		}

		public Dictionary<Tile, Distance[,]> DistanceToMine { get; protected set; }

		public override bool Applies(State state, Tile location, Hero hero, PlayerType playerToMove)
		{
			return
				state.Mines.Count(PlayerType.None, this.Map.Mines.Length) > 0;
		}

		public override MoveDirection GetMove(State state, Tile location, Hero hero, PlayerType playerToMove)
		{
			Tile target = null;
			Distance min = Distance.Unknown;

			foreach (var mine in this.Map.Mines)
			{
				if (state.Mines[mine.MineIndex] == PlayerType.None)
				{
					var distance = this.DistanceToMine[mine].Get(location);
					if (distance < min)
					{
						min = distance;
						target = mine;
					}
				}
			}

			if (target != null)
			{
				foreach (var dir in location.Directions)
				{
					if (this.DistanceToMine[target].Get(location[dir]) < min)
					{
						return dir;
					}
				}
			}

			return MoveDirection.x;
		}
	}
}
