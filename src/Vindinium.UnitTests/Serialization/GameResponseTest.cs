using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.IO;
using Vindinium.Serialization;

namespace Vindinium.UnitTests.Serialization
{
	[TestClass]
	public class GameResponseTest
	{
		[TestMethod]
		public void ToJson_Object_AreEqual()
		{
			var response = new GameResponse()
			{
				game = new Game()
				{
					board = new Board()
					{
						size = 1,
						tiles = "##"
					},
					id = "id1",
					maxTurns = 1200,
					turn = 1,
					heroes = new List<Vindinium.Serialization.Hero>()
					{
						new Vindinium.Serialization.Hero()
						{
							name = "HERO 4 ever"
						},
					}
				},
				token = "ABCDEF1234"
			};

			var act = response.ToJson();
			var exp = "{\"game\":{\"board\":{\"size\":1,\"tiles\":\"##\"},\"finished\":false,\"heroes\":[{\"crashed\":false,\"elo\":0,\"gold\":0,\"id\":0,\"life\":0,\"mineCount\":0,\"name\":\"HERO 4 ever\",\"pos\":null,\"spawnPos\":null}],\"id\":\"id1\",\"maxTurns\":1200,\"turn\":1},\"hero\":null,\"playUrl\":null,\"token\":\"ABCDEF1234\",\"viewUrl\":null}";
			Assert.AreEqual(exp, act);

		}

		[TestMethod]
		public void FromJson_GameResponseString_AreEqual()
		{
			var json = "";

			using (var stream = GetType().Assembly.GetManifestResourceStream("Vindinium.UnitTests.Serialization.GameResponse.json"))
			{
				using (var reader = new StreamReader(stream))
				{
					json = reader.ReadToEnd();
				}
			}

			var act = GameResponse.FromJson(json);

			Assert.AreEqual("##############        ############################        ##############################    ##############################$4    $4############################  @4    ########################  @1##    ##    ####################  []        []  ##################        ####        ####################  $4####$4  ########################  $4####$4  ####################        ####        ##################  []        []  ####################  @2##    ##@3  ########################        ############################$-    $-##############################    ##############################        ############################        ##############", act.game.board.tiles);
			
		}
	}
}
