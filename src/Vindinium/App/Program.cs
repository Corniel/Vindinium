using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
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
			int runs = this.Parameters.Runs;

			while (runs == -1 || runs-- > 0)
			{
				this.Client = new Client(this.Parameters);
				this.Client.CreateGame();
				CreateGame();

				while (!this.Client.IsFinished)
				{
					var move = GetMove();
					this.Client.Move(move);
				}
			}
		}
		protected virtual void CreateGame()
		{
			//opens up a webpage so you can view the game, doing it async so we dont time out
			new Thread(delegate()
			{
				Process.Start(this.Client.Response.viewUrl);
			}).Start();
		}
		
		protected abstract MoveDirection GetMove();

		public static void DoMain(string[] args)
		{
			var program = (T)Activator.CreateInstance(typeof(T));
			program.Parameters = ClientParameters.FromConfig();

			program.Run();
		}
	}
}
