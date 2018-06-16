using System;
using System.Collections.Generic;
using UnityEngine.Profiling;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000145 RID: 325
	public abstract class WorkGiver_Grower : WorkGiver_Scanner
	{
		// Token: 0x17000106 RID: 262
		// (get) Token: 0x060006BE RID: 1726 RVA: 0x000454E0 File Offset: 0x000438E0
		public override bool AllowUnreachable
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060006BF RID: 1727 RVA: 0x000454F8 File Offset: 0x000438F8
		protected virtual bool ExtraRequirements(IPlantToGrowSettable settable, Pawn pawn)
		{
			return true;
		}

		// Token: 0x060006C0 RID: 1728 RVA: 0x00045510 File Offset: 0x00043910
		public override IEnumerable<IntVec3> PotentialWorkCellsGlobal(Pawn pawn)
		{
			Profiler.BeginSample("Grow find cell");
			Danger maxDanger = pawn.NormalMaxDanger();
			List<Building> bList = pawn.Map.listerBuildings.allBuildingsColonist;
			for (int i = 0; i < bList.Count; i++)
			{
				Building_PlantGrower b = bList[i] as Building_PlantGrower;
				if (b != null)
				{
					if (this.ExtraRequirements(b, pawn))
					{
						if (!b.IsForbidden(pawn))
						{
							if (pawn.CanReach(b, PathEndMode.OnCell, maxDanger, false, TraverseMode.ByPawn))
							{
								if (!b.IsBurning())
								{
									CellRect.CellRectIterator cri = b.OccupiedRect().GetIterator();
									while (!cri.Done())
									{
										yield return cri.Current;
										cri.MoveNext();
									}
									WorkGiver_Grower.wantedPlantDef = null;
								}
							}
						}
					}
				}
			}
			WorkGiver_Grower.wantedPlantDef = null;
			List<Zone> zonesList = pawn.Map.zoneManager.AllZones;
			for (int j = 0; j < zonesList.Count; j++)
			{
				Zone_Growing growZone = zonesList[j] as Zone_Growing;
				if (growZone != null)
				{
					if (growZone.cells.Count == 0)
					{
						Log.ErrorOnce("Grow zone has 0 cells: " + growZone, -563487, false);
					}
					else if (this.ExtraRequirements(growZone, pawn))
					{
						if (!growZone.ContainsStaticFire)
						{
							if (pawn.CanReach(growZone.Cells[0], PathEndMode.OnCell, maxDanger, false, TraverseMode.ByPawn))
							{
								for (int k = 0; k < growZone.cells.Count; k++)
								{
									yield return growZone.cells[k];
								}
								WorkGiver_Grower.wantedPlantDef = null;
							}
						}
					}
				}
			}
			WorkGiver_Grower.wantedPlantDef = null;
			Profiler.EndSample();
			yield break;
		}

		// Token: 0x060006C1 RID: 1729 RVA: 0x00045544 File Offset: 0x00043944
		public static ThingDef CalculateWantedPlantDef(IntVec3 c, Map map)
		{
			IPlantToGrowSettable plantToGrowSettable = c.GetPlantToGrowSettable(map);
			ThingDef result;
			if (plantToGrowSettable == null)
			{
				result = null;
			}
			else
			{
				result = plantToGrowSettable.GetPlantDefToGrow();
			}
			return result;
		}

		// Token: 0x04000329 RID: 809
		protected static ThingDef wantedPlantDef = null;
	}
}
