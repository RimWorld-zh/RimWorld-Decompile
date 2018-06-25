using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000337 RID: 823
	internal class IncidentWorker_PoisonShipPartCrash : IncidentWorker_ShipPartCrash
	{
		// Token: 0x1700020B RID: 523
		// (get) Token: 0x06000E10 RID: 3600 RVA: 0x00077FBC File Offset: 0x000763BC
		protected override int CountToSpawn
		{
			get
			{
				return Rand.RangeInclusive(1, 1);
			}
		}
	}
}
