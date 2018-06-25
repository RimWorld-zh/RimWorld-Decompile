using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using RimWorld.Planet;

namespace Verse.AI
{
	// Token: 0x02000A5E RID: 2654
	public class MentalBreaker : IExposable
	{
		// Token: 0x04002547 RID: 9543
		private Pawn pawn;

		// Token: 0x04002548 RID: 9544
		private int ticksBelowExtreme = 0;

		// Token: 0x04002549 RID: 9545
		private int ticksBelowMajor = 0;

		// Token: 0x0400254A RID: 9546
		private int ticksBelowMinor = 0;

		// Token: 0x0400254B RID: 9547
		private const int CheckInterval = 150;

		// Token: 0x0400254C RID: 9548
		private const float ExtremeBreakMTBDays = 0.7f;

		// Token: 0x0400254D RID: 9549
		private const float MajorBreakMTBDays = 3f;

		// Token: 0x0400254E RID: 9550
		private const float MinorBreakMTBDays = 10f;

		// Token: 0x0400254F RID: 9551
		private const int MinTicksBelowToBreak = 1500;

		// Token: 0x04002550 RID: 9552
		private const float MajorBreakMoodSpan = 0.15f;

		// Token: 0x04002551 RID: 9553
		private const float MinorBreakMoodSpan = 0.15f;

		// Token: 0x04002552 RID: 9554
		private static List<Thought> tmpThoughts = new List<Thought>();

		// Token: 0x06003B07 RID: 15111 RVA: 0x001F5399 File Offset: 0x001F3799
		public MentalBreaker()
		{
		}

		// Token: 0x06003B08 RID: 15112 RVA: 0x001F53B7 File Offset: 0x001F37B7
		public MentalBreaker(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x17000902 RID: 2306
		// (get) Token: 0x06003B09 RID: 15113 RVA: 0x001F53DC File Offset: 0x001F37DC
		public float BreakThresholdExtreme
		{
			get
			{
				return this.pawn.GetStatValue(StatDefOf.MentalBreakThreshold, true);
			}
		}

		// Token: 0x17000903 RID: 2307
		// (get) Token: 0x06003B0A RID: 15114 RVA: 0x001F5404 File Offset: 0x001F3804
		public float BreakThresholdMajor
		{
			get
			{
				return this.pawn.GetStatValue(StatDefOf.MentalBreakThreshold, true) + 0.15f;
			}
		}

		// Token: 0x17000904 RID: 2308
		// (get) Token: 0x06003B0B RID: 15115 RVA: 0x001F5430 File Offset: 0x001F3830
		public float BreakThresholdMinor
		{
			get
			{
				return this.pawn.GetStatValue(StatDefOf.MentalBreakThreshold, true) + 0.15f + 0.15f;
			}
		}

		// Token: 0x17000905 RID: 2309
		// (get) Token: 0x06003B0C RID: 15116 RVA: 0x001F5464 File Offset: 0x001F3864
		private bool CanDoRandomMentalBreaks
		{
			get
			{
				return this.pawn.RaceProps.Humanlike && (this.pawn.Spawned || this.pawn.IsCaravanMember());
			}
		}

		// Token: 0x17000906 RID: 2310
		// (get) Token: 0x06003B0D RID: 15117 RVA: 0x001F54B0 File Offset: 0x001F38B0
		public bool BreakExtremeIsImminent
		{
			get
			{
				return this.pawn.MentalStateDef == null && this.CurMood < this.BreakThresholdExtreme;
			}
		}

		// Token: 0x17000907 RID: 2311
		// (get) Token: 0x06003B0E RID: 15118 RVA: 0x001F54E8 File Offset: 0x001F38E8
		public bool BreakMajorIsImminent
		{
			get
			{
				return this.pawn.MentalStateDef == null && !this.BreakExtremeIsImminent && this.CurMood < this.BreakThresholdMajor;
			}
		}

		// Token: 0x17000908 RID: 2312
		// (get) Token: 0x06003B0F RID: 15119 RVA: 0x001F552C File Offset: 0x001F392C
		public bool BreakMinorIsImminent
		{
			get
			{
				return this.pawn.MentalStateDef == null && !this.BreakExtremeIsImminent && !this.BreakMajorIsImminent && this.CurMood < this.BreakThresholdMinor;
			}
		}

		// Token: 0x17000909 RID: 2313
		// (get) Token: 0x06003B10 RID: 15120 RVA: 0x001F5578 File Offset: 0x001F3978
		public bool BreakExtremeIsApproaching
		{
			get
			{
				return this.pawn.MentalStateDef == null && !this.BreakExtremeIsImminent && this.CurMood < this.BreakThresholdExtreme + 0.1f;
			}
		}

		// Token: 0x1700090A RID: 2314
		// (get) Token: 0x06003B11 RID: 15121 RVA: 0x001F55C0 File Offset: 0x001F39C0
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

		// Token: 0x1700090B RID: 2315
		// (get) Token: 0x06003B12 RID: 15122 RVA: 0x001F560C File Offset: 0x001F3A0C
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

		// Token: 0x1700090C RID: 2316
		// (get) Token: 0x06003B13 RID: 15123 RVA: 0x001F5638 File Offset: 0x001F3A38
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

		// Token: 0x06003B14 RID: 15124 RVA: 0x001F5693 File Offset: 0x001F3A93
		internal void Reset()
		{
			this.ticksBelowExtreme = 0;
			this.ticksBelowMajor = 0;
		}

		// Token: 0x06003B15 RID: 15125 RVA: 0x001F56A4 File Offset: 0x001F3AA4
		public void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.ticksBelowExtreme, "ticksBelowExtreme", 0, false);
			Scribe_Values.Look<int>(ref this.ticksBelowMajor, "ticksBelowMajor", 0, false);
			Scribe_Values.Look<int>(ref this.ticksBelowMinor, "ticksBelowMinor", 0, false);
		}

		// Token: 0x06003B16 RID: 15126 RVA: 0x001F56E0 File Offset: 0x001F3AE0
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

		// Token: 0x06003B17 RID: 15127 RVA: 0x001F58A4 File Offset: 0x001F3CA4
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

		// Token: 0x06003B18 RID: 15128 RVA: 0x001F5938 File Offset: 0x001F3D38
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

		// Token: 0x06003B19 RID: 15129 RVA: 0x001F59F8 File Offset: 0x001F3DF8
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

		// Token: 0x06003B1A RID: 15130 RVA: 0x001F5AC8 File Offset: 0x001F3EC8
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

		// Token: 0x06003B1B RID: 15131 RVA: 0x001F5B1C File Offset: 0x001F3F1C
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

		// Token: 0x06003B1C RID: 15132 RVA: 0x001F5CF8 File Offset: 0x001F40F8
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
	}
}
