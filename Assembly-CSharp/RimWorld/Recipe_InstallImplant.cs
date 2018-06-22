using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000466 RID: 1126
	public class Recipe_InstallImplant : Recipe_Surgery
	{
		// Token: 0x060013CF RID: 5071 RVA: 0x000AC824 File Offset: 0x000AAC24
		public override IEnumerable<BodyPartRecord> GetPartsToApplyOn(Pawn pawn, RecipeDef recipe)
		{
			for (int i = 0; i < recipe.appliedOnFixedBodyParts.Count; i++)
			{
				BodyPartDef part = recipe.appliedOnFixedBodyParts[i];
				List<BodyPartRecord> bpList = pawn.RaceProps.body.AllParts;
				for (int j = 0; j < bpList.Count; j++)
				{
					BodyPartRecord record = bpList[j];
					if (record.def == part)
					{
						if (pawn.health.hediffSet.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined, null).Contains(record))
						{
							if (!pawn.health.hediffSet.PartOrAnyAncestorHasDirectlyAddedParts(record))
							{
								if (!pawn.health.hediffSet.hediffs.Any((Hediff x) => x.Part == record && x.def == recipe.addsHediff))
								{
									yield return record;
								}
							}
						}
					}
				}
			}
			yield break;
		}

		// Token: 0x060013D0 RID: 5072 RVA: 0x000AC858 File Offset: 0x000AAC58
		public override void ApplyOnPawn(Pawn pawn, BodyPartRecord part, Pawn billDoer, List<Thing> ingredients, Bill bill)
		{
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
			}
			pawn.health.AddHediff(this.recipe.addsHediff, part, null, null);
		}
	}
}
