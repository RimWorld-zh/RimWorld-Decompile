using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class WorkGiver_ConstructFinishFrames : WorkGiver_Scanner
	{
		public WorkGiver_ConstructFinishFrames()
		{
		}

		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.Touch;
			}
		}

		public override Danger MaxPathDanger(Pawn pawn)
		{
			return Danger.Deadly;
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
				else if (frame.MaterialsNeeded().Count > 0)
				{
					result = null;
				}
				else if (GenConstruct.FirstBlockingThing(frame, pawn) != null)
				{
					result = GenConstruct.HandleBlockingThingJob(frame, pawn, forced);
				}
				else
				{
					Frame t2 = frame;
					if (!GenConstruct.CanConstruct(t2, pawn, true, forced))
					{
						result = null;
					}
					else
					{
						result = new Job(JobDefOf.FinishFrame, frame);
					}
				}
			}
			return result;
		}
	}
}
