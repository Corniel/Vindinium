using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vindinium.Ygritte.Decisions
{
	[DebuggerDisplay("{DebuggerDisplay}")]
	public struct Score
	{
		private ulong m_Value;

		public ulong Hero1Compare { get { return m_Value & ushort.MaxValue; } }
		public ulong Hero2Compare { get { return m_Value & (((ulong)ushort.MaxValue) << 16); } }
		public ulong Hero3Compare { get { return m_Value & (((ulong)ushort.MaxValue) << 32); } }
		public ulong Hero4Compare { get { return m_Value >> 48; } }


		public string DebuggerDisplay
		{
			get
			{
				ulong pt1 = (m_Value >> 00) & ((1 << 14) - 1);
				ulong pt2 = (m_Value >> 16) & ((1 << 14) - 1);
				ulong pt3 = (m_Value >> 32) & ((1 << 14) - 1);
				ulong pt4 = (m_Value >> 48) & ((1 << 14) - 1);

				ulong ps1 = (m_Value >> 14) & 3;
				ulong ps2 = (m_Value >> 30) & 3;
				ulong ps3 = (m_Value >> 46) & 3;
				ulong ps4 = (m_Value >> 62) & 3;

				return String.Format("Score: [1]{0}, {1:#,###0}, [2]{2}, {3:#,###0}, [3]{4}, {5:#,###0}, [4]{6}, {7:#,###0}",
					ps1, pt1,
					ps2, pt2,
					ps3, pt3,
					ps4, pt4);
			}
		}

		public static Score Create(State state)
		{
			unchecked
			{
				var score = new Score();

				var hero1 = state.GetHero(PlayerType.Hero1);
				var hero2 = state.GetHero(PlayerType.Hero2);
				var hero3 = state.GetHero(PlayerType.Hero3);
				var hero4 = state.GetHero(PlayerType.Hero4);

				ulong pt1 = (ulong)(hero1.Gold + hero1.Mines * ((1199 - state.Turn) >> 2));
				ulong pt2 = (ulong)(hero2.Gold + hero2.Mines * ((1198 - state.Turn) >> 2));
				ulong pt3 = (ulong)(hero3.Gold + hero3.Mines * ((1197 - state.Turn) >> 2));
				ulong pt4 = (ulong)(hero4.Gold + hero4.Mines * ((1196 - state.Turn) >> 2));

				pt1 += (ulong)(hero1.Health >> 4);
				pt2 += (ulong)(hero1.Health >> 4);
				pt3 += (ulong)(hero1.Health >> 4);
				pt4 += (ulong)(hero1.Health >> 4);


				ulong ps1 = 0;
				ulong ps2 = 0;
				ulong ps3 = 0;
				ulong ps4 = 0;

				if (pt1 > pt2) { ps1++; } else if (pt1 < pt2) { ps2++; }
				if (pt1 > pt3) { ps1++; } else if (pt1 < pt3) { ps3++; }
				if (pt1 > pt4) { ps1++; } else if (pt1 < pt4) { ps4++; }

				if (pt2 > pt3) { ps2++; } else if (pt2 < pt3) { ps3++; }
				if (pt2 > pt4) { ps2++; } else if (pt2 < pt4) { ps4++; }

				if (pt3 > pt4) { ps3++; } else if (pt3 < pt4) { ps4++; }

				score.m_Value =
					(pt1 << 00) | (ps1 << 14) |
					(pt2 << 16) | (ps2 << 30) |
					(pt3 << 32) | (ps3 << 46) |
					(pt4 << 48) | (ps4 << 62);

				return score;
			}
		}
	}
}
