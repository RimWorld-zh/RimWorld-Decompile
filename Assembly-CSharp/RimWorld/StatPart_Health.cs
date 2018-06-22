using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020009AA RID: 2474
	public class StatPart_Health : StatPart_Curve
	{
		// Token: 0x0600377C RID: 14204 RVA: 0x001D9AD8 File Offset: 0x001D7ED8
		protected override bool AppliesTo(StatRequest req)
		{
			return req.HasThing && req.Thing.def.useHitPoints;
		}

		// Token: 0x0600377D RID: 14205 RVA: 0x001D9B10 File Offset: 0x001D7F10
		protected override float CurveXGetter(StatRequest req)
		{
			return (float)req.Thing.HitPoints / (float)req.Thing.MaxHitPoints;
		}

		// Token: 0x0600377E RID: 14206 RVA: 0x001D9B40 File Offset: 0x001D7F40
		protected override string ExplanationLabel(StatRequest req)
		{
			return "StatsReport_HealthMultiplier".Translate(new object[]
			{
				req.Thing.HitPoints + " / " + req.Thing.MaxHitPoints
			});
		}
	}
}
