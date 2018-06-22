using System;
using System.Collections.Generic;
using UnityEngine.Profiling;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020000AF RID: 175
	public class JobGiver_SatisfyChemicalNeed : ThinkNode_JobGiver
	{
		// Token: 0x06000437 RID: 1079 RVA: 0x00032098 File Offset: 0x00030498
		public override float GetPriority(Pawn pawn)
		{
			float result;
			if (pawn.needs.AllNeeds.Any((Need x) => this.ShouldSatisfy(x)))
			{
				result = 9.25f;
			}
			else
			{
				result = 0f;
			}
			return result;
		}

		// Token: 0x06000438 RID: 1080 RVA: 0x000320E0 File Offset: 0x000304E0
		protected override Job TryGiveJob(Pawn pawn)
		{
			Profiler.BeginSample("SatisfyChemicalNeed");
			JobGiver_SatisfyChemicalNeed.tmpChemicalNeeds.Clear();
			List<Need> allNeeds = pawn.needs.AllNeeds;
			for (int i = 0; i < allNeeds.Count; i++)
			{
				if (this.ShouldSatisfy(allNeeds[i]))
				{
					JobGiver_SatisfyChemicalNeed.tmpChemicalNeeds.Add((Need_Chemical)allNeeds[i]);
				}
			}
			Job result;
			if (!JobGiver_SatisfyChemicalNeed.tmpChemicalNeeds.Any<Need_Chemical>())
			{
				Profiler.EndSample();
				result = null;
			}
			else
			{
				JobGiver_SatisfyChemicalNeed.tmpChemicalNeeds.SortBy((Need_Chemical x) => x.CurLevel);
				for (int j = 0; j < JobGiver_SatisfyChemicalNeed.tmpChemicalNeeds.Count; j++)
				{
					Thing thing = this.FindDrugFor(pawn, JobGiver_SatisfyChemicalNeed.tmpChemicalNeeds[j]);
					if (thing != null)
					{
						JobGiver_SatisfyChemicalNeed.tmpChemicalNeeds.Clear();
						Profiler.EndSample();
						return DrugAIUtility.IngestAndTakeToInventoryJob(thing, pawn, 1);
					}
				}
				JobGiver_SatisfyChemicalNeed.tmpChemicalNeeds.Clear();
				Profiler.EndSample();
				result = null;
			}
			return result;
		}

		// Token: 0x06000439 RID: 1081 RVA: 0x00032200 File Offset: 0x00030600
		private bool ShouldSatisfy(Need need)
		{
			Need_Chemical need_Chemical = need as Need_Chemical;
			return need_Chemical != null && need_Chemical.CurCategory <= DrugDesireCategory.Desire;
		}

		// Token: 0x0600043A RID: 1082 RVA: 0x00032238 File Offset: 0x00030638
		private Thing FindDrugFor(Pawn pawn, Need_Chemical need)
		{
			Hediff_Addiction addictionHediff = need.AddictionHediff;
			Thing result;
			if (addictionHediff == null)
			{
				result = null;
			}
			else
			{
				ThingOwner<Thing> innerContainer = pawn.inventory.innerContainer;
				for (int i = 0; i < innerContainer.Count; i++)
				{
					if (this.DrugValidator(pawn, addictionHediff, innerContainer[i]))
					{
						return innerContainer[i];
					}
				}
				result = GenClosest.ClosestThingReachable(pawn.Position, pawn.Map, ThingRequest.ForGroup(ThingRequestGroup.Drug), PathEndMode.ClosestTouch, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false), 9999f, (Thing x) => this.DrugValidator(pawn, addictionHediff, x), null, 0, -1, false, RegionType.Set_Passable, false);
			}
			return result;
		}

		// Token: 0x0600043B RID: 1083 RVA: 0x0003231C File Offset: 0x0003071C
		private bool DrugValidator(Pawn pawn, Hediff_Addiction addiction, Thing drug)
		{
			bool result;
			if (!drug.def.IsDrug)
			{
				result = false;
			}
			else
			{
				if (drug.Spawned)
				{
					if (drug.IsForbidden(pawn))
					{
						return false;
					}
					if (!pawn.CanReserve(drug, 1, -1, null, false))
					{
						return false;
					}
					if (!drug.IsSociallyProper(pawn))
					{
						return false;
					}
				}
				CompDrug compDrug = drug.TryGetComp<CompDrug>();
				if (compDrug == null || compDrug.Props.chemical == null)
				{
					result = false;
				}
				else if (compDrug.Props.chemical.addictionHediff != addiction.def)
				{
					result = false;
				}
				else
				{
					if (pawn.drugs != null && !pawn.drugs.CurrentPolicy[drug.def].allowedForAddiction && pawn.story != null)
					{
						int num = pawn.story.traits.DegreeOfTrait(TraitDefOf.DrugDesire);
						if (num <= 0 && (!pawn.InMentalState || !pawn.MentalStateDef.ignoreDrugPolicy))
						{
							return false;
						}
					}
					result = true;
				}
			}
			return result;
		}

		// Token: 0x04000282 RID: 642
		private static List<Need_Chemical> tmpChemicalNeeds = new List<Need_Chemical>();
	}
}
