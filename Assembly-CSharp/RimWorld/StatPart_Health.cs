using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020009AE RID: 2478
	public class StatPart_Health : StatPart_Curve
	{
		// Token: 0x06003783 RID: 14211 RVA: 0x001D9914 File Offset: 0x001D7D14
		protected override bool AppliesTo(StatRequest req)
		{
			return req.HasThing && req.Thing.def.useHitPoints;
		}

		// Token: 0x06003784 RID: 14212 RVA: 0x001D994C File Offset: 0x001D7D4C
		protected override float CurveXGetter(StatRequest req)
		{
			return (float)req.Thing.HitPoints / (float)req.Thing.MaxHitPoints;
		}

		// Token: 0x06003785 RID: 14213 RVA: 0x001D997C File Offset: 0x001D7D7C
		protected override string ExplanationLabel(StatRequest req)
		{
			return "StatsReport_HealthMultiplier".Translate(new object[]
			{
				req.Thing.HitPoints + " / " + req.Thing.MaxHitPoints
			});
		}
	}
}
