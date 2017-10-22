using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class WorkGiver_DeepDrill : WorkGiver_Scanner
	{
		public override ThingRequest PotentialWorkThingRequest
		{
			get
			{
				return ThingRequest.ForDef(ThingDefOf.DeepDrill);
			}
		}

		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.InteractionCell;
			}
		}

		public override Danger MaxPathDanger(Pawn pawn)
		{
			return Danger.Deadly;
		}

		public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
		{
			return pawn.Map.listerBuildings.AllBuildingsColonistOfDef(ThingDefOf.DeepDrill).Cast<Thing>();
		}

		public override bool ShouldSkip(Pawn pawn)
		{
			List<Building> allBuildingsColonist = pawn.Map.listerBuildings.allBuildingsColonist;
			int num = 0;
			bool result;
			while (true)
			{
				if (num < allBuildingsColonist.Count)
				{
					if (allBuildingsColonist[num].def == ThingDefOf.DeepDrill)
					{
						CompPowerTrader comp = allBuildingsColonist[num].GetComp<CompPowerTrader>();
						if (comp != null && !comp.PowerOn)
						{
							goto IL_0057;
						}
						result = false;
						break;
					}
					goto IL_0057;
				}
				result = true;
				break;
				IL_0057:
				num++;
			}
			return result;
		}

		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			bool result;
			if (t.Faction != pawn.Faction)
			{
				result = false;
			}
			else
			{
				Building building = t as Building;
				if (building == null)
				{
					result = false;
				}
				else if (building.IsForbidden(pawn))
				{
					result = false;
				}
				else
				{
					LocalTargetInfo target = (Thing)building;
					if (!pawn.CanReserve(target, 1, -1, null, forced))
					{
						result = false;
					}
					else
					{
						CompDeepDrill compDeepDrill = building.TryGetComp<CompDeepDrill>();
						result = ((byte)(compDeepDrill.CanDrillNow() ? ((!building.IsBurning()) ? 1 : 0) : 0) != 0);
					}
				}
			}
			return result;
		}

		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			return new Job(JobDefOf.OperateDeepDrill, t, 1500, true);
		}
	}
}
