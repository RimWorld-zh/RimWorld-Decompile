using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020005C7 RID: 1479
	public abstract class CaravanArrivalAction : IExposable
	{
		// Token: 0x1700042C RID: 1068
		// (get) Token: 0x06001CBA RID: 7354
		public abstract string Label { get; }

		// Token: 0x1700042D RID: 1069
		// (get) Token: 0x06001CBB RID: 7355
		public abstract string ReportString { get; }

		// Token: 0x06001CBC RID: 7356 RVA: 0x000F6CA4 File Offset: 0x000F50A4
		public virtual FloatMenuAcceptanceReport StillValid(Caravan caravan, int destinationTile)
		{
			return true;
		}

		// Token: 0x06001CBD RID: 7357
		public abstract void Arrived(Caravan caravan);

		// Token: 0x06001CBE RID: 7358 RVA: 0x000F6CBF File Offset: 0x000F50BF
		public virtual void ExposeData()
		{
		}
	}
}
