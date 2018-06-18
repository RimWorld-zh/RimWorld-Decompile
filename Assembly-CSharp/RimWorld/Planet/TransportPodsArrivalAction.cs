using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02000611 RID: 1553
	public abstract class TransportPodsArrivalAction : IExposable
	{
		// Token: 0x06001F3D RID: 7997 RVA: 0x0010EDF4 File Offset: 0x0010D1F4
		public virtual FloatMenuAcceptanceReport StillValid(IEnumerable<IThingHolder> pods, int destinationTile)
		{
			return true;
		}

		// Token: 0x06001F3E RID: 7998 RVA: 0x0010EE10 File Offset: 0x0010D210
		public virtual bool ShouldUseLongEvent(List<ActiveDropPodInfo> pods, int tile)
		{
			return false;
		}

		// Token: 0x06001F3F RID: 7999
		public abstract void Arrived(List<ActiveDropPodInfo> pods, int tile);

		// Token: 0x06001F40 RID: 8000 RVA: 0x0010EE26 File Offset: 0x0010D226
		public virtual void ExposeData()
		{
		}
	}
}
