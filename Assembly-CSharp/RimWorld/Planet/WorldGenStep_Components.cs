using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020005BD RID: 1469
	public class WorldGenStep_Components : WorldGenStep
	{
		// Token: 0x1700041D RID: 1053
		// (get) Token: 0x06001C34 RID: 7220 RVA: 0x000F2CEC File Offset: 0x000F10EC
		public override int SeedPart
		{
			get
			{
				return 508565678;
			}
		}

		// Token: 0x06001C35 RID: 7221 RVA: 0x000F2D06 File Offset: 0x000F1106
		public override void GenerateFresh(string seed)
		{
			Find.World.ConstructComponents();
		}

		// Token: 0x06001C36 RID: 7222 RVA: 0x000F2D13 File Offset: 0x000F1113
		public override void GenerateWithoutWorldData(string seed)
		{
			this.GenerateFromScribe(seed);
		}

		// Token: 0x06001C37 RID: 7223 RVA: 0x000F2D1D File Offset: 0x000F111D
		public override void GenerateFromScribe(string seed)
		{
			Find.World.ConstructComponents();
			Find.World.ExposeComponents();
		}
	}
}
