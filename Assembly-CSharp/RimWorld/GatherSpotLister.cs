using System;
using System.Collections.Generic;

namespace RimWorld
{
	// Token: 0x02000713 RID: 1811
	public class GatherSpotLister
	{
		// Token: 0x040015DE RID: 5598
		public List<CompGatherSpot> activeSpots = new List<CompGatherSpot>();

		// Token: 0x060027D2 RID: 10194 RVA: 0x0015510D File Offset: 0x0015350D
		public void RegisterActivated(CompGatherSpot spot)
		{
			if (!this.activeSpots.Contains(spot))
			{
				this.activeSpots.Add(spot);
			}
		}

		// Token: 0x060027D3 RID: 10195 RVA: 0x0015512D File Offset: 0x0015352D
		public void RegisterDeactivated(CompGatherSpot spot)
		{
			if (this.activeSpots.Contains(spot))
			{
				this.activeSpots.Remove(spot);
			}
		}
	}
}
