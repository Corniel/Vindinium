using System;
using Vindinium.App;

namespace Vindinium.MonteCarlo
{
	public class MonteCarlo : Program<MonteCarlo> 
	{
		public static void Main(string[] args)
		{
			MonteCarlo.DoMain(args);
		}

		protected override MoveDirection GetMove()
		{
			throw new NotImplementedException();
		}
	}
}
