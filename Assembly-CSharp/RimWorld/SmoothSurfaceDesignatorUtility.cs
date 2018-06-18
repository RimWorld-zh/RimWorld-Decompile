using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020007DE RID: 2014
	public static class SmoothSurfaceDesignatorUtility
	{
		// Token: 0x06002C9E RID: 11422 RVA: 0x00177958 File Offset: 0x00175D58
		public static bool CanSmoothFloorUnder(Building b)
		{
			return b.def.Fillage != FillCategory.Full || b.def.passability != Traversability.Impassable;
		}

		// Token: 0x06002C9F RID: 11423 RVA: 0x00177994 File Offset: 0x00175D94
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

		// Token: 0x06002CA0 RID: 11424 RVA: 0x00177A0C File Offset: 0x00175E0C
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
