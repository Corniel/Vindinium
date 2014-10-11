using System.Diagnostics;
using System.Linq;

namespace Vindinium.Ygritte.Decisions
{
	public abstract class Move
	{
		public static readonly MoveCrashed Crashed = new MoveCrashed();
		public static readonly MoveStay Stay = new MoveStay();

		public abstract Tile GetTarget(Tile source, Map map, State state);
	}

	[DebuggerDisplay("Move: crashed")]
	public class MoveCrashed : Move
	{
		public override Tile GetTarget(Tile source, Map map, State state) { return source; }
	}

	[DebuggerDisplay("Move: stay")]
	public class MoveStay : Move
	{
		public override Tile GetTarget(Tile source, Map map, State state) { return source; }
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

	[DebuggerDisplay("{DebuggerDisplay}")]
	public class MoveSingle : Move
	{
		public MoveSingle(Tile source, Tile target)
		{
			this.Source = source;
			this.Target = target;
		}

		public Tile Source { get; protected set; }
		public Tile Target { get; protected set; }

		public override Tile GetTarget(Tile source, Map map, State state)
		{
			if (source == this.Source) { return this.Target; }
			return null;
		}

		public override bool Equals(object obj)
		{
			var move = obj as MoveSingle;
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

	[DebuggerDisplay("{DebuggerDisplay}")]
	public class MoveAttack : Move
	{
		public MoveAttack(PlayerType opponent)
		{
			this.Opponent = opponent;
		}
		public PlayerType Opponent { get; protected set; }

		public override Tile GetTarget(Tile source, Map map, State state)
		{
			var oppo = map[state.GetHero(this.Opponent)];
			var distances = MovesGenerator.Instance.GetDistances(map, oppo);

			var distance = distances.Get(source);
			var target = source.Neighbors.FirstOrDefault(n => distances.Get(n) < distance);
			return target;
		}

		public override bool Equals(object obj)
		{
			var move = obj as MoveFlee;
			if (move != null)
			{
				return move.Opponent == this.Opponent;
			}
			return false;
		}
		public override int GetHashCode() { return this.Opponent.GetHashCode(); }
		public string DebuggerDisplay { get { return string.Format("Move: Attack {0}", this.Opponent); } }
	}

	[DebuggerDisplay("{DebuggerDisplay}")]
	public class MoveFlee : Move
	{
		public MoveFlee(PlayerType opponent)
		{
			this.Opponent = opponent;
		}
		public PlayerType Opponent { get; protected set; }

		public override Tile GetTarget(Tile source, Map map, State state)
		{
			var oppo = map[state.GetHero(this.Opponent)];
			var distances = MovesGenerator.Instance.GetDistances(map, oppo);

			var distance = distances.Get(source);
			var target = source.Neighbors.FirstOrDefault(n => distances.Get(n) > distance);
			return target;
		}

		public override bool Equals(object obj)
		{
			var move = obj as MoveFlee;
			if (move != null)
			{
				return move.Opponent == this.Opponent;
			}
			return false;
		}
		public override int GetHashCode() { return this.Opponent.GetHashCode(); }
		public string DebuggerDisplay { get { return string.Format("Move: Flee {0}", this.Opponent); } }
	}
}
