using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020005BC RID: 1468
	public class WorldGenStep_Factions : WorldGenStep
	{
		// Token: 0x1700041E RID: 1054
		// (get) Token: 0x06001C35 RID: 7221 RVA: 0x000F2BEC File Offset: 0x000F0FEC
		public override int SeedPart
		{
			get
			{
				return 777998381;
			}
		}

		// Token: 0x06001C36 RID: 7222 RVA: 0x000F2C06 File Offset: 0x000F1006
		public override void GenerateFresh(string seed)
		{
			FactionGenerator.GenerateFactionsIntoWorld();
		}

		// Token: 0x06001C37 RID: 7223 RVA: 0x000F2C0E File Offset: 0x000F100E
		public override void GenerateWithoutWorldData(string seed)
		{
		}
	}
}
