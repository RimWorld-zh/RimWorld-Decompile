using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020004B8 RID: 1208
	public class InteractionWorker_Nuzzle : InteractionWorker
	{
		// Token: 0x06001586 RID: 5510 RVA: 0x000BF42F File Offset: 0x000BD82F
		public override void Interacted(Pawn initiator, Pawn recipient, List<RulePackDef> extraSentencePacks, out string letterText, out string letterLabel, out LetterDef letterDef)
		{
			this.AddNuzzledThought(initiator, recipient);
			this.TryGiveName(initiator, recipient);
			letterText = null;
			letterLabel = null;
			letterDef = null;
		}

		// Token: 0x06001587 RID: 5511 RVA: 0x000BF450 File Offset: 0x000BD850
		private void AddNuzzledThought(Pawn initiator, Pawn recipient)
		{
			Thought_Memory newThought = (Thought_Memory)ThoughtMaker.MakeThought(ThoughtDefOf.Nuzzled);
			recipient.needs.mood.thoughts.memories.TryGainMemory(newThought, null);
		}

		// Token: 0x06001588 RID: 5512 RVA: 0x000BF48A File Offset: 0x000BD88A
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
