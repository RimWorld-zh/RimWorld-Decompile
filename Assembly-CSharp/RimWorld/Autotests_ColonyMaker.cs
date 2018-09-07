using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public static class Autotests_ColonyMaker
	{
		private static CellRect overRect;

		private static BoolGrid usedCells;

		private const int OverRectSize = 100;

		[CompilerGenerated]
		private static Func<PawnKindDef, bool> <>f__am$cache0;

		[CompilerGenerated]
		private static Func<Thing, bool> <>f__am$cache1;

		[CompilerGenerated]
		private static Func<ThingDef, bool> <>f__am$cache2;

		[CompilerGenerated]
		private static Func<ThingDef, bool> <>f__am$cache3;

		[CompilerGenerated]
		private static Func<ThingDef, bool> <>f__am$cache4;

		[CompilerGenerated]
		private static Action<Pawn> <>f__am$cache5;

		[CompilerGenerated]
		private static Action<Pawn> <>f__am$cache6;

		[CompilerGenerated]
		private static Action<Pawn> <>f__am$cache7;

		[CompilerGenerated]
		private static Func<HediffDef, bool> <>f__am$cache8;

		[CompilerGenerated]
		private static Func<ThingDef, bool> <>f__am$cache9;

		[CompilerGenerated]
		private static Func<DamageDef, bool> <>f__am$cacheA;

		private static Map Map
		{
			get
			{
				return Find.CurrentMap;
			}
		}

		public static void MakeColony_Full()
		{
			Autotests_ColonyMaker.MakeColony(new ColonyMakerFlag[]
			{
				ColonyMakerFlag.ConduitGrid,
				ColonyMakerFlag.PowerPlants,
				ColonyMakerFlag.Batteries,
				ColonyMakerFlag.WorkTables,
				ColonyMakerFlag.AllBuildings,
				ColonyMakerFlag.AllItems,
				ColonyMakerFlag.Filth,
				ColonyMakerFlag.ColonistsMany,
				ColonyMakerFlag.ColonistsHungry,
				ColonyMakerFlag.ColonistsTired,
				ColonyMakerFlag.ColonistsInjured,
				ColonyMakerFlag.ColonistsDiseased,
				ColonyMakerFlag.Beds,
				ColonyMakerFlag.Stockpiles,
				ColonyMakerFlag.GrowingZones
			});
		}

		public static void MakeColony_Animals()
		{
			Autotests_ColonyMaker.MakeColony(new ColonyMakerFlag[1]);
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
			Autotests_ColonyMaker.overRect = new CellRect(Autotests_ColonyMaker.Map.Center.x - 50, Autotests_ColonyMaker.Map.Center.z - 50, 100, 100);
			Autotests_ColonyMaker.DeleteAllSpawnedPawns();
			GenDebug.ClearArea(Autotests_ColonyMaker.overRect, Find.CurrentMap);
			if (flags.Contains(ColonyMakerFlag.Animals))
			{
				foreach (PawnKindDef pawnKindDef in from k in DefDatabase<PawnKindDef>.AllDefs
				where k.RaceProps.Animal
				select k)
				{
					CellRect cellRect;
					if (!Autotests_ColonyMaker.TryGetFreeRect(6, 3, out cellRect))
					{
						return;
					}
					cellRect = cellRect.ContractedBy(1);
					foreach (IntVec3 c in cellRect)
					{
						Autotests_ColonyMaker.Map.terrainGrid.SetTerrain(c, TerrainDefOf.Concrete);
					}
					GenSpawn.Spawn(PawnGenerator.GeneratePawn(pawnKindDef, null), cellRect.Cells.ElementAt(0), Autotests_ColonyMaker.Map, WipeMode.Vanish);
					IntVec3 intVec = cellRect.Cells.ElementAt(1);
					Pawn p = (Pawn)GenSpawn.Spawn(PawnGenerator.GeneratePawn(pawnKindDef, null), intVec, Autotests_ColonyMaker.Map, WipeMode.Vanish);
					HealthUtility.DamageUntilDead(p);
					Corpse thing = (Corpse)intVec.GetThingList(Find.CurrentMap).First((Thing t) => t is Corpse);
					CompRottable compRottable = thing.TryGetComp<CompRottable>();
					if (compRottable != null)
					{
						compRottable.RotProgress += 1200000f;
					}
					if (pawnKindDef.RaceProps.leatherDef != null)
					{
						GenSpawn.Spawn(pawnKindDef.RaceProps.leatherDef, cellRect.Cells.ElementAt(2), Autotests_ColonyMaker.Map, WipeMode.Vanish);
					}
					if (pawnKindDef.RaceProps.meatDef != null)
					{
						GenSpawn.Spawn(pawnKindDef.RaceProps.meatDef, cellRect.Cells.ElementAt(3), Autotests_ColonyMaker.Map, WipeMode.Vanish);
					}
				}
			}
			if (flags.Contains(ColonyMakerFlag.ConduitGrid))
			{
				Designator_Build designator_Build = new Designator_Build(ThingDefOf.PowerConduit);
				for (int i = Autotests_ColonyMaker.overRect.minX; i < Autotests_ColonyMaker.overRect.maxX; i++)
				{
					for (int j = Autotests_ColonyMaker.overRect.minZ; j < Autotests_ColonyMaker.overRect.maxZ; j += 7)
					{
						designator_Build.DesignateSingleCell(new IntVec3(i, 0, j));
					}
				}
				for (int k2 = Autotests_ColonyMaker.overRect.minZ; k2 < Autotests_ColonyMaker.overRect.maxZ; k2++)
				{
					for (int l = Autotests_ColonyMaker.overRect.minX; l < Autotests_ColonyMaker.overRect.maxX; l += 7)
					{
						designator_Build.DesignateSingleCell(new IntVec3(l, 0, k2));
					}
				}
			}
			if (flags.Contains(ColonyMakerFlag.PowerPlants))
			{
				List<ThingDef> list = new List<ThingDef>
				{
					ThingDefOf.SolarGenerator,
					ThingDefOf.WindTurbine
				};
				for (int m = 0; m < 8; m++)
				{
					if (Autotests_ColonyMaker.TryMakeBuilding(list[m % list.Count]) == null)
					{
						Log.Message("Could not make solar generator.", false);
						break;
					}
				}
			}
			if (flags.Contains(ColonyMakerFlag.Batteries))
			{
				for (int n = 0; n < 6; n++)
				{
					Thing thing2 = Autotests_ColonyMaker.TryMakeBuilding(ThingDefOf.Battery);
					if (thing2 == null)
					{
						Log.Message("Could not make battery.", false);
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
				foreach (ThingDef thingDef in enumerable)
				{
					Thing thing3 = Autotests_ColonyMaker.TryMakeBuilding(thingDef);
					if (thing3 == null)
					{
						Log.Message("Could not make worktable: " + thingDef.defName, false);
						break;
					}
					Building_WorkTable building_WorkTable = thing3 as Building_WorkTable;
					if (building_WorkTable != null)
					{
						foreach (RecipeDef recipe in building_WorkTable.def.AllRecipes)
						{
							building_WorkTable.billStack.AddBill(recipe.MakeNewBill());
						}
					}
				}
			}
			if (flags.Contains(ColonyMakerFlag.AllBuildings))
			{
				IEnumerable<ThingDef> enumerable2 = from def in DefDatabase<ThingDef>.AllDefs
				where def.category == ThingCategory.Building && def.BuildableByPlayer
				select def;
				foreach (ThingDef thingDef2 in enumerable2)
				{
					if (thingDef2 != ThingDefOf.PowerConduit)
					{
						if (Autotests_ColonyMaker.TryMakeBuilding(thingDef2) == null)
						{
							Log.Message("Could not make building: " + thingDef2.defName, false);
							break;
						}
					}
				}
			}
			CellRect rect;
			if (!Autotests_ColonyMaker.TryGetFreeRect(33, 33, out rect))
			{
				Log.Error("Could not get wallable rect", false);
			}
			rect = rect.ContractedBy(1);
			if (flags.Contains(ColonyMakerFlag.AllItems))
			{
				List<ThingDef> itemDefs = (from def in DefDatabase<ThingDef>.AllDefs
				where DebugThingPlaceHelper.IsDebugSpawnable(def, false) && def.category == ThingCategory.Item
				select def).ToList<ThingDef>();
				Autotests_ColonyMaker.FillWithItems(rect, itemDefs);
			}
			else if (flags.Contains(ColonyMakerFlag.ItemsRawFood))
			{
				List<ThingDef> list2 = new List<ThingDef>();
				list2.Add(ThingDefOf.RawPotatoes);
				Autotests_ColonyMaker.FillWithItems(rect, list2);
			}
			if (flags.Contains(ColonyMakerFlag.Filth))
			{
				foreach (IntVec3 loc in rect)
				{
					GenSpawn.Spawn(ThingDefOf.Filth_Dirt, loc, Autotests_ColonyMaker.Map, WipeMode.Vanish);
				}
			}
			if (flags.Contains(ColonyMakerFlag.ItemsWall))
			{
				CellRect cellRect2 = rect.ExpandedBy(1);
				Designator_Build designator_Build2 = new Designator_Build(ThingDefOf.Wall);
				designator_Build2.SetStuffDef(ThingDefOf.WoodLog);
				foreach (IntVec3 c2 in cellRect2.EdgeCells)
				{
					designator_Build2.DesignateSingleCell(c2);
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
				CellRect cellRect3;
				if (!Autotests_ColonyMaker.TryGetFreeRect(30, 30, out cellRect3))
				{
					Log.Error("Could not get free rect for fire.", false);
				}
				ThingDef plant_TreeOak = ThingDefOf.Plant_TreeOak;
				foreach (IntVec3 loc2 in cellRect3)
				{
					GenSpawn.Spawn(plant_TreeOak, loc2, Autotests_ColonyMaker.Map, WipeMode.Vanish);
				}
				foreach (IntVec3 center in cellRect3)
				{
					if (center.x % 7 == 0 && center.z % 7 == 0)
					{
						GenExplosion.DoExplosion(center, Find.CurrentMap, 3.9f, DamageDefOf.Flame, null, -1, -1f, null, null, null, null, null, 0f, 1, false, null, 0f, 1, 0f, false);
					}
				}
			}
			if (flags.Contains(ColonyMakerFlag.ColonistsHungry))
			{
				Autotests_ColonyMaker.DoToColonists(0.4f, delegate(Pawn col)
				{
					col.needs.food.CurLevel = Mathf.Max(0f, Rand.Range(-0.05f, 0.05f));
				});
			}
			if (flags.Contains(ColonyMakerFlag.ColonistsTired))
			{
				Autotests_ColonyMaker.DoToColonists(0.4f, delegate(Pawn col)
				{
					col.needs.rest.CurLevel = Mathf.Max(0f, Rand.Range(-0.05f, 0.05f));
				});
			}
			if (flags.Contains(ColonyMakerFlag.ColonistsInjured))
			{
				Autotests_ColonyMaker.DoToColonists(0.4f, delegate(Pawn col)
				{
					DamageDef def3 = (from d in DefDatabase<DamageDef>.AllDefs
					where d.ExternalViolenceFor(null)
					select d).RandomElement<DamageDef>();
					col.TakeDamage(new DamageInfo(def3, 10f, 0f, -1f, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null));
				});
			}
			if (flags.Contains(ColonyMakerFlag.ColonistsDiseased))
			{
				foreach (HediffDef def2 in from d in DefDatabase<HediffDef>.AllDefs
				where d.hediffClass != typeof(Hediff_AddedPart) && (d.HasComp(typeof(HediffComp_Immunizable)) || d.HasComp(typeof(HediffComp_GrowthMode)))
				select d)
				{
					Pawn pawn = PawnGenerator.GeneratePawn(Faction.OfPlayer.def.basicMemberKind, Faction.OfPlayer);
					CellRect cellRect4;
					Autotests_ColonyMaker.TryGetFreeRect(1, 1, out cellRect4);
					GenSpawn.Spawn(pawn, cellRect4.CenterCell, Autotests_ColonyMaker.Map, WipeMode.Vanish);
					pawn.health.AddHediff(def2, null, null, null);
				}
			}
			if (flags.Contains(ColonyMakerFlag.Beds))
			{
				IEnumerable<ThingDef> source = from def in DefDatabase<ThingDef>.AllDefs
				where def.thingClass == typeof(Building_Bed)
				select def;
				int freeColonistsCount = Autotests_ColonyMaker.Map.mapPawns.FreeColonistsCount;
				for (int num = 0; num < freeColonistsCount; num++)
				{
					if (Autotests_ColonyMaker.TryMakeBuilding(source.RandomElement<ThingDef>()) == null)
					{
						Log.Message("Could not make beds.", false);
						break;
					}
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
						object obj = enumerator11.Current;
						StoragePriority priority = (StoragePriority)obj;
						CellRect cellRect5;
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
				Autotests_ColonyMaker.Map.zoneManager.RegisterZone(dummyZone);
				foreach (ThingDef plantDefToGrow in from d in DefDatabase<ThingDef>.AllDefs
				where d.plant != null && PlantUtility.CanSowOnGrower(d, dummyZone)
				select d)
				{
					CellRect cellRect6;
					if (!Autotests_ColonyMaker.TryGetFreeRect(6, 6, out cellRect6))
					{
						Log.Error("Could not get growing zone rect.", false);
					}
					cellRect6 = cellRect6.ContractedBy(1);
					foreach (IntVec3 c3 in cellRect6)
					{
						Autotests_ColonyMaker.Map.terrainGrid.SetTerrain(c3, TerrainDefOf.Soil);
					}
					Designator_ZoneAdd_Growing designator_ZoneAdd_Growing = new Designator_ZoneAdd_Growing();
					designator_ZoneAdd_Growing.DesignateMultiCell(cellRect6.Cells);
					Zone_Growing zone_Growing = Autotests_ColonyMaker.Map.zoneManager.ZoneAt(cellRect6.CenterCell) as Zone_Growing;
					if (zone_Growing != null)
					{
						zone_Growing.SetPlantDefToGrow(plantDefToGrow);
					}
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
			foreach (IntVec3 c in rect)
			{
				if (c.x % 6 != 0 && c.z % 6 != 0)
				{
					ThingDef def = itemDefs[num];
					DebugThingPlaceHelper.DebugSpawn(def, c, -1, true);
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
			CellRect cellRect;
			if (!Autotests_ColonyMaker.TryGetFreeRect(def.size.x + 2, def.size.z + 2, out cellRect))
			{
				return null;
			}
			foreach (IntVec3 c in cellRect)
			{
				Autotests_ColonyMaker.Map.terrainGrid.SetTerrain(c, TerrainDefOf.Concrete);
			}
			Designator_Build designator_Build = new Designator_Build(def);
			designator_Build.DesignateSingleCell(cellRect.CenterCell);
			return cellRect.CenterCell.GetEdifice(Find.CurrentMap);
		}

		private static bool TryGetFreeRect(int width, int height, out CellRect result)
		{
			for (int i = Autotests_ColonyMaker.overRect.minZ; i <= Autotests_ColonyMaker.overRect.maxZ - height; i++)
			{
				for (int j = Autotests_ColonyMaker.overRect.minX; j <= Autotests_ColonyMaker.overRect.maxX - width; j++)
				{
					CellRect cellRect = new CellRect(j, i, width, height);
					bool flag = true;
					for (int k = cellRect.minZ; k <= cellRect.maxZ; k++)
					{
						for (int l = cellRect.minX; l <= cellRect.maxX; l++)
						{
							if (Autotests_ColonyMaker.usedCells[l, k])
							{
								flag = false;
								break;
							}
						}
						if (!flag)
						{
							break;
						}
					}
					if (flag)
					{
						result = cellRect;
						for (int m = cellRect.minZ; m <= cellRect.maxZ; m++)
						{
							for (int n = cellRect.minX; n <= cellRect.maxX; n++)
							{
								IntVec3 c = new IntVec3(n, 0, m);
								Autotests_ColonyMaker.usedCells.Set(c, true);
								if (c.GetTerrain(Find.CurrentMap).passability == Traversability.Impassable)
								{
									Autotests_ColonyMaker.Map.terrainGrid.SetTerrain(c, TerrainDefOf.Concrete);
								}
							}
						}
						return true;
					}
				}
			}
			result = new CellRect(0, 0, width, height);
			return false;
		}

		private static void DoToColonists(float fraction, Action<Pawn> funcToDo)
		{
			int num = Rand.RangeInclusive(1, Mathf.RoundToInt((float)Autotests_ColonyMaker.Map.mapPawns.FreeColonistsCount * fraction));
			int num2 = 0;
			foreach (Pawn obj in Autotests_ColonyMaker.Map.mapPawns.FreeColonists.InRandomOrder(null))
			{
				funcToDo(obj);
				num2++;
				if (num2 >= num)
				{
					break;
				}
			}
		}

		private static void MakeColonists(int count, IntVec3 center)
		{
			for (int i = 0; i < count; i++)
			{
				CellRect cellRect;
				Autotests_ColonyMaker.TryGetFreeRect(1, 1, out cellRect);
				Pawn pawn = PawnGenerator.GeneratePawn(Faction.OfPlayer.def.basicMemberKind, Faction.OfPlayer);
				foreach (WorkTypeDef w in DefDatabase<WorkTypeDef>.AllDefs)
				{
					if (!pawn.story.WorkTypeIsDisabled(w))
					{
						pawn.workSettings.SetPriority(w, 3);
					}
				}
				GenSpawn.Spawn(pawn, cellRect.CenterCell, Autotests_ColonyMaker.Map, WipeMode.Vanish);
			}
		}

		private static void DeleteAllSpawnedPawns()
		{
			foreach (Pawn pawn in Autotests_ColonyMaker.Map.mapPawns.AllPawnsSpawned.ToList<Pawn>())
			{
				pawn.Destroy(DestroyMode.Vanish);
				pawn.relations.ClearAllRelations();
			}
			Find.GameEnder.gameEnding = false;
		}

		private static void ClearAllHomeArea()
		{
			foreach (IntVec3 c in Autotests_ColonyMaker.Map.AllCells)
			{
				Autotests_ColonyMaker.Map.areaManager.Home[c] = false;
			}
		}

		private static void FillWithHomeArea(CellRect r)
		{
			Designator_AreaHomeExpand designator_AreaHomeExpand = new Designator_AreaHomeExpand();
			designator_AreaHomeExpand.DesignateMultiCell(r.Cells);
		}

		[CompilerGenerated]
		private static bool <MakeColony>m__0(PawnKindDef k)
		{
			return k.RaceProps.Animal;
		}

		[CompilerGenerated]
		private static bool <MakeColony>m__1(Thing t)
		{
			return t is Corpse;
		}

		[CompilerGenerated]
		private static bool <MakeColony>m__2(ThingDef def)
		{
			return typeof(Building_WorkTable).IsAssignableFrom(def.thingClass);
		}

		[CompilerGenerated]
		private static bool <MakeColony>m__3(ThingDef def)
		{
			return def.category == ThingCategory.Building && def.BuildableByPlayer;
		}

		[CompilerGenerated]
		private static bool <MakeColony>m__4(ThingDef def)
		{
			return DebugThingPlaceHelper.IsDebugSpawnable(def, false) && def.category == ThingCategory.Item;
		}

		[CompilerGenerated]
		private static void <MakeColony>m__5(Pawn col)
		{
			col.needs.food.CurLevel = Mathf.Max(0f, Rand.Range(-0.05f, 0.05f));
		}

		[CompilerGenerated]
		private static void <MakeColony>m__6(Pawn col)
		{
			col.needs.rest.CurLevel = Mathf.Max(0f, Rand.Range(-0.05f, 0.05f));
		}

		[CompilerGenerated]
		private static void <MakeColony>m__7(Pawn col)
		{
			DamageDef def = (from d in DefDatabase<DamageDef>.AllDefs
			where d.ExternalViolenceFor(null)
			select d).RandomElement<DamageDef>();
			col.TakeDamage(new DamageInfo(def, 10f, 0f, -1f, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null));
		}

		[CompilerGenerated]
		private static bool <MakeColony>m__8(HediffDef d)
		{
			return d.hediffClass != typeof(Hediff_AddedPart) && (d.HasComp(typeof(HediffComp_Immunizable)) || d.HasComp(typeof(HediffComp_GrowthMode)));
		}

		[CompilerGenerated]
		private static bool <MakeColony>m__9(ThingDef def)
		{
			return def.thingClass == typeof(Building_Bed);
		}

		[CompilerGenerated]
		private static bool <MakeColony>m__A(DamageDef d)
		{
			return d.ExternalViolenceFor(null);
		}

		[CompilerGenerated]
		private sealed class <MakeColony>c__AnonStorey0
		{
			internal Zone_Growing dummyZone;

			public <MakeColony>c__AnonStorey0()
			{
			}

			internal bool <>m__0(ThingDef d)
			{
				return d.plant != null && PlantUtility.CanSowOnGrower(d, this.dummyZone);
			}
		}
	}
}
