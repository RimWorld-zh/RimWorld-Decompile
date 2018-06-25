using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200025A RID: 602
	public class CompProperties_UseEffect : CompProperties
	{
		// Token: 0x040004C0 RID: 1216
		public bool doCameraShake;

		// Token: 0x06000A98 RID: 2712 RVA: 0x0005FEFF File Offset: 0x0005E2FF
		public CompProperties_UseEffect()
		{
			this.compClass = typeof(CompUseEffect);
		}
	}
}
