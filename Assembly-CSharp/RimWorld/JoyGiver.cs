using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020000FB RID: 251
	public abstract class JoyGiver
	{
		// Token: 0x06000545 RID: 1349 RVA: 0x000387E4 File Offset: 0x00036BE4
		public virtual float GetChance(Pawn pawn)
		{
			return this.def.baseChance;
		}

		// Token: 0x06000546 RID: 1350 RVA: 0x00038804 File Offset: 0x00036C04
		protected virtual List<Thing> GetSearchSet(Pawn pawn)
		{
			List<Thing> result;
			if (this.def.thingDefs == null)
			{
				JoyGiver.tmpCandidates.Clear();
				result = JoyGiver.tmpCandidates;
			}
			else if (this.def.thingDefs.Count == 1)
			{
				result = pawn.Map.listerThings.ThingsOfDef(this.def.thingDefs[0]);
			}
			else
			{
				JoyGiver.tmpCandidates.Clear();
				for (int i = 0; i < this.def.thingDefs.Count; i++)
				{
					JoyGiver.tmpCandidates.AddRange(pawn.Map.listerThings.ThingsOfDef(this.def.thingDefs[i]));
				}
				result = JoyGiver.tmpCandidates;
			}
			return result;
		}

		// Token: 0x06000547 RID: 1351
		public abstract Job TryGiveJob(Pawn pawn);

		// Token: 0x06000548 RID: 1352 RVA: 0x000388DC File Offset: 0x00036CDC
		public virtual Job TryGiveJobWhileInBed(Pawn pawn)
		{
			return null;
		}

		// Token: 0x06000549 RID: 1353 RVA: 0x000388F4 File Offset: 0x00036CF4
		public virtual Job TryGiveJobInPartyArea(Pawn pawn, IntVec3 partySpot)
		{
			return null;
		}

		// Token: 0x0600054A RID: 1354 RVA: 0x0003890C File Offset: 0x00036D0C
		public PawnCapacityDef MissingRequiredCapacity(Pawn pawn)
		{
			for (int i = 0; i < this.def.requiredCapacities.Count; i++)
			{
				if (!pawn.health.capacities.CapableOf(this.def.requiredCapacities[i]))
				{
					return this.def.requiredCapacities[i];
				}
			}
			return null;
		}

		// Token: 0x040002D2 RID: 722
		public JoyGiverDef def;

		// Token: 0x040002D3 RID: 723
		private static List<Thing> tmpCandidates = new List<Thing>();
	}
}
