using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000757 RID: 1879
	public class CompTargetEffect_MoodBoost : CompTargetEffect
	{
		// Token: 0x06002992 RID: 10642 RVA: 0x00161760 File Offset: 0x0015FB60
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
