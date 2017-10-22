#define ENABLE_PROFILER
using System;
using System.Collections.Generic;
using UnityEngine.Profiling;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JobGiver_SatisfyChemicalNeed : ThinkNode_JobGiver
	{
		private static List<Need_Chemical> tmpChemicalNeeds = new List<Need_Chemical>();

		public override float GetPriority(Pawn pawn)
		{
			return (float)((!pawn.needs.AllNeeds.Any((Predicate<Need>)((Need x) => this.ShouldSatisfy(x)))) ? 0.0 : 9.25);
		}

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
			Thing thing;
			if (!JobGiver_SatisfyChemicalNeed.tmpChemicalNeeds.Any())
			{
				Profiler.EndSample();
				result = null;
			}
			else
			{
				JobGiver_SatisfyChemicalNeed.tmpChemicalNeeds.SortBy((Func<Need_Chemical, float>)((Need_Chemical x) => x.CurLevel));
				for (int j = 0; j < JobGiver_SatisfyChemicalNeed.tmpChemicalNeeds.Count; j++)
				{
					thing = this.FindDrugFor(pawn, JobGiver_SatisfyChemicalNeed.tmpChemicalNeeds[j]);
					if (thing != null)
						goto IL_00c8;
				}
				JobGiver_SatisfyChemicalNeed.tmpChemicalNeeds.Clear();
				Profiler.EndSample();
				result = null;
			}
			goto IL_0112;
			IL_0112:
			return result;
			IL_00c8:
			JobGiver_SatisfyChemicalNeed.tmpChemicalNeeds.Clear();
			Profiler.EndSample();
			result = DrugAIUtility.IngestAndTakeToInventoryJob(thing, pawn, 1);
			goto IL_0112;
		}

		private bool ShouldSatisfy(Need need)
		{
			Need_Chemical need_Chemical = need as Need_Chemical;
			return (byte)((need_Chemical != null && (int)need_Chemical.CurCategory <= 1) ? 1 : 0) != 0;
		}

		private Thing FindDrugFor(Pawn pawn, Need_Chemical need)
		{
			Hediff_Addiction addictionHediff = need.AddictionHediff;
			Thing result;
			ThingOwner<Thing> innerContainer;
			int i;
			if (addictionHediff == null)
			{
				result = null;
			}
			else
			{
				innerContainer = pawn.inventory.innerContainer;
				for (i = 0; i < innerContainer.Count; i++)
				{
					if (this.DrugValidator(pawn, addictionHediff, innerContainer[i]))
						goto IL_006a;
				}
				result = GenClosest.ClosestThingReachable(pawn.Position, pawn.Map, ThingRequest.ForGroup(ThingRequestGroup.Drug), PathEndMode.ClosestTouch, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false), 9999f, (Predicate<Thing>)((Thing x) => this.DrugValidator(pawn, addictionHediff, x)), null, 0, -1, false, RegionType.Set_Passable, false);
			}
			goto IL_00d6;
			IL_006a:
			result = innerContainer[i];
			goto IL_00d6;
			IL_00d6:
			return result;
		}

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
						result = false;
						goto IL_012b;
					}
					if (!pawn.CanReserve(drug, 1, -1, null, false))
					{
						result = false;
						goto IL_012b;
					}
					if (!drug.IsSociallyProper(pawn))
					{
						result = false;
						goto IL_012b;
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
							result = false;
							goto IL_012b;
						}
					}
					result = true;
				}
			}
			goto IL_012b;
			IL_012b:
			return result;
		}
	}
}
