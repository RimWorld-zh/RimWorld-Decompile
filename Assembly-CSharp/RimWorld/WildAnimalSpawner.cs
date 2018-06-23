using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000451 RID: 1105
	public class WildAnimalSpawner
	{
		// Token: 0x04000BB1 RID: 2993
		private Map map;

		// Token: 0x04000BB2 RID: 2994
		private const int AnimalCheckInterval = 1210;

		// Token: 0x04000BB3 RID: 2995
		private const float BaseAnimalSpawnChancePerInterval = 0.0268888883f;

		// Token: 0x0600133B RID: 4923 RVA: 0x000A578E File Offset: 0x000A3B8E
		public WildAnimalSpawner(Map map)
		{
			this.map = map;
		}

		// Token: 0x1700029D RID: 669
		// (get) Token: 0x0600133C RID: 4924 RVA: 0x000A57A0 File Offset: 0x000A3BA0
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

		// Token: 0x1700029E RID: 670
		// (get) Token: 0x0600133D RID: 4925 RVA: 0x000A5884 File Offset: 0x000A3C84
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

		// Token: 0x1700029F RID: 671
		// (get) Token: 0x0600133E RID: 4926 RVA: 0x000A58CC File Offset: 0x000A3CCC
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

		// Token: 0x170002A0 RID: 672
		// (get) Token: 0x0600133F RID: 4927 RVA: 0x000A5938 File Offset: 0x000A3D38
		public bool AnimalEcosystemFull
		{
			get
			{
				return this.CurrentTotalAnimalWeight >= this.DesiredTotalAnimalWeight;
			}
		}

		// Token: 0x06001340 RID: 4928 RVA: 0x000A5960 File Offset: 0x000A3D60
		public void WildAnimalSpawnerTick()
		{
			if (Find.TickManager.TicksGame % 1210 == 0)
			{
				if (!this.AnimalEcosystemFull)
				{
					if (Rand.Value < 0.0268888883f * this.DesiredAnimalDensity)
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

		// Token: 0x06001341 RID: 4929 RVA: 0x000A59CC File Offset: 0x000A3DCC
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

		// Token: 0x06001342 RID: 4930 RVA: 0x000A5A94 File Offset: 0x000A3E94
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
	}
}
