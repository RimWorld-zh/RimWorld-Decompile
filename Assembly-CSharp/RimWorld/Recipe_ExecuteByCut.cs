using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200046A RID: 1130
	public class Recipe_ExecuteByCut : RecipeWorker
	{
		// Token: 0x060013D9 RID: 5081 RVA: 0x000AD0DA File Offset: 0x000AB4DA
		public override void ApplyOnPawn(Pawn pawn, BodyPartRecord part, Pawn billDoer, List<Thing> ingredients, Bill bill)
		{
			ExecutionUtility.DoExecutionByCut(billDoer, pawn);
			ThoughtUtility.GiveThoughtsForPawnExecuted(pawn, PawnExecutionKind.GenericHumane);
		}
	}
}
