using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Xml.Serialization;

namespace Vindinium.Data
{
	[Serializable, DebuggerDisplay("{DebuggerDisplay}")]
	public class Player : IComparable<Player>
	{
		/// <summary>Gets and set the name of the player.</summary>
		[XmlAttribute("n")]
		public string Name { get; set; }

		/// <summary>The collection of rating points.</summary>
		[XmlElement("r")]
		public List<RatingPoint> RatingPoints { get; set; }

		/// <summary>Gets the current Elo.</summary>
		public int CurrentElo { get { return this.RatingPoints == null || this.RatingPoints.Count == 0 ? 0 : this.RatingPoints[0].Elo; } }

		/// <summary>Gets a weighted Elo.</summary>
		/// <remarks>
		/// The moest resent Elo has the biggest weight.
		/// </remarks>
		public double WeightedElo 
		{ 
			get 
			{
				if (this.RatingPoints == null) { return 0; }

				var total = 0.0;
				double sum = 0.0;
				var weight = 1.0;

				for (int i = 0; i <this.RatingPoints.Count; i++)
				{
					sum += this.RatingPoints[i].Elo * weight;
					total += weight;
					weight *= 0.95;
				}

				var average = sum / total;
				return average;
			} 
		}

		/// <summary>Displays debugger info about the player.</summary>
		public string DebuggerDisplay
		{
			get
			{
				return String.Format("{0}, Elo: {1:0000} (cur: {2:0000}), Games: {3}",
					this.Name, 
					this.RatingPoints == null ? 0 :
					this.WeightedElo,
					this.CurrentElo,
					this.RatingPoints.Count);
			}
		}

		/// <summary>Compares two players based on ther weighted Elo (highest first).</summary>
		public int CompareTo(Player other)
		{
			return other.WeightedElo.CompareTo(this.WeightedElo);
		}
	}
}
