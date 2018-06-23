using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000258 RID: 600
	public class CompProperties_UseEffect : CompProperties
	{
		// Token: 0x040004BE RID: 1214
		public bool doCameraShake;

		// Token: 0x06000A95 RID: 2709 RVA: 0x0005FDB3 File Offset: 0x0005E1B3
		public CompProperties_UseEffect()
		{
			this.compClass = typeof(CompUseEffect);
		}
	}
}
