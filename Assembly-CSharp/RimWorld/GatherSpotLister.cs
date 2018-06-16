using System;
using System.Collections.Generic;

namespace RimWorld
{
	// Token: 0x02000717 RID: 1815
	public class GatherSpotLister
	{
		// Token: 0x060027D8 RID: 10200 RVA: 0x00154EDD File Offset: 0x001532DD
		public void RegisterActivated(CompGatherSpot spot)
		{
			if (!this.activeSpots.Contains(spot))
			{
				this.activeSpots.Add(spot);
			}
		}

		// Token: 0x060027D9 RID: 10201 RVA: 0x00154EFD File Offset: 0x001532FD
		public void RegisterDeactivated(CompGatherSpot spot)
		{
			if (this.activeSpots.Contains(spot))
			{
				this.activeSpots.Remove(spot);
			}
		}

		// Token: 0x040015E0 RID: 5600
		public List<CompGatherSpot> activeSpots = new List<CompGatherSpot>();
	}
}
