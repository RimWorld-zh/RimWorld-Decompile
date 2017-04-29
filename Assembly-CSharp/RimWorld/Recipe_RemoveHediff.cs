using System;
using System.Collections.Generic;
using System.Diagnostics;
using Verse;

namespace RimWorld
{
	public class Recipe_RemoveHediff : Recipe_Surgery
	{
		[DebuggerHidden]
		public override IEnumerable<BodyPartRecord> GetPartsToApplyOn(Pawn pawn, RecipeDef recipe)
		{
			Recipe_RemoveHediff.<GetPartsToApplyOn>c__IteratorC5 <GetPartsToApplyOn>c__IteratorC = new Recipe_RemoveHediff.<GetPartsToApplyOn>c__IteratorC5();
			<GetPartsToApplyOn>c__IteratorC.pawn = pawn;
			<GetPartsToApplyOn>c__IteratorC.recipe = recipe;
			<GetPartsToApplyOn>c__IteratorC.<$>pawn = pawn;
			<GetPartsToApplyOn>c__IteratorC.<$>recipe = recipe;
			Recipe_RemoveHediff.<GetPartsToApplyOn>c__IteratorC5 expr_23 = <GetPartsToApplyOn>c__IteratorC;
			expr_23.$PC = -2;
			return expr_23;
		}

		public override void ApplyOnPawn(Pawn pawn, BodyPartRecord part, Pawn billDoer, List<Thing> ingredients)
		{
			if (billDoer != null)
			{
				if (base.CheckSurgeryFail(billDoer, pawn, ingredients, part))
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
					Messages.Message(text, pawn, MessageSound.Benefit);
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
