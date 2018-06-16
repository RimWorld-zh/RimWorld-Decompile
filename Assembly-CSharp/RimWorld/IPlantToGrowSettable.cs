using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000457 RID: 1111
	public interface IPlantToGrowSettable
	{
		// Token: 0x170002A5 RID: 677
		// (get) Token: 0x06001368 RID: 4968
		Map Map { get; }

		// Token: 0x170002A6 RID: 678
		// (get) Token: 0x06001369 RID: 4969
		IEnumerable<IntVec3> Cells { get; }

		// Token: 0x0600136A RID: 4970
		ThingDef GetPlantDefToGrow();

		// Token: 0x0600136B RID: 4971
		void SetPlantDefToGrow(ThingDef plantDef);

		// Token: 0x0600136C RID: 4972
		bool CanAcceptSowNow();
	}
}
