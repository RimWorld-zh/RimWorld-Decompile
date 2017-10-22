using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public class StorytellerComp_ShipChunkDrop : StorytellerComp
	{
		private const float BaseShipChunkDropMTBDays = 20f;

		private float ShipChunkDropMTBDays
		{
			get
			{
				float num = (float)((float)Find.TickManager.TicksGame / 3600000.0);
				if (num > 10.0)
				{
					num = 2.75f;
				}
				return (float)(20.0 * Mathf.Pow(2f, num));
			}
		}

		public override IEnumerable<FiringIncident> MakeIntervalIncidents(IIncidentTarget target)
		{
			if (Rand.MTBEventOccurs(this.ShipChunkDropMTBDays, 60000f, 1000f))
			{
				IncidentDef def = IncidentDefOf.ShipChunkDrop;
				if (def.TargetAllowed(target))
				{
					yield return new FiringIncident(def, this, this.GenerateParms(def.category, target));
				}
			}
		}
	}
}
