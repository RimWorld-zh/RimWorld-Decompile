using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000A4E RID: 2638
	public static class Toils_JobTransforms
	{
		// Token: 0x0400253B RID: 9531
		private static List<IntVec3> yieldedIngPlaceCells = new List<IntVec3>();

		// Token: 0x06003AC1 RID: 15041 RVA: 0x001F2B88 File Offset: 0x001F0F88
		public static Toil ExtractNextTargetFromQueue(TargetIndex ind, bool failIfCountFromQueueTooBig = true)
		{
			Toil toil = new Toil();
			toil.initAction = delegate()
			{
				Pawn actor = toil.actor;
				Job curJob = actor.jobs.curJob;
				List<LocalTargetInfo> targetQueue = curJob.GetTargetQueue(ind);
				if (!targetQueue.NullOrEmpty<LocalTargetInfo>())
				{
					if (failIfCountFromQueueTooBig && !curJob.countQueue.NullOrEmpty<int>() && targetQueue[0].HasThing && curJob.countQueue[0] > targetQueue[0].Thing.stackCount)
					{
						actor.jobs.curDriver.EndJobWith(JobCondition.Incompletable);
					}
					else
					{
						curJob.SetTarget(ind, targetQueue[0]);
						targetQueue.RemoveAt(0);
						if (!curJob.countQueue.NullOrEmpty<int>())
						{
							curJob.count = curJob.countQueue[0];
							curJob.countQueue.RemoveAt(0);
						}
					}
				}
			};
			return toil;
		}

		// Token: 0x06003AC2 RID: 15042 RVA: 0x001F2BDC File Offset: 0x001F0FDC
		public static Toil ClearQueue(TargetIndex ind)
		{
			Toil toil = new Toil();
			toil.initAction = delegate()
			{
				Pawn actor = toil.actor;
				Job curJob = actor.jobs.curJob;
				List<LocalTargetInfo> targetQueue = curJob.GetTargetQueue(ind);
				if (!targetQueue.NullOrEmpty<LocalTargetInfo>())
				{
					targetQueue.Clear();
				}
			};
			return toil;
		}

		// Token: 0x06003AC3 RID: 15043 RVA: 0x001F2C28 File Offset: 0x001F1028
		public static Toil ClearDespawnedNullOrForbiddenQueuedTargets(TargetIndex ind, Func<Thing, bool> validator = null)
		{
			Toil toil = new Toil();
			toil.initAction = delegate()
			{
				Pawn actor = toil.actor;
				Job curJob = actor.jobs.curJob;
				List<LocalTargetInfo> targetQueue = curJob.GetTargetQueue(ind);
				targetQueue.RemoveAll((LocalTargetInfo ta) => !ta.HasThing || !ta.Thing.Spawned || ta.Thing.IsForbidden(actor) || (validator != null && !validator(ta.Thing)));
			};
			return toil;
		}

		// Token: 0x06003AC4 RID: 15044 RVA: 0x001F2C7C File Offset: 0x001F107C
		private static IEnumerable<IntVec3> IngredientPlaceCellsInOrder(Thing destination)
		{
			Toils_JobTransforms.yieldedIngPlaceCells.Clear();
			IntVec3 interactCell = destination.Position;
			IBillGiver billGiver = destination as IBillGiver;
			if (billGiver != null)
			{
				interactCell = ((Thing)billGiver).InteractionCell;
				foreach (IntVec3 c3 in from c in billGiver.IngredientStackCells
				orderby (c - interactCell).LengthHorizontalSquared
				select c)
				{
					Toils_JobTransforms.yieldedIngPlaceCells.Add(c3);
					yield return c3;
				}
			}
			for (int i = 0; i < 200; i++)
			{
				IntVec3 c2 = interactCell + GenRadial.RadialPattern[i];
				if (!Toils_JobTransforms.yieldedIngPlaceCells.Contains(c2))
				{
					Building ed = c2.GetEdifice(destination.Map);
					if (ed == null || ed.def.passability != Traversability.Impassable || ed.def.surfaceType != SurfaceType.None)
					{
						yield return c2;
					}
				}
			}
			yield break;
		}

		// Token: 0x06003AC5 RID: 15045 RVA: 0x001F2CA8 File Offset: 0x001F10A8
		public static Toil SetTargetToIngredientPlaceCell(TargetIndex facilityInd, TargetIndex carryItemInd, TargetIndex cellTargetInd)
		{
			Toil toil = new Toil();
			toil.initAction = delegate()
			{
				Pawn actor = toil.actor;
				Job curJob = actor.jobs.curJob;
				Thing thing = curJob.GetTarget(carryItemInd).Thing;
				IntVec3 c = IntVec3.Invalid;
				foreach (IntVec3 intVec in Toils_JobTransforms.IngredientPlaceCellsInOrder(curJob.GetTarget(facilityInd).Thing))
				{
					if (!c.IsValid)
					{
						c = intVec;
					}
					bool flag = false;
					List<Thing> list = actor.Map.thingGrid.ThingsListAt(intVec);
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
						curJob.SetTarget(cellTargetInd, intVec);
						return;
					}
				}
				curJob.SetTarget(cellTargetInd, c);
			};
			return toil;
		}

		// Token: 0x06003AC6 RID: 15046 RVA: 0x001F2D00 File Offset: 0x001F1100
		public static Toil MoveCurrentTargetIntoQueue(TargetIndex ind)
		{
			Toil toil = new Toil();
			toil.initAction = delegate()
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

		// Token: 0x06003AC7 RID: 15047 RVA: 0x001F2D4C File Offset: 0x001F114C
		public static Toil SucceedOnNoTargetInQueue(TargetIndex ind)
		{
			Toil toil = new Toil();
			toil.EndOnNoTargetInQueue(ind, JobCondition.Succeeded);
			return toil;
		}
	}
}
