using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	public class Recipe_InstallNaturalBodyPart : Recipe_Surgery
	{
		public override IEnumerable<BodyPartRecord> GetPartsToApplyOn(Pawn pawn, RecipeDef recipe)
		{
			for (int j = 0; j < recipe.appliedOnFixedBodyParts.Count; j++)
			{
				BodyPartDef recipePart = recipe.appliedOnFixedBodyParts[j];
				List<BodyPartRecord> bpList = pawn.RaceProps.body.AllParts;
				for (int i = 0; i < bpList.Count; i++)
				{
					_003CGetPartsToApplyOn_003Ec__Iterator0 _003CGetPartsToApplyOn_003Ec__Iterator = (_003CGetPartsToApplyOn_003Ec__Iterator0)/*Error near IL_0083: stateMachine*/;
					BodyPartRecord record = bpList[i];
					if (record.def == recipePart && pawn.health.hediffSet.hediffs.Any((Predicate<Hediff>)((Hediff x) => x.Part == record)) && (record.parent == null || pawn.health.hediffSet.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined).Contains(record.parent)) && (!pawn.health.hediffSet.PartOrAnyAncestorHasDirectlyAddedParts(record) || pawn.health.hediffSet.HasDirectlyAddedPartFor(record)))
					{
						yield return record;
						/*Error: Unable to find new state assignment for yield return*/;
					}
				}
			}
		}

		public override void ApplyOnPawn(Pawn pawn, BodyPartRecord part, Pawn billDoer, List<Thing> ingredients, Bill bill)
		{
			if (billDoer != null && !base.CheckSurgeryFail(billDoer, pawn, ingredients, part, bill))
			{
				TaleRecorder.RecordTale(TaleDefOf.DidSurgery, billDoer, pawn);
				MedicalRecipesUtility.RestorePartAndSpawnAllPreviousParts(pawn, part, billDoer.Position, billDoer.Map);
			}
		}
	}
}
