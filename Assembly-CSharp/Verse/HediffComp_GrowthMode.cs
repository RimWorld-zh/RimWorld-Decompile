using System;
using System.Linq;
using System.Text;
using RimWorld;

namespace Verse
{
	// Token: 0x02000D14 RID: 3348
	public class HediffComp_GrowthMode : HediffComp_SeverityPerDay
	{
		// Token: 0x17000BAC RID: 2988
		// (get) Token: 0x060049B3 RID: 18867 RVA: 0x00268AD8 File Offset: 0x00266ED8
		public HediffCompProperties_GrowthMode Props
		{
			get
			{
				return (HediffCompProperties_GrowthMode)this.props;
			}
		}

		// Token: 0x17000BAD RID: 2989
		// (get) Token: 0x060049B4 RID: 18868 RVA: 0x00268AF8 File Offset: 0x00266EF8
		public override string CompLabelInBracketsExtra
		{
			get
			{
				return this.growthMode.GetLabel();
			}
		}

		// Token: 0x060049B5 RID: 18869 RVA: 0x00268B18 File Offset: 0x00266F18
		public override void CompExposeData()
		{
			base.CompExposeData();
			Scribe_Values.Look<HediffGrowthMode>(ref this.growthMode, "growthMode", HediffGrowthMode.Growing, false);
			Scribe_Values.Look<float>(ref this.severityPerDayGrowingRandomFactor, "severityPerDayGrowingRandomFactor", 1f, false);
			Scribe_Values.Look<float>(ref this.severityPerDayRemissionRandomFactor, "severityPerDayRemissionRandomFactor", 1f, false);
		}

		// Token: 0x060049B6 RID: 18870 RVA: 0x00268B6C File Offset: 0x00266F6C
		public override void CompPostPostAdd(DamageInfo? dinfo)
		{
			base.CompPostPostAdd(dinfo);
			this.growthMode = ((HediffGrowthMode[])Enum.GetValues(typeof(HediffGrowthMode))).RandomElement<HediffGrowthMode>();
			this.severityPerDayGrowingRandomFactor = this.Props.severityPerDayGrowingRandomFactor.RandomInRange;
			this.severityPerDayRemissionRandomFactor = this.Props.severityPerDayRemissionRandomFactor.RandomInRange;
		}

		// Token: 0x060049B7 RID: 18871 RVA: 0x00268BCC File Offset: 0x00266FCC
		public override void CompPostTick(ref float severityAdjustment)
		{
			base.CompPostTick(ref severityAdjustment);
			if (base.Pawn.IsHashIntervalTick(5000) && Rand.MTBEventOccurs(100f, 60000f, 5000f))
			{
				this.ChangeGrowthMode();
			}
		}

		// Token: 0x060049B8 RID: 18872 RVA: 0x00268C0C File Offset: 0x0026700C
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

		// Token: 0x060049B9 RID: 18873 RVA: 0x00268C80 File Offset: 0x00267080
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

		// Token: 0x060049BA RID: 18874 RVA: 0x00268DC8 File Offset: 0x002671C8
		public override string CompDebugString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(base.CompDebugString());
			stringBuilder.AppendLine("severity: " + this.parent.Severity.ToString("F3") + ((this.parent.Severity < base.Def.maxSeverity) ? "" : " (reached max)"));
			stringBuilder.AppendLine("severityPerDayGrowingRandomFactor: " + this.severityPerDayGrowingRandomFactor.ToString("0.##"));
			stringBuilder.AppendLine("severityPerDayRemissionRandomFactor: " + this.severityPerDayRemissionRandomFactor.ToString("0.##"));
			return stringBuilder.ToString();
		}

		// Token: 0x040031FD RID: 12797
		private const int CheckGrowthModeChangeInterval = 5000;

		// Token: 0x040031FE RID: 12798
		private const float GrowthModeChangeMtbDays = 100f;

		// Token: 0x040031FF RID: 12799
		public HediffGrowthMode growthMode = HediffGrowthMode.Growing;

		// Token: 0x04003200 RID: 12800
		private float severityPerDayGrowingRandomFactor = 1f;

		// Token: 0x04003201 RID: 12801
		private float severityPerDayRemissionRandomFactor = 1f;
	}
}
