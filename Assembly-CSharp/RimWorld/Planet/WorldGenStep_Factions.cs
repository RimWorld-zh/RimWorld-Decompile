using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020005BE RID: 1470
	public class WorldGenStep_Factions : WorldGenStep
	{
		// Token: 0x1700041E RID: 1054
		// (get) Token: 0x06001C39 RID: 7225 RVA: 0x000F2D3C File Offset: 0x000F113C
		public override int SeedPart
		{
			get
			{
				return 777998381;
			}
		}

		// Token: 0x06001C3A RID: 7226 RVA: 0x000F2D56 File Offset: 0x000F1156
		public override void GenerateFresh(string seed)
		{
			FactionGenerator.GenerateFactionsIntoWorld();
		}

		// Token: 0x06001C3B RID: 7227 RVA: 0x000F2D5E File Offset: 0x000F115E
		public override void GenerateWithoutWorldData(string seed)
		{
		}
	}
}
