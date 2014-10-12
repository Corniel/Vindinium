using System.Diagnostics;
using System.Linq;

namespace Vindinium.Ygritte.Decisions
{
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
