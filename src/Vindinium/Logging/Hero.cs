using System;
using System.Diagnostics;
using System.Xml.Serialization;

namespace Vindinium.Logging
{
	[Serializable, DebuggerDisplay("{DebuggerDisplay}")]
	public class Hero
	{
		[XmlAttribute("id")]
		public int Id { get; set; }

		[XmlAttribute("elo")]
		public int Elo { get; set; }

		[XmlAttribute("name")]
		public string Name { get; set; }

		[XmlAttribute("rank")]
		public int Rank { get; set; }

		[XmlAttribute("gold")]
		public int Gold { get; set; }

		[XmlAttribute("crashed")]
		public bool IsCrashed { get; set; }

		[XmlAttribute("message")]
		public string Message { get; set; }

		public string DebuggerDisplay { get { return string.Format("Hero{0}: {1}, '{2}', Rank: {3}{4}", Id, Elo, Name, Rank, IsCrashed ? ", IsCrashed" : ""); } }
	}
}
