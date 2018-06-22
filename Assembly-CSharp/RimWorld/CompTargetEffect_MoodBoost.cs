using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000755 RID: 1877
	public class CompTargetEffect_MoodBoost : CompTargetEffect
	{
		// Token: 0x0600298E RID: 10638 RVA: 0x00161610 File Offset: 0x0015FA10
		public override void DoEffectOn(Pawn user, Thing target)
		{
			Pawn pawn = (Pawn)target;
			if (!pawn.Dead && pawn.needs != null && pawn.needs.mood != null)
			{
				pawn.needs.mood.thoughts.memories.TryGainMemory((Thought_Memory)ThoughtMaker.MakeThought(ThoughtDefOf.ArtifactMoodBoost), null);
			}
		}
	}
}
