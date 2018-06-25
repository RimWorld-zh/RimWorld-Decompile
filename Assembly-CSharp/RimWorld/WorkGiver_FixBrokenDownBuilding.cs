using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000144 RID: 324
	public class WorkGiver_FixBrokenDownBuilding : WorkGiver_Scanner
	{
		// Token: 0x04000328 RID: 808
		public static string NotInHomeAreaTrans;

		// Token: 0x04000329 RID: 809
		private static string NoComponentsToRepairTrans;

		// Token: 0x060006B3 RID: 1715 RVA: 0x000451B8 File Offset: 0x000435B8
		public static void ResetStaticData()
		{
			WorkGiver_FixBrokenDownBuilding.NotInHomeAreaTrans = "NotInHomeArea".Translate();
			WorkGiver_FixBrokenDownBuilding.NoComponentsToRepairTrans = "NoComponentsToRepair".Translate();
		}

		// Token: 0x17000104 RID: 260
		// (get) Token: 0x060006B4 RID: 1716 RVA: 0x000451DC File Offset: 0x000435DC
		public override ThingRequest PotentialWorkThingRequest
		{
			get
			{
				return ThingRequest.ForGroup(ThingRequestGroup.BuildingArtificial);
			}
		}

		// Token: 0x060006B5 RID: 1717 RVA: 0x000451F8 File Offset: 0x000435F8
		public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
		{
			return pawn.Map.GetComponent<BreakdownManager>().brokenDownThings;
		}

		// Token: 0x060006B6 RID: 1718 RVA: 0x00045220 File Offset: 0x00043620
		public override bool ShouldSkip(Pawn pawn, bool forced = false)
		{
			return pawn.Map.GetComponent<BreakdownManager>().brokenDownThings.Count == 0;
		}

		// Token: 0x17000105 RID: 261
		// (get) Token: 0x060006B7 RID: 1719 RVA: 0x00045250 File Offset: 0x00043650
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.Touch;
			}
		}

		// Token: 0x060006B8 RID: 1720 RVA: 0x00045268 File Offset: 0x00043668
		public override Danger MaxPathDanger(Pawn pawn)
		{
			return Danger.Deadly;
		}

		// Token: 0x060006B9 RID: 1721 RVA: 0x00045280 File Offset: 0x00043680
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			Building building = t as Building;
			bool result;
			if (building == null)
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
			else if (t.IsForbidden(pawn))
			{
				result = false;
			}
			else if (!t.IsBrokenDown())
			{
				result = false;
			}
			else if (pawn.Faction == Faction.OfPlayer && !pawn.Map.areaManager.Home[t.Position])
			{
				JobFailReason.Is(WorkGiver_FixBrokenDownBuilding.NotInHomeAreaTrans, null);
				result = false;
			}
			else
			{
				LocalTargetInfo target = building;
				if (!pawn.CanReserve(target, 1, -1, null, forced))
				{
					result = false;
				}
				else if (pawn.Map.designationManager.DesignationOn(building, DesignationDefOf.Deconstruct) != null)
				{
					result = false;
				}
				else if (building.IsBurning())
				{
					result = false;
				}
				else if (this.FindClosestComponent(pawn) == null)
				{
					JobFailReason.Is(WorkGiver_FixBrokenDownBuilding.NoComponentsToRepairTrans, null);
					result = false;
				}
				else
				{
					result = true;
				}
			}
			return result;
		}

		// Token: 0x060006BA RID: 1722 RVA: 0x000453C4 File Offset: 0x000437C4
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			Thing t2 = this.FindClosestComponent(pawn);
			return new Job(JobDefOf.FixBrokenDownBuilding, t, t2)
			{
				count = 1
			};
		}

		// Token: 0x060006BB RID: 1723 RVA: 0x00045400 File Offset: 0x00043800
		private Thing FindClosestComponent(Pawn pawn)
		{
			return GenClosest.ClosestThingReachable(pawn.Position, pawn.Map, ThingRequest.ForDef(ThingDefOf.ComponentIndustrial), PathEndMode.InteractionCell, TraverseParms.For(pawn, pawn.NormalMaxDanger(), TraverseMode.ByPawn, false), 9999f, (Thing x) => !x.IsForbidden(pawn) && pawn.CanReserve(x, 1, -1, null, false), null, 0, -1, false, RegionType.Set_Passable, false);
		}
	}
}
