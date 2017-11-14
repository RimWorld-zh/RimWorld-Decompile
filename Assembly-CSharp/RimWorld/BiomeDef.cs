using RimWorld.Planet;
using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public class BiomeDef : Def
	{
		public Type workerClass = typeof(BiomeWorker);

		public bool implemented = true;

		public bool canBuildBase = true;

		public bool canAutoChoose = true;

		public bool allowRoads = true;

		public bool allowRivers = true;

		public float animalDensity;

		public float plantDensity;

		public float diseaseMtbDays = 60f;

		public float factionBaseSelectionWeight = 1f;

		public bool impassable;

		public bool hasVirtualPlants = true;

		public int pathCost_spring = 1650;

		public int pathCost_summer = 1650;

		public int pathCost_fall = 1650;

		public int pathCost_winter = 27500;

		public List<WeatherCommonalityRecord> baseWeatherCommonalities = new List<WeatherCommonalityRecord>();

		public List<TerrainThreshold> terrainsByFertility = new List<TerrainThreshold>();

		public List<SoundDef> soundsAmbient = new List<SoundDef>();

		public List<TerrainPatchMaker> terrainPatchMakers = new List<TerrainPatchMaker>();

		private List<BiomePlantRecord> wildPlants = new List<BiomePlantRecord>();

		private List<BiomeAnimalRecord> wildAnimals = new List<BiomeAnimalRecord>();

		private List<BiomeDiseaseRecord> diseases = new List<BiomeDiseaseRecord>();

		private List<ThingDef> allowedPackAnimals = new List<ThingDef>();

		public string texture;

		[Unsaved]
		private Dictionary<PawnKindDef, float> cachedAnimalCommonalities;

		[Unsaved]
		private Dictionary<ThingDef, float> cachedPlantCommonalities;

		[Unsaved]
		private Dictionary<IncidentDef, float> cachedDiseaseCommonalities;

		[Unsaved]
		private Material cachedMat;

		[Unsaved]
		private BiomeWorker workerInt;

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

		public Material DrawMaterial
		{
			get
			{
				if ((UnityEngine.Object)this.cachedMat == (UnityEngine.Object)null)
				{
					if (this.texture.NullOrEmpty())
					{
						return null;
					}
					if (this == BiomeDefOf.Ocean || this == BiomeDefOf.Lake)
					{
						this.cachedMat = new Material(WorldMaterials.WorldOcean);
					}
					else if (!this.allowRoads && !this.allowRivers)
					{
						this.cachedMat = new Material(WorldMaterials.WorldIce);
					}
					else
					{
						this.cachedMat = new Material(WorldMaterials.WorldTerrain);
					}
					this.cachedMat.mainTexture = ContentFinder<Texture2D>.Get(this.texture, true);
				}
				return this.cachedMat;
			}
		}

		public IEnumerable<ThingDef> AllWildPlants
		{
			get
			{
				foreach (ThingDef allDef in DefDatabase<ThingDef>.AllDefs)
				{
					if (allDef.category == ThingCategory.Plant && this.CommonalityOfPlant(allDef) > 0.0)
					{
						yield return allDef;
						/*Error: Unable to find new state assignment for yield return*/;
					}
				}
				yield break;
				IL_00df:
				/*Error near IL_00e0: Unexpected return in MoveNext()*/;
			}
		}

		public IEnumerable<PawnKindDef> AllWildAnimals
		{
			get
			{
				foreach (PawnKindDef allDef in DefDatabase<PawnKindDef>.AllDefs)
				{
					if (this.CommonalityOfAnimal(allDef) > 0.0)
					{
						yield return allDef;
						/*Error: Unable to find new state assignment for yield return*/;
					}
				}
				yield break;
				IL_00ce:
				/*Error near IL_00cf: Unexpected return in MoveNext()*/;
			}
		}

		public float CommonalityOfAnimal(PawnKindDef animalDef)
		{
			if (this.cachedAnimalCommonalities == null)
			{
				this.cachedAnimalCommonalities = new Dictionary<PawnKindDef, float>();
				for (int i = 0; i < this.wildAnimals.Count; i++)
				{
					this.cachedAnimalCommonalities.Add(this.wildAnimals[i].animal, this.wildAnimals[i].commonality);
				}
				foreach (PawnKindDef allDef in DefDatabase<PawnKindDef>.AllDefs)
				{
					if (allDef.RaceProps.wildBiomes != null)
					{
						for (int j = 0; j < allDef.RaceProps.wildBiomes.Count; j++)
						{
							if (allDef.RaceProps.wildBiomes[j].biome == this)
							{
								this.cachedAnimalCommonalities.Add(allDef, allDef.RaceProps.wildBiomes[j].commonality);
							}
						}
					}
				}
			}
			float result = default(float);
			if (this.cachedAnimalCommonalities.TryGetValue(animalDef, out result))
			{
				return result;
			}
			return 0f;
		}

		public float CommonalityOfPlant(ThingDef plantDef)
		{
			if (this.cachedPlantCommonalities == null)
			{
				this.cachedPlantCommonalities = new Dictionary<ThingDef, float>();
				for (int i = 0; i < this.wildPlants.Count; i++)
				{
					this.cachedPlantCommonalities.Add(this.wildPlants[i].plant, this.wildPlants[i].commonality);
				}
				foreach (ThingDef allDef in DefDatabase<ThingDef>.AllDefs)
				{
					if (allDef.plant != null && allDef.plant.wildBiomes != null)
					{
						for (int j = 0; j < allDef.plant.wildBiomes.Count; j++)
						{
							if (allDef.plant.wildBiomes[j].biome == this)
							{
								this.cachedPlantCommonalities.Add(allDef, allDef.plant.wildBiomes[j].commonality);
							}
						}
					}
				}
			}
			float result = default(float);
			if (this.cachedPlantCommonalities.TryGetValue(plantDef, out result))
			{
				return result;
			}
			return 0f;
		}

		public float CommonalityOfDisease(IncidentDef diseaseInc)
		{
			if (this.cachedDiseaseCommonalities == null)
			{
				this.cachedDiseaseCommonalities = new Dictionary<IncidentDef, float>();
				for (int i = 0; i < this.diseases.Count; i++)
				{
					this.cachedDiseaseCommonalities.Add(this.diseases[i].diseaseInc, this.diseases[i].commonality);
				}
				foreach (IncidentDef allDef in DefDatabase<IncidentDef>.AllDefs)
				{
					if (allDef.diseaseBiomeRecords != null)
					{
						for (int j = 0; j < allDef.diseaseBiomeRecords.Count; j++)
						{
							if (allDef.diseaseBiomeRecords[j].biome == this)
							{
								this.cachedDiseaseCommonalities.Add(allDef.diseaseBiomeRecords[j].diseaseInc, allDef.diseaseBiomeRecords[j].commonality);
							}
						}
					}
				}
			}
			float result = default(float);
			if (this.cachedDiseaseCommonalities.TryGetValue(diseaseInc, out result))
			{
				return result;
			}
			return 0f;
		}

		public bool IsPackAnimalAllowed(ThingDef pawn)
		{
			return this.allowedPackAnimals.Contains(pawn);
		}

		public static BiomeDef Named(string defName)
		{
			return DefDatabase<BiomeDef>.GetNamed(defName, true);
		}
	}
}
