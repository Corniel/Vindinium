using System;
using System.Globalization;
using Vindinium.Decisions;

namespace Vindinium.MonteCarlo.Decisions
{
	public struct AverageScore : IScore
	{
		private UInt32 m_Value;

		public AverageScore(long average, int runs)
		{
			var score = (average * 100) / runs;
			m_Value = (uint)score;
		}

		public UInt32 ToUInt32() { return m_Value; }

		public decimal Score { get { return m_Value / 100m; } }

		public string DebuggerDisplay { get { return string.Format(CultureInfo.InvariantCulture, "{0:#,##0.00}", Score); } }
	}
}
