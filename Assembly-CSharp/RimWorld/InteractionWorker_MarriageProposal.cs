using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public class InteractionWorker_MarriageProposal : InteractionWorker
	{
		private const float BaseSelectionWeight = 0.4f;

		private const float BaseAcceptanceChance = 0.9f;

		private const float BreakupChanceOnRejection = 0.4f;

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
				if (spouse != null && !spouse.Dead)
				{
					goto IL_0054;
				}
				if (spouse2 != null && !spouse2.Dead)
					goto IL_0054;
				float num = 0.4f;
				int ticksGame = Find.TickManager.TicksGame;
				float value = (float)((float)(ticksGame - directRelation.startTicks) / 60000.0);
				num *= Mathf.InverseLerp(0f, 60f, value);
				num *= Mathf.InverseLerp(0f, 60f, (float)initiator.relations.OpinionOf(recipient));
				if (recipient.relations.OpinionOf(initiator) < 0)
				{
					num = (float)(num * 0.30000001192092896);
				}
				if (initiator.gender == Gender.Female)
				{
					num = (float)(num * 0.20000000298023224);
				}
				result = num;
			}
			goto IL_00f5;
			IL_00f5:
			return result;
			IL_0054:
			result = 0f;
			goto IL_00f5;
		}

		public override void Interacted(Pawn initiator, Pawn recipient, List<RulePackDef> extraSentencePacks)
		{
			float num = this.AcceptanceChance(initiator, recipient);
			bool flag = Rand.Value < num;
			bool brokeUp = false;
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
				if (Rand.Value < 0.40000000596046448)
				{
					initiator.relations.RemoveDirectRelation(PawnRelationDefOf.Lover, recipient);
					initiator.relations.AddDirectRelation(PawnRelationDefOf.ExLover, recipient);
					brokeUp = true;
					extraSentencePacks.Add(RulePackDefOf.Sentence_MarriageProposalRejectedBrokeUp);
				}
			}
			if (!initiator.IsColonist && !recipient.IsColonist)
				return;
			this.SendLetter(initiator, recipient, flag, brokeUp);
		}

		public float AcceptanceChance(Pawn initiator, Pawn recipient)
		{
			float num = 0.9f;
			num *= Mathf.Clamp01(GenMath.LerpDouble(-20f, 60f, 0f, 1f, (float)recipient.relations.OpinionOf(initiator)));
			return Mathf.Clamp01(num);
		}

		private void SendLetter(Pawn initiator, Pawn recipient, bool accepted, bool brokeUp)
		{
			StringBuilder stringBuilder = new StringBuilder();
			string label;
			LetterDef textLetterDef;
			if (accepted)
			{
				label = "LetterLabelAcceptedProposal".Translate();
				textLetterDef = LetterDefOf.PositiveEvent;
				stringBuilder.AppendLine("LetterAcceptedProposal".Translate(initiator, recipient));
			}
			else
			{
				label = "LetterLabelRejectedProposal".Translate();
				textLetterDef = LetterDefOf.NegativeEvent;
				stringBuilder.AppendLine("LetterRejectedProposal".Translate(initiator, recipient));
				if (brokeUp)
				{
					stringBuilder.AppendLine();
					stringBuilder.AppendLine("LetterNoLongerLovers".Translate(initiator, recipient));
				}
			}
			Find.LetterStack.ReceiveLetter(label, stringBuilder.ToString().TrimEndNewlines(), textLetterDef, (Thing)initiator, (string)null);
		}
	}
}
