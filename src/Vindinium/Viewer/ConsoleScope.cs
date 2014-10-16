using System;

namespace Vindinium.Viewer
{
	public class ConsoleScope : IDisposable
	{
		public ConsoleScope()
		{
			this.BackgroundColor = Console.BackgroundColor;
			this.ForegroundColor = Console.ForegroundColor;
		}

		public ConsoleColor BackgroundColor { get; protected set; }
		public ConsoleColor ForegroundColor { get; protected set; }

		public void Dispose()
		{
			Console.BackgroundColor = this.BackgroundColor;
			Console.ForegroundColor = this.ForegroundColor;
		}
	}
}
