using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000D0B RID: 3339
	public class HediffComp_DrugEffectFactor : HediffComp
	{
		// Token: 0x17000BA5 RID: 2981
		// (get) Token: 0x0600499D RID: 18845 RVA: 0x0026847C File Offset: 0x0026687C
		public HediffCompProperties_DrugEffectFactor Props
		{
			get
			{
				return (HediffCompProperties_DrugEffectFactor)this.props;
			}
		}

		// Token: 0x17000BA6 RID: 2982
		// (get) Token: 0x0600499E RID: 18846 RVA: 0x0026849C File Offset: 0x0026689C
		private float CurrentFactor
		{
			get
			{
				return HediffComp_DrugEffectFactor.EffectFactorSeverityCurve.Evaluate(this.parent.Severity);
			}
		}

		// Token: 0x17000BA7 RID: 2983
		// (get) Token: 0x0600499F RID: 18847 RVA: 0x002684C8 File Offset: 0x002668C8
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

		// Token: 0x060049A0 RID: 18848 RVA: 0x00268513 File Offset: 0x00266913
		public override void CompModifyChemicalEffect(ChemicalDef chem, ref float effect)
		{
			if (this.Props.chemical == chem)
			{
				effect *= this.CurrentFactor;
			}
		}

		// Token: 0x040031E9 RID: 12777
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
