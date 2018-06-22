using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200097B RID: 2427
	public static class GenConstruct
	{
		// Token: 0x06003699 RID: 13977 RVA: 0x001D1B05 File Offset: 0x001CFF05
		public static void Reset()
		{
			GenConstruct.ConstructionSkillTooLowTrans = "ConstructionSkillTooLow".Translate();
			GenConstruct.IncapableOfDeconstruction = "IncapableOfDeconstruction".Translate();
		}

		// Token: 0x0600369A RID: 13978 RVA: 0x001D1B28 File Offset: 0x001CFF28
		public static Blueprint_Build PlaceBlueprintForBuild(BuildableDef sourceDef, IntVec3 center, Map map, Rot4 rotation, Faction faction, ThingDef stuff)
		{
			Blueprint_Build blueprint_Build = (Blueprint_Build)ThingMaker.MakeThing(sourceDef.blueprintDef, null);
			blueprint_Build.SetFactionDirect(faction);
			blueprint_Build.stuffToUse = stuff;
			GenSpawn.Spawn(blueprint_Build, center, map, rotation, WipeMode.Vanish, false);
			return blueprint_Build;
		}

		// Token: 0x0600369B RID: 13979 RVA: 0x001D1B6C File Offset: 0x001CFF6C
		public static Blueprint_Install PlaceBlueprintForInstall(MinifiedThing itemToInstall, IntVec3 center, Map map, Rot4 rotation, Faction faction)
		{
			Blueprint_Install blueprint_Install = (Blueprint_Install)ThingMaker.MakeThing(itemToInstall.InnerThing.def.installBlueprintDef, null);
			blueprint_Install.SetThingToInstallFromMinified(itemToInstall);
			blueprint_Install.SetFactionDirect(faction);
			GenSpawn.Spawn(blueprint_Install, center, map, rotation, WipeMode.Vanish, false);
			return blueprint_Install;
		}

		// Token: 0x0600369C RID: 13980 RVA: 0x001D1BBC File Offset: 0x001CFFBC
		public static Blueprint_Install PlaceBlueprintForReinstall(Building buildingToReinstall, IntVec3 center, Map map, Rot4 rotation, Faction faction)
		{
			Blueprint_Install blueprint_Install = (Blueprint_Install)ThingMaker.MakeThing(buildingToReinstall.def.installBlueprintDef, null);
			blueprint_Install.SetBuildingToReinstall(buildingToReinstall);
			blueprint_Install.SetFactionDirect(faction);
			GenSpawn.Spawn(blueprint_Install, center, map, rotation, WipeMode.Vanish, false);
			return blueprint_Install;
		}

		// Token: 0x0600369D RID: 13981 RVA: 0x001D1C04 File Offset: 0x001D0004
		public static bool CanBuildOnTerrain(BuildableDef entDef, IntVec3 c, Map map, Rot4 rot, Thing thingToIgnore = null)
		{
			TerrainDef terrainDef = entDef as TerrainDef;
			if (terrainDef != null)
			{
				if (!c.GetTerrain(map).changeable)
				{
					return false;
				}
			}
			CellRect cellRect = GenAdj.OccupiedRect(c, rot, entDef.Size);
			cellRect.ClipInsideMap(map);
			CellRect.CellRectIterator iterator = cellRect.GetIterator();
			while (!iterator.Done())
			{
				TerrainDef terrainDef2 = map.terrainGrid.TerrainAt(iterator.Current);
				if (entDef.terrainAffordanceNeeded != null && !terrainDef2.affordances.Contains(entDef.terrainAffordanceNeeded))
				{
					return false;
				}
				List<Thing> thingList = iterator.Current.GetThingList(map);
				for (int i = 0; i < thingList.Count; i++)
				{
					if (thingList[i] != thingToIgnore)
					{
						TerrainDef terrainDef3 = thingList[i].def.entityDefToBuild as TerrainDef;
						if (terrainDef3 != null && !terrainDef3.affordances.Contains(entDef.terrainAffordanceNeeded))
						{
							return false;
						}
					}
				}
				iterator.MoveNext();
			}
			return true;
		}

		// Token: 0x0600369E RID: 13982 RVA: 0x001D1D38 File Offset: 0x001D0138
		public static Thing MiniToInstallOrBuildingToReinstall(Blueprint b)
		{
			Blueprint_Install blueprint_Install = b as Blueprint_Install;
			Thing result;
			if (blueprint_Install != null)
			{
				result = blueprint_Install.MiniToInstallOrBuildingToReinstall;
			}
			else
			{
				result = null;
			}
			return result;
		}

		// Token: 0x0600369F RID: 13983 RVA: 0x001D1D68 File Offset: 0x001D0168
		public static bool CanConstruct(Thing t, Pawn p, bool checkConstructionSkill = true, bool forced = false)
		{
			bool result;
			if (GenConstruct.FirstBlockingThing(t, p) != null)
			{
				result = false;
			}
			else
			{
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
				else if (checkConstructionSkill && p.skills.GetSkill(SkillDefOf.Construction).Level < t.def.constructionSkillPrerequisite)
				{
					JobFailReason.Is(GenConstruct.ConstructionSkillTooLowTrans, null);
					result = false;
				}
				else
				{
					result = true;
				}
			}
			return result;
		}

		// Token: 0x060036A0 RID: 13984 RVA: 0x001D1E20 File Offset: 0x001D0220
		public static int AmountNeededByOf(IConstructible c, ThingDef resDef)
		{
			foreach (ThingDefCountClass thingDefCountClass in c.MaterialsNeeded())
			{
				if (thingDefCountClass.thingDef == resDef)
				{
					return thingDefCountClass.count;
				}
			}
			return 0;
		}

		// Token: 0x060036A1 RID: 13985 RVA: 0x001D1E9C File Offset: 0x001D029C
		public static AcceptanceReport CanPlaceBlueprintAt(BuildableDef entDef, IntVec3 center, Rot4 rot, Map map, bool godMode = false, Thing thingToIgnore = null)
		{
			CellRect cellRect = GenAdj.OccupiedRect(center, rot, entDef.Size);
			CellRect.CellRectIterator iterator = cellRect.GetIterator();
			while (!iterator.Done())
			{
				IntVec3 c = iterator.Current;
				AcceptanceReport result;
				if (!c.InBounds(map))
				{
					result = new AcceptanceReport("OutOfBounds".Translate());
				}
				else
				{
					if (!c.InNoBuildEdgeArea(map) || DebugSettings.godMode)
					{
						iterator.MoveNext();
						continue;
					}
					result = "TooCloseToMapEdge".Translate();
				}
				return result;
			}
			if (center.Fogged(map))
			{
				return "CannotPlaceInUndiscovered".Translate();
			}
			List<Thing> thingList = center.GetThingList(map);
			for (int i = 0; i < thingList.Count; i++)
			{
				Thing thing = thingList[i];
				if (thing != thingToIgnore)
				{
					if (thing.Position == center && thing.Rotation == rot)
					{
						if (thing.def == entDef)
						{
							return new AcceptanceReport("IdenticalThingExists".Translate());
						}
						if (thing.def.entityDefToBuild == entDef)
						{
							if (thing is Blueprint)
							{
								return new AcceptanceReport("IdenticalBlueprintExists".Translate());
							}
							return new AcceptanceReport("IdenticalThingExists".Translate());
						}
					}
				}
			}
			ThingDef thingDef = entDef as ThingDef;
			if (thingDef != null && thingDef.hasInteractionCell)
			{
				IntVec3 c2 = ThingUtility.InteractionCellWhenAt(thingDef, center, rot, map);
				if (!c2.InBounds(map))
				{
					return new AcceptanceReport("InteractionSpotOutOfBounds".Translate());
				}
				List<Thing> list = map.thingGrid.ThingsListAtFast(c2);
				for (int j = 0; j < list.Count; j++)
				{
					if (list[j] != thingToIgnore)
					{
						if (list[j].def.passability == Traversability.Impassable)
						{
							return new AcceptanceReport("InteractionSpotBlocked".Translate(new object[]
							{
								list[j].LabelNoCount
							}).CapitalizeFirst());
						}
						Blueprint blueprint = list[j] as Blueprint;
						if (blueprint != null && blueprint.def.entityDefToBuild.passability == Traversability.Impassable)
						{
							return new AcceptanceReport("InteractionSpotWillBeBlocked".Translate(new object[]
							{
								blueprint.LabelNoCount
							}).CapitalizeFirst());
						}
					}
				}
			}
			if (entDef.passability == Traversability.Impassable)
			{
				foreach (IntVec3 c3 in GenAdj.CellsAdjacentCardinal(center, rot, entDef.Size))
				{
					if (c3.InBounds(map))
					{
						thingList = c3.GetThingList(map);
						for (int k = 0; k < thingList.Count; k++)
						{
							Thing thing2 = thingList[k];
							if (thing2 != thingToIgnore)
							{
								Blueprint blueprint2 = thing2 as Blueprint;
								ThingDef thingDef3;
								if (blueprint2 != null)
								{
									ThingDef thingDef2 = blueprint2.def.entityDefToBuild as ThingDef;
									if (thingDef2 == null)
									{
										goto IL_3C3;
									}
									thingDef3 = thingDef2;
								}
								else
								{
									thingDef3 = thing2.def;
								}
								if (thingDef3.hasInteractionCell && cellRect.Contains(ThingUtility.InteractionCellWhenAt(thingDef3, thing2.Position, thing2.Rotation, thing2.Map)))
								{
									return new AcceptanceReport("WouldBlockInteractionSpot".Translate(new object[]
									{
										entDef.label,
										thingDef3.label
									}).CapitalizeFirst());
								}
							}
							IL_3C3:;
						}
					}
				}
			}
			TerrainDef terrainDef = entDef as TerrainDef;
			if (terrainDef != null)
			{
				if (map.terrainGrid.TerrainAt(center) == terrainDef)
				{
					return new AcceptanceReport("TerrainIsAlready".Translate(new object[]
					{
						terrainDef.label
					}));
				}
				if (map.designationManager.DesignationAt(center, DesignationDefOf.SmoothFloor) != null)
				{
					return new AcceptanceReport("SpaceBeingSmoothed".Translate());
				}
			}
			if (!GenConstruct.CanBuildOnTerrain(entDef, center, map, rot, thingToIgnore))
			{
				return new AcceptanceReport("TerrainCannotSupport".Translate());
			}
			if (!godMode)
			{
				CellRect.CellRectIterator iterator2 = cellRect.GetIterator();
				while (!iterator2.Done())
				{
					thingList = iterator2.Current.GetThingList(map);
					for (int l = 0; l < thingList.Count; l++)
					{
						Thing thing3 = thingList[l];
						if (thing3 != thingToIgnore)
						{
							if (!GenConstruct.CanPlaceBlueprintOver(entDef, thing3.def))
							{
								return new AcceptanceReport("SpaceAlreadyOccupied".Translate());
							}
						}
					}
					iterator2.MoveNext();
				}
			}
			if (entDef.PlaceWorkers != null)
			{
				for (int m = 0; m < entDef.PlaceWorkers.Count; m++)
				{
					AcceptanceReport result2 = entDef.PlaceWorkers[m].AllowsPlacing(entDef, center, rot, map, thingToIgnore);
					if (!result2.Accepted)
					{
						return result2;
					}
				}
			}
			return AcceptanceReport.WasAccepted;
		}

		// Token: 0x060036A2 RID: 13986 RVA: 0x001D245C File Offset: 0x001D085C
		public static BuildableDef BuiltDefOf(ThingDef def)
		{
			return (def.entityDefToBuild == null) ? def : def.entityDefToBuild;
		}

		// Token: 0x060036A3 RID: 13987 RVA: 0x001D2488 File Offset: 0x001D0888
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
						return false;
					}
					if ((oldDef.IsBlueprint || oldDef.IsFrame) && !terrainDef.affordances.Contains(oldDef.entityDefToBuild.terrainAffordanceNeeded))
					{
						return false;
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
							return (oldDef.building == null || oldDef.building.canBuildNonEdificesUnder) && (!thingDef.EverTransmitsPower || !oldDef.EverTransmitsPower);
						}
						if (thingDef.IsEdifice() && oldDef != null && oldDef.category == ThingCategory.Building && !oldDef.IsEdifice())
						{
							return thingDef.building == null || thingDef.building.canBuildNonEdificesUnder;
						}
						if (thingDef2 != null && thingDef2 == ThingDefOf.Wall && thingDef.building != null && thingDef.building.canPlaceOverWall)
						{
							return true;
						}
						if (newDef != ThingDefOf.PowerConduit && buildableDef == ThingDefOf.PowerConduit)
						{
							return true;
						}
					}
					result = ((newDef is TerrainDef && buildableDef is ThingDef && ((ThingDef)buildableDef).CoexistsWithFloors) || (buildableDef is TerrainDef && !(newDef is TerrainDef)));
				}
				else
				{
					result = true;
				}
			}
			return result;
		}

		// Token: 0x060036A4 RID: 13988 RVA: 0x001D2710 File Offset: 0x001D0B10
		public static Thing FirstBlockingThing(Thing constructible, Pawn pawnToIgnore)
		{
			Blueprint blueprint = constructible as Blueprint;
			Thing thing;
			if (blueprint != null)
			{
				thing = GenConstruct.MiniToInstallOrBuildingToReinstall(blueprint);
			}
			else
			{
				thing = null;
			}
			CellRect.CellRectIterator iterator = constructible.OccupiedRect().GetIterator();
			while (!iterator.Done())
			{
				List<Thing> thingList = iterator.Current.GetThingList(constructible.Map);
				for (int i = 0; i < thingList.Count; i++)
				{
					Thing thing2 = thingList[i];
					if (GenConstruct.BlocksConstruction(constructible, thing2) && thing2 != pawnToIgnore && thing2 != thing)
					{
						return thing2;
					}
				}
				iterator.MoveNext();
			}
			return null;
		}

		// Token: 0x060036A5 RID: 13989 RVA: 0x001D27D0 File Offset: 0x001D0BD0
		public static Job HandleBlockingThingJob(Thing constructible, Pawn worker, bool forced = false)
		{
			Thing thing = GenConstruct.FirstBlockingThing(constructible, worker);
			Job result;
			if (thing == null)
			{
				result = null;
			}
			else
			{
				if (thing.def.category == ThingCategory.Plant)
				{
					LocalTargetInfo target = thing;
					PathEndMode peMode = PathEndMode.ClosestTouch;
					Danger maxDanger = worker.NormalMaxDanger();
					if (worker.CanReserveAndReach(target, peMode, maxDanger, 1, -1, null, forced))
					{
						return new Job(JobDefOf.CutPlant, thing);
					}
				}
				else if (thing.def.category == ThingCategory.Item)
				{
					if (thing.def.EverHaulable)
					{
						return HaulAIUtility.HaulAsideJobFor(worker, thing);
					}
					Log.ErrorOnce(string.Concat(new object[]
					{
						"Never haulable ",
						thing,
						" blocking ",
						constructible.ToStringSafe<Thing>(),
						" at ",
						constructible.Position
					}), 6429262, false);
				}
				else if (thing.def.category == ThingCategory.Building)
				{
					if (worker.story != null && worker.story.WorkTypeIsDisabled(WorkTypeDefOf.Construction))
					{
						JobFailReason.Is(GenConstruct.IncapableOfDeconstruction, null);
						return null;
					}
					LocalTargetInfo target = thing;
					PathEndMode peMode = PathEndMode.Touch;
					Danger maxDanger = worker.NormalMaxDanger();
					if (worker.CanReserveAndReach(target, peMode, maxDanger, 1, -1, null, forced))
					{
						return new Job(JobDefOf.Deconstruct, thing)
						{
							ignoreDesignations = true
						};
					}
				}
				result = null;
			}
			return result;
		}

		// Token: 0x060036A6 RID: 13990 RVA: 0x001D2968 File Offset: 0x001D0D68
		public static bool BlocksConstruction(Thing constructible, Thing t)
		{
			bool result;
			if (t == constructible)
			{
				result = false;
			}
			else
			{
				ThingDef thingDef;
				if (constructible is Blueprint)
				{
					thingDef = constructible.def;
				}
				else if (constructible is Frame)
				{
					thingDef = constructible.def.entityDefToBuild.blueprintDef;
				}
				else
				{
					thingDef = constructible.def.blueprintDef;
				}
				if (t.def.category == ThingCategory.Building && GenSpawn.SpawningWipes(thingDef.entityDefToBuild, t.def))
				{
					result = true;
				}
				else if (t.def.category == ThingCategory.Plant)
				{
					result = (t.def.plant.harvestWork >= 200f);
				}
				else if (!thingDef.clearBuildingArea)
				{
					result = false;
				}
				else if (t.def == ThingDefOf.SteamGeyser && thingDef.entityDefToBuild.ForceAllowPlaceOver(t.def))
				{
					result = false;
				}
				else
				{
					ThingDef thingDef2 = thingDef.entityDefToBuild as ThingDef;
					if (thingDef2 != null)
					{
						if (thingDef2.EverTransmitsPower)
						{
							if (t.def == ThingDefOf.PowerConduit && thingDef2 != ThingDefOf.PowerConduit)
							{
								return false;
							}
						}
						if (t.def == ThingDefOf.Wall && thingDef2.building != null && thingDef2.building.canPlaceOverWall)
						{
							return false;
						}
					}
					result = ((t.def.IsEdifice() && thingDef2.IsEdifice()) || (t.def.category == ThingCategory.Pawn || (t.def.category == ThingCategory.Item && thingDef.entityDefToBuild.passability == Traversability.Impassable)) || t.def.Fillage >= FillCategory.Partial);
				}
			}
			return result;
		}

		// Token: 0x060036A7 RID: 13991 RVA: 0x001D2B64 File Offset: 0x001D0F64
		public static bool TerrainCanSupport(CellRect rect, Map map, ThingDef thing)
		{
			CellRect.CellRectIterator iterator = rect.GetIterator();
			while (!iterator.Done())
			{
				if (!iterator.Current.SupportsStructureType(map, thing.terrainAffordanceNeeded))
				{
					return false;
				}
				iterator.MoveNext();
			}
			return true;
		}

		// Token: 0x0400233F RID: 9023
		private static string ConstructionSkillTooLowTrans;

		// Token: 0x04002340 RID: 9024
		private static string IncapableOfDeconstruction;
	}
}
