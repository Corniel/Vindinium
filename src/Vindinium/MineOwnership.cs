using System.Linq;

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
			else
			{
				return MineOwnership64.Create(mines.Select(mine => (PlayerType)mine).ToArray());
			}
		}
	}
}
