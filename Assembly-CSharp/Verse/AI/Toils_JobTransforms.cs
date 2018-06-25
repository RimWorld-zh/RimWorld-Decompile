using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000A4F RID: 2639
	public static class Toils_JobTransforms
	{
		// Token: 0x0400254B RID: 9547
		private static List<IntVec3> yieldedIngPlaceCells = new List<IntVec3>();

		// Token: 0x06003AC2 RID: 15042 RVA: 0x001F2EB4 File Offset: 0x001F12B4
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

		// Token: 0x06003AC3 RID: 15043 RVA: 0x001F2F08 File Offset: 0x001F1308
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

		// Token: 0x06003AC4 RID: 15044 RVA: 0x001F2F54 File Offset: 0x001F1354
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

		// Token: 0x06003AC5 RID: 15045 RVA: 0x001F2FA8 File Offset: 0x001F13A8
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

		// Token: 0x06003AC6 RID: 15046 RVA: 0x001F2FD4 File Offset: 0x001F13D4
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

		// Token: 0x06003AC7 RID: 15047 RVA: 0x001F302C File Offset: 0x001F142C
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

		// Token: 0x06003AC8 RID: 15048 RVA: 0x001F3078 File Offset: 0x001F1478
		public static Toil SucceedOnNoTargetInQueue(TargetIndex ind)
		{
			Toil toil = new Toil();
			toil.EndOnNoTargetInQueue(ind, JobCondition.Succeeded);
			return toil;
		}
	}
}
