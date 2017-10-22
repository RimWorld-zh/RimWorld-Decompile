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
			using (IEnumerator<BodyPartRecord> enumerator = parts.GetEnumerator())
			{
				BodyPartRecord part;
				while (true)
				{
					if (enumerator.MoveNext())
					{
						_003CGetPartsToApplyOn_003Ec__Iterator0 _003CGetPartsToApplyOn_003Ec__Iterator = (_003CGetPartsToApplyOn_003Ec__Iterator0)/*Error near IL_0088: stateMachine*/;
						part = enumerator.Current;
						if (pawn.health.hediffSet.HasDirectlyAddedPartFor(part))
						{
							yield return part;
							/*Error: Unable to find new state assignment for yield return*/;
						}
						if (MedicalRecipesUtility.IsCleanAndDroppable(pawn, part))
						{
							yield return part;
							/*Error: Unable to find new state assignment for yield return*/;
						}
						if (part != pawn.RaceProps.body.corePart && !part.def.dontSuggestAmputation && pawn.health.hediffSet.hediffs.Any((Predicate<Hediff>)((Hediff d) => !(d is Hediff_Injury) && d.def.isBad && d.Visible && d.Part == part)))
							break;
						continue;
					}
					yield break;
				}
				yield return part;
				/*Error: Unable to find new state assignment for yield return*/;
			}
			IL_0213:
			/*Error near IL_0214: Unexpected return in MoveNext()*/;
		}

		public override bool IsViolationOnPawn(Pawn pawn, BodyPartRecord part, Faction billDoerFaction)
		{
			return (byte)((pawn.Faction != billDoerFaction) ? ((HealthUtility.PartRemovalIntent(pawn, part) == BodyPartRemovalIntent.Harvest) ? 1 : 0) : 0) != 0;
		}

		public override void ApplyOnPawn(Pawn pawn, BodyPartRecord part, Pawn billDoer, List<Thing> ingredients, Bill bill)
		{
			bool flag = MedicalRecipesUtility.IsClean(pawn, part);
			bool flag2 = this.IsViolationOnPawn(pawn, part, Faction.OfPlayer);
			if (billDoer != null)
			{
				if (base.CheckSurgeryFail(billDoer, pawn, ingredients, part, bill))
				{
					return;
				}
				TaleRecorder.RecordTale(TaleDefOf.DidSurgery, billDoer, pawn);
				MedicalRecipesUtility.SpawnNaturalPartIfClean(pawn, part, billDoer.Position, billDoer.Map);
				MedicalRecipesUtility.SpawnThingsFromHediffs(pawn, part, billDoer.Position, billDoer.Map);
			}
			DamageDef surgicalCut = DamageDefOf.SurgicalCut;
			int amount = 99999;
			pawn.TakeDamage(new DamageInfo(surgicalCut, amount, -1f, null, part, null, DamageInfo.SourceCategory.ThingOrUnknown));
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
			string result;
			if (!pawn.RaceProps.IsMechanoid && !pawn.health.hediffSet.PartOrAnyAncestorHasDirectlyAddedParts(part))
			{
				switch (HealthUtility.PartRemovalIntent(pawn, part))
				{
				case BodyPartRemovalIntent.Amputate:
				{
					result = ((part.depth != BodyPartDepth.Inside && !part.def.useDestroyedOutLabel) ? "Amputate".Translate() : "RemoveOrgan".Translate());
					break;
				}
				case BodyPartRemovalIntent.Harvest:
				{
					result = "Harvest".Translate();
					break;
				}
				default:
				{
					throw new InvalidOperationException();
				}
				}
			}
			else
			{
				result = RecipeDefOf.RemoveBodyPart.LabelCap;
			}
			return result;
		}
	}
}
