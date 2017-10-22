using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	public class Recipe_RemoveHediff : Recipe_Surgery
	{
		public override IEnumerable<BodyPartRecord> GetPartsToApplyOn(Pawn pawn, RecipeDef recipe)
		{
			List<Hediff> allHediffs = pawn.health.hediffSet.hediffs;
			for (int i = 0; i < allHediffs.Count; i++)
			{
				if (allHediffs[i].Part != null && allHediffs[i].def == recipe.removesHediff && allHediffs[i].Visible)
				{
					yield return allHediffs[i].Part;
				}
			}
		}

		public override void ApplyOnPawn(Pawn pawn, BodyPartRecord part, Pawn billDoer, List<Thing> ingredients)
		{
			if (billDoer != null)
			{
				if (base.CheckSurgeryFail(billDoer, pawn, ingredients, part))
				{
					return;
				}
				TaleRecorder.RecordTale(TaleDefOf.DidSurgery, billDoer, pawn);
				if (PawnUtility.ShouldSendNotificationAbout(pawn) || PawnUtility.ShouldSendNotificationAbout(billDoer))
				{
					string text = base.recipe.successfullyRemovedHediffMessage.NullOrEmpty() ? "MessageSuccessfullyRemovedHediff".Translate(billDoer.LabelShort, pawn.LabelShort, base.recipe.removesHediff.label) : string.Format(base.recipe.successfullyRemovedHediffMessage, billDoer.LabelShort, pawn.LabelShort);
					Messages.Message(text, (Thing)pawn, MessageSound.Benefit);
				}
			}
			Hediff hediff = pawn.health.hediffSet.hediffs.Find((Predicate<Hediff>)((Hediff x) => x.def == base.recipe.removesHediff && x.Part == part && x.Visible));
			if (hediff != null)
			{
				pawn.health.RemoveHediff(hediff);
			}
		}
	}
}
