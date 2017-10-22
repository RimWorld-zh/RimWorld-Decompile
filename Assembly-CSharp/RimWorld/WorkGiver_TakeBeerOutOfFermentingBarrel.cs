using Verse;
using Verse.AI;

namespace RimWorld
{
	public class WorkGiver_TakeBeerOutOfFermentingBarrel : WorkGiver_Scanner
	{
		public override ThingRequest PotentialWorkThingRequest
		{
			get
			{
				return ThingRequest.ForDef(ThingDefOf.FermentingBarrel);
			}
		}

		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.Touch;
			}
		}

		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			Building_FermentingBarrel building_FermentingBarrel = t as Building_FermentingBarrel;
			bool result;
			if (building_FermentingBarrel != null && building_FermentingBarrel.Fermented)
			{
				if (t.IsBurning())
				{
					result = false;
					goto IL_0069;
				}
				if (!t.IsForbidden(pawn))
				{
					LocalTargetInfo target = t;
					if (!pawn.CanReserve(target, 1, -1, null, forced))
						goto IL_005b;
					result = true;
					goto IL_0069;
				}
				goto IL_005b;
			}
			result = false;
			goto IL_0069;
			IL_0069:
			return result;
			IL_005b:
			result = false;
			goto IL_0069;
		}

		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			return new Job(JobDefOf.TakeBeerOutOfFermentingBarrel, t);
		}
	}
}
