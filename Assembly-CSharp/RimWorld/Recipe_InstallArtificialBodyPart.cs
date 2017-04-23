using System;
using System.Collections.Generic;
using System.Diagnostics;
using Verse;

namespace RimWorld
{
	public class Recipe_InstallArtificialBodyPart : Recipe_Surgery
	{
		[DebuggerHidden]
		public override IEnumerable<BodyPartRecord> GetPartsToApplyOn(Pawn pawn, RecipeDef recipe)
		{
			Recipe_InstallArtificialBodyPart.<GetPartsToApplyOn>c__IteratorBF <GetPartsToApplyOn>c__IteratorBF = new Recipe_InstallArtificialBodyPart.<GetPartsToApplyOn>c__IteratorBF();
			<GetPartsToApplyOn>c__IteratorBF.recipe = recipe;
			<GetPartsToApplyOn>c__IteratorBF.pawn = pawn;
			<GetPartsToApplyOn>c__IteratorBF.<$>recipe = recipe;
			<GetPartsToApplyOn>c__IteratorBF.<$>pawn = pawn;
			Recipe_InstallArtificialBodyPart.<GetPartsToApplyOn>c__IteratorBF expr_23 = <GetPartsToApplyOn>c__IteratorBF;
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
			else if (pawn.Map != null)
			{
				MedicalRecipesUtility.RestorePartAndSpawnAllPreviousParts(pawn, part, pawn.Position, pawn.Map);
			}
			else
			{
				pawn.health.RestorePart(part, null, true);
			}
			pawn.health.AddHediff(this.recipe.addsHediff, part, null);
		}
	}
}
