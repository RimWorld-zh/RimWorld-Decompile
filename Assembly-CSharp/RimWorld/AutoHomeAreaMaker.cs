using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020008F1 RID: 2289
	public static class AutoHomeAreaMaker
	{
		// Token: 0x060034F4 RID: 13556 RVA: 0x001C4BF4 File Offset: 0x001C2FF4
		private static bool ShouldAdd()
		{
			return Find.PlaySettings.autoHomeArea && Current.ProgramState == ProgramState.Playing;
		}

		// Token: 0x060034F5 RID: 13557 RVA: 0x001C4C23 File Offset: 0x001C3023
		public static void Notify_BuildingSpawned(Thing b)
		{
			if (AutoHomeAreaMaker.ShouldAdd() && b.def.building.expandHomeArea && b.Faction == Faction.OfPlayer)
			{
				AutoHomeAreaMaker.MarkHomeAroundThing(b);
			}
		}

		// Token: 0x060034F6 RID: 13558 RVA: 0x001C4C60 File Offset: 0x001C3060
		public static void Notify_BuildingClaimed(Thing b)
		{
			if (AutoHomeAreaMaker.ShouldAdd() && b.def.building.expandHomeArea && b.Faction == Faction.OfPlayer)
			{
				AutoHomeAreaMaker.MarkHomeAroundThing(b);
			}
		}

		// Token: 0x060034F7 RID: 13559 RVA: 0x001C4CA0 File Offset: 0x001C30A0
		public static void MarkHomeAroundThing(Thing t)
		{
			if (AutoHomeAreaMaker.ShouldAdd())
			{
				CellRect cellRect = new CellRect(t.Position.x - t.RotatedSize.x / 2 - 4, t.Position.z - t.RotatedSize.z / 2 - 4, t.RotatedSize.x + 8, t.RotatedSize.z + 8);
				cellRect.ClipInsideMap(t.Map);
				foreach (IntVec3 c in cellRect)
				{
					t.Map.areaManager.Home[c] = true;
				}
			}
		}

		// Token: 0x060034F8 RID: 13560 RVA: 0x001C4D9C File Offset: 0x001C319C
		public static void Notify_ZoneCellAdded(IntVec3 c, Zone zone)
		{
			if (AutoHomeAreaMaker.ShouldAdd())
			{
				CellRect.CellRectIterator iterator = CellRect.CenteredOn(c, 4).ClipInsideMap(zone.Map).GetIterator();
				while (!iterator.Done())
				{
					zone.Map.areaManager.Home[iterator.Current] = true;
					iterator.MoveNext();
				}
			}
		}

		// Token: 0x04001C9B RID: 7323
		private const int BorderWidth = 4;
	}
}
