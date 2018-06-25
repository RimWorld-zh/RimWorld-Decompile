using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200025A RID: 602
	public class CompProperties_UseEffect : CompProperties
	{
		// Token: 0x040004BE RID: 1214
		public bool doCameraShake;

		// Token: 0x06000A99 RID: 2713 RVA: 0x0005FF03 File Offset: 0x0005E303
		public CompProperties_UseEffect()
		{
			this.compClass = typeof(CompUseEffect);
		}
	}
}
