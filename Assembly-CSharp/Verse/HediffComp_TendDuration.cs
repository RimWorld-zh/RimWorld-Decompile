using RimWorld;
using System.Text;
using UnityEngine;

namespace Verse
{
	[StaticConstructorOnStartup]
	public class HediffComp_TendDuration : HediffComp_SeverityPerDay
	{
		public int tendTick = -999999;

		public float tendQuality = 0f;

		private int tendedCount = 0;

		private const float TendQualityRandomVariance = 0.25f;

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
				return base.CompShouldRemove || (this.TProps.disappearsAtTendedCount >= 0 && this.tendedCount >= this.TProps.disappearsAtTendedCount);
			}
		}

		public bool IsTended
		{
			get
			{
				return Current.ProgramState == ProgramState.Playing && ((this.TProps.tendDuration <= 0) ? (this.tendTick > 0) : (Find.TickManager.TicksGame <= this.tendTick + this.TProps.tendDuration));
			}
		}

		public override string CompTipStringExtra
		{
			get
			{
				string result;
				if (base.parent.IsOld())
				{
					result = (string)null;
				}
				else
				{
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
					result = stringBuilder.ToString().TrimEndNewlines();
				}
				return result;
			}
		}

		public override TextureAndColor CompStateIcon
		{
			get
			{
				TextureAndColor result;
				if (base.parent is Hediff_Injury)
				{
					if (this.IsTended && !base.parent.IsOld())
					{
						Color color = Color.Lerp(HediffComp_TendDuration.UntendedColor, Color.white, Mathf.Clamp01(this.tendQuality));
						result = new TextureAndColor(HediffComp_TendDuration.TendedIcon_Well_Injury, color);
						goto IL_00dd;
					}
				}
				else if (!(base.parent is Hediff_MissingPart) && !base.parent.FullyImmune())
				{
					if (this.IsTended)
					{
						Color color2 = Color.Lerp(HediffComp_TendDuration.UntendedColor, Color.white, Mathf.Clamp01(this.tendQuality));
						result = new TextureAndColor(HediffComp_TendDuration.TendedIcon_Well_General, color2);
					}
					else
					{
						result = HediffComp_TendDuration.TendedIcon_Need_General;
					}
					goto IL_00dd;
				}
				result = TextureAndColor.None;
				goto IL_00dd;
				IL_00dd:
				return result;
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
			return (float)((!this.IsTended) ? 0.0 : (this.TProps.severityPerDayTended * this.tendQuality));
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
