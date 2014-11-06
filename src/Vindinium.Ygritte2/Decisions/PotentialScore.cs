using System;
using System.Globalization;
using Vindinium.Decisions;

namespace Vindinium.Ygritte2.Decisions
{
	public struct PotentialScore : IScore
	{
		public static readonly ScoreCollection EmptyCollection = new ScoreCollection(Empty, Empty, Empty, Empty);

			public static readonly PotentialScore Empty = default(PotentialScore);

		public PotentialScore(int score)
		{
			m_Value = (UInt32)score;
		}
		private UInt32 m_Value;

		public uint ToUInt32() { return m_Value; }

		public string DebuggerDisplay { get { return String.Format(CultureInfo.InvariantCulture, "{0:#,##0.00}", m_Value / 1000.0m); } }

		public static ScoreCollection CreateCollection(int s1, int s2, int s3, int s4)
		{
			return new ScoreCollection(
				new PotentialScore(s1),
				new PotentialScore(s2),
				new PotentialScore(s3),
				new PotentialScore(s4));
		}
	}
}
