using System;
using System.Diagnostics;
using System.Globalization;
using System.Xml.Serialization;

namespace Vindinium.Data
{
	[Serializable, DebuggerDisplay("{DebuggerDisplay}")]
	public class RatingPoint
	{
		private const string DatePattern = "yyyy-MM-dd hh:mm";
		[XmlAttribute("e")]
		public int Elo { get; set; }

		[XmlAttribute("d")]
		public string Dt
		{
			get { return this.Date.ToString(DatePattern, CultureInfo.InvariantCulture); }
			set { this.Date = DateTime.ParseExact(value, DatePattern, CultureInfo.InvariantCulture); }
		}

		[XmlIgnore]
		public DateTime Date { get; set; }

		public string DebuggerDisplay { get { return String.Format("{0:0000}, {1}", this.Elo, this.Dt); } }
	}
}
