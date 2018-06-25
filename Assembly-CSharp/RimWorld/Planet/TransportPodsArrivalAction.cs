using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x0200060F RID: 1551
	public abstract class TransportPodsArrivalAction : IExposable
	{
		// Token: 0x06001F37 RID: 7991 RVA: 0x0010F200 File Offset: 0x0010D600
		public virtual FloatMenuAcceptanceReport StillValid(IEnumerable<IThingHolder> pods, int destinationTile)
		{
			return true;
		}

		// Token: 0x06001F38 RID: 7992 RVA: 0x0010F21C File Offset: 0x0010D61C
		public virtual bool ShouldUseLongEvent(List<ActiveDropPodInfo> pods, int tile)
		{
			return false;
		}

		// Token: 0x06001F39 RID: 7993
		public abstract void Arrived(List<ActiveDropPodInfo> pods, int tile);

		// Token: 0x06001F3A RID: 7994 RVA: 0x0010F232 File Offset: 0x0010D632
		public virtual void ExposeData()
		{
		}
	}
}
