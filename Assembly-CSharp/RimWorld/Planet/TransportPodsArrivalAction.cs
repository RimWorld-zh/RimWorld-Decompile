using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02000611 RID: 1553
	public abstract class TransportPodsArrivalAction : IExposable
	{
		// Token: 0x06001F3B RID: 7995 RVA: 0x0010ED7C File Offset: 0x0010D17C
		public virtual FloatMenuAcceptanceReport StillValid(IEnumerable<IThingHolder> pods, int destinationTile)
		{
			return true;
		}

		// Token: 0x06001F3C RID: 7996 RVA: 0x0010ED98 File Offset: 0x0010D198
		public virtual bool ShouldUseLongEvent(List<ActiveDropPodInfo> pods, int tile)
		{
			return false;
		}

		// Token: 0x06001F3D RID: 7997
		public abstract void Arrived(List<ActiveDropPodInfo> pods, int tile);

		// Token: 0x06001F3E RID: 7998 RVA: 0x0010EDAE File Offset: 0x0010D1AE
		public virtual void ExposeData()
		{
		}
	}
}
