using System;
using System.Collections.Generic;
using RimWorld;
using Verse.AI;

namespace Verse
{
	// Token: 0x02000F52 RID: 3922
	public static class GenPlace
	{
		// Token: 0x04003E3E RID: 15934
		private static readonly int PlaceNearMaxRadialCells = GenRadial.NumCellsInRadius(12.9f);

		// Token: 0x04003E3F RID: 15935
		private static readonly int PlaceNearMiddleRadialCells = GenRadial.NumCellsInRadius(3f);

		// Token: 0x06005ED0 RID: 24272 RVA: 0x00304918 File Offset: 0x00302D18
		public static bool TryPlaceThing(Thing thing, IntVec3 center, Map map, ThingPlaceMode mode, Action<Thing, int> placedAction = null, Predicate<IntVec3> nearPlaceValidator = null)
		{
			Thing thing2;
			return GenPlace.TryPlaceThing(thing, center, map, mode, out thing2, placedAction, nearPlaceValidator);
		}

		// Token: 0x06005ED1 RID: 24273 RVA: 0x0030493C File Offset: 0x00302D3C
		public static bool TryPlaceThing(Thing thing, IntVec3 center, Map map, ThingPlaceMode mode, out Thing lastResultingThing, Action<Thing, int> placedAction = null, Predicate<IntVec3> nearPlaceValidator = null)
		{
			bool result;
			if (map == null)
			{
				Log.Error("Tried to place thing " + thing + " in a null map.", false);
				lastResultingThing = null;
				result = false;
			}
			else
			{
				if (thing.def.category == ThingCategory.Filth)
				{
					mode = ThingPlaceMode.Direct;
				}
				if (mode == ThingPlaceMode.Direct)
				{
					result = GenPlace.TryPlaceDirect(thing, center, map, out lastResultingThing, placedAction);
				}
				else
				{
					if (mode != ThingPlaceMode.Near)
					{
						throw new InvalidOperationException();
					}
					lastResultingThing = null;
					for (;;)
					{
						int stackCount = thing.stackCount;
						IntVec3 loc;
						if (!GenPlace.TryFindPlaceSpotNear(center, map, thing, true, out loc, nearPlaceValidator))
						{
							break;
						}
						if (GenPlace.TryPlaceDirect(thing, loc, map, out lastResultingThing, placedAction))
						{
							goto Block_6;
						}
						if (thing.stackCount == stackCount)
						{
							goto Block_7;
						}
					}
					return false;
					Block_6:
					return true;
					Block_7:
					Log.Error(string.Concat(new object[]
					{
						"Failed to place ",
						thing,
						" at ",
						center,
						" in mode ",
						mode,
						"."
					}), false);
					lastResultingThing = null;
					result = false;
				}
			}
			return result;
		}

		// Token: 0x06005ED2 RID: 24274 RVA: 0x00304A54 File Offset: 0x00302E54
		private static bool TryFindPlaceSpotNear(IntVec3 center, Map map, Thing thing, bool allowStacking, out IntVec3 bestSpot, Predicate<IntVec3> extraValidator = null)
		{
			GenPlace.PlaceSpotQuality placeSpotQuality = GenPlace.PlaceSpotQuality.Unusable;
			bestSpot = center;
			for (int i = 0; i < 9; i++)
			{
				IntVec3 intVec = center + GenRadial.RadialPattern[i];
				GenPlace.PlaceSpotQuality placeSpotQuality2 = GenPlace.PlaceSpotQualityAt(intVec, map, thing, center, allowStacking, extraValidator);
				if (placeSpotQuality2 > placeSpotQuality)
				{
					bestSpot = intVec;
					placeSpotQuality = placeSpotQuality2;
				}
				if (placeSpotQuality == GenPlace.PlaceSpotQuality.Perfect)
				{
					break;
				}
			}
			bool result;
			if (placeSpotQuality >= GenPlace.PlaceSpotQuality.Okay)
			{
				result = true;
			}
			else
			{
				for (int j = 0; j < GenPlace.PlaceNearMiddleRadialCells; j++)
				{
					IntVec3 intVec = center + GenRadial.RadialPattern[j];
					GenPlace.PlaceSpotQuality placeSpotQuality2 = GenPlace.PlaceSpotQualityAt(intVec, map, thing, center, allowStacking, extraValidator);
					if (placeSpotQuality2 > placeSpotQuality)
					{
						bestSpot = intVec;
						placeSpotQuality = placeSpotQuality2;
					}
					if (placeSpotQuality == GenPlace.PlaceSpotQuality.Perfect)
					{
						break;
					}
				}
				if (placeSpotQuality >= GenPlace.PlaceSpotQuality.Okay)
				{
					result = true;
				}
				else
				{
					for (int k = 0; k < GenPlace.PlaceNearMaxRadialCells; k++)
					{
						IntVec3 intVec = center + GenRadial.RadialPattern[k];
						GenPlace.PlaceSpotQuality placeSpotQuality2 = GenPlace.PlaceSpotQualityAt(intVec, map, thing, center, allowStacking, extraValidator);
						if (placeSpotQuality2 > placeSpotQuality)
						{
							bestSpot = intVec;
							placeSpotQuality = placeSpotQuality2;
						}
						if (placeSpotQuality == GenPlace.PlaceSpotQuality.Perfect)
						{
							break;
						}
					}
					if (placeSpotQuality > GenPlace.PlaceSpotQuality.Unusable)
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

		// Token: 0x06005ED3 RID: 24275 RVA: 0x00304BC4 File Offset: 0x00302FC4
		private static GenPlace.PlaceSpotQuality PlaceSpotQualityAt(IntVec3 c, Map map, Thing thing, IntVec3 center, bool allowStacking, Predicate<IntVec3> extraValidator = null)
		{
			GenPlace.PlaceSpotQuality result;
			if (!c.InBounds(map) || !c.Walkable(map))
			{
				result = GenPlace.PlaceSpotQuality.Unusable;
			}
			else if (extraValidator != null && !extraValidator(c))
			{
				result = GenPlace.PlaceSpotQuality.Unusable;
			}
			else
			{
				List<Thing> list = map.thingGrid.ThingsListAt(c);
				for (int i = 0; i < list.Count; i++)
				{
					Thing thing2 = list[i];
					if (thing.def.saveCompressible && thing2.def.saveCompressible)
					{
						return GenPlace.PlaceSpotQuality.Unusable;
					}
					if (thing.def.category == ThingCategory.Item && thing2.def.category == ThingCategory.Item)
					{
						if (!thing2.CanStackWith(thing) || thing2.stackCount >= thing.def.stackLimit)
						{
							return GenPlace.PlaceSpotQuality.Unusable;
						}
					}
				}
				if (c.GetRoom(map, RegionType.Set_Passable) != center.GetRoom(map, RegionType.Set_Passable))
				{
					if (!map.reachability.CanReach(center, c, PathEndMode.OnCell, TraverseMode.PassDoors, Danger.Deadly))
					{
						result = GenPlace.PlaceSpotQuality.Awful;
					}
					else
					{
						result = GenPlace.PlaceSpotQuality.Bad;
					}
				}
				else
				{
					if (allowStacking)
					{
						for (int j = 0; j < list.Count; j++)
						{
							Thing thing3 = list[j];
							if (thing3.def.category == ThingCategory.Item && thing3.CanStackWith(thing) && thing3.stackCount < thing.def.stackLimit)
							{
								return GenPlace.PlaceSpotQuality.Perfect;
							}
						}
					}
					Pawn pawn = thing as Pawn;
					bool flag = pawn != null && pawn.Downed;
					GenPlace.PlaceSpotQuality placeSpotQuality = GenPlace.PlaceSpotQuality.Perfect;
					for (int k = 0; k < list.Count; k++)
					{
						Thing thing4 = list[k];
						if (thing4.def.IsDoor)
						{
							return GenPlace.PlaceSpotQuality.Bad;
						}
						if (thing4 is Building_WorkTable)
						{
							return GenPlace.PlaceSpotQuality.Bad;
						}
						Pawn pawn2 = thing4 as Pawn;
						if (pawn2 != null)
						{
							if (pawn2.Downed || flag)
							{
								return GenPlace.PlaceSpotQuality.Bad;
							}
							if (placeSpotQuality > GenPlace.PlaceSpotQuality.Okay)
							{
								placeSpotQuality = GenPlace.PlaceSpotQuality.Okay;
							}
						}
						if (thing4.def.category == ThingCategory.Plant)
						{
							if (thing4.def.selectable)
							{
								if (placeSpotQuality > GenPlace.PlaceSpotQuality.Okay)
								{
									placeSpotQuality = GenPlace.PlaceSpotQuality.Okay;
								}
							}
						}
					}
					result = placeSpotQuality;
				}
			}
			return result;
		}

		// Token: 0x06005ED4 RID: 24276 RVA: 0x00304E40 File Offset: 0x00303240
		private static bool TryPlaceDirect(Thing thing, IntVec3 loc, Map map, out Thing resultingThing, Action<Thing, int> placedAction = null)
		{
			Thing thing2 = thing;
			bool flag = false;
			if (thing.stackCount > thing.def.stackLimit)
			{
				thing = thing.SplitOff(thing.def.stackLimit);
				flag = true;
			}
			if (thing.def.stackLimit > 1)
			{
				List<Thing> thingList = loc.GetThingList(map);
				int i = 0;
				while (i < thingList.Count)
				{
					Thing thing3 = thingList[i];
					if (!thing3.CanStackWith(thing))
					{
						i++;
					}
					else
					{
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
				}
			}
			resultingThing = GenSpawn.Spawn(thing, loc, map, WipeMode.Vanish);
			if (placedAction != null)
			{
				placedAction(thing, thing.stackCount);
			}
			return !flag;
		}

		// Token: 0x06005ED5 RID: 24277 RVA: 0x00304F74 File Offset: 0x00303374
		public static Thing HaulPlaceBlockerIn(Thing haulThing, IntVec3 c, Map map, bool checkBlueprints)
		{
			List<Thing> list = map.thingGrid.ThingsListAt(c);
			for (int i = 0; i < list.Count; i++)
			{
				Thing thing = list[i];
				if (checkBlueprints)
				{
					if (thing.def.IsBlueprint)
					{
						return thing;
					}
				}
				if ((thing.def.category != ThingCategory.Plant || thing.def.passability != Traversability.Standable) && thing.def.category != ThingCategory.Filth)
				{
					if (haulThing == null || thing.def.category != ThingCategory.Item || !thing.CanStackWith(haulThing) || thing.def.stackLimit - thing.stackCount < haulThing.stackCount)
					{
						if (thing.def.EverHaulable)
						{
							return thing;
						}
						if (haulThing != null && GenSpawn.SpawningWipes(haulThing.def, thing.def))
						{
							return thing;
						}
						if (thing.def.passability != Traversability.Standable)
						{
							if (thing.def.surfaceType != SurfaceType.Item)
							{
								return thing;
							}
						}
					}
				}
			}
			return null;
		}

		// Token: 0x02000F53 RID: 3923
		private enum PlaceSpotQuality : byte
		{
			// Token: 0x04003E41 RID: 15937
			Unusable,
			// Token: 0x04003E42 RID: 15938
			Awful,
			// Token: 0x04003E43 RID: 15939
			Bad,
			// Token: 0x04003E44 RID: 15940
			Okay,
			// Token: 0x04003E45 RID: 15941
			Perfect
		}
	}
}
