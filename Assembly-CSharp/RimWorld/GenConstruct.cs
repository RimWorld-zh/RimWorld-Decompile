using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public static class GenConstruct
	{
		private static string ConstructionSkillTooLowTrans;

		public static void Reset()
		{
			GenConstruct.ConstructionSkillTooLowTrans = "ConstructionSkillTooLow".Translate();
		}

		public static Blueprint_Build PlaceBlueprintForBuild(BuildableDef sourceDef, IntVec3 center, Map map, Rot4 rotation, Faction faction, ThingDef stuff)
		{
			Blueprint_Build blueprint_Build = (Blueprint_Build)ThingMaker.MakeThing(sourceDef.blueprintDef, null);
			blueprint_Build.SetFactionDirect(faction);
			blueprint_Build.stuffToUse = stuff;
			GenSpawn.Spawn(blueprint_Build, center, map, rotation, false);
			return blueprint_Build;
		}

		public static Blueprint_Install PlaceBlueprintForInstall(MinifiedThing itemToInstall, IntVec3 center, Map map, Rot4 rotation, Faction faction)
		{
			Blueprint_Install blueprint_Install = (Blueprint_Install)ThingMaker.MakeThing(itemToInstall.InnerThing.def.installBlueprintDef, null);
			blueprint_Install.SetThingToInstallFromMinified(itemToInstall);
			blueprint_Install.SetFactionDirect(faction);
			GenSpawn.Spawn(blueprint_Install, center, map, rotation, false);
			return blueprint_Install;
		}

		public static Blueprint_Install PlaceBlueprintForReinstall(Building buildingToReinstall, IntVec3 center, Map map, Rot4 rotation, Faction faction)
		{
			Blueprint_Install blueprint_Install = (Blueprint_Install)ThingMaker.MakeThing(buildingToReinstall.def.installBlueprintDef, null);
			blueprint_Install.SetBuildingToReinstall(buildingToReinstall);
			blueprint_Install.SetFactionDirect(faction);
			GenSpawn.Spawn(blueprint_Install, center, map, rotation, false);
			return blueprint_Install;
		}

		public static bool CanBuildOnTerrain(BuildableDef entDef, IntVec3 c, Map map, Rot4 rot, Thing thingToIgnore = null)
		{
			TerrainDef terrainDef = entDef as TerrainDef;
			bool result;
			if (terrainDef != null && !c.GetTerrain(map).changeable)
			{
				result = false;
			}
			else
			{
				CellRect cellRect = GenAdj.OccupiedRect(c, rot, entDef.Size);
				cellRect.ClipInsideMap(map);
				CellRect.CellRectIterator iterator = cellRect.GetIterator();
				while (!iterator.Done())
				{
					TerrainDef terrainDef2 = map.terrainGrid.TerrainAt(iterator.Current);
					if (!terrainDef2.affordances.Contains(entDef.terrainAffordanceNeeded))
						goto IL_0078;
					List<Thing> thingList = iterator.Current.GetThingList(map);
					for (int i = 0; i < thingList.Count; i++)
					{
						if (thingList[i] != thingToIgnore)
						{
							TerrainDef terrainDef3 = thingList[i].def.entityDefToBuild as TerrainDef;
							if (terrainDef3 != null && !terrainDef3.affordances.Contains(entDef.terrainAffordanceNeeded))
								goto IL_00e4;
						}
					}
					iterator.MoveNext();
				}
				result = true;
			}
			goto IL_011b;
			IL_00e4:
			result = false;
			goto IL_011b;
			IL_011b:
			return result;
			IL_0078:
			result = false;
			goto IL_011b;
		}

		public static Thing MiniToInstallOrBuildingToReinstall(Blueprint b)
		{
			Blueprint_Install blueprint_Install = b as Blueprint_Install;
			return (blueprint_Install == null) ? null : blueprint_Install.MiniToInstallOrBuildingToReinstall;
		}

		public static bool CanConstruct(Thing t, Pawn p, bool forced = false)
		{
			Blueprint blueprint = t as Blueprint;
			bool result;
			if (blueprint != null)
			{
				Thing thingToIgnore = GenConstruct.MiniToInstallOrBuildingToReinstall(blueprint);
				if (blueprint.FirstBlockingThing(p, thingToIgnore, false) != null)
				{
					result = false;
					goto IL_00bd;
				}
			}
			LocalTargetInfo target = t;
			PathEndMode peMode = PathEndMode.Touch;
			Danger maxDanger = (!forced) ? p.NormalMaxDanger() : Danger.Deadly;
			if (!p.CanReserveAndReach(target, peMode, maxDanger, 1, -1, null, forced))
			{
				result = false;
			}
			else if (t.IsBurning())
			{
				result = false;
			}
			else if (p.skills.GetSkill(SkillDefOf.Construction).Level < t.def.constructionSkillPrerequisite)
			{
				JobFailReason.Is(GenConstruct.ConstructionSkillTooLowTrans);
				result = false;
			}
			else
			{
				result = true;
			}
			goto IL_00bd;
			IL_00bd:
			return result;
		}

		public static int AmountNeededByOf(IConstructible c, ThingDef resDef)
		{
			foreach (ThingCountClass item in c.MaterialsNeeded())
			{
				if (item.thingDef == resDef)
				{
					return item.count;
				}
			}
			return 0;
		}

		public static AcceptanceReport CanPlaceBlueprintAt(BuildableDef entDef, IntVec3 center, Rot4 rot, Map map, bool godMode = false, Thing thingToIgnore = null)
		{
			CellRect cellRect = GenAdj.OccupiedRect(center, rot, entDef.Size);
			CellRect.CellRectIterator iterator = cellRect.GetIterator();
			AcceptanceReport result;
			while (true)
			{
				if (!iterator.Done())
				{
					IntVec3 current = iterator.Current;
					if (!current.InBounds(map))
					{
						result = new AcceptanceReport("OutOfBounds".Translate());
						break;
					}
					if (current.InNoBuildEdgeArea(map) && !DebugSettings.godMode)
					{
						result = "TooCloseToMapEdge".Translate();
						break;
					}
					iterator.MoveNext();
					continue;
				}
				Thing thing;
				List<Thing> list;
				int j;
				Blueprint blueprint;
				AcceptanceReport acceptanceReport;
				if (center.Fogged(map))
				{
					result = "CannotPlaceInUndiscovered".Translate();
				}
				else
				{
					List<Thing> thingList = center.GetThingList(map);
					for (int i = 0; i < thingList.Count; i++)
					{
						thing = thingList[i];
						if (thing != thingToIgnore && thing.Position == center && thing.Rotation == rot)
						{
							if (thing.def == entDef)
								goto IL_0103;
							if (thing.def.entityDefToBuild == entDef)
								goto IL_012a;
						}
					}
					ThingDef thingDef = entDef as ThingDef;
					if (thingDef != null && thingDef.hasInteractionCell)
					{
						IntVec3 c = ThingUtility.InteractionCellWhenAt(thingDef, center, rot, map);
						if (!c.InBounds(map))
						{
							result = new AcceptanceReport("InteractionSpotOutOfBounds".Translate());
							break;
						}
						list = map.thingGrid.ThingsListAtFast(c);
						for (j = 0; j < list.Count; j++)
						{
							if (list[j] != thingToIgnore)
							{
								if (list[j].def.passability == Traversability.Impassable)
									goto IL_0207;
								blueprint = (list[j] as Blueprint);
								if (blueprint != null && blueprint.def.entityDefToBuild.passability == Traversability.Impassable)
									goto IL_0266;
							}
						}
					}
					if (entDef.passability != 0)
					{
						foreach (IntVec3 item in GenAdj.CellsAdjacentCardinal(center, rot, entDef.Size))
						{
							if (item.InBounds(map))
							{
								thingList = item.GetThingList(map);
								for (int k = 0; k < thingList.Count; k++)
								{
									Thing thing2 = thingList[k];
									ThingDef thingDef2;
									if (thing2 != thingToIgnore)
									{
										thingDef2 = null;
										Blueprint blueprint2 = thing2 as Blueprint;
										if (blueprint2 != null)
										{
											ThingDef thingDef3 = blueprint2.def.entityDefToBuild as ThingDef;
											if (thingDef3 != null)
											{
												thingDef2 = thingDef3;
												goto IL_035a;
											}
											continue;
										}
										thingDef2 = thing2.def;
										goto IL_035a;
									}
									continue;
									IL_035a:
									if (thingDef2.hasInteractionCell && cellRect.Contains(ThingUtility.InteractionCellWhenAt(thingDef2, thing2.Position, thing2.Rotation, thing2.Map)))
									{
										return new AcceptanceReport("WouldBlockInteractionSpot".Translate(entDef.label, thingDef2.label).CapitalizeFirst());
									}
								}
							}
						}
					}
					TerrainDef terrainDef = entDef as TerrainDef;
					if (terrainDef != null)
					{
						if (map.terrainGrid.TerrainAt(center) == terrainDef)
						{
							result = new AcceptanceReport("TerrainIsAlready".Translate(terrainDef.label));
							break;
						}
						if (map.designationManager.DesignationAt(center, DesignationDefOf.SmoothFloor) != null)
						{
							result = new AcceptanceReport("SpaceBeingSmoothed".Translate());
							break;
						}
					}
					if (!GenConstruct.CanBuildOnTerrain(entDef, center, map, rot, thingToIgnore))
					{
						result = new AcceptanceReport("TerrainCannotSupport".Translate());
					}
					else
					{
						if (!godMode)
						{
							CellRect.CellRectIterator iterator2 = cellRect.GetIterator();
							while (!iterator2.Done())
							{
								thingList = iterator2.Current.GetThingList(map);
								for (int l = 0; l < thingList.Count; l++)
								{
									Thing thing3 = thingList[l];
									if (thing3 != thingToIgnore && !GenConstruct.CanPlaceBlueprintOver(entDef, thing3.def))
										goto IL_04eb;
								}
								iterator2.MoveNext();
							}
						}
						if (entDef.PlaceWorkers != null)
						{
							for (int m = 0; m < entDef.PlaceWorkers.Count; m++)
							{
								acceptanceReport = entDef.PlaceWorkers[m].AllowsPlacing(entDef, center, rot, map, thingToIgnore);
								if (!acceptanceReport.Accepted)
									goto IL_0565;
							}
						}
						result = AcceptanceReport.WasAccepted;
					}
				}
				break;
				IL_04eb:
				result = new AcceptanceReport("SpaceAlreadyOccupied".Translate());
				break;
				IL_0565:
				result = acceptanceReport;
				break;
				IL_0103:
				result = new AcceptanceReport("IdenticalThingExists".Translate());
				break;
				IL_0207:
				result = new AcceptanceReport("InteractionSpotBlocked".Translate(list[j].LabelNoCount).CapitalizeFirst());
				break;
				IL_0266:
				result = new AcceptanceReport("InteractionSpotWillBeBlocked".Translate(blueprint.LabelNoCount).CapitalizeFirst());
				break;
				IL_012a:
				result = ((!(thing is Blueprint)) ? new AcceptanceReport("IdenticalThingExists".Translate()) : new AcceptanceReport("IdenticalBlueprintExists".Translate()));
				break;
			}
			return result;
		}

		public static BuildableDef BuiltDefOf(ThingDef def)
		{
			return (def.entityDefToBuild == null) ? def : def.entityDefToBuild;
		}

		public static bool CanPlaceBlueprintOver(BuildableDef newDef, ThingDef oldDef)
		{
			bool result;
			if (oldDef.EverHaulable)
			{
				result = true;
			}
			else
			{
				TerrainDef terrainDef = newDef as TerrainDef;
				if (terrainDef != null)
				{
					if (oldDef.category == ThingCategory.Building && !terrainDef.affordances.Contains(oldDef.terrainAffordanceNeeded))
					{
						result = false;
						goto IL_0279;
					}
					if ((oldDef.IsBlueprint || oldDef.IsFrame) && !terrainDef.affordances.Contains(oldDef.entityDefToBuild.terrainAffordanceNeeded))
					{
						result = false;
						goto IL_0279;
					}
				}
				ThingDef thingDef = newDef as ThingDef;
				BuildableDef buildableDef = GenConstruct.BuiltDefOf(oldDef);
				ThingDef thingDef2 = buildableDef as ThingDef;
				if (oldDef == ThingDefOf.SteamGeyser && !newDef.ForceAllowPlaceOver(oldDef))
				{
					result = false;
				}
				else if (oldDef.category == ThingCategory.Plant && oldDef.passability == Traversability.Impassable && thingDef != null && thingDef.category == ThingCategory.Building && !thingDef.building.canPlaceOverImpassablePlant)
				{
					result = false;
				}
				else if (oldDef.category == ThingCategory.Building || oldDef.IsBlueprint || oldDef.IsFrame)
				{
					if (thingDef != null)
					{
						if (!thingDef.IsEdifice())
						{
							result = ((byte)((oldDef.building == null || oldDef.building.canBuildNonEdificesUnder) ? ((!thingDef.EverTransmitsPower || !oldDef.EverTransmitsPower) ? 1 : 0) : 0) != 0);
							goto IL_0279;
						}
						if (thingDef.IsEdifice() && oldDef != null && oldDef.category == ThingCategory.Building && !oldDef.IsEdifice())
						{
							result = ((byte)((thingDef.building == null || thingDef.building.canBuildNonEdificesUnder) ? 1 : 0) != 0);
							goto IL_0279;
						}
						if (thingDef2 != null && thingDef2 == ThingDefOf.Wall && thingDef.building != null && thingDef.building.canPlaceOverWall)
						{
							result = true;
							goto IL_0279;
						}
						if (newDef != ThingDefOf.PowerConduit && buildableDef == ThingDefOf.PowerConduit)
						{
							result = true;
							goto IL_0279;
						}
					}
					result = ((byte)((newDef is TerrainDef && buildableDef is ThingDef && ((ThingDef)buildableDef).CoexistsWithFloors) ? 1 : ((buildableDef is TerrainDef && !(newDef is TerrainDef)) ? 1 : 0)) != 0);
				}
				else
				{
					result = true;
				}
			}
			goto IL_0279;
			IL_0279:
			return result;
		}

		public static bool BlocksFramePlacement(Blueprint blue, Thing t)
		{
			bool result;
			if (t.def.category == ThingCategory.Plant)
			{
				result = ((byte)((t.def.plant.harvestWork >= 200.0) ? 1 : 0) != 0);
			}
			else if (!blue.def.clearBuildingArea)
			{
				result = false;
			}
			else if (t.def == ThingDefOf.SteamGeyser && blue.def.entityDefToBuild.ForceAllowPlaceOver(t.def))
			{
				result = false;
			}
			else
			{
				ThingDef thingDef = blue.def.entityDefToBuild as ThingDef;
				if (thingDef != null)
				{
					if (thingDef.EverTransmitsPower && t.def == ThingDefOf.PowerConduit && thingDef != ThingDefOf.PowerConduit)
					{
						result = false;
						goto IL_017e;
					}
					if (t.def == ThingDefOf.Wall && thingDef.building != null && thingDef.building.canPlaceOverWall)
					{
						result = false;
						goto IL_017e;
					}
				}
				result = ((byte)((t.def.IsEdifice() && thingDef.IsEdifice()) ? 1 : ((t.def.category == ThingCategory.Pawn || (t.def.category == ThingCategory.Item && blue.def.entityDefToBuild.passability == Traversability.Impassable)) ? 1 : (((int)t.def.Fillage >= 1) ? 1 : 0))) != 0);
			}
			goto IL_017e;
			IL_017e:
			return result;
		}

		public static bool TerrainCanSupport(CellRect rect, Map map, ThingDef thing)
		{
			CellRect.CellRectIterator iterator = rect.GetIterator();
			bool result;
			while (true)
			{
				if (!iterator.Done())
				{
					if (!iterator.Current.SupportsStructureType(map, thing.terrainAffordanceNeeded))
					{
						result = false;
						break;
					}
					iterator.MoveNext();
					continue;
				}
				result = true;
				break;
			}
			return result;
		}
	}
}
