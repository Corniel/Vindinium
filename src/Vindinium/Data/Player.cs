using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Xml.Serialization;

namespace Vindinium.Data
{
	[Serializable, DebuggerDisplay("{DebuggerDisplay}")]
	public class Player
	{
		[XmlAttribute("n")]
		public string Name { get; set; }

		[XmlElement("r")]
		public List<RatingPoint> RatingPoints { get; set; }

		public string DebuggerDisplay { get { return String.Format("{0}, Elo: {1:0000}", this.Name, this.RatingPoints == null ? 0 : this.RatingPoints.Last().Elo); } }
	}
}
