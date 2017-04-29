using System;
using System.Collections.Generic;
using System.Diagnostics;
using Verse;

namespace RimWorld
{
	internal class Recipe_ShutDown : RecipeWorker
	{
		[DebuggerHidden]
		public override IEnumerable<BodyPartRecord> GetPartsToApplyOn(Pawn pawn, RecipeDef recipe)
		{
			Recipe_ShutDown.<GetPartsToApplyOn>c__IteratorC3 <GetPartsToApplyOn>c__IteratorC = new Recipe_ShutDown.<GetPartsToApplyOn>c__IteratorC3();
			<GetPartsToApplyOn>c__IteratorC.pawn = pawn;
			<GetPartsToApplyOn>c__IteratorC.<$>pawn = pawn;
			Recipe_ShutDown.<GetPartsToApplyOn>c__IteratorC3 expr_15 = <GetPartsToApplyOn>c__IteratorC;
			expr_15.$PC = -2;
			return expr_15;
		}

		public override void ApplyOnPawn(Pawn pawn, BodyPartRecord part, Pawn billDoer, List<Thing> ingredients)
		{
			pawn.health.AddHediff(this.recipe.addsHediff, part, null);
			ThoughtUtility.GiveThoughtsForPawnExecuted(pawn, PawnExecutionKind.GenericHumane);
		}
	}
}
