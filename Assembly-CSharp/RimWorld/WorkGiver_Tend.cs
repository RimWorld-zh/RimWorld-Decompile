using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000161 RID: 353
	public class WorkGiver_Tend : WorkGiver_Scanner
	{
		// Token: 0x17000121 RID: 289
		// (get) Token: 0x06000747 RID: 1863 RVA: 0x00048FDC File Offset: 0x000473DC
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.InteractionCell;
			}
		}

		// Token: 0x06000748 RID: 1864 RVA: 0x00048FF4 File Offset: 0x000473F4
		public override Danger MaxPathDanger(Pawn pawn)
		{
			return Danger.Deadly;
		}

		// Token: 0x17000122 RID: 290
		// (get) Token: 0x06000749 RID: 1865 RVA: 0x0004900C File Offset: 0x0004740C
		public override ThingRequest PotentialWorkThingRequest
		{
			get
			{
				return ThingRequest.ForGroup(ThingRequestGroup.Pawn);
			}
		}

		// Token: 0x0600074A RID: 1866 RVA: 0x00049028 File Offset: 0x00047428
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			Pawn pawn2 = t as Pawn;
			if (pawn2 != null && (!this.def.tendToHumanlikesOnly || pawn2.RaceProps.Humanlike) && (!this.def.tendToAnimalsOnly || pawn2.RaceProps.Animal) && WorkGiver_Tend.GoodLayingStatusForTend(pawn2, pawn) && HealthAIUtility.ShouldBeTendedNowByPlayer(pawn2))
			{
				LocalTargetInfo target = pawn2;
				if (pawn.CanReserve(target, 1, -1, null, forced))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600074B RID: 1867 RVA: 0x000490C8 File Offset: 0x000474C8
		public static bool GoodLayingStatusForTend(Pawn patient, Pawn doctor)
		{
			bool result;
			if (patient == doctor)
			{
				result = true;
			}
			else if (patient.RaceProps.Humanlike)
			{
				result = patient.InBed();
			}
			else
			{
				result = (patient.GetPosture() != PawnPosture.Standing);
			}
			return result;
		}

		// Token: 0x0600074C RID: 1868 RVA: 0x00049114 File Offset: 0x00047514
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			Pawn pawn2 = t as Pawn;
			Thing thing = HealthAIUtility.FindBestMedicine(pawn, pawn2);
			Job result;
			if (thing != null)
			{
				result = new Job(JobDefOf.TendPatient, pawn2, thing);
			}
			else
			{
				result = new Job(JobDefOf.TendPatient, pawn2);
			}
			return result;
		}
	}
}
