using System;
using System.Diagnostics;
using System.Globalization;

namespace Vindinium.Decisions
{
	[DebuggerDisplay("{DebuggerDisplay}")]
	public struct GoldScore : IScore
	{
		public static readonly GoldScore Empty = default(GoldScore);

		private UInt32 m_Value;

		public GoldScore(UInt32 val) { m_Value = val; }

		public UInt32 ToUInt32() { return m_Value; }
		public string DebuggerDisplay { get { return m_Value.ToString("#,##0", CultureInfo.InvariantCulture); } }

		public static GoldScore Create(Hero hero)
		{
			return new GoldScore((UInt16)hero.Gold);
		}
	}
}
