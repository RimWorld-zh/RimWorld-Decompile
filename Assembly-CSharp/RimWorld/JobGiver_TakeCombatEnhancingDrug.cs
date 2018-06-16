using System;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020000DC RID: 220
	public class JobGiver_TakeCombatEnhancingDrug : ThinkNode_JobGiver
	{
		// Token: 0x060004D7 RID: 1239 RVA: 0x000361A0 File Offset: 0x000345A0
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			JobGiver_TakeCombatEnhancingDrug jobGiver_TakeCombatEnhancingDrug = (JobGiver_TakeCombatEnhancingDrug)base.DeepCopy(resolve);
			jobGiver_TakeCombatEnhancingDrug.onlyIfInDanger = this.onlyIfInDanger;
			return jobGiver_TakeCombatEnhancingDrug;
		}

		// Token: 0x060004D8 RID: 1240 RVA: 0x000361D0 File Offset: 0x000345D0
		protected override Job TryGiveJob(Pawn pawn)
		{
			Job result;
			if (pawn.IsTeetotaler())
			{
				result = null;
			}
			else if (Find.TickManager.TicksGame - pawn.mindState.lastTakeCombatEnhancingDrugTick < 20000)
			{
				result = null;
			}
			else
			{
				Thing thing = this.FindCombatEnhancingDrug(pawn);
				if (thing == null)
				{
					result = null;
				}
				else
				{
					if (this.onlyIfInDanger)
					{
						Lord lord = pawn.GetLord();
						if (lord == null)
						{
							if (!this.HarmedRecently(pawn))
							{
								return null;
							}
						}
						else
						{
							int num = 0;
							int num2 = Mathf.Clamp(lord.ownedPawns.Count / 2, 1, 4);
							for (int i = 0; i < lord.ownedPawns.Count; i++)
							{
								if (this.HarmedRecently(lord.ownedPawns[i]))
								{
									num++;
									if (num >= num2)
									{
										break;
									}
								}
							}
							if (num < num2)
							{
								return null;
							}
						}
					}
					result = new Job(JobDefOf.Ingest, thing)
					{
						count = 1
					};
				}
			}
			return result;
		}

		// Token: 0x060004D9 RID: 1241 RVA: 0x000362FC File Offset: 0x000346FC
		private bool HarmedRecently(Pawn pawn)
		{
			return Find.TickManager.TicksGame - pawn.mindState.lastHarmTick < 2500;
		}

		// Token: 0x060004DA RID: 1242 RVA: 0x00036330 File Offset: 0x00034730
		private Thing FindCombatEnhancingDrug(Pawn pawn)
		{
			for (int i = 0; i < pawn.inventory.innerContainer.Count; i++)
			{
				Thing thing = pawn.inventory.innerContainer[i];
				CompDrug compDrug = thing.TryGetComp<CompDrug>();
				if (compDrug != null)
				{
					if (compDrug.Props.isCombatEnhancingDrug)
					{
						return thing;
					}
				}
			}
			return null;
		}

		// Token: 0x040002B2 RID: 690
		private bool onlyIfInDanger;

		// Token: 0x040002B3 RID: 691
		private const int TakeEveryTicks = 20000;
	}
}
