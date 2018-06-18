using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020005BE RID: 1470
	public class WorldGenStep_AncientSites : WorldGenStep
	{
		// Token: 0x1700041C RID: 1052
		// (get) Token: 0x06001C35 RID: 7221 RVA: 0x000F2AB8 File Offset: 0x000F0EB8
		public override int SeedPart
		{
			get
			{
				return 976238715;
			}
		}

		// Token: 0x06001C36 RID: 7222 RVA: 0x000F2AD2 File Offset: 0x000F0ED2
		public override void GenerateFresh(string seed)
		{
			this.GenerateAncientSites();
		}

		// Token: 0x06001C37 RID: 7223 RVA: 0x000F2ADC File Offset: 0x000F0EDC
		private void GenerateAncientSites()
		{
			int num = GenMath.RoundRandom((float)Find.WorldGrid.TilesCount / 100000f * this.ancientSitesPer100kTiles.RandomInRange);
			for (int i = 0; i < num; i++)
			{
				Find.World.genData.ancientSites.Add(TileFinder.RandomFactionBaseTileFor(null, false, null));
			}
		}

		// Token: 0x040010E8 RID: 4328
		public FloatRange ancientSitesPer100kTiles;
	}
}
