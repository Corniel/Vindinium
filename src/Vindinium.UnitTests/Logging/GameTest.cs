using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vindinium.App;
using Vindinium.Logging;
using Vindinium.Random;

namespace Vindinium.UnitTests.Logging
{
	[TestFixture]
	public class GameTest
	{
		[Test, Ignore]
		public void Create_()
		{
			var hero1 = new RandomBot();
			var hero2 = new RandomBot();
			var hero3 = new Vindinium.MonteCarlo.MonteCarlo();
			var hero4 = new RandomBot();

			var map = MapTest.Map10;
			var state = State.Create(map);

			var game = new Game()
			{
				Heros = new Vindinium.Logging.Hero[]
				{
					new Vindinium.Logging.Hero(){ Elo = 1200, Name = "Random 1"	},
					new Vindinium.Logging.Hero(){ Elo = 1250, Name = "Random 2"	},
					new Vindinium.Logging.Hero(){ Elo = 1683, Name = "Monte Carlo 1000"	},
					new Vindinium.Logging.Hero(){ Elo = 1100, Name = "Ad's bot is better"	},
				},
				Id = "Unknown",
				Map = new Vindinium.Logging.Map(){ Mp = map },
				Turns = new List<Turn>(),
			};

			hero1.Set(map, state, PlayerType.Hero1);
			hero2.Set(map, state, PlayerType.Hero2);
			hero3.Set(map, state, PlayerType.Hero3);
			hero4.Set(map, state, PlayerType.Hero4);
			
			for (int i = 0; i < 1200; i++)
			{
				MoveDirection d = MoveDirection.x;

				hero1.Set(map, state, PlayerType.Hero1);
				hero2.Set(map, state, PlayerType.Hero2);
				hero3.Set(map, state, PlayerType.Hero3);
				hero4.Set(map, state, PlayerType.Hero4);

				switch (i & 3)
				{
					case 0: d = hero1.GetMove(); break;
					case 1: d = hero2.GetMove(); break;
					case 2: d = hero3.GetMove(); break;
					case 3: d = hero4.GetMove(); break;
				}

				state = state.Move(map, d, (PlayerType)(1 + (i & 3)));
				game.Turns.Add(Turn.Create(state));
			}
			game.Save(@"d:\temp\test.xml");
		}
	}

	public static class BotStub
	{
		


	}
}
