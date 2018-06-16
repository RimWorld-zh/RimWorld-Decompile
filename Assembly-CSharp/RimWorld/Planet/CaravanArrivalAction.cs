using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020005CB RID: 1483
	public abstract class CaravanArrivalAction : IExposable
	{
		// Token: 0x1700042C RID: 1068
		// (get) Token: 0x06001CC1 RID: 7361
		public abstract string Label { get; }

		// Token: 0x1700042D RID: 1069
		// (get) Token: 0x06001CC2 RID: 7362
		public abstract string ReportString { get; }

		// Token: 0x06001CC3 RID: 7363 RVA: 0x000F6BD8 File Offset: 0x000F4FD8
		public virtual FloatMenuAcceptanceReport StillValid(Caravan caravan, int destinationTile)
		{
			return true;
		}

		// Token: 0x06001CC4 RID: 7364
		public abstract void Arrived(Caravan caravan);

		// Token: 0x06001CC5 RID: 7365 RVA: 0x000F6BF3 File Offset: 0x000F4FF3
		public virtual void ExposeData()
		{
		}
	}
}
