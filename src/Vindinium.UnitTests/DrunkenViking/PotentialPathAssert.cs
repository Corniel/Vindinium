using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vindinium.DrunkenViking;

namespace Vindinium.UnitTests.DrunkenViking
{
	public static class PotentialPathAssert
	{
		public static void AreEqual(
			int expTurns, 
			Tile expSource, 
			int expHealth,
			IMineOwnership expMines,
			MoveDirections expDirections,
			int expProfit,
			PotentialPath actual)
		{
			Assert.IsNotNull(actual);
			Assert.AreEqual(expTurns, actual.Turns, "Turns, {0}", actual.DebuggerDisplay);
			Assert.AreEqual(expSource, actual.Source, "Sources, {0}", actual.DebuggerDisplay);
			Assert.AreEqual(expHealth, actual.Health, "Health, {0}", actual.DebuggerDisplay);
			Assert.AreEqual(expMines, actual.Mines, "Mines, {0}", actual.DebuggerDisplay);
			Assert.AreEqual(expDirections, actual.Directions, "Directions, {0}", actual.DebuggerDisplay);
			Assert.AreEqual(expProfit, actual.Profit, "Profit, {0}", actual.DebuggerDisplay);
			//public List<PotentialOpponent> Opponents { get; set; }
		}
	}
}
