using System;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace Vindinium.Serialization
{
	[DataContract, DebuggerDisplay("{DebugToString()}")]
	public class Pos
	{
		[DataMember]
		public int x;

		[DataMember]
		public int y;

		/// <summary>Represents the position as debug string.</summary>
		public string DebugToString()
		{
			return String.Format("({0},{1})", x, y);
		}
	}
}
