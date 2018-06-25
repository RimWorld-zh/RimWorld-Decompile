using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;

namespace Verse
{
	// Token: 0x02000E17 RID: 3607
	[HasDebugOutput]
	internal static class DebugOutputsEcology
	{
		// Token: 0x060051E7 RID: 20967 RVA: 0x0029F648 File Offset: 0x0029DA48
		[DebugOutput]
		public static void PlantsBasics()
		{
			IEnumerable<ThingDef> dataSources = from d in DefDatabase<ThingDef>.AllDefs
			where d.category == ThingCategory.Plant
			orderby d.plant.fertilitySensitivity
			select d;
			TableDataGetter<ThingDef>[] array = new TableDataGetter<ThingDef>[6];
			array[0] = new TableDataGetter<ThingDef>("defName", (ThingDef d) => d.defName);
			array[1] = new TableDataGetter<ThingDef>("growDays", (ThingDef d) => d.plant.growDays.ToString("F2"));
			array[2] = new TableDataGetter<ThingDef>("nutrition", (ThingDef d) => (d.ingestible == null) ? "-" : d.GetStatValueAbstract(StatDefOf.Nutrition, null).ToString("F2"));
			array[3] = new TableDataGetter<ThingDef>("nut/day", (ThingDef d) => (d.ingestible == null) ? "-" : (d.GetStatValueAbstract(StatDefOf.Nutrition, null) / d.plant.growDays).ToString("F4"));
			array[4] = new TableDataGetter<ThingDef>("fertilityMin", (ThingDef d) => d.plant.fertilityMin.ToString("F2"));
			array[5] = new TableDataGetter<ThingDef>("fertilitySensitivity", (ThingDef d) => d.plant.fertilitySensitivity.ToString("F2"));
			DebugTables.MakeTablesDialog<ThingDef>(dataSources, array);
		}

		// Token: 0x060051E8 RID: 20968 RVA: 0x0029F7A6 File Offset: 0x0029DBA6
		[DebugOutput]
		[ModeRestrictionPlay]
		public static void PlantCurrentProportions()
		{
			GenPlant.LogPlantProportions();
		}

		// Token: 0x060051E9 RID: 20969 RVA: 0x0029F7B0 File Offset: 0x0029DBB0
		[DebugOutput]
		public static void Biomes()
		{
			IEnumerable<BiomeDef> dataSources = from d in DefDatabase<BiomeDef>.AllDefs
			orderby d.plantDensity descending
			select d;
			TableDataGetter<BiomeDef>[] array = new TableDataGetter<BiomeDef>[10];
			array[0] = new TableDataGetter<BiomeDef>("defName", (BiomeDef d) => d.defName);
			array[1] = new TableDataGetter<BiomeDef>("animalDensity", (BiomeDef d) => d.animalDensity.ToString("F2"));
			array[2] = new TableDataGetter<BiomeDef>("plantDensity", (BiomeDef d) => d.plantDensity.ToString("F2"));
			array[3] = new TableDataGetter<BiomeDef>("diseaseMtbDays", (BiomeDef d) => d.diseaseMtbDays.ToString("F0"));
			array[4] = new TableDataGetter<BiomeDef>("movementDifficulty", (BiomeDef d) => (!d.impassable) ? d.movementDifficulty.ToString("F1") : "-");
			array[5] = new TableDataGetter<BiomeDef>("forageability", (BiomeDef d) => d.forageability.ToStringPercent());
			array[6] = new TableDataGetter<BiomeDef>("forageFood", (BiomeDef d) => (d.foragedFood == null) ? "" : d.foragedFood.label);
			array[7] = new TableDataGetter<BiomeDef>("forageable plants", (BiomeDef d) => (from pd in d.AllWildPlants
			where pd.plant.harvestedThingDef != null && pd.plant.harvestedThingDef.IsNutritionGivingIngestible
			select pd.defName).ToCommaList(false));
			array[8] = new TableDataGetter<BiomeDef>("wildPlantRegrowDays", (BiomeDef d) => d.wildPlantRegrowDays.ToString("F0"));
			array[9] = new TableDataGetter<BiomeDef>("wildPlantsCareAboutLocalFertility", (BiomeDef d) => d.wildPlantsCareAboutLocalFertility.ToStringCheckBlank());
			DebugTables.MakeTablesDialog<BiomeDef>(dataSources, array);
		}

		// Token: 0x060051EA RID: 20970 RVA: 0x0029F996 File Offset: 0x0029DD96
		[DebugOutput]
		public static void BiomeAnimalsSpawnChances()
		{
			DebugOutputsEcology.BiomeAnimalsInternal(delegate(PawnKindDef k, BiomeDef b)
			{
				float num = b.CommonalityOfAnimal(k);
				string result;
				if (num == 0f)
				{
					result = "";
				}
				else
				{
					float f = num / DefDatabase<PawnKindDef>.AllDefs.Sum((PawnKindDef ki) => b.CommonalityOfAnimal(ki));
					result = f.ToStringPercent("F1");
				}
				return result;
			});
		}

		// Token: 0x060051EB RID: 20971 RVA: 0x0029F9BB File Offset: 0x0029DDBB
		[DebugOutput]
		public static void BiomeAnimalsTypicalCounts()
		{
			DebugOutputsEcology.BiomeAnimalsInternal((PawnKindDef k, BiomeDef b) => DebugOutputsEcology.ExpectedAnimalCount(k, b).ToStringEmptyZero("F2"));
		}

		// Token: 0x060051EC RID: 20972 RVA: 0x0029F9E0 File Offset: 0x0029DDE0
		private static float ExpectedAnimalCount(PawnKindDef k, BiomeDef b)
		{
			float num = b.CommonalityOfAnimal(k);
			float result;
			if (num == 0f)
			{
				result = 0f;
			}
			else
			{
				float num2 = DefDatabase<PawnKindDef>.AllDefs.Sum((PawnKindDef ki) => b.CommonalityOfAnimal(ki));
				float num3 = num / num2;
				float num4 = 10000f / b.animalDensity;
				float num5 = 62500f / num4;
				float totalCommonality = DefDatabase<PawnKindDef>.AllDefs.Sum((PawnKindDef ki) => b.CommonalityOfAnimal(ki));
				float num6 = DefDatabase<PawnKindDef>.AllDefs.Sum((PawnKindDef ki) => k.ecoSystemWeight * (b.CommonalityOfAnimal(ki) / totalCommonality));
				float num7 = num5 / num6;
				float num8 = num7 * num3;
				result = num8;
			}
			return result;
		}

		// Token: 0x060051ED RID: 20973 RVA: 0x0029FAB0 File Offset: 0x0029DEB0
		private static void BiomeAnimalsInternal(Func<PawnKindDef, BiomeDef, string> densityInBiomeOutputter)
		{
			List<TableDataGetter<PawnKindDef>> list = (from b in DefDatabase<BiomeDef>.AllDefs
			where b.implemented && b.canBuildBase
			orderby b.animalDensity
			select new TableDataGetter<PawnKindDef>(b.defName, (PawnKindDef k) => densityInBiomeOutputter(k, b))).ToList<TableDataGetter<PawnKindDef>>();
			list.Insert(0, new TableDataGetter<PawnKindDef>("animal", (PawnKindDef k) => k.defName + "" + ((!k.race.race.predator) ? "" : " (P)")));
			DebugTables.MakeTablesDialog<PawnKindDef>(from d in DefDatabase<PawnKindDef>.AllDefs
			where d.race != null && d.RaceProps.Animal
			orderby d.defName
			select d, list.ToArray());
		}

		// Token: 0x060051EE RID: 20974 RVA: 0x0029FBB0 File Offset: 0x0029DFB0
		[DebugOutput]
		public static void BiomePlantsExpectedCount()
		{
			Func<ThingDef, BiomeDef, string> expectedCountInBiomeOutputter = (ThingDef p, BiomeDef b) => (b.CommonalityOfPlant(p) * b.plantDensity * 4000f).ToString("F0");
			List<TableDataGetter<ThingDef>> list = (from b in DefDatabase<BiomeDef>.AllDefs
			where b.implemented && b.canBuildBase
			orderby b.plantDensity
			select new TableDataGetter<ThingDef>(b.defName + " (" + b.plantDensity.ToString("F2") + ")", (ThingDef k) => expectedCountInBiomeOutputter(k, b))).ToList<TableDataGetter<ThingDef>>();
			list.Insert(0, new TableDataGetter<ThingDef>("plant", (ThingDef k) => k.defName));
			DebugTables.MakeTablesDialog<ThingDef>(from d in DefDatabase<ThingDef>.AllDefs
			where d.category == ThingCategory.Plant
			orderby d.defName
			select d, list.ToArray());
		}

		// Token: 0x060051EF RID: 20975 RVA: 0x0029FCCC File Offset: 0x0029E0CC
		[DebugOutput]
		public static void AnimalWildCountsOnMap()
		{
			Map map = Find.CurrentMap;
			IEnumerable<PawnKindDef> dataSources = from k in DefDatabase<PawnKindDef>.AllDefs
			where k.race != null && k.RaceProps.Animal && DebugOutputsEcology.ExpectedAnimalCount(k, map.Biome) > 0f
			orderby DebugOutputsEcology.ExpectedAnimalCount(k, map.Biome) descending
			select k;
			TableDataGetter<PawnKindDef>[] array = new TableDataGetter<PawnKindDef>[3];
			array[0] = new TableDataGetter<PawnKindDef>("animal", (PawnKindDef k) => k.defName);
			array[1] = new TableDataGetter<PawnKindDef>("expected count on map (inaccurate)", (PawnKindDef k) => DebugOutputsEcology.ExpectedAnimalCount(k, map.Biome).ToString("F2"));
			array[2] = new TableDataGetter<PawnKindDef>("actual count on map", (PawnKindDef k) => (from p in map.mapPawns.AllPawnsSpawned
			where p.kindDef == k
			select p).Count<Pawn>().ToString());
			DebugTables.MakeTablesDialog<PawnKindDef>(dataSources, array);
		}

		// Token: 0x060051F0 RID: 20976 RVA: 0x0029FD7C File Offset: 0x0029E17C
		[DebugOutput]
		public static void PlantCountsOnMap()
		{
			Map map = Find.CurrentMap;
			IEnumerable<ThingDef> dataSources = from p in DefDatabase<ThingDef>.AllDefs
			where p.category == ThingCategory.Plant && map.Biome.CommonalityOfPlant(p) > 0f
			orderby map.Biome.CommonalityOfPlant(p) descending
			select p;
			TableDataGetter<ThingDef>[] array = new TableDataGetter<ThingDef>[4];
			array[0] = new TableDataGetter<ThingDef>("plant", (ThingDef p) => p.defName);
			array[1] = new TableDataGetter<ThingDef>("biome-defined commonality", (ThingDef p) => map.Biome.CommonalityOfPlant(p).ToString("F2"));
			array[2] = new TableDataGetter<ThingDef>("expected count (rough)", (ThingDef p) => (map.Biome.CommonalityOfPlant(p) * map.Biome.plantDensity * 4000f).ToString("F0"));
			array[3] = new TableDataGetter<ThingDef>("actual count on map", (ThingDef p) => (from c in map.AllCells
			where c.GetPlant(map) != null && c.GetPlant(map).def == p
			select c).Count<IntVec3>().ToString());
			DebugTables.MakeTablesDialog<ThingDef>(dataSources, array);
		}
	}
}
