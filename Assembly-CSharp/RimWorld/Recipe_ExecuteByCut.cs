using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000468 RID: 1128
	public class Recipe_ExecuteByCut : RecipeWorker
	{
		// Token: 0x060013D5 RID: 5077 RVA: 0x000ACF8A File Offset: 0x000AB38A
		public override void ApplyOnPawn(Pawn pawn, BodyPartRecord part, Pawn billDoer, List<Thing> ingredients, Bill bill)
		{
			ExecutionUtility.DoExecutionByCut(billDoer, pawn);
			ThoughtUtility.GiveThoughtsForPawnExecuted(pawn, PawnExecutionKind.GenericHumane);
		}
	}
}
