using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Vindinium.Serialization
{
	[DataContract]
	public class Game
	{
		[DataMember]
		public string id;

		[DataMember]
		public int turn;

		[DataMember]
		public int maxTurns;

		[DataMember]
		public List<Hero> heroes;

		[DataMember]
		public Board board;

		[DataMember]
		public bool finished;
	}
}
