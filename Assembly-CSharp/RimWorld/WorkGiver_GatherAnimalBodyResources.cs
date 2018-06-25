using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200011D RID: 285
	public abstract class WorkGiver_GatherAnimalBodyResources : WorkGiver_Scanner
	{
		// Token: 0x170000D5 RID: 213
		// (get) Token: 0x060005E5 RID: 1509
		protected abstract JobDef JobDef { get; }

		// Token: 0x060005E6 RID: 1510
		protected abstract CompHasGatherableBodyResource GetComp(Pawn animal);

		// Token: 0x060005E7 RID: 1511 RVA: 0x0003F444 File Offset: 0x0003D844
		public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
		{
			List<Pawn> pawns = pawn.Map.mapPawns.SpawnedPawnsInFaction(pawn.Faction);
			for (int i = 0; i < pawns.Count; i++)
			{
				yield return pawns[i];
			}
			yield break;
		}

		// Token: 0x170000D6 RID: 214
		// (get) Token: 0x060005E8 RID: 1512 RVA: 0x0003F470 File Offset: 0x0003D870
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.Touch;
			}
		}

		// Token: 0x060005E9 RID: 1513 RVA: 0x0003F488 File Offset: 0x0003D888
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			Pawn pawn2 = t as Pawn;
			bool result;
			if (pawn2 == null || !pawn2.RaceProps.Animal)
			{
				result = false;
			}
			else
			{
				CompHasGatherableBodyResource comp = this.GetComp(pawn2);
				if (comp != null && comp.ActiveAndFull && !pawn2.Downed && pawn2.CanCasuallyInteractNow(false))
				{
					LocalTargetInfo target = pawn2;
					if (pawn.CanReserve(target, 1, -1, null, forced))
					{
						return true;
					}
				}
				result = false;
			}
			return result;
		}

		// Token: 0x060005EA RID: 1514 RVA: 0x0003F51C File Offset: 0x0003D91C
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			return new Job(this.JobDef, t);
		}
	}
}
