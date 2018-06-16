using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000527 RID: 1319
	public class SkillRecord : IExposable
	{
		// Token: 0x0600180B RID: 6155 RVA: 0x000D1E55 File Offset: 0x000D0255
		public SkillRecord()
		{
		}

		// Token: 0x0600180C RID: 6156 RVA: 0x000D1E73 File Offset: 0x000D0273
		public SkillRecord(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x0600180D RID: 6157 RVA: 0x000D1E98 File Offset: 0x000D0298
		public SkillRecord(Pawn pawn, SkillDef def)
		{
			this.pawn = pawn;
			this.def = def;
		}

		// Token: 0x17000355 RID: 853
		// (get) Token: 0x0600180E RID: 6158 RVA: 0x000D1EC4 File Offset: 0x000D02C4
		// (set) Token: 0x0600180F RID: 6159 RVA: 0x000D1EF1 File Offset: 0x000D02F1
		public int Level
		{
			get
			{
				int result;
				if (this.TotallyDisabled)
				{
					result = 0;
				}
				else
				{
					result = this.levelInt;
				}
				return result;
			}
			set
			{
				this.levelInt = Mathf.Clamp(value, 0, 20);
			}
		}

		// Token: 0x17000356 RID: 854
		// (get) Token: 0x06001810 RID: 6160 RVA: 0x000D1F04 File Offset: 0x000D0304
		public float XpRequiredForLevelUp
		{
			get
			{
				return SkillRecord.XpRequiredToLevelUpFrom(this.levelInt);
			}
		}

		// Token: 0x17000357 RID: 855
		// (get) Token: 0x06001811 RID: 6161 RVA: 0x000D1F24 File Offset: 0x000D0324
		public float XpProgressPercent
		{
			get
			{
				return this.xpSinceLastLevel / this.XpRequiredForLevelUp;
			}
		}

		// Token: 0x17000358 RID: 856
		// (get) Token: 0x06001812 RID: 6162 RVA: 0x000D1F48 File Offset: 0x000D0348
		public float XpTotalEarned
		{
			get
			{
				float num = 0f;
				for (int i = 0; i < this.levelInt; i++)
				{
					num += SkillRecord.XpRequiredToLevelUpFrom(i);
				}
				return num;
			}
		}

		// Token: 0x17000359 RID: 857
		// (get) Token: 0x06001813 RID: 6163 RVA: 0x000D1F88 File Offset: 0x000D0388
		public bool TotallyDisabled
		{
			get
			{
				if (this.cachedTotallyDisabled == BoolUnknown.Unknown)
				{
					this.cachedTotallyDisabled = ((!this.CalculateTotallyDisabled()) ? BoolUnknown.False : BoolUnknown.True);
				}
				return this.cachedTotallyDisabled == BoolUnknown.True;
			}
		}

		// Token: 0x1700035A RID: 858
		// (get) Token: 0x06001814 RID: 6164 RVA: 0x000D1FCC File Offset: 0x000D03CC
		public string LevelDescriptor
		{
			get
			{
				string result;
				switch (this.levelInt)
				{
				case 0:
					result = "Skill0".Translate();
					break;
				case 1:
					result = "Skill1".Translate();
					break;
				case 2:
					result = "Skill2".Translate();
					break;
				case 3:
					result = "Skill3".Translate();
					break;
				case 4:
					result = "Skill4".Translate();
					break;
				case 5:
					result = "Skill5".Translate();
					break;
				case 6:
					result = "Skill6".Translate();
					break;
				case 7:
					result = "Skill7".Translate();
					break;
				case 8:
					result = "Skill8".Translate();
					break;
				case 9:
					result = "Skill9".Translate();
					break;
				case 10:
					result = "Skill10".Translate();
					break;
				case 11:
					result = "Skill11".Translate();
					break;
				case 12:
					result = "Skill12".Translate();
					break;
				case 13:
					result = "Skill13".Translate();
					break;
				case 14:
					result = "Skill14".Translate();
					break;
				case 15:
					result = "Skill15".Translate();
					break;
				case 16:
					result = "Skill16".Translate();
					break;
				case 17:
					result = "Skill17".Translate();
					break;
				case 18:
					result = "Skill18".Translate();
					break;
				case 19:
					result = "Skill19".Translate();
					break;
				case 20:
					result = "Skill20".Translate();
					break;
				default:
					result = "Unknown";
					break;
				}
				return result;
			}
		}

		// Token: 0x1700035B RID: 859
		// (get) Token: 0x06001815 RID: 6165 RVA: 0x000D219C File Offset: 0x000D059C
		public bool LearningSaturatedToday
		{
			get
			{
				return this.xpSinceMidnight > 4000f;
			}
		}

		// Token: 0x06001816 RID: 6166 RVA: 0x000D21C0 File Offset: 0x000D05C0
		public void ExposeData()
		{
			Scribe_Defs.Look<SkillDef>(ref this.def, "def");
			Scribe_Values.Look<int>(ref this.levelInt, "level", 0, false);
			Scribe_Values.Look<float>(ref this.xpSinceLastLevel, "xpSinceLastLevel", 0f, false);
			Scribe_Values.Look<Passion>(ref this.passion, "passion", Passion.None, false);
			Scribe_Values.Look<float>(ref this.xpSinceMidnight, "xpSinceMidnight", 0f, false);
		}

		// Token: 0x06001817 RID: 6167 RVA: 0x000D2230 File Offset: 0x000D0630
		public void Interval()
		{
			float num = (!this.pawn.story.traits.HasTrait(TraitDefOf.GreatMemory)) ? 1f : 0.5f;
			switch (this.levelInt)
			{
			case 10:
				this.Learn(-0.1f * num, false);
				break;
			case 11:
				this.Learn(-0.2f * num, false);
				break;
			case 12:
				this.Learn(-0.4f * num, false);
				break;
			case 13:
				this.Learn(-0.6f * num, false);
				break;
			case 14:
				this.Learn(-1f * num, false);
				break;
			case 15:
				this.Learn(-1.8f * num, false);
				break;
			case 16:
				this.Learn(-2.8f * num, false);
				break;
			case 17:
				this.Learn(-4f * num, false);
				break;
			case 18:
				this.Learn(-6f * num, false);
				break;
			case 19:
				this.Learn(-8f * num, false);
				break;
			case 20:
				this.Learn(-12f * num, false);
				break;
			}
		}

		// Token: 0x06001818 RID: 6168 RVA: 0x000D2380 File Offset: 0x000D0780
		public static float XpRequiredToLevelUpFrom(int startingLevel)
		{
			return SkillRecord.XpForLevelUpCurve.Evaluate((float)startingLevel);
		}

		// Token: 0x06001819 RID: 6169 RVA: 0x000D23A4 File Offset: 0x000D07A4
		public void Learn(float xp, bool direct = false)
		{
			if (!this.TotallyDisabled)
			{
				if (xp >= 0f || this.levelInt != 0)
				{
					if (xp > 0f)
					{
						xp *= this.LearnRateFactor(direct);
					}
					this.xpSinceLastLevel += xp;
					if (!direct)
					{
						this.xpSinceMidnight += xp;
					}
					if (this.levelInt == 20 && this.xpSinceLastLevel > this.XpRequiredForLevelUp - 1f)
					{
						this.xpSinceLastLevel = this.XpRequiredForLevelUp - 1f;
					}
					while (this.xpSinceLastLevel >= this.XpRequiredForLevelUp)
					{
						this.xpSinceLastLevel -= this.XpRequiredForLevelUp;
						this.levelInt++;
						if (this.levelInt == 14)
						{
							if (this.passion == Passion.None)
							{
								TaleRecorder.RecordTale(TaleDefOf.GainedMasterSkillWithoutPassion, new object[]
								{
									this.pawn,
									this.def
								});
							}
							else
							{
								TaleRecorder.RecordTale(TaleDefOf.GainedMasterSkillWithPassion, new object[]
								{
									this.pawn,
									this.def
								});
							}
						}
						if (this.levelInt >= 20)
						{
							this.levelInt = 20;
							this.xpSinceLastLevel = Mathf.Clamp(this.xpSinceLastLevel, 0f, this.XpRequiredForLevelUp - 1f);
							break;
						}
					}
					while (this.xpSinceLastLevel < 0f)
					{
						this.levelInt--;
						this.xpSinceLastLevel += this.XpRequiredForLevelUp;
						if (this.levelInt <= 0)
						{
							this.levelInt = 0;
							this.xpSinceLastLevel = 0f;
							break;
						}
					}
				}
			}
		}

		// Token: 0x0600181A RID: 6170 RVA: 0x000D2580 File Offset: 0x000D0980
		public float LearnRateFactor(bool direct = false)
		{
			float result;
			if (DebugSettings.fastLearning)
			{
				result = 200f;
			}
			else
			{
				float num;
				switch (this.passion)
				{
				case Passion.None:
					num = 0.35f;
					break;
				case Passion.Minor:
					num = 1f;
					break;
				case Passion.Major:
					num = 1.5f;
					break;
				default:
					throw new NotImplementedException("Passion level " + this.passion);
				}
				if (!direct)
				{
					num *= this.pawn.GetStatValue(StatDefOf.GlobalLearningFactor, true);
					if (this.LearningSaturatedToday)
					{
						num *= 0.2f;
					}
				}
				result = num;
			}
			return result;
		}

		// Token: 0x0600181B RID: 6171 RVA: 0x000D2634 File Offset: 0x000D0A34
		public void EnsureMinLevelWithMargin(int minLevel)
		{
			if (!this.TotallyDisabled)
			{
				if (this.Level < minLevel || (this.Level == minLevel && this.xpSinceLastLevel < this.XpRequiredForLevelUp / 2f))
				{
					this.Level = minLevel;
					this.xpSinceLastLevel = this.XpRequiredForLevelUp / 2f;
				}
			}
		}

		// Token: 0x0600181C RID: 6172 RVA: 0x000D269C File Offset: 0x000D0A9C
		public void Notify_SkillDisablesChanged()
		{
			this.cachedTotallyDisabled = BoolUnknown.Unknown;
		}

		// Token: 0x0600181D RID: 6173 RVA: 0x000D26A8 File Offset: 0x000D0AA8
		private bool CalculateTotallyDisabled()
		{
			return this.def.IsDisabled(this.pawn.story.CombinedDisabledWorkTags, this.pawn.story.DisabledWorkTypes);
		}

		// Token: 0x0600181E RID: 6174 RVA: 0x000D26E8 File Offset: 0x000D0AE8
		public override string ToString()
		{
			return string.Concat(new object[]
			{
				this.def.defName,
				": ",
				this.levelInt,
				" (",
				this.xpSinceLastLevel,
				"xp)"
			});
		}

		// Token: 0x04000E46 RID: 3654
		private Pawn pawn;

		// Token: 0x04000E47 RID: 3655
		public SkillDef def;

		// Token: 0x04000E48 RID: 3656
		public int levelInt = 0;

		// Token: 0x04000E49 RID: 3657
		public Passion passion = Passion.None;

		// Token: 0x04000E4A RID: 3658
		public float xpSinceLastLevel;

		// Token: 0x04000E4B RID: 3659
		public float xpSinceMidnight;

		// Token: 0x04000E4C RID: 3660
		private BoolUnknown cachedTotallyDisabled = BoolUnknown.Unknown;

		// Token: 0x04000E4D RID: 3661
		public const int IntervalTicks = 200;

		// Token: 0x04000E4E RID: 3662
		public const int MinLevel = 0;

		// Token: 0x04000E4F RID: 3663
		public const int MaxLevel = 20;

		// Token: 0x04000E50 RID: 3664
		public const int MaxFullRateXpPerDay = 4000;

		// Token: 0x04000E51 RID: 3665
		public const int MasterSkillThreshold = 14;

		// Token: 0x04000E52 RID: 3666
		public const float SaturatedLearningFactor = 0.2f;

		// Token: 0x04000E53 RID: 3667
		public const float LearnFactorPassionNone = 0.35f;

		// Token: 0x04000E54 RID: 3668
		public const float LearnFactorPassionMinor = 1f;

		// Token: 0x04000E55 RID: 3669
		public const float LearnFactorPassionMajor = 1.5f;

		// Token: 0x04000E56 RID: 3670
		private static readonly SimpleCurve XpForLevelUpCurve = new SimpleCurve
		{
			{
				new CurvePoint(0f, 1000f),
				true
			},
			{
				new CurvePoint(9f, 10000f),
				true
			},
			{
				new CurvePoint(19f, 30000f),
				true
			}
		};
	}
}
