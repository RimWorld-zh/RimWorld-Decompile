using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000374 RID: 884
	public class StorytellerComp_ShipChunkDrop : StorytellerComp
	{
		// Token: 0x0400095C RID: 2396
		private static readonly SimpleCurve ShipChunkDropMTBDaysCurve = new SimpleCurve
		{
			{
				new CurvePoint(0f, 20f),
				true
			},
			{
				new CurvePoint(1f, 40f),
				true
			},
			{
				new CurvePoint(2f, 80f),
				true
			},
			{
				new CurvePoint(2.75f, 135f),
				true
			}
		};

		// Token: 0x17000224 RID: 548
		// (get) Token: 0x06000F46 RID: 3910 RVA: 0x000815F8 File Offset: 0x0007F9F8
		private float ShipChunkDropMTBDays
		{
			get
			{
				float x = (float)Find.TickManager.TicksGame / 3600000f;
				return StorytellerComp_ShipChunkDrop.ShipChunkDropMTBDaysCurve.Evaluate(x);
			}
		}

		// Token: 0x06000F47 RID: 3911 RVA: 0x0008162C File Offset: 0x0007FA2C
		public override IEnumerable<FiringIncident> MakeIntervalIncidents(IIncidentTarget target)
		{
			if (Rand.MTBEventOccurs(this.ShipChunkDropMTBDays, 60000f, 1000f))
			{
				IncidentDef def = IncidentDefOf.ShipChunkDrop;
				IncidentParms parms = this.GenerateParms(def.category, target);
				if (def.Worker.CanFireNow(parms))
				{
					yield return new FiringIncident(def, this, parms);
				}
			}
			yield break;
		}
	}
}
