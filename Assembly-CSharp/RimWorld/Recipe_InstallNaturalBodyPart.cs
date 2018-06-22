using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000467 RID: 1127
	public class Recipe_InstallNaturalBodyPart : Recipe_Surgery
	{
		// Token: 0x060013D2 RID: 5074 RVA: 0x000ACBF4 File Offset: 0x000AAFF4
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

		// Token: 0x060013D3 RID: 5075 RVA: 0x000ACC28 File Offset: 0x000AB028
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
