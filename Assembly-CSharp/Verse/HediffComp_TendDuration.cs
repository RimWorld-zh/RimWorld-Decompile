using System;
using System.Text;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000D1C RID: 3356
	[StaticConstructorOnStartup]
	public class HediffComp_TendDuration : HediffComp_SeverityPerDay
	{
		// Token: 0x04003222 RID: 12834
		public int tendTick = -999999;

		// Token: 0x04003223 RID: 12835
		public float tendQuality = 0f;

		// Token: 0x04003224 RID: 12836
		private float totalTendQuality = 0f;

		// Token: 0x04003225 RID: 12837
		public const float TendQualityRandomVariance = 0.25f;

		// Token: 0x04003226 RID: 12838
		private static readonly Color UntendedColor;

		// Token: 0x04003227 RID: 12839
		private static readonly Texture2D TendedIcon_Need_General;

		// Token: 0x04003228 RID: 12840
		private static readonly Texture2D TendedIcon_Well_General;

		// Token: 0x04003229 RID: 12841
		private static readonly Texture2D TendedIcon_Well_Injury;

		// Token: 0x17000BB2 RID: 2994
		// (get) Token: 0x060049ED RID: 18925 RVA: 0x0026AB50 File Offset: 0x00268F50
		public HediffCompProperties_TendDuration TProps
		{
			get
			{
				return (HediffCompProperties_TendDuration)this.props;
			}
		}

		// Token: 0x17000BB3 RID: 2995
		// (get) Token: 0x060049EE RID: 18926 RVA: 0x0026AB70 File Offset: 0x00268F70
		private int FullTendDurationTicks
		{
			get
			{
				return Mathf.RoundToInt((this.TProps.baseTendDurationHours + this.TProps.tendOverlapHours) * 2500f);
			}
		}

		// Token: 0x17000BB4 RID: 2996
		// (get) Token: 0x060049EF RID: 18927 RVA: 0x0026ABA8 File Offset: 0x00268FA8
		private int BaseTendDurationTicks
		{
			get
			{
				return Mathf.RoundToInt(this.TProps.baseTendDurationHours * 2500f);
			}
		}

		// Token: 0x17000BB5 RID: 2997
		// (get) Token: 0x060049F0 RID: 18928 RVA: 0x0026ABD4 File Offset: 0x00268FD4
		public override bool CompShouldRemove
		{
			get
			{
				return base.CompShouldRemove || (this.TProps.disappearsAtTotalTendQuality >= 0 && this.totalTendQuality >= (float)this.TProps.disappearsAtTotalTendQuality);
			}
		}

		// Token: 0x17000BB6 RID: 2998
		// (get) Token: 0x060049F1 RID: 18929 RVA: 0x0026AC28 File Offset: 0x00269028
		public bool IsTended
		{
			get
			{
				bool result;
				if (Current.ProgramState != ProgramState.Playing)
				{
					result = false;
				}
				else if (this.TProps.baseTendDurationHours > 0f)
				{
					result = (Find.TickManager.TicksGame <= this.tendTick + this.FullTendDurationTicks);
				}
				else
				{
					result = (this.tendTick > 0);
				}
				return result;
			}
		}

		// Token: 0x17000BB7 RID: 2999
		// (get) Token: 0x060049F2 RID: 18930 RVA: 0x0026AC90 File Offset: 0x00269090
		public bool AllowTend
		{
			get
			{
				bool result;
				if (this.TProps.baseTendDurationHours > 0f)
				{
					result = (Find.TickManager.TicksGame > this.tendTick + this.BaseTendDurationTicks);
				}
				else
				{
					result = !this.IsTended;
				}
				return result;
			}
		}

		// Token: 0x17000BB8 RID: 3000
		// (get) Token: 0x060049F3 RID: 18931 RVA: 0x0026ACE4 File Offset: 0x002690E4
		public override string CompTipStringExtra
		{
			get
			{
				string result;
				if (this.parent.IsPermanent())
				{
					result = null;
				}
				else
				{
					StringBuilder stringBuilder = new StringBuilder();
					if (!this.IsTended)
					{
						if (!base.Pawn.Dead && this.parent.TendableNow(false))
						{
							stringBuilder.AppendLine("NeedsTendingNow".Translate());
						}
					}
					else
					{
						if (this.TProps.showTendQuality)
						{
							string text;
							if (this.parent.Part != null && this.parent.Part.def.IsSolid(this.parent.Part, base.Pawn.health.hediffSet.hediffs))
							{
								text = this.TProps.labelSolidTendedWell;
							}
							else if (this.parent.Part != null && this.parent.Part.depth == BodyPartDepth.Inside)
							{
								text = this.TProps.labelTendedWellInner;
							}
							else
							{
								text = this.TProps.labelTendedWell;
							}
							if (text != null)
							{
								stringBuilder.AppendLine(string.Concat(new string[]
								{
									text.CapitalizeFirst(),
									" (",
									"Quality".Translate().ToLower(),
									" ",
									this.tendQuality.ToStringPercent("F0"),
									")"
								}));
							}
							else
							{
								stringBuilder.AppendLine(string.Format("{0}: {1}", "TendQuality".Translate(), this.tendQuality.ToStringPercent()));
							}
						}
						if (!base.Pawn.Dead && this.TProps.baseTendDurationHours > 0f && this.parent.TendableNow(true))
						{
							int num = this.tendTick + this.BaseTendDurationTicks - Find.TickManager.TicksGame;
							int numTicks = this.tendTick + this.FullTendDurationTicks - Find.TickManager.TicksGame;
							if (num < 0)
							{
								stringBuilder.AppendLine("CanTendNow".Translate());
							}
							else if ("NextTendIn".CanTranslate())
							{
								stringBuilder.AppendLine("NextTendIn".Translate(new object[]
								{
									num.ToStringTicksToPeriod()
								}));
							}
							else
							{
								stringBuilder.AppendLine("NextTreatmentIn".Translate(new object[]
								{
									num.ToStringTicksToPeriod()
								}));
							}
							stringBuilder.AppendLine("TreatmentExpiresIn".Translate(new object[]
							{
								numTicks.ToStringTicksToPeriod()
							}));
						}
					}
					result = stringBuilder.ToString().TrimEndNewlines();
				}
				return result;
			}
		}

		// Token: 0x17000BB9 RID: 3001
		// (get) Token: 0x060049F4 RID: 18932 RVA: 0x0026AFA0 File Offset: 0x002693A0
		public override TextureAndColor CompStateIcon
		{
			get
			{
				if (this.parent is Hediff_Injury)
				{
					if (this.IsTended && !this.parent.IsPermanent())
					{
						Color color = Color.Lerp(HediffComp_TendDuration.UntendedColor, Color.white, Mathf.Clamp01(this.tendQuality));
						return new TextureAndColor(HediffComp_TendDuration.TendedIcon_Well_Injury, color);
					}
				}
				else if (!(this.parent is Hediff_MissingPart))
				{
					if (!this.parent.FullyImmune())
					{
						if (this.IsTended)
						{
							Color color2 = Color.Lerp(HediffComp_TendDuration.UntendedColor, Color.white, Mathf.Clamp01(this.tendQuality));
							return new TextureAndColor(HediffComp_TendDuration.TendedIcon_Well_General, color2);
						}
						return HediffComp_TendDuration.TendedIcon_Need_General;
					}
				}
				return TextureAndColor.None;
			}
		}

		// Token: 0x060049F5 RID: 18933 RVA: 0x0026B08C File Offset: 0x0026948C
		public override void CompExposeData()
		{
			Scribe_Values.Look<int>(ref this.tendTick, "tendTick", -999999, false);
			Scribe_Values.Look<float>(ref this.tendQuality, "tendQuality", 0f, false);
			Scribe_Values.Look<float>(ref this.totalTendQuality, "totalTendQuality", 0f, false);
		}

		// Token: 0x060049F6 RID: 18934 RVA: 0x0026B0DC File Offset: 0x002694DC
		protected override float SeverityChangePerDay()
		{
			float result;
			if (this.IsTended)
			{
				result = this.TProps.severityPerDayTended * this.tendQuality;
			}
			else
			{
				result = 0f;
			}
			return result;
		}

		// Token: 0x060049F7 RID: 18935 RVA: 0x0026B11C File Offset: 0x0026951C
		public override void CompTended(float quality, int batchPosition = 0)
		{
			this.tendQuality = Mathf.Clamp01(quality + Rand.Range(-0.25f, 0.25f));
			this.tendTick = Find.TickManager.TicksGame;
			this.totalTendQuality += this.tendQuality;
			if (batchPosition == 0 && base.Pawn.Spawned)
			{
				string text = string.Concat(new string[]
				{
					"TextMote_Tended".Translate(new object[]
					{
						this.parent.Label
					}).CapitalizeFirst(),
					"\n",
					"Quality".Translate(),
					" ",
					this.tendQuality.ToStringPercent()
				});
				MoteMaker.ThrowText(base.Pawn.DrawPos, base.Pawn.Map, text, Color.white, 3.65f);
			}
			base.Pawn.health.Notify_HediffChanged(this.parent);
		}

		// Token: 0x060049F8 RID: 18936 RVA: 0x0026B21C File Offset: 0x0026961C
		public override string CompDebugString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (this.IsTended)
			{
				stringBuilder.AppendLine("tendQuality: " + this.tendQuality.ToStringPercent());
				if (this.TProps.baseTendDurationHours > 0f)
				{
					int num = Find.TickManager.TicksGame - this.tendTick;
					stringBuilder.AppendLine("ticks since tend: " + num);
					stringBuilder.AppendLine("full tend duration passed: " + ((float)num / (float)this.FullTendDurationTicks).ToStringPercent());
					stringBuilder.AppendLine("severity change per day: " + (this.TProps.severityPerDayTended * this.tendQuality).ToString());
				}
			}
			else
			{
				stringBuilder.AppendLine("untended");
			}
			if (this.TProps.disappearsAtTotalTendQuality >= 0)
			{
				stringBuilder.AppendLine(string.Concat(new object[]
				{
					"total tend quality: ",
					this.totalTendQuality.ToString("F2"),
					" / ",
					this.TProps.disappearsAtTotalTendQuality
				}));
			}
			return stringBuilder.ToString().Trim();
		}

		// Token: 0x060049F9 RID: 18937 RVA: 0x0026B368 File Offset: 0x00269768
		// Note: this type is marked as 'beforefieldinit'.
		static HediffComp_TendDuration()
		{
			ColorInt colorInt = new ColorInt(116, 101, 72);
			HediffComp_TendDuration.UntendedColor = colorInt.ToColor;
			HediffComp_TendDuration.TendedIcon_Need_General = ContentFinder<Texture2D>.Get("UI/Icons/Medical/TendedNeed", true);
			HediffComp_TendDuration.TendedIcon_Well_General = ContentFinder<Texture2D>.Get("UI/Icons/Medical/TendedWell", true);
			HediffComp_TendDuration.TendedIcon_Well_Injury = ContentFinder<Texture2D>.Get("UI/Icons/Medical/BandageWell", true);
		}
	}
}
