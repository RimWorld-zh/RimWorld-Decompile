using Verse;
using Verse.AI;

namespace RimWorld
{
	public class WorkGiver_Tend : WorkGiver_Scanner
	{
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.InteractionCell;
			}
		}

		public override ThingRequest PotentialWorkThingRequest
		{
			get
			{
				return ThingRequest.ForGroup(ThingRequestGroup.Pawn);
			}
		}

		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			Pawn pawn2 = t as Pawn;
			if (pawn2 != null && (!base.def.tendToHumanlikesOnly || pawn2.RaceProps.Humanlike) && (!base.def.tendToAnimalsOnly || pawn2.RaceProps.Animal) && WorkGiver_Tend.GoodLayingStatusForTend(pawn2, pawn) && HealthAIUtility.ShouldBeTendedNow(pawn2) && pawn.CanReserve((Thing)pawn2, 1, -1, null, forced))
			{
				return true;
			}
			return false;
		}

		public static bool GoodLayingStatusForTend(Pawn patient, Pawn doctor)
		{
			if (patient == doctor)
			{
				return patient.GetPosture() == PawnPosture.Standing;
			}
			if (patient.RaceProps.Humanlike)
			{
				return patient.InBed();
			}
			return patient.GetPosture() != PawnPosture.Standing;
		}

		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			Pawn pawn2 = t as Pawn;
			Thing thing = null;
			if (Medicine.GetMedicineCountToFullyHeal(pawn2) > 0)
			{
				thing = HealthAIUtility.FindBestMedicine(pawn, pawn2);
			}
			if (thing != null)
			{
				return new Job(JobDefOf.TendPatient, (Thing)pawn2, thing);
			}
			return new Job(JobDefOf.TendPatient, (Thing)pawn2);
		}
	}
}
