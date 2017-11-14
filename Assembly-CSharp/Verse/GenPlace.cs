using RimWorld;
using System;
using System.Collections.Generic;
using Verse.AI;

namespace Verse
{
	public static class GenPlace
	{
		private enum PlaceSpotQuality : byte
		{
			Unusable,
			Awful,
			Bad,
			Okay,
			Perfect
		}

		private static readonly int PlaceNearMaxRadialCells = GenRadial.NumCellsInRadius(12.9f);

		private static readonly int PlaceNearMiddleRadialCells = GenRadial.NumCellsInRadius(3f);

		public static bool TryPlaceThing(Thing thing, IntVec3 center, Map map, ThingPlaceMode mode, Action<Thing, int> placedAction = null)
		{
			Thing thing2 = default(Thing);
			return GenPlace.TryPlaceThing(thing, center, map, mode, out thing2, placedAction);
		}

		public static bool TryPlaceThing(Thing thing, IntVec3 center, Map map, ThingPlaceMode mode, out Thing lastResultingThing, Action<Thing, int> placedAction = null)
		{
			if (map == null)
			{
				Log.Error("Tried to place thing " + thing + " in a null map.");
				lastResultingThing = null;
				return false;
			}
			if (thing.def.category == ThingCategory.Filth)
			{
				mode = ThingPlaceMode.Direct;
			}
			switch (mode)
			{
			case ThingPlaceMode.Direct:
				return GenPlace.TryPlaceDirect(thing, center, map, out lastResultingThing, placedAction);
			case ThingPlaceMode.Near:
			{
				lastResultingThing = null;
				int num = -1;
				while (true)
				{
					num = thing.stackCount;
					IntVec3 loc = default(IntVec3);
					if (!GenPlace.TryFindPlaceSpotNear(center, map, thing, true, out loc))
					{
						return false;
					}
					if (GenPlace.TryPlaceDirect(thing, loc, map, out lastResultingThing, placedAction))
					{
						return true;
					}
					if (thing.stackCount == num)
						break;
				}
				Log.Error("Failed to place " + thing + " at " + center + " in mode " + mode + ".");
				lastResultingThing = null;
				return false;
			}
			default:
				throw new InvalidOperationException();
			}
		}

		public static bool TryMoveThing(Thing thing, IntVec3 loc, Map map)
		{
			IntVec3 position = default(IntVec3);
			if (!GenPlace.TryFindPlaceSpotNear(loc, map, thing, false, out position))
			{
				return false;
			}
			thing.Position = position;
			return true;
		}

		private static bool TryFindPlaceSpotNear(IntVec3 center, Map map, Thing thing, bool allowStacking, out IntVec3 bestSpot)
		{
			PlaceSpotQuality placeSpotQuality = PlaceSpotQuality.Unusable;
			bestSpot = center;
			int num = 0;
			while (num < 9)
			{
				IntVec3 intVec = center + GenRadial.RadialPattern[num];
				PlaceSpotQuality placeSpotQuality2 = GenPlace.PlaceSpotQualityAt(intVec, map, thing, center, allowStacking);
				if ((int)placeSpotQuality2 > (int)placeSpotQuality)
				{
					bestSpot = intVec;
					placeSpotQuality = placeSpotQuality2;
				}
				if (placeSpotQuality != PlaceSpotQuality.Perfect)
				{
					num++;
					continue;
				}
				break;
			}
			if ((int)placeSpotQuality >= 3)
			{
				return true;
			}
			int num2 = 0;
			while (num2 < GenPlace.PlaceNearMiddleRadialCells)
			{
				IntVec3 intVec = center + GenRadial.RadialPattern[num2];
				PlaceSpotQuality placeSpotQuality2 = GenPlace.PlaceSpotQualityAt(intVec, map, thing, center, allowStacking);
				if ((int)placeSpotQuality2 > (int)placeSpotQuality)
				{
					bestSpot = intVec;
					placeSpotQuality = placeSpotQuality2;
				}
				if (placeSpotQuality != PlaceSpotQuality.Perfect)
				{
					num2++;
					continue;
				}
				break;
			}
			if ((int)placeSpotQuality >= 3)
			{
				return true;
			}
			int num3 = 0;
			while (num3 < GenPlace.PlaceNearMaxRadialCells)
			{
				IntVec3 intVec = center + GenRadial.RadialPattern[num3];
				PlaceSpotQuality placeSpotQuality2 = GenPlace.PlaceSpotQualityAt(intVec, map, thing, center, allowStacking);
				if ((int)placeSpotQuality2 > (int)placeSpotQuality)
				{
					bestSpot = intVec;
					placeSpotQuality = placeSpotQuality2;
				}
				if (placeSpotQuality != PlaceSpotQuality.Perfect)
				{
					num3++;
					continue;
				}
				break;
			}
			if ((int)placeSpotQuality > 0)
			{
				return true;
			}
			bestSpot = center;
			return false;
		}

		private static PlaceSpotQuality PlaceSpotQualityAt(IntVec3 c, Map map, Thing thing, IntVec3 center, bool allowStacking)
		{
			if (c.InBounds(map) && c.Walkable(map))
			{
				List<Thing> list = map.thingGrid.ThingsListAt(c);
				for (int i = 0; i < list.Count; i++)
				{
					Thing thing2 = list[i];
					if (thing.def.saveCompressible && thing2.def.saveCompressible)
					{
						return PlaceSpotQuality.Unusable;
					}
					if (thing.def.category == ThingCategory.Item && thing2.def.category == ThingCategory.Item && (!thing2.CanStackWith(thing) || thing2.stackCount >= thing.def.stackLimit))
					{
						return PlaceSpotQuality.Unusable;
					}
				}
				if (c.GetRoom(map, RegionType.Set_Passable) != center.GetRoom(map, RegionType.Set_Passable))
				{
					if (!map.reachability.CanReach(center, c, PathEndMode.OnCell, TraverseMode.PassDoors, Danger.Deadly))
					{
						return PlaceSpotQuality.Awful;
					}
					return PlaceSpotQuality.Bad;
				}
				if (allowStacking)
				{
					for (int j = 0; j < list.Count; j++)
					{
						Thing thing3 = list[j];
						if (thing3.def.category == ThingCategory.Item && thing3.CanStackWith(thing) && thing3.stackCount < thing.def.stackLimit)
						{
							return PlaceSpotQuality.Perfect;
						}
					}
				}
				Pawn pawn = thing as Pawn;
				bool flag = pawn != null && pawn.Downed;
				PlaceSpotQuality placeSpotQuality = PlaceSpotQuality.Perfect;
				for (int k = 0; k < list.Count; k++)
				{
					Thing thing4 = list[k];
					if (thing4.def.IsDoor)
					{
						return PlaceSpotQuality.Bad;
					}
					if (thing4 is Building_WorkTable)
					{
						return PlaceSpotQuality.Bad;
					}
					Pawn pawn2 = thing4 as Pawn;
					if (pawn2 != null)
					{
						if (pawn2.Downed || flag)
						{
							return PlaceSpotQuality.Bad;
						}
						if ((int)placeSpotQuality > 3)
						{
							placeSpotQuality = PlaceSpotQuality.Okay;
						}
					}
					if (thing4.def.category == ThingCategory.Plant && thing4.def.selectable && (int)placeSpotQuality > 3)
					{
						placeSpotQuality = PlaceSpotQuality.Okay;
					}
				}
				return placeSpotQuality;
			}
			return PlaceSpotQuality.Unusable;
		}

		private static bool TryPlaceDirect(Thing thing, IntVec3 loc, Map map, out Thing resultingThing, Action<Thing, int> placedAction = null)
		{
			Thing thing2 = thing;
			bool flag = false;
			if (thing.stackCount > thing.def.stackLimit)
			{
				thing = thing.SplitOff(thing.def.stackLimit);
				flag = true;
			}
			Thing thing3;
			if (thing.def.stackLimit > 1)
			{
				List<Thing> thingList = loc.GetThingList(map);
				for (int i = 0; i < thingList.Count; i++)
				{
					thing3 = thingList[i];
					if (thing3.CanStackWith(thing))
						goto IL_006a;
				}
			}
			resultingThing = GenSpawn.Spawn(thing, loc, map);
			if (placedAction != null)
			{
				placedAction(thing, thing.stackCount);
			}
			return !flag;
			IL_006a:
			int stackCount = thing.stackCount;
			if (thing3.TryAbsorbStack(thing, true))
			{
				resultingThing = thing3;
				if (placedAction != null)
				{
					placedAction(thing3, stackCount);
				}
				return !flag;
			}
			resultingThing = null;
			if (placedAction != null && stackCount != thing.stackCount)
			{
				placedAction(thing3, stackCount - thing.stackCount);
			}
			if (thing2 != thing)
			{
				thing2.TryAbsorbStack(thing, false);
			}
			return false;
		}

		public static Thing HaulPlaceBlockerIn(Thing haulThing, IntVec3 c, Map map, bool checkBlueprints)
		{
			List<Thing> list = map.thingGrid.ThingsListAt(c);
			for (int i = 0; i < list.Count; i++)
			{
				Thing thing = list[i];
				if (checkBlueprints && thing.def.IsBlueprint)
				{
					return thing;
				}
				if ((thing.def.category != ThingCategory.Plant || thing.def.passability != 0) && thing.def.category != ThingCategory.Filth && (haulThing == null || thing.def.category != ThingCategory.Item || !thing.CanStackWith(haulThing) || thing.def.stackLimit - thing.stackCount < haulThing.stackCount))
				{
					if (thing.def.EverHaulable)
					{
						return thing;
					}
					if (haulThing != null && GenSpawn.SpawningWipes(haulThing.def, thing.def))
					{
						return thing;
					}
					if (thing.def.passability != 0 && thing.def.surfaceType != SurfaceType.Item)
					{
						return thing;
					}
				}
			}
			return null;
		}
	}
}
