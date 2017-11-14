using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public class WildSpawner
	{
		private Map map;

		private static List<ThingDef> cavePlants;

		private const int AnimalCheckInterval = 1210;

		private const float BaseAnimalSpawnChancePerInterval = 0.0268888883f;

		private const int PlantTrySpawnIntervalAt100EdgeLength = 650;

		private const int CavePlantSpawnIntervalPer10kCells = 3600000;

		private static List<IntVec3> undergroundCells = new List<IntVec3>();

		private float DesiredAnimalDensity
		{
			get
			{
				float animalDensity = this.map.Biome.animalDensity;
				float num = 0f;
				float num2 = 0f;
				foreach (PawnKindDef allWildAnimal in this.map.Biome.AllWildAnimals)
				{
					num2 += allWildAnimal.wildSpawn_EcoSystemWeight;
					if (this.map.mapTemperature.SeasonAcceptableFor(allWildAnimal.race))
					{
						num += allWildAnimal.wildSpawn_EcoSystemWeight;
					}
				}
				animalDensity *= num / num2;
				return animalDensity * this.map.gameConditionManager.AggregateAnimalDensityFactor();
			}
		}

		private float DesiredTotalAnimalWeight
		{
			get
			{
				float desiredAnimalDensity = this.DesiredAnimalDensity;
				if (desiredAnimalDensity == 0.0)
				{
					return 0f;
				}
				float num = (float)(10000.0 / desiredAnimalDensity);
				return (float)this.map.Area / num;
			}
		}

		private float CurrentTotalAnimalWeight
		{
			get
			{
				float num = 0f;
				List<Pawn> allPawnsSpawned = this.map.mapPawns.AllPawnsSpawned;
				for (int i = 0; i < allPawnsSpawned.Count; i++)
				{
					if (allPawnsSpawned[i].kindDef.wildSpawn_spawnWild && allPawnsSpawned[i].Faction == null)
					{
						num += allPawnsSpawned[i].kindDef.wildSpawn_EcoSystemWeight;
					}
				}
				return num;
			}
		}

		public bool AnimalEcosystemFull
		{
			get
			{
				return this.CurrentTotalAnimalWeight >= this.DesiredTotalAnimalWeight;
			}
		}

		public WildSpawner(Map map)
		{
			this.map = map;
		}

		public static void Reset()
		{
			WildSpawner.cavePlants = (from x in DefDatabase<ThingDef>.AllDefsListForReading
			where x.category == ThingCategory.Plant && x.plant.cavePlant
			select x).ToList();
		}

		public void WildSpawnerTick()
		{
			IntVec3 loc = default(IntVec3);
			if (Find.TickManager.TicksGame % 1210 == 0 && !this.AnimalEcosystemFull && Rand.Value < 0.026888888329267502 * this.DesiredAnimalDensity && RCellFinder.TryFindRandomPawnEntryCell(out loc, this.map, CellFinder.EdgeRoadChance_Animal, (Predicate<IntVec3>)null))
			{
				this.SpawnRandomWildAnimalAt(loc);
			}
			float num = this.map.gameConditionManager.AggregatePlantDensityFactor();
			if (num > 9.9999997473787516E-05)
			{
				IntVec3 size = this.map.Size;
				int num2 = size.x * 2;
				IntVec3 size2 = this.map.Size;
				int num3 = num2 + size2.z * 2;
				float num4 = (float)(650.0 / ((float)num3 / 100.0));
				int num5 = (int)(num4 / num);
				if (num5 <= 0 || Find.TickManager.TicksGame % num5 == 0)
				{
					this.TrySpawnPlantFromMapEdge();
				}
			}
			int num6 = (int)(3600000.0 / ((float)this.map.Area / 10000.0));
			if (num6 > 0 && Find.TickManager.TicksGame % num6 != 0)
				return;
			this.TrySpawnCavePlant();
		}

		private void TrySpawnPlantFromMapEdge()
		{
			ThingDef plantDef = default(ThingDef);
			IntVec3 source = default(IntVec3);
			if (this.map.Biome.AllWildPlants.TryRandomElementByWeight<ThingDef>((Func<ThingDef, float>)((ThingDef def) => this.map.Biome.CommonalityOfPlant(def)), out plantDef) && RCellFinder.TryFindRandomCellToPlantInFromOffMap(plantDef, this.map, out source))
			{
				GenPlantReproduction.TryReproduceFrom(source, plantDef, SeedTargFindMode.MapEdge, this.map);
			}
		}

		private void TrySpawnCavePlant()
		{
			WildSpawner.undergroundCells.Clear();
			CellRect.CellRectIterator iterator = CellRect.WholeMap(this.map).GetIterator();
			while (!iterator.Done())
			{
				IntVec3 current = iterator.Current;
				if (GenPlantReproduction.GoodRoofForCavePlantReproduction(current, this.map) && current.GetFirstItem(this.map) == null && current.GetFirstPawn(this.map) == null && current.GetFirstBuilding(this.map) == null)
				{
					bool flag = false;
					int num = 0;
					while (num < WildSpawner.cavePlants.Count)
					{
						if (!WildSpawner.cavePlants[num].CanEverPlantAt(current, this.map))
						{
							num++;
							continue;
						}
						flag = true;
						break;
					}
					if (flag)
					{
						WildSpawner.undergroundCells.Add(current);
					}
				}
				iterator.MoveNext();
			}
			if (WildSpawner.undergroundCells.Any())
			{
				IntVec3 cell = WildSpawner.undergroundCells.RandomElement();
				ThingDef plantDef = (from x in WildSpawner.cavePlants
				where x.CanEverPlantAt(cell, this.map)
				select x).RandomElement();
				GenPlantReproduction.TryReproduceFrom(cell, plantDef, SeedTargFindMode.Cave, this.map);
			}
		}

		public bool SpawnRandomWildAnimalAt(IntVec3 loc)
		{
			PawnKindDef pawnKindDef = (from a in this.map.Biome.AllWildAnimals
			where this.map.mapTemperature.SeasonAcceptableFor(a.race)
			select a).RandomElementByWeight((PawnKindDef def) => this.map.Biome.CommonalityOfAnimal(def) / def.wildSpawn_GroupSizeRange.Average);
			if (pawnKindDef == null)
			{
				Log.Error("No spawnable animals right now.");
				return false;
			}
			int randomInRange = pawnKindDef.wildSpawn_GroupSizeRange.RandomInRange;
			int radius = Mathf.CeilToInt(Mathf.Sqrt((float)pawnKindDef.wildSpawn_GroupSizeRange.max));
			for (int i = 0; i < randomInRange; i++)
			{
				IntVec3 loc2 = CellFinder.RandomClosewalkCellNear(loc, this.map, radius, null);
				Pawn newThing = PawnGenerator.GeneratePawn(pawnKindDef, null);
				GenSpawn.Spawn(newThing, loc2, this.map);
			}
			return true;
		}

		public string DebugString()
		{
			return "DesiredTotalAnimalWeight: " + this.DesiredTotalAnimalWeight + "\nCurrentTotalAnimalWeight: " + this.CurrentTotalAnimalWeight + "\nDesiredAnimalDensity: " + this.DesiredAnimalDensity;
		}
	}
}
