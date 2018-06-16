using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020009AE RID: 2478
	public class StatPart_Health : StatPart_Curve
	{
		// Token: 0x06003781 RID: 14209 RVA: 0x001D9840 File Offset: 0x001D7C40
		protected override bool AppliesTo(StatRequest req)
		{
			return req.HasThing && req.Thing.def.useHitPoints;
		}

		// Token: 0x06003782 RID: 14210 RVA: 0x001D9878 File Offset: 0x001D7C78
		protected override float CurveXGetter(StatRequest req)
		{
			return (float)req.Thing.HitPoints / (float)req.Thing.MaxHitPoints;
		}

		// Token: 0x06003783 RID: 14211 RVA: 0x001D98A8 File Offset: 0x001D7CA8
		protected override string ExplanationLabel(StatRequest req)
		{
			return "StatsReport_HealthMultiplier".Translate(new object[]
			{
				req.Thing.HitPoints + " / " + req.Thing.MaxHitPoints
			});
		}
	}
}
