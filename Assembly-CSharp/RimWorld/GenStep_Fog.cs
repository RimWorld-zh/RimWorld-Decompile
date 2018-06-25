using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020003EC RID: 1004
	public class GenStep_Fog : GenStep
	{
		// Token: 0x1700024C RID: 588
		// (get) Token: 0x0600113B RID: 4411 RVA: 0x00094030 File Offset: 0x00092430
		public override int SeedPart
		{
			get
			{
				return 1568957891;
			}
		}

		// Token: 0x0600113C RID: 4412 RVA: 0x0009404C File Offset: 0x0009244C
		public override void Generate(Map map)
		{
			DeepProfiler.Start("GenerateInitialFogGrid");
			map.fogGrid.SetAllFogged();
			FloodFillerFog.FloodUnfog(MapGenerator.PlayerStartSpot, map);
			List<IntVec3> rootsToUnfog = MapGenerator.rootsToUnfog;
			for (int i = 0; i < rootsToUnfog.Count; i++)
			{
				FloodFillerFog.FloodUnfog(rootsToUnfog[i], map);
			}
			DeepProfiler.End();
		}
	}
}
