using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020004B7 RID: 1207
	public class InteractionWorker_MarriageProposal : InteractionWorker
	{
		// Token: 0x04000CB5 RID: 3253
		private const float BaseSelectionWeight = 0.4f;

		// Token: 0x04000CB6 RID: 3254
		private const float BaseAcceptanceChance = 0.9f;

		// Token: 0x04000CB7 RID: 3255
		private const float BreakupChanceOnRejection = 0.4f;

		// Token: 0x06001581 RID: 5505 RVA: 0x000BF244 File Offset: 0x000BD644
		public override float RandomSelectionWeight(Pawn initiator, Pawn recipient)
		{
			DirectPawnRelation directRelation = initiator.relations.GetDirectRelation(PawnRelationDefOf.Lover, recipient);
			float result;
			if (directRelation == null)
			{
				result = 0f;
			}
			else
			{
				Pawn spouse = recipient.GetSpouse();
				Pawn spouse2 = initiator.GetSpouse();
				if ((spouse != null && !spouse.Dead) || (spouse2 != null && !spouse2.Dead))
				{
					result = 0f;
				}
				else
				{
					float num = 0.4f;
					int ticksGame = Find.TickManager.TicksGame;
					float value = (float)(ticksGame - directRelation.startTicks) / 60000f;
					num *= Mathf.InverseLerp(0f, 60f, value);
					num *= Mathf.InverseLerp(0f, 60f, (float)initiator.relations.OpinionOf(recipient));
					if (recipient.relations.OpinionOf(initiator) < 0)
					{
						num *= 0.3f;
					}
					if (initiator.gender == Gender.Female)
					{
						num *= 0.2f;
					}
					result = num;
				}
			}
			return result;
		}

		// Token: 0x06001582 RID: 5506 RVA: 0x000BF348 File Offset: 0x000BD748
		public override void Interacted(Pawn initiator, Pawn recipient, List<RulePackDef> extraSentencePacks, out string letterText, out string letterLabel, out LetterDef letterDef)
		{
			float num = this.AcceptanceChance(initiator, recipient);
			bool flag = Rand.Value < num;
			bool flag2 = false;
			if (flag)
			{
				initiator.relations.RemoveDirectRelation(PawnRelationDefOf.Lover, recipient);
				initiator.relations.AddDirectRelation(PawnRelationDefOf.Fiance, recipient);
				initiator.needs.mood.thoughts.memories.RemoveMemoriesOfDefWhereOtherPawnIs(ThoughtDefOf.RejectedMyProposal, recipient);
				recipient.needs.mood.thoughts.memories.RemoveMemoriesOfDefWhereOtherPawnIs(ThoughtDefOf.RejectedMyProposal, initiator);
				initiator.needs.mood.thoughts.memories.RemoveMemoriesOfDefWhereOtherPawnIs(ThoughtDefOf.RejectedMyProposalMood, recipient);
				recipient.needs.mood.thoughts.memories.RemoveMemoriesOfDefWhereOtherPawnIs(ThoughtDefOf.RejectedMyProposalMood, initiator);
				initiator.needs.mood.thoughts.memories.RemoveMemoriesOfDefWhereOtherPawnIs(ThoughtDefOf.IRejectedTheirProposal, recipient);
				recipient.needs.mood.thoughts.memories.RemoveMemoriesOfDefWhereOtherPawnIs(ThoughtDefOf.IRejectedTheirProposal, initiator);
				extraSentencePacks.Add(RulePackDefOf.Sentence_MarriageProposalAccepted);
			}
			else
			{
				initiator.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.RejectedMyProposal, recipient);
				recipient.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.IRejectedTheirProposal, initiator);
				extraSentencePacks.Add(RulePackDefOf.Sentence_MarriageProposalRejected);
				if (Rand.Value < 0.4f)
				{
					initiator.relations.RemoveDirectRelation(PawnRelationDefOf.Lover, recipient);
					initiator.relations.AddDirectRelation(PawnRelationDefOf.ExLover, recipient);
					flag2 = true;
					extraSentencePacks.Add(RulePackDefOf.Sentence_MarriageProposalRejectedBrokeUp);
				}
			}
			if (PawnUtility.ShouldSendNotificationAbout(initiator) || PawnUtility.ShouldSendNotificationAbout(recipient))
			{
				StringBuilder stringBuilder = new StringBuilder();
				if (flag)
				{
					letterLabel = "LetterLabelAcceptedProposal".Translate();
					letterDef = LetterDefOf.PositiveEvent;
					stringBuilder.AppendLine("LetterAcceptedProposal".Translate(new object[]
					{
						initiator,
						recipient
					}));
				}
				else
				{
					letterLabel = "LetterLabelRejectedProposal".Translate();
					letterDef = LetterDefOf.NegativeEvent;
					stringBuilder.AppendLine("LetterRejectedProposal".Translate(new object[]
					{
						initiator,
						recipient
					}));
					if (flag2)
					{
						stringBuilder.AppendLine();
						stringBuilder.AppendLine("LetterNoLongerLovers".Translate(new object[]
						{
							initiator,
							recipient
						}));
					}
				}
				letterText = stringBuilder.ToString().TrimEndNewlines();
			}
			else
			{
				letterLabel = null;
				letterText = null;
				letterDef = null;
			}
		}

		// Token: 0x06001583 RID: 5507 RVA: 0x000BF5D8 File Offset: 0x000BD9D8
		public float AcceptanceChance(Pawn initiator, Pawn recipient)
		{
			float num = 0.9f;
			num *= Mathf.Clamp01(GenMath.LerpDouble(-20f, 60f, 0f, 1f, (float)recipient.relations.OpinionOf(initiator)));
			return Mathf.Clamp01(num);
		}
	}
}
