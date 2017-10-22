using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	internal class Recipe_ShutDown : RecipeWorker
	{
		public override IEnumerable<BodyPartRecord> GetPartsToApplyOn(Pawn pawn, RecipeDef recipe)
		{
			BodyPartRecord brain = pawn.health.hediffSet.GetBrain();
			if (brain == null)
				yield break;
			yield return brain;
			/*Error: Unable to find new state assignment for yield return*/;
		}

		public override void ApplyOnPawn(Pawn pawn, BodyPartRecord part, Pawn billDoer, List<Thing> ingredients, Bill bill)
		{
			pawn.health.AddHediff(base.recipe.addsHediff, part, default(DamageInfo?));
			ThoughtUtility.GiveThoughtsForPawnExecuted(pawn, PawnExecutionKind.GenericHumane);
		}
	}
}
