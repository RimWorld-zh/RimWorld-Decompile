using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000249 RID: 585
	public class CompProperties_Glower : CompProperties
	{
		// Token: 0x0400048F RID: 1167
		public float overlightRadius = 0f;

		// Token: 0x04000490 RID: 1168
		public float glowRadius = 14f;

		// Token: 0x04000491 RID: 1169
		public ColorInt glowColor = new ColorInt(255, 255, 255, 0) * 1.45f;

		// Token: 0x06000A7F RID: 2687 RVA: 0x0005F410 File Offset: 0x0005D810
		public CompProperties_Glower()
		{
			this.compClass = typeof(CompGlower);
		}
	}
}
