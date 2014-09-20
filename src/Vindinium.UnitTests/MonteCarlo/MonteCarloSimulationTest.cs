using NUnit.Framework;
using System;

namespace Vindinium.UnitTests.MonteCarlo
{
	[TestFixture]
	public class MonteCarloSimulationTest
	{
#if DEBUG
		public static TimeSpan SimulationTime = TimeSpan.FromMilliseconds(50);
#else
		public static TimeSpan SimulationTime = TimeSpan.FromSeconds(5);
#endif

		/// <summary>Tests the speed of the single monte carlo approach.</summary>
		/// <remarks>
		/// 2,06k/s
		/// </remarks>
		[Test]
		public void GetMoveSingle_Simulation_()
		{
			var state = State.Create(MapTest.Map16);

			var simSingle = new MonteCarloSimulation(17, false);
			var act = simSingle.GetMove(MapTest.Map16, state, SimulationTime);
			Console.WriteLine("{0:#,##0.00}k/s (Single)", simSingle.Simulations / SimulationTime.TotalMilliseconds);

			Assert.AreEqual(MoveDirection.E, act);
		}

		/// <summary>Tests the speed of the parallel monte carlo approach.</summary>
		/// <remarks>
		/// 4,02k/s
		/// </remarks>
		[Test]
		public void GetMoveParallel_Simulation_()
		{
			var state = State.Create(MapTest.Map16);

			var simParallel = new MonteCarloSimulation(17, true);
			var act = simParallel.GetMove(MapTest.Map16, state, SimulationTime);
			Console.WriteLine("{0:#,##0.00}k/s (Parallel)", simParallel.Simulations / SimulationTime.TotalMilliseconds);

			Assert.AreEqual(MoveDirection.E, act);
		}
		[Test]
		public void GetMove_Performance_()
		{
			var state = State.Create(MapTest.Map16);

			var simSingle = new MonteCarloSimulation(17, false);

			var time = TimeSpan.FromSeconds(2);

			var moveSingle = simSingle.GetMove(MapTest.Map16, state, time);
			Console.WriteLine("{0:#,##0.00}k/s (Single)", simSingle.Simulations / time.TotalMilliseconds);
		}
	}
}
