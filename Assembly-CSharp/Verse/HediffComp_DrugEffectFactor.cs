using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000D0A RID: 3338
	public class HediffComp_DrugEffectFactor : HediffComp
	{
		// Token: 0x17000BA4 RID: 2980
		// (get) Token: 0x0600499B RID: 18843 RVA: 0x00268454 File Offset: 0x00266854
		public HediffCompProperties_DrugEffectFactor Props
		{
			get
			{
				return (HediffCompProperties_DrugEffectFactor)this.props;
			}
		}

		// Token: 0x17000BA5 RID: 2981
		// (get) Token: 0x0600499C RID: 18844 RVA: 0x00268474 File Offset: 0x00266874
		private float CurrentFactor
		{
			get
			{
				return HediffComp_DrugEffectFactor.EffectFactorSeverityCurve.Evaluate(this.parent.Severity);
			}
		}

		// Token: 0x17000BA6 RID: 2982
		// (get) Token: 0x0600499D RID: 18845 RVA: 0x002684A0 File Offset: 0x002668A0
		public override string CompTipStringExtra
		{
			get
			{
				return "DrugEffectMultiplier".Translate(new object[]
				{
					this.Props.chemical.label,
					this.CurrentFactor.ToStringPercent()
				}).CapitalizeFirst();
			}
		}

		// Token: 0x0600499E RID: 18846 RVA: 0x002684EB File Offset: 0x002668EB
		public override void CompModifyChemicalEffect(ChemicalDef chem, ref float effect)
		{
			if (this.Props.chemical == chem)
			{
				effect *= this.CurrentFactor;
			}
		}

		// Token: 0x040031E7 RID: 12775
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
	}
}
