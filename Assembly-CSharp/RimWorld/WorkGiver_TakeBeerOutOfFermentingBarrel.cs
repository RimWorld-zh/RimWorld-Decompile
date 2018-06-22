using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200015E RID: 350
	public class WorkGiver_TakeBeerOutOfFermentingBarrel : WorkGiver_Scanner
	{
		// Token: 0x1700011D RID: 285
		// (get) Token: 0x0600073A RID: 1850 RVA: 0x00048D24 File Offset: 0x00047124
		public override ThingRequest PotentialWorkThingRequest
		{
			get
			{
				return ThingRequest.ForDef(ThingDefOf.FermentingBarrel);
			}
		}

		// Token: 0x1700011E RID: 286
		// (get) Token: 0x0600073B RID: 1851 RVA: 0x00048D44 File Offset: 0x00047144
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.Touch;
			}
		}

		// Token: 0x0600073C RID: 1852 RVA: 0x00048D5C File Offset: 0x0004715C
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			Building_FermentingBarrel building_FermentingBarrel = t as Building_FermentingBarrel;
			bool result;
			if (building_FermentingBarrel == null || !building_FermentingBarrel.Fermented)
			{
				result = false;
			}
			else if (t.IsBurning())
			{
				result = false;
			}
			else
			{
				if (!t.IsForbidden(pawn))
				{
					LocalTargetInfo target = t;
					if (pawn.CanReserve(target, 1, -1, null, forced))
					{
						return true;
					}
				}
				result = false;
			}
			return result;
		}

		// Token: 0x0600073D RID: 1853 RVA: 0x00048DD4 File Offset: 0x000471D4
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			return new Job(JobDefOf.TakeBeerOutOfFermentingBarrel, t);
		}
	}
}
