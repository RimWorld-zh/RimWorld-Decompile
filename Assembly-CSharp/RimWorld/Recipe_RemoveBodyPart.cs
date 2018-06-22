using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02000469 RID: 1129
	internal class Recipe_RemoveBodyPart : Recipe_Surgery
	{
		// Token: 0x060013D7 RID: 5079 RVA: 0x000ACFA4 File Offset: 0x000AB3A4
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

		// Token: 0x060013D8 RID: 5080 RVA: 0x000ACFD0 File Offset: 0x000AB3D0
		public override bool IsViolationOnPawn(Pawn pawn, BodyPartRecord part, Faction billDoerFaction)
		{
			return pawn.Faction != billDoerFaction && pawn.Faction != null && HealthUtility.PartRemovalIntent(pawn, part) == BodyPartRemovalIntent.Harvest;
		}

		// Token: 0x060013D9 RID: 5081 RVA: 0x000AD018 File Offset: 0x000AB418
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

		// Token: 0x060013DA RID: 5082 RVA: 0x000AD14C File Offset: 0x000AB54C
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
