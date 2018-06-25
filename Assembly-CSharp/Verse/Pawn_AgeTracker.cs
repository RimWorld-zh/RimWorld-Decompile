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
		// Token: 0x040032FC RID: 13052
		private Pawn pawn;

		// Token: 0x040032FD RID: 13053
		private long ageBiologicalTicksInt = -1L;

		// Token: 0x040032FE RID: 13054
		private long birthAbsTicksInt = -1L;

		// Token: 0x040032FF RID: 13055
		private int cachedLifeStageIndex = -1;

		// Token: 0x04003300 RID: 13056
		private long nextLifeStageChangeTick = -1L;

		// Token: 0x04003301 RID: 13057
		private const float BornAtLongitude = 0f;

		// Token: 0x06004C12 RID: 19474 RVA: 0x0027B8F1 File Offset: 0x00279CF1
		public Pawn_AgeTracker(Pawn newPawn)
		{
			this.pawn = newPawn;
		}

		// Token: 0x17000C57 RID: 3159
		// (get) Token: 0x06004C13 RID: 19475 RVA: 0x0027B920 File Offset: 0x00279D20
		// (set) Token: 0x06004C14 RID: 19476 RVA: 0x0027B93B File Offset: 0x00279D3B
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

		// Token: 0x17000C58 RID: 3160
		// (get) Token: 0x06004C15 RID: 19477 RVA: 0x0027B948 File Offset: 0x00279D48
		public int AgeBiologicalYears
		{
			get
			{
				return (int)(this.ageBiologicalTicksInt / 3600000L);
			}
		}

		// Token: 0x17000C59 RID: 3161
		// (get) Token: 0x06004C16 RID: 19478 RVA: 0x0027B96C File Offset: 0x00279D6C
		public float AgeBiologicalYearsFloat
		{
			get
			{
				return (float)this.ageBiologicalTicksInt / 3600000f;
			}
		}

		// Token: 0x17000C5A RID: 3162
		// (get) Token: 0x06004C17 RID: 19479 RVA: 0x0027B990 File Offset: 0x00279D90
		// (set) Token: 0x06004C18 RID: 19480 RVA: 0x0027B9AB File Offset: 0x00279DAB
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

		// Token: 0x17000C5B RID: 3163
		// (get) Token: 0x06004C19 RID: 19481 RVA: 0x0027B9BC File Offset: 0x00279DBC
		// (set) Token: 0x06004C1A RID: 19482 RVA: 0x0027B9DE File Offset: 0x00279DDE
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

		// Token: 0x17000C5C RID: 3164
		// (get) Token: 0x06004C1B RID: 19483 RVA: 0x0027B9F0 File Offset: 0x00279DF0
		public int AgeChronologicalYears
		{
			get
			{
				return (int)(this.AgeChronologicalTicks / 3600000L);
			}
		}

		// Token: 0x17000C5D RID: 3165
		// (get) Token: 0x06004C1C RID: 19484 RVA: 0x0027BA14 File Offset: 0x00279E14
		public float AgeChronologicalYearsFloat
		{
			get
			{
				return (float)this.AgeChronologicalTicks / 3600000f;
			}
		}

		// Token: 0x17000C5E RID: 3166
		// (get) Token: 0x06004C1D RID: 19485 RVA: 0x0027BA38 File Offset: 0x00279E38
		public int BirthYear
		{
			get
			{
				return GenDate.Year(this.birthAbsTicksInt, 0f);
			}
		}

		// Token: 0x17000C5F RID: 3167
		// (get) Token: 0x06004C1E RID: 19486 RVA: 0x0027BA60 File Offset: 0x00279E60
		public int BirthDayOfSeasonZeroBased
		{
			get
			{
				return GenDate.DayOfSeason(this.birthAbsTicksInt, 0f);
			}
		}

		// Token: 0x17000C60 RID: 3168
		// (get) Token: 0x06004C1F RID: 19487 RVA: 0x0027BA88 File Offset: 0x00279E88
		public int BirthDayOfYear
		{
			get
			{
				return GenDate.DayOfYear(this.birthAbsTicksInt, 0f);
			}
		}

		// Token: 0x17000C61 RID: 3169
		// (get) Token: 0x06004C20 RID: 19488 RVA: 0x0027BAB0 File Offset: 0x00279EB0
		public Quadrum BirthQuadrum
		{
			get
			{
				return GenDate.Quadrum(this.birthAbsTicksInt, 0f);
			}
		}

		// Token: 0x17000C62 RID: 3170
		// (get) Token: 0x06004C21 RID: 19489 RVA: 0x0027BAD8 File Offset: 0x00279ED8
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

		// Token: 0x17000C63 RID: 3171
		// (get) Token: 0x06004C22 RID: 19490 RVA: 0x0027BB3C File Offset: 0x00279F3C
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

		// Token: 0x17000C64 RID: 3172
		// (get) Token: 0x06004C23 RID: 19491 RVA: 0x0027BCC4 File Offset: 0x0027A0C4
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

		// Token: 0x17000C65 RID: 3173
		// (get) Token: 0x06004C24 RID: 19492 RVA: 0x0027BCF4 File Offset: 0x0027A0F4
		public LifeStageDef CurLifeStage
		{
			get
			{
				return this.CurLifeStageRace.def;
			}
		}

		// Token: 0x17000C66 RID: 3174
		// (get) Token: 0x06004C25 RID: 19493 RVA: 0x0027BD14 File Offset: 0x0027A114
		public LifeStageAge CurLifeStageRace
		{
			get
			{
				return this.pawn.RaceProps.lifeStageAges[this.CurLifeStageIndex];
			}
		}

		// Token: 0x17000C67 RID: 3175
		// (get) Token: 0x06004C26 RID: 19494 RVA: 0x0027BD44 File Offset: 0x0027A144
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

		// Token: 0x06004C27 RID: 19495 RVA: 0x0027BDAC File Offset: 0x0027A1AC
		public void ExposeData()
		{
			Scribe_Values.Look<long>(ref this.ageBiologicalTicksInt, "ageBiologicalTicks", 0L, false);
			Scribe_Values.Look<long>(ref this.birthAbsTicksInt, "birthAbsTicks", 0L, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.cachedLifeStageIndex = -1;
			}
		}

		// Token: 0x06004C28 RID: 19496 RVA: 0x0027BDE8 File Offset: 0x0027A1E8
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

		// Token: 0x06004C29 RID: 19497 RVA: 0x0027BE3C File Offset: 0x0027A23C
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

		// Token: 0x06004C2A RID: 19498 RVA: 0x0027BEB4 File Offset: 0x0027A2B4
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

		// Token: 0x06004C2B RID: 19499 RVA: 0x0027BFE4 File Offset: 0x0027A3E4
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
					}).AdjustedFor(this.pawn, "PAWN");
					Find.LetterStack.ReceiveLetter("LetterLabelBirthday".Translate(), text, LetterDefOf.NegativeEvent, this.pawn, null, null);
				}
			}
		}

		// Token: 0x06004C2C RID: 19500 RVA: 0x0027C12C File Offset: 0x0027A52C
		public void DebugForceBirthdayBiological()
		{
			this.BirthdayBiological();
		}

		// Token: 0x06004C2D RID: 19501 RVA: 0x0027C138 File Offset: 0x0027A538
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

		// Token: 0x06004C2E RID: 19502 RVA: 0x0027C1ED File Offset: 0x0027A5ED
		public void DebugMake1YearOlder()
		{
			this.ageBiologicalTicksInt += 3600000L;
			this.birthAbsTicksInt -= 3600000L;
			this.RecalculateLifeStageIndex();
		}
	}
}
