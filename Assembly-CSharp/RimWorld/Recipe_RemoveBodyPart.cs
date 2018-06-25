using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x0200046B RID: 1131
	internal class Recipe_RemoveBodyPart : Recipe_Surgery
	{
		// Token: 0x060013DB RID: 5083 RVA: 0x000AD0F4 File Offset: 0x000AB4F4
		public override IEnumerable<BodyPartRecord> GetPartsToApplyOn(Pawn pawn, RecipeDef recipe)
		{
			IEnumerable<BodyPartRecord> parts = pawn.health.hediffSet.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined, null);
			using (IEnumerator<BodyPartRecord> enumerator = parts.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					BodyPartRecord part = enumerator.Current;
					if (pawn.health.hediffSet.HasDirectlyAddedPartFor(part))
					{
						yield return part;
					}
					else if (MedicalRecipesUtility.IsCleanAndDroppable(pawn, part))
					{
						yield return part;
					}
					else if (part != pawn.RaceProps.body.corePart && part.def.canSuggestAmputation && pawn.health.hediffSet.hediffs.Any((Hediff d) => !(d is Hediff_Injury) && d.def.isBad && d.Visible && d.Part == part))
					{
						yield return part;
					}
				}
			}
			yield break;
		}

		// Token: 0x060013DC RID: 5084 RVA: 0x000AD120 File Offset: 0x000AB520
		public override bool IsViolationOnPawn(Pawn pawn, BodyPartRecord part, Faction billDoerFaction)
		{
			return pawn.Faction != billDoerFaction && pawn.Faction != null && HealthUtility.PartRemovalIntent(pawn, part) == BodyPartRemovalIntent.Harvest;
		}

		// Token: 0x060013DD RID: 5085 RVA: 0x000AD168 File Offset: 0x000AB568
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
				TaleRecorder.RecordTale(TaleDefOf.DidSurgery, new object[]
				{
					billDoer,
					pawn
				});
				MedicalRecipesUtility.SpawnNaturalPartIfClean(pawn, part, billDoer.Position, billDoer.Map);
				MedicalRecipesUtility.SpawnThingsFromHediffs(pawn, part, billDoer.Position, billDoer.Map);
			}
			DamageDef surgicalCut = DamageDefOf.SurgicalCut;
			float amount = 99999f;
			pawn.TakeDamage(new DamageInfo(surgicalCut, amount, -1f, null, part, null, DamageInfo.SourceCategory.ThingOrUnknown, null));
			if (flag)
			{
				if (pawn.Dead)
				{
					ThoughtUtility.GiveThoughtsForPawnExecuted(pawn, PawnExecutionKind.OrganHarvesting);
				}
				ThoughtUtility.GiveThoughtsForPawnOrganHarvested(pawn);
			}
			if (flag2 && pawn.Faction != null && billDoer.Faction != null)
			{
				Faction faction = pawn.Faction;
				Faction faction2 = billDoer.Faction;
				int goodwillChange = -15;
				string reason = "GoodwillChangedReason_RemovedBodyPart".Translate(new object[]
				{
					part.LabelShort
				});
				GlobalTargetInfo? lookTarget = new GlobalTargetInfo?(pawn);
				faction.TryAffectGoodwillWith(faction2, goodwillChange, true, true, reason, lookTarget);
			}
		}

		// Token: 0x060013DE RID: 5086 RVA: 0x000AD29C File Offset: 0x000AB69C
		public override string GetLabelWhenUsedOn(Pawn pawn, BodyPartRecord part)
		{
			string result;
			if (pawn.RaceProps.IsMechanoid || pawn.health.hediffSet.PartOrAnyAncestorHasDirectlyAddedParts(part))
			{
				result = RecipeDefOf.RemoveBodyPart.label;
			}
			else
			{
				BodyPartRemovalIntent bodyPartRemovalIntent = HealthUtility.PartRemovalIntent(pawn, part);
				if (bodyPartRemovalIntent != BodyPartRemovalIntent.Amputate)
				{
					if (bodyPartRemovalIntent != BodyPartRemovalIntent.Harvest)
					{
						throw new InvalidOperationException();
					}
					result = "HarvestOrgan".Translate();
				}
				else if (part.depth == BodyPartDepth.Inside || part.def.socketed)
				{
					result = "RemoveOrgan".Translate();
				}
				else
				{
					result = "Amputate".Translate();
				}
			}
			return result;
		}
	}
}
