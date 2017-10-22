using RimWorld;
using System.Text;
using UnityEngine;

namespace Verse
{
	[StaticConstructorOnStartup]
	public class HediffComp_TendDuration : HediffComp_SeverityPerDay
	{
		private const float TendQualityRandomVariance = 0.25f;

		public int tendTick = -999999;

		public float tendQuality;

		private int tendedCount;

		private static readonly Color UntendedColor = new ColorInt(116, 101, 72).ToColor;

		private static readonly Texture2D TendedIcon_Need_General = ContentFinder<Texture2D>.Get("UI/Icons/Medical/TendedNeed", true);

		private static readonly Texture2D TendedIcon_Well_General = ContentFinder<Texture2D>.Get("UI/Icons/Medical/TendedWell", true);

		private static readonly Texture2D TendedIcon_Well_Injury = ContentFinder<Texture2D>.Get("UI/Icons/Medical/BandageWell", true);

		public HediffCompProperties_TendDuration TProps
		{
			get
			{
				return (HediffCompProperties_TendDuration)base.props;
			}
		}

		public override bool CompShouldRemove
		{
			get
			{
				if (base.CompShouldRemove)
				{
					return true;
				}
				return this.TProps.disappearsAtTendedCount >= 0 && this.tendedCount >= this.TProps.disappearsAtTendedCount;
			}
		}

		public bool IsTended
		{
			get
			{
				if (Current.ProgramState != ProgramState.Playing)
				{
					return false;
				}
				if (this.TProps.tendDuration > 0)
				{
					return Find.TickManager.TicksGame <= this.tendTick + this.TProps.tendDuration;
				}
				return this.tendTick > 0;
			}
		}

		public override string CompTipStringExtra
		{
			get
			{
				if (base.parent.IsOld())
				{
					return (string)null;
				}
				StringBuilder stringBuilder = new StringBuilder();
				if (!this.IsTended)
				{
					if (!base.Pawn.Dead && base.parent.TendableNow)
					{
						stringBuilder.AppendLine("NeedsTendingNow".Translate());
					}
				}
				else
				{
					string text = (string)null;
					text = ((base.parent.Part == null || !base.parent.Part.def.IsSolid(base.parent.Part, base.Pawn.health.hediffSet.hediffs)) ? ((base.parent.Part == null || base.parent.Part.depth != BodyPartDepth.Inside) ? this.TProps.labelTendedWell : this.TProps.labelTendedWellInner) : this.TProps.labelSolidTendedWell);
					if (text != null)
					{
						stringBuilder.AppendLine(text.CapitalizeFirst() + " (" + "Quality".Translate().ToLower() + " " + this.tendQuality.ToStringPercent("F0") + ")");
					}
					if (!base.Pawn.Dead && this.TProps.tendDuration > 0)
					{
						int numTicks = this.tendTick + this.TProps.tendDuration - Find.TickManager.TicksGame;
						string text2 = numTicks.ToStringTicksToPeriod(true, false, true);
						text2 = ((!"NextTendIn".CanTranslate()) ? "NextTreatmentIn".Translate(text2) : "NextTendIn".Translate(text2));
						stringBuilder.AppendLine(text2);
					}
				}
				return stringBuilder.ToString().TrimEndNewlines();
			}
		}

		public override TextureAndColor CompStateIcon
		{
			get
			{
				if (base.parent is Hediff_Injury)
				{
					if (this.IsTended && !base.parent.IsOld())
					{
						Color color = Color.Lerp(HediffComp_TendDuration.UntendedColor, Color.white, Mathf.Clamp01(this.tendQuality));
						return new TextureAndColor(HediffComp_TendDuration.TendedIcon_Well_Injury, color);
					}
				}
				else if (!(base.parent is Hediff_MissingPart) && !base.parent.FullyImmune())
				{
					if (this.IsTended)
					{
						Color color2 = Color.Lerp(HediffComp_TendDuration.UntendedColor, Color.white, Mathf.Clamp01(this.tendQuality));
						return new TextureAndColor(HediffComp_TendDuration.TendedIcon_Well_General, color2);
					}
					return HediffComp_TendDuration.TendedIcon_Need_General;
				}
				return TextureAndColor.None;
			}
		}

		public override void CompExposeData()
		{
			Scribe_Values.Look<int>(ref this.tendTick, "tendTick", -999999, false);
			Scribe_Values.Look<float>(ref this.tendQuality, "tendQuality", 0f, false);
			Scribe_Values.Look<int>(ref this.tendedCount, "tendedCount", 0, false);
		}

		protected override float SeverityChangePerDay()
		{
			if (this.IsTended)
			{
				return this.TProps.severityPerDayTended * this.tendQuality;
			}
			return 0f;
		}

		public override void CompTended(float quality, int batchPosition = 0)
		{
			this.tendQuality = Mathf.Clamp01(quality + Rand.Range(-0.25f, 0.25f));
			this.tendTick = Find.TickManager.TicksGame;
			this.tendedCount++;
			if (batchPosition == 0 && base.Pawn.Spawned)
			{
				string text = "TextMote_Tended".Translate(base.parent.Label).CapitalizeFirst() + "\n" + "Quality".Translate() + " " + this.tendQuality.ToStringPercent();
				MoteMaker.ThrowText(base.Pawn.DrawPos, base.Pawn.Map, text, Color.white, 3.65f);
			}
			base.Pawn.health.Notify_HediffChanged(base.parent);
		}

		public override string CompDebugString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (this.IsTended)
			{
				stringBuilder.AppendLine("tendQuality: " + this.tendQuality.ToStringPercent());
				if (this.TProps.tendDuration > 0)
				{
					int num = Find.TickManager.TicksGame - this.tendTick;
					stringBuilder.AppendLine("ticks since tend: " + num);
					stringBuilder.AppendLine("tend duration passed: " + ((float)num / (float)this.TProps.tendDuration).ToStringPercent());
					stringBuilder.AppendLine("severity change per day: " + (this.TProps.severityPerDayTended * this.tendQuality).ToString());
				}
			}
			else
			{
				stringBuilder.AppendLine("untended");
			}
			if (this.TProps.disappearsAtTendedCount >= 0)
			{
				stringBuilder.AppendLine("tended count: " + this.tendedCount + " / " + this.TProps.disappearsAtTendedCount);
			}
			return stringBuilder.ToString().Trim();
		}
	}
}
