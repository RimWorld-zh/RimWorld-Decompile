using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200012C RID: 300
	public class WorkGiver_ConstructFinishFrames : WorkGiver_Scanner
	{
		// Token: 0x170000E3 RID: 227
		// (get) Token: 0x0600062E RID: 1582 RVA: 0x00041658 File Offset: 0x0003FA58
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.Touch;
			}
		}

		// Token: 0x0600062F RID: 1583 RVA: 0x00041670 File Offset: 0x0003FA70
		public override Danger MaxPathDanger(Pawn pawn)
		{
			return Danger.Deadly;
		}

		// Token: 0x170000E4 RID: 228
		// (get) Token: 0x06000630 RID: 1584 RVA: 0x00041688 File Offset: 0x0003FA88
		public override ThingRequest PotentialWorkThingRequest
		{
			get
			{
				return ThingRequest.ForGroup(ThingRequestGroup.BuildingFrame);
			}
		}

		// Token: 0x06000631 RID: 1585 RVA: 0x000416A4 File Offset: 0x0003FAA4
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
