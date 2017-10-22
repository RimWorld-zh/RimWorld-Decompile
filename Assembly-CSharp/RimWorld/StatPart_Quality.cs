using System;
using Verse;

namespace RimWorld
{
	public class StatPart_Quality : StatPart
	{
		private float factorAwful = 1f;

		private float factorShoddy = 1f;

		private float factorPoor = 1f;

		private float factorNormal = 1f;

		private float factorGood = 1f;

		private float factorSuperior = 1f;

		private float factorExcellent = 1f;

		private float factorMasterwork = 1f;

		private float factorLegendary = 1f;

		private bool alsoAppliesToNegativeValues = false;

		public override void TransformValue(StatRequest req, ref float val)
		{
			if (val <= 0.0 && !this.alsoAppliesToNegativeValues)
				return;
			val *= this.QualityMultiplier(req.QualityCategory);
		}

		public override string ExplanationPart(StatRequest req)
		{
			QualityCategory qc = default(QualityCategory);
			return (!req.HasThing || this.alsoAppliesToNegativeValues || !(req.Thing.GetStatValue(base.parentStat, true) <= 0.0)) ? ((!req.HasThing || !req.Thing.TryGetQuality(out qc)) ? null : ("StatsReport_QualityMultiplier".Translate() + ": x" + this.QualityMultiplier(qc).ToStringPercent())) : null;
		}

		private float QualityMultiplier(QualityCategory qc)
		{
			float result;
			switch (qc)
			{
			case QualityCategory.Awful:
			{
				result = this.factorAwful;
				break;
			}
			case QualityCategory.Shoddy:
			{
				result = this.factorShoddy;
				break;
			}
			case QualityCategory.Poor:
			{
				result = this.factorPoor;
				break;
			}
			case QualityCategory.Normal:
			{
				result = this.factorNormal;
				break;
			}
			case QualityCategory.Good:
			{
				result = this.factorGood;
				break;
			}
			case QualityCategory.Superior:
			{
				result = this.factorSuperior;
				break;
			}
			case QualityCategory.Excellent:
			{
				result = this.factorExcellent;
				break;
			}
			case QualityCategory.Masterwork:
			{
				result = this.factorMasterwork;
				break;
			}
			case QualityCategory.Legendary:
			{
				result = this.factorLegendary;
				break;
			}
			default:
			{
				throw new ArgumentOutOfRangeException();
			}
			}
			return result;
		}
	}
}
