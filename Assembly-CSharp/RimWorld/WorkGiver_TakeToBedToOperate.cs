using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000160 RID: 352
	public class WorkGiver_TakeToBedToOperate : WorkGiver_TakeToBed
	{
		// Token: 0x1700011F RID: 287
		// (get) Token: 0x06000740 RID: 1856 RVA: 0x00048E00 File Offset: 0x00047200
		public override ThingRequest PotentialWorkThingRequest
		{
			get
			{
				return ThingRequest.ForGroup(ThingRequestGroup.Pawn);
			}
		}

		// Token: 0x17000120 RID: 288
		// (get) Token: 0x06000741 RID: 1857 RVA: 0x00048E1C File Offset: 0x0004721C
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.OnCell;
			}
		}

		// Token: 0x06000742 RID: 1858 RVA: 0x00048E34 File Offset: 0x00047234
		public override Danger MaxPathDanger(Pawn pawn)
		{
			return Danger.Deadly;
		}

		// Token: 0x06000743 RID: 1859 RVA: 0x00048E4C File Offset: 0x0004724C
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			Pawn pawn2 = t as Pawn;
			if (pawn2 != null && pawn2 != pawn && !pawn2.InBed() && pawn2.RaceProps.IsFlesh && HealthAIUtility.ShouldHaveSurgeryDoneNow(pawn2))
			{
				LocalTargetInfo target = pawn2;
				if (pawn.CanReserve(target, 1, -1, null, forced) && (!pawn2.InMentalState || !pawn2.MentalStateDef.IsAggro))
				{
					if (!pawn2.Downed)
					{
						if (pawn2.IsColonist)
						{
							return false;
						}
						if (!pawn2.IsPrisonerOfColony && pawn2.Faction != Faction.OfPlayer)
						{
							return false;
						}
						if (pawn2.guest != null && pawn2.guest.Released)
						{
							return false;
						}
					}
					Building_Bed building_Bed = base.FindBed(pawn, pawn2);
					return building_Bed != null && pawn2.CanReserve(building_Bed, building_Bed.SleepingSlotsCount, -1, null, false);
				}
			}
			return false;
		}

		// Token: 0x06000744 RID: 1860 RVA: 0x00048F78 File Offset: 0x00047378
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			Pawn pawn2 = t as Pawn;
			Building_Bed t2 = base.FindBed(pawn, pawn2);
			return new Job(JobDefOf.TakeToBedToOperate, pawn2, t2)
			{
				count = 1
			};
		}
	}
}
