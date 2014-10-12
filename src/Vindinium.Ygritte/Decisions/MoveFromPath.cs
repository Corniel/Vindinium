using System.Diagnostics;
using System.Linq;

namespace Vindinium.Ygritte.Decisions
{
	[DebuggerDisplay("{DebuggerDisplay}")]
	public class MoveFromPath : Move
	{
		public MoveFromPath(Distance[,] distances)
		{
			this.Distances = distances;
		}
		public Distance[,] Distances { get; protected set; }

		public Distance GetDistance(Tile source)
		{
			return Distances.Get(source);
		}

		public override Tile GetTarget(Tile source, Map map, State state)
		{
			var distance = this.Distances.Get(source);
			return source.Neighbors.FirstOrDefault(target => this.Distances.Get(target) < distance);
		}

		public string DebuggerDisplay
		{
			get
			{
				var x_best = 0;
				var y_best = 0;
				var distance = Distance.Unknown;

				for (var x = 0; x < Distances.GetLength(0); x++)
				{
					for (var y = 0; y < Distances.GetLength(1); y++)
					{
						var test = Distances[x, y];
						if (test < distance)
						{
							distance = test;
							x_best = x;
							y_best = y;
						}
					}
				}
				return string.Format("Move: Target: ({0},{1})", x_best, y_best);
			}
		}
	}
}
