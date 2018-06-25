using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020008F3 RID: 2291
	public static class AutoHomeAreaMaker
	{
		// Token: 0x04001CA1 RID: 7329
		private const int BorderWidth = 4;

		// Token: 0x060034F8 RID: 13560 RVA: 0x001C5008 File Offset: 0x001C3408
		private static bool ShouldAdd()
		{
			return Find.PlaySettings.autoHomeArea && Current.ProgramState == ProgramState.Playing;
		}

		// Token: 0x060034F9 RID: 13561 RVA: 0x001C5037 File Offset: 0x001C3437
		public static void Notify_BuildingSpawned(Thing b)
		{
			if (AutoHomeAreaMaker.ShouldAdd() && b.def.building.expandHomeArea && b.Faction == Faction.OfPlayer)
			{
				AutoHomeAreaMaker.MarkHomeAroundThing(b);
			}
		}

		// Token: 0x060034FA RID: 13562 RVA: 0x001C5074 File Offset: 0x001C3474
		public static void Notify_BuildingClaimed(Thing b)
		{
			if (AutoHomeAreaMaker.ShouldAdd() && b.def.building.expandHomeArea && b.Faction == Faction.OfPlayer)
			{
				AutoHomeAreaMaker.MarkHomeAroundThing(b);
			}
		}

		// Token: 0x060034FB RID: 13563 RVA: 0x001C50B4 File Offset: 0x001C34B4
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

		// Token: 0x060034FC RID: 13564 RVA: 0x001C51B0 File Offset: 0x001C35B0
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
	}
}
