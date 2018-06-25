using System;
using System.Linq;
using System.Text;
using RimWorld;

namespace Verse
{
	// Token: 0x02000D13 RID: 3347
	public class HediffComp_GrowthMode : HediffComp_SeverityPerDay
	{
		// Token: 0x0400320D RID: 12813
		private const int CheckGrowthModeChangeInterval = 5000;

		// Token: 0x0400320E RID: 12814
		private const float GrowthModeChangeMtbDays = 100f;

		// Token: 0x0400320F RID: 12815
		public HediffGrowthMode growthMode = HediffGrowthMode.Growing;

		// Token: 0x04003210 RID: 12816
		private float severityPerDayGrowingRandomFactor = 1f;

		// Token: 0x04003211 RID: 12817
		private float severityPerDayRemissionRandomFactor = 1f;

		// Token: 0x17000BAC RID: 2988
		// (get) Token: 0x060049C5 RID: 18885 RVA: 0x0026A2A0 File Offset: 0x002686A0
		public HediffCompProperties_GrowthMode Props
		{
			get
			{
				return (HediffCompProperties_GrowthMode)this.props;
			}
		}

		// Token: 0x17000BAD RID: 2989
		// (get) Token: 0x060049C6 RID: 18886 RVA: 0x0026A2C0 File Offset: 0x002686C0
		public override string CompLabelInBracketsExtra
		{
			get
			{
				return this.growthMode.GetLabel();
			}
		}

		// Token: 0x060049C7 RID: 18887 RVA: 0x0026A2E0 File Offset: 0x002686E0
		public override void CompExposeData()
		{
			base.CompExposeData();
			Scribe_Values.Look<HediffGrowthMode>(ref this.growthMode, "growthMode", HediffGrowthMode.Growing, false);
			Scribe_Values.Look<float>(ref this.severityPerDayGrowingRandomFactor, "severityPerDayGrowingRandomFactor", 1f, false);
			Scribe_Values.Look<float>(ref this.severityPerDayRemissionRandomFactor, "severityPerDayRemissionRandomFactor", 1f, false);
		}

		// Token: 0x060049C8 RID: 18888 RVA: 0x0026A334 File Offset: 0x00268734
		public override void CompPostPostAdd(DamageInfo? dinfo)
		{
			base.CompPostPostAdd(dinfo);
			this.growthMode = ((HediffGrowthMode[])Enum.GetValues(typeof(HediffGrowthMode))).RandomElement<HediffGrowthMode>();
			this.severityPerDayGrowingRandomFactor = this.Props.severityPerDayGrowingRandomFactor.RandomInRange;
			this.severityPerDayRemissionRandomFactor = this.Props.severityPerDayRemissionRandomFactor.RandomInRange;
		}

		// Token: 0x060049C9 RID: 18889 RVA: 0x0026A394 File Offset: 0x00268794
		public override void CompPostTick(ref float severityAdjustment)
		{
			base.CompPostTick(ref severityAdjustment);
			if (base.Pawn.IsHashIntervalTick(5000) && Rand.MTBEventOccurs(100f, 60000f, 5000f))
			{
				this.ChangeGrowthMode();
			}
		}

		// Token: 0x060049CA RID: 18890 RVA: 0x0026A3D4 File Offset: 0x002687D4
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

		// Token: 0x060049CB RID: 18891 RVA: 0x0026A448 File Offset: 0x00268848
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

		// Token: 0x060049CC RID: 18892 RVA: 0x0026A590 File Offset: 0x00268990
		public override string CompDebugString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(base.CompDebugString());
			stringBuilder.AppendLine("severity: " + this.parent.Severity.ToString("F3") + ((this.parent.Severity < base.Def.maxSeverity) ? "" : " (reached max)"));
			stringBuilder.AppendLine("severityPerDayGrowingRandomFactor: " + this.severityPerDayGrowingRandomFactor.ToString("0.##"));
			stringBuilder.AppendLine("severityPerDayRemissionRandomFactor: " + this.severityPerDayRemissionRandomFactor.ToString("0.##"));
			return stringBuilder.ToString();
		}
	}
}
