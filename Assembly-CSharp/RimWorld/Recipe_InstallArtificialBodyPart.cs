using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	public class Recipe_InstallArtificialBodyPart : Recipe_Surgery
	{
		public override IEnumerable<BodyPartRecord> GetPartsToApplyOn(Pawn pawn, RecipeDef recipe)
		{
			for (int j = 0; j < recipe.appliedOnFixedBodyParts.Count; j++)
			{
				BodyPartDef part = recipe.appliedOnFixedBodyParts[j];
				List<BodyPartRecord> bpList = pawn.RaceProps.body.AllParts;
				for (int i = 0; i < bpList.Count; i++)
				{
					_003CGetPartsToApplyOn_003Ec__Iterator0 _003CGetPartsToApplyOn_003Ec__Iterator = (_003CGetPartsToApplyOn_003Ec__Iterator0)/*Error near IL_0083: stateMachine*/;
					BodyPartRecord record = bpList[i];
					if (record.def == part)
					{
						IEnumerable<Hediff> diffs = from x in pawn.health.hediffSet.hediffs
						where x.Part == record
						select x;
						if (diffs.Count() == 1 && diffs.First().def == recipe.addsHediff)
						{
							continue;
						}
						if (record.parent != null && !pawn.health.hediffSet.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined).Contains(record.parent))
						{
							continue;
						}
						if (pawn.health.hediffSet.PartOrAnyAncestorHasDirectlyAddedParts(record) && !pawn.health.hediffSet.HasDirectlyAddedPartFor(record))
						{
							continue;
						}
						yield return record;
						/*Error: Unable to find new state assignment for yield return*/;
					}
				}
			}
		}

		public override void ApplyOnPawn(Pawn pawn, BodyPartRecord part, Pawn billDoer, List<Thing> ingredients, Bill bill)
		{
			if (billDoer != null)
			{
				if (!base.CheckSurgeryFail(billDoer, pawn, ingredients, part, bill))
				{
					TaleRecorder.RecordTale(TaleDefOf.DidSurgery, billDoer, pawn);
					MedicalRecipesUtility.RestorePartAndSpawnAllPreviousParts(pawn, part, billDoer.Position, billDoer.Map);
					goto IL_0085;
				}
				return;
			}
			if (pawn.Map != null)
			{
				MedicalRecipesUtility.RestorePartAndSpawnAllPreviousParts(pawn, part, pawn.Position, pawn.Map);
			}
			else
			{
				pawn.health.RestorePart(part, null, true);
			}
			goto IL_0085;
			IL_0085:
			pawn.health.AddHediff(base.recipe.addsHediff, part, default(DamageInfo?));
		}
	}
}
