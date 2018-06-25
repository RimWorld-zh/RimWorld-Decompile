using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200025B RID: 603
	public class CompProperties_Flickable : CompProperties
	{
		// Token: 0x040004C1 RID: 1217
		[NoTranslate]
		public string commandTexture = "UI/Commands/DesirePower";

		// Token: 0x040004C2 RID: 1218
		[NoTranslate]
		public string commandLabelKey = "CommandDesignateTogglePowerLabel";

		// Token: 0x040004C3 RID: 1219
		[NoTranslate]
		public string commandDescKey = "CommandDesignateTogglePowerDesc";

		// Token: 0x06000A99 RID: 2713 RVA: 0x0005FFD4 File Offset: 0x0005E3D4
		public CompProperties_Flickable()
		{
			this.compClass = typeof(CompFlickable);
		}
	}
}
