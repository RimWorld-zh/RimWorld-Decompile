using System;
using System.Collections.Generic;
using Verse.Noise;

namespace Verse
{
	// Token: 0x02000C64 RID: 3172
	public static class RockNoises
	{
		// Token: 0x060045B5 RID: 17845 RVA: 0x0024C0B4 File Offset: 0x0024A4B4
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

		// Token: 0x060045B6 RID: 17846 RVA: 0x0024C194 File Offset: 0x0024A594
		public static void Reset()
		{
			RockNoises.rockNoises = null;
		}

		// Token: 0x04002FA7 RID: 12199
		public static List<RockNoises.RockNoise> rockNoises = null;

		// Token: 0x04002FA8 RID: 12200
		private const float RockNoiseFreq = 0.005f;

		// Token: 0x02000C65 RID: 3173
		public class RockNoise
		{
			// Token: 0x04002FA9 RID: 12201
			public ThingDef rockDef;

			// Token: 0x04002FAA RID: 12202
			public ModuleBase noise;
		}
	}
}
