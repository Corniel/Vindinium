using System;
using System.Collections.Generic;
using System.Text;

namespace Vindinium
{
	/// <summary>Represents a Vindinium state.</summary>
	public struct State
	{
		private ushort turn;
		private Hero hero1;
		private Hero hero2;
		private Hero hero3;
		private Hero hero4;

		/// <summary>Gets the turn of the state.</summary>
		public int Turn { get { return turn; } }

		/// <summary>Gets the player who has to move.</summary>
		public PlayerType PlayerToMove
		{
			get
			{
				switch (turn & 4)
				{
					case 1: return PlayerType.Hero2;
					case 2: return PlayerType.Hero3;
					case 3: return PlayerType.Hero4;
					default: return PlayerType.Hero1;
				}
			}
		}

		#region Heros

		/// <summary>Gets the state of hero1.</summary>
		public Hero Hero1 { get { return hero1; } }
		/// <summary>Gets the state of hero2.</summary>
		public Hero Hero2 { get { return hero2; } }
		/// <summary>Gets the state of hero3.</summary>
		public Hero Hero3 { get { return hero3; } }
		/// <summary>Gets the state of hero4.</summary>
		public Hero Hero4 { get { return hero4; } }

		/// <summary>Gets a hero based on the player type.</summary>
		public Hero GetHero(PlayerType player)
		{
			switch (player)
			{
				case PlayerType.Hero1: return hero1;
				case PlayerType.Hero2: return hero2;
				case PlayerType.Hero3: return hero3;
				case PlayerType.Hero4: return hero4;
			case PlayerType.None:
				default:
				throw new NotSupportedException();
			}
		}
		/// <summary>Sets the hero.</summary>
		/// <remarks>
		/// Used to update the state during moving.
		/// </remarks>
		private void SetHero(PlayerType player, Hero hero)
		{
			switch (player)
			{
				case PlayerType.Hero1: hero1 = hero; break;
				case PlayerType.Hero2: hero2 = hero; break;
				case PlayerType.Hero3: hero3 = hero; break;
				case PlayerType.Hero4: hero4 = hero; break;
				case PlayerType.None:
				default: break;
			}
		}

		#endregion

		#region Mines

		private ulong mines0;
		private ulong mines1;

		public PlayerType GetMine(int index)
		{
			var shift = (index & 15) << 2;
			var player = (GetMineUInt64(index) >> shift) & 15;

			switch (player)
			{
				case 1: return PlayerType.Hero1;
				case 2: return PlayerType.Hero2;
				case 3: return PlayerType.Hero3;
				case 4: return PlayerType.Hero4;
				default: return PlayerType.None;
			}
		}
		
		private void SetMines(Map map, int max, PlayerType oldOwner, PlayerType newOwner)
		{
			for (int i = 0; max > 0 && i < map.Mines.Length; i++)
			{
				var cur = GetMine(i);
				if (cur == oldOwner)
				{
					SetMine(i, newOwner);
					max--;
				}
			}
		}

		private void SetMine(int index, PlayerType owner)
		{
			switch (index >> 4)
			{
				case 1: mines1 = SetMineUInt64(index, mines1, owner); break;
				case 0:
				default: mines0 = SetMineUInt64(index, mines0, owner); break;
			}
		}
		private ulong SetMineUInt64(int index, ulong mine, PlayerType owner)
		{
			var lookup = index & 15;
			var res = mine & Bits.Unflag4[lookup];
			if (owner != PlayerType.None)
			{
				res |= OwnMine[owner][lookup];
			}
			return res;
		}
		public static readonly Dictionary<PlayerType, ulong[]> OwnMine = new Dictionary<PlayerType, ulong[]>()
		{
			{ PlayerType.Hero1, new ulong[]{
				0x0000000000000001,
				0x0000000000000010,
				0x0000000000000100,
				0x0000000000001000,
				0x0000000000010000,
				0x0000000000100000,
				0x0000000001000000,
				0x0000000010000000,
				0x0000000100000000,
				0x0000001000000000,
				0x0000010000000000,
				0x0000100000000000,
				0x0001000000000000,
				0x0010000000000000,
				0x0100000000000000,
				0x1000000000000000 } },
			{ PlayerType.Hero2, new ulong[]{
				0x0000000000000002,
				0x0000000000000020,
				0x0000000000000200,
				0x0000000000002000,
				0x0000000000020000,
				0x0000000000200000,
				0x0000000002000000,
				0x0000000020000000,
				0x0000000200000000,
				0x0000002000000000,
				0x0000020000000000,
				0x0000200000000000,
				0x0002000000000000,
				0x0020000000000000,
				0x0200000000000000,
				0x2000000000000000 } },
			{ PlayerType.Hero3, new ulong[]{
				0x0000000000000003,
				0x0000000000000030,
				0x0000000000000300,
				0x0000000000003000,
				0x0000000000030000,
				0x0000000000300000,
				0x0000000003000000,
				0x0000000030000000,
				0x0000000300000000,
				0x0000003000000000,
				0x0000030000000000,
				0x0000300000000000,
				0x0003000000000000,
				0x0030000000000000,
				0x0300000000000000,
				0x3000000000000000 } },
			{ PlayerType.Hero4, new ulong[]{
				0x0000000000000004,
				0x0000000000000040,
				0x0000000000000400,
				0x0000000000004000,
				0x0000000000040000,
				0x0000000000400000,
				0x0000000004000000,
				0x0000000040000000,
				0x0000000400000000,
				0x0000004000000000,
				0x0000040000000000,
				0x0000400000000000,
				0x0004000000000000,
				0x0040000000000000,
				0x0400000000000000,
				0x4000000000000000 } },
		};

		private ulong GetMineUInt64(int index)
		{
			// 16 mines per bank.
			switch (index >> 4)
			{
				case 1: return mines1;
				case 0:
				default: return mines0;
			}
		}

		/// <summary>Returns true if the tile is occupied by another the the requested player.</summary>
		public PlayerType GetOccupied(Tile tile, PlayerType player)
		{
			switch (player)
			{
				case PlayerType.Hero1:
					if (hero2.IsCurrentLocation(tile)) { return PlayerType.Hero2; }
					if (hero3.IsCurrentLocation(tile)) { return PlayerType.Hero3; }
					if (hero4.IsCurrentLocation(tile)) { return PlayerType.Hero4; }
					break;
				case PlayerType.Hero2:
					if (hero1.IsCurrentLocation(tile)) { return PlayerType.Hero1; }
					if (hero3.IsCurrentLocation(tile)) { return PlayerType.Hero3; }
					if (hero4.IsCurrentLocation(tile)) { return PlayerType.Hero4; }
					break;
				case PlayerType.Hero3:
					if (hero1.IsCurrentLocation(tile)) { return PlayerType.Hero1; }
					if (hero2.IsCurrentLocation(tile)) { return PlayerType.Hero2; }
					if (hero4.IsCurrentLocation(tile)) { return PlayerType.Hero4; }
					break;
				case PlayerType.Hero4:
					if (hero1.IsCurrentLocation(tile)) { return PlayerType.Hero1; }
					if (hero2.IsCurrentLocation(tile)) { return PlayerType.Hero2; }
					if (hero3.IsCurrentLocation(tile)) { return PlayerType.Hero3; }
					break;
			}
			return PlayerType.None;
		}
		#endregion

		/// <summary>The hero moves.</summary>
		public State Move(Map map, MoveDirection dir, PlayerType player)
		{
			var hero = GetHero(player);
			var source = map[hero];
			var target = source[dir];

			return Move(map, hero, player, source, target);
		}
		/// <summary>The hero moves.</summary>
		/// <remarks>
		/// If the hero:
		/// 
		/// * Tries to step out of the map, or over a tree, nothing happens.
		///     
		/// * Steps into a gold mine, he stays in place, and:
		///	  - If the mine already belongs to the hero, nothing happens.
		///   - If the mine is neutral, or belongs to another hero, a fight
		///     happens against the goblin guarding the mine. The hero loses
		///     20 life points. If he survives, the mine is his.
		///     
		/// * Steps into another hero, he stays in place and nothing happens.
		///   Hero fights are resolved at the end of the turn.
		///
		/// * Steps into a tavern, he stays in place and orders a beer. The hero
		///   pays 2 gold and receive 50HP. Note than HP can never exceed 100.
		///   
		/// * Times out, i.e. fails to send an order in the given delay (1 second),
		///   he stays in place until the game is finished. Note that he can
		///   still win if he as more gold than the other players at the end
		///   of the game.
		///   
		/// 
		/// After the hero has moved, a few things happen:
		/// 
		/// ::Fights
		/// Heroes are quite nervous and never miss an opportunity to slap each
		/// others with their big blades. After the hero has moved, if there is
		/// an enemy at a distance of one square in any direction, the hero
		/// attacks the nemy. For instance, in this situation, 
		/// after Hero 1 (@1) has moved:
		/// 
		/// @1@2
		///   @3
		///   
		/// @1 attacks @2. @1 does not attack @3 because it's 2 moves away.
		/// The attacking hero doesn't lose any life point, the defending one
		/// loses 20 life points.
		/// If the defender dies (see: Death of a hero), the attacking hero
		/// obtains control of all the loser's gold mines.
		/// 
		/// ::Mining
		/// After the hero has moved, he gains one gold per controlled mine.
		/// 
		/// ::Thirst
		/// 
		/// After the hero has moved, he loses one HP (because all this action made him thirsty).
		/// Note that heroes don't die of thirst. Worse case, they fall to 1 HP.
		/// </remarks>
		public State Move(Map map, Hero hero, PlayerType player, Tile source, Tile target)
		{
			unchecked
			{
				var state = new State()
				{
					turn = (ushort)(turn + 1),
					hero1 = hero1,
					hero2 = hero2,
					hero3 = hero3,
					hero4 = hero4,
					mines0 = mines0,
					mines1 = mines1,
				};

				int health = hero.Health;
				int mines = hero.Mines;
				int gold = hero.Gold;

				// Try to get a mine.
				if (target.MineIndex > -1)
				{
					var ownerPlayer = state.GetMine(target.MineIndex);

					// Try to get the mine.
					if (ownerPlayer != player)
					{
						health = Math.Max(Hero.HealthMin, health - Hero.HealthBattle);

						if (health > Hero.HealthMin)
						{
							mines++;
							state.SetMine(target.MineIndex, player);

							if (ownerPlayer != PlayerType.None)
							{
								var ownerHero = state.GetHero(ownerPlayer);
								ownerHero = ownerHero.LoseMine(ownerHero.Mines);
								state.SetHero(ownerPlayer, ownerHero);
							}
						}
					}
					target = source;
				}
				// Drink a beer.
				else if (target.TileType == TileType.Taverne)
				{
					target = source;

					if (gold >= Hero.CostsTavern)
					{
						gold -= Hero.CostsTavern;
						health = Math.Min(health + Hero.HealthTavern, Hero.HealthMax);
					}
				}
				// move or combat.
				else
				{
					var occupied = state.GetOccupied(target, player);

					if (occupied != PlayerType.None)
					{
						target = source;
					}
				}
				// died trying to get a mine.
				if (health == Hero.HealthMin)
				{
					hero = Hero.Respawn(map.GetSpawn(player), gold);
				}
				// try to battle.
				else
				{
					foreach (var other_player in PlayerTypes.Other[player])
					{
						var other_hero = state.GetHero(other_player);
						var other_tile = map[other_hero];

						foreach (var neighbor_tile in other_tile.Neighbors)
						{
							if (neighbor_tile == target)
							{
								var other_health = other_hero.Health;
								if (other_health <= Hero.HealthBattle)
								{
									var other_gold = other_hero.Gold;
									mines += other_hero.Mines;
									state.SetHero(other_player, Hero.Respawn(map.GetSpawn(other_player), other_gold));
									state.SetMines(map, other_gold, other_player, player);
								}
								break;
							}
						}
					}
					gold += mines;
					health = Math.Max(Hero.HealthThirst, health - Hero.HealthThirst);
					hero = Hero.Create(health, target, mines, gold);
				}

				state.SetHero(player, hero);

				return state;
			}
		}

		/// <summary>Updates the state.</summary>
		public State Update(Serialization.Game game)
		{
			var state = new State()
			{
				turn = (ushort)game.turn,
				hero1 = Hero.Create(game.heroes[0]),
				hero2 = Hero.Create(game.heroes[1]),
				hero3 = Hero.Create(game.heroes[2]),
				hero4 = Hero.Create(game.heroes[3]),
				mines0 = mines0,
				mines1 = mines1,
			};

			var index = 0;

			for (int p = 0; p < game.board.tiles.Length; p += 2)
			{
				if (game.board.tiles[p] == '$')
				{
					switch (game.board.tiles[p + 1])
					{
						case '1': state.SetMine(index, PlayerType.Hero1); break;
						case '2': state.SetMine(index, PlayerType.Hero2); break;
						case '3': state.SetMine(index, PlayerType.Hero3); break;
						case '4': state.SetMine(index, PlayerType.Hero4); break;
						case '-':
						default: state.SetMine(index, PlayerType.None); break; 
					}
					index++;
				}
			}
			return state;
		}

		/// <summary>Represents the state as unit test string.</summary>
		public string ToUnitTestString()
		{
			var sb = new StringBuilder();
			sb.AppendLine(hero1.DebugToString());
			sb.AppendLine(hero2.DebugToString());
			sb.AppendLine(hero3.DebugToString());
			sb.AppendLine(hero4.DebugToString());
			sb.AppendFormat("Turn: {0}", turn).AppendLine();
			sb.Append("Mines: ");

			for (int i = 0; i < 32; i++)
			{
				var owner = GetMine(i);
				switch (owner)
				{
					case PlayerType.Hero1: sb.Append('1'); break;
					case PlayerType.Hero2: sb.Append('2'); break;
					case PlayerType.Hero3: sb.Append('3'); break;
					case PlayerType.Hero4: sb.Append('4'); break;
					case PlayerType.None: default: sb.Append('.'); break;
				}
			}
			return sb.ToString();
		}

		/// <summary>Creates a state.</summary>
		public static State Create(Map map)
		{
			return Create(
				0,
				Hero.Initial(map, PlayerType.Hero1),
				Hero.Initial(map, PlayerType.Hero2),
				Hero.Initial(map, PlayerType.Hero3),
				Hero.Initial(map, PlayerType.Hero4),
				new int[0]);
		}

		/// <summary>Creates a state.</summary>
		public static State Create(
			int turn, 
			Hero hero1, 
			Hero hero2,
			Hero hero3, 
			Hero hero4, 
			int[] mines)
		{
			var state = new State()
			{
				turn = (ushort)turn,
				hero1 = hero1,
				hero2 = hero2,
				hero3 = hero3,
				hero4 = hero4,
			};

			for (int i = 0; i < mines.Length; i++)
			{
				state.SetMine(i, (PlayerType)mines[i]);
			}
			return state;
		}
	}
}
