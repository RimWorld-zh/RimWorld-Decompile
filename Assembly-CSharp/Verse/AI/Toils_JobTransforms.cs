using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using RimWorld;

namespace Verse.AI
{
	public static class Toils_JobTransforms
	{
		private static List<IntVec3> yieldedIngPlaceCells = new List<IntVec3>();

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

		public static Toil SucceedOnNoTargetInQueue(TargetIndex ind)
		{
			Toil toil = new Toil();
			toil.EndOnNoTargetInQueue(ind, JobCondition.Succeeded);
			return toil;
		}

		// Note: this type is marked as 'beforefieldinit'.
		static Toils_JobTransforms()
		{
		}

		[CompilerGenerated]
		private sealed class <ExtractNextTargetFromQueue>c__AnonStorey1
		{
			internal Toil toil;

			internal TargetIndex ind;

			internal bool failIfCountFromQueueTooBig;

			public <ExtractNextTargetFromQueue>c__AnonStorey1()
			{
			}

			internal void <>m__0()
			{
				Pawn actor = this.toil.actor;
				Job curJob = actor.jobs.curJob;
				List<LocalTargetInfo> targetQueue = curJob.GetTargetQueue(this.ind);
				if (!targetQueue.NullOrEmpty<LocalTargetInfo>())
				{
					if (this.failIfCountFromQueueTooBig && !curJob.countQueue.NullOrEmpty<int>() && targetQueue[0].HasThing && curJob.countQueue[0] > targetQueue[0].Thing.stackCount)
					{
						actor.jobs.curDriver.EndJobWith(JobCondition.Incompletable);
					}
					else
					{
						curJob.SetTarget(this.ind, targetQueue[0]);
						targetQueue.RemoveAt(0);
						if (!curJob.countQueue.NullOrEmpty<int>())
						{
							curJob.count = curJob.countQueue[0];
							curJob.countQueue.RemoveAt(0);
						}
					}
				}
			}
		}

		[CompilerGenerated]
		private sealed class <ClearQueue>c__AnonStorey2
		{
			internal Toil toil;

			internal TargetIndex ind;

			public <ClearQueue>c__AnonStorey2()
			{
			}

			internal void <>m__0()
			{
				Pawn actor = this.toil.actor;
				Job curJob = actor.jobs.curJob;
				List<LocalTargetInfo> targetQueue = curJob.GetTargetQueue(this.ind);
				if (!targetQueue.NullOrEmpty<LocalTargetInfo>())
				{
					targetQueue.Clear();
				}
			}
		}

		[CompilerGenerated]
		private sealed class <ClearDespawnedNullOrForbiddenQueuedTargets>c__AnonStorey3
		{
			internal Toil toil;

			internal TargetIndex ind;

			internal Func<Thing, bool> validator;

			public <ClearDespawnedNullOrForbiddenQueuedTargets>c__AnonStorey3()
			{
			}

			internal void <>m__0()
			{
				Pawn actor = this.toil.actor;
				Job curJob = actor.jobs.curJob;
				List<LocalTargetInfo> targetQueue = curJob.GetTargetQueue(this.ind);
				targetQueue.RemoveAll((LocalTargetInfo ta) => !ta.HasThing || !ta.Thing.Spawned || ta.Thing.IsForbidden(actor) || (this.validator != null && !this.validator(ta.Thing)));
			}

			private sealed class <ClearDespawnedNullOrForbiddenQueuedTargets>c__AnonStorey4
			{
				internal Pawn actor;

				internal Toils_JobTransforms.<ClearDespawnedNullOrForbiddenQueuedTargets>c__AnonStorey3 <>f__ref$3;

				public <ClearDespawnedNullOrForbiddenQueuedTargets>c__AnonStorey4()
				{
				}

				internal bool <>m__0(LocalTargetInfo ta)
				{
					return !ta.HasThing || !ta.Thing.Spawned || ta.Thing.IsForbidden(this.actor) || (this.<>f__ref$3.validator != null && !this.<>f__ref$3.validator(ta.Thing));
				}
			}
		}

		[CompilerGenerated]
		private sealed class <IngredientPlaceCellsInOrder>c__Iterator0 : IEnumerable, IEnumerable<IntVec3>, IEnumerator, IDisposable, IEnumerator<IntVec3>
		{
			internal Thing destination;

			internal IBillGiver <billGiver>__1;

			internal IEnumerator<IntVec3> $locvar0;

			internal IntVec3 <c>__2;

			internal int <i>__3;

			internal IntVec3 <c>__4;

			internal Building <ed>__4;

			internal IntVec3 $current;

			internal bool $disposing;

			internal int $PC;

			private Toils_JobTransforms.<IngredientPlaceCellsInOrder>c__Iterator0.<IngredientPlaceCellsInOrder>c__AnonStorey5 $locvar1;

			[DebuggerHidden]
			public <IngredientPlaceCellsInOrder>c__Iterator0()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				bool flag = false;
				switch (num)
				{
				case 0u:
				{
					Toils_JobTransforms.yieldedIngPlaceCells.Clear();
					IntVec3 interactCell = destination.Position;
					billGiver = (destination as IBillGiver);
					if (billGiver == null)
					{
						goto IL_14F;
					}
					interactCell = ((Thing)billGiver).InteractionCell;
					enumerator = (from c in billGiver.IngredientStackCells
					orderby (c - interactCell).LengthHorizontalSquared
					select c).GetEnumerator();
					num = 4294967293u;
					break;
				}
				case 1u:
					break;
				case 2u:
					goto IL_21A;
				default:
					return false;
				}
				try
				{
					switch (num)
					{
					}
					if (enumerator.MoveNext())
					{
						c = enumerator.Current;
						Toils_JobTransforms.yieldedIngPlaceCells.Add(c);
						this.$current = c;
						if (!this.$disposing)
						{
							this.$PC = 1;
						}
						flag = true;
						return true;
					}
				}
				finally
				{
					if (!flag)
					{
						if (enumerator != null)
						{
							enumerator.Dispose();
						}
					}
				}
				IL_14F:
				i = 0;
				goto IL_228;
				IL_21A:
				i++;
				IL_228:
				if (i >= 200)
				{
					this.$PC = -1;
				}
				else
				{
					c2 = <IngredientPlaceCellsInOrder>c__AnonStorey.interactCell + GenRadial.RadialPattern[i];
					if (Toils_JobTransforms.yieldedIngPlaceCells.Contains(c2))
					{
						goto IL_21A;
					}
					ed = c2.GetEdifice(destination.Map);
					if (ed != null && ed.def.passability == Traversability.Impassable && ed.def.surfaceType == SurfaceType.None)
					{
						goto IL_21A;
					}
					this.$current = c2;
					if (!this.$disposing)
					{
						this.$PC = 2;
					}
					return true;
				}
				return false;
			}

			IntVec3 IEnumerator<IntVec3>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			[DebuggerHidden]
			public void Dispose()
			{
				uint num = (uint)this.$PC;
				this.$disposing = true;
				this.$PC = -1;
				switch (num)
				{
				case 1u:
					try
					{
					}
					finally
					{
						if (enumerator != null)
						{
							enumerator.Dispose();
						}
					}
					break;
				}
			}

			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.System.Collections.Generic.IEnumerable<Verse.IntVec3>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<IntVec3> IEnumerable<IntVec3>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				Toils_JobTransforms.<IngredientPlaceCellsInOrder>c__Iterator0 <IngredientPlaceCellsInOrder>c__Iterator = new Toils_JobTransforms.<IngredientPlaceCellsInOrder>c__Iterator0();
				<IngredientPlaceCellsInOrder>c__Iterator.destination = destination;
				return <IngredientPlaceCellsInOrder>c__Iterator;
			}

			private sealed class <IngredientPlaceCellsInOrder>c__AnonStorey5
			{
				internal IntVec3 interactCell;

				internal Toils_JobTransforms.<IngredientPlaceCellsInOrder>c__Iterator0 <>f__ref$0;

				public <IngredientPlaceCellsInOrder>c__AnonStorey5()
				{
				}

				internal int <>m__0(IntVec3 c)
				{
					return (c - this.interactCell).LengthHorizontalSquared;
				}
			}
		}

		[CompilerGenerated]
		private sealed class <SetTargetToIngredientPlaceCell>c__AnonStorey6
		{
			internal Toil toil;

			internal TargetIndex carryItemInd;

			internal TargetIndex facilityInd;

			internal TargetIndex cellTargetInd;

			public <SetTargetToIngredientPlaceCell>c__AnonStorey6()
			{
			}

			internal void <>m__0()
			{
				Pawn actor = this.toil.actor;
				Job curJob = actor.jobs.curJob;
				Thing thing = curJob.GetTarget(this.carryItemInd).Thing;
				IntVec3 c = IntVec3.Invalid;
				foreach (IntVec3 intVec in Toils_JobTransforms.IngredientPlaceCellsInOrder(curJob.GetTarget(this.facilityInd).Thing))
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
						curJob.SetTarget(this.cellTargetInd, intVec);
						return;
					}
				}
				curJob.SetTarget(this.cellTargetInd, c);
			}
		}

		[CompilerGenerated]
		private sealed class <MoveCurrentTargetIntoQueue>c__AnonStorey7
		{
			internal Toil toil;

			internal TargetIndex ind;

			public <MoveCurrentTargetIntoQueue>c__AnonStorey7()
			{
			}

			internal void <>m__0()
			{
				Job curJob = this.toil.actor.CurJob;
				LocalTargetInfo target = curJob.GetTarget(this.ind);
				if (target.IsValid)
				{
					List<LocalTargetInfo> targetQueue = curJob.GetTargetQueue(this.ind);
					if (targetQueue == null)
					{
						curJob.AddQueuedTarget(this.ind, target);
					}
					else
					{
						targetQueue.Insert(0, target);
					}
					curJob.SetTarget(this.ind, null);
				}
			}
		}
	}
}
