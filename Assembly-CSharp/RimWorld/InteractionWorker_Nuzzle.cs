using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020004B8 RID: 1208
	public class InteractionWorker_Nuzzle : InteractionWorker
	{
		// Token: 0x06001585 RID: 5509 RVA: 0x000BF62F File Offset: 0x000BDA2F
		public override void Interacted(Pawn initiator, Pawn recipient, List<RulePackDef> extraSentencePacks, out string letterText, out string letterLabel, out LetterDef letterDef)
		{
			this.AddNuzzledThought(initiator, recipient);
			this.TryGiveName(initiator, recipient);
			letterText = null;
			letterLabel = null;
			letterDef = null;
		}

		// Token: 0x06001586 RID: 5510 RVA: 0x000BF650 File Offset: 0x000BDA50
		private void AddNuzzledThought(Pawn initiator, Pawn recipient)
		{
			Thought_Memory newThought = (Thought_Memory)ThoughtMaker.MakeThought(ThoughtDefOf.Nuzzled);
			recipient.needs.mood.thoughts.memories.TryGainMemory(newThought, null);
		}

		// Token: 0x06001587 RID: 5511 RVA: 0x000BF68A File Offset: 0x000BDA8A
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
