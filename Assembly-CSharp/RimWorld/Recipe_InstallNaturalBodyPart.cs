using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000469 RID: 1129
	public class Recipe_InstallNaturalBodyPart : Recipe_Surgery
	{
		// Token: 0x060013D6 RID: 5078 RVA: 0x000ACD44 File Offset: 0x000AB144
		public override IEnumerable<BodyPartRecord> GetPartsToApplyOn(Pawn pawn, RecipeDef recipe)
		{
			for (int i = 0; i < recipe.appliedOnFixedBodyParts.Count; i++)
			{
				BodyPartDef recipePart = recipe.appliedOnFixedBodyParts[i];
				List<BodyPartRecord> bpList = pawn.RaceProps.body.AllParts;
				for (int j = 0; j < bpList.Count; j++)
				{
					BodyPartRecord record = bpList[j];
					if (record.def == recipePart)
					{
						if (pawn.health.hediffSet.hediffs.Any((Hediff x) => x.Part == record))
						{
							if (record.parent == null || pawn.health.hediffSet.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined, null).Contains(record.parent))
							{
								if (!pawn.health.hediffSet.PartOrAnyAncestorHasDirectlyAddedParts(record) || pawn.health.hediffSet.HasDirectlyAddedPartFor(record))
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

		// Token: 0x060013D7 RID: 5079 RVA: 0x000ACD78 File Offset: 0x000AB178
		public override void ApplyOnPawn(Pawn pawn, BodyPartRecord part, Pawn billDoer, List<Thing> ingredients, Bill bill)
		{
			if (billDoer != null)
			{
				if (!base.CheckSurgeryFail(billDoer, pawn, ingredients, part, bill))
				{
					TaleRecorder.RecordTale(TaleDefOf.DidSurgery, new object[]
					{
						billDoer,
						pawn
					});
					MedicalRecipesUtility.RestorePartAndSpawnAllPreviousParts(pawn, part, billDoer.Position, billDoer.Map);
				}
			}
		}
	}
}
