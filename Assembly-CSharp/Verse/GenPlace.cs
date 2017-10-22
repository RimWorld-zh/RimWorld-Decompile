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
			Unusable = 0,
			Awful = 1,
			Bad = 2,
			Okay = 3,
			Perfect = 4
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
			bool result;
			if (map == null)
			{
				Log.Error("Tried to place thing " + thing + " in a null map.");
				lastResultingThing = null;
				result = false;
			}
			else
			{
				if (thing.def.category == ThingCategory.Filth)
				{
					mode = ThingPlaceMode.Direct;
				}
				switch (mode)
				{
				case ThingPlaceMode.Direct:
				{
					result = GenPlace.TryPlaceDirect(thing, center, map, out lastResultingThing, placedAction);
					break;
				}
				case ThingPlaceMode.Near:
				{
					lastResultingThing = null;
					int num = -1;
					while (true)
					{
						num = thing.stackCount;
						IntVec3 loc = default(IntVec3);
						if (GenPlace.TryFindPlaceSpotNear(center, map, thing, true, out loc))
						{
							if (GenPlace.TryPlaceDirect(thing, loc, map, out lastResultingThing, placedAction))
								goto IL_0093;
							if (thing.stackCount != num)
								continue;
							goto IL_00a6;
						}
						break;
					}
					result = false;
					break;
				}
				default:
				{
					throw new InvalidOperationException();
				}
				}
			}
			goto IL_0104;
			IL_00a6:
			Log.Error("Failed to place " + thing + " at " + center + " in mode " + mode + ".");
			lastResultingThing = null;
			result = false;
			goto IL_0104;
			IL_0093:
			result = true;
			goto IL_0104;
			IL_0104:
			return result;
		}

		public static bool TryMoveThing(Thing thing, IntVec3 loc, Map map)
		{
			IntVec3 position = default(IntVec3);
			bool result;
			if (!GenPlace.TryFindPlaceSpotNear(loc, map, thing, false, out position))
			{
				result = false;
			}
			else
			{
				thing.Position = position;
				result = true;
			}
			return result;
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
			bool result;
			if ((int)placeSpotQuality >= 3)
			{
				result = true;
			}
			else
			{
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
					result = true;
				}
				else
				{
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
						result = true;
					}
					else
					{
						bestSpot = center;
						result = false;
					}
				}
			}
			return result;
		}

		private static PlaceSpotQuality PlaceSpotQualityAt(IntVec3 c, Map map, Thing thing, IntVec3 center, bool allowStacking)
		{
			PlaceSpotQuality result;
			if (!c.InBounds(map) || !c.Walkable(map))
			{
				result = PlaceSpotQuality.Unusable;
			}
			else
			{
				List<Thing> list = map.thingGrid.ThingsListAt(c);
				for (int i = 0; i < list.Count; i++)
				{
					Thing thing2 = list[i];
					if (thing.def.saveCompressible && thing2.def.saveCompressible)
						goto IL_005d;
					if (thing.def.category == ThingCategory.Item && thing2.def.category == ThingCategory.Item && (!thing2.CanStackWith(thing) || thing2.stackCount >= thing.def.stackLimit))
					{
						goto IL_00a9;
					}
				}
				if (c.GetRoom(map, RegionType.Set_Passable) != center.GetRoom(map, RegionType.Set_Passable))
				{
					result = (PlaceSpotQuality)((!map.reachability.CanReach(center, c, PathEndMode.OnCell, TraverseMode.PassDoors, Danger.Deadly)) ? 1 : 2);
				}
				else
				{
					if (allowStacking)
					{
						for (int j = 0; j < list.Count; j++)
						{
							Thing thing3 = list[j];
							if (thing3.def.category == ThingCategory.Item && thing3.CanStackWith(thing) && thing3.stackCount < thing.def.stackLimit)
								goto IL_0153;
						}
					}
					Pawn pawn = thing as Pawn;
					bool flag = pawn != null && pawn.Downed;
					PlaceSpotQuality placeSpotQuality = PlaceSpotQuality.Perfect;
					for (int k = 0; k < list.Count; k++)
					{
						Thing thing4 = list[k];
						if (thing4.def.IsDoor)
							goto IL_01b2;
						if (thing4 is Building_WorkTable)
							goto IL_01c5;
						Pawn pawn2 = thing4 as Pawn;
						if (pawn2 != null)
						{
							if (pawn2.Downed || flag)
							{
								goto IL_01f0;
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
					result = placeSpotQuality;
				}
			}
			goto IL_0253;
			IL_005d:
			result = PlaceSpotQuality.Unusable;
			goto IL_0253;
			IL_01b2:
			result = PlaceSpotQuality.Bad;
			goto IL_0253;
			IL_01f0:
			result = PlaceSpotQuality.Bad;
			goto IL_0253;
			IL_01c5:
			result = PlaceSpotQuality.Bad;
			goto IL_0253;
			IL_0153:
			result = PlaceSpotQuality.Perfect;
			goto IL_0253;
			IL_0253:
			return result;
			IL_00a9:
			result = PlaceSpotQuality.Unusable;
			goto IL_0253;
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
						goto IL_006f;
				}
			}
			resultingThing = GenSpawn.Spawn(thing, loc, map);
			if ((object)placedAction != null)
			{
				placedAction(thing, thing.stackCount);
			}
			bool result = !flag;
			goto IL_0124;
			IL_006f:
			int stackCount = thing.stackCount;
			if (thing3.TryAbsorbStack(thing, true))
			{
				resultingThing = thing3;
				if ((object)placedAction != null)
				{
					placedAction(thing3, stackCount);
				}
				result = !flag;
			}
			else
			{
				resultingThing = null;
				if ((object)placedAction != null && stackCount != thing.stackCount)
				{
					placedAction(thing3, stackCount - thing.stackCount);
				}
				if (thing2 != thing)
				{
					thing2.TryAbsorbStack(thing, false);
				}
				result = false;
			}
			goto IL_0124;
			IL_0124:
			return result;
		}

		public static Thing HaulPlaceBlockerIn(Thing haulThing, IntVec3 c, Map map, bool checkBlueprints)
		{
			List<Thing> list = map.thingGrid.ThingsListAt(c);
			int num = 0;
			Thing result;
			while (true)
			{
				if (num < list.Count)
				{
					Thing thing = list[num];
					if (checkBlueprints && thing.def.IsBlueprint)
					{
						result = thing;
						break;
					}
					if (((thing.def.category != ThingCategory.Plant) ? Traversability.PassThroughOnly : thing.def.passability) != 0 && thing.def.category != ThingCategory.Filth && (haulThing == null || thing.def.category != ThingCategory.Item || !thing.CanStackWith(haulThing) || thing.def.stackLimit - thing.stackCount < haulThing.stackCount))
					{
						if (thing.def.EverHaulable)
						{
							result = thing;
							break;
						}
						if (haulThing != null && GenSpawn.SpawningWipes(haulThing.def, thing.def))
						{
							result = thing;
							break;
						}
						if (thing.def.passability != 0 && thing.def.surfaceType != SurfaceType.Item)
						{
							result = thing;
							break;
						}
					}
					num++;
					continue;
				}
				result = null;
				break;
			}
			return result;
		}
	}
}
