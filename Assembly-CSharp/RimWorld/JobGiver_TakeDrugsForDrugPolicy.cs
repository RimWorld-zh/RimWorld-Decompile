using System;
using UnityEngine.Profiling;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020000B0 RID: 176
	public class JobGiver_TakeDrugsForDrugPolicy : ThinkNode_JobGiver
	{
		// Token: 0x06000440 RID: 1088 RVA: 0x000324D8 File Offset: 0x000308D8
		public override float GetPriority(Pawn pawn)
		{
			DrugPolicy currentPolicy = pawn.drugs.CurrentPolicy;
			for (int i = 0; i < currentPolicy.Count; i++)
			{
				if (pawn.drugs.ShouldTryToTakeScheduledNow(currentPolicy[i].drug))
				{
					return 7.5f;
				}
			}
			return 0f;
		}

		// Token: 0x06000441 RID: 1089 RVA: 0x00032540 File Offset: 0x00030940
		protected override Job TryGiveJob(Pawn pawn)
		{
			Profiler.BeginSample("DrugPolicy");
			DrugPolicy currentPolicy = pawn.drugs.CurrentPolicy;
			for (int i = 0; i < currentPolicy.Count; i++)
			{
				if (pawn.drugs.ShouldTryToTakeScheduledNow(currentPolicy[i].drug))
				{
					Thing thing = this.FindDrugFor(pawn, currentPolicy[i].drug);
					if (thing != null)
					{
						Profiler.EndSample();
						return DrugAIUtility.IngestAndTakeToInventoryJob(thing, pawn, 1);
					}
				}
			}
			Profiler.EndSample();
			return null;
		}

		// Token: 0x06000442 RID: 1090 RVA: 0x000325DC File Offset: 0x000309DC
		private Thing FindDrugFor(Pawn pawn, ThingDef drugDef)
		{
			ThingOwner<Thing> innerContainer = pawn.inventory.innerContainer;
			for (int i = 0; i < innerContainer.Count; i++)
			{
				if (innerContainer[i].def == drugDef && this.DrugValidator(pawn, innerContainer[i]))
				{
					return innerContainer[i];
				}
			}
			return GenClosest.ClosestThingReachable(pawn.Position, pawn.Map, ThingRequest.ForDef(drugDef), PathEndMode.ClosestTouch, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false), 9999f, (Thing x) => this.DrugValidator(pawn, x), null, 0, -1, false, RegionType.Set_Passable, false);
		}

		// Token: 0x06000443 RID: 1091 RVA: 0x000326B0 File Offset: 0x00030AB0
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
				result = true;
			}
			return result;
		}
	}
}
