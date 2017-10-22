using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public class WildSpawner
	{
		private const int AnimalCheckInterval = 1210;

		private const float BaseAnimalSpawnChancePerInterval = 0.0268888883f;

		private const int PlantTrySpawnIntervalAt100EdgeLength = 650;

		private Map map;

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
				int num5 = GenMath.RoundRandom(num4 / num);
				if (Find.TickManager.TicksGame % num5 == 0)
				{
					this.TrySpawnPlantFromMapEdge();
				}
			}
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

		public void SpawnRandomWildAnimalAt(IntVec3 loc)
		{
			PawnKindDef pawnKindDef = (from a in this.map.Biome.AllWildAnimals
			where this.map.mapTemperature.SeasonAcceptableFor(a.race)
			select a).RandomElementByWeight((Func<PawnKindDef, float>)((PawnKindDef def) => this.map.Biome.CommonalityOfAnimal(def) / def.wildSpawn_GroupSizeRange.Average));
			if (pawnKindDef == null)
			{
				Log.Error("No spawnable animals right now.");
			}
			else
			{
				int randomInRange = pawnKindDef.wildSpawn_GroupSizeRange.RandomInRange;
				int radius = Mathf.CeilToInt(Mathf.Sqrt((float)pawnKindDef.wildSpawn_GroupSizeRange.max));
				for (int num = 0; num < randomInRange; num++)
				{
					IntVec3 loc2 = CellFinder.RandomClosewalkCellNear(loc, this.map, radius, null);
					Pawn newThing = PawnGenerator.GeneratePawn(pawnKindDef, null);
					GenSpawn.Spawn(newThing, loc2, this.map);
				}
			}
		}

		public string DebugString()
		{
			return "DesiredTotalAnimalWeight: " + this.DesiredTotalAnimalWeight + "\nCurrentTotalAnimalWeight: " + this.CurrentTotalAnimalWeight + "\nDesiredAnimalDensity: " + this.DesiredAnimalDensity;
		}
	}
}
