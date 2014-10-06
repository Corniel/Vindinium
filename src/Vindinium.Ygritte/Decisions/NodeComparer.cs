using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vindinium.Ygritte.Decisions
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
		public override int Compare(Node l, Node r)
		{
			return r.Score.Hero1Compare.CompareTo(l.Score.Hero1Compare);
		}
	}
	public class ScoreHero2Comparer : NodeComparer
	{
		public override int Compare(Node l, Node r)
		{
			return r.Score.Hero2Compare.CompareTo(l.Score.Hero2Compare);
		}
	}
	public class ScoreHero3Comparer : NodeComparer
	{
		public override int Compare(Node l, Node r)
		{
			return r.Score.Hero3Compare.CompareTo(l.Score.Hero3Compare);
		}
	}
	public class ScoreHero4Comparer : NodeComparer
	{
		public override int Compare(Node l, Node r)
		{
			return r.Score.Hero4Compare.CompareTo(l.Score.Hero4Compare);
		}
	}
}
