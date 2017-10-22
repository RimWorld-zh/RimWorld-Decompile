using Verse;
using Verse.AI;

namespace RimWorld
{
	public class WorkGiver_Researcher : WorkGiver_Scanner
	{
		public override ThingRequest PotentialWorkThingRequest
		{
			get
			{
				return (Find.ResearchManager.currentProj != null) ? ThingRequest.ForGroup(ThingRequestGroup.BuildingArtificial) : ThingRequest.ForGroup(ThingRequestGroup.Nothing);
			}
		}

		public override bool Prioritized
		{
			get
			{
				return true;
			}
		}

		public override bool ShouldSkip(Pawn pawn)
		{
			return (byte)((Find.ResearchManager.currentProj == null) ? 1 : 0) != 0;
		}

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
					result = ((byte)(pawn.CanReserve(target, 1, -1, null, forced) ? 1 : 0) != 0);
				}
			}
			return result;
		}

		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			return new Job(JobDefOf.Research, t);
		}

		public override float GetPriority(Pawn pawn, TargetInfo t)
		{
			return t.Thing.GetStatValue(StatDefOf.ResearchSpeedFactor, true);
		}
	}
}
