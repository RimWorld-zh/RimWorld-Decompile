using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000453 RID: 1107
	public interface IPlantToGrowSettable
	{
		// Token: 0x170002A5 RID: 677
		// (get) Token: 0x0600135F RID: 4959
		Map Map { get; }

		// Token: 0x170002A6 RID: 678
		// (get) Token: 0x06001360 RID: 4960
		IEnumerable<IntVec3> Cells { get; }

		// Token: 0x06001361 RID: 4961
		ThingDef GetPlantDefToGrow();

		// Token: 0x06001362 RID: 4962
		void SetPlantDefToGrow(ThingDef plantDef);

		// Token: 0x06001363 RID: 4963
		bool CanAcceptSowNow();
	}
}
