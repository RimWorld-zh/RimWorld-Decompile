using System;
using System.Collections.Generic;
using Verse.Noise;

namespace Verse
{
	// Token: 0x02000C62 RID: 3170
	public static class RockNoises
	{
		// Token: 0x04002FAF RID: 12207
		public static List<RockNoises.RockNoise> rockNoises = null;

		// Token: 0x04002FB0 RID: 12208
		private const float RockNoiseFreq = 0.005f;

		// Token: 0x060045BF RID: 17855 RVA: 0x0024D538 File Offset: 0x0024B938
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

		// Token: 0x060045C0 RID: 17856 RVA: 0x0024D618 File Offset: 0x0024BA18
		public static void Reset()
		{
			RockNoises.rockNoises = null;
		}

		// Token: 0x02000C63 RID: 3171
		public class RockNoise
		{
			// Token: 0x04002FB1 RID: 12209
			public ThingDef rockDef;

			// Token: 0x04002FB2 RID: 12210
			public ModuleBase noise;
		}
	}
}
