using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x0200060D RID: 1549
	public abstract class TransportPodsArrivalAction : IExposable
	{
		// Token: 0x06001F34 RID: 7988 RVA: 0x0010EE48 File Offset: 0x0010D248
		public virtual FloatMenuAcceptanceReport StillValid(IEnumerable<IThingHolder> pods, int destinationTile)
		{
			return true;
		}

		// Token: 0x06001F35 RID: 7989 RVA: 0x0010EE64 File Offset: 0x0010D264
		public virtual bool ShouldUseLongEvent(List<ActiveDropPodInfo> pods, int tile)
		{
			return false;
		}

		// Token: 0x06001F36 RID: 7990
		public abstract void Arrived(List<ActiveDropPodInfo> pods, int tile);

		// Token: 0x06001F37 RID: 7991 RVA: 0x0010EE7A File Offset: 0x0010D27A
		public virtual void ExposeData()
		{
		}
	}
}
