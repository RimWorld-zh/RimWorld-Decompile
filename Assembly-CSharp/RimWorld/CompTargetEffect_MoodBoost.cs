using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000757 RID: 1879
	public class CompTargetEffect_MoodBoost : CompTargetEffect
	{
		// Token: 0x06002991 RID: 10641 RVA: 0x001619C0 File Offset: 0x0015FDC0
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
