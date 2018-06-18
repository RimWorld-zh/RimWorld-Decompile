using System;
using System.Collections.Generic;
using System.Text;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000D55 RID: 3413
	public class Pawn_AgeTracker : IExposable
	{
		// Token: 0x06004BFA RID: 19450 RVA: 0x00279F4D File Offset: 0x0027834D
		public Pawn_AgeTracker(Pawn newPawn)
		{
			this.pawn = newPawn;
		}

		// Token: 0x17000C56 RID: 3158
		// (get) Token: 0x06004BFB RID: 19451 RVA: 0x00279F7C File Offset: 0x0027837C
		// (set) Token: 0x06004BFC RID: 19452 RVA: 0x00279F97 File Offset: 0x00278397
		public long BirthAbsTicks
		{
			get
			{
				return this.birthAbsTicksInt;
			}
			set
			{
				this.birthAbsTicksInt = value;
			}
		}

		// Token: 0x17000C57 RID: 3159
		// (get) Token: 0x06004BFD RID: 19453 RVA: 0x00279FA4 File Offset: 0x002783A4
		public int AgeBiologicalYears
		{
			get
			{
				return (int)(this.ageBiologicalTicksInt / 3600000L);
			}
		}

		// Token: 0x17000C58 RID: 3160
		// (get) Token: 0x06004BFE RID: 19454 RVA: 0x00279FC8 File Offset: 0x002783C8
		public float AgeBiologicalYearsFloat
		{
			get
			{
				return (float)this.ageBiologicalTicksInt / 3600000f;
			}
		}

		// Token: 0x17000C59 RID: 3161
		// (get) Token: 0x06004BFF RID: 19455 RVA: 0x00279FEC File Offset: 0x002783EC
		// (set) Token: 0x06004C00 RID: 19456 RVA: 0x0027A007 File Offset: 0x00278407
		public long AgeBiologicalTicks
		{
			get
			{
				return this.ageBiologicalTicksInt;
			}
			set
			{
				this.ageBiologicalTicksInt = value;
				this.cachedLifeStageIndex = -1;
			}
		}

		// Token: 0x17000C5A RID: 3162
		// (get) Token: 0x06004C01 RID: 19457 RVA: 0x0027A018 File Offset: 0x00278418
		// (set) Token: 0x06004C02 RID: 19458 RVA: 0x0027A03A File Offset: 0x0027843A
		public long AgeChronologicalTicks
		{
			get
			{
				return (long)GenTicks.TicksAbs - this.birthAbsTicksInt;
			}
			set
			{
				this.BirthAbsTicks = (long)GenTicks.TicksAbs - value;
			}
		}

		// Token: 0x17000C5B RID: 3163
		// (get) Token: 0x06004C03 RID: 19459 RVA: 0x0027A04C File Offset: 0x0027844C
		public int AgeChronologicalYears
		{
			get
			{
				return (int)(this.AgeChronologicalTicks / 3600000L);
			}
		}

		// Token: 0x17000C5C RID: 3164
		// (get) Token: 0x06004C04 RID: 19460 RVA: 0x0027A070 File Offset: 0x00278470
		public float AgeChronologicalYearsFloat
		{
			get
			{
				return (float)this.AgeChronologicalTicks / 3600000f;
			}
		}

		// Token: 0x17000C5D RID: 3165
		// (get) Token: 0x06004C05 RID: 19461 RVA: 0x0027A094 File Offset: 0x00278494
		public int BirthYear
		{
			get
			{
				return GenDate.Year(this.birthAbsTicksInt, 0f);
			}
		}

		// Token: 0x17000C5E RID: 3166
		// (get) Token: 0x06004C06 RID: 19462 RVA: 0x0027A0BC File Offset: 0x002784BC
		public int BirthDayOfSeasonZeroBased
		{
			get
			{
				return GenDate.DayOfSeason(this.birthAbsTicksInt, 0f);
			}
		}

		// Token: 0x17000C5F RID: 3167
		// (get) Token: 0x06004C07 RID: 19463 RVA: 0x0027A0E4 File Offset: 0x002784E4
		public int BirthDayOfYear
		{
			get
			{
				return GenDate.DayOfYear(this.birthAbsTicksInt, 0f);
			}
		}

		// Token: 0x17000C60 RID: 3168
		// (get) Token: 0x06004C08 RID: 19464 RVA: 0x0027A10C File Offset: 0x0027850C
		public Quadrum BirthQuadrum
		{
			get
			{
				return GenDate.Quadrum(this.birthAbsTicksInt, 0f);
			}
		}

		// Token: 0x17000C61 RID: 3169
		// (get) Token: 0x06004C09 RID: 19465 RVA: 0x0027A134 File Offset: 0x00278534
		public string AgeNumberString
		{
			get
			{
				string text = this.AgeBiologicalYearsFloat.ToStringApproxAge();
				if (this.AgeChronologicalYears != this.AgeBiologicalYears)
				{
					string text2 = text;
					text = string.Concat(new object[]
					{
						text2,
						" (",
						this.AgeChronologicalYears,
						")"
					});
				}
				return text;
			}
		}

		// Token: 0x17000C62 RID: 3170
		// (get) Token: 0x06004C0A RID: 19466 RVA: 0x0027A198 File Offset: 0x00278598
		public string AgeTooltipString
		{
			get
			{
				int num;
				int num2;
				int num3;
				float num4;
				this.ageBiologicalTicksInt.TicksToPeriod(out num, out num2, out num3, out num4);
				long numTicks = (long)GenTicks.TicksAbs - this.birthAbsTicksInt;
				int num5;
				int num6;
				int num7;
				numTicks.TicksToPeriod(out num5, out num6, out num7, out num4);
				string text = "FullDate".Translate(new object[]
				{
					Find.ActiveLanguageWorker.OrdinalNumber(this.BirthDayOfSeasonZeroBased + 1),
					this.BirthQuadrum.Label(),
					this.BirthYear
				});
				string text2 = string.Concat(new string[]
				{
					"Born".Translate(new object[]
					{
						text
					}),
					"\n",
					"AgeChronological".Translate(new object[]
					{
						num5,
						num6,
						num7
					}),
					"\n",
					"AgeBiological".Translate(new object[]
					{
						num,
						num2,
						num3
					})
				});
				if (Prefs.DevMode)
				{
					text2 += "\n\nDev mode info:";
					text2 = text2 + "\nageBiologicalTicksInt: " + this.ageBiologicalTicksInt;
					text2 = text2 + "\nbirthAbsTicksInt: " + this.birthAbsTicksInt;
					text2 = text2 + "\nnextLifeStageChangeTick: " + this.nextLifeStageChangeTick;
				}
				return text2;
			}
		}

		// Token: 0x17000C63 RID: 3171
		// (get) Token: 0x06004C0B RID: 19467 RVA: 0x0027A320 File Offset: 0x00278720
		public int CurLifeStageIndex
		{
			get
			{
				if (this.cachedLifeStageIndex < 0)
				{
					this.RecalculateLifeStageIndex();
				}
				return this.cachedLifeStageIndex;
			}
		}

		// Token: 0x17000C64 RID: 3172
		// (get) Token: 0x06004C0C RID: 19468 RVA: 0x0027A350 File Offset: 0x00278750
		public LifeStageDef CurLifeStage
		{
			get
			{
				return this.CurLifeStageRace.def;
			}
		}

		// Token: 0x17000C65 RID: 3173
		// (get) Token: 0x06004C0D RID: 19469 RVA: 0x0027A370 File Offset: 0x00278770
		public LifeStageAge CurLifeStageRace
		{
			get
			{
				return this.pawn.RaceProps.lifeStageAges[this.CurLifeStageIndex];
			}
		}

		// Token: 0x17000C66 RID: 3174
		// (get) Token: 0x06004C0E RID: 19470 RVA: 0x0027A3A0 File Offset: 0x002787A0
		public PawnKindLifeStage CurKindLifeStage
		{
			get
			{
				PawnKindLifeStage result;
				if (this.pawn.RaceProps.Humanlike)
				{
					Log.ErrorOnce("Tried to get CurKindLifeStage from humanlike pawn " + this.pawn, 8888811, false);
					result = null;
				}
				else
				{
					result = this.pawn.kindDef.lifeStages[this.CurLifeStageIndex];
				}
				return result;
			}
		}

		// Token: 0x06004C0F RID: 19471 RVA: 0x0027A408 File Offset: 0x00278808
		public void ExposeData()
		{
			Scribe_Values.Look<long>(ref this.ageBiologicalTicksInt, "ageBiologicalTicks", 0L, false);
			Scribe_Values.Look<long>(ref this.birthAbsTicksInt, "birthAbsTicks", 0L, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.cachedLifeStageIndex = -1;
			}
		}

		// Token: 0x06004C10 RID: 19472 RVA: 0x0027A444 File Offset: 0x00278844
		public void AgeTick()
		{
			this.ageBiologicalTicksInt += 1L;
			if ((long)Find.TickManager.TicksGame >= this.nextLifeStageChangeTick)
			{
				this.RecalculateLifeStageIndex();
			}
			if (this.ageBiologicalTicksInt % 3600000L == 0L)
			{
				this.BirthdayBiological();
			}
		}

		// Token: 0x06004C11 RID: 19473 RVA: 0x0027A498 File Offset: 0x00278898
		public void AgeTickMothballed(int interval)
		{
			long num = this.ageBiologicalTicksInt;
			this.ageBiologicalTicksInt += (long)interval;
			while ((long)Find.TickManager.TicksGame >= this.nextLifeStageChangeTick)
			{
				this.RecalculateLifeStageIndex();
			}
			int num2 = (int)(num / 3600000L);
			while ((long)num2 < this.ageBiologicalTicksInt / 3600000L)
			{
				this.BirthdayBiological();
				num2 += 3600000;
			}
		}

		// Token: 0x06004C12 RID: 19474 RVA: 0x0027A510 File Offset: 0x00278910
		private void RecalculateLifeStageIndex()
		{
			int num = -1;
			List<LifeStageAge> lifeStageAges = this.pawn.RaceProps.lifeStageAges;
			for (int i = lifeStageAges.Count - 1; i >= 0; i--)
			{
				if (lifeStageAges[i].minAge <= this.AgeBiologicalYearsFloat + 1E-06f)
				{
					num = i;
					break;
				}
			}
			if (num == -1)
			{
				num = 0;
			}
			bool flag = this.cachedLifeStageIndex != num;
			this.cachedLifeStageIndex = num;
			if (flag && !this.pawn.RaceProps.Humanlike)
			{
				LongEventHandler.ExecuteWhenFinished(delegate
				{
					this.pawn.Drawer.renderer.graphics.ResolveAllGraphics();
				});
				this.CheckChangePawnKindName();
			}
			if (this.cachedLifeStageIndex < lifeStageAges.Count - 1)
			{
				float num2 = lifeStageAges[this.cachedLifeStageIndex + 1].minAge - this.AgeBiologicalYearsFloat;
				int num3 = (Current.ProgramState != ProgramState.Playing) ? 0 : Find.TickManager.TicksGame;
				this.nextLifeStageChangeTick = (long)num3 + (long)Mathf.Ceil(num2 * 3600000f);
			}
			else
			{
				this.nextLifeStageChangeTick = long.MaxValue;
			}
		}

		// Token: 0x06004C13 RID: 19475 RVA: 0x0027A640 File Offset: 0x00278A40
		private void BirthdayBiological()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (HediffGiver_Birthday hediffGiver_Birthday in AgeInjuryUtility.RandomHediffsToGainOnBirthday(this.pawn, this.AgeBiologicalYears))
			{
				if (hediffGiver_Birthday.TryApply(this.pawn, null))
				{
					if (stringBuilder.Length != 0)
					{
						stringBuilder.AppendLine();
					}
					stringBuilder.Append("    - " + hediffGiver_Birthday.hediff.LabelCap);
				}
			}
			if (this.pawn.RaceProps.Humanlike && PawnUtility.ShouldSendNotificationAbout(this.pawn))
			{
				if (stringBuilder.Length > 0)
				{
					string text = "BirthdayBiologicalAgeInjuries".Translate(new object[]
					{
						this.pawn,
						this.AgeBiologicalYears,
						stringBuilder
					}).AdjustedFor(this.pawn);
					Find.LetterStack.ReceiveLetter("LetterLabelBirthday".Translate(), text, LetterDefOf.NegativeEvent, this.pawn, null, null);
				}
			}
		}

		// Token: 0x06004C14 RID: 19476 RVA: 0x0027A784 File Offset: 0x00278B84
		public void DebugForceBirthdayBiological()
		{
			this.BirthdayBiological();
		}

		// Token: 0x06004C15 RID: 19477 RVA: 0x0027A790 File Offset: 0x00278B90
		private void CheckChangePawnKindName()
		{
			NameSingle nameSingle = this.pawn.Name as NameSingle;
			if (nameSingle != null && nameSingle.Numerical)
			{
				string kindLabel = this.pawn.KindLabel;
				if (!(nameSingle.NameWithoutNumber == kindLabel))
				{
					int number = nameSingle.Number;
					string text = this.pawn.KindLabel + " " + number;
					if (!NameUseChecker.NameSingleIsUsed(text))
					{
						this.pawn.Name = new NameSingle(text, true);
					}
					else
					{
						this.pawn.Name = PawnBioAndNameGenerator.GeneratePawnName(this.pawn, NameStyle.Numeric, null);
					}
				}
			}
		}

		// Token: 0x06004C16 RID: 19478 RVA: 0x0027A845 File Offset: 0x00278C45
		public void DebugMake1YearOlder()
		{
			this.ageBiologicalTicksInt += 3600000L;
			this.birthAbsTicksInt -= 3600000L;
			this.RecalculateLifeStageIndex();
		}

		// Token: 0x040032EA RID: 13034
		private Pawn pawn;

		// Token: 0x040032EB RID: 13035
		private long ageBiologicalTicksInt = -1L;

		// Token: 0x040032EC RID: 13036
		private long birthAbsTicksInt = -1L;

		// Token: 0x040032ED RID: 13037
		private int cachedLifeStageIndex = -1;

		// Token: 0x040032EE RID: 13038
		private long nextLifeStageChangeTick = -1L;

		// Token: 0x040032EF RID: 13039
		private const float BornAtLongitude = 0f;
	}
}
