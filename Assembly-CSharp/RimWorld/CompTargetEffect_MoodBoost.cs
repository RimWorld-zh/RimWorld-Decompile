using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000759 RID: 1881
	public class CompTargetEffect_MoodBoost : CompTargetEffect
	{
		// Token: 0x06002995 RID: 10645 RVA: 0x00161438 File Offset: 0x0015F838
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
