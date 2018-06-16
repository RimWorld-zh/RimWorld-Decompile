using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020005BF RID: 1471
	public class WorldGenStep_Components : WorldGenStep
	{
		// Token: 0x1700041D RID: 1053
		// (get) Token: 0x06001C37 RID: 7223 RVA: 0x000F2AD0 File Offset: 0x000F0ED0
		public override int SeedPart
		{
			get
			{
				return 508565678;
			}
		}

		// Token: 0x06001C38 RID: 7224 RVA: 0x000F2AEA File Offset: 0x000F0EEA
		public override void GenerateFresh(string seed)
		{
			Find.World.ConstructComponents();
		}

		// Token: 0x06001C39 RID: 7225 RVA: 0x000F2AF7 File Offset: 0x000F0EF7
		public override void GenerateWithoutWorldData(string seed)
		{
			this.GenerateFromScribe(seed);
		}

		// Token: 0x06001C3A RID: 7226 RVA: 0x000F2B01 File Offset: 0x000F0F01
		public override void GenerateFromScribe(string seed)
		{
			Find.World.ConstructComponents();
			Find.World.ExposeComponents();
		}
	}
}
