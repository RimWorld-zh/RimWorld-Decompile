using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020005C0 RID: 1472
	public class WorldGenStep_Factions : WorldGenStep
	{
		// Token: 0x1700041E RID: 1054
		// (get) Token: 0x06001C3C RID: 7228 RVA: 0x000F2B20 File Offset: 0x000F0F20
		public override int SeedPart
		{
			get
			{
				return 777998381;
			}
		}

		// Token: 0x06001C3D RID: 7229 RVA: 0x000F2B3A File Offset: 0x000F0F3A
		public override void GenerateFresh(string seed)
		{
			FactionGenerator.GenerateFactionsIntoWorld();
		}

		// Token: 0x06001C3E RID: 7230 RVA: 0x000F2B42 File Offset: 0x000F0F42
		public override void GenerateWithoutWorldData(string seed)
		{
		}
	}
}
