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
			for (int j = 0; j < recipe.appliedOnFixedBodyParts.Count; j++)
			{
				BodyPartDef part = recipe.appliedOnFixedBodyParts[j];
				List<BodyPartRecord> bpList = pawn.RaceProps.body.AllParts;
				for (int i = 0; i < bpList.Count; i++)
				{
					BodyPartRecord record = bpList[i];
					if (record.def == part && pawn.health.hediffSet.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined).Contains(record) && !pawn.health.hediffSet.PartOrAnyAncestorHasDirectlyAddedParts(record) && !pawn.health.hediffSet.hediffs.Any((Predicate<Hediff>)((Hediff x) => x.Part == ((_003CGetPartsToApplyOn_003Ec__IteratorC1)/*Error near IL_0108: stateMachine*/)._003Crecord_003E__4 && x.def == ((_003CGetPartsToApplyOn_003Ec__IteratorC1)/*Error near IL_0108: stateMachine*/).recipe.addsHediff)))
					{
						yield return record;
					}
				}
			}
		}

		public override void ApplyOnPawn(Pawn pawn, BodyPartRecord part, Pawn billDoer, List<Thing> ingredients)
		{
			if (billDoer != null)
			{
				if (base.CheckSurgeryFail(billDoer, pawn, ingredients, part))
				{
					return;
				}
				TaleRecorder.RecordTale(TaleDefOf.DidSurgery, billDoer, pawn);
			}
			pawn.health.AddHediff(base.recipe.addsHediff, part, default(DamageInfo?));
		}
	}
}
