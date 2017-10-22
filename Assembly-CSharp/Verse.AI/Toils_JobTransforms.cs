using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Verse.AI
{
	public static class Toils_JobTransforms
	{
		private static List<IntVec3> yieldedIngPlaceCells = new List<IntVec3>();

		public static Toil ExtractNextTargetFromQueue(TargetIndex ind)
		{
			Toil toil = new Toil();
			toil.initAction = (Action)delegate()
			{
				Pawn actor = toil.actor;
				Job curJob = actor.jobs.curJob;
				List<LocalTargetInfo> targetQueue = curJob.GetTargetQueue(ind);
				if (!targetQueue.NullOrEmpty())
				{
					curJob.SetTarget(ind, targetQueue[0]);
					targetQueue.RemoveAt(0);
					if (!curJob.countQueue.NullOrEmpty())
					{
						curJob.count = curJob.countQueue[0];
						curJob.countQueue.RemoveAt(0);
					}
				}
			};
			return toil;
		}

		public static Toil ClearDespawnedNullOrForbiddenQueuedTargets(TargetIndex ind)
		{
			Toil toil = new Toil();
			toil.initAction = (Action)delegate()
			{
				Pawn actor = toil.actor;
				Job curJob = actor.jobs.curJob;
				List<LocalTargetInfo> targetQueue = curJob.GetTargetQueue(ind);
				targetQueue.RemoveAll((Predicate<LocalTargetInfo>)((LocalTargetInfo ta) => !ta.HasThing || !ta.Thing.Spawned || ta.Thing.IsForbidden(actor)));
			};
			return toil;
		}

		private static IEnumerable<IntVec3> IngredientPlaceCellsInOrder(IBillGiver billGiver)
		{
			Toils_JobTransforms.yieldedIngPlaceCells.Clear();
			IntVec3 interactCell = ((Thing)billGiver).InteractionCell;
			foreach (IntVec3 item in from c in billGiver.IngredientStackCells
			orderby (c - ((_003CIngredientPlaceCellsInOrder_003Ec__Iterator1BC)/*Error near IL_0053: stateMachine*/)._003CinteractCell_003E__0).LengthHorizontalSquared
			select c)
			{
				Toils_JobTransforms.yieldedIngPlaceCells.Add(item);
				yield return item;
			}
			for (int i = 0; i < 200; i++)
			{
				IntVec3 c2 = interactCell + GenRadial.RadialPattern[i];
				if (!Toils_JobTransforms.yieldedIngPlaceCells.Contains(c2))
				{
					Building ed = c2.GetEdifice(billGiver.Map);
					if (((ed == null) ? SurfaceType.Item : ((ed.def.passability != Traversability.Impassable) ? SurfaceType.Item : ed.def.surfaceType)) != 0)
					{
						yield return c2;
					}
				}
			}
		}

		public static Toil SetTargetToIngredientPlaceCell(TargetIndex billGiverInd, TargetIndex carryItemInd, TargetIndex cellTargetInd)
		{
			Toil toil = new Toil();
			toil.initAction = (Action)delegate()
			{
				Pawn actor = toil.actor;
				Job curJob = actor.jobs.curJob;
				Thing thing = curJob.GetTarget(carryItemInd).Thing;
				IBillGiver billGiver = curJob.GetTarget(billGiverInd).Thing as IBillGiver;
				IntVec3 c = IntVec3.Invalid;
				foreach (IntVec3 item in Toils_JobTransforms.IngredientPlaceCellsInOrder(billGiver))
				{
					if (!c.IsValid)
					{
						c = item;
					}
					bool flag = false;
					List<Thing> list = actor.Map.thingGrid.ThingsListAt(item);
					int num = 0;
					while (num < list.Count)
					{
						if (list[num].def.category != ThingCategory.Item || (list[num].def == thing.def && list[num].stackCount != list[num].def.stackLimit))
						{
							num++;
							continue;
						}
						flag = true;
						break;
					}
					if (!flag)
					{
						curJob.SetTarget(cellTargetInd, item);
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
			toil.initAction = (Action)delegate()
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
					curJob.SetTarget(ind, (Thing)null);
				}
			};
			return toil;
		}
	}
}
