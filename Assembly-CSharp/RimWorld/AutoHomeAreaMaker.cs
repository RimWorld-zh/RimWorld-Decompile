using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020008F5 RID: 2293
	public static class AutoHomeAreaMaker
	{
		// Token: 0x060034FB RID: 13563 RVA: 0x001C4A0C File Offset: 0x001C2E0C
		private static bool ShouldAdd()
		{
			return Find.PlaySettings.autoHomeArea && Current.ProgramState == ProgramState.Playing;
		}

		// Token: 0x060034FC RID: 13564 RVA: 0x001C4A3B File Offset: 0x001C2E3B
		public static void Notify_BuildingSpawned(Thing b)
		{
			if (AutoHomeAreaMaker.ShouldAdd() && b.def.building.expandHomeArea && b.Faction == Faction.OfPlayer)
			{
				AutoHomeAreaMaker.MarkHomeAroundThing(b);
			}
		}

		// Token: 0x060034FD RID: 13565 RVA: 0x001C4A78 File Offset: 0x001C2E78
		public static void Notify_BuildingClaimed(Thing b)
		{
			if (AutoHomeAreaMaker.ShouldAdd() && b.def.building.expandHomeArea && b.Faction == Faction.OfPlayer)
			{
				AutoHomeAreaMaker.MarkHomeAroundThing(b);
			}
		}

		// Token: 0x060034FE RID: 13566 RVA: 0x001C4AB8 File Offset: 0x001C2EB8
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

		// Token: 0x060034FF RID: 13567 RVA: 0x001C4BB4 File Offset: 0x001C2FB4
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

		// Token: 0x04001C9D RID: 7325
		private const int BorderWidth = 4;
	}
}
