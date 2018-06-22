using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200046A RID: 1130
	public class Recipe_RemoveHediff : Recipe_Surgery
	{
		// Token: 0x060013DC RID: 5084 RVA: 0x000AD5A0 File Offset: 0x000AB9A0
		public override IEnumerable<BodyPartRecord> GetPartsToApplyOn(Pawn pawn, RecipeDef recipe)
		{
			List<Hediff> allHediffs = pawn.health.hediffSet.hediffs;
			for (int i = 0; i < allHediffs.Count; i++)
			{
				if (allHediffs[i].Part != null)
				{
					if (allHediffs[i].def == recipe.removesHediff)
					{
						if (allHediffs[i].Visible)
						{
							yield return allHediffs[i].Part;
						}
					}
				}
			}
			yield break;
		}

		// Token: 0x060013DD RID: 5085 RVA: 0x000AD5D4 File Offset: 0x000AB9D4
		public override void ApplyOnPawn(Pawn pawn, BodyPartRecord part, Pawn billDoer, List<Thing> ingredients, Bill bill)
		{
			if (billDoer != null)
			{
				if (base.CheckSurgeryFail(billDoer, pawn, ingredients, part, bill))
				{
					return;
				}
				TaleRecorder.RecordTale(TaleDefOf.DidSurgery, new object[]
				{
					billDoer,
					pawn
				});
				if (PawnUtility.ShouldSendNotificationAbout(pawn) || PawnUtility.ShouldSendNotificationAbout(billDoer))
				{
					string text;
					if (!this.recipe.successfullyRemovedHediffMessage.NullOrEmpty())
					{
						text = string.Format(this.recipe.successfullyRemovedHediffMessage, billDoer.LabelShort, pawn.LabelShort);
					}
					else
					{
						text = "MessageSuccessfullyRemovedHediff".Translate(new object[]
						{
							billDoer.LabelShort,
							pawn.LabelShort,
							this.recipe.removesHediff.label
						});
					}
					Messages.Message(text, pawn, MessageTypeDefOf.PositiveEvent, true);
				}
			}
			Hediff hediff = pawn.health.hediffSet.hediffs.Find((Hediff x) => x.def == this.recipe.removesHediff && x.Part == part && x.Visible);
			if (hediff != null)
			{
				pawn.health.RemoveHediff(hediff);
			}
		}
	}
}
