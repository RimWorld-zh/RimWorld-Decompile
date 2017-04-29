using RimWorld;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Verse.AI
{
	public static class Toils_JobTransforms
	{
		private static List<IntVec3> yieldedIngPlaceCells = new List<IntVec3>();

		public static Toil ExtractNextTargetFromQueue(TargetIndex ind)
		{
			Toil toil = new Toil();
			toil.initAction = delegate
			{
				Pawn actor = toil.actor;
				Job curJob = actor.jobs.curJob;
				List<LocalTargetInfo> targetQueue = curJob.GetTargetQueue(ind);
				if (targetQueue.NullOrEmpty<LocalTargetInfo>())
				{
					return;
				}
				curJob.SetTarget(ind, targetQueue[0]);
				targetQueue.RemoveAt(0);
				if (!curJob.countQueue.NullOrEmpty<int>())
				{
					curJob.count = curJob.countQueue[0];
					curJob.countQueue.RemoveAt(0);
				}
			};
			return toil;
		}

		public static Toil ClearDespawnedNullOrForbiddenQueuedTargets(TargetIndex ind)
		{
			Toil toil = new Toil();
			toil.initAction = delegate
			{
				Pawn actor = toil.actor;
				Job curJob = actor.jobs.curJob;
				List<LocalTargetInfo> targetQueue = curJob.GetTargetQueue(ind);
				targetQueue.RemoveAll((LocalTargetInfo ta) => !ta.HasThing || !ta.Thing.Spawned || ta.Thing.IsForbidden(actor));
			};
			return toil;
		}

		[DebuggerHidden]
		private static IEnumerable<IntVec3> IngredientPlaceCellsInOrder(IBillGiver billGiver)
		{
			Toils_JobTransforms.<IngredientPlaceCellsInOrder>c__Iterator1B5 <IngredientPlaceCellsInOrder>c__Iterator1B = new Toils_JobTransforms.<IngredientPlaceCellsInOrder>c__Iterator1B5();
			<IngredientPlaceCellsInOrder>c__Iterator1B.billGiver = billGiver;
			<IngredientPlaceCellsInOrder>c__Iterator1B.<$>billGiver = billGiver;
			Toils_JobTransforms.<IngredientPlaceCellsInOrder>c__Iterator1B5 expr_15 = <IngredientPlaceCellsInOrder>c__Iterator1B;
			expr_15.$PC = -2;
			return expr_15;
		}

		public static Toil SetTargetToIngredientPlaceCell(TargetIndex billGiverInd, TargetIndex carryItemInd, TargetIndex cellTargetInd)
		{
			Toil toil = new Toil();
			toil.initAction = delegate
			{
				Pawn actor = toil.actor;
				Job curJob = actor.jobs.curJob;
				Thing thing = curJob.GetTarget(carryItemInd).Thing;
				IBillGiver billGiver = curJob.GetTarget(billGiverInd).Thing as IBillGiver;
				IntVec3 c = IntVec3.Invalid;
				foreach (IntVec3 current in Toils_JobTransforms.IngredientPlaceCellsInOrder(billGiver))
				{
					if (!c.IsValid)
					{
						c = current;
					}
					bool flag = false;
					List<Thing> list = actor.Map.thingGrid.ThingsListAt(current);
					for (int i = 0; i < list.Count; i++)
					{
						if (list[i].def.category == ThingCategory.Item && (list[i].def != thing.def || list[i].stackCount == list[i].def.stackLimit))
						{
							flag = true;
							break;
						}
					}
					if (!flag)
					{
						curJob.SetTarget(cellTargetInd, current);
						return;
					}
				}
				curJob.SetTarget(cellTargetInd, c);
			};
			return toil;
		}

		public static Toil MoveCurrentTargetIntoQueue(TargetIndex ind)
		{
			Toil toil = new Toil();
			toil.initAction = delegate
			{
				Job curJob = toil.actor.CurJob;
				LocalTargetInfo target = curJob.GetTarget(ind);
				if (target.IsValid)
				{
					List<LocalTargetInfo> targetQueue = curJob.GetTargetQueue(ind);
					if (targetQueue == null)
					{
						curJob.AddQueuedTarget(ind, target);
					}
					else
					{
						targetQueue.Insert(0, target);
					}
					curJob.SetTarget(ind, null);
				}
			};
			return toil;
		}
	}
}
