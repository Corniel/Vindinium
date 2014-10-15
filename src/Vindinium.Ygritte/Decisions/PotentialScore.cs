using System;
using Vindinium.Decisions;

namespace Vindinium.Ygritte.Decisions
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

		public string DebuggerDisplay { get { return string.Format("{0:#,##0.00}", m_Value / 1000.0m); } }
	}
}
