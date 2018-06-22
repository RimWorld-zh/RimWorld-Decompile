using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020007DA RID: 2010
	public static class SmoothSurfaceDesignatorUtility
	{
		// Token: 0x06002C97 RID: 11415 RVA: 0x00177B30 File Offset: 0x00175F30
		public static bool CanSmoothFloorUnder(Building b)
		{
			return b.def.Fillage != FillCategory.Full || b.def.passability != Traversability.Impassable;
		}

		// Token: 0x06002C98 RID: 11416 RVA: 0x00177B6C File Offset: 0x00175F6C
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

		// Token: 0x06002C99 RID: 11417 RVA: 0x00177BE4 File Offset: 0x00175FE4
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
