using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020009B4 RID: 2484
	public class StatPart_Quality : StatPart
	{
		// Token: 0x040023B4 RID: 9140
		private float factorAwful = 1f;

		// Token: 0x040023B5 RID: 9141
		private float factorPoor = 1f;

		// Token: 0x040023B6 RID: 9142
		private float factorNormal = 1f;

		// Token: 0x040023B7 RID: 9143
		private float factorGood = 1f;

		// Token: 0x040023B8 RID: 9144
		private float factorExcellent = 1f;

		// Token: 0x040023B9 RID: 9145
		private float factorMasterwork = 1f;

		// Token: 0x040023BA RID: 9146
		private float factorLegendary = 1f;

		// Token: 0x040023BB RID: 9147
		private bool alsoAppliesToNegativeValues = false;

		// Token: 0x060037A7 RID: 14247 RVA: 0x001DA93F File Offset: 0x001D8D3F
		public override void TransformValue(StatRequest req, ref float val)
		{
			if (val > 0f || this.alsoAppliesToNegativeValues)
			{
				val *= this.QualityMultiplier(req.QualityCategory);
			}
		}

		// Token: 0x060037A8 RID: 14248 RVA: 0x001DA970 File Offset: 0x001D8D70
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

		// Token: 0x060037A9 RID: 14249 RVA: 0x001DAA0C File Offset: 0x001D8E0C
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
