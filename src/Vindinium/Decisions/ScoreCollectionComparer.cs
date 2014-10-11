using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vindinium.Decisions
{
	public abstract class ScoreCollectionComparer : IComparer<ScoreCollection>
	{
		public static ScoreCollectionComparer Get(PlayerType player)
		{
			return dict[player];
		}

		private static readonly Dictionary<PlayerType, ScoreCollectionComparer> dict = new Dictionary<PlayerType, ScoreCollectionComparer>()
		{
			{ PlayerType.Hero1, new Score1CollectionComparer() },
			{ PlayerType.Hero2, new Score2CollectionComparer() },
			{ PlayerType.Hero3, new Score3CollectionComparer() },
			{ PlayerType.Hero4, new Score4CollectionComparer() },
		};

		public abstract int Compare(ScoreCollection x, ScoreCollection y);
	}
	public class Score1CollectionComparer : ScoreCollectionComparer
	{
		public override int Compare(ScoreCollection x, ScoreCollection y) { return x.Compare(y, PlayerType.Hero1); }
	}
	public class Score2CollectionComparer : ScoreCollectionComparer
	{
		public override int Compare(ScoreCollection x, ScoreCollection y) { return x.Compare(y, PlayerType.Hero2); }
	}
	public class Score3CollectionComparer : ScoreCollectionComparer
	{
		public override int Compare(ScoreCollection x, ScoreCollection y) { return x.Compare(y, PlayerType.Hero3); }
	}
	public class Score4CollectionComparer : ScoreCollectionComparer
	{
		public override int Compare(ScoreCollection x, ScoreCollection y) { return x.Compare(y, PlayerType.Hero4); }
	}
}
