using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200013D RID: 317
	public class WorkGiver_DeepDrill : WorkGiver_Scanner
	{
		// Token: 0x170000F8 RID: 248
		// (get) Token: 0x06000676 RID: 1654 RVA: 0x0004317C File Offset: 0x0004157C
		public override ThingRequest PotentialWorkThingRequest
		{
			get
			{
				return ThingRequest.ForDef(ThingDefOf.DeepDrill);
			}
		}

		// Token: 0x170000F9 RID: 249
		// (get) Token: 0x06000677 RID: 1655 RVA: 0x0004319C File Offset: 0x0004159C
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.InteractionCell;
			}
		}

		// Token: 0x06000678 RID: 1656 RVA: 0x000431B4 File Offset: 0x000415B4
		public override Danger MaxPathDanger(Pawn pawn)
		{
			return Danger.Deadly;
		}

		// Token: 0x06000679 RID: 1657 RVA: 0x000431CC File Offset: 0x000415CC
		public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
		{
			return pawn.Map.listerBuildings.AllBuildingsColonistOfDef(ThingDefOf.DeepDrill).Cast<Thing>();
		}

		// Token: 0x0600067A RID: 1658 RVA: 0x000431FC File Offset: 0x000415FC
		public override bool ShouldSkip(Pawn pawn, bool forced = false)
		{
			List<Building> allBuildingsColonist = pawn.Map.listerBuildings.allBuildingsColonist;
			for (int i = 0; i < allBuildingsColonist.Count; i++)
			{
				if (allBuildingsColonist[i].def == ThingDefOf.DeepDrill)
				{
					CompPowerTrader comp = allBuildingsColonist[i].GetComp<CompPowerTrader>();
					if (comp == null || comp.PowerOn)
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x0600067B RID: 1659 RVA: 0x0004327C File Offset: 0x0004167C
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
					LocalTargetInfo target = building;
					if (!pawn.CanReserve(target, 1, -1, null, forced))
					{
						result = false;
					}
					else
					{
						CompDeepDrill compDeepDrill = building.TryGetComp<CompDeepDrill>();
						result = (compDeepDrill.CanDrillNow() && !building.IsBurning());
					}
				}
			}
			return result;
		}

		// Token: 0x0600067C RID: 1660 RVA: 0x00043324 File Offset: 0x00041724
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			return new Job(JobDefOf.OperateDeepDrill, t, 1500, true);
		}
	}
}
