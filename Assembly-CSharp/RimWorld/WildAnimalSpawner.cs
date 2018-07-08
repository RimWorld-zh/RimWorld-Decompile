using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public class WildAnimalSpawner
	{
		private Map map;

		private const int AnimalCheckInterval = 1213;

		private const float BaseAnimalSpawnChancePerInterval = 0.0173285715f;

		public WildAnimalSpawner(Map map)
		{
			this.map = map;
		}

		private float DesiredAnimalDensity
		{
			get
			{
				float num = this.map.Biome.animalDensity;
				float num2 = 0f;
				float num3 = 0f;
				foreach (PawnKindDef pawnKindDef in this.map.Biome.AllWildAnimals)
				{
					float num4 = this.map.Biome.CommonalityOfAnimal(pawnKindDef);
					num3 += num4;
					if (this.map.mapTemperature.SeasonAcceptableFor(pawnKindDef.race))
					{
						num2 += num4;
					}
				}
				num *= num2 / num3;
				num *= this.map.gameConditionManager.AggregateAnimalDensityFactor(this.map);
				return num;
			}
		}

		private float DesiredTotalAnimalWeight
		{
			get
			{
				float desiredAnimalDensity = this.DesiredAnimalDensity;
				float result;
				if (desiredAnimalDensity == 0f)
				{
					result = 0f;
				}
				else
				{
					float num = 10000f / desiredAnimalDensity;
					result = (float)this.map.Area / num;
				}
				return result;
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
					if (allPawnsSpawned[i].Faction == null)
					{
						num += allPawnsSpawned[i].kindDef.ecoSystemWeight;
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

		public void WildAnimalSpawnerTick()
		{
			if (Find.TickManager.TicksGame % 1213 == 0)
			{
				if (!this.AnimalEcosystemFull)
				{
					if (Rand.Chance(0.0173285715f * this.DesiredAnimalDensity))
					{
						IntVec3 loc;
						if (RCellFinder.TryFindRandomPawnEntryCell(out loc, this.map, CellFinder.EdgeRoadChance_Animal, null))
						{
							this.SpawnRandomWildAnimalAt(loc);
						}
					}
				}
			}
		}

		public bool SpawnRandomWildAnimalAt(IntVec3 loc)
		{
			PawnKindDef pawnKindDef = (from a in this.map.Biome.AllWildAnimals
			where this.map.mapTemperature.SeasonAcceptableFor(a.race)
			select a).RandomElementByWeight((PawnKindDef def) => this.map.Biome.CommonalityOfAnimal(def) / def.wildGroupSize.Average);
			bool result;
			if (pawnKindDef == null)
			{
				Log.Error("No spawnable animals right now.", false);
				result = false;
			}
			else
			{
				int randomInRange = pawnKindDef.wildGroupSize.RandomInRange;
				int radius = Mathf.CeilToInt(Mathf.Sqrt((float)pawnKindDef.wildGroupSize.max));
				for (int i = 0; i < randomInRange; i++)
				{
					IntVec3 loc2 = CellFinder.RandomClosewalkCellNear(loc, this.map, radius, null);
					Pawn newThing = PawnGenerator.GeneratePawn(pawnKindDef, null);
					GenSpawn.Spawn(newThing, loc2, this.map, WipeMode.Vanish);
				}
				result = true;
			}
			return result;
		}

		public string DebugString()
		{
			return string.Concat(new object[]
			{
				"DesiredTotalAnimalWeight: ",
				this.DesiredTotalAnimalWeight,
				"\nCurrentTotalAnimalWeight: ",
				this.CurrentTotalAnimalWeight,
				"\nDesiredAnimalDensity: ",
				this.DesiredAnimalDensity
			});
		}

		[CompilerGenerated]
		private bool <SpawnRandomWildAnimalAt>m__0(PawnKindDef a)
		{
			return this.map.mapTemperature.SeasonAcceptableFor(a.race);
		}

		[CompilerGenerated]
		private float <SpawnRandomWildAnimalAt>m__1(PawnKindDef def)
		{
			return this.map.Biome.CommonalityOfAnimal(def) / def.wildGroupSize.Average;
		}
	}
}
