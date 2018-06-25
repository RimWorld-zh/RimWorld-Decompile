using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020005BD RID: 1469
	public class WorldGenStep_Components : WorldGenStep
	{
		// Token: 0x1700041D RID: 1053
		// (get) Token: 0x06001C33 RID: 7219 RVA: 0x000F2F54 File Offset: 0x000F1354
		public override int SeedPart
		{
			get
			{
				return 508565678;
			}
		}

		// Token: 0x06001C34 RID: 7220 RVA: 0x000F2F6E File Offset: 0x000F136E
		public override void GenerateFresh(string seed)
		{
			Find.World.ConstructComponents();
		}

		// Token: 0x06001C35 RID: 7221 RVA: 0x000F2F7B File Offset: 0x000F137B
		public override void GenerateWithoutWorldData(string seed)
		{
			this.GenerateFromScribe(seed);
		}

		// Token: 0x06001C36 RID: 7222 RVA: 0x000F2F85 File Offset: 0x000F1385
		public override void GenerateFromScribe(string seed)
		{
			Find.World.ConstructComponents();
			Find.World.ExposeComponents();
		}
	}
}
