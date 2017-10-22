#define ENABLE_PROFILER
using System;
using UnityEngine.Profiling;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JobGiver_TakeDrugsForDrugPolicy : ThinkNode_JobGiver
	{
		public override float GetPriority(Pawn pawn)
		{
			DrugPolicy currentPolicy = pawn.drugs.CurrentPolicy;
			int num = 0;
			float result;
			while (true)
			{
				if (num < currentPolicy.Count)
				{
					if (pawn.drugs.ShouldTryToTakeScheduledNow(currentPolicy[num].drug))
					{
						result = 7.5f;
						break;
					}
					num++;
					continue;
				}
				result = 0f;
				break;
			}
			return result;
		}

		protected override Job TryGiveJob(Pawn pawn)
		{
			Profiler.BeginSample("DrugPolicy");
			DrugPolicy currentPolicy = pawn.drugs.CurrentPolicy;
			int num = 0;
			Job result;
			while (true)
			{
				if (num < currentPolicy.Count)
				{
					if (pawn.drugs.ShouldTryToTakeScheduledNow(currentPolicy[num].drug))
					{
						Thing thing = this.FindDrugFor(pawn, currentPolicy[num].drug);
						if (thing != null)
						{
							Profiler.EndSample();
							result = DrugAIUtility.IngestAndTakeToInventoryJob(thing, pawn, 1);
							break;
						}
					}
					num++;
					continue;
				}
				Profiler.EndSample();
				result = null;
				break;
			}
			return result;
		}

		private Thing FindDrugFor(Pawn pawn, ThingDef drugDef)
		{
			ThingOwner<Thing> innerContainer = pawn.inventory.innerContainer;
			int num = 0;
			Thing result;
			while (true)
			{
				if (num < innerContainer.Count)
				{
					if (innerContainer[num].def == drugDef && this.DrugValidator(pawn, innerContainer[num]))
					{
						result = innerContainer[num];
						break;
					}
					num++;
					continue;
				}
				result = GenClosest.ClosestThingReachable(pawn.Position, pawn.Map, ThingRequest.ForDef(drugDef), PathEndMode.ClosestTouch, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false), 9999f, (Predicate<Thing>)((Thing x) => this.DrugValidator(pawn, x)), null, 0, -1, false, RegionType.Set_Passable, false);
				break;
			}
			return result;
		}

		private bool DrugValidator(Pawn pawn, Thing drug)
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
						goto IL_006e;
					}
					if (!pawn.CanReserve(drug, 1, -1, null, false))
					{
						result = false;
						goto IL_006e;
					}
					if (!drug.IsSociallyProper(pawn))
					{
						result = false;
						goto IL_006e;
					}
				}
				result = true;
			}
			goto IL_006e;
			IL_006e:
			return result;
		}
	}
}
