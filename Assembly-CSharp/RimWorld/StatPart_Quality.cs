using System;
using Verse;

namespace RimWorld
{
	public class StatPart_Quality : StatPart
	{
		private float factorAwful = 1f;

		private float factorPoor = 1f;

		private float factorNormal = 1f;

		private float factorGood = 1f;

		private float factorExcellent = 1f;

		private float factorMasterwork = 1f;

		private float factorLegendary = 1f;

		private bool alsoAppliesToNegativeValues = false;

		public StatPart_Quality()
		{
		}

		public override void TransformValue(StatRequest req, ref float val)
		{
			if (val > 0f || this.alsoAppliesToNegativeValues)
			{
				val *= this.QualityMultiplier(req.QualityCategory);
			}
		}

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
