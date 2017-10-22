using RimWorld;

namespace Verse.AI
{
	public static class GenPath
	{
		public static TargetInfo ResolvePathMode(Pawn pawn, TargetInfo dest, ref PathEndMode peMode)
		{
			TargetInfo result;
			if (dest.HasThing && !dest.Thing.Spawned)
			{
				peMode = PathEndMode.Touch;
				result = dest;
			}
			else if (peMode == PathEndMode.InteractionCell)
			{
				if (!dest.HasThing)
				{
					Log.Error("Pathed to cell " + dest + " with PathEndMode.InteractionCell.");
				}
				peMode = PathEndMode.OnCell;
				result = new TargetInfo(dest.Thing.InteractionCell, dest.Thing.Map, false);
			}
			else
			{
				if (peMode == PathEndMode.ClosestTouch)
				{
					peMode = GenPath.ResolveClosestTouchPathMode(pawn, dest.Map, dest.Cell);
				}
				result = dest;
			}
			return result;
		}

		public static PathEndMode ResolveClosestTouchPathMode(Pawn pawn, Map map, IntVec3 target)
		{
			return (PathEndMode)((!GenPath.ShouldNotEnterCell(pawn, map, target)) ? 1 : 2);
		}

		private static bool ShouldNotEnterCell(Pawn pawn, Map map, IntVec3 dest)
		{
			bool result;
			if (map.pathGrid.PerceivedPathCostAt(dest) > 30)
			{
				result = true;
			}
			else if (!dest.Walkable(map))
			{
				result = true;
			}
			else
			{
				if (pawn != null)
				{
					if (dest.IsForbidden(pawn))
					{
						result = true;
						goto IL_0095;
					}
					Building edifice = dest.GetEdifice(map);
					if (edifice != null)
					{
						Building_Door building_Door = edifice as Building_Door;
						if (building_Door != null)
						{
							if (building_Door.IsForbidden(pawn))
							{
								result = true;
								goto IL_0095;
							}
							if (!building_Door.PawnCanOpen(pawn))
							{
								result = true;
								goto IL_0095;
							}
						}
					}
				}
				result = false;
			}
			goto IL_0095;
			IL_0095:
			return result;
		}
	}
}
