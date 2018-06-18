using System;
using System.Collections.Generic;
using Verse.Noise;

namespace Verse
{
	// Token: 0x02000C63 RID: 3171
	public static class RockNoises
	{
		// Token: 0x060045B3 RID: 17843 RVA: 0x0024C08C File Offset: 0x0024A48C
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

		// Token: 0x060045B4 RID: 17844 RVA: 0x0024C16C File Offset: 0x0024A56C
		public static void Reset()
		{
			RockNoises.rockNoises = null;
		}

		// Token: 0x04002FA5 RID: 12197
		public static List<RockNoises.RockNoise> rockNoises = null;

		// Token: 0x04002FA6 RID: 12198
		private const float RockNoiseFreq = 0.005f;

		// Token: 0x02000C64 RID: 3172
		public class RockNoise
		{
			// Token: 0x04002FA7 RID: 12199
			public ThingDef rockDef;

			// Token: 0x04002FA8 RID: 12200
			public ModuleBase noise;
		}
	}
}
