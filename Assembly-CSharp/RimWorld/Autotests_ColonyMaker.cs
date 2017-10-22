using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public static class Autotests_ColonyMaker
	{
		private static CellRect overRect;

		private static BoolGrid usedCells;

		private const int OverRectSize = 100;

		private static Map Map
		{
			get
			{
				return Find.VisibleMap;
			}
		}

		public static void MakeColony_Full()
		{
			Autotests_ColonyMaker.MakeColony(ColonyMakerFlag.ConduitGrid, ColonyMakerFlag.PowerPlants, ColonyMakerFlag.Batteries, ColonyMakerFlag.WorkTables, ColonyMakerFlag.AllBuildings, ColonyMakerFlag.AllItems, ColonyMakerFlag.Filth, ColonyMakerFlag.ColonistsMany, ColonyMakerFlag.ColonistsHungry, ColonyMakerFlag.ColonistsTired, ColonyMakerFlag.ColonistsInjured, ColonyMakerFlag.ColonistsDiseased, ColonyMakerFlag.Beds, ColonyMakerFlag.Stockpiles, ColonyMakerFlag.GrowingZones);
		}

		public static void MakeColony_Animals()
		{
			Autotests_ColonyMaker.MakeColony(default(ColonyMakerFlag));
		}

		public static void MakeColony(params ColonyMakerFlag[] flags)
		{
			bool godMode = DebugSettings.godMode;
			DebugSettings.godMode = true;
			Thing.allowDestroyNonDestroyable = true;
			if (Autotests_ColonyMaker.usedCells == null)
			{
				Autotests_ColonyMaker.usedCells = new BoolGrid(Autotests_ColonyMaker.Map);
			}
			else
			{
				Autotests_ColonyMaker.usedCells.ClearAndResizeTo(Autotests_ColonyMaker.Map);
			}
			IntVec3 center = Autotests_ColonyMaker.Map.Center;
			int minX = center.x - 50;
			IntVec3 center2 = Autotests_ColonyMaker.Map.Center;
			Autotests_ColonyMaker.overRect = new CellRect(minX, center2.z - 50, 100, 100);
			Autotests_ColonyMaker.DeleteAllSpawnedPawns();
			GenDebug.ClearArea(Autotests_ColonyMaker.overRect, Find.VisibleMap);
			if (flags.Contains(ColonyMakerFlag.Animals))
			{
				foreach (PawnKindDef item in from k in DefDatabase<PawnKindDef>.AllDefs
				where k.RaceProps.Animal
				select k)
				{
					CellRect cellRect = default(CellRect);
					if (Autotests_ColonyMaker.TryGetFreeRect(6, 3, out cellRect))
					{
						cellRect = cellRect.ContractedBy(1);
						foreach (IntVec3 item2 in cellRect)
						{
							Autotests_ColonyMaker.Map.terrainGrid.SetTerrain(item2, TerrainDefOf.Concrete);
						}
						GenSpawn.Spawn(PawnGenerator.GeneratePawn(item, null), cellRect.Cells.ElementAt(0), Autotests_ColonyMaker.Map);
						IntVec3 intVec = cellRect.Cells.ElementAt(1);
						Pawn p = (Pawn)GenSpawn.Spawn(PawnGenerator.GeneratePawn(item, null), intVec, Autotests_ColonyMaker.Map);
						HealthUtility.DamageUntilDead(p);
						Corpse thing = (Corpse)intVec.GetThingList(Find.VisibleMap).First((Func<Thing, bool>)((Thing t) => t is Corpse));
						CompRottable compRottable = thing.TryGetComp<CompRottable>();
						if (compRottable != null)
						{
							compRottable.RotProgress += 1200000f;
						}
						if (item.RaceProps.leatherDef != null)
						{
							GenSpawn.Spawn(item.RaceProps.leatherDef, cellRect.Cells.ElementAt(2), Autotests_ColonyMaker.Map);
						}
						if (item.RaceProps.meatDef != null)
						{
							GenSpawn.Spawn(item.RaceProps.meatDef, cellRect.Cells.ElementAt(3), Autotests_ColonyMaker.Map);
						}
						continue;
					}
					return;
				}
			}
			if (flags.Contains(ColonyMakerFlag.ConduitGrid))
			{
				Designator_Build designator_Build = new Designator_Build(ThingDefOf.PowerConduit);
				for (int i = Autotests_ColonyMaker.overRect.minX; i < Autotests_ColonyMaker.overRect.maxX; i++)
				{
					for (int num = Autotests_ColonyMaker.overRect.minZ; num < Autotests_ColonyMaker.overRect.maxZ; num += 7)
					{
						designator_Build.DesignateSingleCell(new IntVec3(i, 0, num));
					}
				}
				for (int j = Autotests_ColonyMaker.overRect.minZ; j < Autotests_ColonyMaker.overRect.maxZ; j++)
				{
					for (int num2 = Autotests_ColonyMaker.overRect.minX; num2 < Autotests_ColonyMaker.overRect.maxX; num2 += 7)
					{
						designator_Build.DesignateSingleCell(new IntVec3(num2, 0, j));
					}
				}
			}
			if (flags.Contains(ColonyMakerFlag.PowerPlants))
			{
				List<ThingDef> list = new List<ThingDef>();
				list.Add(ThingDefOf.SolarGenerator);
				list.Add(ThingDefOf.WindTurbine);
				List<ThingDef> list2 = list;
				int num3 = 0;
				while (num3 < 8)
				{
					if (Autotests_ColonyMaker.TryMakeBuilding(list2[num3 % list2.Count]) != null)
					{
						num3++;
						continue;
					}
					Log.Message("Could not make solar generator.");
					break;
				}
			}
			if (flags.Contains(ColonyMakerFlag.Batteries))
			{
				for (int l = 0; l < 6; l++)
				{
					Thing thing2 = Autotests_ColonyMaker.TryMakeBuilding(ThingDefOf.Battery);
					if (thing2 == null)
					{
						Log.Message("Could not make battery.");
						break;
					}
					((Building_Battery)thing2).GetComp<CompPowerBattery>().AddEnergy(999999f);
				}
			}
			if (flags.Contains(ColonyMakerFlag.WorkTables))
			{
				IEnumerable<ThingDef> enumerable = from def in DefDatabase<ThingDef>.AllDefs
				where typeof(Building_WorkTable).IsAssignableFrom(def.thingClass)
				select def;
				foreach (ThingDef item3 in enumerable)
				{
					Thing thing3 = Autotests_ColonyMaker.TryMakeBuilding(item3);
					if (thing3 == null)
					{
						Log.Message("Could not make worktable: " + item3.defName);
						break;
					}
					Building_WorkTable building_WorkTable = thing3 as Building_WorkTable;
					if (building_WorkTable != null)
					{
						foreach (RecipeDef allRecipe in building_WorkTable.def.AllRecipes)
						{
							building_WorkTable.billStack.AddBill(allRecipe.MakeNewBill());
						}
					}
				}
			}
			if (flags.Contains(ColonyMakerFlag.AllBuildings))
			{
				IEnumerable<ThingDef> enumerable2 = from def in DefDatabase<ThingDef>.AllDefs
				where def.category == ThingCategory.Building && def.designationCategory != null
				select def;
				foreach (ThingDef item4 in enumerable2)
				{
					if (item4 != ThingDefOf.PowerConduit)
					{
						Thing thing4 = Autotests_ColonyMaker.TryMakeBuilding(item4);
						if (thing4 == null)
						{
							Log.Message("Could not make building: " + item4.defName);
							break;
						}
					}
				}
			}
			CellRect rect = default(CellRect);
			if (!Autotests_ColonyMaker.TryGetFreeRect(33, 33, out rect))
			{
				Log.Error("Could not get wallable rect");
			}
			rect = rect.ContractedBy(1);
			if (flags.Contains(ColonyMakerFlag.AllItems))
			{
				List<ThingDef> itemDefs = (from def in DefDatabase<ThingDef>.AllDefs
				where DebugThingPlaceHelper.IsDebugSpawnable(def) && def.category == ThingCategory.Item
				select def).ToList();
				Autotests_ColonyMaker.FillWithItems(rect, itemDefs);
			}
			else if (flags.Contains(ColonyMakerFlag.ItemsRawFood))
			{
				List<ThingDef> list3 = new List<ThingDef>();
				list3.Add(ThingDefOf.RawPotatoes);
				Autotests_ColonyMaker.FillWithItems(rect, list3);
			}
			if (flags.Contains(ColonyMakerFlag.Filth))
			{
				foreach (IntVec3 item5 in rect)
				{
					GenSpawn.Spawn(ThingDefOf.FilthDirt, item5, Autotests_ColonyMaker.Map);
				}
			}
			if (flags.Contains(ColonyMakerFlag.ItemsWall))
			{
				CellRect cellRect2 = rect.ExpandedBy(1);
				Designator_Build designator_Build2 = new Designator_Build(ThingDefOf.Wall);
				designator_Build2.SetStuffDef(ThingDefOf.WoodLog);
				foreach (IntVec3 edgeCell in cellRect2.EdgeCells)
				{
					designator_Build2.DesignateSingleCell(edgeCell);
				}
			}
			if (flags.Contains(ColonyMakerFlag.ColonistsMany))
			{
				Autotests_ColonyMaker.MakeColonists(15, Autotests_ColonyMaker.overRect.CenterCell);
			}
			else if (flags.Contains(ColonyMakerFlag.ColonistOne))
			{
				Autotests_ColonyMaker.MakeColonists(1, Autotests_ColonyMaker.overRect.CenterCell);
			}
			if (flags.Contains(ColonyMakerFlag.Fire))
			{
				CellRect cellRect3 = default(CellRect);
				if (!Autotests_ColonyMaker.TryGetFreeRect(30, 30, out cellRect3))
				{
					Log.Error("Could not get free rect for fire.");
				}
				ThingDef plantTreeOak = ThingDefOf.PlantTreeOak;
				foreach (IntVec3 item6 in cellRect3)
				{
					GenSpawn.Spawn(plantTreeOak, item6, Autotests_ColonyMaker.Map);
				}
				foreach (IntVec3 item7 in cellRect3)
				{
					IntVec3 current9 = item7;
					if (current9.x % 7 == 0 && current9.z % 7 == 0)
					{
						GenExplosion.DoExplosion(current9, Find.VisibleMap, 3.9f, DamageDefOf.Flame, null, -1, null, null, null, null, 0f, 1, false, null, 0f, 1, 0f, false);
					}
				}
			}
			if (flags.Contains(ColonyMakerFlag.ColonistsHungry))
			{
				Autotests_ColonyMaker.DoToColonists(0.4f, (Action<Pawn>)delegate(Pawn col)
				{
					col.needs.food.CurLevel = Mathf.Max(0f, Rand.Range(-0.05f, 0.05f));
				});
			}
			if (flags.Contains(ColonyMakerFlag.ColonistsTired))
			{
				Autotests_ColonyMaker.DoToColonists(0.4f, (Action<Pawn>)delegate(Pawn col)
				{
					col.needs.rest.CurLevel = Mathf.Max(0f, Rand.Range(-0.05f, 0.05f));
				});
			}
			if (flags.Contains(ColonyMakerFlag.ColonistsInjured))
			{
				Autotests_ColonyMaker.DoToColonists(0.4f, (Action<Pawn>)delegate(Pawn col)
				{
					DamageDef def2 = (from d in DefDatabase<DamageDef>.AllDefs
					where d.externalViolence
					select d).RandomElement();
					col.TakeDamage(new DamageInfo(def2, 10, -1f, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown));
				});
			}
			if (flags.Contains(ColonyMakerFlag.ColonistsDiseased))
			{
				foreach (HediffDef item8 in from d in DefDatabase<HediffDef>.AllDefs
				where d.hediffClass != typeof(Hediff_AddedPart) && (d.HasComp(typeof(HediffComp_Immunizable)) || d.HasComp(typeof(HediffComp_GrowthMode)))
				select d)
				{
					Pawn pawn = PawnGenerator.GeneratePawn(Faction.OfPlayer.def.basicMemberKind, Faction.OfPlayer);
					CellRect cellRect4 = default(CellRect);
					Autotests_ColonyMaker.TryGetFreeRect(1, 1, out cellRect4);
					GenSpawn.Spawn(pawn, cellRect4.CenterCell, Autotests_ColonyMaker.Map);
					pawn.health.AddHediff(item8, null, default(DamageInfo?));
				}
			}
			if (flags.Contains(ColonyMakerFlag.Beds))
			{
				IEnumerable<ThingDef> source = from def in DefDatabase<ThingDef>.AllDefs
				where def.thingClass == typeof(Building_Bed)
				select def;
				int freeColonistsCount = Autotests_ColonyMaker.Map.mapPawns.FreeColonistsCount;
				int num4 = 0;
				while (num4 < freeColonistsCount)
				{
					if (Autotests_ColonyMaker.TryMakeBuilding(source.RandomElement()) != null)
					{
						num4++;
						continue;
					}
					Log.Message("Could not make beds.");
					break;
				}
			}
			if (flags.Contains(ColonyMakerFlag.Stockpiles))
			{
				Designator_ZoneAddStockpile_Resources designator_ZoneAddStockpile_Resources = new Designator_ZoneAddStockpile_Resources();
				IEnumerator enumerator11 = Enum.GetValues(typeof(StoragePriority)).GetEnumerator();
				try
				{
					while (enumerator11.MoveNext())
					{
						StoragePriority priority = (StoragePriority)enumerator11.Current;
						CellRect cellRect5 = default(CellRect);
						Autotests_ColonyMaker.TryGetFreeRect(7, 7, out cellRect5);
						cellRect5 = cellRect5.ContractedBy(1);
						designator_ZoneAddStockpile_Resources.DesignateMultiCell(cellRect5.Cells);
						Zone_Stockpile zone_Stockpile = (Zone_Stockpile)Autotests_ColonyMaker.Map.zoneManager.ZoneAt(cellRect5.CenterCell);
						zone_Stockpile.settings.Priority = priority;
					}
				}
				finally
				{
					IDisposable disposable;
					if ((disposable = (enumerator11 as IDisposable)) != null)
					{
						disposable.Dispose();
					}
				}
			}
			if (flags.Contains(ColonyMakerFlag.GrowingZones))
			{
				Zone_Growing dummyZone = new Zone_Growing(Autotests_ColonyMaker.Map.zoneManager);
				foreach (ThingDef item9 in from d in DefDatabase<ThingDef>.AllDefs
				where d.plant != null && GenPlant.CanSowOnGrower(d, dummyZone)
				select d)
				{
					CellRect cellRect6 = default(CellRect);
					if (!Autotests_ColonyMaker.TryGetFreeRect(6, 6, out cellRect6))
					{
						Log.Error("Could not get growing zone rect.");
					}
					cellRect6 = cellRect6.ContractedBy(1);
					foreach (IntVec3 item10 in cellRect6)
					{
						Autotests_ColonyMaker.Map.terrainGrid.SetTerrain(item10, TerrainDefOf.Soil);
					}
					Designator_ZoneAdd_Growing designator_ZoneAdd_Growing = new Designator_ZoneAdd_Growing();
					designator_ZoneAdd_Growing.DesignateMultiCell(cellRect6.Cells);
					Zone_Growing zone_Growing = (Zone_Growing)Autotests_ColonyMaker.Map.zoneManager.ZoneAt(cellRect6.CenterCell);
					zone_Growing.SetPlantDefToGrow(item9);
				}
				dummyZone.Delete();
			}
			Autotests_ColonyMaker.ClearAllHomeArea();
			Autotests_ColonyMaker.FillWithHomeArea(Autotests_ColonyMaker.overRect);
			DebugSettings.godMode = godMode;
			Thing.allowDestroyNonDestroyable = false;
		}

		private static void FillWithItems(CellRect rect, List<ThingDef> itemDefs)
		{
			int num = 0;
			foreach (IntVec3 item in rect)
			{
				IntVec3 current = item;
				if (((current.x % 6 != 0) ? (current.z % 6) : 0) != 0)
				{
					ThingDef def = itemDefs[num];
					DebugThingPlaceHelper.DebugSpawn(def, current, -1, true);
					num++;
					if (num >= itemDefs.Count)
					{
						num = 0;
					}
				}
			}
		}

		private static Thing TryMakeBuilding(ThingDef def)
		{
			CellRect cellRect = default(CellRect);
			Thing result;
			if (!Autotests_ColonyMaker.TryGetFreeRect(def.size.x + 2, def.size.z + 2, out cellRect))
			{
				result = null;
			}
			else
			{
				foreach (IntVec3 item in cellRect)
				{
					Autotests_ColonyMaker.Map.terrainGrid.SetTerrain(item, TerrainDefOf.Concrete);
				}
				Designator_Build designator_Build = new Designator_Build(def);
				designator_Build.DesignateSingleCell(cellRect.CenterCell);
				Thing thing = result = cellRect.CenterCell.GetEdifice(Find.VisibleMap);
			}
			return result;
		}

		private static bool TryGetFreeRect(int width, int height, out CellRect result)
		{
			int num = Autotests_ColonyMaker.overRect.minZ;
			bool result2;
			while (true)
			{
				CellRect cellRect;
				if (num <= Autotests_ColonyMaker.overRect.maxZ - height)
				{
					for (int i = Autotests_ColonyMaker.overRect.minX; i <= Autotests_ColonyMaker.overRect.maxX - width; i++)
					{
						cellRect = new CellRect(i, num, width, height);
						bool flag = true;
						int num2 = cellRect.minZ;
						while (num2 <= cellRect.maxZ)
						{
							int num3 = cellRect.minX;
							while (num3 <= cellRect.maxX)
							{
								if (!Autotests_ColonyMaker.usedCells[num3, num2])
								{
									num3++;
									continue;
								}
								flag = false;
								break;
							}
							if (flag)
							{
								num2++;
								continue;
							}
							break;
						}
						if (flag)
							goto IL_00a4;
					}
					num++;
					continue;
				}
				result = new CellRect(0, 0, width, height);
				result2 = false;
				break;
				IL_00a4:
				result = cellRect;
				for (int j = cellRect.minZ; j <= cellRect.maxZ; j++)
				{
					for (int k = cellRect.minX; k <= cellRect.maxX; k++)
					{
						IntVec3 c = new IntVec3(k, 0, j);
						Autotests_ColonyMaker.usedCells.Set(c, true);
						if (c.GetTerrain(Find.VisibleMap).passability == Traversability.Impassable)
						{
							Autotests_ColonyMaker.Map.terrainGrid.SetTerrain(c, TerrainDefOf.Concrete);
						}
					}
				}
				result2 = true;
				break;
			}
			return result2;
		}

		private static void DoToColonists(float fraction, Action<Pawn> funcToDo)
		{
			int num = Rand.RangeInclusive(1, Mathf.RoundToInt((float)Autotests_ColonyMaker.Map.mapPawns.FreeColonistsCount * fraction));
			int num2 = 0;
			foreach (Pawn item in Autotests_ColonyMaker.Map.mapPawns.FreeColonists.InRandomOrder(null))
			{
				funcToDo(item);
				num2++;
				if (num2 >= num)
					break;
			}
		}

		private static void MakeColonists(int count, IntVec3 center)
		{
			for (int num = 0; num < count; num++)
			{
				CellRect cellRect = default(CellRect);
				Autotests_ColonyMaker.TryGetFreeRect(1, 1, out cellRect);
				Pawn pawn = PawnGenerator.GeneratePawn(Faction.OfPlayer.def.basicMemberKind, Faction.OfPlayer);
				foreach (WorkTypeDef allDef in DefDatabase<WorkTypeDef>.AllDefs)
				{
					if (!pawn.story.WorkTypeIsDisabled(allDef))
					{
						pawn.workSettings.SetPriority(allDef, 3);
					}
				}
				GenSpawn.Spawn(pawn, cellRect.CenterCell, Autotests_ColonyMaker.Map);
			}
		}

		private static void DeleteAllSpawnedPawns()
		{
			foreach (Pawn item in Autotests_ColonyMaker.Map.mapPawns.AllPawnsSpawned.ToList())
			{
				item.Destroy(DestroyMode.Vanish);
				item.relations.ClearAllRelations();
			}
			Find.GameEnder.gameEnding = false;
		}

		private static void ClearAllHomeArea()
		{
			foreach (IntVec3 allCell in Autotests_ColonyMaker.Map.AllCells)
			{
				((Area)Autotests_ColonyMaker.Map.areaManager.Home)[allCell] = false;
			}
		}

		private static void FillWithHomeArea(CellRect r)
		{
			Designator_AreaHomeExpand designator_AreaHomeExpand = new Designator_AreaHomeExpand();
			designator_AreaHomeExpand.DesignateMultiCell(r.Cells);
		}
	}
}
