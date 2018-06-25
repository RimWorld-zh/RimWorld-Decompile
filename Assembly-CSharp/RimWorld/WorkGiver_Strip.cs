using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200015D RID: 349
	public class WorkGiver_Strip : WorkGiver_Scanner
	{
		// Token: 0x06000733 RID: 1843 RVA: 0x00048A0C File Offset: 0x00046E0C
		public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
		{
			foreach (Designation des in pawn.Map.designationManager.SpawnedDesignationsOfDef(DesignationDefOf.Strip))
			{
				if (!des.target.HasThing)
				{
					Log.ErrorOnce("Strip designation has no target.", 63126, false);
				}
				else
				{
					yield return des.target.Thing;
				}
			}
			yield break;
		}

		// Token: 0x1700011C RID: 284
		// (get) Token: 0x06000734 RID: 1844 RVA: 0x00048A38 File Offset: 0x00046E38
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.ClosestTouch;
			}
		}

		// Token: 0x06000735 RID: 1845 RVA: 0x00048A50 File Offset: 0x00046E50
		public override Danger MaxPathDanger(Pawn pawn)
		{
			return Danger.Deadly;
		}

		// Token: 0x06000736 RID: 1846 RVA: 0x00048A68 File Offset: 0x00046E68
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			bool result;
			if (t.Map.designationManager.DesignationOn(t, DesignationDefOf.Strip) == null)
			{
				result = false;
			}
			else
			{
				LocalTargetInfo target = t;
				result = (pawn.CanReserve(target, 1, -1, null, forced) && StrippableUtility.CanBeStrippedByColony(t));
			}
			return result;
		}

		// Token: 0x06000737 RID: 1847 RVA: 0x00048AD4 File Offset: 0x00046ED4
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			return new Job(JobDefOf.Strip, t);
		}
	}
}
