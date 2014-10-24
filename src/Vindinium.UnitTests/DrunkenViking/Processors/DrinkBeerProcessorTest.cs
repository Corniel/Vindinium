using System;
using NUnit.Framework;
using Vindinium.DrunkenViking.Processors;
using Vindinium.DrunkenViking;

namespace Vindinium.UnitTests.DrunkenViking.Processors
{
	[TestFixture]
	public class DrinkBeerProcessorTest
	{
		[Test]
		public void Process_Health100BesideATaverne_NoPaths()
		{
			var map = MapTest.Map10Mines8;
			var state = State.Create(0,
				new Hero(100, 1, 0, 0, 0),
				new Hero(100, 8, 0, 0, 0),
				new Hero(100, 8, 9, 0, 0),
				new Hero(100, 1, 9, 0, 0),
				MineOwnership.Create(map));

			var collection = SafePathCollection.Create(map, state);
			var processor = new DrinkBeerProcessor();
			processor.Initialize(map, state);

			var path = PotentialPath.Initial(map[state.Hero1], map, state);
			processor.Process(path, collection);

			var act = collection.GetPotentialPaths();

			Assert.AreEqual(0, act.Count);
		}

		[Test]
		public void Process_Health99BesideATaverne_NoPaths()
		{
			var map = MapTest.Map10Mines8;
			var state = State.Create(0,
				new Hero(100, 1, 0, 0, 0),
				new Hero(100, 8, 0, 0, 0),
				new Hero(100, 8, 9, 0, 0),
				new Hero(100, 1, 9, 0, 0),
				MineOwnership.Create(map));

			var collection = SafePathCollection.Create(map, state);
			var processor = new DrinkBeerProcessor();
			processor.Initialize(map, state);

			var path = PotentialPath.Initial(map[state.Hero1], map, state);
			processor.Process(path, collection);

			var act = collection.GetPotentialPaths();

			Assert.AreEqual(0, act.Count);
		}

		[Test]
		public void Process_Health98BesideATaverne_1Paths()
		{
			var map = MapTest.Map10Mines8;
			var state = State.Create(0,
				new Hero(98, 1, 0, 0, 0),
				new Hero(100, 8, 0, 0, 0),
				new Hero(100, 8, 9, 0, 0),
				new Hero(100, 1, 9, 0, 0),
				MineOwnership.Create(map));
			
			var source = map[state.Hero1];
			var mines = state.Mines;

			var collection = SafePathCollection.Create(map, state);
			var processor = new DrinkBeerProcessor();
			processor.Initialize(map, state);

			var path = PotentialPath.Initial(source, map, state);
			processor.Process(path, collection);

			var act = collection.GetPotentialPaths();

			Assert.AreEqual(1, act.Count);
			PotentialPathAssert.AreEqual(1, source, 99, mines, MoveDirections.E, -2, act[0]);
		}

		[Test]
		public void Process_Health50BesideATaverne_1Paths()
		{
			var map = MapTest.Map10Mines8;
			var state = State.Create(0,
				new Hero(50, 1, 0, 0, 0),
				new Hero(100, 8, 0, 0, 0),
				new Hero(100, 8, 9, 0, 0),
				new Hero(100, 1, 9, 0, 0),
				MineOwnership.Create(map));

			var source = map[state.Hero1];
			var mines = state.Mines;

			var collection = SafePathCollection.Create(map, state);
			var processor = new DrinkBeerProcessor();
			processor.Initialize(map, state);

			var path = PotentialPath.Initial(source, map, state);
			processor.Process(path, collection);

			var act = collection.GetPotentialPaths();

			Assert.AreEqual(1, act.Count);
			PotentialPathAssert.AreEqual(1, source, 99, mines, MoveDirections.E, -2, act[0]);
		}
		[Test]
		public void Process_Health49BesideATaverne_1Paths()
		{
			var map = MapTest.Map10Mines8;
			var state = State.Create(0,
				new Hero(49, 1, 0, 0, 0),
				new Hero(100, 8, 0, 0, 0),
				new Hero(100, 8, 9, 0, 0),
				new Hero(100, 1, 9, 0, 0),
				MineOwnership.Create(map));

			var source = map[state.Hero1];
			var mines = state.Mines;

			var collection = SafePathCollection.Create(map, state);
			var processor = new DrinkBeerProcessor();
			processor.Initialize(map, state);

			var path = PotentialPath.Initial(source, map, state);
			processor.Process(path, collection);

			var act = collection.GetPotentialPaths();

			Assert.AreEqual(1, act.Count);
			PotentialPathAssert.AreEqual(1, source, 98, mines, MoveDirections.E, -2, act[0]);
		}
	}
}
