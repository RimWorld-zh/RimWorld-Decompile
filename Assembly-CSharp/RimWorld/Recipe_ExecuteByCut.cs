using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200046A RID: 1130
	public class Recipe_ExecuteByCut : RecipeWorker
	{
		// Token: 0x060013D8 RID: 5080 RVA: 0x000AD2DA File Offset: 0x000AB6DA
		public override void ApplyOnPawn(Pawn pawn, BodyPartRecord part, Pawn billDoer, List<Thing> ingredients, Bill bill)
		{
			ExecutionUtility.DoExecutionByCut(billDoer, pawn);
			ThoughtUtility.GiveThoughtsForPawnExecuted(pawn, PawnExecutionKind.GenericHumane);
		}
	}
}
