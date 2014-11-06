using System;

namespace Vindinium
{
	public interface IMineOwnership : IEquatable<IMineOwnership>
	{
		PlayerType this[int index]{get;}

		int Count(PlayerType player);
		int Count(PlayerType player, int length);

		IMineOwnership ChangeOwnership(PlayerType curOwner, PlayerType newOwner);
		IMineOwnership ChangeOwnership(PlayerType curOwner, PlayerType newOwner, int mineCount);
		IMineOwnership Set(int index, PlayerType owner);
		IMineOwnership UpdateFromTiles(string tiles);

		string ToString(int length);
	}
}
