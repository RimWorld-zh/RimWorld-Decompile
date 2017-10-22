using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Verse
{
	public static class GenSpawn
	{
		public static Thing Spawn(ThingDef def, IntVec3 loc, Map map)
		{
			return GenSpawn.Spawn(ThingMaker.MakeThing(def, null), loc, map);
		}

		public static Thing Spawn(Thing newThing, IntVec3 loc, Map map)
		{
			return GenSpawn.Spawn(newThing, loc, map, Rot4.North, false);
		}

		public static Thing Spawn(Thing newThing, IntVec3 loc, Map map, Rot4 rot, bool respawningAfterLoad = false)
		{
			Thing result;
			if (map == null)
			{
				Log.Error("Tried to spawn " + newThing + " in a null map.");
				result = null;
			}
			else if (!loc.InBounds(map))
			{
				Log.Error("Tried to spawn " + newThing + " out of bounds at " + loc + ".");
				result = null;
			}
			else if (newThing.Spawned)
			{
				Log.Error("Tried to spawn " + newThing + " but it's already spawned.");
				result = newThing;
			}
			else
			{
				GenSpawn.WipeExistingThings(loc, rot, newThing.def, map, DestroyMode.Vanish);
				if (newThing.def.randomizeRotationOnSpawn)
				{
					newThing.Rotation = Rot4.Random;
				}
				else
				{
					newThing.Rotation = rot;
				}
				newThing.Position = loc;
				if (newThing.holdingOwner != null)
				{
					newThing.holdingOwner.Remove(newThing);
				}
				newThing.SpawnSetup(map, respawningAfterLoad);
				if (newThing.Spawned && newThing.stackCount == 0)
				{
					Log.Error("Spawned thing with 0 stackCount: " + newThing);
					newThing.Destroy(DestroyMode.Vanish);
					result = null;
				}
				else
				{
					result = newThing;
				}
			}
			return result;
		}

		public static void SpawnBuildingAsPossible(Building building, Map map, bool respawningAfterLoad = false)
		{
			bool flag = false;
			foreach (IntVec3 item in building.OccupiedRect())
			{
				List<Thing> thingList = item.GetThingList(map);
				int num = 0;
				while (num < thingList.Count)
				{
					if (!(thingList[num] is Pawn) || building.def.passability != Traversability.Impassable)
					{
						if ((thingList[num].def.category == ThingCategory.Building || thingList[num].def.category == ThingCategory.Item) && GenSpawn.SpawningWipes(building.def, thingList[num].def))
						{
							flag = true;
							break;
						}
						num++;
						continue;
					}
					flag = true;
					break;
				}
				if (flag)
					break;
			}
			if (flag)
			{
				bool flag2 = false;
				if (building.def.Minifiable)
				{
					MinifiedThing minifiedThing = building.MakeMinified();
					if (GenPlace.TryPlaceThing(minifiedThing, building.Position, map, ThingPlaceMode.Near, null))
					{
						flag2 = true;
					}
					else
					{
						minifiedThing.GetDirectlyHeldThings().Clear();
						minifiedThing.Destroy(DestroyMode.Vanish);
					}
				}
				if (!flag2)
				{
					GenLeaving.DoLeavingsFor(building, map, DestroyMode.Refund, building.OccupiedRect());
				}
			}
			else
			{
				GenSpawn.Spawn(building, building.Position, map, building.Rotation, respawningAfterLoad);
			}
		}

		public static void WipeExistingThings(IntVec3 thingPos, Rot4 thingRot, BuildableDef thingDef, Map map, DestroyMode mode)
		{
			foreach (IntVec3 item in GenAdj.CellsOccupiedBy(thingPos, thingRot, thingDef.Size))
			{
				foreach (Thing item2 in map.thingGrid.ThingsAt(item).ToList())
				{
					if (GenSpawn.SpawningWipes(thingDef, item2.def))
					{
						item2.Destroy(mode);
					}
				}
			}
		}

		public static bool WouldWipeAnythingWith(IntVec3 thingPos, Rot4 thingRot, BuildableDef thingDef, Map map, Predicate<Thing> predicate)
		{
			return GenSpawn.WouldWipeAnythingWith(GenAdj.OccupiedRect(thingPos, thingRot, thingDef.Size), thingDef, map, predicate);
		}

		public static bool WouldWipeAnythingWith(CellRect cellRect, BuildableDef thingDef, Map map, Predicate<Thing> predicate)
		{
			foreach (IntVec3 item in cellRect)
			{
				foreach (Thing item2 in map.thingGrid.ThingsAt(item).ToList())
				{
					if (GenSpawn.SpawningWipes(thingDef, item2.def) && predicate(item2))
					{
						return true;
					}
				}
			}
			return false;
		}

		public static bool SpawningWipes(BuildableDef newEntDef, BuildableDef oldEntDef)
		{
			ThingDef thingDef = newEntDef as ThingDef;
			ThingDef thingDef2 = oldEntDef as ThingDef;
			bool result;
			if (thingDef == null || thingDef2 == null)
			{
				result = false;
			}
			else if (thingDef.category == ThingCategory.Attachment || thingDef.category == ThingCategory.Mote || thingDef.category == ThingCategory.Filth || thingDef.category == ThingCategory.Projectile)
			{
				result = false;
			}
			else if (!thingDef2.destroyable)
			{
				result = false;
			}
			else if (thingDef.category == ThingCategory.Plant)
			{
				result = false;
			}
			else if (((thingDef2.category == ThingCategory.Filth) ? thingDef.passability : Traversability.Standable) != 0)
			{
				result = true;
			}
			else if (thingDef2.category == ThingCategory.Item && thingDef.passability == Traversability.Impassable && thingDef.surfaceType == SurfaceType.None)
			{
				result = true;
			}
			else if (thingDef.EverTransmitsPower && thingDef2 == ThingDefOf.PowerConduit)
			{
				result = true;
			}
			else if (thingDef.IsFrame && GenSpawn.SpawningWipes(thingDef.entityDefToBuild, oldEntDef))
			{
				result = true;
			}
			else
			{
				BuildableDef buildableDef = GenConstruct.BuiltDefOf(thingDef);
				BuildableDef buildableDef2 = GenConstruct.BuiltDefOf(thingDef2);
				if (buildableDef == null || buildableDef2 == null)
				{
					result = false;
				}
				else
				{
					ThingDef thingDef3 = thingDef.entityDefToBuild as ThingDef;
					if (thingDef2.IsBlueprint)
					{
						if (thingDef.IsBlueprint)
						{
							if (thingDef3 != null && thingDef3.building != null && thingDef3.building.canPlaceOverWall && thingDef2.entityDefToBuild is ThingDef && (ThingDef)thingDef2.entityDefToBuild == ThingDefOf.Wall)
							{
								result = true;
								goto IL_032f;
							}
							if (thingDef2.entityDefToBuild is TerrainDef)
							{
								if (thingDef.entityDefToBuild is ThingDef && ((ThingDef)thingDef.entityDefToBuild).coversFloor)
								{
									result = true;
									goto IL_032f;
								}
								if (thingDef.entityDefToBuild is TerrainDef)
								{
									result = true;
									goto IL_032f;
								}
							}
						}
						result = ((byte)((thingDef2.entityDefToBuild == ThingDefOf.PowerConduit && thingDef.entityDefToBuild is ThingDef && (thingDef.entityDefToBuild as ThingDef).EverTransmitsPower) ? 1 : 0) != 0);
					}
					else
					{
						if ((thingDef2.IsFrame || thingDef2.IsBlueprint) && thingDef2.entityDefToBuild is TerrainDef)
						{
							ThingDef thingDef4 = buildableDef as ThingDef;
							if (thingDef4 != null && !thingDef4.CoexistsWithFloors)
							{
								result = true;
								goto IL_032f;
							}
						}
						if (thingDef2 == ThingDefOf.ActiveDropPod)
						{
							result = false;
						}
						else if (thingDef == ThingDefOf.ActiveDropPod)
						{
							result = ((byte)((thingDef2 != ThingDefOf.ActiveDropPod) ? ((thingDef2.category == ThingCategory.Building && thingDef2.passability == Traversability.Impassable) ? 1 : 0) : 0) != 0);
						}
						else
						{
							if (thingDef.IsEdifice())
							{
								if (thingDef.BlockPlanting && thingDef2.category == ThingCategory.Plant)
								{
									result = true;
									goto IL_032f;
								}
								if (!(buildableDef is TerrainDef) && buildableDef2.IsEdifice())
								{
									result = true;
									goto IL_032f;
								}
							}
							result = false;
						}
					}
				}
			}
			goto IL_032f;
			IL_032f:
			return result;
		}
	}
}
