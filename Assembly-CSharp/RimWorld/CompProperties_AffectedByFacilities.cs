using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200023C RID: 572
	public class CompProperties_AffectedByFacilities : CompProperties
	{
		// Token: 0x06000A67 RID: 2663 RVA: 0x0005E69D File Offset: 0x0005CA9D
		public CompProperties_AffectedByFacilities()
		{
			this.compClass = typeof(CompAffectedByFacilities);
		}

		// Token: 0x04000456 RID: 1110
		public List<ThingDef> linkableFacilities = null;
	}
}
