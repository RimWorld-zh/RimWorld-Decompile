using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using RimWorld;

namespace Verse
{
	[HasDebugOutput]
	public static class DebugOutputsEcology
	{
		[CompilerGenerated]
		private static Func<ThingDef, bool> <>f__am$cache0;

		[CompilerGenerated]
		private static Func<ThingDef, float> <>f__am$cache1;

		[CompilerGenerated]
		private static Func<ThingDef, string> <>f__am$cache2;

		[CompilerGenerated]
		private static Func<ThingDef, string> <>f__am$cache3;

		[CompilerGenerated]
		private static Func<ThingDef, string> <>f__am$cache4;

		[CompilerGenerated]
		private static Func<ThingDef, string> <>f__am$cache5;

		[CompilerGenerated]
		private static Func<ThingDef, string> <>f__am$cache6;

		[CompilerGenerated]
		private static Func<ThingDef, string> <>f__am$cache7;

		[CompilerGenerated]
		private static Func<BiomeDef, float> <>f__am$cache8;

		[CompilerGenerated]
		private static Func<BiomeDef, string> <>f__am$cache9;

		[CompilerGenerated]
		private static Func<BiomeDef, string> <>f__am$cacheA;

		[CompilerGenerated]
		private static Func<BiomeDef, string> <>f__am$cacheB;

		[CompilerGenerated]
		private static Func<BiomeDef, string> <>f__am$cacheC;

		[CompilerGenerated]
		private static Func<BiomeDef, string> <>f__am$cacheD;

		[CompilerGenerated]
		private static Func<BiomeDef, string> <>f__am$cacheE;

		[CompilerGenerated]
		private static Func<BiomeDef, string> <>f__am$cacheF;

		[CompilerGenerated]
		private static Func<BiomeDef, string> <>f__am$cache10;

		[CompilerGenerated]
		private static Func<BiomeDef, string> <>f__am$cache11;

		[CompilerGenerated]
		private static Func<BiomeDef, string> <>f__am$cache12;

		[CompilerGenerated]
		private static Func<PawnKindDef, BiomeDef, string> <>f__am$cache13;

		[CompilerGenerated]
		private static Func<PawnKindDef, BiomeDef, string> <>f__am$cache14;

		[CompilerGenerated]
		private static Func<BiomeDef, bool> <>f__am$cache15;

		[CompilerGenerated]
		private static Func<BiomeDef, float> <>f__am$cache16;

		[CompilerGenerated]
		private static Func<PawnKindDef, string> <>f__am$cache17;

		[CompilerGenerated]
		private static Func<PawnKindDef, bool> <>f__am$cache18;

		[CompilerGenerated]
		private static Func<PawnKindDef, string> <>f__am$cache19;

		[CompilerGenerated]
		private static Func<ThingDef, BiomeDef, string> <>f__am$cache1A;

		[CompilerGenerated]
		private static Func<BiomeDef, bool> <>f__am$cache1B;

		[CompilerGenerated]
		private static Func<BiomeDef, float> <>f__am$cache1C;

		[CompilerGenerated]
		private static Func<ThingDef, string> <>f__am$cache1D;

		[CompilerGenerated]
		private static Func<ThingDef, bool> <>f__am$cache1E;

		[CompilerGenerated]
		private static Func<ThingDef, string> <>f__am$cache1F;

		[CompilerGenerated]
		private static Func<PawnKindDef, string> <>f__am$cache20;

		[CompilerGenerated]
		private static Func<ThingDef, string> <>f__am$cache21;

		[CompilerGenerated]
		private static Func<ThingDef, bool> <>f__am$cache22;

		[CompilerGenerated]
		private static Func<ThingDef, string> <>f__am$cache23;

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

		[DebugOutput]
		[ModeRestrictionPlay]
		public static void PlantCurrentProportions()
		{
			PlantUtility.LogPlantProportions();
		}

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

		[DebugOutput]
		public static void BiomeAnimalsTypicalCounts()
		{
			DebugOutputsEcology.BiomeAnimalsInternal((PawnKindDef k, BiomeDef b) => DebugOutputsEcology.ExpectedAnimalCount(k, b).ToStringEmptyZero("F2"));
		}

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

		[CompilerGenerated]
		private static bool <PlantsBasics>m__0(ThingDef d)
		{
			return d.category == ThingCategory.Plant;
		}

		[CompilerGenerated]
		private static float <PlantsBasics>m__1(ThingDef d)
		{
			return d.plant.fertilitySensitivity;
		}

		[CompilerGenerated]
		private static string <PlantsBasics>m__2(ThingDef d)
		{
			return d.defName;
		}

		[CompilerGenerated]
		private static string <PlantsBasics>m__3(ThingDef d)
		{
			return d.plant.growDays.ToString("F2");
		}

		[CompilerGenerated]
		private static string <PlantsBasics>m__4(ThingDef d)
		{
			return (d.ingestible == null) ? "-" : d.GetStatValueAbstract(StatDefOf.Nutrition, null).ToString("F2");
		}

		[CompilerGenerated]
		private static string <PlantsBasics>m__5(ThingDef d)
		{
			return (d.ingestible == null) ? "-" : (d.GetStatValueAbstract(StatDefOf.Nutrition, null) / d.plant.growDays).ToString("F4");
		}

		[CompilerGenerated]
		private static string <PlantsBasics>m__6(ThingDef d)
		{
			return d.plant.fertilityMin.ToString("F2");
		}

		[CompilerGenerated]
		private static string <PlantsBasics>m__7(ThingDef d)
		{
			return d.plant.fertilitySensitivity.ToString("F2");
		}

		[CompilerGenerated]
		private static float <Biomes>m__8(BiomeDef d)
		{
			return d.plantDensity;
		}

		[CompilerGenerated]
		private static string <Biomes>m__9(BiomeDef d)
		{
			return d.defName;
		}

		[CompilerGenerated]
		private static string <Biomes>m__A(BiomeDef d)
		{
			return d.animalDensity.ToString("F2");
		}

		[CompilerGenerated]
		private static string <Biomes>m__B(BiomeDef d)
		{
			return d.plantDensity.ToString("F2");
		}

		[CompilerGenerated]
		private static string <Biomes>m__C(BiomeDef d)
		{
			return d.diseaseMtbDays.ToString("F0");
		}

		[CompilerGenerated]
		private static string <Biomes>m__D(BiomeDef d)
		{
			return (!d.impassable) ? d.movementDifficulty.ToString("F1") : "-";
		}

		[CompilerGenerated]
		private static string <Biomes>m__E(BiomeDef d)
		{
			return d.forageability.ToStringPercent();
		}

		[CompilerGenerated]
		private static string <Biomes>m__F(BiomeDef d)
		{
			return (d.foragedFood == null) ? "" : d.foragedFood.label;
		}

		[CompilerGenerated]
		private static string <Biomes>m__10(BiomeDef d)
		{
			return (from pd in d.AllWildPlants
			where pd.plant.harvestedThingDef != null && pd.plant.harvestedThingDef.IsNutritionGivingIngestible
			select pd.defName).ToCommaList(false);
		}

		[CompilerGenerated]
		private static string <Biomes>m__11(BiomeDef d)
		{
			return d.wildPlantRegrowDays.ToString("F0");
		}

		[CompilerGenerated]
		private static string <Biomes>m__12(BiomeDef d)
		{
			return d.wildPlantsCareAboutLocalFertility.ToStringCheckBlank();
		}

		[CompilerGenerated]
		private static string <BiomeAnimalsSpawnChances>m__13(PawnKindDef k, BiomeDef b)
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
		}

		[CompilerGenerated]
		private static string <BiomeAnimalsTypicalCounts>m__14(PawnKindDef k, BiomeDef b)
		{
			return DebugOutputsEcology.ExpectedAnimalCount(k, b).ToStringEmptyZero("F2");
		}

		[CompilerGenerated]
		private static bool <BiomeAnimalsInternal>m__15(BiomeDef b)
		{
			return b.implemented && b.canBuildBase;
		}

		[CompilerGenerated]
		private static float <BiomeAnimalsInternal>m__16(BiomeDef b)
		{
			return b.animalDensity;
		}

		[CompilerGenerated]
		private static string <BiomeAnimalsInternal>m__17(PawnKindDef k)
		{
			return k.defName + "" + ((!k.race.race.predator) ? "" : " (P)");
		}

		[CompilerGenerated]
		private static bool <BiomeAnimalsInternal>m__18(PawnKindDef d)
		{
			return d.race != null && d.RaceProps.Animal;
		}

		[CompilerGenerated]
		private static string <BiomeAnimalsInternal>m__19(PawnKindDef d)
		{
			return d.defName;
		}

		[CompilerGenerated]
		private static string <BiomePlantsExpectedCount>m__1A(ThingDef p, BiomeDef b)
		{
			return (b.CommonalityOfPlant(p) * b.plantDensity * 4000f).ToString("F0");
		}

		[CompilerGenerated]
		private static bool <BiomePlantsExpectedCount>m__1B(BiomeDef b)
		{
			return b.implemented && b.canBuildBase;
		}

		[CompilerGenerated]
		private static float <BiomePlantsExpectedCount>m__1C(BiomeDef b)
		{
			return b.plantDensity;
		}

		[CompilerGenerated]
		private static string <BiomePlantsExpectedCount>m__1D(ThingDef k)
		{
			return k.defName;
		}

		[CompilerGenerated]
		private static bool <BiomePlantsExpectedCount>m__1E(ThingDef d)
		{
			return d.category == ThingCategory.Plant;
		}

		[CompilerGenerated]
		private static string <BiomePlantsExpectedCount>m__1F(ThingDef d)
		{
			return d.defName;
		}

		[CompilerGenerated]
		private static string <AnimalWildCountsOnMap>m__20(PawnKindDef k)
		{
			return k.defName;
		}

		[CompilerGenerated]
		private static string <PlantCountsOnMap>m__21(ThingDef p)
		{
			return p.defName;
		}

		[CompilerGenerated]
		private static bool <Biomes>m__22(ThingDef pd)
		{
			return pd.plant.harvestedThingDef != null && pd.plant.harvestedThingDef.IsNutritionGivingIngestible;
		}

		[CompilerGenerated]
		private static string <Biomes>m__23(ThingDef pd)
		{
			return pd.defName;
		}

		[CompilerGenerated]
		private sealed class <ExpectedAnimalCount>c__AnonStorey1
		{
			internal BiomeDef b;

			internal PawnKindDef k;

			internal float totalCommonality;

			public <ExpectedAnimalCount>c__AnonStorey1()
			{
			}

			internal float <>m__0(PawnKindDef ki)
			{
				return this.b.CommonalityOfAnimal(ki);
			}

			internal float <>m__1(PawnKindDef ki)
			{
				return this.b.CommonalityOfAnimal(ki);
			}

			internal float <>m__2(PawnKindDef ki)
			{
				return this.k.ecoSystemWeight * (this.b.CommonalityOfAnimal(ki) / this.totalCommonality);
			}
		}

		[CompilerGenerated]
		private sealed class <BiomeAnimalsInternal>c__AnonStorey2
		{
			internal Func<PawnKindDef, BiomeDef, string> densityInBiomeOutputter;

			public <BiomeAnimalsInternal>c__AnonStorey2()
			{
			}

			internal TableDataGetter<PawnKindDef> <>m__0(BiomeDef b)
			{
				return new TableDataGetter<PawnKindDef>(b.defName, (PawnKindDef k) => this.densityInBiomeOutputter(k, b));
			}

			private sealed class <BiomeAnimalsInternal>c__AnonStorey3
			{
				internal BiomeDef b;

				internal DebugOutputsEcology.<BiomeAnimalsInternal>c__AnonStorey2 <>f__ref$2;

				public <BiomeAnimalsInternal>c__AnonStorey3()
				{
				}

				internal string <>m__0(PawnKindDef k)
				{
					return this.<>f__ref$2.densityInBiomeOutputter(k, this.b);
				}
			}
		}

		[CompilerGenerated]
		private sealed class <BiomePlantsExpectedCount>c__AnonStorey4
		{
			internal Func<ThingDef, BiomeDef, string> expectedCountInBiomeOutputter;

			public <BiomePlantsExpectedCount>c__AnonStorey4()
			{
			}

			internal TableDataGetter<ThingDef> <>m__0(BiomeDef b)
			{
				return new TableDataGetter<ThingDef>(b.defName + " (" + b.plantDensity.ToString("F2") + ")", (ThingDef k) => this.expectedCountInBiomeOutputter(k, b));
			}

			private sealed class <BiomePlantsExpectedCount>c__AnonStorey5
			{
				internal BiomeDef b;

				internal DebugOutputsEcology.<BiomePlantsExpectedCount>c__AnonStorey4 <>f__ref$4;

				public <BiomePlantsExpectedCount>c__AnonStorey5()
				{
				}

				internal string <>m__0(ThingDef k)
				{
					return this.<>f__ref$4.expectedCountInBiomeOutputter(k, this.b);
				}
			}
		}

		[CompilerGenerated]
		private sealed class <AnimalWildCountsOnMap>c__AnonStorey6
		{
			internal Map map;

			public <AnimalWildCountsOnMap>c__AnonStorey6()
			{
			}

			internal bool <>m__0(PawnKindDef k)
			{
				return k.race != null && k.RaceProps.Animal && DebugOutputsEcology.ExpectedAnimalCount(k, this.map.Biome) > 0f;
			}

			internal float <>m__1(PawnKindDef k)
			{
				return DebugOutputsEcology.ExpectedAnimalCount(k, this.map.Biome);
			}

			internal string <>m__2(PawnKindDef k)
			{
				return DebugOutputsEcology.ExpectedAnimalCount(k, this.map.Biome).ToString("F2");
			}

			internal string <>m__3(PawnKindDef k)
			{
				return (from p in this.map.mapPawns.AllPawnsSpawned
				where p.kindDef == k
				select p).Count<Pawn>().ToString();
			}

			private sealed class <AnimalWildCountsOnMap>c__AnonStorey7
			{
				internal PawnKindDef k;

				internal DebugOutputsEcology.<AnimalWildCountsOnMap>c__AnonStorey6 <>f__ref$6;

				public <AnimalWildCountsOnMap>c__AnonStorey7()
				{
				}

				internal bool <>m__0(Pawn p)
				{
					return p.kindDef == this.k;
				}
			}
		}

		[CompilerGenerated]
		private sealed class <PlantCountsOnMap>c__AnonStorey8
		{
			internal Map map;

			public <PlantCountsOnMap>c__AnonStorey8()
			{
			}

			internal bool <>m__0(ThingDef p)
			{
				return p.category == ThingCategory.Plant && this.map.Biome.CommonalityOfPlant(p) > 0f;
			}

			internal float <>m__1(ThingDef p)
			{
				return this.map.Biome.CommonalityOfPlant(p);
			}

			internal string <>m__2(ThingDef p)
			{
				return this.map.Biome.CommonalityOfPlant(p).ToString("F2");
			}

			internal string <>m__3(ThingDef p)
			{
				return (this.map.Biome.CommonalityOfPlant(p) * this.map.Biome.plantDensity * 4000f).ToString("F0");
			}

			internal string <>m__4(ThingDef p)
			{
				return (from c in this.map.AllCells
				where c.GetPlant(this.map) != null && c.GetPlant(this.map).def == p
				select c).Count<IntVec3>().ToString();
			}

			private sealed class <PlantCountsOnMap>c__AnonStorey9
			{
				internal ThingDef p;

				internal DebugOutputsEcology.<PlantCountsOnMap>c__AnonStorey8 <>f__ref$8;

				public <PlantCountsOnMap>c__AnonStorey9()
				{
				}

				internal bool <>m__0(IntVec3 c)
				{
					return c.GetPlant(this.<>f__ref$8.map) != null && c.GetPlant(this.<>f__ref$8.map).def == this.p;
				}
			}
		}

		[CompilerGenerated]
		private sealed class <BiomeAnimalsSpawnChances>c__AnonStorey0
		{
			internal BiomeDef b;

			public <BiomeAnimalsSpawnChances>c__AnonStorey0()
			{
			}

			internal float <>m__0(PawnKindDef ki)
			{
				return this.b.CommonalityOfAnimal(ki);
			}
		}
	}
}
