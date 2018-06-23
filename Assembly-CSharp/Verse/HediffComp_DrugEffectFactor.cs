using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000D07 RID: 3335
	public class HediffComp_DrugEffectFactor : HediffComp
	{
		// Token: 0x040031F2 RID: 12786
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

		// Token: 0x17000BA6 RID: 2982
		// (get) Token: 0x060049AC RID: 18860 RVA: 0x00269888 File Offset: 0x00267C88
		public HediffCompProperties_DrugEffectFactor Props
		{
			get
			{
				return (HediffCompProperties_DrugEffectFactor)this.props;
			}
		}

		// Token: 0x17000BA7 RID: 2983
		// (get) Token: 0x060049AD RID: 18861 RVA: 0x002698A8 File Offset: 0x00267CA8
		private float CurrentFactor
		{
			get
			{
				return HediffComp_DrugEffectFactor.EffectFactorSeverityCurve.Evaluate(this.parent.Severity);
			}
		}

		// Token: 0x17000BA8 RID: 2984
		// (get) Token: 0x060049AE RID: 18862 RVA: 0x002698D4 File Offset: 0x00267CD4
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

		// Token: 0x060049AF RID: 18863 RVA: 0x0026991F File Offset: 0x00267D1F
		public override void CompModifyChemicalEffect(ChemicalDef chem, ref float effect)
		{
			if (this.Props.chemical == chem)
			{
				effect *= this.CurrentFactor;
			}
		}
	}
}
