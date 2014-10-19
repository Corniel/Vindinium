using NUnit.Framework;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;

namespace Vindinium.UnitTests.Deployment
{
	[TestFixture]
	public class Deployer
	{
		public static readonly Dictionary<string, string> Mappings = new Dictionary<string, string>()
		{
			{ "Drunken Viking 1", "Vindinium.DrunkenViking" },
			{ "Monte Carlo 1000", "Vindinium.MonteCarlo" },
			{ "Njord", "Vindinium.Njord" },
			{ "Slowhand", "Vindinium.Slowhand" },
			{ "Ygritte", "Vindinium.Ygritte" },
		};

#if !DEBUG
		[Test]
#endif
		public void Deploy_All_ToBotsDir()
		{
			var botDir = new DirectoryInfo(ConfigurationManager.AppSettings["bots"]);
			var srcDir = new DirectoryInfo(ConfigurationManager.AppSettings["source"]);

			foreach (var kvp in Mappings)
			{
				var source =  new DirectoryInfo(Path.Combine(srcDir.FullName, kvp.Value, @"bin\Release"));
				var target = new DirectoryInfo(Path.Combine(botDir.FullName, kvp.Key));

				foreach (var file in source.GetFiles().Where(f => f.Extension == ".dll" || f.Extension == ".exe"))
				{
					var location = Path.Combine(target.FullName, file.Name);
					file.CopyTo(location);
				}
			}
		}
	}
}
