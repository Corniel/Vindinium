using System.Collections.Generic;

namespace Vindinium.Ygritte2.Decisions
{
	public abstract class NodeComparer : IComparer<Node>
	{
		private static readonly Dictionary<PlayerType, NodeComparer> instances = new Dictionary<PlayerType, NodeComparer>()
		{
			{ PlayerType.Hero1, new ScoreHero1Comparer() },
			{ PlayerType.Hero2, new ScoreHero2Comparer() },
			{ PlayerType.Hero3, new ScoreHero3Comparer() },
			{ PlayerType.Hero4, new ScoreHero4Comparer() },
		};

		public static NodeComparer Get(PlayerType player)
		{
			return instances[player];
		}

		public abstract int Compare(Node l, Node r);
	}

	public class ScoreHero1Comparer : NodeComparer
	{
		public override int Compare(Node l, Node r) { return l.Score.Compare(r.Score, PlayerType.Hero1); }
	}
	public class ScoreHero2Comparer : NodeComparer
	{
		public override int Compare(Node l, Node r) { return l.Score.Compare(r.Score, PlayerType.Hero2); }
	}
	public class ScoreHero3Comparer : NodeComparer
	{
		public override int Compare(Node l, Node r) { return l.Score.Compare(r.Score, PlayerType.Hero3); }
	}
	public class ScoreHero4Comparer : NodeComparer
	{
		public override int Compare(Node l, Node r) { return l.Score.Compare(r.Score, PlayerType.Hero4); }
	}
}
