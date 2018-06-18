using System;
using System.Collections.Generic;

namespace RimWorld
{
	// Token: 0x02000717 RID: 1815
	public class GatherSpotLister
	{
		// Token: 0x060027DA RID: 10202 RVA: 0x00154F55 File Offset: 0x00153355
		public void RegisterActivated(CompGatherSpot spot)
		{
			if (!this.activeSpots.Contains(spot))
			{
				this.activeSpots.Add(spot);
			}
		}

		// Token: 0x060027DB RID: 10203 RVA: 0x00154F75 File Offset: 0x00153375
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
