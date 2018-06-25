using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020000FB RID: 251
	public abstract class JoyGiver
	{
		// Token: 0x040002D3 RID: 723
		public JoyGiverDef def;

		// Token: 0x040002D4 RID: 724
		private static List<Thing> tmpCandidates = new List<Thing>();

		// Token: 0x06000545 RID: 1349 RVA: 0x000387EC File Offset: 0x00036BEC
		public virtual float GetChance(Pawn pawn)
		{
			return this.def.baseChance;
		}

		// Token: 0x06000546 RID: 1350 RVA: 0x0003880C File Offset: 0x00036C0C
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

		// Token: 0x06000548 RID: 1352 RVA: 0x000388E4 File Offset: 0x00036CE4
		public virtual Job TryGiveJobWhileInBed(Pawn pawn)
		{
			return null;
		}

		// Token: 0x06000549 RID: 1353 RVA: 0x000388FC File Offset: 0x00036CFC
		public virtual Job TryGiveJobInPartyArea(Pawn pawn, IntVec3 partySpot)
		{
			return null;
		}

		// Token: 0x0600054A RID: 1354 RVA: 0x00038914 File Offset: 0x00036D14
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
	}
}
