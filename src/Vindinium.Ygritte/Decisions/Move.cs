using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vindinium.Ygritte.Decisions
{
	public abstract class Move
	{
		public abstract Tile Get(Tile source);
	}

	public class MoveCrashed : Move
	{
		public override Tile Get(Tile source) { return source; }
	}

	public class MoveFromPath : Move
	{
		public MoveFromPath(Distance[,] distances)
		{
			this.Distances = distances;
		}
		public Distance[,] Distances { get; protected set; }

		public override Tile Get(Tile source)
		{
			var distance = this.Distances.Get(source);
			return source.Neighbors.FirstOrDefault(target => this.Distances.Get(target) < distance);
		}
	}
}
