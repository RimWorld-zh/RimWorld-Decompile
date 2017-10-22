using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	public class Recipe_InstallImplant : Recipe_Surgery
	{
		public override IEnumerable<BodyPartRecord> GetPartsToApplyOn(Pawn pawn, RecipeDef recipe)
		{
			_003CGetPartsToApplyOn_003Ec__Iterator0 _003CGetPartsToApplyOn_003Ec__Iterator = (_003CGetPartsToApplyOn_003Ec__Iterator0)/*Error near IL_0032: stateMachine*/;
			for (int j = 0; j < recipe.appliedOnFixedBodyParts.Count; j++)
			{
				BodyPartDef part = recipe.appliedOnFixedBodyParts[j];
				List<BodyPartRecord> bpList = pawn.RaceProps.body.AllParts;
				for (int i = 0; i < bpList.Count; i++)
				{
					_003CGetPartsToApplyOn_003Ec__Iterator0 _003CGetPartsToApplyOn_003Ec__Iterator2 = (_003CGetPartsToApplyOn_003Ec__Iterator0)/*Error near IL_00b0: stateMachine*/;
					BodyPartRecord record = bpList[i];
					if (record.def == part && pawn.health.hediffSet.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined).Contains(record) && !pawn.health.hediffSet.PartOrAnyAncestorHasDirectlyAddedParts(record) && !pawn.health.hediffSet.hediffs.Any((Predicate<Hediff>)((Hediff x) => x.Part == record && x.def == recipe.addsHediff)))
					{
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
				if (base.CheckSurgeryFail(billDoer, pawn, ingredients, part, bill))
				{
					return;
				}
				TaleRecorder.RecordTale(TaleDefOf.DidSurgery, billDoer, pawn);
			}
			pawn.health.AddHediff(base.recipe.addsHediff, part, default(DamageInfo?));
		}
	}
}
