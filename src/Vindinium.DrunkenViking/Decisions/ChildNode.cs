using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vindinium.DrunkenViking.Decisions
{
	public class ChildNode : Node
	{
		public ChildNode()
		{
			this.Plans = new Queue<PlanType>();
		}
		
		public Queue<PlanType> Plans { get; protected set; }

		public override void GeneratePlans(Map map)
		{
			var player = this.PlayerToMove;
			var hero = this.State.GetHero(player);

			if (hero.IsCrashed)
			{
				this.Plans.Enqueue(PlanType.Crashed);
			}
			else
			{
				var source = map[hero];
				int health = hero.Health;
				// Combat, nothing else.
				if (source.Neighbors.Any(tile => this.State.GetOccupied(tile, player) != PlayerType.None))
				{
					this.Plans.Enqueue(PlanType.Flee);
					this.Plans.Enqueue(PlanType.Attack);
				}
				else if (health < Hero.HealthBattle)
				{
					this.Plans.Enqueue(PlanType.ToTaverne);
					// kill yourself.
					this.Plans.Enqueue(PlanType.Attack);
					this.Plans.Enqueue(PlanType.Flee);
				}
				else
				{
					// If beside a taverne, check if usefull.
					if (source.Neighbors.Any(tile => tile.IsTaverne))
					{
						this.Plans.Enqueue(PlanType.ToTaverne);
					}

					this.Plans.Enqueue(PlanType.ToOppoMine);
					this.Plans.Enqueue(PlanType.ToFreeMine);
					if (hero.Mines >= (map.Mines.Length >> 1))
					{
						this.Plans.Enqueue(PlanType.ToOwnMine);
					}
					if (PlayerTypes.Other[player].Select(tp => this.State.GetHero(tp)).Any(oppo => oppo.Health + Hero.HealthBattle < health))
					{
						this.Plans.Enqueue(PlanType.Attack);
					}
					this.Plans.Enqueue(PlanType.Flee);
				}
			}
		}

		public override int GetScore(PlayerType player)
		{
			throw new NotImplementedException();
		}


		public static Node Create(State state)
		{
			var child = new ChildNode()
			{
				State = state,
			};
			return child;
		}
	}
}
