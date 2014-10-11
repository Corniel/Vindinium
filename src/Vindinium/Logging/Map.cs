using System;
using System.Xml.Serialization;

namespace Vindinium.Logging
{
	[Serializable]
	public class Map
	{
		[XmlAttribute("size")]
		public int Size { get; set; }

		[XmlAttribute("mines")]
		public int Mines { get; set; }

		[XmlAttribute("tiles")]
		public int FreeTiles { get; set; }

		public string Tiles { get; set; }

		[XmlIgnore]
		public Vindinium.Map Mp
		{
			get { return Vindinium.Map.Parse(this.Tiles); }
			set
			{
				this.Size = value.Height;
				this.Mines = value.Mines.Length;
				this.FreeTiles = value.Count - this.Mines - 4;
				this.Tiles = value.ToString();
			}
		}
	}
}
