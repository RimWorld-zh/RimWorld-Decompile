using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020004FC RID: 1276
	public class Need_Mood : Need_Seeker
	{
		// Token: 0x04000D85 RID: 3461
		public ThoughtHandler thoughts;

		// Token: 0x04000D86 RID: 3462
		public PawnObserver observer;

		// Token: 0x04000D87 RID: 3463
		public PawnRecentMemory recentMemory;

		// Token: 0x060016F9 RID: 5881 RVA: 0x000CAC66 File Offset: 0x000C9066
		public Need_Mood(Pawn pawn) : base(pawn)
		{
			this.thoughts = new ThoughtHandler(pawn);
			this.observer = new PawnObserver(pawn);
			this.recentMemory = new PawnRecentMemory(pawn);
		}

		// Token: 0x17000321 RID: 801
		// (get) Token: 0x060016FA RID: 5882 RVA: 0x000CAC94 File Offset: 0x000C9094
		public override float CurInstantLevel
		{
			get
			{
				float num = this.thoughts.TotalMoodOffset();
				if (this.pawn.IsColonist || this.pawn.IsPrisonerOfColony)
				{
					num += Find.Storyteller.difficulty.colonistMoodOffset;
				}
				return Mathf.Clamp01(this.def.baseLevel + num / 100f);
			}
		}

		// Token: 0x17000322 RID: 802
		// (get) Token: 0x060016FB RID: 5883 RVA: 0x000CAD00 File Offset: 0x000C9100
		public string MoodString
		{
			get
			{
				string result;
				if (this.pawn.MentalStateDef != null)
				{
					result = "Mood_MentalState".Translate();
				}
				else
				{
					float statValue = this.pawn.GetStatValue(StatDefOf.MentalBreakThreshold, true);
					if (this.CurLevel < statValue)
					{
						result = "Mood_AboutToBreak".Translate();
					}
					else if (this.CurLevel < statValue + 0.05f)
					{
						result = "Mood_OnEdge".Translate();
					}
					else if (this.CurLevel < this.pawn.mindState.mentalBreaker.BreakThresholdMinor)
					{
						result = "Mood_Stressed".Translate();
					}
					else if (this.CurLevel < 0.65f)
					{
						result = "Mood_Neutral".Translate();
					}
					else if (this.CurLevel < 0.9f)
					{
						result = "Mood_Content".Translate();
					}
					else
					{
						result = "Mood_Happy".Translate();
					}
				}
				return result;
			}
		}

		// Token: 0x060016FC RID: 5884 RVA: 0x000CAE00 File Offset: 0x000C9200
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Deep.Look<ThoughtHandler>(ref this.thoughts, "thoughts", new object[]
			{
				this.pawn
			});
			Scribe_Deep.Look<PawnRecentMemory>(ref this.recentMemory, "recentMemory", new object[]
			{
				this.pawn
			});
		}

		// Token: 0x060016FD RID: 5885 RVA: 0x000CAE52 File Offset: 0x000C9252
		public override void NeedInterval()
		{
			base.NeedInterval();
			this.recentMemory.RecentMemoryInterval();
			this.thoughts.ThoughtInterval();
			this.observer.ObserverInterval();
		}

		// Token: 0x060016FE RID: 5886 RVA: 0x000CAE7C File Offset: 0x000C927C
		public override string GetTipString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine(base.GetTipString());
			stringBuilder.AppendLine();
			stringBuilder.AppendLine("MentalBreakThresholdExtreme".Translate() + ": " + this.pawn.mindState.mentalBreaker.BreakThresholdExtreme.ToStringPercent());
			stringBuilder.AppendLine("MentalBreakThresholdMajor".Translate() + ": " + this.pawn.mindState.mentalBreaker.BreakThresholdMajor.ToStringPercent());
			stringBuilder.AppendLine("MentalBreakThresholdMinor".Translate() + ": " + this.pawn.mindState.mentalBreaker.BreakThresholdMinor.ToStringPercent());
			return stringBuilder.ToString();
		}

		// Token: 0x060016FF RID: 5887 RVA: 0x000CAF50 File Offset: 0x000C9350
		public override void DrawOnGUI(Rect rect, int maxThresholdMarkers = 2147483647, float customMargin = -1f, bool drawArrows = true, bool doTooltip = true)
		{
			if (this.threshPercents == null)
			{
				this.threshPercents = new List<float>();
			}
			this.threshPercents.Clear();
			this.threshPercents.Add(this.pawn.mindState.mentalBreaker.BreakThresholdExtreme);
			this.threshPercents.Add(this.pawn.mindState.mentalBreaker.BreakThresholdMajor);
			this.threshPercents.Add(this.pawn.mindState.mentalBreaker.BreakThresholdMinor);
			base.DrawOnGUI(rect, maxThresholdMarkers, customMargin, drawArrows, doTooltip);
		}
	}
}
