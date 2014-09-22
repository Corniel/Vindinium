using System;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace Vindinium.Serialization
{
	[DataContract, DebuggerDisplay("{DebugToString()}")]
	public class Hero
	{
		[DataMember]
		public int id;

		[DataMember]
		public string name;

		[DataMember]
		public int elo;

		[DataMember]
		public Pos pos;

		[DataMember]
		public int life;

		[DataMember]
		public int gold;

		[DataMember]
		public int mineCount;

		[DataMember]
		public Pos spawnPos;

		[DataMember]
		public bool crashed;

		/// <summary>Represents the hero as debug string.</summary>
		public string DebugToString()
		{
			return String.Format("Hero[{0}] {1}, Pos: {2}, Health: {3}, Mines: {4}, Gold: {5}, Spawn: {6}{7}",
				id, 
				name,
				pos.DebugToString(),
				life,
				mineCount,
				gold,
				spawnPos.DebugToString(),
				crashed ? ", Crashed" : "");
		}
	}
}
