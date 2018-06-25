using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200025B RID: 603
	public class CompProperties_Flickable : CompProperties
	{
		// Token: 0x040004BF RID: 1215
		[NoTranslate]
		public string commandTexture = "UI/Commands/DesirePower";

		// Token: 0x040004C0 RID: 1216
		[NoTranslate]
		public string commandLabelKey = "CommandDesignateTogglePowerLabel";

		// Token: 0x040004C1 RID: 1217
		[NoTranslate]
		public string commandDescKey = "CommandDesignateTogglePowerDesc";

		// Token: 0x06000A9A RID: 2714 RVA: 0x0005FFD8 File Offset: 0x0005E3D8
		public CompProperties_Flickable()
		{
			this.compClass = typeof(CompFlickable);
		}
	}
}
