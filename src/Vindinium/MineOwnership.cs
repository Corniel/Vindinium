using System;
using System.Linq;
using System.Text;

namespace Vindinium
{
	public static class MineOwnership
	{
		public static IMineOwnership Create(Map map)
		{
			if (map.Mines.Length <= 20)
			{
				return MineOwnership20.Empty;
			}
			else
			{
				return MineOwnership64.Empty;
			}
		}

		public static IMineOwnership Create(params int[] mines)
		{
			return Create(mines.Select(mine => (PlayerType)mine).ToArray());
		}

		public static IMineOwnership Create(params PlayerType[] mines)
		{
			if (mines.Length <= 20)
			{
				return MineOwnership20.Create(mines.Select(mine => (PlayerType)mine).ToArray());
			}
			if (mines.Length <= 64)
			{
				return MineOwnership64.Create(mines.Select(mine => (PlayerType)mine).ToArray());
			}
			else
			{
				return MineOwnership128.Create(mines.Select(mine => (PlayerType)mine).ToArray());
			}
		}

		public static IMineOwnership Parse(string val)
		{
			if (string.IsNullOrEmpty(val)) { throw new ArgumentNullException("val"); }

			var mines = new int[val.Length];

			for (var i = 0; i < val.Length; i++)
			{
				switch (val[i])
				{
					case '1': mines[i] = 1; break;
					case '2': mines[i] = 2; break;
					case '3': mines[i] = 3; break;
					case '4': mines[i] = 4; break;
					default:
					case '.': mines[i] = 0; break;

				}
			}
			return Create(mines);
		}

		public static IMineOwnership UpdateFromTiles(IMineOwnership mines, string tiles)
		{
			var index = 0;

			for (int p = 0; p < tiles.Length; p += 2)
			{
				if (tiles[p] == '$')
				{
					switch (tiles[p + 1])
					{
						case '1': mines = mines.Set(index, PlayerType.Hero1); break;
						case '2': mines = mines.Set(index, PlayerType.Hero2); break;
						case '3': mines = mines.Set(index, PlayerType.Hero3); break;
						case '4': mines = mines.Set(index, PlayerType.Hero4); break;
						case '-':
						default: break;
					}
					index++;
				}
			}
			return mines;
		}

		public static string ToString(IMineOwnership ownership, int length)
		{
			var sb = new StringBuilder();

			for (int index = 0; index < length; index++)
			{
				switch (ownership[index])
				{
					default:
					case PlayerType.None: sb.Append('.'); break;
					case PlayerType.Hero1: sb.Append('1'); break;
					case PlayerType.Hero2: sb.Append('2'); break;
					case PlayerType.Hero3: sb.Append('3'); break;
					case PlayerType.Hero4: sb.Append('4'); break;
				}
			}
			return sb.ToString();
		}
	}
}
