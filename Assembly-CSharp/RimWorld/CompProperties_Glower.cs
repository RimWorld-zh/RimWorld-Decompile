using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000247 RID: 583
	public class CompProperties_Glower : CompProperties
	{
		// Token: 0x06000A7D RID: 2685 RVA: 0x0005F264 File Offset: 0x0005D664
		public CompProperties_Glower()
		{
			this.compClass = typeof(CompGlower);
		}

		// Token: 0x04000491 RID: 1169
		public float overlightRadius = 0f;

		// Token: 0x04000492 RID: 1170
		public float glowRadius = 14f;

		// Token: 0x04000493 RID: 1171
		public ColorInt glowColor = new ColorInt(255, 255, 255, 0) * 1.45f;
	}
}
