using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000258 RID: 600
	public class CompProperties_UseEffect : CompProperties
	{
		// Token: 0x06000A97 RID: 2711 RVA: 0x0005FD57 File Offset: 0x0005E157
		public CompProperties_UseEffect()
		{
			this.compClass = typeof(CompUseEffect);
		}

		// Token: 0x040004C0 RID: 1216
		public bool doCameraShake;
	}
}
