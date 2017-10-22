using RimWorld;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Verse
{
	public class Pawn_AgeTracker : IExposable
	{
		private Pawn pawn;

		private long ageBiologicalTicksInt = -1L;

		private long birthAbsTicksInt = -1L;

		private int cachedLifeStageIndex = -1;

		private int nextLifeStageChangeTick = -1;

		private const float BornAtLongitude = 0f;

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

		public int AgeBiologicalYears
		{
			get
			{
				return (int)(this.ageBiologicalTicksInt / 3600000);
			}
		}

		public float AgeBiologicalYearsFloat
		{
			get
			{
				return (float)((float)this.ageBiologicalTicksInt / 3600000.0);
			}
		}

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

		public long AgeChronologicalTicks
		{
			get
			{
				return GenTicks.TicksAbs - this.birthAbsTicksInt;
			}
			set
			{
				this.BirthAbsTicks = GenTicks.TicksAbs - value;
			}
		}

		public int AgeChronologicalYears
		{
			get
			{
				return (int)(this.AgeChronologicalTicks / 3600000);
			}
		}

		public float AgeChronologicalYearsFloat
		{
			get
			{
				return (float)((float)this.AgeChronologicalTicks / 3600000.0);
			}
		}

		public int BirthYear
		{
			get
			{
				return GenDate.Year(this.birthAbsTicksInt, 0f);
			}
		}

		public int BirthDayOfSeasonZeroBased
		{
			get
			{
				return GenDate.DayOfSeason(this.birthAbsTicksInt, 0f);
			}
		}

		public int BirthDayOfYear
		{
			get
			{
				return GenDate.DayOfYear(this.birthAbsTicksInt, 0f);
			}
		}

		public Quadrum BirthQuadrum
		{
			get
			{
				return GenDate.Quadrum(this.birthAbsTicksInt, 0f);
			}
		}

		public string AgeNumberString
		{
			get
			{
				string text = this.AgeBiologicalYearsFloat.ToStringApproxAge();
				if (this.AgeChronologicalYears != this.AgeBiologicalYears)
				{
					string text2 = text;
					text = text2 + " (" + this.AgeChronologicalYears + ")";
				}
				return text;
			}
		}

		public string AgeTooltipString
		{
			get
			{
				int num = default(int);
				int num2 = default(int);
				int num3 = default(int);
				float num4 = default(float);
				this.ageBiologicalTicksInt.TicksToPeriod(out num, out num2, out num3, out num4);
				long numTicks = GenTicks.TicksAbs - this.birthAbsTicksInt;
				int num5 = default(int);
				int num6 = default(int);
				int num7 = default(int);
				numTicks.TicksToPeriod(out num5, out num6, out num7, out num4);
				string text = "FullDate".Translate(Find.ActiveLanguageWorker.OrdinalNumber(this.BirthDayOfSeasonZeroBased + 1), this.BirthQuadrum.Label(), this.BirthYear);
				string text2 = "Born".Translate(text) + "\n" + "AgeChronological".Translate(num5, num6, num7) + "\n" + "AgeBiological".Translate(num, num2, num3);
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

		public LifeStageDef CurLifeStage
		{
			get
			{
				return this.CurLifeStageRace.def;
			}
		}

		public LifeStageAge CurLifeStageRace
		{
			get
			{
				return this.pawn.RaceProps.lifeStageAges[this.CurLifeStageIndex];
			}
		}

		public PawnKindLifeStage CurKindLifeStage
		{
			get
			{
				PawnKindLifeStage result;
				if (this.pawn.RaceProps.Humanlike)
				{
					Log.ErrorOnce("Tried to get CurKindLifeStage from humanlike pawn " + this.pawn, 8888811);
					result = null;
				}
				else
				{
					result = this.pawn.kindDef.lifeStages[this.CurLifeStageIndex];
				}
				return result;
			}
		}

		public Pawn_AgeTracker(Pawn newPawn)
		{
			this.pawn = newPawn;
		}

		public void ExposeData()
		{
			Scribe_Values.Look<long>(ref this.ageBiologicalTicksInt, "ageBiologicalTicks", 0L, false);
			Scribe_Values.Look<long>(ref this.birthAbsTicksInt, "birthAbsTicks", 0L, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.cachedLifeStageIndex = -1;
			}
		}

		public void AgeTick()
		{
			this.ageBiologicalTicksInt += 1L;
			if (Find.TickManager.TicksGame >= this.nextLifeStageChangeTick)
			{
				this.RecalculateLifeStageIndex();
			}
			if (this.ageBiologicalTicksInt % 3600000 == 0)
			{
				this.BirthdayBiological();
			}
		}

		public void AgeTickMothballed(int interval)
		{
			long num = this.ageBiologicalTicksInt;
			this.ageBiologicalTicksInt += (long)interval;
			while (Find.TickManager.TicksGame >= this.nextLifeStageChangeTick)
			{
				this.RecalculateLifeStageIndex();
			}
			for (int num2 = (int)(num / 3600000); num2 < this.ageBiologicalTicksInt / 3600000; num2 += 3600000)
			{
				this.BirthdayBiological();
			}
		}

		private void RecalculateLifeStageIndex()
		{
			int num = -1;
			List<LifeStageAge> lifeStageAges = this.pawn.RaceProps.lifeStageAges;
			int num2 = lifeStageAges.Count - 1;
			while (num2 >= 0)
			{
				if (!(lifeStageAges[num2].minAge <= this.AgeBiologicalYearsFloat + 9.9999999747524271E-07))
				{
					num2--;
					continue;
				}
				num = num2;
				break;
			}
			if (num == -1)
			{
				num = 0;
			}
			bool flag = this.cachedLifeStageIndex != num;
			this.cachedLifeStageIndex = num;
			if (flag && !this.pawn.RaceProps.Humanlike)
			{
				LongEventHandler.ExecuteWhenFinished((Action)delegate
				{
					this.pawn.Drawer.renderer.graphics.ResolveAllGraphics();
				});
				this.CheckChangePawnKindName();
			}
			if (this.cachedLifeStageIndex < lifeStageAges.Count - 1)
			{
				float num3 = lifeStageAges[this.cachedLifeStageIndex + 1].minAge - this.AgeBiologicalYearsFloat;
				int num4 = (Current.ProgramState == ProgramState.Playing) ? Find.TickManager.TicksGame : 0;
				this.nextLifeStageChangeTick = num4 + Mathf.CeilToInt((float)(num3 * 3600000.0));
			}
			else
			{
				this.nextLifeStageChangeTick = 2147483647;
			}
		}

		private void BirthdayBiological()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (HediffGiver_Birthday item in AgeInjuryUtility.RandomHediffsToGainOnBirthday(this.pawn, this.AgeBiologicalYears))
			{
				if (item.TryApply(this.pawn, null))
				{
					if (stringBuilder.Length != 0)
					{
						stringBuilder.AppendLine();
					}
					stringBuilder.Append("    - " + item.hediff.LabelCap);
				}
			}
			if (this.pawn.RaceProps.Humanlike && PawnUtility.ShouldSendNotificationAbout(this.pawn) && stringBuilder.Length > 0)
			{
				string text = "BirthdayBiologicalAgeInjuries".Translate(this.pawn, this.AgeBiologicalYears, stringBuilder).AdjustedFor(this.pawn);
				Find.LetterStack.ReceiveLetter("LetterLabelBirthday".Translate(), text, LetterDefOf.NegativeEvent, (Thing)this.pawn, (string)null);
			}
		}

		public void DebugForceBirthdayBiological()
		{
			this.BirthdayBiological();
		}

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
						this.pawn.Name = PawnBioAndNameGenerator.GeneratePawnName(this.pawn, NameStyle.Numeric, (string)null);
					}
				}
			}
		}

		public void DebugMake1YearOlder()
		{
			this.ageBiologicalTicksInt += 3600000L;
			this.birthAbsTicksInt -= 3600000L;
			this.RecalculateLifeStageIndex();
		}
	}
}
