using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x0200060F RID: 1551
	public abstract class TransportPodsArrivalAction : IExposable
	{
		// Token: 0x06001F38 RID: 7992 RVA: 0x0010EF98 File Offset: 0x0010D398
		public virtual FloatMenuAcceptanceReport StillValid(IEnumerable<IThingHolder> pods, int destinationTile)
		{
			return true;
		}

		// Token: 0x06001F39 RID: 7993 RVA: 0x0010EFB4 File Offset: 0x0010D3B4
		public virtual bool ShouldUseLongEvent(List<ActiveDropPodInfo> pods, int tile)
		{
			return false;
		}

		// Token: 0x06001F3A RID: 7994
		public abstract void Arrived(List<ActiveDropPodInfo> pods, int tile);

		// Token: 0x06001F3B RID: 7995 RVA: 0x0010EFCA File Offset: 0x0010D3CA
		public virtual void ExposeData()
		{
		}
	}
}
