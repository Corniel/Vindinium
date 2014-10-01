using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vindinium.DrunkenViking;

namespace Vindinium.UnitTests.DrunkenViking
{
	[TestFixture]
	public class NodeStateTest
	{
		[Test]
		public void Test()
		{
			var map = MapTest.Map10;
			var state = State.Create(map);

			var node = NodeState.Create(map, state, PlayerType.Hero1);
			var result = node.Process();

			Console.WriteLine(node.ToUnitTestString());
		}
	}
}
