using System;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020000B1 RID: 177
	public static class DrugAIUtility
	{
		// Token: 0x06000444 RID: 1092 RVA: 0x0003275C File Offset: 0x00030B5C
		public static Job IngestAndTakeToInventoryJob(Thing drug, Pawn pawn, int maxNumToCarry = 9999)
		{
			Job job = new Job(JobDefOf.Ingest, drug);
			job.count = Mathf.Min(new int[]
			{
				drug.stackCount,
				drug.def.ingestible.maxNumToIngestAtOnce,
				maxNumToCarry
			});
			if (drug.Spawned && pawn.drugs != null && !pawn.inventory.innerContainer.Contains(drug.def))
			{
				DrugPolicyEntry drugPolicyEntry = pawn.drugs.CurrentPolicy[drug.def];
				if (drugPolicyEntry.allowScheduled)
				{
					job.takeExtraIngestibles = drugPolicyEntry.takeToInventory;
				}
			}
			return job;
		}
	}
}
