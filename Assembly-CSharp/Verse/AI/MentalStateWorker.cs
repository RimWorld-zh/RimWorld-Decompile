using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000A61 RID: 2657
	public class MentalStateWorker
	{
		// Token: 0x04002567 RID: 9575
		public MentalStateDef def;

		// Token: 0x06003B2E RID: 15150 RVA: 0x001F6A98 File Offset: 0x001F4E98
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
