using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020007DC RID: 2012
	public static class SmoothSurfaceDesignatorUtility
	{
		// Token: 0x06002C9B RID: 11419 RVA: 0x00177C80 File Offset: 0x00176080
		public static bool CanSmoothFloorUnder(Building b)
		{
			return b.def.Fillage != FillCategory.Full || b.def.passability != Traversability.Impassable;
		}

		// Token: 0x06002C9C RID: 11420 RVA: 0x00177CBC File Offset: 0x001760BC
		public static void Notify_BuildingSpawned(Building b)
		{
			if (!SmoothSurfaceDesignatorUtility.CanSmoothFloorUnder(b))
			{
				CellRect.CellRectIterator iterator = b.OccupiedRect().GetIterator();
				while (!iterator.Done())
				{
					Designation designation = b.Map.designationManager.DesignationAt(iterator.Current, DesignationDefOf.SmoothFloor);
					if (designation != null)
					{
						b.Map.designationManager.RemoveDesignation(designation);
					}
					iterator.MoveNext();
				}
			}
		}

		// Token: 0x06002C9D RID: 11421 RVA: 0x00177D34 File Offset: 0x00176134
		public static void Notify_BuildingDespawned(Building b, Map map)
		{
			CellRect.CellRectIterator iterator = b.OccupiedRect().GetIterator();
			while (!iterator.Done())
			{
				Designation designation = map.designationManager.DesignationAt(iterator.Current, DesignationDefOf.SmoothWall);
				if (designation != null)
				{
					map.designationManager.RemoveDesignation(designation);
				}
				iterator.MoveNext();
			}
		}
	}
}
