using System;
using Verse;

namespace RimWorld
{
	internal class IncidentWorker_PoisonShipPartCrash : IncidentWorker_ShipPartCrash
	{
		public IncidentWorker_PoisonShipPartCrash()
		{
		}

		protected override int CountToSpawn
		{
			get
			{
				return Rand.RangeInclusive(1, 1);
			}
		}
	}
}
