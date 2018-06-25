using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020009A7 RID: 2471
	public abstract class StatPart_Curve : StatPart
	{
		// Token: 0x040023A3 RID: 9123
		protected SimpleCurve curve = null;

		// Token: 0x06003765 RID: 14181
		protected abstract bool AppliesTo(StatRequest req);

		// Token: 0x06003766 RID: 14182
		protected abstract float CurveXGetter(StatRequest req);

		// Token: 0x06003767 RID: 14183
		protected abstract string ExplanationLabel(StatRequest req);

		// Token: 0x06003768 RID: 14184 RVA: 0x001D9864 File Offset: 0x001D7C64
		public override void TransformValue(StatRequest req, ref float val)
		{
			if (req.HasThing && this.AppliesTo(req))
			{
				val *= this.curve.Evaluate(this.CurveXGetter(req));
			}
		}

		// Token: 0x06003769 RID: 14185 RVA: 0x001D9898 File Offset: 0x001D7C98
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
	}
}
