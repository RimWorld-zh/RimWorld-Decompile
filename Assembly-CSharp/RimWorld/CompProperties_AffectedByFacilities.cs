using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200023E RID: 574
	public class CompProperties_AffectedByFacilities : CompProperties
	{
		// Token: 0x04000454 RID: 1108
		public List<ThingDef> linkableFacilities = null;

		// Token: 0x06000A69 RID: 2665 RVA: 0x0005E849 File Offset: 0x0005CC49
		public CompProperties_AffectedByFacilities()
		{
			this.compClass = typeof(CompAffectedByFacilities);
		}
	}
}
