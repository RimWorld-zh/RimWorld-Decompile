using Verse;
using Verse.AI;

namespace RimWorld
{
	public class WorkGiver_ConstructDeliverResourcesToFrames : WorkGiver_ConstructDeliverResources
	{
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
				result = ((frame == null) ? null : (GenConstruct.CanConstruct(frame, pawn, forced) ? base.ResourceDeliverJobFor(pawn, frame, true) : null));
			}
			return result;
		}
	}
}
