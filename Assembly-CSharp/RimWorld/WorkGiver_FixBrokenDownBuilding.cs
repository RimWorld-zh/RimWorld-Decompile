using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class WorkGiver_FixBrokenDownBuilding : WorkGiver_Scanner
	{
		public static string NotInHomeAreaTrans;

		private static string NoComponentsToRepairTrans;

		public WorkGiver_FixBrokenDownBuilding()
		{
		}

		public static void ResetStaticData()
		{
			WorkGiver_FixBrokenDownBuilding.NotInHomeAreaTrans = "NotInHomeArea".Translate();
			WorkGiver_FixBrokenDownBuilding.NoComponentsToRepairTrans = "NoComponentsToRepair".Translate();
		}

		public override ThingRequest PotentialWorkThingRequest
		{
			get
			{
				return ThingRequest.ForGroup(ThingRequestGroup.BuildingArtificial);
			}
		}

		public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
		{
			return pawn.Map.GetComponent<BreakdownManager>().brokenDownThings;
		}

		public override bool ShouldSkip(Pawn pawn, bool forced = false)
		{
			return pawn.Map.GetComponent<BreakdownManager>().brokenDownThings.Count == 0;
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

		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			Thing t2 = this.FindClosestComponent(pawn);
			return new Job(JobDefOf.FixBrokenDownBuilding, t, t2)
			{
				count = 1
			};
		}

		private Thing FindClosestComponent(Pawn pawn)
		{
			return GenClosest.ClosestThingReachable(pawn.Position, pawn.Map, ThingRequest.ForDef(ThingDefOf.ComponentIndustrial), PathEndMode.InteractionCell, TraverseParms.For(pawn, pawn.NormalMaxDanger(), TraverseMode.ByPawn, false), 9999f, (Thing x) => !x.IsForbidden(pawn) && pawn.CanReserve(x, 1, -1, null, false), null, 0, -1, false, RegionType.Set_Passable, false);
		}

		[CompilerGenerated]
		private sealed class <FindClosestComponent>c__AnonStorey0
		{
			internal Pawn pawn;

			public <FindClosestComponent>c__AnonStorey0()
			{
			}

			internal bool <>m__0(Thing x)
			{
				return !x.IsForbidden(this.pawn) && this.pawn.CanReserve(x, 1, -1, null, false);
			}
		}
	}
}
