using System.Diagnostics;

namespace Vindinium.Ygritte.Decisions
{
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
}
