using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using RimWorld.Planet;

namespace Verse.AI
{
	// Token: 0x02000A60 RID: 2656
	public class MentalBreaker : IExposable
	{
		// Token: 0x06003B08 RID: 15112 RVA: 0x001F4F71 File Offset: 0x001F3371
		public MentalBreaker()
		{
		}

		// Token: 0x06003B09 RID: 15113 RVA: 0x001F4F8F File Offset: 0x001F338F
		public MentalBreaker(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x17000901 RID: 2305
		// (get) Token: 0x06003B0A RID: 15114 RVA: 0x001F4FB4 File Offset: 0x001F33B4
		public float BreakThresholdExtreme
		{
			get
			{
				return this.pawn.GetStatValue(StatDefOf.MentalBreakThreshold, true);
			}
		}

		// Token: 0x17000902 RID: 2306
		// (get) Token: 0x06003B0B RID: 15115 RVA: 0x001F4FDC File Offset: 0x001F33DC
		public float BreakThresholdMajor
		{
			get
			{
				return this.pawn.GetStatValue(StatDefOf.MentalBreakThreshold, true) + 0.15f;
			}
		}

		// Token: 0x17000903 RID: 2307
		// (get) Token: 0x06003B0C RID: 15116 RVA: 0x001F5008 File Offset: 0x001F3408
		public float BreakThresholdMinor
		{
			get
			{
				return this.pawn.GetStatValue(StatDefOf.MentalBreakThreshold, true) + 0.15f + 0.15f;
			}
		}

		// Token: 0x17000904 RID: 2308
		// (get) Token: 0x06003B0D RID: 15117 RVA: 0x001F503C File Offset: 0x001F343C
		private bool CanDoRandomMentalBreaks
		{
			get
			{
				return this.pawn.RaceProps.Humanlike && (this.pawn.Spawned || this.pawn.IsCaravanMember());
			}
		}

		// Token: 0x17000905 RID: 2309
		// (get) Token: 0x06003B0E RID: 15118 RVA: 0x001F5088 File Offset: 0x001F3488
		public bool BreakExtremeIsImminent
		{
			get
			{
				return this.pawn.MentalStateDef == null && this.CurMood < this.BreakThresholdExtreme;
			}
		}

		// Token: 0x17000906 RID: 2310
		// (get) Token: 0x06003B0F RID: 15119 RVA: 0x001F50C0 File Offset: 0x001F34C0
		public bool BreakMajorIsImminent
		{
			get
			{
				return this.pawn.MentalStateDef == null && !this.BreakExtremeIsImminent && this.CurMood < this.BreakThresholdMajor;
			}
		}

		// Token: 0x17000907 RID: 2311
		// (get) Token: 0x06003B10 RID: 15120 RVA: 0x001F5104 File Offset: 0x001F3504
		public bool BreakMinorIsImminent
		{
			get
			{
				return this.pawn.MentalStateDef == null && !this.BreakExtremeIsImminent && !this.BreakMajorIsImminent && this.CurMood < this.BreakThresholdMinor;
			}
		}

		// Token: 0x17000908 RID: 2312
		// (get) Token: 0x06003B11 RID: 15121 RVA: 0x001F5150 File Offset: 0x001F3550
		public bool BreakExtremeIsApproaching
		{
			get
			{
				return this.pawn.MentalStateDef == null && !this.BreakExtremeIsImminent && this.CurMood < this.BreakThresholdExtreme + 0.1f;
			}
		}

		// Token: 0x17000909 RID: 2313
		// (get) Token: 0x06003B12 RID: 15122 RVA: 0x001F5198 File Offset: 0x001F3598
		private float CurMood
		{
			get
			{
				float result;
				if (this.pawn.needs.mood == null)
				{
					result = 0.5f;
				}
				else
				{
					result = this.pawn.needs.mood.CurLevel;
				}
				return result;
			}
		}

		// Token: 0x1700090A RID: 2314
		// (get) Token: 0x06003B13 RID: 15123 RVA: 0x001F51E4 File Offset: 0x001F35E4
		private IEnumerable<MentalBreakDef> CurrentPossibleMoodBreaks
		{
			get
			{
				MentalBreakIntensity intensity;
				for (intensity = this.CurrentDesiredMoodBreakIntensity; intensity != MentalBreakIntensity.None; intensity = (MentalBreakIntensity)(intensity - MentalBreakIntensity.Minor))
				{
					IEnumerable<MentalBreakDef> breaks = from d in DefDatabase<MentalBreakDef>.AllDefsListForReading
					where d.intensity == intensity && d.Worker.BreakCanOccur(this.pawn)
					select d;
					bool yieldedAny = false;
					foreach (MentalBreakDef b in breaks)
					{
						yield return b;
						yieldedAny = true;
					}
					if (yieldedAny)
					{
						yield break;
					}
				}
				yield break;
			}
		}

		// Token: 0x1700090B RID: 2315
		// (get) Token: 0x06003B14 RID: 15124 RVA: 0x001F5210 File Offset: 0x001F3610
		private MentalBreakIntensity CurrentDesiredMoodBreakIntensity
		{
			get
			{
				MentalBreakIntensity result;
				if (this.ticksBelowExtreme >= 1500)
				{
					result = MentalBreakIntensity.Extreme;
				}
				else if (this.ticksBelowMajor >= 1500)
				{
					result = MentalBreakIntensity.Major;
				}
				else if (this.ticksBelowMinor >= 1500)
				{
					result = MentalBreakIntensity.Minor;
				}
				else
				{
					result = MentalBreakIntensity.None;
				}
				return result;
			}
		}

		// Token: 0x06003B15 RID: 15125 RVA: 0x001F526B File Offset: 0x001F366B
		internal void Reset()
		{
			this.ticksBelowExtreme = 0;
			this.ticksBelowMajor = 0;
		}

		// Token: 0x06003B16 RID: 15126 RVA: 0x001F527C File Offset: 0x001F367C
		public void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.ticksBelowExtreme, "ticksBelowExtreme", 0, false);
			Scribe_Values.Look<int>(ref this.ticksBelowMajor, "ticksBelowMajor", 0, false);
			Scribe_Values.Look<int>(ref this.ticksBelowMinor, "ticksBelowMinor", 0, false);
		}

		// Token: 0x06003B17 RID: 15127 RVA: 0x001F52B8 File Offset: 0x001F36B8
		public void MentalBreakerTick()
		{
			if (this.CanDoRandomMentalBreaks && this.pawn.MentalStateDef == null && this.pawn.IsHashIntervalTick(150))
			{
				if (DebugSettings.enableRandomMentalStates)
				{
					if (this.CurMood < this.BreakThresholdExtreme)
					{
						this.ticksBelowExtreme += 150;
					}
					else
					{
						this.ticksBelowExtreme = 0;
					}
					if (this.CurMood < this.BreakThresholdMajor)
					{
						this.ticksBelowMajor += 150;
					}
					else
					{
						this.ticksBelowMajor = 0;
					}
					if (this.CurMood < this.BreakThresholdMinor)
					{
						this.ticksBelowMinor += 150;
					}
					else
					{
						this.ticksBelowMinor = 0;
					}
					if (this.TestMoodMentalBreak())
					{
						if (this.TryDoRandomMoodCausedMentalBreak())
						{
							return;
						}
					}
					if (this.pawn.story != null)
					{
						List<Trait> allTraits = this.pawn.story.traits.allTraits;
						for (int i = 0; i < allTraits.Count; i++)
						{
							TraitDegreeData currentData = allTraits[i].CurrentData;
							if (currentData.randomMentalState != null)
							{
								float mtb = currentData.randomMentalStateMtbDaysMoodCurve.Evaluate(this.CurMood);
								if (Rand.MTBEventOccurs(mtb, 60000f, 150f))
								{
									if (currentData.randomMentalState.Worker.StateCanOccur(this.pawn))
									{
										if (this.pawn.mindState.mentalStateHandler.TryStartMentalState(currentData.randomMentalState, null, false, false, null, false))
										{
											break;
										}
									}
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x06003B18 RID: 15128 RVA: 0x001F547C File Offset: 0x001F387C
		private bool TestMoodMentalBreak()
		{
			bool result;
			if (this.ticksBelowExtreme > 1500)
			{
				result = Rand.MTBEventOccurs(0.7f, 60000f, 150f);
			}
			else if (this.ticksBelowMajor > 1500)
			{
				result = Rand.MTBEventOccurs(3f, 60000f, 150f);
			}
			else
			{
				result = (this.ticksBelowMinor > 1500 && Rand.MTBEventOccurs(10f, 60000f, 150f));
			}
			return result;
		}

		// Token: 0x06003B19 RID: 15129 RVA: 0x001F5510 File Offset: 0x001F3910
		public bool TryDoRandomMoodCausedMentalBreak()
		{
			bool result;
			MentalBreakDef mentalBreakDef;
			if (!this.CanDoRandomMentalBreaks || this.pawn.Downed || !this.pawn.Awake() || this.pawn.InMentalState)
			{
				result = false;
			}
			else if (this.pawn.Faction != Faction.OfPlayer && this.CurrentDesiredMoodBreakIntensity != MentalBreakIntensity.Extreme)
			{
				result = false;
			}
			else if (!this.CurrentPossibleMoodBreaks.TryRandomElementByWeight((MentalBreakDef d) => d.Worker.CommonalityFor(this.pawn), out mentalBreakDef))
			{
				result = false;
			}
			else
			{
				Thought reason = this.RandomFinalStraw();
				result = mentalBreakDef.Worker.TryStart(this.pawn, reason, true);
			}
			return result;
		}

		// Token: 0x06003B1A RID: 15130 RVA: 0x001F55D0 File Offset: 0x001F39D0
		private Thought RandomFinalStraw()
		{
			this.pawn.needs.mood.thoughts.GetAllMoodThoughts(MentalBreaker.tmpThoughts);
			float num = 0f;
			for (int i = 0; i < MentalBreaker.tmpThoughts.Count; i++)
			{
				float num2 = MentalBreaker.tmpThoughts[i].MoodOffset();
				if (num2 < num)
				{
					num = num2;
				}
			}
			float maxMoodOffset = num * 0.5f;
			Thought result = null;
			(from x in MentalBreaker.tmpThoughts
			where x.MoodOffset() <= maxMoodOffset
			select x).TryRandomElementByWeight((Thought x) => -x.MoodOffset(), out result);
			MentalBreaker.tmpThoughts.Clear();
			return result;
		}

		// Token: 0x06003B1B RID: 15131 RVA: 0x001F56A0 File Offset: 0x001F3AA0
		public float MentalBreakThresholdFor(MentalBreakIntensity intensity)
		{
			float result;
			if (intensity != MentalBreakIntensity.Extreme)
			{
				if (intensity != MentalBreakIntensity.Major)
				{
					if (intensity != MentalBreakIntensity.Minor)
					{
						throw new NotImplementedException();
					}
					result = this.BreakThresholdMinor;
				}
				else
				{
					result = this.BreakThresholdMajor;
				}
			}
			else
			{
				result = this.BreakThresholdExtreme;
			}
			return result;
		}

		// Token: 0x06003B1C RID: 15132 RVA: 0x001F56F4 File Offset: 0x001F3AF4
		internal string DebugString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine(this.pawn.ToString());
			stringBuilder.AppendLine(string.Concat(new object[]
			{
				"   ticksBelowExtreme=",
				this.ticksBelowExtreme,
				"/",
				1500
			}));
			stringBuilder.AppendLine(string.Concat(new object[]
			{
				"   ticksBelowSerious=",
				this.ticksBelowMajor,
				"/",
				1500
			}));
			stringBuilder.AppendLine(string.Concat(new object[]
			{
				"   ticksBelowMinor=",
				this.ticksBelowMinor,
				"/",
				1500
			}));
			stringBuilder.AppendLine();
			stringBuilder.AppendLine("Current desired mood break intensity: " + this.CurrentDesiredMoodBreakIntensity.ToString());
			stringBuilder.AppendLine();
			stringBuilder.AppendLine("Current possible mood breaks:");
			float num = (from d in this.CurrentPossibleMoodBreaks
			select d.Worker.CommonalityFor(this.pawn)).Sum();
			foreach (MentalBreakDef mentalBreakDef in this.CurrentPossibleMoodBreaks)
			{
				float num2 = mentalBreakDef.Worker.CommonalityFor(this.pawn);
				stringBuilder.AppendLine(string.Concat(new object[]
				{
					"   ",
					mentalBreakDef,
					"     ",
					(num2 / num).ToStringPercent()
				}));
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06003B1D RID: 15133 RVA: 0x001F58D0 File Offset: 0x001F3CD0
		internal void LogPossibleMentalBreaks()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine(this.pawn + " current possible mood mental breaks:");
			stringBuilder.AppendLine("CurrentDesiredMoodBreakIntensity: " + this.CurrentDesiredMoodBreakIntensity);
			foreach (MentalBreakDef arg in this.CurrentPossibleMoodBreaks)
			{
				stringBuilder.AppendLine("  " + arg);
			}
			Log.Message(stringBuilder.ToString(), false);
		}

		// Token: 0x0400254B RID: 9547
		private Pawn pawn;

		// Token: 0x0400254C RID: 9548
		private int ticksBelowExtreme = 0;

		// Token: 0x0400254D RID: 9549
		private int ticksBelowMajor = 0;

		// Token: 0x0400254E RID: 9550
		private int ticksBelowMinor = 0;

		// Token: 0x0400254F RID: 9551
		private const int CheckInterval = 150;

		// Token: 0x04002550 RID: 9552
		private const float ExtremeBreakMTBDays = 0.7f;

		// Token: 0x04002551 RID: 9553
		private const float MajorBreakMTBDays = 3f;

		// Token: 0x04002552 RID: 9554
		private const float MinorBreakMTBDays = 10f;

		// Token: 0x04002553 RID: 9555
		private const int MinTicksBelowToBreak = 1500;

		// Token: 0x04002554 RID: 9556
		private const float MajorBreakMoodSpan = 0.15f;

		// Token: 0x04002555 RID: 9557
		private const float MinorBreakMoodSpan = 0.15f;

		// Token: 0x04002556 RID: 9558
		private static List<Thought> tmpThoughts = new List<Thought>();
	}
}
