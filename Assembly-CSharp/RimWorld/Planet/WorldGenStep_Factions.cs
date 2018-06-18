using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020005C0 RID: 1472
	public class WorldGenStep_Factions : WorldGenStep
	{
		// Token: 0x1700041E RID: 1054
		// (get) Token: 0x06001C3E RID: 7230 RVA: 0x000F2B98 File Offset: 0x000F0F98
		public override int SeedPart
		{
			get
			{
				return 777998381;
			}
		}

		// Token: 0x06001C3F RID: 7231 RVA: 0x000F2BB2 File Offset: 0x000F0FB2
		public override void GenerateFresh(string seed)
		{
			FactionGenerator.GenerateFactionsIntoWorld();
		}

		// Token: 0x06001C40 RID: 7232 RVA: 0x000F2BBA File Offset: 0x000F0FBA
		public override void GenerateWithoutWorldData(string seed)
		{
		}
	}
}
