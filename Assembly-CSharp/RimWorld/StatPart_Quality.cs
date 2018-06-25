using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020009B4 RID: 2484
	public class StatPart_Quality : StatPart
	{
		// Token: 0x040023AD RID: 9133
		private float factorAwful = 1f;

		// Token: 0x040023AE RID: 9134
		private float factorPoor = 1f;

		// Token: 0x040023AF RID: 9135
		private float factorNormal = 1f;

		// Token: 0x040023B0 RID: 9136
		private float factorGood = 1f;

		// Token: 0x040023B1 RID: 9137
		private float factorExcellent = 1f;

		// Token: 0x040023B2 RID: 9138
		private float factorMasterwork = 1f;

		// Token: 0x040023B3 RID: 9139
		private float factorLegendary = 1f;

		// Token: 0x040023B4 RID: 9140
		private bool alsoAppliesToNegativeValues = false;

		// Token: 0x060037A7 RID: 14247 RVA: 0x001DA66B File Offset: 0x001D8A6B
		public override void TransformValue(StatRequest req, ref float val)
		{
			if (val > 0f || this.alsoAppliesToNegativeValues)
			{
				val *= this.QualityMultiplier(req.QualityCategory);
			}
		}

		// Token: 0x060037A8 RID: 14248 RVA: 0x001DA69C File Offset: 0x001D8A9C
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

		// Token: 0x060037A9 RID: 14249 RVA: 0x001DA738 File Offset: 0x001D8B38
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
