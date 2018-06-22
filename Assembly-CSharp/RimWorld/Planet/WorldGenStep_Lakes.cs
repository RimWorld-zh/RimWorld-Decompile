using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020005BE RID: 1470
	public class WorldGenStep_Lakes : WorldGenStep
	{
		// Token: 0x17000420 RID: 1056
		// (get) Token: 0x06001C3E RID: 7230 RVA: 0x000F2D74 File Offset: 0x000F1174
		public override int SeedPart
		{
			get
			{
				return 401463656;
			}
		}

		// Token: 0x06001C3F RID: 7231 RVA: 0x000F2D8E File Offset: 0x000F118E
		public override void GenerateFresh(string seed)
		{
			this.GenerateLakes();
		}

		// Token: 0x06001C40 RID: 7232 RVA: 0x000F2D98 File Offset: 0x000F1198
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

		// Token: 0x040010E8 RID: 4328
		private const int LakeMaxSize = 15;
	}
}
