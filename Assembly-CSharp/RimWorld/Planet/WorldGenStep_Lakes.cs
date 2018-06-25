using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020005C0 RID: 1472
	public class WorldGenStep_Lakes : WorldGenStep
	{
		// Token: 0x040010E8 RID: 4328
		private const int LakeMaxSize = 15;

		// Token: 0x17000420 RID: 1056
		// (get) Token: 0x06001C42 RID: 7234 RVA: 0x000F2EC4 File Offset: 0x000F12C4
		public override int SeedPart
		{
			get
			{
				return 401463656;
			}
		}

		// Token: 0x06001C43 RID: 7235 RVA: 0x000F2EDE File Offset: 0x000F12DE
		public override void GenerateFresh(string seed)
		{
			this.GenerateLakes();
		}

		// Token: 0x06001C44 RID: 7236 RVA: 0x000F2EE8 File Offset: 0x000F12E8
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
	}
}
