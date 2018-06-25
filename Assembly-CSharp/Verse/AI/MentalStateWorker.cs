using System;
using RimWorld;

namespace Verse.AI
{
	public class MentalStateWorker
	{
		public MentalStateDef def;

		public MentalStateWorker()
		{
		}

		public virtual bool StateCanOccur(Pawn pawn)
		{
			bool result;
			if (!this.def.unspawnedCanDo && !pawn.Spawned)
			{
				result = false;
			}
			else if (!this.def.prisonersCanDo && pawn.HostFaction != null)
			{
				result = false;
			}
			else if (this.def.colonistsOnly && pawn.Faction != Faction.OfPlayer)
			{
				result = false;
			}
			else
			{
				for (int i = 0; i < this.def.requiredCapacities.Count; i++)
				{
					if (!pawn.health.capacities.CapableOf(this.def.requiredCapacities[i]))
					{
						return false;
					}
				}
				result = true;
			}
			return result;
		}
	}
}
