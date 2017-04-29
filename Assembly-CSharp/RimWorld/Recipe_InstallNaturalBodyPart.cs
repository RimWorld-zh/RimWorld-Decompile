using System;
using System.Collections.Generic;
using System.Diagnostics;
using Verse;

namespace RimWorld
{
	public class Recipe_InstallNaturalBodyPart : Recipe_Surgery
	{
		[DebuggerHidden]
		public override IEnumerable<BodyPartRecord> GetPartsToApplyOn(Pawn pawn, RecipeDef recipe)
		{
			Recipe_InstallNaturalBodyPart.<GetPartsToApplyOn>c__IteratorC2 <GetPartsToApplyOn>c__IteratorC = new Recipe_InstallNaturalBodyPart.<GetPartsToApplyOn>c__IteratorC2();
			<GetPartsToApplyOn>c__IteratorC.recipe = recipe;
			<GetPartsToApplyOn>c__IteratorC.pawn = pawn;
			<GetPartsToApplyOn>c__IteratorC.<$>recipe = recipe;
			<GetPartsToApplyOn>c__IteratorC.<$>pawn = pawn;
			Recipe_InstallNaturalBodyPart.<GetPartsToApplyOn>c__IteratorC2 expr_23 = <GetPartsToApplyOn>c__IteratorC;
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
				MedicalRecipesUtility.RestorePartAndSpawnAllPreviousParts(pawn, part, billDoer.Position, billDoer.Map);
			}
		}
	}
}
