using System.Diagnostics;
using System.Linq;

namespace Vindinium.Ygritte.Decisions
{
	public abstract class Move
	{
		public static readonly MoveCrashed Crashed = new MoveCrashed();

		public abstract Tile GetTarget(Tile source);
	}

	[DebuggerDisplay("Move: crashed")]
	public class MoveCrashed : Move
	{
		public override Tile GetTarget(Tile source) { return source; }
	}

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

		public override Tile GetTarget(Tile source)
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
					for (var y = 0; x < Distances.GetLength(1); y++)
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

	[DebuggerDisplay("{DebuggerDisplay}")]
	public class SingleMove : Move
	{
		public SingleMove(Tile source, Tile target)
		{
			this.Source = source;
			this.Target = target;
		}

		public Tile Source { get; protected set; }
		public Tile Target { get; protected set; }

		public override Tile GetTarget(Tile source)
		{
			if (source == this.Source) { return this.Target; }
			return null;
		}

		public override bool Equals(object obj)
		{
			var move = obj as SingleMove;
			if (move != null)
			{
				return move.Target == this.Target && move.Source == this.Source;
			}
			return false;
		}
		public override int GetHashCode()
		{
			return this.Source.GetHashCode() ^ this.Target.GetHashCode();
		}

		public string DebuggerDisplay
		{
			get
			{
				return string.Format("Move: ({0},{1}) => ({2},{3})",
					Source.X,
					Source.Y,
					Target.X,
					Target.Y);
			}
		}
	}
}
