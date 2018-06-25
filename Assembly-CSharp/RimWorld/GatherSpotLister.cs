using System;
using System.Collections.Generic;

namespace RimWorld
{
	// Token: 0x02000715 RID: 1813
	public class GatherSpotLister
	{
		// Token: 0x040015DE RID: 5598
		public List<CompGatherSpot> activeSpots = new List<CompGatherSpot>();

		// Token: 0x060027D6 RID: 10198 RVA: 0x0015525D File Offset: 0x0015365D
		public void RegisterActivated(CompGatherSpot spot)
		{
			if (!this.activeSpots.Contains(spot))
			{
				this.activeSpots.Add(spot);
			}
		}

		// Token: 0x060027D7 RID: 10199 RVA: 0x0015527D File Offset: 0x0015367D
		public void RegisterDeactivated(CompGatherSpot spot)
		{
			if (this.activeSpots.Contains(spot))
			{
				this.activeSpots.Remove(spot);
			}
		}
	}
}
