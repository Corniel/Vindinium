using System;
using System.Diagnostics;
using System.Globalization;
using System.Xml.Serialization;

namespace Vindinium.Data
{
	[Serializable, DebuggerDisplay("{DebuggerDisplay}")]
	public class RatingPoint : IComparable<RatingPoint>, IEquatable<RatingPoint>
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

		/// <summary>Displays debugger info about the raiting point.</summary>
		public string DebuggerDisplay { get { return String.Format("{0:0000}, {1}", this.Elo, this.Dt); } }

		public int CompareTo(RatingPoint other)
		{
			return other.Date.CompareTo(this.Date);
		}

		public bool Equals(RatingPoint other)
		{
			var datedif = Math.Abs((this.Date - other.Date).TotalMinutes);

			return
				this.Elo.Equals(other.Elo) && datedif < 2.0;
		}
	}
}
