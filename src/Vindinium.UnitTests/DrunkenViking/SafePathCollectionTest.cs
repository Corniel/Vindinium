using NUnit.Framework;
using System;
using Vindinium.DrunkenViking;

namespace Vindinium.UnitTests.DrunkenViking
{
	[TestFixture]
	public class SafePathCollectionTest
	{
		/// <summary>Get 1 mine and walk to a Tavern.</summary>
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

		/// <summary>Do a run a long 4 mines and hit the Tavern.</summary>
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


		/// <summary>Do a run a long 4 mines and hit the Tavern.</summary>
		/// <remarks>
		/// ##@2[]########[]  ##
		///     ..########      
		/// $-  @3  ####      $2
		/// ####            ####
		/// ######  $-$-  ######
		/// ######  $-$-  ######
		/// ####            ####
		/// $2      ####      $-
		///     ..########..  @4  
		/// ##  []########[]@1##
		/// </remarks>
		[Test]
		public void Process_Get1MineAfterDrinkingTwice_4Mines15Turns42Profit()
		{
			var map = MapTest.Map10Mines8;
			var state = State.Create(1,
				new Hero(99, 8, 9, 0, 2),
				new Hero(01, 1, 0, 0, 0),
				new Hero(50, 3, 2, 0, 0),
				new Hero(99, 8, 8, 0, 0),
				MineOwnership.Parse(".22......"));

			var collection = SafePathCollection.Create(map, state);
			collection.Procces();
			foreach (var item in collection)
			{
				Console.WriteLine(item.DebuggerDisplay);
			}

			var act = collection.BestPath;
			var exp = new SafePath(3, 7, 13, MoveDirection.E);

			Assert.AreEqual(exp, act);
		}
	}
}
