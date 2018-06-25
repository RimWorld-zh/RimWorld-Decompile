using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200028F RID: 655
	public class BiomeDef : Def
	{
		// Token: 0x0400056D RID: 1389
		public Type workerClass = typeof(BiomeWorker);

		// Token: 0x0400056E RID: 1390
		public bool implemented = true;

		// Token: 0x0400056F RID: 1391
		public bool canBuildBase = true;

		// Token: 0x04000570 RID: 1392
		public bool canAutoChoose = true;

		// Token: 0x04000571 RID: 1393
		public bool allowRoads = true;

		// Token: 0x04000572 RID: 1394
		public bool allowRivers = true;

		// Token: 0x04000573 RID: 1395
		public float animalDensity = 0f;

		// Token: 0x04000574 RID: 1396
		public float plantDensity = 0f;

		// Token: 0x04000575 RID: 1397
		public float diseaseMtbDays = 60f;

		// Token: 0x04000576 RID: 1398
		public float factionBaseSelectionWeight = 1f;

		// Token: 0x04000577 RID: 1399
		public bool impassable;

		// Token: 0x04000578 RID: 1400
		public bool hasVirtualPlants = true;

		// Token: 0x04000579 RID: 1401
		public float forageability;

		// Token: 0x0400057A RID: 1402
		public ThingDef foragedFood;

		// Token: 0x0400057B RID: 1403
		public bool wildPlantsCareAboutLocalFertility = true;

		// Token: 0x0400057C RID: 1404
		public float wildPlantRegrowDays = 25f;

		// Token: 0x0400057D RID: 1405
		public float movementDifficulty = 1f;

		// Token: 0x0400057E RID: 1406
		public List<WeatherCommonalityRecord> baseWeatherCommonalities = new List<WeatherCommonalityRecord>();

		// Token: 0x0400057F RID: 1407
		public List<TerrainThreshold> terrainsByFertility = new List<TerrainThreshold>();

		// Token: 0x04000580 RID: 1408
		public List<SoundDef> soundsAmbient = new List<SoundDef>();

		// Token: 0x04000581 RID: 1409
		public List<TerrainPatchMaker> terrainPatchMakers = new List<TerrainPatchMaker>();

		// Token: 0x04000582 RID: 1410
		private List<BiomePlantRecord> wildPlants = new List<BiomePlantRecord>();

		// Token: 0x04000583 RID: 1411
		private List<BiomeAnimalRecord> wildAnimals = new List<BiomeAnimalRecord>();

		// Token: 0x04000584 RID: 1412
		private List<BiomeDiseaseRecord> diseases = new List<BiomeDiseaseRecord>();

		// Token: 0x04000585 RID: 1413
		private List<ThingDef> allowedPackAnimals = new List<ThingDef>();

		// Token: 0x04000586 RID: 1414
		public bool hasBedrock = true;

		// Token: 0x04000587 RID: 1415
		[NoTranslate]
		public string texture;

		// Token: 0x04000588 RID: 1416
		[Unsaved]
		private Dictionary<PawnKindDef, float> cachedAnimalCommonalities = null;

		// Token: 0x04000589 RID: 1417
		[Unsaved]
		private Dictionary<ThingDef, float> cachedPlantCommonalities = null;

		// Token: 0x0400058A RID: 1418
		[Unsaved]
		private Dictionary<IncidentDef, float> cachedDiseaseCommonalities = null;

		// Token: 0x0400058B RID: 1419
		[Unsaved]
		private Material cachedMat;

		// Token: 0x0400058C RID: 1420
		[Unsaved]
		private List<ThingDef> cachedWildPlants;

		// Token: 0x0400058D RID: 1421
		[Unsaved]
		private int? cachedMaxWildPlantsClusterRadius;

		// Token: 0x0400058E RID: 1422
		[Unsaved]
		private float cachedPlantCommonalitiesSum;

		// Token: 0x0400058F RID: 1423
		[Unsaved]
		private float? cachedLowestWildPlantOrder;

		// Token: 0x04000590 RID: 1424
		[Unsaved]
		private BiomeWorker workerInt;

		// Token: 0x17000197 RID: 407
		// (get) Token: 0x06000B06 RID: 2822 RVA: 0x00063EC0 File Offset: 0x000622C0
		public BiomeWorker Worker
		{
			get
			{
				if (this.workerInt == null)
				{
					this.workerInt = (BiomeWorker)Activator.CreateInstance(this.workerClass);
				}
				return this.workerInt;
			}
		}

		// Token: 0x17000198 RID: 408
		// (get) Token: 0x06000B07 RID: 2823 RVA: 0x00063EFC File Offset: 0x000622FC
		public Material DrawMaterial
		{
			get
			{
				if (this.cachedMat == null)
				{
					if (this.texture.NullOrEmpty())
					{
						return null;
					}
					if (this == BiomeDefOf.Ocean || this == BiomeDefOf.Lake)
					{
						this.cachedMat = MaterialAllocator.Create(WorldMaterials.WorldOcean);
					}
					else if (!this.allowRoads && !this.allowRivers)
					{
						this.cachedMat = MaterialAllocator.Create(WorldMaterials.WorldIce);
					}
					else
					{
						this.cachedMat = MaterialAllocator.Create(WorldMaterials.WorldTerrain);
					}
					this.cachedMat.mainTexture = ContentFinder<Texture2D>.Get(this.texture, true);
				}
				return this.cachedMat;
			}
		}

		// Token: 0x17000199 RID: 409
		// (get) Token: 0x06000B08 RID: 2824 RVA: 0x00063FC0 File Offset: 0x000623C0
		public List<ThingDef> AllWildPlants
		{
			get
			{
				if (this.cachedWildPlants == null)
				{
					this.cachedWildPlants = new List<ThingDef>();
					foreach (ThingDef thingDef in DefDatabase<ThingDef>.AllDefsListForReading)
					{
						if (thingDef.category == ThingCategory.Plant && this.CommonalityOfPlant(thingDef) > 0f)
						{
							this.cachedWildPlants.Add(thingDef);
						}
					}
				}
				return this.cachedWildPlants;
			}
		}

		// Token: 0x1700019A RID: 410
		// (get) Token: 0x06000B09 RID: 2825 RVA: 0x00064068 File Offset: 0x00062468
		public int MaxWildAndCavePlantsClusterRadius
		{
			get
			{
				int? num = this.cachedMaxWildPlantsClusterRadius;
				if (num == null)
				{
					this.cachedMaxWildPlantsClusterRadius = new int?(0);
					List<ThingDef> allWildPlants = this.AllWildPlants;
					for (int i = 0; i < allWildPlants.Count; i++)
					{
						if (allWildPlants[i].plant.GrowsInClusters)
						{
							this.cachedMaxWildPlantsClusterRadius = new int?(Mathf.Max(this.cachedMaxWildPlantsClusterRadius.Value, allWildPlants[i].plant.wildClusterRadius));
						}
					}
					List<ThingDef> allDefsListForReading = DefDatabase<ThingDef>.AllDefsListForReading;
					for (int j = 0; j < allDefsListForReading.Count; j++)
					{
						if (allDefsListForReading[j].category == ThingCategory.Plant && allDefsListForReading[j].plant.cavePlant)
						{
							this.cachedMaxWildPlantsClusterRadius = new int?(Mathf.Max(this.cachedMaxWildPlantsClusterRadius.Value, allDefsListForReading[j].plant.wildClusterRadius));
						}
					}
				}
				return this.cachedMaxWildPlantsClusterRadius.Value;
			}
		}

		// Token: 0x1700019B RID: 411
		// (get) Token: 0x06000B0A RID: 2826 RVA: 0x0006418C File Offset: 0x0006258C
		public float LowestWildAndCavePlantOrder
		{
			get
			{
				float? num = this.cachedLowestWildPlantOrder;
				if (num == null)
				{
					this.cachedLowestWildPlantOrder = new float?(2.14748365E+09f);
					List<ThingDef> allWildPlants = this.AllWildPlants;
					for (int i = 0; i < allWildPlants.Count; i++)
					{
						this.cachedLowestWildPlantOrder = new float?(Mathf.Min(this.cachedLowestWildPlantOrder.Value, allWildPlants[i].plant.wildOrder));
					}
					List<ThingDef> allDefsListForReading = DefDatabase<ThingDef>.AllDefsListForReading;
					for (int j = 0; j < allDefsListForReading.Count; j++)
					{
						if (allDefsListForReading[j].category == ThingCategory.Plant && allDefsListForReading[j].plant.cavePlant)
						{
							this.cachedLowestWildPlantOrder = new float?(Mathf.Min(this.cachedLowestWildPlantOrder.Value, allDefsListForReading[j].plant.wildOrder));
						}
					}
				}
				return this.cachedLowestWildPlantOrder.Value;
			}
		}

		// Token: 0x1700019C RID: 412
		// (get) Token: 0x06000B0B RID: 2827 RVA: 0x0006429C File Offset: 0x0006269C
		public IEnumerable<PawnKindDef> AllWildAnimals
		{
			get
			{
				foreach (PawnKindDef kindDef in DefDatabase<PawnKindDef>.AllDefs)
				{
					if (this.CommonalityOfAnimal(kindDef) > 0f)
					{
						yield return kindDef;
					}
				}
				yield break;
			}
		}

		// Token: 0x1700019D RID: 413
		// (get) Token: 0x06000B0C RID: 2828 RVA: 0x000642C8 File Offset: 0x000626C8
		public float PlantCommonalitiesSum
		{
			get
			{
				this.CachePlantCommonalitiesIfShould();
				return this.cachedPlantCommonalitiesSum;
			}
		}

		// Token: 0x06000B0D RID: 2829 RVA: 0x000642EC File Offset: 0x000626EC
		public float CommonalityOfAnimal(PawnKindDef animalDef)
		{
			if (this.cachedAnimalCommonalities == null)
			{
				this.cachedAnimalCommonalities = new Dictionary<PawnKindDef, float>();
				for (int i = 0; i < this.wildAnimals.Count; i++)
				{
					this.cachedAnimalCommonalities.Add(this.wildAnimals[i].animal, this.wildAnimals[i].commonality);
				}
				foreach (PawnKindDef pawnKindDef in DefDatabase<PawnKindDef>.AllDefs)
				{
					if (pawnKindDef.RaceProps.wildBiomes != null)
					{
						for (int j = 0; j < pawnKindDef.RaceProps.wildBiomes.Count; j++)
						{
							if (pawnKindDef.RaceProps.wildBiomes[j].biome == this)
							{
								this.cachedAnimalCommonalities.Add(pawnKindDef, pawnKindDef.RaceProps.wildBiomes[j].commonality);
							}
						}
					}
				}
			}
			float num;
			float result;
			if (this.cachedAnimalCommonalities.TryGetValue(animalDef, out num))
			{
				result = num;
			}
			else
			{
				result = 0f;
			}
			return result;
		}

		// Token: 0x06000B0E RID: 2830 RVA: 0x00064444 File Offset: 0x00062844
		public float CommonalityOfPlant(ThingDef plantDef)
		{
			this.CachePlantCommonalitiesIfShould();
			float num;
			float result;
			if (this.cachedPlantCommonalities.TryGetValue(plantDef, out num))
			{
				result = num;
			}
			else
			{
				result = 0f;
			}
			return result;
		}

		// Token: 0x06000B0F RID: 2831 RVA: 0x00064480 File Offset: 0x00062880
		public float CommonalityPctOfPlant(ThingDef plantDef)
		{
			return this.CommonalityOfPlant(plantDef) / this.PlantCommonalitiesSum;
		}

		// Token: 0x06000B10 RID: 2832 RVA: 0x000644A4 File Offset: 0x000628A4
		public float CommonalityOfDisease(IncidentDef diseaseInc)
		{
			if (this.cachedDiseaseCommonalities == null)
			{
				this.cachedDiseaseCommonalities = new Dictionary<IncidentDef, float>();
				for (int i = 0; i < this.diseases.Count; i++)
				{
					this.cachedDiseaseCommonalities.Add(this.diseases[i].diseaseInc, this.diseases[i].commonality);
				}
				foreach (IncidentDef incidentDef in DefDatabase<IncidentDef>.AllDefs)
				{
					if (incidentDef.diseaseBiomeRecords != null)
					{
						for (int j = 0; j < incidentDef.diseaseBiomeRecords.Count; j++)
						{
							if (incidentDef.diseaseBiomeRecords[j].biome == this)
							{
								this.cachedDiseaseCommonalities.Add(incidentDef.diseaseBiomeRecords[j].diseaseInc, incidentDef.diseaseBiomeRecords[j].commonality);
							}
						}
					}
				}
			}
			float num;
			float result;
			if (this.cachedDiseaseCommonalities.TryGetValue(diseaseInc, out num))
			{
				result = num;
			}
			else
			{
				result = 0f;
			}
			return result;
		}

		// Token: 0x06000B11 RID: 2833 RVA: 0x000645F8 File Offset: 0x000629F8
		public bool IsPackAnimalAllowed(ThingDef pawn)
		{
			return this.allowedPackAnimals.Contains(pawn);
		}

		// Token: 0x06000B12 RID: 2834 RVA: 0x0006461C File Offset: 0x00062A1C
		public static BiomeDef Named(string defName)
		{
			return DefDatabase<BiomeDef>.GetNamed(defName, true);
		}

		// Token: 0x06000B13 RID: 2835 RVA: 0x00064638 File Offset: 0x00062A38
		private void CachePlantCommonalitiesIfShould()
		{
			if (this.cachedPlantCommonalities == null)
			{
				this.cachedPlantCommonalities = new Dictionary<ThingDef, float>();
				for (int i = 0; i < this.wildPlants.Count; i++)
				{
					this.cachedPlantCommonalities.Add(this.wildPlants[i].plant, this.wildPlants[i].commonality);
				}
				foreach (ThingDef thingDef in DefDatabase<ThingDef>.AllDefs)
				{
					if (thingDef.plant != null && thingDef.plant.wildBiomes != null)
					{
						for (int j = 0; j < thingDef.plant.wildBiomes.Count; j++)
						{
							if (thingDef.plant.wildBiomes[j].biome == this)
							{
								this.cachedPlantCommonalities.Add(thingDef, thingDef.plant.wildBiomes[j].commonality);
							}
						}
					}
				}
				this.cachedPlantCommonalitiesSum = this.cachedPlantCommonalities.Sum((KeyValuePair<ThingDef, float> x) => x.Value);
			}
		}

		// Token: 0x06000B14 RID: 2836 RVA: 0x0006479C File Offset: 0x00062B9C
		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string e in this.<ConfigErrors>__BaseCallProxy0())
			{
				yield return e;
			}
			if (Prefs.DevMode)
			{
				using (List<BiomeAnimalRecord>.Enumerator enumerator2 = this.wildAnimals.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						BiomeAnimalRecord wa = enumerator2.Current;
						if (this.wildAnimals.Count((BiomeAnimalRecord a) => a.animal == wa.animal) > 1)
						{
							yield return "Duplicate animal record: " + wa.animal.defName;
						}
					}
				}
			}
			yield break;
		}
	}
}
