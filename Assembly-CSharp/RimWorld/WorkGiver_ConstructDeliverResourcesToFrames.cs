using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class WorkGiver_ConstructDeliverResourcesToFrames : WorkGiver_ConstructDeliverResources
	{
		public WorkGiver_ConstructDeliverResourcesToFrames()
		{
		}

		public override ThingRequest PotentialWorkThingRequest
		{
			get
			{
				return ThingRequest.ForGroup(ThingRequestGroup.BuildingFrame);
			}
		}

		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			Job result;
			if (t.Faction != pawn.Faction)
			{
				result = null;
			}
			else
			{
				Frame frame = t as Frame;
				if (frame == null)
				{
					result = null;
				}
				else if (GenConstruct.FirstBlockingThing(frame, pawn) != null)
				{
					result = GenConstruct.HandleBlockingThingJob(frame, pawn, forced);
				}
				else
				{
					bool checkConstructionSkill = this.def.workType == WorkTypeDefOf.Construction;
					if (!GenConstruct.CanConstruct(frame, pawn, checkConstructionSkill, forced))
					{
						result = null;
					}
					else
					{
						result = base.ResourceDeliverJobFor(pawn, frame, true);
					}
				}
			}
			return result;
		}
	}
}
