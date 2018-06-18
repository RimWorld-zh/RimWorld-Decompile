using System;
using System.Linq;
using System.Text;
using RimWorld;

namespace Verse
{
	// Token: 0x02000D13 RID: 3347
	public class HediffComp_GrowthMode : HediffComp_SeverityPerDay
	{
		// Token: 0x17000BAB RID: 2987
		// (get) Token: 0x060049B1 RID: 18865 RVA: 0x00268AB0 File Offset: 0x00266EB0
		public HediffCompProperties_GrowthMode Props
		{
			get
			{
				return (HediffCompProperties_GrowthMode)this.props;
			}
		}

		// Token: 0x17000BAC RID: 2988
		// (get) Token: 0x060049B2 RID: 18866 RVA: 0x00268AD0 File Offset: 0x00266ED0
		public override string CompLabelInBracketsExtra
		{
			get
			{
				return this.growthMode.GetLabel();
			}
		}

		// Token: 0x060049B3 RID: 18867 RVA: 0x00268AF0 File Offset: 0x00266EF0
		public override void CompExposeData()
		{
			base.CompExposeData();
			Scribe_Values.Look<HediffGrowthMode>(ref this.growthMode, "growthMode", HediffGrowthMode.Growing, false);
			Scribe_Values.Look<float>(ref this.severityPerDayGrowingRandomFactor, "severityPerDayGrowingRandomFactor", 1f, false);
			Scribe_Values.Look<float>(ref this.severityPerDayRemissionRandomFactor, "severityPerDayRemissionRandomFactor", 1f, false);
		}

		// Token: 0x060049B4 RID: 18868 RVA: 0x00268B44 File Offset: 0x00266F44
		public override void CompPostPostAdd(DamageInfo? dinfo)
		{
			base.CompPostPostAdd(dinfo);
			this.growthMode = ((HediffGrowthMode[])Enum.GetValues(typeof(HediffGrowthMode))).RandomElement<HediffGrowthMode>();
			this.severityPerDayGrowingRandomFactor = this.Props.severityPerDayGrowingRandomFactor.RandomInRange;
			this.severityPerDayRemissionRandomFactor = this.Props.severityPerDayRemissionRandomFactor.RandomInRange;
		}

		// Token: 0x060049B5 RID: 18869 RVA: 0x00268BA4 File Offset: 0x00266FA4
		public override void CompPostTick(ref float severityAdjustment)
		{
			base.CompPostTick(ref severityAdjustment);
			if (base.Pawn.IsHashIntervalTick(5000) && Rand.MTBEventOccurs(100f, 60000f, 5000f))
			{
				this.ChangeGrowthMode();
			}
		}

		// Token: 0x060049B6 RID: 18870 RVA: 0x00268BE4 File Offset: 0x00266FE4
		protected override float SeverityChangePerDay()
		{
			float result;
			switch (this.growthMode)
			{
			case HediffGrowthMode.Growing:
				result = this.Props.severityPerDayGrowing * this.severityPerDayGrowingRandomFactor;
				break;
			case HediffGrowthMode.Stable:
				result = 0f;
				break;
			case HediffGrowthMode.Remission:
				result = this.Props.severityPerDayRemission * this.severityPerDayRemissionRandomFactor;
				break;
			default:
				throw new NotImplementedException("GrowthMode");
			}
			return result;
		}

		// Token: 0x060049B7 RID: 18871 RVA: 0x00268C58 File Offset: 0x00267058
		private void ChangeGrowthMode()
		{
			this.growthMode = (from x in (HediffGrowthMode[])Enum.GetValues(typeof(HediffGrowthMode))
			where x != this.growthMode
			select x).RandomElement<HediffGrowthMode>();
			if (PawnUtility.ShouldSendNotificationAbout(base.Pawn))
			{
				HediffGrowthMode hediffGrowthMode = this.growthMode;
				if (hediffGrowthMode != HediffGrowthMode.Growing)
				{
					if (hediffGrowthMode != HediffGrowthMode.Stable)
					{
						if (hediffGrowthMode == HediffGrowthMode.Remission)
						{
							Messages.Message("DiseaseGrowthModeChanged_Remission".Translate(new object[]
							{
								base.Pawn.LabelShort,
								base.Def.label
							}), base.Pawn, MessageTypeDefOf.PositiveEvent, true);
						}
					}
					else
					{
						Messages.Message("DiseaseGrowthModeChanged_Stable".Translate(new object[]
						{
							base.Pawn.LabelShort,
							base.Def.label
						}), base.Pawn, MessageTypeDefOf.NeutralEvent, true);
					}
				}
				else
				{
					Messages.Message("DiseaseGrowthModeChanged_Growing".Translate(new object[]
					{
						base.Pawn.LabelShort,
						base.Def.label
					}), base.Pawn, MessageTypeDefOf.NegativeHealthEvent, true);
				}
			}
		}

		// Token: 0x060049B8 RID: 18872 RVA: 0x00268DA0 File Offset: 0x002671A0
		public override string CompDebugString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(base.CompDebugString());
			stringBuilder.AppendLine("severity: " + this.parent.Severity.ToString("F3") + ((this.parent.Severity < base.Def.maxSeverity) ? "" : " (reached max)"));
			stringBuilder.AppendLine("severityPerDayGrowingRandomFactor: " + this.severityPerDayGrowingRandomFactor.ToString("0.##"));
			stringBuilder.AppendLine("severityPerDayRemissionRandomFactor: " + this.severityPerDayRemissionRandomFactor.ToString("0.##"));
			return stringBuilder.ToString();
		}

		// Token: 0x040031FB RID: 12795
		private const int CheckGrowthModeChangeInterval = 5000;

		// Token: 0x040031FC RID: 12796
		private const float GrowthModeChangeMtbDays = 100f;

		// Token: 0x040031FD RID: 12797
		public HediffGrowthMode growthMode = HediffGrowthMode.Growing;

		// Token: 0x040031FE RID: 12798
		private float severityPerDayGrowingRandomFactor = 1f;

		// Token: 0x040031FF RID: 12799
		private float severityPerDayRemissionRandomFactor = 1f;
	}
}
