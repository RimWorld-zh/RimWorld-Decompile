using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000455 RID: 1109
	public interface IPlantToGrowSettable
	{
		// Token: 0x170002A5 RID: 677
		// (get) Token: 0x06001362 RID: 4962
		Map Map { get; }

		// Token: 0x170002A6 RID: 678
		// (get) Token: 0x06001363 RID: 4963
		IEnumerable<IntVec3> Cells { get; }

		// Token: 0x06001364 RID: 4964
		ThingDef GetPlantDefToGrow();

		// Token: 0x06001365 RID: 4965
		void SetPlantDefToGrow(ThingDef plantDef);

		// Token: 0x06001366 RID: 4966
		bool CanAcceptSowNow();
	}
}
