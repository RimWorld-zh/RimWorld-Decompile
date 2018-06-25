using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000374 RID: 884
	public class StorytellerComp_ShipChunkDrop : StorytellerComp
	{
		// Token: 0x04000959 RID: 2393
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
		// (get) Token: 0x06000F47 RID: 3911 RVA: 0x000815E8 File Offset: 0x0007F9E8
		private float ShipChunkDropMTBDays
		{
			get
			{
				float x = (float)Find.TickManager.TicksGame / 3600000f;
				return StorytellerComp_ShipChunkDrop.ShipChunkDropMTBDaysCurve.Evaluate(x);
			}
		}

		// Token: 0x06000F48 RID: 3912 RVA: 0x0008161C File Offset: 0x0007FA1C
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
