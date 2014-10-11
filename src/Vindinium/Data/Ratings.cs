using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Vindinium.Data
{
	[Serializable]
	public class Ratings
	{
		[XmlElement("p")]
		public List<Player> Players { get; set; }
	}
}
