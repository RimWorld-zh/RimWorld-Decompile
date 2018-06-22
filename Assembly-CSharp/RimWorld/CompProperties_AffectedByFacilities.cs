using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200023C RID: 572
	public class CompProperties_AffectedByFacilities : CompProperties
	{
		// Token: 0x06000A65 RID: 2661 RVA: 0x0005E6F9 File Offset: 0x0005CAF9
		public CompProperties_AffectedByFacilities()
		{
			this.compClass = typeof(CompAffectedByFacilities);
		}

		// Token: 0x04000454 RID: 1108
		public List<ThingDef> linkableFacilities = null;
	}
}
