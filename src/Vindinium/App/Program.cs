using System;
using System.IO;
using System.Net;
using Vindinium.Net;

namespace Vindinium.App
{
	public abstract class Program<T> where T : Program<T>
	{
		protected Program() { }

		/// <summary>The client parameters.</summary>
		public ClientParameters Parameters { get; protected set; }

		/// <summary>The client.</summary>
		public Client Client { get; protected set; }

		public void Run()
		{
			int left = this.Parameters.Runs;
			int run = 1;
			while (left == -1 || left-- > 0)
			{
				Console.WriteLine("Run {0} of {1}", run++, left == -1 ? "oo" : this.Parameters.Runs.ToString());

				this.Client = new Client(this.Parameters);
				this.Client.CreateGame();

				if (this.Client.IsCrashed) { continue; }
				
				using (var writer = new StreamWriter("replay.html", true))
				{
					writer.WriteLine("<a href='{0}'>{0}</a><br />", this.Client.Response.viewUrl);
				}
				
				CreateGame();

				while (!this.Client.IsFinished && !this.Client.IsCrashed)
				{
					try
					{
						Console.Write("{0,4}\r", this.Client.Response.game.turn);
						var move = GetMove();
						this.Client.Move(move);
					}
					catch (WebException x)
					{
						Console.WriteLine(x.Message);
						this.Client.IsCrashed = true;
					}
					catch (Exception x)
					{
						Console.WriteLine(x);
						this.Client.IsCrashed = true;
					}
				}
				Console.WriteLine(this.Client.Response.viewUrl);
			}
		}
		protected virtual void CreateGame() { }
		
		protected abstract MoveDirection GetMove();

		public static void DoMain(string[] args)
		{
			var program = (T)Activator.CreateInstance(typeof(T));
			program.Parameters = ClientParameters.FromConfig();

			program.Run();
		}
	}
}
