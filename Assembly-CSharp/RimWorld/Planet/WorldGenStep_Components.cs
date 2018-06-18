using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020005BF RID: 1471
	public class WorldGenStep_Components : WorldGenStep
	{
		// Token: 0x1700041D RID: 1053
		// (get) Token: 0x06001C39 RID: 7225 RVA: 0x000F2B48 File Offset: 0x000F0F48
		public override int SeedPart
		{
			get
			{
				return 508565678;
			}
		}

		// Token: 0x06001C3A RID: 7226 RVA: 0x000F2B62 File Offset: 0x000F0F62
		public override void GenerateFresh(string seed)
		{
			Find.World.ConstructComponents();
		}

		// Token: 0x06001C3B RID: 7227 RVA: 0x000F2B6F File Offset: 0x000F0F6F
		public override void GenerateWithoutWorldData(string seed)
		{
			this.GenerateFromScribe(seed);
		}

		// Token: 0x06001C3C RID: 7228 RVA: 0x000F2B79 File Offset: 0x000F0F79
		public override void GenerateFromScribe(string seed)
		{
			Find.World.ConstructComponents();
			Find.World.ExposeComponents();
		}
	}
}
