using System;
using System.Runtime.CompilerServices;
using RimWorld;

namespace Verse.AI
{
	public static class Toils_General
	{
		public static Toil Wait(int ticks, TargetIndex face = TargetIndex.None)
		{
			Toil toil = new Toil();
			toil.initAction = delegate()
			{
				toil.actor.pather.StopDead();
			};
			toil.defaultCompleteMode = ToilCompleteMode.Delay;
			toil.defaultDuration = ticks;
			if (face != TargetIndex.None)
			{
				toil.handlingFacing = true;
				toil.tickAction = delegate()
				{
					toil.actor.rotationTracker.FaceTarget(toil.actor.CurJob.GetTarget(face));
				};
			}
			return toil;
		}

		public static Toil WaitWith(TargetIndex targetInd, int ticks, bool useProgressBar = false, bool maintainPosture = false)
		{
			Toil toil = new Toil();
			toil.initAction = delegate()
			{
				toil.actor.pather.StopDead();
				Pawn pawn = toil.actor.CurJob.GetTarget(targetInd).Thing as Pawn;
				if (pawn != null)
				{
					if (pawn == toil.actor)
					{
						Log.Warning("Executing WaitWith toil but otherPawn is the same as toil.actor", false);
					}
					else
					{
						Pawn pawn2 = pawn;
						int ticks2 = ticks;
						bool maintainPosture2 = maintainPosture;
						PawnUtility.ForceWait(pawn2, ticks2, null, maintainPosture2);
					}
				}
			};
			toil.FailOnDespawnedOrNull(targetInd);
			toil.FailOnCannotTouch(targetInd, PathEndMode.Touch);
			toil.defaultCompleteMode = ToilCompleteMode.Delay;
			toil.defaultDuration = ticks;
			if (useProgressBar)
			{
				toil.WithProgressBarToilDelay(targetInd, false, -0.5f);
			}
			return toil;
		}

		public static Toil RemoveDesignationsOnThing(TargetIndex ind, DesignationDef def)
		{
			Toil toil = new Toil();
			toil.initAction = delegate()
			{
				toil.actor.Map.designationManager.RemoveAllDesignationsOn(toil.actor.jobs.curJob.GetTarget(ind).Thing, false);
			};
			return toil;
		}

		public static Toil ClearTarget(TargetIndex ind)
		{
			Toil toil = new Toil();
			toil.initAction = delegate()
			{
				toil.GetActor().CurJob.SetTarget(ind, null);
			};
			return toil;
		}

		public static Toil PutCarriedThingInInventory()
		{
			Toil toil = new Toil();
			toil.initAction = delegate()
			{
				Pawn actor = toil.GetActor();
				if (actor.carryTracker.CarriedThing != null && !actor.carryTracker.innerContainer.TryTransferToContainer(actor.carryTracker.CarriedThing, actor.inventory.innerContainer, true))
				{
					Thing thing;
					actor.carryTracker.TryDropCarriedThing(actor.Position, actor.carryTracker.CarriedThing.stackCount, ThingPlaceMode.Near, out thing, null);
				}
			};
			return toil;
		}

		public static Toil Do(Action action)
		{
			return new Toil
			{
				initAction = action
			};
		}

		public static Toil DoAtomic(Action action)
		{
			return new Toil
			{
				initAction = action,
				atomicWithPrevious = true
			};
		}

		public static Toil Open(TargetIndex openableInd)
		{
			Toil open = new Toil();
			open.initAction = delegate()
			{
				Pawn actor = open.actor;
				Thing thing = actor.CurJob.GetTarget(openableInd).Thing;
				Designation designation = actor.Map.designationManager.DesignationOn(thing, DesignationDefOf.Open);
				if (designation != null)
				{
					designation.Delete();
				}
				IOpenable openable = (IOpenable)thing;
				if (openable.CanOpen)
				{
					openable.Open();
					actor.records.Increment(RecordDefOf.ContainersOpened);
				}
			};
			open.defaultCompleteMode = ToilCompleteMode.Instant;
			return open;
		}

		public static Toil Label()
		{
			return new Toil
			{
				atomicWithPrevious = true,
				defaultCompleteMode = ToilCompleteMode.Instant
			};
		}

		[CompilerGenerated]
		private sealed class <Wait>c__AnonStorey0
		{
			internal Toil toil;

			internal TargetIndex face;

			public <Wait>c__AnonStorey0()
			{
			}

			internal void <>m__0()
			{
				this.toil.actor.pather.StopDead();
			}

			internal void <>m__1()
			{
				this.toil.actor.rotationTracker.FaceTarget(this.toil.actor.CurJob.GetTarget(this.face));
			}
		}

		[CompilerGenerated]
		private sealed class <WaitWith>c__AnonStorey1
		{
			internal Toil toil;

			internal TargetIndex targetInd;

			internal int ticks;

			internal bool maintainPosture;

			public <WaitWith>c__AnonStorey1()
			{
			}

			internal void <>m__0()
			{
				this.toil.actor.pather.StopDead();
				Pawn pawn = this.toil.actor.CurJob.GetTarget(this.targetInd).Thing as Pawn;
				if (pawn != null)
				{
					if (pawn == this.toil.actor)
					{
						Log.Warning("Executing WaitWith toil but otherPawn is the same as toil.actor", false);
					}
					else
					{
						Pawn pawn2 = pawn;
						int num = this.ticks;
						bool flag = this.maintainPosture;
						PawnUtility.ForceWait(pawn2, num, null, flag);
					}
				}
			}
		}

		[CompilerGenerated]
		private sealed class <RemoveDesignationsOnThing>c__AnonStorey2
		{
			internal Toil toil;

			internal TargetIndex ind;

			public <RemoveDesignationsOnThing>c__AnonStorey2()
			{
			}

			internal void <>m__0()
			{
				this.toil.actor.Map.designationManager.RemoveAllDesignationsOn(this.toil.actor.jobs.curJob.GetTarget(this.ind).Thing, false);
			}
		}

		[CompilerGenerated]
		private sealed class <ClearTarget>c__AnonStorey3
		{
			internal Toil toil;

			internal TargetIndex ind;

			public <ClearTarget>c__AnonStorey3()
			{
			}

			internal void <>m__0()
			{
				this.toil.GetActor().CurJob.SetTarget(this.ind, null);
			}
		}

		[CompilerGenerated]
		private sealed class <PutCarriedThingInInventory>c__AnonStorey4
		{
			internal Toil toil;

			public <PutCarriedThingInInventory>c__AnonStorey4()
			{
			}

			internal void <>m__0()
			{
				Pawn actor = this.toil.GetActor();
				if (actor.carryTracker.CarriedThing != null && !actor.carryTracker.innerContainer.TryTransferToContainer(actor.carryTracker.CarriedThing, actor.inventory.innerContainer, true))
				{
					Thing thing;
					actor.carryTracker.TryDropCarriedThing(actor.Position, actor.carryTracker.CarriedThing.stackCount, ThingPlaceMode.Near, out thing, null);
				}
			}
		}

		[CompilerGenerated]
		private sealed class <Open>c__AnonStorey5
		{
			internal Toil open;

			internal TargetIndex openableInd;

			public <Open>c__AnonStorey5()
			{
			}

			internal void <>m__0()
			{
				Pawn actor = this.open.actor;
				Thing thing = actor.CurJob.GetTarget(this.openableInd).Thing;
				Designation designation = actor.Map.designationManager.DesignationOn(thing, DesignationDefOf.Open);
				if (designation != null)
				{
					designation.Delete();
				}
				IOpenable openable = (IOpenable)thing;
				if (openable.CanOpen)
				{
					openable.Open();
					actor.records.Increment(RecordDefOf.ContainersOpened);
				}
			}
		}
	}
}
