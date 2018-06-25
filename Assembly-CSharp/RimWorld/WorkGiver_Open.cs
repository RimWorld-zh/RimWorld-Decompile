using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000151 RID: 337
	public class WorkGiver_Open : WorkGiver_Scanner
	{
		// Token: 0x060006F5 RID: 1781 RVA: 0x00047298 File Offset: 0x00045698
		public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
		{
			foreach (Designation des in pawn.Map.designationManager.SpawnedDesignationsOfDef(DesignationDefOf.Open))
			{
				yield return des.target.Thing;
			}
			yield break;
		}

		// Token: 0x1700010D RID: 269
		// (get) Token: 0x060006F6 RID: 1782 RVA: 0x000472C4 File Offset: 0x000456C4
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.ClosestTouch;
			}
		}

		// Token: 0x060006F7 RID: 1783 RVA: 0x000472DC File Offset: 0x000456DC
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			bool result;
			if (pawn.Map.designationManager.DesignationOn(t, DesignationDefOf.Open) == null)
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

		// Token: 0x060006F8 RID: 1784 RVA: 0x00047338 File Offset: 0x00045738
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			return new Job(JobDefOf.Open, t);
		}
	}
}
