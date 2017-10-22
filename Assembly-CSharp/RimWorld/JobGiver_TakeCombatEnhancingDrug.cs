using UnityEngine;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	public class JobGiver_TakeCombatEnhancingDrug : ThinkNode_JobGiver
	{
		private bool onlyIfInDanger;

		private const int TakeEveryTicks = 20000;

		public override ThinkNode DeepCopy(bool resolve = true)
		{
			JobGiver_TakeCombatEnhancingDrug jobGiver_TakeCombatEnhancingDrug = (JobGiver_TakeCombatEnhancingDrug)base.DeepCopy(resolve);
			jobGiver_TakeCombatEnhancingDrug.onlyIfInDanger = this.onlyIfInDanger;
			return jobGiver_TakeCombatEnhancingDrug;
		}

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
								result = null;
								goto IL_011b;
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
										break;
								}
							}
							if (num < num2)
							{
								result = null;
								goto IL_011b;
							}
						}
					}
					Job job = new Job(JobDefOf.Ingest, thing);
					job.count = 1;
					result = job;
				}
			}
			goto IL_011b;
			IL_011b:
			return result;
		}

		private bool HarmedRecently(Pawn pawn)
		{
			return Find.TickManager.TicksGame - pawn.mindState.lastHarmTick < 2500;
		}

		private Thing FindCombatEnhancingDrug(Pawn pawn)
		{
			int num = 0;
			Thing result;
			while (true)
			{
				if (num < pawn.inventory.innerContainer.Count)
				{
					Thing thing = pawn.inventory.innerContainer[num];
					CompDrug compDrug = thing.TryGetComp<CompDrug>();
					if (compDrug != null && compDrug.Props.isCombatEnhancingDrug)
					{
						result = thing;
						break;
					}
					num++;
					continue;
				}
				result = null;
				break;
			}
			return result;
		}
	}
}
