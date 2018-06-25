using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02000351 RID: 849
	public class IncidentWorker_QuestDownedRefugee : IncidentWorker
	{
		// Token: 0x040008FD RID: 2301
		private const float NoSitePartChance = 0.3f;

		// Token: 0x040008FE RID: 2302
		private const int MinDistance = 2;

		// Token: 0x040008FF RID: 2303
		private const int MaxDistance = 13;

		// Token: 0x04000900 RID: 2304
		private static readonly string DownedRefugeeQuestThreatTag = "DownedRefugeeQuestThreat";

		// Token: 0x04000901 RID: 2305
		private static readonly IntRange TimeoutDaysRange = new IntRange(7, 15);

		// Token: 0x06000EA5 RID: 3749 RVA: 0x0007BF68 File Offset: 0x0007A368
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			int num;
			Faction faction;
			return base.CanFireNowSub(parms) && this.TryFindTile(out num) && SiteMakerHelper.TryFindRandomFactionFor(SiteCoreDefOf.DownedRefugee, null, out faction, true, null);
		}

		// Token: 0x06000EA6 RID: 3750 RVA: 0x0007BFB0 File Offset: 0x0007A3B0
		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			int tile;
			bool result;
			if (!this.TryFindTile(out tile))
			{
				result = false;
			}
			else
			{
				Site site = SiteMaker.TryMakeSite_SingleSitePart(SiteCoreDefOf.DownedRefugee, (!Rand.Chance(0.3f)) ? IncidentWorker_QuestDownedRefugee.DownedRefugeeQuestThreatTag : null, null, true, null, true);
				if (site == null)
				{
					result = false;
				}
				else
				{
					site.Tile = tile;
					Pawn pawn = DownedRefugeeQuestUtility.GenerateRefugee(tile);
					site.GetComponent<DownedRefugeeComp>().pawn.TryAdd(pawn, true);
					int randomInRange = IncidentWorker_QuestDownedRefugee.TimeoutDaysRange.RandomInRange;
					site.GetComponent<TimeoutComp>().StartTimeout(randomInRange * 60000);
					Find.WorldObjects.Add(site);
					string text = this.def.letterLabel;
					string text2 = string.Format(this.def.letterText.AdjustedFor(pawn, "PAWN"), randomInRange, pawn.ageTracker.AgeBiologicalYears, pawn.story.Title).CapitalizeFirst();
					Pawn mostImportantColonyRelative = PawnRelationUtility.GetMostImportantColonyRelative(pawn);
					if (mostImportantColonyRelative != null)
					{
						PawnRelationDef mostImportantRelation = mostImportantColonyRelative.GetMostImportantRelation(pawn);
						if (mostImportantRelation != null && mostImportantRelation.opinionOffset > 0)
						{
							pawn.relations.relativeInvolvedInRescueQuest = mostImportantColonyRelative;
							text2 = text2 + "\n\n" + "RelatedPawnInvolvedInQuest".Translate(new object[]
							{
								mostImportantColonyRelative.LabelShort,
								mostImportantRelation.GetGenderSpecificLabel(pawn)
							}).AdjustedFor(pawn, "PAWN");
						}
						else
						{
							PawnRelationUtility.TryAppendRelationsWithColonistsInfo(ref text2, pawn);
						}
						text = text + " " + "RelationshipAppendedLetterSuffix".Translate();
					}
					if (pawn.relations != null)
					{
						pawn.relations.everSeenByPlayer = true;
					}
					Find.LetterStack.ReceiveLetter(text, text2, this.def.letterDef, site, null, null);
					result = true;
				}
			}
			return result;
		}

		// Token: 0x06000EA7 RID: 3751 RVA: 0x0007C18C File Offset: 0x0007A58C
		private bool TryFindTile(out int tile)
		{
			return TileFinder.TryFindNewSiteTile(out tile, 2, 13, true, false, -1);
		}
	}
}
