using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public class InteractionWorker_RomanceAttempt : InteractionWorker
	{
		private const float MinAttractionForRomanceAttempt = 0.25f;

		private const int MinOpinionForRomanceAttempt = 5;

		private const float BaseSelectionWeight = 1.15f;

		private const float BaseSuccessChance = 0.6f;

		public override float RandomSelectionWeight(Pawn initiator, Pawn recipient)
		{
			float result;
			if (LovePartnerRelationUtility.LovePartnerRelationExists(initiator, recipient))
			{
				result = 0f;
			}
			else
			{
				float num = initiator.relations.SecondaryRomanceChanceFactor(recipient);
				if (num < 0.25)
				{
					result = 0f;
				}
				else
				{
					int num2 = initiator.relations.OpinionOf(recipient);
					if (num2 < 5)
					{
						result = 0f;
					}
					else if (recipient.relations.OpinionOf(initiator) < 5)
					{
						result = 0f;
					}
					else
					{
						float num3 = 1f;
						Pawn pawn = LovePartnerRelationUtility.ExistingMostLikedLovePartner(initiator, false);
						if (pawn != null)
						{
							float value = (float)initiator.relations.OpinionOf(pawn);
							num3 = Mathf.InverseLerp(50f, -50f, value);
						}
						float num4 = (float)((!initiator.story.traits.HasTrait(TraitDefOf.Gay)) ? ((initiator.gender != Gender.Female) ? 1.0 : 0.15000000596046448) : 1.0);
						float num5 = Mathf.InverseLerp(0.25f, 1f, num);
						float num6 = Mathf.InverseLerp(5f, 100f, (float)num2);
						float num7 = (float)((initiator.gender != recipient.gender) ? ((initiator.story.traits.HasTrait(TraitDefOf.Gay) || recipient.story.traits.HasTrait(TraitDefOf.Gay)) ? 0.15000000596046448 : 1.0) : ((!initiator.story.traits.HasTrait(TraitDefOf.Gay) || !recipient.story.traits.HasTrait(TraitDefOf.Gay)) ? 0.15000000596046448 : 1.0));
						result = (float)(1.1499999761581421 * num4 * num5 * num6 * num3 * num7);
					}
				}
			}
			return result;
		}

		public float SuccessChance(Pawn initiator, Pawn recipient)
		{
			float num = 0.6f;
			num *= recipient.relations.SecondaryRomanceChanceFactor(initiator);
			num *= Mathf.InverseLerp(5f, 100f, (float)recipient.relations.OpinionOf(initiator));
			float num2 = 1f;
			Pawn pawn = null;
			if (recipient.relations.GetFirstDirectRelationPawn(PawnRelationDefOf.Lover, (Predicate<Pawn>)((Pawn x) => !x.Dead)) != null)
			{
				pawn = recipient.relations.GetFirstDirectRelationPawn(PawnRelationDefOf.Lover, null);
				num2 = 0.6f;
			}
			else if (recipient.relations.GetFirstDirectRelationPawn(PawnRelationDefOf.Fiance, (Predicate<Pawn>)((Pawn x) => !x.Dead)) != null)
			{
				pawn = recipient.relations.GetFirstDirectRelationPawn(PawnRelationDefOf.Fiance, null);
				num2 = 0.1f;
			}
			else if (recipient.GetSpouse() != null && !recipient.GetSpouse().Dead)
			{
				pawn = recipient.GetSpouse();
				num2 = 0.3f;
			}
			if (pawn != null)
			{
				num2 *= Mathf.InverseLerp(100f, 0f, (float)recipient.relations.OpinionOf(pawn));
				num2 *= Mathf.Clamp01((float)(1.0 - recipient.relations.SecondaryRomanceChanceFactor(pawn)));
			}
			num *= num2;
			return Mathf.Clamp01(num);
		}

		public override void Interacted(Pawn initiator, Pawn recipient, List<RulePackDef> extraSentencePacks)
		{
			if (Rand.Value < this.SuccessChance(initiator, recipient))
			{
				List<Pawn> list = default(List<Pawn>);
				this.BreakLoverAndFianceRelations(initiator, out list);
				List<Pawn> list2 = default(List<Pawn>);
				this.BreakLoverAndFianceRelations(recipient, out list2);
				for (int i = 0; i < list.Count; i++)
				{
					this.TryAddCheaterThought(list[i], initiator);
				}
				for (int j = 0; j < list2.Count; j++)
				{
					this.TryAddCheaterThought(list2[j], recipient);
				}
				initiator.relations.TryRemoveDirectRelation(PawnRelationDefOf.ExLover, recipient);
				initiator.relations.AddDirectRelation(PawnRelationDefOf.Lover, recipient);
				TaleRecorder.RecordTale(TaleDefOf.BecameLover, initiator, recipient);
				initiator.needs.mood.thoughts.memories.RemoveMemoriesOfDefWhereOtherPawnIs(ThoughtDefOf.BrokeUpWithMe, recipient);
				recipient.needs.mood.thoughts.memories.RemoveMemoriesOfDefWhereOtherPawnIs(ThoughtDefOf.BrokeUpWithMe, initiator);
				initiator.needs.mood.thoughts.memories.RemoveMemoriesOfDefWhereOtherPawnIs(ThoughtDefOf.FailedRomanceAttemptOnMe, recipient);
				initiator.needs.mood.thoughts.memories.RemoveMemoriesOfDefWhereOtherPawnIs(ThoughtDefOf.FailedRomanceAttemptOnMeLowOpinionMood, recipient);
				recipient.needs.mood.thoughts.memories.RemoveMemoriesOfDefWhereOtherPawnIs(ThoughtDefOf.FailedRomanceAttemptOnMe, initiator);
				recipient.needs.mood.thoughts.memories.RemoveMemoriesOfDefWhereOtherPawnIs(ThoughtDefOf.FailedRomanceAttemptOnMeLowOpinionMood, initiator);
				if (initiator.IsColonist || recipient.IsColonist)
				{
					this.SendNewLoversLetter(initiator, recipient, list, list2);
				}
				extraSentencePacks.Add(RulePackDefOf.Sentence_RomanceAttemptAccepted);
				LovePartnerRelationUtility.TryToShareBed(initiator, recipient);
			}
			else
			{
				initiator.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.RebuffedMyRomanceAttempt, recipient);
				recipient.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.FailedRomanceAttemptOnMe, initiator);
				if (recipient.relations.OpinionOf(initiator) <= 0)
				{
					recipient.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.FailedRomanceAttemptOnMeLowOpinionMood, initiator);
				}
				extraSentencePacks.Add(RulePackDefOf.Sentence_RomanceAttemptRejected);
			}
		}

		private void BreakLoverAndFianceRelations(Pawn pawn, out List<Pawn> oldLoversAndFiances)
		{
			oldLoversAndFiances = new List<Pawn>();
			goto IL_0008;
			IL_0008:
			while (true)
			{
				Pawn firstDirectRelationPawn = pawn.relations.GetFirstDirectRelationPawn(PawnRelationDefOf.Lover, null);
				if (firstDirectRelationPawn != null)
				{
					pawn.relations.RemoveDirectRelation(PawnRelationDefOf.Lover, firstDirectRelationPawn);
					pawn.relations.AddDirectRelation(PawnRelationDefOf.ExLover, firstDirectRelationPawn);
					oldLoversAndFiances.Add(firstDirectRelationPawn);
				}
				else
				{
					Pawn firstDirectRelationPawn2 = pawn.relations.GetFirstDirectRelationPawn(PawnRelationDefOf.Fiance, null);
					if (firstDirectRelationPawn2 == null)
						break;
					pawn.relations.RemoveDirectRelation(PawnRelationDefOf.Fiance, firstDirectRelationPawn2);
					pawn.relations.AddDirectRelation(PawnRelationDefOf.ExLover, firstDirectRelationPawn2);
					oldLoversAndFiances.Add(firstDirectRelationPawn2);
				}
			}
			return;
			IL_009f:
			goto IL_0008;
		}

		private void TryAddCheaterThought(Pawn pawn, Pawn cheater)
		{
			if (!pawn.Dead)
			{
				pawn.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.CheatedOnMe, cheater);
			}
		}

		private void SendNewLoversLetter(Pawn initiator, Pawn recipient, List<Pawn> initiatorOldLoversAndFiances, List<Pawn> recipientOldLoversAndFiances)
		{
			bool flag = false;
			if (initiator.GetSpouse() != null && !initiator.GetSpouse().Dead)
			{
				goto IL_0039;
			}
			if (recipient.GetSpouse() != null && !recipient.GetSpouse().Dead)
				goto IL_0039;
			string label = "LetterLabelNewLovers".Translate();
			LetterDef textLetterDef = LetterDefOf.PositiveEvent;
			Pawn t = initiator;
			goto IL_008c;
			IL_0039:
			label = "LetterLabelAffair".Translate();
			textLetterDef = LetterDefOf.NegativeEvent;
			t = ((initiator.GetSpouse() == null || initiator.GetSpouse().Dead) ? recipient : initiator);
			flag = true;
			goto IL_008c;
			IL_008c:
			StringBuilder stringBuilder = new StringBuilder();
			if (flag)
			{
				if (initiator.GetSpouse() != null)
				{
					stringBuilder.AppendLine("LetterAffair".Translate(initiator.LabelShort, initiator.GetSpouse().LabelShort, recipient.LabelShort));
				}
				if (recipient.GetSpouse() != null)
				{
					if (stringBuilder.Length != 0)
					{
						stringBuilder.AppendLine();
					}
					stringBuilder.AppendLine("LetterAffair".Translate(recipient.LabelShort, recipient.GetSpouse().LabelShort, initiator.LabelShort));
				}
			}
			else
			{
				stringBuilder.AppendLine("LetterNewLovers".Translate(initiator.LabelShort, recipient.LabelShort));
			}
			for (int i = 0; i < initiatorOldLoversAndFiances.Count; i++)
			{
				if (!initiatorOldLoversAndFiances[i].Dead)
				{
					stringBuilder.AppendLine();
					stringBuilder.AppendLine("LetterNoLongerLovers".Translate(initiator.LabelShort, initiatorOldLoversAndFiances[i].LabelShort));
				}
			}
			for (int j = 0; j < recipientOldLoversAndFiances.Count; j++)
			{
				if (!recipientOldLoversAndFiances[j].Dead)
				{
					stringBuilder.AppendLine();
					stringBuilder.AppendLine("LetterNoLongerLovers".Translate(recipient.LabelShort, recipientOldLoversAndFiances[j].LabelShort));
				}
			}
			Find.LetterStack.ReceiveLetter(label, stringBuilder.ToString().TrimEndNewlines(), textLetterDef, (Thing)t, (string)null);
		}
	}
}
