using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000335 RID: 821
	internal class IncidentWorker_PoisonShipPartCrash : IncidentWorker_ShipPartCrash
	{
		// Token: 0x1700020B RID: 523
		// (get) Token: 0x06000E0D RID: 3597 RVA: 0x00077E64 File Offset: 0x00076264
		protected override int CountToSpawn
		{
			get
			{
				return Rand.RangeInclusive(1, 1);
			}
		}
	}
}
