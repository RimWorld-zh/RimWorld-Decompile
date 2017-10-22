using RimWorld;

namespace Verse
{
	public class HediffComp_DrugEffectFactor : HediffComp
	{
		private static readonly SimpleCurve EffectFactorSeverityCurve = new SimpleCurve
		{
			{
				new CurvePoint(0f, 1f),
				true
			},
			{
				new CurvePoint(1f, 0.25f),
				true
			}
		};

		public HediffCompProperties_DrugEffectFactor Props
		{
			get
			{
				return (HediffCompProperties_DrugEffectFactor)base.props;
			}
		}

		private float CurrentFactor
		{
			get
			{
				return HediffComp_DrugEffectFactor.EffectFactorSeverityCurve.Evaluate(base.parent.Severity);
			}
		}

		public override string CompTipStringExtra
		{
			get
			{
				return "DrugEffectMultiplier".Translate(this.Props.chemical.label, this.CurrentFactor.ToStringPercent()).CapitalizeFirst();
			}
		}

		public override void CompModifyChemicalEffect(ChemicalDef chem, ref float effect)
		{
			if (this.Props.chemical == chem)
			{
				effect *= this.CurrentFactor;
			}
		}
	}
}
