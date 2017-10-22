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

		private bool alsoAppliesToNegativeValues;

		public override void TransformValue(StatRequest req, ref float val)
		{
			if (val <= 0.0 && !this.alsoAppliesToNegativeValues)
				return;
			val *= this.QualityMultiplier(req.QualityCategory);
		}

		public override string ExplanationPart(StatRequest req)
		{
			if (req.HasThing && !this.alsoAppliesToNegativeValues && req.Thing.GetStatValue(base.parentStat, true) <= 0.0)
			{
				return (string)null;
			}
			QualityCategory qc = default(QualityCategory);
			if (req.HasThing && req.Thing.TryGetQuality(out qc))
			{
				return "StatsReport_QualityMultiplier".Translate() + ": x" + this.QualityMultiplier(qc).ToStringPercent();
			}
			return (string)null;
		}

		private float QualityMultiplier(QualityCategory qc)
		{
			switch (qc)
			{
			case QualityCategory.Awful:
			{
				return this.factorAwful;
			}
			case QualityCategory.Shoddy:
			{
				return this.factorShoddy;
			}
			case QualityCategory.Poor:
			{
				return this.factorPoor;
			}
			case QualityCategory.Normal:
			{
				return this.factorNormal;
			}
			case QualityCategory.Good:
			{
				return this.factorGood;
			}
			case QualityCategory.Superior:
			{
				return this.factorSuperior;
			}
			case QualityCategory.Excellent:
			{
				return this.factorExcellent;
			}
			case QualityCategory.Masterwork:
			{
				return this.factorMasterwork;
			}
			case QualityCategory.Legendary:
			{
				return this.factorLegendary;
			}
			default:
			{
				throw new ArgumentOutOfRangeException();
			}
			}
		}
	}
}
