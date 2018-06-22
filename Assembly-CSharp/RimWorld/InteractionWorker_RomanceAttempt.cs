using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020004B8 RID: 1208
	public class InteractionWorker_RomanceAttempt : InteractionWorker
	{
		// Token: 0x0600158A RID: 5514 RVA: 0x000BFA58 File Offset: 0x000BDE58
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
				if (num < 0.25f)
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
						float num4 = (!initiator.story.traits.HasTrait(TraitDefOf.Gay)) ? ((initiator.gender != Gender.Female) ? 1f : 0.15f) : 1f;
						float num5 = Mathf.InverseLerp(0.25f, 1f, num);
						float num6 = Mathf.InverseLerp(5f, 100f, (float)num2);
						float num7;
						if (initiator.gender == recipient.gender)
						{
							if (initiator.story.traits.HasTrait(TraitDefOf.Gay) && recipient.story.traits.HasTrait(TraitDefOf.Gay))
							{
								num7 = 1f;
							}
							else
							{
								num7 = 0.15f;
							}
						}
						else if (!initiator.story.traits.HasTrait(TraitDefOf.Gay) && !recipient.story.traits.HasTrait(TraitDefOf.Gay))
						{
							num7 = 1f;
						}
						else
						{
							num7 = 0.15f;
						}
						result = 1.15f * num4 * num5 * num6 * num3 * num7;
					}
				}
			}
			return result;
		}

		// Token: 0x0600158B RID: 5515 RVA: 0x000BFC40 File Offset: 0x000BE040
		public float SuccessChance(Pawn initiator, Pawn recipient)
		{
			float num = 0.6f;
			num *= recipient.relations.SecondaryRomanceChanceFactor(initiator);
			num *= Mathf.InverseLerp(5f, 100f, (float)recipient.relations.OpinionOf(initiator));
			float num2 = 1f;
			Pawn pawn = null;
			if (recipient.relations.GetFirstDirectRelationPawn(PawnRelationDefOf.Lover, (Pawn x) => !x.Dead) != null)
			{
				pawn = recipient.relations.GetFirstDirectRelationPawn(PawnRelationDefOf.Lover, null);
				num2 = 0.6f;
			}
			else if (recipient.relations.GetFirstDirectRelationPawn(PawnRelationDefOf.Fiance, (Pawn x) => !x.Dead) != null)
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
				num2 *= Mathf.Clamp01(1f - recipient.relations.SecondaryRomanceChanceFactor(pawn));
			}
			num *= num2;
			return Mathf.Clamp01(num);
		}

		// Token: 0x0600158C RID: 5516 RVA: 0x000BFDA8 File Offset: 0x000BE1A8
		public override void Interacted(Pawn initiator, Pawn recipient, List<RulePackDef> extraSentencePacks, out string letterText, out string letterLabel, out LetterDef letterDef)
		{
			if (Rand.Value < this.SuccessChance(initiator, recipient))
			{
				List<Pawn> list;
				this.BreakLoverAndFianceRelations(initiator, out list);
				List<Pawn> list2;
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
				TaleRecorder.RecordTale(TaleDefOf.BecameLover, new object[]
				{
					initiator,
					recipient
				});
				initiator.needs.mood.thoughts.memories.RemoveMemoriesOfDefWhereOtherPawnIs(ThoughtDefOf.BrokeUpWithMe, recipient);
				recipient.needs.mood.thoughts.memories.RemoveMemoriesOfDefWhereOtherPawnIs(ThoughtDefOf.BrokeUpWithMe, initiator);
				initiator.needs.mood.thoughts.memories.RemoveMemoriesOfDefWhereOtherPawnIs(ThoughtDefOf.FailedRomanceAttemptOnMe, recipient);
				initiator.needs.mood.thoughts.memories.RemoveMemoriesOfDefWhereOtherPawnIs(ThoughtDefOf.FailedRomanceAttemptOnMeLowOpinionMood, recipient);
				recipient.needs.mood.thoughts.memories.RemoveMemoriesOfDefWhereOtherPawnIs(ThoughtDefOf.FailedRomanceAttemptOnMe, initiator);
				recipient.needs.mood.thoughts.memories.RemoveMemoriesOfDefWhereOtherPawnIs(ThoughtDefOf.FailedRomanceAttemptOnMeLowOpinionMood, initiator);
				if (PawnUtility.ShouldSendNotificationAbout(initiator) || PawnUtility.ShouldSendNotificationAbout(recipient))
				{
					this.GetNewLoversLetter(initiator, recipient, list, list2, out letterText, out letterLabel, out letterDef);
				}
				else
				{
					letterText = null;
					letterLabel = null;
					letterDef = null;
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
				letterText = null;
				letterLabel = null;
				letterDef = null;
			}
		}

		// Token: 0x0600158D RID: 5517 RVA: 0x000C0004 File Offset: 0x000BE404
		private void BreakLoverAndFianceRelations(Pawn pawn, out List<Pawn> oldLoversAndFiances)
		{
			oldLoversAndFiances = new List<Pawn>();
			for (;;)
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
					{
						break;
					}
					pawn.relations.RemoveDirectRelation(PawnRelationDefOf.Fiance, firstDirectRelationPawn2);
					pawn.relations.AddDirectRelation(PawnRelationDefOf.ExLover, firstDirectRelationPawn2);
					oldLoversAndFiances.Add(firstDirectRelationPawn2);
				}
			}
		}

		// Token: 0x0600158E RID: 5518 RVA: 0x000C00B5 File Offset: 0x000BE4B5
		private void TryAddCheaterThought(Pawn pawn, Pawn cheater)
		{
			if (!pawn.Dead)
			{
				pawn.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.CheatedOnMe, cheater);
			}
		}

		// Token: 0x0600158F RID: 5519 RVA: 0x000C00E8 File Offset: 0x000BE4E8
		private void GetNewLoversLetter(Pawn initiator, Pawn recipient, List<Pawn> initiatorOldLoversAndFiances, List<Pawn> recipientOldLoversAndFiances, out string letterText, out string letterLabel, out LetterDef letterDef)
		{
			bool flag = false;
			if ((initiator.GetSpouse() != null && !initiator.GetSpouse().Dead) || (recipient.GetSpouse() != null && !recipient.GetSpouse().Dead))
			{
				letterLabel = "LetterLabelAffair".Translate();
				letterDef = LetterDefOf.NegativeEvent;
				flag = true;
			}
			else
			{
				letterLabel = "LetterLabelNewLovers".Translate();
				letterDef = LetterDefOf.PositiveEvent;
			}
			StringBuilder stringBuilder = new StringBuilder();
			if (flag)
			{
				if (initiator.GetSpouse() != null)
				{
					stringBuilder.AppendLine("LetterAffair".Translate(new object[]
					{
						initiator.LabelShort,
						initiator.GetSpouse().LabelShort,
						recipient.LabelShort
					}));
				}
				if (recipient.GetSpouse() != null)
				{
					if (stringBuilder.Length != 0)
					{
						stringBuilder.AppendLine();
					}
					stringBuilder.AppendLine("LetterAffair".Translate(new object[]
					{
						recipient.LabelShort,
						recipient.GetSpouse().LabelShort,
						initiator.LabelShort
					}));
				}
			}
			for (int i = 0; i < initiatorOldLoversAndFiances.Count; i++)
			{
				if (!initiatorOldLoversAndFiances[i].Dead)
				{
					if (stringBuilder.Length > 0)
					{
						stringBuilder.AppendLine();
					}
					stringBuilder.AppendLine("LetterNoLongerLovers".Translate(new object[]
					{
						initiator.LabelShort,
						initiatorOldLoversAndFiances[i].LabelShort
					}));
				}
			}
			for (int j = 0; j < recipientOldLoversAndFiances.Count; j++)
			{
				if (!recipientOldLoversAndFiances[j].Dead)
				{
					if (stringBuilder.Length > 0)
					{
						stringBuilder.AppendLine();
					}
					stringBuilder.AppendLine("LetterNoLongerLovers".Translate(new object[]
					{
						recipient.LabelShort,
						recipientOldLoversAndFiances[j].LabelShort
					}));
				}
			}
			letterText = stringBuilder.ToString().TrimEndNewlines();
		}

		// Token: 0x04000CBC RID: 3260
		private const float MinAttractionForRomanceAttempt = 0.25f;

		// Token: 0x04000CBD RID: 3261
		private const int MinOpinionForRomanceAttempt = 5;

		// Token: 0x04000CBE RID: 3262
		private const float BaseSelectionWeight = 1.15f;

		// Token: 0x04000CBF RID: 3263
		private const float BaseSuccessChance = 0.6f;
	}
}
