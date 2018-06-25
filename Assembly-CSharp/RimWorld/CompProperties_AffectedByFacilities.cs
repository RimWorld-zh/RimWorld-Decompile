using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200023E RID: 574
	public class CompProperties_AffectedByFacilities : CompProperties
	{
		// Token: 0x04000456 RID: 1110
		public List<ThingDef> linkableFacilities = null;

		// Token: 0x06000A68 RID: 2664 RVA: 0x0005E845 File Offset: 0x0005CC45
		public CompProperties_AffectedByFacilities()
		{
			this.compClass = typeof(CompAffectedByFacilities);
		}
	}
}
