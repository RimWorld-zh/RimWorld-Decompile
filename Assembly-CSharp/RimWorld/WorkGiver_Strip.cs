using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200015D RID: 349
	public class WorkGiver_Strip : WorkGiver_Scanner
	{
		// Token: 0x06000734 RID: 1844 RVA: 0x00048A10 File Offset: 0x00046E10
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
		// (get) Token: 0x06000735 RID: 1845 RVA: 0x00048A3C File Offset: 0x00046E3C
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.ClosestTouch;
			}
		}

		// Token: 0x06000736 RID: 1846 RVA: 0x00048A54 File Offset: 0x00046E54
		public override Danger MaxPathDanger(Pawn pawn)
		{
			return Danger.Deadly;
		}

		// Token: 0x06000737 RID: 1847 RVA: 0x00048A6C File Offset: 0x00046E6C
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

		// Token: 0x06000738 RID: 1848 RVA: 0x00048AD8 File Offset: 0x00046ED8
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			return new Job(JobDefOf.Strip, t);
		}
	}
}
