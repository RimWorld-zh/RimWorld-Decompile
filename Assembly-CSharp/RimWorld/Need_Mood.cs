using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public class Need_Mood : Need_Seeker
	{
		public ThoughtHandler thoughts;

		public PawnObserver observer;

		public PawnRecentMemory recentMemory;

		public override float CurInstantLevel
		{
			get
			{
				float num = this.thoughts.TotalMoodOffset();
				if (base.pawn.IsColonist || base.pawn.IsPrisonerOfColony)
				{
					num += Find.Storyteller.difficulty.colonistMoodOffset;
				}
				return Mathf.Clamp01((float)(base.def.baseLevel + num / 100.0));
			}
		}

		public string MoodString
		{
			get
			{
				string result;
				if (base.pawn.MentalStateDef != null)
				{
					result = "Mood_MentalState".Translate();
				}
				else
				{
					float statValue = base.pawn.GetStatValue(StatDefOf.MentalBreakThreshold, true);
					result = ((!(this.CurLevel < statValue)) ? ((!(this.CurLevel < statValue + 0.05000000074505806)) ? ((!(this.CurLevel < base.pawn.mindState.mentalBreaker.BreakThresholdMinor)) ? ((!(this.CurLevel < 0.64999997615814209)) ? ((!(this.CurLevel < 0.89999997615814209)) ? "Mood_Happy".Translate() : "Mood_Content".Translate()) : "Mood_Neutral".Translate()) : "Mood_Stressed".Translate()) : "Mood_OnEdge".Translate()) : "Mood_AboutToBreak".Translate());
				}
				return result;
			}
		}

		public Need_Mood(Pawn pawn) : base(pawn)
		{
			this.thoughts = new ThoughtHandler(pawn);
			this.observer = new PawnObserver(pawn);
			this.recentMemory = new PawnRecentMemory(pawn);
		}

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Deep.Look<ThoughtHandler>(ref this.thoughts, "thoughts", new object[1]
			{
				base.pawn
			});
			Scribe_Deep.Look<PawnRecentMemory>(ref this.recentMemory, "recentMemory", new object[1]
			{
				base.pawn
			});
		}

		public override void NeedInterval()
		{
			base.NeedInterval();
			this.recentMemory.RecentMemoryInterval();
			this.thoughts.ThoughtInterval();
			this.observer.ObserverInterval();
		}

		public override string GetTipString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine(base.GetTipString());
			stringBuilder.AppendLine();
			stringBuilder.AppendLine("MentalBreakThresholdExtreme".Translate() + ": " + base.pawn.mindState.mentalBreaker.BreakThresholdExtreme.ToStringPercent());
			stringBuilder.AppendLine("MentalBreakThresholdMajor".Translate() + ": " + base.pawn.mindState.mentalBreaker.BreakThresholdMajor.ToStringPercent());
			stringBuilder.AppendLine("MentalBreakThresholdMinor".Translate() + ": " + base.pawn.mindState.mentalBreaker.BreakThresholdMinor.ToStringPercent());
			return stringBuilder.ToString();
		}

		public override void DrawOnGUI(Rect rect, int maxThresholdMarkers = 2147483647, float customMargin = -1f, bool drawArrows = true, bool doTooltip = true)
		{
			if (base.threshPercents == null)
			{
				base.threshPercents = new List<float>();
			}
			base.threshPercents.Clear();
			base.threshPercents.Add(base.pawn.mindState.mentalBreaker.BreakThresholdExtreme);
			base.threshPercents.Add(base.pawn.mindState.mentalBreaker.BreakThresholdMajor);
			base.threshPercents.Add(base.pawn.mindState.mentalBreaker.BreakThresholdMinor);
			base.DrawOnGUI(rect, maxThresholdMarkers, customMargin, drawArrows, doTooltip);
		}
	}
}
