using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using RimWorld.Planet;
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

		public float animalDensity = 0f;

		public float plantDensity = 0f;

		public float diseaseMtbDays = 60f;

		public float factionBaseSelectionWeight = 1f;

		public bool impassable;

		public bool hasVirtualPlants = true;

		public float forageability;

		public ThingDef foragedFood;

		public bool wildPlantsCareAboutLocalFertility = true;

		public float wildPlantRegrowDays = 25f;

		public float movementDifficulty = 1f;

		public List<WeatherCommonalityRecord> baseWeatherCommonalities = new List<WeatherCommonalityRecord>();

		public List<TerrainThreshold> terrainsByFertility = new List<TerrainThreshold>();

		public List<SoundDef> soundsAmbient = new List<SoundDef>();

		public List<TerrainPatchMaker> terrainPatchMakers = new List<TerrainPatchMaker>();

		private List<BiomePlantRecord> wildPlants = new List<BiomePlantRecord>();

		private List<BiomeAnimalRecord> wildAnimals = new List<BiomeAnimalRecord>();

		private List<BiomeDiseaseRecord> diseases = new List<BiomeDiseaseRecord>();

		private List<ThingDef> allowedPackAnimals = new List<ThingDef>();

		public bool hasBedrock = true;

		[NoTranslate]
		public string texture;

		[Unsaved]
		private Dictionary<PawnKindDef, float> cachedAnimalCommonalities = null;

		[Unsaved]
		private Dictionary<ThingDef, float> cachedPlantCommonalities = null;

		[Unsaved]
		private Dictionary<IncidentDef, float> cachedDiseaseCommonalities = null;

		[Unsaved]
		private Material cachedMat;

		[Unsaved]
		private List<ThingDef> cachedWildPlants;

		[Unsaved]
		private int? cachedMaxWildPlantsClusterRadius;

		[Unsaved]
		private float cachedPlantCommonalitiesSum;

		[Unsaved]
		private float? cachedLowestWildPlantOrder;

		[Unsaved]
		private BiomeWorker workerInt;

		[CompilerGenerated]
		private static Func<KeyValuePair<ThingDef, float>, float> <>f__am$cache0;

		public BiomeDef()
		{
		}

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

		public float PlantCommonalitiesSum
		{
			get
			{
				this.CachePlantCommonalitiesIfShould();
				return this.cachedPlantCommonalitiesSum;
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

		public float CommonalityPctOfPlant(ThingDef plantDef)
		{
			return this.CommonalityOfPlant(plantDef) / this.PlantCommonalitiesSum;
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

		public bool IsPackAnimalAllowed(ThingDef pawn)
		{
			return this.allowedPackAnimals.Contains(pawn);
		}

		public static BiomeDef Named(string defName)
		{
			return DefDatabase<BiomeDef>.GetNamed(defName, true);
		}

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

		[CompilerGenerated]
		private static float <CachePlantCommonalitiesIfShould>m__0(KeyValuePair<ThingDef, float> x)
		{
			return x.Value;
		}

		[DebuggerHidden]
		[CompilerGenerated]
		private IEnumerable<string> <ConfigErrors>__BaseCallProxy0()
		{
			return base.ConfigErrors();
		}

		[CompilerGenerated]
		private sealed class <>c__Iterator0 : IEnumerable, IEnumerable<PawnKindDef>, IEnumerator, IDisposable, IEnumerator<PawnKindDef>
		{
			internal IEnumerator<PawnKindDef> $locvar0;

			internal PawnKindDef <kindDef>__1;

			internal BiomeDef $this;

			internal PawnKindDef $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <>c__Iterator0()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				bool flag = false;
				switch (num)
				{
				case 0u:
					enumerator = DefDatabase<PawnKindDef>.AllDefs.GetEnumerator();
					num = 4294967293u;
					break;
				case 1u:
					break;
				default:
					return false;
				}
				try
				{
					switch (num)
					{
					case 1u:
						IL_98:
						break;
					}
					if (enumerator.MoveNext())
					{
						kindDef = enumerator.Current;
						if (base.CommonalityOfAnimal(kindDef) > 0f)
						{
							this.$current = kindDef;
							if (!this.$disposing)
							{
								this.$PC = 1;
							}
							flag = true;
							return true;
						}
						goto IL_98;
					}
				}
				finally
				{
					if (!flag)
					{
						if (enumerator != null)
						{
							enumerator.Dispose();
						}
					}
				}
				this.$PC = -1;
				return false;
			}

			PawnKindDef IEnumerator<PawnKindDef>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			[DebuggerHidden]
			public void Dispose()
			{
				uint num = (uint)this.$PC;
				this.$disposing = true;
				this.$PC = -1;
				switch (num)
				{
				case 1u:
					try
					{
					}
					finally
					{
						if (enumerator != null)
						{
							enumerator.Dispose();
						}
					}
					break;
				}
			}

			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.System.Collections.Generic.IEnumerable<Verse.PawnKindDef>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<PawnKindDef> IEnumerable<PawnKindDef>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				BiomeDef.<>c__Iterator0 <>c__Iterator = new BiomeDef.<>c__Iterator0();
				<>c__Iterator.$this = this;
				return <>c__Iterator;
			}
		}

		[CompilerGenerated]
		private sealed class <ConfigErrors>c__Iterator1 : IEnumerable, IEnumerable<string>, IEnumerator, IDisposable, IEnumerator<string>
		{
			internal IEnumerator<string> $locvar0;

			internal string <e>__1;

			internal List<BiomeAnimalRecord>.Enumerator $locvar1;

			internal BiomeDef $this;

			internal string $current;

			internal bool $disposing;

			internal int $PC;

			private BiomeDef.<ConfigErrors>c__Iterator1.<ConfigErrors>c__AnonStorey2 $locvar2;

			[DebuggerHidden]
			public <ConfigErrors>c__Iterator1()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				bool flag = false;
				switch (num)
				{
				case 0u:
					enumerator = base.<ConfigErrors>__BaseCallProxy0().GetEnumerator();
					num = 4294967293u;
					break;
				case 1u:
					break;
				case 2u:
					goto IL_DD;
				default:
					return false;
				}
				try
				{
					switch (num)
					{
					}
					if (enumerator.MoveNext())
					{
						e = enumerator.Current;
						this.$current = e;
						if (!this.$disposing)
						{
							this.$PC = 1;
						}
						flag = true;
						return true;
					}
				}
				finally
				{
					if (!flag)
					{
						if (enumerator != null)
						{
							enumerator.Dispose();
						}
					}
				}
				if (!Prefs.DevMode)
				{
					goto IL_1AB;
				}
				enumerator2 = this.wildAnimals.GetEnumerator();
				num = 4294967293u;
				try
				{
					IL_DD:
					switch (num)
					{
					case 2u:
						IL_17E:
						break;
					}
					if (enumerator2.MoveNext())
					{
						BiomeAnimalRecord wa = enumerator2.Current;
						if (this.wildAnimals.Count((BiomeAnimalRecord a) => a.animal == wa.animal) > 1)
						{
							this.$current = "Duplicate animal record: " + wa.animal.defName;
							if (!this.$disposing)
							{
								this.$PC = 2;
							}
							flag = true;
							return true;
						}
						goto IL_17E;
					}
				}
				finally
				{
					if (!flag)
					{
						((IDisposable)enumerator2).Dispose();
					}
				}
				IL_1AB:
				this.$PC = -1;
				return false;
			}

			string IEnumerator<string>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			[DebuggerHidden]
			public void Dispose()
			{
				uint num = (uint)this.$PC;
				this.$disposing = true;
				this.$PC = -1;
				switch (num)
				{
				case 1u:
					try
					{
					}
					finally
					{
						if (enumerator != null)
						{
							enumerator.Dispose();
						}
					}
					break;
				case 2u:
					try
					{
					}
					finally
					{
						((IDisposable)enumerator2).Dispose();
					}
					break;
				}
			}

			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.System.Collections.Generic.IEnumerable<string>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<string> IEnumerable<string>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				BiomeDef.<ConfigErrors>c__Iterator1 <ConfigErrors>c__Iterator = new BiomeDef.<ConfigErrors>c__Iterator1();
				<ConfigErrors>c__Iterator.$this = this;
				return <ConfigErrors>c__Iterator;
			}

			private sealed class <ConfigErrors>c__AnonStorey2
			{
				internal BiomeAnimalRecord wa;

				internal BiomeDef.<ConfigErrors>c__Iterator1 <>f__ref$1;

				public <ConfigErrors>c__AnonStorey2()
				{
				}

				internal bool <>m__0(BiomeAnimalRecord a)
				{
					return a.animal == this.wa.animal;
				}
			}
		}
	}
}
