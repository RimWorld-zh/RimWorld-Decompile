using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class WorkGiver_Repair : WorkGiver_Scanner
	{
		public override ThingRequest PotentialWorkThingRequest
		{
			get
			{
				return ThingRequest.ForGroup(ThingRequestGroup.BuildingArtificial);
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

		public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
		{
			return pawn.Map.listerBuildingsRepairable.RepairableBuildings(pawn.Faction);
		}

		public override bool ShouldSkip(Pawn pawn)
		{
			return pawn.Map.listerBuildingsRepairable.RepairableBuildings(pawn.Faction).Count == 0;
		}

		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			Building building = t as Building;
			bool result;
			if (building == null)
			{
				result = false;
			}
			else if (!pawn.Map.listerBuildingsRepairable.Contains(pawn.Faction, building))
			{
				result = false;
			}
			else if (!building.def.building.repairable)
			{
				result = false;
			}
			else if (t.Faction != pawn.Faction)
			{
				result = false;
			}
			else if (pawn.Faction == Faction.OfPlayer && !((Area)pawn.Map.areaManager.Home)[t.Position])
			{
				JobFailReason.Is(WorkGiver_FixBrokenDownBuilding.NotInHomeAreaTrans);
				result = false;
			}
			else if (!t.def.useHitPoints || t.HitPoints == t.MaxHitPoints)
			{
				result = false;
			}
			else
			{
				LocalTargetInfo target = (Thing)building;
				result = ((byte)(pawn.CanReserve(target, 1, -1, null, forced) ? ((building.Map.designationManager.DesignationOn(building, DesignationDefOf.Deconstruct) == null) ? ((!building.IsBurning()) ? 1 : 0) : 0) : 0) != 0);
			}
			return result;
		}

		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			return new Job(JobDefOf.Repair, t);
		}
	}
}
