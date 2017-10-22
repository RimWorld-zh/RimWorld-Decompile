using Verse;
using Verse.AI;

namespace RimWorld
{
	internal class WorkGiver_FightFires : WorkGiver_Scanner
	{
		private const int NearbyPawnRadius = 15;

		private const int MaxReservationCheckDistance = 15;

		private const float HandledDistance = 5f;

		public override ThingRequest PotentialWorkThingRequest
		{
			get
			{
				return ThingRequest.ForDef(ThingDefOf.Fire);
			}
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
						result = false;
						goto IL_0183;
					}
					if ((pawn2.Faction == pawn.Faction || pawn2.HostFaction == pawn.Faction || pawn2.HostFaction == pawn.HostFaction) && !((Area)pawn.Map.areaManager.Home)[fire.Position] && IntVec3Utility.ManhattanDistanceFlat(pawn.Position, pawn2.Position) > 15)
					{
						result = false;
						goto IL_0183;
					}
					if (!pawn.CanReach((Thing)pawn2, PathEndMode.Touch, Danger.Deadly, false, TraverseMode.ByPawn))
					{
						result = false;
						goto IL_0183;
					}
				}
				else
				{
					if (pawn.story.WorkTagIsDisabled(WorkTags.Firefighting))
					{
						result = false;
						goto IL_0183;
					}
					if (!((Area)pawn.Map.areaManager.Home)[fire.Position])
					{
						JobFailReason.Is(WorkGiver_FixBrokenDownBuilding.NotInHomeAreaTrans);
						result = false;
						goto IL_0183;
					}
				}
				if ((pawn.Position - fire.Position).LengthHorizontalSquared > 225)
				{
					LocalTargetInfo target = (Thing)fire;
					if (!pawn.CanReserve(target, 1, -1, null, forced))
					{
						result = false;
						goto IL_0183;
					}
				}
				result = ((byte)((!WorkGiver_FightFires.FireIsBeingHandled(fire, pawn)) ? 1 : 0) != 0);
			}
			goto IL_0183;
			IL_0183:
			return result;
		}

		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			return new Job(JobDefOf.BeatFire, t);
		}

		public static bool FireIsBeingHandled(Fire f, Pawn potentialHandler)
		{
			bool result;
			if (!f.Spawned)
			{
				result = false;
			}
			else
			{
				Pawn pawn = f.Map.reservationManager.FirstRespectedReserver((Thing)f, potentialHandler);
				result = (pawn != null && pawn.Position.InHorDistOf(f.Position, 5f));
			}
			return result;
		}
	}
}
