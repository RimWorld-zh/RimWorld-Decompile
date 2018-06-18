using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000A62 RID: 2658
	public class MentalStateWorker
	{
		// Token: 0x06003B2E RID: 15150 RVA: 0x001F6344 File Offset: 0x001F4744
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

		// Token: 0x0400255B RID: 9563
		public MentalStateDef def;
	}
}
