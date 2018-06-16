using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020005C2 RID: 1474
	public class WorldGenStep_Lakes : WorldGenStep
	{
		// Token: 0x17000420 RID: 1056
		// (get) Token: 0x06001C45 RID: 7237 RVA: 0x000F2CA8 File Offset: 0x000F10A8
		public override int SeedPart
		{
			get
			{
				return 401463656;
			}
		}

		// Token: 0x06001C46 RID: 7238 RVA: 0x000F2CC2 File Offset: 0x000F10C2
		public override void GenerateFresh(string seed)
		{
			this.GenerateLakes();
		}

		// Token: 0x06001C47 RID: 7239 RVA: 0x000F2CCC File Offset: 0x000F10CC
		private void GenerateLakes()
		{
			WorldGrid grid = Find.WorldGrid;
			bool[] touched = new bool[grid.TilesCount];
			List<int> oceanChunk = new List<int>();
			for (int i = 0; i < grid.TilesCount; i++)
			{
				if (!touched[i])
				{
					if (grid[i].biome == BiomeDefOf.Ocean)
					{
						Find.WorldFloodFiller.FloodFill(i, (int tid) => grid[tid].biome == BiomeDefOf.Ocean, delegate(int tid)
						{
							oceanChunk.Add(tid);
							touched[tid] = true;
						}, int.MaxValue, null);
						if (oceanChunk.Count <= 15)
						{
							for (int j = 0; j < oceanChunk.Count; j++)
							{
								grid[oceanChunk[j]].biome = BiomeDefOf.Lake;
							}
						}
						oceanChunk.Clear();
					}
				}
			}
		}

		// Token: 0x040010EB RID: 4331
		private const int LakeMaxSize = 15;
	}
}
