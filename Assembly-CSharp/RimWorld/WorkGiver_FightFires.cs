using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000142 RID: 322
	internal class WorkGiver_FightFires : WorkGiver_Scanner
	{
		// Token: 0x04000322 RID: 802
		private const int NearbyPawnRadius = 15;

		// Token: 0x04000323 RID: 803
		private const int MaxReservationCheckDistance = 15;

		// Token: 0x04000324 RID: 804
		private const float HandledDistance = 5f;

		// Token: 0x17000100 RID: 256
		// (get) Token: 0x060006A6 RID: 1702 RVA: 0x00044CB4 File Offset: 0x000430B4
		public override ThingRequest PotentialWorkThingRequest
		{
			get
			{
				return ThingRequest.ForDef(ThingDefOf.Fire);
			}
		}

		// Token: 0x17000101 RID: 257
		// (get) Token: 0x060006A7 RID: 1703 RVA: 0x00044CD4 File Offset: 0x000430D4
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.Touch;
			}
		}

		// Token: 0x060006A8 RID: 1704 RVA: 0x00044CEC File Offset: 0x000430EC
		public override Danger MaxPathDanger(Pawn pawn)
		{
			return Danger.Deadly;
		}

		// Token: 0x060006A9 RID: 1705 RVA: 0x00044D04 File Offset: 0x00043104
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			Fire fire = t as Fire;
			bool result;
			if (fire == null)
			{
				result = false;
			}
			else
			{
				Pawn pawn2 = fire.parent as Pawn;
				if (pawn2 != null)
				{
					if (pawn2 == pawn)
					{
						return false;
					}
					if (pawn2.Faction == pawn.Faction || pawn2.HostFaction == pawn.Faction || pawn2.HostFaction == pawn.HostFaction)
					{
						if (!pawn.Map.areaManager.Home[fire.Position] && IntVec3Utility.ManhattanDistanceFlat(pawn.Position, pawn2.Position) > 15)
						{
							return false;
						}
					}
					if (!pawn.CanReach(pawn2, PathEndMode.Touch, Danger.Deadly, false, TraverseMode.ByPawn))
					{
						return false;
					}
				}
				else
				{
					if (pawn.story.WorkTagIsDisabled(WorkTags.Firefighting))
					{
						return false;
					}
					if (!pawn.Map.areaManager.Home[fire.Position])
					{
						JobFailReason.Is(WorkGiver_FixBrokenDownBuilding.NotInHomeAreaTrans, null);
						return false;
					}
				}
				if ((pawn.Position - fire.Position).LengthHorizontalSquared > 225)
				{
					LocalTargetInfo target = fire;
					if (!pawn.CanReserve(target, 1, -1, null, forced))
					{
						return false;
					}
				}
				result = !WorkGiver_FightFires.FireIsBeingHandled(fire, pawn);
			}
			return result;
		}

		// Token: 0x060006AA RID: 1706 RVA: 0x00044E98 File Offset: 0x00043298
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			return new Job(JobDefOf.BeatFire, t);
		}

		// Token: 0x060006AB RID: 1707 RVA: 0x00044EC0 File Offset: 0x000432C0
		public static bool FireIsBeingHandled(Fire f, Pawn potentialHandler)
		{
			bool result;
			if (!f.Spawned)
			{
				result = false;
			}
			else
			{
				Pawn pawn = f.Map.reservationManager.FirstRespectedReserver(f, potentialHandler);
				result = (pawn != null && pawn.Position.InHorDistOf(f.Position, 5f));
			}
			return result;
		}
	}
}
