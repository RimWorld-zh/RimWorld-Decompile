using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020004B6 RID: 1206
	public class InteractionWorker_Nuzzle : InteractionWorker
	{
		// Token: 0x06001582 RID: 5506 RVA: 0x000BF2DF File Offset: 0x000BD6DF
		public override void Interacted(Pawn initiator, Pawn recipient, List<RulePackDef> extraSentencePacks, out string letterText, out string letterLabel, out LetterDef letterDef)
		{
			this.AddNuzzledThought(initiator, recipient);
			this.TryGiveName(initiator, recipient);
			letterText = null;
			letterLabel = null;
			letterDef = null;
		}

		// Token: 0x06001583 RID: 5507 RVA: 0x000BF300 File Offset: 0x000BD700
		private void AddNuzzledThought(Pawn initiator, Pawn recipient)
		{
			Thought_Memory newThought = (Thought_Memory)ThoughtMaker.MakeThought(ThoughtDefOf.Nuzzled);
			recipient.needs.mood.thoughts.memories.TryGainMemory(newThought, null);
		}

		// Token: 0x06001584 RID: 5508 RVA: 0x000BF33A File Offset: 0x000BD73A
		private void TryGiveName(Pawn initiator, Pawn recipient)
		{
			if (initiator.Name == null || initiator.Name.Numerical)
			{
				if (Rand.Value < initiator.RaceProps.nameOnNuzzleChance)
				{
					PawnUtility.GiveNameBecauseOfNuzzle(recipient, initiator);
				}
			}
		}
	}
}
