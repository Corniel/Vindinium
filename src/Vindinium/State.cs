using System;
using System.Text;

namespace Vindinium
{
	/// <summary>Represents a Vindinium state.</summary>
	public struct State : IEquatable<State>
	{
		private int hash;
		private Hero hero1;
		private Hero hero2;
		private Hero hero3;
		private Hero hero4;
		private IMineOwnership ownership;

		private const int TurnMask = 2047;
		private const int HashMask = (int.MaxValue ^ 2047);
		
		/// <summary>Gets the turn of the state.</summary>
		public int Turn { get { return hash & TurnMask; } }

		/// <summary>Gets the mines.</summary>
		public IMineOwnership Mines { get { return ownership; } }

		/// <summary>Gets the player who has to move.</summary>
		public PlayerType PlayerToMove
		{
			get
			{
				switch (hash & 3)
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
		public Hero GetActiveHero()
		{
			return GetHero(this.PlayerToMove);
		}

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
		/// attacks the enemy. For instance, in this situation, 
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
#if DEBUG
				var test = GetHero(player);
				if (hero != test)
				{
					throw new Exception("Invalid hero.");
				}
#endif
				var state = new State()
				{
					hero1 = hero1,
					hero2 = hero2,
					hero3 = hero3,
					hero4 = hero4,
					ownership = ownership,
				};

				int health = hero.Health;
				int mines = hero.Mines;
				int gold = hero.Gold;

				// Try to get a mine.
				if (target.MineIndex > -1)
				{
					var ownerPlayer = state.ownership[target.MineIndex];

					// Try to get the mine.
					if (ownerPlayer != player)
					{
						health = Math.Max(Hero.HealthMin, health - Hero.HealthBattle);

						if (health > Hero.HealthMin)
						{
							mines++;
							state.ownership = state.ownership.Set(target.MineIndex, player);

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
				else if (target.TileType == TileType.Tavern)
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
					state.ownership = state.ownership.ChangeOwnership(player, PlayerType.None, map.Mines.Length);
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

									if (map.Mines.Length > 28)
									{
									}
									state.ownership = state.ownership.ChangeOwnership(other_player, player, map.Mines.Length);
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

#if DEBUG
				if (state.hero1.Mines != state.Mines.Count(PlayerType.Hero1) ||
					state.hero2.Mines != state.Mines.Count(PlayerType.Hero2) ||
					state.hero3.Mines != state.Mines.Count(PlayerType.Hero3) ||
					state.hero4.Mines != state.Mines.Count(PlayerType.Hero4))
				{
					throw new Exception("Invalid state");
				}
#endif
				state.hash = GetHash(this.Turn + 1, state.hero1, state.hero2, state.hero3, state.hero4);
				return state;
			}
		}

		private static int GetHash(int turn, Hero hero1, Hero hero2, Hero hero3, Hero hero4)
		{
			var hash =
				hero1.GetHashCode() ^
				(hero2.GetHashCode() << 1) ^
				(hero3.GetHashCode() << 2) ^
				(hero4.GetHashCode() << 3);

			return (hash & HashMask) | turn;
		}

		/// <summary>Updates the state.</summary>
		public State Update(Serialization.Game game)
		{
			var state = new State()
			{
				hero1 = Hero.Create(game.heroes[0]),
				hero2 = Hero.Create(game.heroes[1]),
				hero3 = Hero.Create(game.heroes[2]),
				hero4 = Hero.Create(game.heroes[3]),
				ownership = ownership.UpdateFromTiles(game.board.tiles),
			};
			state.hash = GetHash(game.turn, state.hero1, state.hero2, state.hero3, state.hero4);
			return state;
		}

		/// <summary>Represents the state as unit test string.</summary>
		public string ToUnitTestString()
		{
			var sb = new StringBuilder();
			sb.AppendLine(hero1.DebuggerDisplay);
			sb.AppendLine(hero2.DebuggerDisplay);
			sb.AppendLine(hero3.DebuggerDisplay);
			sb.AppendLine(hero4.DebuggerDisplay);
			sb.AppendFormat("Turn: {0}", this.Turn).AppendLine();
			sb.AppendFormat("Mines: {0}", ownership);
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
				MineOwnership.Create(map));
		}

		/// <summary>Creates a state.</summary>
		public static State Create(
			int turn,
			Hero hero1,
			Hero hero2,
			Hero hero3,
			Hero hero4,
			IMineOwnership ownership)
		{
			var state = new State()
			{
				hash = GetHash(turn, hero1, hero2, hero3, hero4),
				hero1 = hero1,
				hero2 = hero2,
				hero3 = hero3,
				hero4 = hero4,
				ownership = ownership
			};
			return state;
		}

		public override bool Equals(object obj)
		{
			return base.Equals(obj);
		}
		public override int GetHashCode()
		{
			return hash;
		}

		public bool Equals(State other)
		{
			return this.hash == other.hash &&
			this.hero1 == other.hero1 &&
			this.hero2 == other.hero2 &&
			this.hero3 == other.hero3 &&
			this.hero4 == other.hero4 &&
			this.ownership.Equals(other.ownership);
		}

		public static bool operator ==(State l, State r)
		{
			return l.Equals(r);
		}
		public static bool operator !=(State l, State r)
		{
			return !(l == r);
		}
	}
}
