using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020009B2 RID: 2482
	public class StatPart_Quality : StatPart
	{
		// Token: 0x040023AC RID: 9132
		private float factorAwful = 1f;

		// Token: 0x040023AD RID: 9133
		private float factorPoor = 1f;

		// Token: 0x040023AE RID: 9134
		private float factorNormal = 1f;

		// Token: 0x040023AF RID: 9135
		private float factorGood = 1f;

		// Token: 0x040023B0 RID: 9136
		private float factorExcellent = 1f;

		// Token: 0x040023B1 RID: 9137
		private float factorMasterwork = 1f;

		// Token: 0x040023B2 RID: 9138
		private float factorLegendary = 1f;

		// Token: 0x040023B3 RID: 9139
		private bool alsoAppliesToNegativeValues = false;

		// Token: 0x060037A3 RID: 14243 RVA: 0x001DA52B File Offset: 0x001D892B
		public override void TransformValue(StatRequest req, ref float val)
		{
			if (val > 0f || this.alsoAppliesToNegativeValues)
			{
				val *= this.QualityMultiplier(req.QualityCategory);
			}
		}

		// Token: 0x060037A4 RID: 14244 RVA: 0x001DA55C File Offset: 0x001D895C
		public override string ExplanationPart(StatRequest req)
		{
			string result;
			if (req.HasThing && !this.alsoAppliesToNegativeValues && req.Thing.GetStatValue(this.parentStat, true) <= 0f)
			{
				result = null;
			}
			else
			{
				if (req.HasThing)
				{
					QualityCategory qc;
					if (req.Thing.TryGetQuality(out qc))
					{
						return "StatsReport_QualityMultiplier".Translate() + ": x" + this.QualityMultiplier(qc).ToStringPercent();
					}
				}
				result = null;
			}
			return result;
		}

		// Token: 0x060037A5 RID: 14245 RVA: 0x001DA5F8 File Offset: 0x001D89F8
		private float QualityMultiplier(QualityCategory qc)
		{
			float result;
			switch (qc)
			{
			case QualityCategory.Awful:
				result = this.factorAwful;
				break;
			case QualityCategory.Poor:
				result = this.factorPoor;
				break;
			case QualityCategory.Normal:
				result = this.factorNormal;
				break;
			case QualityCategory.Good:
				result = this.factorGood;
				break;
			case QualityCategory.Excellent:
				result = this.factorExcellent;
				break;
			case QualityCategory.Masterwork:
				result = this.factorMasterwork;
				break;
			case QualityCategory.Legendary:
				result = this.factorLegendary;
				break;
			default:
				throw new ArgumentOutOfRangeException();
			}
			return result;
		}
	}
}
