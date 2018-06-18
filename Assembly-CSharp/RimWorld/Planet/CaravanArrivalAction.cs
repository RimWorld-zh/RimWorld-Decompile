using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020005CB RID: 1483
	public abstract class CaravanArrivalAction : IExposable
	{
		// Token: 0x1700042C RID: 1068
		// (get) Token: 0x06001CC3 RID: 7363
		public abstract string Label { get; }

		// Token: 0x1700042D RID: 1069
		// (get) Token: 0x06001CC4 RID: 7364
		public abstract string ReportString { get; }

		// Token: 0x06001CC5 RID: 7365 RVA: 0x000F6C50 File Offset: 0x000F5050
		public virtual FloatMenuAcceptanceReport StillValid(Caravan caravan, int destinationTile)
		{
			return true;
		}

		// Token: 0x06001CC6 RID: 7366
		public abstract void Arrived(Caravan caravan);

		// Token: 0x06001CC7 RID: 7367 RVA: 0x000F6C6B File Offset: 0x000F506B
		public virtual void ExposeData()
		{
		}
	}
}
