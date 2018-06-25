using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020005BE RID: 1470
	public class WorldGenStep_Factions : WorldGenStep
	{
		// Token: 0x1700041E RID: 1054
		// (get) Token: 0x06001C38 RID: 7224 RVA: 0x000F2FA4 File Offset: 0x000F13A4
		public override int SeedPart
		{
			get
			{
				return 777998381;
			}
		}

		// Token: 0x06001C39 RID: 7225 RVA: 0x000F2FBE File Offset: 0x000F13BE
		public override void GenerateFresh(string seed)
		{
			FactionGenerator.GenerateFactionsIntoWorld();
		}

		// Token: 0x06001C3A RID: 7226 RVA: 0x000F2FC6 File Offset: 0x000F13C6
		public override void GenerateWithoutWorldData(string seed)
		{
		}
	}
}
