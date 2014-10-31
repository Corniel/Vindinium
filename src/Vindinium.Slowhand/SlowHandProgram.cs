using System;
using System.Configuration;
using System.Diagnostics;
using System.Threading;
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

			ThreadPool.QueueUserWorkItem((waitpoint) =>
			{
				while (true)
				{
					var key = Console.ReadKey();
					switch (key.Key)
					{
						case ConsoleKey.LeftArrow: this.Move = MoveDirection.W; this.ReceivedMove = false; break;
						case ConsoleKey.RightArrow: this.Move = MoveDirection.E; this.ReceivedMove = false; break;
						case ConsoleKey.UpArrow: this.Move = MoveDirection.N; this.ReceivedMove = false; break;
						case ConsoleKey.DownArrow: this.Move = MoveDirection.S; this.ReceivedMove = false; break;
						case ConsoleKey.X: this.Move = MoveDirection.x; this.ReceivedMove = false; break;
						default: break;
					}
				}
			});
		}

		public TimeSpan Timout { get; protected set; }

		public MoveDirection Move { get; protected set; }
		public bool ReceivedMove { get; protected set; }

		
		public override Move GetMove()
		{
			var sw = new Stopwatch();
			sw.Start();
			UpdateState();

			Render();

			while (!this.ReceivedMove && sw.Elapsed < this.Timout)
			{
				Console.Write("\r{0} {1:0.0}", this.Move, (this.Timout - sw.Elapsed).TotalMilliseconds/1000.0);
				System.Threading.Thread.Sleep(17);
			}
			this.ReceivedMove = false;
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
