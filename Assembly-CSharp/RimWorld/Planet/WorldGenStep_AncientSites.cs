using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020005BA RID: 1466
	public class WorldGenStep_AncientSites : WorldGenStep
	{
		// Token: 0x040010E5 RID: 4325
		public FloatRange ancientSitesPer100kTiles;

		// Token: 0x1700041C RID: 1052
		// (get) Token: 0x06001C2C RID: 7212 RVA: 0x000F2B0C File Offset: 0x000F0F0C
		public override int SeedPart
		{
			get
			{
				return 976238715;
			}
		}

		// Token: 0x06001C2D RID: 7213 RVA: 0x000F2B26 File Offset: 0x000F0F26
		public override void GenerateFresh(string seed)
		{
			this.GenerateAncientSites();
		}

		// Token: 0x06001C2E RID: 7214 RVA: 0x000F2B30 File Offset: 0x000F0F30
		private void GenerateAncientSites()
		{
			int num = GenMath.RoundRandom((float)Find.WorldGrid.TilesCount / 100000f * this.ancientSitesPer100kTiles.RandomInRange);
			for (int i = 0; i < num; i++)
			{
				Find.World.genData.ancientSites.Add(TileFinder.RandomFactionBaseTileFor(null, false, null));
			}
		}
	}
}
