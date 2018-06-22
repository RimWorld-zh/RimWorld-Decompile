using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020003EA RID: 1002
	public class GenStep_Fog : GenStep
	{
		// Token: 0x1700024C RID: 588
		// (get) Token: 0x06001138 RID: 4408 RVA: 0x00093ED0 File Offset: 0x000922D0
		public override int SeedPart
		{
			get
			{
				return 1568957891;
			}
		}

		// Token: 0x06001139 RID: 4409 RVA: 0x00093EEC File Offset: 0x000922EC
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
