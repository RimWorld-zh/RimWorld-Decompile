using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020007DC RID: 2012
	public static class SmoothSurfaceDesignatorUtility
	{
		// Token: 0x06002C9A RID: 11418 RVA: 0x00177EE4 File Offset: 0x001762E4
		public static bool CanSmoothFloorUnder(Building b)
		{
			return b.def.Fillage != FillCategory.Full || b.def.passability != Traversability.Impassable;
		}

		// Token: 0x06002C9B RID: 11419 RVA: 0x00177F20 File Offset: 0x00176320
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

		// Token: 0x06002C9C RID: 11420 RVA: 0x00177F98 File Offset: 0x00176398
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
