using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000465 RID: 1125
	public class Recipe_InstallArtificialBodyPart : Recipe_Surgery
	{
		// Token: 0x060013CC RID: 5068 RVA: 0x000AC400 File Offset: 0x000AA800
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
						IEnumerable<Hediff> diffs = from x in pawn.health.hediffSet.hediffs
						where x.Part == record
						select x;
						if (diffs.Count<Hediff>() != 1 || diffs.First<Hediff>().def != recipe.addsHediff)
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

		// Token: 0x060013CD RID: 5069 RVA: 0x000AC434 File Offset: 0x000AA834
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
				MedicalRecipesUtility.RestorePartAndSpawnAllPreviousParts(pawn, part, billDoer.Position, billDoer.Map);
			}
			else if (pawn.Map != null)
			{
				MedicalRecipesUtility.RestorePartAndSpawnAllPreviousParts(pawn, part, pawn.Position, pawn.Map);
			}
			else
			{
				pawn.health.RestorePart(part, null, true);
			}
			pawn.health.AddHediff(this.recipe.addsHediff, part, null, null);
		}
	}
}
