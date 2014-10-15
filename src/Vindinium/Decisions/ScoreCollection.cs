using System;
using System.Diagnostics;
using System.Globalization;

namespace Vindinium.Decisions
{
	[DebuggerDisplay("{DebuggerDisplay}")]
	public struct ScoreCollection
	{
		public ScoreCollection(IScore sc1, IScore sc2, IScore sc3, IScore sc4)
		{
			score1 = sc1;
			score2 = sc2;
			score3 = sc3;
			score4 = sc4;
		}

		private IScore score1;
		private IScore score2;
		private IScore score3;
		private IScore score4;

		public IScore Get(PlayerType player)
		{
			switch (player)
			{
				case PlayerType.Hero1: return score1;
				case PlayerType.Hero2: return score2;
				case PlayerType.Hero3: return score3;
				case PlayerType.Hero4: return score4;
				case PlayerType.None:
				default:
					throw new NotSupportedException(string.Format("PlayerType '{0}' is not supported.", player));
			}
		}

		public int Compare(ScoreCollection other, PlayerType player)
		{
			switch (player)
			{
				case PlayerType.Hero1: return other.score1.ToUInt32().CompareTo(score1.ToUInt32());
				case PlayerType.Hero2: return other.score2.ToUInt32().CompareTo(score2.ToUInt32());
				case PlayerType.Hero3: return other.score3.ToUInt32().CompareTo(score3.ToUInt32());
				case PlayerType.Hero4: return other.score4.ToUInt32().CompareTo(score4.ToUInt32());
			case PlayerType.None:
			default:
				throw new NotSupportedException(string.Format("PlayerType '{0}' is not supported.", player));
			}
		}

		/// <summary>If any of the alpha's of the opponents is worse then the test score: break.
		/// 
		/// Otherwise update if a better alpha is found for the player.
		/// </summary>
		public bool ContinueProccesingAlphas(ScoreCollection test, PlayerType player, out ScoreCollection alphasOut)
		{
			foreach (var other in PlayerTypes.Other[player])
			{
				if (Compare(test, other) > 0)
				{
					alphasOut = this;
					return false;
				}
			}

			IScore s1 = score1;
			IScore s2 = score2;
			IScore s3 = score3;
			IScore s4 = score4;

			switch (player)
			{
				case PlayerType.Hero1: if (Compare(test, player) > 0) { s1 = test.score1; } break;
				case PlayerType.Hero2: if (Compare(test, player) > 0) { s2 = test.score2; } break;
				case PlayerType.Hero3: if (Compare(test, player) > 0) { s3 = test.score3; } break;
				case PlayerType.Hero4: if (Compare(test, player) > 0) { s4 = test.score4; } break;
			}
			alphasOut = new ScoreCollection(s1, s2, s3, s4);
			return true;
		}

		public string ToConsoleDisplay(PlayerType player)
		{
			var str = String.Format(
					CultureInfo.InvariantCulture,
					"h1: {0}, h2: {1}, h3: {2}, h4: {3}",
					score1.DebuggerDisplay,
					score2.DebuggerDisplay,
					score3.DebuggerDisplay,
					score4.DebuggerDisplay);

			str = ToConsoleDisplay(str, player);
			return str;
		}
		public string DebuggerDisplay { get { return ToConsoleDisplay(PlayerType.None); } }

		public static string ToConsoleDisplay(string str, PlayerType player)
		{
			var search = String.Format("h{0}: ", (int)player);
			var replace = String.Format("h{0}*: ", (int)player);

			str = str.Replace(search, replace);
			return str;
		}

		public UInt32 Hero1 { get { return score1.ToUInt32(); } }
		public UInt32 Hero2 { get { return score2.ToUInt32(); } }
		public UInt32 Hero3 { get { return score3.ToUInt32(); } }
		public UInt32 Hero4 { get { return score4.ToUInt32(); } }

		public static void Sort(long s1, long s2, long s3, long s4, out int p1, out int p2, out int p3, out int p4)
		{
			p1 = 0;
			p2 = 0;
			p3 = 0;
			p4 = 0;

			if (s1 > s2) { p1++; } else if (s1 < s2) { p2++; }
			if (s1 > s3) { p1++; } else if (s1 < s3) { p3++; }
			if (s1 > s4) { p1++; } else if (s1 < s4) { p4++; }

			if (s2 > s3) { p2++; } else if (s2 < s3) { p3++; }
			if (s2 > s4) { p2++; } else if (s2 < s4) { p4++; }

			if (s3 > s4) { p3++; } else if (s3 < s4) { p4++; }
		}
	}
}
