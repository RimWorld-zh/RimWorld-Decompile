using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using RimWorld.Planet;

namespace Verse.AI
{
	// Token: 0x02000A5F RID: 2655
	public class MentalBreaker : IExposable
	{
		// Token: 0x04002557 RID: 9559
		private Pawn pawn;

		// Token: 0x04002558 RID: 9560
		private int ticksBelowExtreme = 0;

		// Token: 0x04002559 RID: 9561
		private int ticksBelowMajor = 0;

		// Token: 0x0400255A RID: 9562
		private int ticksBelowMinor = 0;

		// Token: 0x0400255B RID: 9563
		private const int CheckInterval = 150;

		// Token: 0x0400255C RID: 9564
		private const float ExtremeBreakMTBDays = 0.7f;

		// Token: 0x0400255D RID: 9565
		private const float MajorBreakMTBDays = 3f;

		// Token: 0x0400255E RID: 9566
		private const float MinorBreakMTBDays = 10f;

		// Token: 0x0400255F RID: 9567
		private const int MinTicksBelowToBreak = 1500;

		// Token: 0x04002560 RID: 9568
		private const float MajorBreakMoodSpan = 0.15f;

		// Token: 0x04002561 RID: 9569
		private const float MinorBreakMoodSpan = 0.15f;

		// Token: 0x04002562 RID: 9570
		private static List<Thought> tmpThoughts = new List<Thought>();

		// Token: 0x06003B08 RID: 15112 RVA: 0x001F56C5 File Offset: 0x001F3AC5
		public MentalBreaker()
		{
		}

		// Token: 0x06003B09 RID: 15113 RVA: 0x001F56E3 File Offset: 0x001F3AE3
		public MentalBreaker(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x17000902 RID: 2306
		// (get) Token: 0x06003B0A RID: 15114 RVA: 0x001F5708 File Offset: 0x001F3B08
		public float BreakThresholdExtreme
		{
			get
			{
				return this.pawn.GetStatValue(StatDefOf.MentalBreakThreshold, true);
			}
		}

		// Token: 0x17000903 RID: 2307
		// (get) Token: 0x06003B0B RID: 15115 RVA: 0x001F5730 File Offset: 0x001F3B30
		public float BreakThresholdMajor
		{
			get
			{
				return this.pawn.GetStatValue(StatDefOf.MentalBreakThreshold, true) + 0.15f;
			}
		}

		// Token: 0x17000904 RID: 2308
		// (get) Token: 0x06003B0C RID: 15116 RVA: 0x001F575C File Offset: 0x001F3B5C
		public float BreakThresholdMinor
		{
			get
			{
				return this.pawn.GetStatValue(StatDefOf.MentalBreakThreshold, true) + 0.15f + 0.15f;
			}
		}

		// Token: 0x17000905 RID: 2309
		// (get) Token: 0x06003B0D RID: 15117 RVA: 0x001F5790 File Offset: 0x001F3B90
		private bool CanDoRandomMentalBreaks
		{
			get
			{
				return this.pawn.RaceProps.Humanlike && (this.pawn.Spawned || this.pawn.IsCaravanMember());
			}
		}

		// Token: 0x17000906 RID: 2310
		// (get) Token: 0x06003B0E RID: 15118 RVA: 0x001F57DC File Offset: 0x001F3BDC
		public bool BreakExtremeIsImminent
		{
			get
			{
				return this.pawn.MentalStateDef == null && this.CurMood < this.BreakThresholdExtreme;
			}
		}

		// Token: 0x17000907 RID: 2311
		// (get) Token: 0x06003B0F RID: 15119 RVA: 0x001F5814 File Offset: 0x001F3C14
		public bool BreakMajorIsImminent
		{
			get
			{
				return this.pawn.MentalStateDef == null && !this.BreakExtremeIsImminent && this.CurMood < this.BreakThresholdMajor;
			}
		}

		// Token: 0x17000908 RID: 2312
		// (get) Token: 0x06003B10 RID: 15120 RVA: 0x001F5858 File Offset: 0x001F3C58
		public bool BreakMinorIsImminent
		{
			get
			{
				return this.pawn.MentalStateDef == null && !this.BreakExtremeIsImminent && !this.BreakMajorIsImminent && this.CurMood < this.BreakThresholdMinor;
			}
		}

		// Token: 0x17000909 RID: 2313
		// (get) Token: 0x06003B11 RID: 15121 RVA: 0x001F58A4 File Offset: 0x001F3CA4
		public bool BreakExtremeIsApproaching
		{
			get
			{
				return this.pawn.MentalStateDef == null && !this.BreakExtremeIsImminent && this.CurMood < this.BreakThresholdExtreme + 0.1f;
			}
		}

		// Token: 0x1700090A RID: 2314
		// (get) Token: 0x06003B12 RID: 15122 RVA: 0x001F58EC File Offset: 0x001F3CEC
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
		// (get) Token: 0x06003B13 RID: 15123 RVA: 0x001F5938 File Offset: 0x001F3D38
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
		// (get) Token: 0x06003B14 RID: 15124 RVA: 0x001F5964 File Offset: 0x001F3D64
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

		// Token: 0x06003B15 RID: 15125 RVA: 0x001F59BF File Offset: 0x001F3DBF
		internal void Reset()
		{
			this.ticksBelowExtreme = 0;
			this.ticksBelowMajor = 0;
		}

		// Token: 0x06003B16 RID: 15126 RVA: 0x001F59D0 File Offset: 0x001F3DD0
		public void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.ticksBelowExtreme, "ticksBelowExtreme", 0, false);
			Scribe_Values.Look<int>(ref this.ticksBelowMajor, "ticksBelowMajor", 0, false);
			Scribe_Values.Look<int>(ref this.ticksBelowMinor, "ticksBelowMinor", 0, false);
		}

		// Token: 0x06003B17 RID: 15127 RVA: 0x001F5A0C File Offset: 0x001F3E0C
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

		// Token: 0x06003B18 RID: 15128 RVA: 0x001F5BD0 File Offset: 0x001F3FD0
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

		// Token: 0x06003B19 RID: 15129 RVA: 0x001F5C64 File Offset: 0x001F4064
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

		// Token: 0x06003B1A RID: 15130 RVA: 0x001F5D24 File Offset: 0x001F4124
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

		// Token: 0x06003B1B RID: 15131 RVA: 0x001F5DF4 File Offset: 0x001F41F4
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

		// Token: 0x06003B1C RID: 15132 RVA: 0x001F5E48 File Offset: 0x001F4248
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

		// Token: 0x06003B1D RID: 15133 RVA: 0x001F6024 File Offset: 0x001F4424
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
