using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020005BC RID: 1468
	public class WorldGenStep_AncientSites : WorldGenStep
	{
		// Token: 0x040010E5 RID: 4325
		public FloatRange ancientSitesPer100kTiles;

		// Token: 0x1700041C RID: 1052
		// (get) Token: 0x06001C30 RID: 7216 RVA: 0x000F2C5C File Offset: 0x000F105C
		public override int SeedPart
		{
			get
			{
				return 976238715;
			}
		}

		// Token: 0x06001C31 RID: 7217 RVA: 0x000F2C76 File Offset: 0x000F1076
		public override void GenerateFresh(string seed)
		{
			this.GenerateAncientSites();
		}

		// Token: 0x06001C32 RID: 7218 RVA: 0x000F2C80 File Offset: 0x000F1080
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
