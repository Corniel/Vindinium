using NUnit.Framework;
using System;
using System.Linq;
using Vindinium.DrunkenViking.Strategies;

namespace Vindinium.UnitTests.DrunkenViking.Stragties
{
	[TestFixture]
	public class SafePathCollectionTest
	{
		[Test]
		public void Process_Get1Mine1Mine4Turns2Profit()
		{
			var map = MapTest.Map10;
			var state = State.Create(0,
				new Hero(23, 2, 0, 0, 0),
				new Hero(99, 9, 0, 0, 0),
				new Hero(99, 9, 9, 0, 0),
				new Hero(99, 0, 9, 0, 0),
				MineOwnership.Create(map));

			var collection = SafePathCollection.Create(map, state);
			collection.Procces();
			foreach (var item in collection)
			{
				Console.WriteLine(item.DebuggerDisplay);
			}

			var act = collection.BestPath;
			var exp = new SafePath(1, 4, 2, MoveDirection.W, MoveDirection.W);

			Assert.AreEqual(exp, act);
		}

		/// <summary>Do a run a long 4 mines and hit the taverne.</summary>
		/// <remarks>
		/// ##  []########[]@2##
		///     ..########16    
		/// $-      ####1415  $-
		/// ####        13  ####
		/// ######0 $-$-12######
		/// ######2 $-$-10######
		/// ####  4 5 7  9  ####
		/// $-      ####      $-
		///     ..########..    
		/// ##@4[]########[]@3##
		/// </remarks>
		[Test]
		public void Process_Get4Mines_4Mines15Turns30Profit()
		{
			var map = MapTest.Map10Mines8;
			var state = State.Create(0,
				new Hero(99, 3, 4, 0, 0),
				new Hero(99, 8, 0, 0, 0, true),
				new Hero(99, 8, 9, 0, 0, true),
				new Hero(99, 1, 9, 0, 0, true),
				MineOwnership.Create(map));

			var collection = SafePathCollection.Create(map, state);
			collection.Procces();
			foreach (var item in collection)
			{
				Console.WriteLine(item.DebuggerDisplay);
			}

			var act = collection.BestPath;
			var exp = new SafePath(4, 15, 30, MoveDirection.W);

			Assert.AreEqual(exp, act);
		}
	}
}
