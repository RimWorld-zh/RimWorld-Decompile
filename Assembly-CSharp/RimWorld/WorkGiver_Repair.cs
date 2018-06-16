using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200015A RID: 346
	public class WorkGiver_Repair : WorkGiver_Scanner
	{
		// Token: 0x17000116 RID: 278
		// (get) Token: 0x0600071F RID: 1823 RVA: 0x00048538 File Offset: 0x00046938
		public override ThingRequest PotentialWorkThingRequest
		{
			get
			{
				return ThingRequest.ForGroup(ThingRequestGroup.BuildingArtificial);
			}
		}

		// Token: 0x17000117 RID: 279
		// (get) Token: 0x06000720 RID: 1824 RVA: 0x00048554 File Offset: 0x00046954
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.Touch;
			}
		}

		// Token: 0x06000721 RID: 1825 RVA: 0x0004856C File Offset: 0x0004696C
		public override Danger MaxPathDanger(Pawn pawn)
		{
			return Danger.Deadly;
		}

		// Token: 0x06000722 RID: 1826 RVA: 0x00048584 File Offset: 0x00046984
		public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
		{
			return pawn.Map.listerBuildingsRepairable.RepairableBuildings(pawn.Faction);
		}

		// Token: 0x06000723 RID: 1827 RVA: 0x000485B0 File Offset: 0x000469B0
		public override bool ShouldSkip(Pawn pawn, bool forced = false)
		{
			return pawn.Map.listerBuildingsRepairable.RepairableBuildings(pawn.Faction).Count == 0;
		}

		// Token: 0x06000724 RID: 1828 RVA: 0x000485E4 File Offset: 0x000469E4
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
			else if (pawn.Faction == Faction.OfPlayer && !pawn.Map.areaManager.Home[t.Position])
			{
				JobFailReason.Is(WorkGiver_FixBrokenDownBuilding.NotInHomeAreaTrans, null);
				result = false;
			}
			else if (!t.def.useHitPoints || t.HitPoints == t.MaxHitPoints)
			{
				result = false;
			}
			else
			{
				LocalTargetInfo target = building;
				result = (pawn.CanReserve(target, 1, -1, null, forced) && building.Map.designationManager.DesignationOn(building, DesignationDefOf.Deconstruct) == null && !building.IsBurning());
			}
			return result;
		}

		// Token: 0x06000725 RID: 1829 RVA: 0x00048728 File Offset: 0x00046B28
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			return new Job(JobDefOf.Repair, t);
		}
	}
}
