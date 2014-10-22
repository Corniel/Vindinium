using NUnit.Framework;
using System;
using Vindinium.DrunkenViking.Strategies;

namespace Vindinium.UnitTests.DrunkenViking.Stragties
{
	[TestFixture]
	public class SafePathCollectionTest
	{
		/// <summary>Get 1 mine and walk to a taverne.</summary>
		/// <remarks>
		///  1 2 3########    @2 
		/// $-  @1[]####[]..  $-
		///     ############    
		///                     
		/// ##  ##  ####  ##  ##
		/// ##  ##  ####  ##  ##
		///                     
		///     ############    
		/// $-  ..[]####[]..  $-
		/// @4    ########    @3
		/// </remarks>
		[Test]
		public void Process_Get1Mine1Mine4Turns4Profit()
		{
			var map = MapTest.Map10;
			var state = State.Create(0,
				new Hero(23, 0, 0, 0, 0),
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
			var exp = new SafePath(1, 4, 4, MoveDirection.S);

			Assert.AreEqual(exp, act);
		}

		/// <summary>Do a run a long 4 mines and hit the taverne.</summary>
		/// <remarks>
		/// ##  []########[]@2##
		///     ..########15    
		/// $-      ####1314  $-
		/// ####        12  ####
		/// ######0 $-$-11######
		/// ######1 $-$- 9######
		/// ####  3 4 6  8  ####
		/// $-      ####      $-
		///     ..########..    
		/// ##@4[]########[]@3##
		/// </remarks>
		[Test]
		public void Process_Get4Mines_4Mines15Turns42Profit()
		{
			var map = MapTest.Map10Mines8;
			var state = State.Create(0,
				new Hero(100, 3, 4, 0, 0),
				new Hero(100, 8, 0, 0, 0, true),
				new Hero(100, 8, 9, 0, 0, true),
				new Hero(100, 1, 9, 0, 0, true),
				MineOwnership.Create(map));

			var collection = SafePathCollection.Create(map, state);
			collection.Procces();
			foreach (var item in collection)
			{
				Console.WriteLine(item.DebuggerDisplay);
			}

			var act = collection.BestPath;
			var exp = new SafePath(4, 15, 42, MoveDirection.E);

			Assert.AreEqual(exp, act);
		}
	}
}
