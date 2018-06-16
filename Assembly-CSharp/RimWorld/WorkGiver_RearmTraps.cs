using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000156 RID: 342
	public class WorkGiver_RearmTraps : WorkGiver_Scanner
	{
		// Token: 0x06000708 RID: 1800 RVA: 0x00047AB8 File Offset: 0x00045EB8
		public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
		{
			foreach (Designation des in pawn.Map.designationManager.SpawnedDesignationsOfDef(DesignationDefOf.RearmTrap))
			{
				yield return des.target.Thing;
			}
			yield break;
		}

		// Token: 0x1700010F RID: 271
		// (get) Token: 0x06000709 RID: 1801 RVA: 0x00047AE4 File Offset: 0x00045EE4
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.ClosestTouch;
			}
		}

		// Token: 0x0600070A RID: 1802 RVA: 0x00047AFC File Offset: 0x00045EFC
		public override Danger MaxPathDanger(Pawn pawn)
		{
			return Danger.Deadly;
		}

		// Token: 0x0600070B RID: 1803 RVA: 0x00047B14 File Offset: 0x00045F14
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			bool result;
			if (pawn.Map.designationManager.DesignationOn(t, DesignationDefOf.RearmTrap) == null)
			{
				result = false;
			}
			else
			{
				LocalTargetInfo target = t;
				if (!pawn.CanReserve(target, 1, -1, null, forced))
				{
					result = false;
				}
				else
				{
					List<Thing> thingList = t.Position.GetThingList(t.Map);
					for (int i = 0; i < thingList.Count; i++)
					{
						if (thingList[i] != t && thingList[i].def.category == ThingCategory.Item)
						{
							IntVec3 intVec;
							if (thingList[i].IsForbidden(pawn) || thingList[i].IsInValidStorage() || !HaulAIUtility.CanHaulAside(pawn, thingList[i], out intVec))
							{
								return false;
							}
						}
					}
					result = true;
				}
			}
			return result;
		}

		// Token: 0x0600070C RID: 1804 RVA: 0x00047C10 File Offset: 0x00046010
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			List<Thing> thingList = t.Position.GetThingList(t.Map);
			for (int i = 0; i < thingList.Count; i++)
			{
				if (thingList[i] != t && thingList[i].def.category == ThingCategory.Item)
				{
					Job job = HaulAIUtility.HaulAsideJobFor(pawn, thingList[i]);
					if (job != null)
					{
						return job;
					}
				}
			}
			return new Job(JobDefOf.RearmTrap, t);
		}
	}
}
