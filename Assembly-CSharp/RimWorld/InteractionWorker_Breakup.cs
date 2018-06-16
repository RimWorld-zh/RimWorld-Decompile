using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020004B3 RID: 1203
	public class InteractionWorker_Breakup : InteractionWorker
	{
		// Token: 0x06001577 RID: 5495 RVA: 0x000BE728 File Offset: 0x000BCB28
		public override float RandomSelectionWeight(Pawn initiator, Pawn recipient)
		{
			float result;
			if (!LovePartnerRelationUtility.LovePartnerRelationExists(initiator, recipient))
			{
				result = 0f;
			}
			else
			{
				float num = Mathf.InverseLerp(100f, -100f, (float)initiator.relations.OpinionOf(recipient));
				float num2 = 1f;
				if (initiator.relations.DirectRelationExists(PawnRelationDefOf.Spouse, recipient))
				{
					num2 = 0.4f;
				}
				result = 0.02f * num * num2;
			}
			return result;
		}

		// Token: 0x06001578 RID: 5496 RVA: 0x000BE79C File Offset: 0x000BCB9C
		public Thought RandomBreakupReason(Pawn initiator, Pawn recipient)
		{
			List<Thought_Memory> list = (from m in initiator.needs.mood.thoughts.memories.Memories
			where m != null && m.otherPawn == recipient && m.CurStage != null && m.CurStage.baseOpinionOffset < 0f
			select m).ToList<Thought_Memory>();
			Thought result;
			if (list.Count == 0)
			{
				result = null;
			}
			else
			{
				float worstMemoryOpinionOffset = list.Max((Thought_Memory m) => -m.CurStage.baseOpinionOffset);
				Thought_Memory thought_Memory = null;
				(from m in list
				where -m.CurStage.baseOpinionOffset >= worstMemoryOpinionOffset / 2f
				select m).TryRandomElementByWeight((Thought_Memory m) => -m.CurStage.baseOpinionOffset, out thought_Memory);
				result = thought_Memory;
			}
			return result;
		}

		// Token: 0x06001579 RID: 5497 RVA: 0x000BE864 File Offset: 0x000BCC64
		public override void Interacted(Pawn initiator, Pawn recipient, List<RulePackDef> extraSentencePacks, out string letterText, out string letterLabel, out LetterDef letterDef)
		{
			Thought thought = this.RandomBreakupReason(initiator, recipient);
			if (initiator.relations.DirectRelationExists(PawnRelationDefOf.Spouse, recipient))
			{
				initiator.relations.RemoveDirectRelation(PawnRelationDefOf.Spouse, recipient);
				initiator.relations.AddDirectRelation(PawnRelationDefOf.ExSpouse, recipient);
				recipient.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.DivorcedMe, initiator);
				initiator.needs.mood.thoughts.memories.RemoveMemoriesOfDef(ThoughtDefOf.GotMarried);
				recipient.needs.mood.thoughts.memories.RemoveMemoriesOfDef(ThoughtDefOf.GotMarried);
				initiator.needs.mood.thoughts.memories.RemoveMemoriesOfDefWhereOtherPawnIs(ThoughtDefOf.HoneymoonPhase, recipient);
				recipient.needs.mood.thoughts.memories.RemoveMemoriesOfDefWhereOtherPawnIs(ThoughtDefOf.HoneymoonPhase, initiator);
			}
			else
			{
				initiator.relations.TryRemoveDirectRelation(PawnRelationDefOf.Lover, recipient);
				initiator.relations.TryRemoveDirectRelation(PawnRelationDefOf.Fiance, recipient);
				initiator.relations.AddDirectRelation(PawnRelationDefOf.ExLover, recipient);
				recipient.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.BrokeUpWithMe, initiator);
			}
			if (initiator.ownership.OwnedBed != null && initiator.ownership.OwnedBed == recipient.ownership.OwnedBed)
			{
				Pawn pawn = (Rand.Value >= 0.5f) ? recipient : initiator;
				pawn.ownership.UnclaimBed();
			}
			TaleRecorder.RecordTale(TaleDefOf.Breakup, new object[]
			{
				initiator,
				recipient
			});
			if (PawnUtility.ShouldSendNotificationAbout(initiator) || PawnUtility.ShouldSendNotificationAbout(recipient))
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendLine("LetterNoLongerLovers".Translate(new object[]
				{
					initiator.LabelShort,
					recipient.LabelShort
				}));
				if (thought != null)
				{
					stringBuilder.AppendLine();
					stringBuilder.AppendLine("FinalStraw".Translate(new object[]
					{
						thought.CurStage.label.CapitalizeFirst()
					}));
				}
				letterLabel = "LetterLabelBreakup".Translate();
				letterText = stringBuilder.ToString();
				letterDef = LetterDefOf.NegativeEvent;
			}
			else
			{
				letterLabel = null;
				letterText = null;
				letterDef = null;
			}
		}

		// Token: 0x04000CAB RID: 3243
		private const float BaseChance = 0.02f;

		// Token: 0x04000CAC RID: 3244
		private const float SpouseRelationChanceFactor = 0.4f;
	}
}
