using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020009A9 RID: 2473
	public abstract class StatPart_Curve : StatPart
	{
		// Token: 0x06003768 RID: 14184
		protected abstract bool AppliesTo(StatRequest req);

		// Token: 0x06003769 RID: 14185
		protected abstract float CurveXGetter(StatRequest req);

		// Token: 0x0600376A RID: 14186
		protected abstract string ExplanationLabel(StatRequest req);

		// Token: 0x0600376B RID: 14187 RVA: 0x001D9254 File Offset: 0x001D7654
		public override void TransformValue(StatRequest req, ref float val)
		{
			if (req.HasThing && this.AppliesTo(req))
			{
				val *= this.curve.Evaluate(this.CurveXGetter(req));
			}
		}

		// Token: 0x0600376C RID: 14188 RVA: 0x001D9288 File Offset: 0x001D7688
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
