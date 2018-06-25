using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000D0A RID: 3338
	public class HediffComp_DrugEffectFactor : HediffComp
	{
		// Token: 0x040031F9 RID: 12793
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

		// Token: 0x17000BA5 RID: 2981
		// (get) Token: 0x060049AF RID: 18863 RVA: 0x00269C44 File Offset: 0x00268044
		public HediffCompProperties_DrugEffectFactor Props
		{
			get
			{
				return (HediffCompProperties_DrugEffectFactor)this.props;
			}
		}

		// Token: 0x17000BA6 RID: 2982
		// (get) Token: 0x060049B0 RID: 18864 RVA: 0x00269C64 File Offset: 0x00268064
		private float CurrentFactor
		{
			get
			{
				return HediffComp_DrugEffectFactor.EffectFactorSeverityCurve.Evaluate(this.parent.Severity);
			}
		}

		// Token: 0x17000BA7 RID: 2983
		// (get) Token: 0x060049B1 RID: 18865 RVA: 0x00269C90 File Offset: 0x00268090
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

		// Token: 0x060049B2 RID: 18866 RVA: 0x00269CDB File Offset: 0x002680DB
		public override void CompModifyChemicalEffect(ChemicalDef chem, ref float effect)
		{
			if (this.Props.chemical == chem)
			{
				effect *= this.CurrentFactor;
			}
		}
	}
}
