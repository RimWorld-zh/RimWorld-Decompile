using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020009A5 RID: 2469
	public abstract class StatPart_Curve : StatPart
	{
		// Token: 0x06003761 RID: 14177
		protected abstract bool AppliesTo(StatRequest req);

		// Token: 0x06003762 RID: 14178
		protected abstract float CurveXGetter(StatRequest req);

		// Token: 0x06003763 RID: 14179
		protected abstract string ExplanationLabel(StatRequest req);

		// Token: 0x06003764 RID: 14180 RVA: 0x001D9450 File Offset: 0x001D7850
		public override void TransformValue(StatRequest req, ref float val)
		{
			if (req.HasThing && this.AppliesTo(req))
			{
				val *= this.curve.Evaluate(this.CurveXGetter(req));
			}
		}

		// Token: 0x06003765 RID: 14181 RVA: 0x001D9484 File Offset: 0x001D7884
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

		// Token: 0x0400239B RID: 9115
		protected SimpleCurve curve = null;
	}
}
