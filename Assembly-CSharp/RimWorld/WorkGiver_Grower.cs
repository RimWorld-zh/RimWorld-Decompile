using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public abstract class WorkGiver_Grower : WorkGiver_Scanner
	{
		protected static ThingDef wantedPlantDef;

		protected virtual bool ExtraRequirements(IPlantToGrowSettable settable, Pawn pawn)
		{
			return true;
		}

		public override IEnumerable<IntVec3> PotentialWorkCellsGlobal(Pawn pawn)
		{
			Danger maxDanger = pawn.NormalMaxDanger();
			List<Building> bList = pawn.Map.listerBuildings.allBuildingsColonist;
			for (int k = 0; k < bList.Count; k++)
			{
				Building_PlantGrower b = bList[k] as Building_PlantGrower;
				if (b != null && this.ExtraRequirements(b, pawn) && !b.IsForbidden(pawn) && pawn.CanReach((Thing)b, PathEndMode.OnCell, maxDanger, false, TraverseMode.ByPawn) && !b.IsBurning())
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
			WorkGiver_Grower.wantedPlantDef = null;
			List<Zone> zonesList = pawn.Map.zoneManager.AllZones;
			for (int j = 0; j < zonesList.Count; j++)
			{
				Zone_Growing growZone = zonesList[j] as Zone_Growing;
				if (growZone != null)
				{
					if (growZone.cells.Count == 0)
					{
						Log.ErrorOnce("Grow zone has 0 cells: " + growZone, -563487);
					}
					else if (this.ExtraRequirements(growZone, pawn) && !growZone.ContainsStaticFire && pawn.CanReach(growZone.Cells[0], PathEndMode.OnCell, maxDanger, false, TraverseMode.ByPawn))
					{
						for (int i = 0; i < growZone.cells.Count; i++)
						{
							yield return growZone.cells[i];
						}
						WorkGiver_Grower.wantedPlantDef = null;
					}
				}
			}
			WorkGiver_Grower.wantedPlantDef = null;
		}

		public static ThingDef CalculateWantedPlantDef(IntVec3 c, Map map)
		{
			IPlantToGrowSettable plantToGrowSettable = c.GetPlantToGrowSettable(map);
			if (plantToGrowSettable == null)
			{
				return null;
			}
			return plantToGrowSettable.GetPlantDefToGrow();
		}
	}
}
