using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020009AC RID: 2476
	public class StatPart_Health : StatPart_Curve
	{
		// Token: 0x06003780 RID: 14208 RVA: 0x001D9C18 File Offset: 0x001D8018
		protected override bool AppliesTo(StatRequest req)
		{
			return req.HasThing && req.Thing.def.useHitPoints;
		}

		// Token: 0x06003781 RID: 14209 RVA: 0x001D9C50 File Offset: 0x001D8050
		protected override float CurveXGetter(StatRequest req)
		{
			return (float)req.Thing.HitPoints / (float)req.Thing.MaxHitPoints;
		}

		// Token: 0x06003782 RID: 14210 RVA: 0x001D9C80 File Offset: 0x001D8080
		protected override string ExplanationLabel(StatRequest req)
		{
			return "StatsReport_HealthMultiplier".Translate(new object[]
			{
				req.Thing.HitPoints + " / " + req.Thing.MaxHitPoints
			});
		}
	}
}
