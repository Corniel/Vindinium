using System;
using System.Configuration;
using System.Diagnostics;
using Vindinium.App;
using Vindinium.Viewer;

namespace Vindinium.SlowHand
{
	public class SlowHandProgram: Program<SlowHandProgram>
	{
		public static void Main(string[] args) {
			Console.SetWindowSize(80, 40);
			SlowHandProgram.DoMain(args); 
		}

		public SlowHandProgram()
		{
			this.Timout = TimeSpan.FromMilliseconds(int.Parse(ConfigurationManager.AppSettings["maxtime"]));
		}

		public TimeSpan Timout { get; protected set; }

		public MoveDirection Move { get; protected set; }

		
		public override Move GetMove()
		{
			var sw = new Stopwatch();
			sw.Start();
			UpdateState();

			Render();


			ConsoleKeyInfo key = new ConsoleKeyInfo();

			while (sw.Elapsed < this.Timout)
			{
				
				if (Console.KeyAvailable == true)
				{
					key = Console.ReadKey();
					break;
				}
				else
				{
					Console.Write("\r{0} {1:##0.0}", this.Move, (this.Timout - sw.Elapsed).TotalMilliseconds);
					System.Threading.Thread.Sleep(170);
				}
				
			}
			switch (key.Key)
			{
				case ConsoleKey.LeftArrow: this.Move = MoveDirection.W;  break;
				case ConsoleKey.RightArrow: this.Move = MoveDirection.E; break;
				case ConsoleKey.UpArrow: this.Move = MoveDirection.N; break;
				case ConsoleKey.DownArrow: this.Move = MoveDirection.S; break;
				case ConsoleKey.X: this.Move = MoveDirection.x; break;
				default: break;
			}
			try
			{
				this.State = this.State.Move(this.Map, this.Move, this.Player);
			}
			catch { }

			Console.Clear();
			Render();
			return new Move(this.Move);
		}

		private void Render()
		{
			var viewer = new GameViewer();
			Console.Clear();

			Console.WriteLine("Turn: {0:#,##0}", this.State.Turn);
			foreach (var p in PlayerTypes.All)
			{
				var hero = this.State.GetHero(p);

				Console.ForegroundColor = (p == this.Player) ? ConsoleColor.White : ConsoleColor.Gray;
				Console.WriteLine("{0} Health: {1,3}, Mines: {2,2}, Gold: {3,4}, Exp: {4}",
					p,
					hero.Health, 
					hero.Mines,
					hero.Gold,
					hero.Gold + hero.Mines * ((1195 + (int)p - this.State.Turn) >> 2)
				);
			}
			viewer.Render(this.Map, this.State);
		}
	}
}
