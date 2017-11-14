using RimWorld;
using RimWorld.Planet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Verse.AI
{
	public class MentalBreaker : IExposable
	{
		private Pawn pawn;

		private int ticksBelowExtreme;

		private int ticksBelowMajor;

		private int ticksBelowMinor;

		private const int CheckInterval = 150;

		private const float ExtremeBreakMTBDays = 0.7f;

		private const float MajorBreakMTBDays = 3f;

		private const float MinorBreakMTBDays = 10f;

		private const int MinTicksBelowToBreak = 1500;

		private const float MajorBreakMoodSpan = 0.15f;

		private const float MinorBreakMoodSpan = 0.15f;

		private static List<Thought> tmpThoughts = new List<Thought>();

		public float BreakThresholdExtreme
		{
			get
			{
				return this.pawn.GetStatValue(StatDefOf.MentalBreakThreshold, true);
			}
		}

		public float BreakThresholdMajor
		{
			get
			{
				return (float)(this.pawn.GetStatValue(StatDefOf.MentalBreakThreshold, true) + 0.15000000596046448);
			}
		}

		public float BreakThresholdMinor
		{
			get
			{
				return (float)(this.pawn.GetStatValue(StatDefOf.MentalBreakThreshold, true) + 0.15000000596046448 + 0.15000000596046448);
			}
		}

		private bool CanDoRandomMentalBreaks
		{
			get
			{
				return this.pawn.RaceProps.Humanlike && (this.pawn.Spawned || this.pawn.IsCaravanMember());
			}
		}

		public bool BreakExtremeIsImminent
		{
			get
			{
				return this.pawn.MentalStateDef == null && this.CurMood < this.BreakThresholdExtreme;
			}
		}

		public bool BreakMajorIsImminent
		{
			get
			{
				return this.pawn.MentalStateDef == null && !this.BreakExtremeIsImminent && this.CurMood < this.BreakThresholdMajor;
			}
		}

		public bool BreakMinorIsImminent
		{
			get
			{
				return this.pawn.MentalStateDef == null && !this.BreakExtremeIsImminent && !this.BreakMajorIsImminent && this.CurMood < this.BreakThresholdMinor;
			}
		}

		public bool BreakExtremeIsApproaching
		{
			get
			{
				return this.pawn.MentalStateDef == null && !this.BreakExtremeIsImminent && this.CurMood < this.BreakThresholdExtreme + 0.10000000149011612;
			}
		}

		private float CurMood
		{
			get
			{
				if (this.pawn.needs.mood == null)
				{
					return 0.5f;
				}
				return this.pawn.needs.mood.CurLevel;
			}
		}

		private IEnumerable<MentalBreakDef> CurrentPossibleMoodBreaks
		{
			get
			{
				_003C_003Ec__Iterator0 _003C_003Ec__Iterator = (_003C_003Ec__Iterator0)/*Error near IL_0034: stateMachine*/;
				MentalBreakIntensity intensity = this.CurrentDesiredMoodBreakIntensity;
				while (true)
				{
					IEnumerable<MentalBreakDef> breaks = from d in DefDatabase<MentalBreakDef>.AllDefsListForReading
					where d.intensity == intensity && d.Worker.BreakCanOccur(_003C_003Ec__Iterator._0024this.pawn)
					select d;
					bool yieldedOne = false;
					using (IEnumerator<MentalBreakDef> enumerator = breaks.GetEnumerator())
					{
						if (enumerator.MoveNext())
						{
							MentalBreakDef b = enumerator.Current;
							yield return b;
							/*Error: Unable to find new state assignment for yield return*/;
						}
					}
					if (!yieldedOne)
					{
						if (intensity != MentalBreakIntensity.Minor)
						{
							intensity--;
							continue;
						}
						break;
					}
					yield break;
				}
				Log.ErrorOnce("No mental breaks possible for " + this.pawn, 888112);
				yield break;
				IL_0178:
				/*Error near IL_0179: Unexpected return in MoveNext()*/;
			}
		}

		private MentalBreakIntensity CurrentDesiredMoodBreakIntensity
		{
			get
			{
				if (this.ticksBelowExtreme >= 1500)
				{
					return MentalBreakIntensity.Extreme;
				}
				if (this.ticksBelowMajor >= 1500)
				{
					return MentalBreakIntensity.Major;
				}
				if (this.ticksBelowMinor >= 1500)
				{
					return MentalBreakIntensity.Minor;
				}
				Log.ErrorOnce("Got CurrentDesiredBreakIntensity for " + this.pawn + " but he don't desire any break right now.", 123126);
				return MentalBreakIntensity.Minor;
			}
		}

		public MentalBreaker()
		{
		}

		public MentalBreaker(Pawn pawn)
		{
			this.pawn = pawn;
		}

		internal void Reset()
		{
			this.ticksBelowExtreme = 0;
			this.ticksBelowMajor = 0;
		}

		public void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.ticksBelowExtreme, "ticksBelowExtreme", 0, false);
			Scribe_Values.Look<int>(ref this.ticksBelowMajor, "ticksBelowMajor", 0, false);
			Scribe_Values.Look<int>(ref this.ticksBelowMinor, "ticksBelowMinor", 0, false);
		}

		public void MentalBreakerTick()
		{
			if (this.CanDoRandomMentalBreaks && this.pawn.MentalStateDef == null && this.pawn.IsHashIntervalTick(150) && DebugSettings.enableRandomMentalStates)
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
				if (this.TestMoodMentalBreak() && this.TryDoRandomMoodCausedMentalBreak())
					return;
				if (this.pawn.story != null)
				{
					List<Trait> allTraits = this.pawn.story.traits.allTraits;
					for (int i = 0; i < allTraits.Count; i++)
					{
						TraitDegreeData currentData = allTraits[i].CurrentData;
						if (currentData.randomMentalState != null)
						{
							float mtb = currentData.randomMentalStateMtbDaysMoodCurve.Evaluate(this.CurMood);
							if (Rand.MTBEventOccurs(mtb, 60000f, 150f) && currentData.randomMentalState.Worker.StateCanOccur(this.pawn) && this.pawn.mindState.mentalStateHandler.TryStartMentalState(currentData.randomMentalState, null, false, false, null))
								break;
						}
					}
				}
			}
		}

		private bool TestMoodMentalBreak()
		{
			if (this.ticksBelowExtreme > 1500)
			{
				return Rand.MTBEventOccurs(0.7f, 60000f, 150f);
			}
			if (this.ticksBelowMajor > 1500)
			{
				return Rand.MTBEventOccurs(3f, 60000f, 150f);
			}
			if (this.ticksBelowMinor > 1500)
			{
				return Rand.MTBEventOccurs(10f, 60000f, 150f);
			}
			return false;
		}

		public bool TryDoRandomMoodCausedMentalBreak()
		{
			if (this.CanDoRandomMentalBreaks && !this.pawn.Downed && this.pawn.Awake() && !this.pawn.InMentalState)
			{
				if (this.pawn.Faction != Faction.OfPlayer && this.CurrentDesiredMoodBreakIntensity != 0)
				{
					return false;
				}
				MentalBreakDef mentalBreakDef = default(MentalBreakDef);
				if (!this.CurrentPossibleMoodBreaks.TryRandomElementByWeight<MentalBreakDef>((Func<MentalBreakDef, float>)((MentalBreakDef d) => d.Worker.CommonalityFor(this.pawn)), out mentalBreakDef))
				{
					return false;
				}
				Thought reason = this.RandomFinalStraw();
				return mentalBreakDef.Worker.TryStart(this.pawn, reason, true);
			}
			return false;
		}

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
			float maxMoodOffset = (float)(num * 0.5);
			Thought result = null;
			(from x in MentalBreaker.tmpThoughts
			where x.MoodOffset() <= maxMoodOffset
			select x).TryRandomElementByWeight<Thought>((Func<Thought, float>)((Thought x) => (float)(0.0 - x.MoodOffset())), out result);
			MentalBreaker.tmpThoughts.Clear();
			return result;
		}

		public float MentalBreakThresholdFor(MentalBreakIntensity intensity)
		{
			switch (intensity)
			{
			case MentalBreakIntensity.Extreme:
				return this.BreakThresholdExtreme;
			case MentalBreakIntensity.Major:
				return this.BreakThresholdMajor;
			case MentalBreakIntensity.Minor:
				return this.BreakThresholdMinor;
			default:
				throw new NotImplementedException();
			}
		}

		internal string DebugString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine(this.pawn.ToString());
			stringBuilder.AppendLine("   ticksBelowExtreme=" + this.ticksBelowExtreme + "/" + 1500);
			stringBuilder.AppendLine("   ticksBelowSerious=" + this.ticksBelowMajor + "/" + 1500);
			stringBuilder.AppendLine("   ticksBelowMinor=" + this.ticksBelowMinor + "/" + 1500);
			float num = (from d in this.CurrentPossibleMoodBreaks
			select d.Worker.CommonalityFor(this.pawn)).Sum();
			foreach (MentalBreakDef currentPossibleMoodBreak in this.CurrentPossibleMoodBreaks)
			{
				float num2 = currentPossibleMoodBreak.Worker.CommonalityFor(this.pawn);
				stringBuilder.AppendLine("   " + currentPossibleMoodBreak + " " + (num2 / num).ToStringPercent("F4"));
			}
			return stringBuilder.ToString();
		}

		internal void LogPossibleMentalBreaks()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine(this.pawn + " current possible mood mental breaks:");
			stringBuilder.AppendLine("CurrentDesiredMoodBreakIntensity: " + this.CurrentDesiredMoodBreakIntensity);
			foreach (MentalBreakDef currentPossibleMoodBreak in this.CurrentPossibleMoodBreaks)
			{
				stringBuilder.AppendLine("  " + currentPossibleMoodBreak);
			}
			Log.Message(stringBuilder.ToString());
		}
	}
}
