using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020005BB RID: 1467
	public class WorldGenStep_Components : WorldGenStep
	{
		// Token: 0x1700041D RID: 1053
		// (get) Token: 0x06001C30 RID: 7216 RVA: 0x000F2B9C File Offset: 0x000F0F9C
		public override int SeedPart
		{
			get
			{
				return 508565678;
			}
		}

		// Token: 0x06001C31 RID: 7217 RVA: 0x000F2BB6 File Offset: 0x000F0FB6
		public override void GenerateFresh(string seed)
		{
			Find.World.ConstructComponents();
		}

		// Token: 0x06001C32 RID: 7218 RVA: 0x000F2BC3 File Offset: 0x000F0FC3
		public override void GenerateWithoutWorldData(string seed)
		{
			this.GenerateFromScribe(seed);
		}

		// Token: 0x06001C33 RID: 7219 RVA: 0x000F2BCD File Offset: 0x000F0FCD
		public override void GenerateFromScribe(string seed)
		{
			Find.World.ConstructComponents();
			Find.World.ExposeComponents();
		}
	}
}
