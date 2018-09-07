using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Verse;
using Verse.Noise;

namespace RimWorld
{
	public class TerrainPatchMaker
	{
		private Map currentlyInitializedForMap;

		public List<TerrainThreshold> thresholds = new List<TerrainThreshold>();

		public float perlinFrequency = 0.01f;

		public float perlinLacunarity = 2f;

		public float perlinPersistence = 0.5f;

		public int perlinOctaves = 6;

		public float minFertility = -999f;

		public float maxFertility = 999f;

		public int minSize;

		[Unsaved]
		private ModuleBase noise;

		public TerrainPatchMaker()
		{
		}

		private void Init(Map map)
		{
			this.noise = new Perlin((double)this.perlinFrequency, (double)this.perlinLacunarity, (double)this.perlinPersistence, this.perlinOctaves, Rand.Range(0, int.MaxValue), QualityMode.Medium);
			NoiseDebugUI.RenderSize = new IntVec2(map.Size.x, map.Size.z);
			NoiseDebugUI.StoreNoiseRender(this.noise, "TerrainPatchMaker " + this.thresholds[0].terrain.defName);
			this.currentlyInitializedForMap = map;
		}

		public void Cleanup()
		{
			this.noise = null;
			this.currentlyInitializedForMap = null;
		}

		public TerrainDef TerrainAt(IntVec3 c, Map map, float fertility)
		{
			if (fertility < this.minFertility || fertility > this.maxFertility)
			{
				return null;
			}
			if (this.noise != null && map != this.currentlyInitializedForMap)
			{
				this.Cleanup();
			}
			if (this.noise == null)
			{
				this.Init(map);
			}
			if (this.minSize > 0)
			{
				int count = 0;
				map.floodFiller.FloodFill(c, (IntVec3 x) => TerrainThreshold.TerrainAtValue(this.thresholds, this.noise.GetValue(x)) != null, delegate(IntVec3 x)
				{
					count++;
					return count >= this.minSize;
				}, int.MaxValue, false, null);
				if (count < this.minSize)
				{
					return null;
				}
			}
			return TerrainThreshold.TerrainAtValue(this.thresholds, this.noise.GetValue(c));
		}

		[CompilerGenerated]
		private sealed class <TerrainAt>c__AnonStorey0
		{
			internal int count;

			internal TerrainPatchMaker $this;

			public <TerrainAt>c__AnonStorey0()
			{
			}

			internal bool <>m__0(IntVec3 x)
			{
				return TerrainThreshold.TerrainAtValue(this.$this.thresholds, this.$this.noise.GetValue(x)) != null;
			}

			internal bool <>m__1(IntVec3 x)
			{
				this.count++;
				return this.count >= this.$this.minSize;
			}
		}
	}
}
