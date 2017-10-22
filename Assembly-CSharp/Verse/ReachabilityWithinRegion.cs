using Verse.AI;

namespace Verse
{
	public static class ReachabilityWithinRegion
	{
		public static bool ThingFromRegionListerReachable(Thing thing, Region region, PathEndMode peMode, Pawn traveler)
		{
			Map map = region.Map;
			if (peMode == PathEndMode.ClosestTouch)
			{
				peMode = GenPath.ResolveClosestTouchPathMode(traveler, map, thing.Position);
			}
			bool result;
			switch (peMode)
			{
			case PathEndMode.None:
			{
				result = false;
				break;
			}
			case PathEndMode.Touch:
			{
				result = true;
				break;
			}
			case PathEndMode.OnCell:
			{
				if (thing.def.size.x == 1 && thing.def.size.z == 1)
				{
					if (thing.Position.GetRegion(map, RegionType.Set_Passable) == region)
					{
						result = true;
						goto IL_0122;
					}
				}
				else
				{
					CellRect.CellRectIterator iterator = thing.OccupiedRect().GetIterator();
					while (!iterator.Done())
					{
						if (iterator.Current.GetRegion(map, RegionType.Set_Passable) == region)
							goto IL_00c2;
						iterator.MoveNext();
					}
				}
				result = false;
				break;
			}
			case PathEndMode.InteractionCell:
			{
				result = ((byte)((thing.InteractionCell.GetRegion(map, RegionType.Set_Passable) == region) ? 1 : 0) != 0);
				break;
			}
			default:
			{
				Log.Error("Unsupported PathEndMode: " + peMode);
				result = false;
				break;
			}
			}
			goto IL_0122;
			IL_00c2:
			result = true;
			goto IL_0122;
			IL_0122:
			return result;
		}
	}
}
