using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public class SkillRecord : IExposable
	{
		private Pawn pawn;

		public SkillDef def;

		public int levelInt = 0;

		public Passion passion = Passion.None;

		public float xpSinceLastLevel;

		public float xpSinceMidnight;

		private BoolUnknown cachedTotallyDisabled = BoolUnknown.Unknown;

		public const int IntervalTicks = 200;

		public const int MinLevel = 0;

		public const int MaxLevel = 20;

		public const int MaxFullRateXpPerDay = 4000;

		public const int MasterSkillThreshold = 14;

		public const float SaturatedLearningFactor = 0.2f;

		public const float LearnFactorPassionNone = 0.35f;

		public const float LearnFactorPassionMinor = 1f;

		public const float LearnFactorPassionMajor = 1.5f;

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

		public int Level
		{
			get
			{
				return (!this.TotallyDisabled) ? this.levelInt : 0;
			}
			set
			{
				this.levelInt = value;
			}
		}

		public float XpRequiredForLevelUp
		{
			get
			{
				return SkillRecord.XpRequiredToLevelUpFrom(this.levelInt);
			}
		}

		public float XpProgressPercent
		{
			get
			{
				return this.xpSinceLastLevel / this.XpRequiredForLevelUp;
			}
		}

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

		public bool TotallyDisabled
		{
			get
			{
				if (this.cachedTotallyDisabled == BoolUnknown.Unknown)
				{
					this.cachedTotallyDisabled = (BoolUnknown)((!this.CalculateTotallyDisabled()) ? 1 : 0);
				}
				return this.cachedTotallyDisabled == BoolUnknown.True;
			}
		}

		public string LevelDescriptor
		{
			get
			{
				string result;
				switch (this.levelInt)
				{
				case 0:
				{
					result = "Skill0".Translate();
					break;
				}
				case 1:
				{
					result = "Skill1".Translate();
					break;
				}
				case 2:
				{
					result = "Skill2".Translate();
					break;
				}
				case 3:
				{
					result = "Skill3".Translate();
					break;
				}
				case 4:
				{
					result = "Skill4".Translate();
					break;
				}
				case 5:
				{
					result = "Skill5".Translate();
					break;
				}
				case 6:
				{
					result = "Skill6".Translate();
					break;
				}
				case 7:
				{
					result = "Skill7".Translate();
					break;
				}
				case 8:
				{
					result = "Skill8".Translate();
					break;
				}
				case 9:
				{
					result = "Skill9".Translate();
					break;
				}
				case 10:
				{
					result = "Skill10".Translate();
					break;
				}
				case 11:
				{
					result = "Skill11".Translate();
					break;
				}
				case 12:
				{
					result = "Skill12".Translate();
					break;
				}
				case 13:
				{
					result = "Skill13".Translate();
					break;
				}
				case 14:
				{
					result = "Skill14".Translate();
					break;
				}
				case 15:
				{
					result = "Skill15".Translate();
					break;
				}
				case 16:
				{
					result = "Skill16".Translate();
					break;
				}
				case 17:
				{
					result = "Skill17".Translate();
					break;
				}
				case 18:
				{
					result = "Skill18".Translate();
					break;
				}
				case 19:
				{
					result = "Skill19".Translate();
					break;
				}
				case 20:
				{
					result = "Skill20".Translate();
					break;
				}
				default:
				{
					result = "Unknown";
					break;
				}
				}
				return result;
			}
		}

		public bool LearningSaturatedToday
		{
			get
			{
				return this.xpSinceMidnight > 4000.0;
			}
		}

		public SkillRecord()
		{
		}

		public SkillRecord(Pawn pawn)
		{
			this.pawn = pawn;
		}

		public SkillRecord(Pawn pawn, SkillDef def)
		{
			this.pawn = pawn;
			this.def = def;
		}

		public void ExposeData()
		{
			Scribe_Defs.Look<SkillDef>(ref this.def, "def");
			Scribe_Values.Look<int>(ref this.levelInt, "level", 0, false);
			Scribe_Values.Look<float>(ref this.xpSinceLastLevel, "xpSinceLastLevel", 0f, false);
			Scribe_Values.Look<Passion>(ref this.passion, "passion", Passion.None, false);
			Scribe_Values.Look<float>(ref this.xpSinceMidnight, "xpSinceMidnight", 0f, false);
		}

		public void Interval()
		{
			switch (this.levelInt)
			{
			case 10:
			{
				this.Learn(-0.1f, false);
				break;
			}
			case 11:
			{
				this.Learn(-0.2f, false);
				break;
			}
			case 12:
			{
				this.Learn(-0.4f, false);
				break;
			}
			case 13:
			{
				this.Learn(-0.6f, false);
				break;
			}
			case 14:
			{
				this.Learn(-1f, false);
				break;
			}
			case 15:
			{
				this.Learn(-1.8f, false);
				break;
			}
			case 16:
			{
				this.Learn(-2.8f, false);
				break;
			}
			case 17:
			{
				this.Learn(-4f, false);
				break;
			}
			case 18:
			{
				this.Learn(-6f, false);
				break;
			}
			case 19:
			{
				this.Learn(-8f, false);
				break;
			}
			case 20:
			{
				this.Learn(-12f, false);
				break;
			}
			}
		}

		public static float XpRequiredToLevelUpFrom(int startingLevel)
		{
			return SkillRecord.XpForLevelUpCurve.Evaluate((float)startingLevel);
		}

		public void Learn(float xp, bool direct = false)
		{
			if (((!this.TotallyDisabled) ? ((!(xp < 0.0)) ? 1 : this.levelInt) : 0) != 0)
			{
				if (xp > 0.0)
				{
					if (this.pawn.needs.joy != null)
					{
						float amount = 0f;
						switch (this.passion)
						{
						case Passion.Minor:
						{
							amount = (float)(1.9999999494757503E-05 * xp);
							break;
						}
						case Passion.Major:
						{
							amount = (float)(3.9999998989515007E-05 * xp);
							break;
						}
						case Passion.None:
						{
							amount = (float)(0.0 * xp);
							break;
						}
						}
						this.pawn.needs.joy.GainJoy(amount, JoyKindDefOf.Work);
					}
					xp *= this.LearnRateFactor(direct);
				}
				this.xpSinceLastLevel += xp;
				if (!direct)
				{
					this.xpSinceMidnight += xp;
				}
				if (this.levelInt == 20 && this.xpSinceLastLevel > this.XpRequiredForLevelUp - 1.0)
				{
					this.xpSinceLastLevel = (float)(this.XpRequiredForLevelUp - 1.0);
				}
				while (this.xpSinceLastLevel >= this.XpRequiredForLevelUp)
				{
					this.xpSinceLastLevel -= this.XpRequiredForLevelUp;
					this.levelInt++;
					if (this.levelInt == 14)
					{
						if (this.passion == Passion.None)
						{
							TaleRecorder.RecordTale(TaleDefOf.GainedMasterSkillWithoutPassion, this.pawn, this.def);
						}
						else
						{
							TaleRecorder.RecordTale(TaleDefOf.GainedMasterSkillWithPassion, this.pawn, this.def);
						}
					}
					if (this.levelInt >= 20)
					{
						this.levelInt = 20;
						this.xpSinceLastLevel = Mathf.Clamp(this.xpSinceLastLevel, 0f, (float)(this.XpRequiredForLevelUp - 1.0));
						break;
					}
				}
				while (true)
				{
					if (this.xpSinceLastLevel < 0.0)
					{
						this.levelInt--;
						this.xpSinceLastLevel += this.XpRequiredForLevelUp;
						if (this.levelInt <= 0)
							break;
						continue;
					}
					return;
				}
				this.levelInt = 0;
				this.xpSinceLastLevel = 0f;
			}
		}

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
				{
					num = 0.35f;
					break;
				}
				case Passion.Minor:
				{
					num = 1f;
					break;
				}
				case Passion.Major:
				{
					num = 1.5f;
					break;
				}
				default:
				{
					throw new NotImplementedException("Passion level " + this.passion);
				}
				}
				if (!direct)
				{
					num *= this.pawn.GetStatValue(StatDefOf.GlobalLearningFactor, true);
					if (this.LearningSaturatedToday)
					{
						num = (float)(num * 0.20000000298023224);
					}
				}
				result = num;
			}
			return result;
		}

		public void Notify_SkillDisablesChanged()
		{
			this.cachedTotallyDisabled = BoolUnknown.Unknown;
		}

		private bool CalculateTotallyDisabled()
		{
			return this.def.IsDisabled(this.pawn.story.CombinedDisabledWorkTags, this.pawn.story.DisabledWorkTypes);
		}

		public override string ToString()
		{
			return this.def.defName + ": " + this.levelInt + " (" + this.xpSinceLastLevel + "xp)";
		}
	}
}
