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
			int i = 0;
			while (true)
			{
				if (i < allHediffs.Count)
				{
					if (allHediffs[i].Part != null && allHediffs[i].def == recipe.removesHediff && allHediffs[i].Visible)
						break;
					i++;
					continue;
				}
				yield break;
			}
			yield return allHediffs[i].Part;
			/*Error: Unable to find new state assignment for yield return*/;
		}

		public override void ApplyOnPawn(Pawn pawn, BodyPartRecord part, Pawn billDoer, List<Thing> ingredients, Bill bill)
		{
			if (billDoer != null)
			{
				if (base.CheckSurgeryFail(billDoer, pawn, ingredients, part, bill))
				{
					return;
				}
				TaleRecorder.RecordTale(TaleDefOf.DidSurgery, billDoer, pawn);
				if (PawnUtility.ShouldSendNotificationAbout(pawn) || PawnUtility.ShouldSendNotificationAbout(billDoer))
				{
					string text = base.recipe.successfullyRemovedHediffMessage.NullOrEmpty() ? "MessageSuccessfullyRemovedHediff".Translate(billDoer.LabelShort, pawn.LabelShort, base.recipe.removesHediff.label) : string.Format(base.recipe.successfullyRemovedHediffMessage, billDoer.LabelShort, pawn.LabelShort);
					Messages.Message(text, (Thing)pawn, MessageTypeDefOf.PositiveEvent);
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
