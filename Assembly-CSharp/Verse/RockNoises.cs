using System;
using System.Collections.Generic;
using Verse.Noise;

namespace Verse
{
	// Token: 0x02000C60 RID: 3168
	public static class RockNoises
	{
		// Token: 0x04002FAF RID: 12207
		public static List<RockNoises.RockNoise> rockNoises = null;

		// Token: 0x04002FB0 RID: 12208
		private const float RockNoiseFreq = 0.005f;

		// Token: 0x060045BC RID: 17852 RVA: 0x0024D45C File Offset: 0x0024B85C
		public static void Init(Map map)
		{
			RockNoises.rockNoises = new List<RockNoises.RockNoise>();
			foreach (ThingDef rockDef in Find.World.NaturalRockTypesIn(map.Tile))
			{
				RockNoises.RockNoise rockNoise = new RockNoises.RockNoise();
				rockNoise.rockDef = rockDef;
				rockNoise.noise = new Perlin(0.004999999888241291, 2.0, 0.5, 6, Rand.Range(0, int.MaxValue), QualityMode.Medium);
				RockNoises.rockNoises.Add(rockNoise);
				NoiseDebugUI.StoreNoiseRender(rockNoise.noise, rockNoise.rockDef + " score", map.Size.ToIntVec2);
			}
		}

		// Token: 0x060045BD RID: 17853 RVA: 0x0024D53C File Offset: 0x0024B93C
		public static void Reset()
		{
			RockNoises.rockNoises = null;
		}

		// Token: 0x02000C61 RID: 3169
		public class RockNoise
		{
			// Token: 0x04002FB1 RID: 12209
			public ThingDef rockDef;

			// Token: 0x04002FB2 RID: 12210
			public ModuleBase noise;
		}
	}
}
