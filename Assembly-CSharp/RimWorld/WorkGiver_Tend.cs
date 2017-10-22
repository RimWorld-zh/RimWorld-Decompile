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

		public override Danger MaxPathDanger(Pawn pawn)
		{
			return Danger.Deadly;
		}

		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			Pawn pawn2 = t as Pawn;
			bool result;
			if (pawn2 != null && (!base.def.tendToHumanlikesOnly || pawn2.RaceProps.Humanlike) && (!base.def.tendToAnimalsOnly || pawn2.RaceProps.Animal) && WorkGiver_Tend.GoodLayingStatusForTend(pawn2, pawn) && HealthAIUtility.ShouldBeTendedNow(pawn2))
			{
				LocalTargetInfo target = (Thing)pawn2;
				if (!pawn.CanReserve(target, 1, -1, null, forced))
					goto IL_0080;
				result = true;
				goto IL_0091;
			}
			goto IL_0080;
			IL_0080:
			result = false;
			goto IL_0091;
			IL_0091:
			return result;
		}

		public static bool GoodLayingStatusForTend(Pawn patient, Pawn doctor)
		{
			return patient == doctor || ((!patient.RaceProps.Humanlike) ? (patient.GetPosture() != PawnPosture.Standing) : patient.InBed());
		}

		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			Pawn pawn2 = t as Pawn;
			Thing thing = null;
			if (Medicine.GetMedicineCountToFullyHeal(pawn2) > 0)
			{
				thing = HealthAIUtility.FindBestMedicine(pawn, pawn2);
			}
			return (thing == null) ? new Job(JobDefOf.TendPatient, (Thing)pawn2) : new Job(JobDefOf.TendPatient, (Thing)pawn2, thing);
		}
	}
}
