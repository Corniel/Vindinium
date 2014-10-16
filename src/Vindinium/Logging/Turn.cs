using System;
using System.Xml.Serialization;

namespace Vindinium.Logging
{
	[Serializable]
	public class Turn
	{
		[XmlAttribute("turn")]
		public int Id { get; set; }

		[XmlAttribute("mines")]
		public string Mines { get; set; }

		[XmlAttribute("move")]
		public string Move { get; set; }

		[XmlIgnore]
		public IMineOwnership MineOwnership
		{
			get { return Vindinium.MineOwnership.Parse(this.Mines); }
			set { this.Mines = value.ToString(); }
		}

		[XmlAttribute("h1")]
		public string Hero1String { get; set; }

		[XmlIgnore]
		public Vindinium.Hero Hero1
		{
			get { return Vindinium.Hero.Parse(this.Hero1String); }
			set { this.Hero1String = value.ToString(); }
		}

		[XmlAttribute("h2")]
		public string Hero2String { get; set; }

		[XmlIgnore]
		public Vindinium.Hero Hero2
		{
			get { return Vindinium.Hero.Parse(this.Hero2String); }
			set { this.Hero2String = value.ToString(); }
		}
		[XmlAttribute("h3")]
		public string Hero3String { get; set; }

		[XmlIgnore]
		public Vindinium.Hero Hero3
		{
			get { return Vindinium.Hero.Parse(this.Hero3String); }
			set { this.Hero3String = value.ToString(); }
		}
		[XmlAttribute("h4")]
		public string Hero4String { get; set; }

		[XmlIgnore]
		public Vindinium.Hero Hero4
		{
			get { return Vindinium.Hero.Parse(this.Hero4String); }
			set { this.Hero4String = value.ToString(); }
		}

		public static Turn Create(State state)
		{
			var turn = new Turn()
			{
				Id = state.Turn,
				Hero1 = state.Hero1,
				Hero2 = state.Hero2,
				Hero3 = state.Hero3,
				Hero4 = state.Hero4,
				MineOwnership = state.Mines
			};
			return turn;
		}

		/// <summary>Gets the state of the turn.</summary>
		public State State
		{
			get
			{
				return State.Create(Id, Hero1, Hero2, Hero3, Hero4, MineOwnership);
			}
		}
	}
}
