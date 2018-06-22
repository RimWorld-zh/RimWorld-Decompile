using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200015C RID: 348
	public class WorkGiver_Researcher : WorkGiver_Scanner
	{
		// Token: 0x1700011A RID: 282
		// (get) Token: 0x0600072D RID: 1837 RVA: 0x000488C0 File Offset: 0x00046CC0
		public override ThingRequest PotentialWorkThingRequest
		{
			get
			{
				ThingRequest result;
				if (Find.ResearchManager.currentProj == null)
				{
					result = ThingRequest.ForGroup(ThingRequestGroup.Nothing);
				}
				else
				{
					result = ThingRequest.ForGroup(ThingRequestGroup.ResearchBench);
				}
				return result;
			}
		}

		// Token: 0x1700011B RID: 283
		// (get) Token: 0x0600072E RID: 1838 RVA: 0x000488F8 File Offset: 0x00046CF8
		public override bool Prioritized
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600072F RID: 1839 RVA: 0x00048910 File Offset: 0x00046D10
		public override bool ShouldSkip(Pawn pawn, bool forced = false)
		{
			return Find.ResearchManager.currentProj == null;
		}

		// Token: 0x06000730 RID: 1840 RVA: 0x0004893C File Offset: 0x00046D3C
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			ResearchProjectDef currentProj = Find.ResearchManager.currentProj;
			bool result;
			if (currentProj == null)
			{
				result = false;
			}
			else
			{
				Building_ResearchBench building_ResearchBench = t as Building_ResearchBench;
				if (building_ResearchBench == null)
				{
					result = false;
				}
				else if (!currentProj.CanBeResearchedAt(building_ResearchBench, false))
				{
					result = false;
				}
				else
				{
					LocalTargetInfo target = t;
					result = pawn.CanReserve(target, 1, -1, null, forced);
				}
			}
			return result;
		}

		// Token: 0x06000731 RID: 1841 RVA: 0x000489B8 File Offset: 0x00046DB8
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			return new Job(JobDefOf.Research, t);
		}

		// Token: 0x06000732 RID: 1842 RVA: 0x000489E0 File Offset: 0x00046DE0
		public override float GetPriority(Pawn pawn, TargetInfo t)
		{
			return t.Thing.GetStatValue(StatDefOf.ResearchSpeedFactor, true);
		}
	}
}
