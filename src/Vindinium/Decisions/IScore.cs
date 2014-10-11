using System;

namespace Vindinium.Decisions
{
	public interface IScore
	{
		UInt32 ToUInt32();
		string DebuggerDisplay { get; }
	}
}
