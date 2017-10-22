using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	internal class Recipe_RemoveBodyPart : Recipe_Surgery
	{
		private const float ViolationGoodwillImpact = 20f;

		public override IEnumerable<BodyPartRecord> GetPartsToApplyOn(Pawn pawn, RecipeDef recipe)
		{
			IEnumerable<BodyPartRecord> parts = pawn.health.hediffSet.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined);
			foreach (BodyPartRecord item in parts)
			{
				if (pawn.health.hediffSet.HasDirectlyAddedPartFor(item))
				{
					yield return item;
				}
				else if (MedicalRecipesUtility.IsCleanAndDroppable(pawn, item))
				{
					yield return item;
				}
				else if (item != pawn.RaceProps.body.corePart && !item.def.dontSuggestAmputation && pawn.health.hediffSet.hediffs.Any((Predicate<Hediff>)((Hediff d) => !(d is Hediff_Injury) && d.def.isBad && d.Visible && d.Part == ((_003CGetPartsToApplyOn_003Ec__IteratorC4)/*Error near IL_0144: stateMachine*/)._003Cpart_003E__2)))
				{
					yield return item;
				}
			}
		}

		public override bool IsViolationOnPawn(Pawn pawn, BodyPartRecord part, Faction billDoerFaction)
		{
			if (pawn.Faction == billDoerFaction)
			{
				return false;
			}
			if (HealthUtility.PartRemovalIntent(pawn, part) == BodyPartRemovalIntent.Harvest)
			{
				return true;
			}
			return false;
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
				TaleRecorder.RecordTale(TaleDefOf.DidSurgery, billDoer, pawn);
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
			if (!pawn.RaceProps.IsMechanoid && !pawn.health.hediffSet.PartOrAnyAncestorHasDirectlyAddedParts(part))
			{
				switch (HealthUtility.PartRemovalIntent(pawn, part))
				{
				case BodyPartRemovalIntent.Amputate:
				{
					if (part.depth != BodyPartDepth.Inside && !part.def.useDestroyedOutLabel)
					{
						return "Amputate".Translate();
					}
					return "RemoveOrgan".Translate();
				}
				case BodyPartRemovalIntent.Harvest:
				{
					return "Harvest".Translate();
				}
				default:
				{
					throw new InvalidOperationException();
				}
				}
			}
			return RecipeDefOf.RemoveBodyPart.LabelCap;
		}
	}
}
