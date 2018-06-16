using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000259 RID: 601
	public class CompProperties_Flickable : CompProperties
	{
		// Token: 0x06000A98 RID: 2712 RVA: 0x0005FE2C File Offset: 0x0005E22C
		public CompProperties_Flickable()
		{
			this.compClass = typeof(CompFlickable);
		}

		// Token: 0x040004C1 RID: 1217
		[NoTranslate]
		public string commandTexture = "UI/Commands/DesirePower";

		// Token: 0x040004C2 RID: 1218
		[NoTranslate]
		public string commandLabelKey = "CommandDesignateTogglePowerLabel";

		// Token: 0x040004C3 RID: 1219
		[NoTranslate]
		public string commandDescKey = "CommandDesignateTogglePowerDesc";
	}
}
