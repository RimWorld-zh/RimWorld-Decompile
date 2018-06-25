using System;
using System.Collections.Generic;
using Verse;
using Verse.Noise;

namespace RimWorld
{
	// Token: 0x02000405 RID: 1029
	public class TerrainPatchMaker
	{
		// Token: 0x04000ABA RID: 2746
		private Map currentlyInitializedForMap;

		// Token: 0x04000ABB RID: 2747
		public List<TerrainThreshold> thresholds = new List<TerrainThreshold>();

		// Token: 0x04000ABC RID: 2748
		public float perlinFrequency = 0.01f;

		// Token: 0x04000ABD RID: 2749
		public float perlinLacunarity = 2f;

		// Token: 0x04000ABE RID: 2750
		public float perlinPersistence = 0.5f;

		// Token: 0x04000ABF RID: 2751
		public int perlinOctaves = 6;

		// Token: 0x04000AC0 RID: 2752
		public float minFertility = -999f;

		// Token: 0x04000AC1 RID: 2753
		public float maxFertility = 999f;

		// Token: 0x04000AC2 RID: 2754
		public int minSize;

		// Token: 0x04000AC3 RID: 2755
		[Unsaved]
		private ModuleBase noise;

		// Token: 0x060011B0 RID: 4528 RVA: 0x00099E44 File Offset: 0x00098244
		private void Init(Map map)
		{
			this.noise = new Perlin((double)this.perlinFrequency, (double)this.perlinLacunarity, (double)this.perlinPersistence, this.perlinOctaves, Rand.Range(0, int.MaxValue), QualityMode.Medium);
			NoiseDebugUI.RenderSize = new IntVec2(map.Size.x, map.Size.z);
			NoiseDebugUI.StoreNoiseRender(this.noise, "TerrainPatchMaker " + this.thresholds[0].terrain.defName);
			this.currentlyInitializedForMap = map;
		}

		// Token: 0x060011B1 RID: 4529 RVA: 0x00099EDC File Offset: 0x000982DC
		public void Cleanup()
		{
			this.noise = null;
			this.currentlyInitializedForMap = null;
		}

		// Token: 0x060011B2 RID: 4530 RVA: 0x00099EF0 File Offset: 0x000982F0
		public TerrainDef TerrainAt(IntVec3 c, Map map, float fertility)
		{
			TerrainDef result;
			if (fertility < this.minFertility || fertility > this.maxFertility)
			{
				result = null;
			}
			else
			{
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
				result = TerrainThreshold.TerrainAtValue(this.thresholds, this.noise.GetValue(c));
			}
			return result;
		}
	}
}
