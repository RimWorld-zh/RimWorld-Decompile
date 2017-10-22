using Verse;
using Verse.AI;

namespace RimWorld
{
	public class WorkGiver_ConstructFinishFrames : WorkGiver_Scanner
	{
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.Touch;
			}
		}

		public override ThingRequest PotentialWorkThingRequest
		{
			get
			{
				return ThingRequest.ForGroup(ThingRequestGroup.BuildingFrame);
			}
		}

		public override Danger MaxPathDanger(Pawn pawn)
		{
			return Danger.Deadly;
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
				result = ((frame == null) ? null : (GenConstruct.CanConstruct(frame, pawn, forced) ? ((frame.MaterialsNeeded().Count <= 0) ? new Job(JobDefOf.FinishFrame, (Thing)frame) : null) : null));
			}
			return result;
		}
	}
}
