using System;
using System.Collections.Generic;

namespace RimWorld
{
	// Token: 0x02000715 RID: 1813
	public class GatherSpotLister
	{
		// Token: 0x040015E2 RID: 5602
		public List<CompGatherSpot> activeSpots = new List<CompGatherSpot>();

		// Token: 0x060027D5 RID: 10197 RVA: 0x001554BD File Offset: 0x001538BD
		public void RegisterActivated(CompGatherSpot spot)
		{
			if (!this.activeSpots.Contains(spot))
			{
				this.activeSpots.Add(spot);
			}
		}

		// Token: 0x060027D6 RID: 10198 RVA: 0x001554DD File Offset: 0x001538DD
		public void RegisterDeactivated(CompGatherSpot spot)
		{
			if (this.activeSpots.Contains(spot))
			{
				this.activeSpots.Remove(spot);
			}
		}
	}
}
