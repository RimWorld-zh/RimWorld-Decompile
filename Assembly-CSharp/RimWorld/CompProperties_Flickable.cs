using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000259 RID: 601
	public class CompProperties_Flickable : CompProperties
	{
		// Token: 0x06000A96 RID: 2710 RVA: 0x0005FE88 File Offset: 0x0005E288
		public CompProperties_Flickable()
		{
			this.compClass = typeof(CompFlickable);
		}

		// Token: 0x040004BF RID: 1215
		[NoTranslate]
		public string commandTexture = "UI/Commands/DesirePower";

		// Token: 0x040004C0 RID: 1216
		[NoTranslate]
		public string commandLabelKey = "CommandDesignateTogglePowerLabel";

		// Token: 0x040004C1 RID: 1217
		[NoTranslate]
		public string commandDescKey = "CommandDesignateTogglePowerDesc";
	}
}
