using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020009A9 RID: 2473
	public abstract class StatPart_Curve : StatPart
	{
		// Token: 0x06003766 RID: 14182
		protected abstract bool AppliesTo(StatRequest req);

		// Token: 0x06003767 RID: 14183
		protected abstract float CurveXGetter(StatRequest req);

		// Token: 0x06003768 RID: 14184
		protected abstract string ExplanationLabel(StatRequest req);

		// Token: 0x06003769 RID: 14185 RVA: 0x001D9180 File Offset: 0x001D7580
		public override void TransformValue(StatRequest req, ref float val)
		{
			if (req.HasThing && this.AppliesTo(req))
			{
				val *= this.curve.Evaluate(this.CurveXGetter(req));
			}
		}

		// Token: 0x0600376A RID: 14186 RVA: 0x001D91B4 File Offset: 0x001D75B4
		public override string ExplanationPart(StatRequest req)
		{
			string result;
			if (req.HasThing && this.AppliesTo(req))
			{
				result = this.ExplanationLabel(req) + ": x" + this.curve.Evaluate(this.CurveXGetter(req)).ToStringPercent();
			}
			else
			{
				result = null;
			}
			return result;
		}

		// Token: 0x0400239D RID: 9117
		protected SimpleCurve curve = null;
	}
}
