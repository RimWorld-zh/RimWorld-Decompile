using System;
using System.Collections.Generic;
using System.Diagnostics;
using Verse;

namespace RimWorld
{
	internal class Recipe_RemoveBodyPart : Recipe_Surgery
	{
		private const float ViolationGoodwillImpact = 20f;

		[DebuggerHidden]
		public override IEnumerable<BodyPartRecord> GetPartsToApplyOn(Pawn pawn, RecipeDef recipe)
		{
			Recipe_RemoveBodyPart.<GetPartsToApplyOn>c__IteratorC4 <GetPartsToApplyOn>c__IteratorC = new Recipe_RemoveBodyPart.<GetPartsToApplyOn>c__IteratorC4();
			<GetPartsToApplyOn>c__IteratorC.pawn = pawn;
			<GetPartsToApplyOn>c__IteratorC.<$>pawn = pawn;
			Recipe_RemoveBodyPart.<GetPartsToApplyOn>c__IteratorC4 expr_15 = <GetPartsToApplyOn>c__IteratorC;
			expr_15.$PC = -2;
			return expr_15;
		}

		public override bool IsViolationOnPawn(Pawn pawn, BodyPartRecord part, Faction billDoerFaction)
		{
			return pawn.Faction != billDoerFaction && HealthUtility.PartRemovalIntent(pawn, part) == BodyPartRemovalIntent.Harvest;
		}

		public override void ApplyOnPawn(Pawn pawn, BodyPartRecord part, Pawn billDoer, List<Thing> ingredients)
		{
			bool flag = MedicalRecipesUtility.IsClean(pawn, part);
			bool flag2 = this.IsViolationOnPawn(pawn, part, Faction.OfPlayer);
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
				MedicalRecipesUtility.SpawnNaturalPartIfClean(pawn, part, billDoer.Position, billDoer.Map);
				MedicalRecipesUtility.SpawnThingsFromHediffs(pawn, part, billDoer.Position, billDoer.Map);
			}
			pawn.TakeDamage(new DamageInfo(DamageDefOf.SurgicalCut, 99999, -1f, null, part, null, DamageInfo.SourceCategory.ThingOrUnknown));
			if (flag)
			{
				if (pawn.Dead)
				{
					ThoughtUtility.GiveThoughtsForPawnExecuted(pawn, PawnExecutionKind.OrganHarvesting);
				}
				else
				{
					ThoughtUtility.GiveThoughtsForPawnOrganHarvested(pawn);
				}
			}
			if (flag2)
			{
				pawn.Faction.AffectGoodwillWith(billDoer.Faction, -20f);
			}
		}

		public override string GetLabelWhenUsedOn(Pawn pawn, BodyPartRecord part)
		{
			if (pawn.RaceProps.IsMechanoid || pawn.health.hediffSet.PartOrAnyAncestorHasDirectlyAddedParts(part))
			{
				return RecipeDefOf.RemoveBodyPart.LabelCap;
			}
			BodyPartRemovalIntent bodyPartRemovalIntent = HealthUtility.PartRemovalIntent(pawn, part);
			if (bodyPartRemovalIntent == BodyPartRemovalIntent.Harvest)
			{
				return "Harvest".Translate();
			}
			if (bodyPartRemovalIntent != BodyPartRemovalIntent.Amputate)
			{
				throw new InvalidOperationException();
			}
			if (part.depth == BodyPartDepth.Inside)
			{
				return "RemoveOrgan".Translate();
			}
			return "Amputate".Translate();
		}
	}
}
