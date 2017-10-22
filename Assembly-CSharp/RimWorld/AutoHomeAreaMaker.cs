using Verse;

namespace RimWorld
{
	public static class AutoHomeAreaMaker
	{
		private const int BorderWidth = 4;

		private static bool ShouldAdd()
		{
			return Find.PlaySettings.autoHomeArea && Current.ProgramState == ProgramState.Playing;
		}

		public static void Notify_BuildingSpawned(Thing b)
		{
			if (AutoHomeAreaMaker.ShouldAdd() && b.def.building.expandHomeArea && b.Faction == Faction.OfPlayer)
			{
				IntVec3 position = b.Position;
				int x = position.x;
				IntVec2 rotatedSize = b.RotatedSize;
				int minX = x - rotatedSize.x / 2 - 4;
				IntVec3 position2 = b.Position;
				int z = position2.z;
				IntVec2 rotatedSize2 = b.RotatedSize;
				int minZ = z - rotatedSize2.z / 2 - 4;
				IntVec2 rotatedSize3 = b.RotatedSize;
				int width = rotatedSize3.x + 8;
				IntVec2 rotatedSize4 = b.RotatedSize;
				CellRect cellRect = new CellRect(minX, minZ, width, rotatedSize4.z + 8);
				cellRect.ClipInsideMap(b.Map);
				foreach (IntVec3 item in cellRect)
				{
					((Area)b.Map.areaManager.Home)[item] = true;
				}
			}
		}

		public static void Notify_ZoneCellAdded(IntVec3 c, Zone zone)
		{
			if (AutoHomeAreaMaker.ShouldAdd())
			{
				CellRect.CellRectIterator iterator = CellRect.CenteredOn(c, 4).ClipInsideMap(zone.Map).GetIterator();
				while (!iterator.Done())
				{
					((Area)zone.Map.areaManager.Home)[iterator.Current] = true;
					iterator.MoveNext();
				}
			}
		}
	}
}
