using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000165 RID: 357
	public class WorkGiver_Flick : WorkGiver_Scanner
	{
		// Token: 0x17000124 RID: 292
		// (get) Token: 0x06000756 RID: 1878 RVA: 0x00049398 File Offset: 0x00047798
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.Touch;
			}
		}

		// Token: 0x06000757 RID: 1879 RVA: 0x000493B0 File Offset: 0x000477B0
		public override Danger MaxPathDanger(Pawn pawn)
		{
			return Danger.Deadly;
		}

		// Token: 0x06000758 RID: 1880 RVA: 0x000493C8 File Offset: 0x000477C8
		public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
		{
			List<Designation> desList = pawn.Map.designationManager.allDesignations;
			for (int i = 0; i < desList.Count; i++)
			{
				if (desList[i].def == DesignationDefOf.Flick)
				{
					yield return desList[i].target.Thing;
				}
			}
			yield break;
		}

		// Token: 0x06000759 RID: 1881 RVA: 0x000493F4 File Offset: 0x000477F4
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			bool result;
			if (pawn.Map.designationManager.DesignationOn(t, DesignationDefOf.Flick) == null)
			{
				result = false;
			}
			else
			{
				LocalTargetInfo target = t;
				result = pawn.CanReserve(target, 1, -1, null, forced);
			}
			return result;
		}

		// Token: 0x0600075A RID: 1882 RVA: 0x00049450 File Offset: 0x00047850
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			return new Job(JobDefOf.Flick, t);
		}
	}
}
