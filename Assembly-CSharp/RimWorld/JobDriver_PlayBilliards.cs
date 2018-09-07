using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using Verse;
using Verse.AI;
using Verse.Sound;

namespace RimWorld
{
	public class JobDriver_PlayBilliards : JobDriver
	{
		private const int ShotDuration = 600;

		public JobDriver_PlayBilliards()
		{
		}

		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			Pawn pawn = this.pawn;
			LocalTargetInfo targetA = this.job.targetA;
			Job job = this.job;
			int joyMaxParticipants = this.job.def.joyMaxParticipants;
			int stackCount = 0;
			return pawn.Reserve(targetA, job, joyMaxParticipants, stackCount, null, errorOnFailed);
		}

		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.EndOnDespawnedOrNull(TargetIndex.A, JobCondition.Incompletable);
			Toil chooseCell = Toils_Misc.FindRandomAdjacentReachableCell(TargetIndex.A, TargetIndex.B);
			yield return chooseCell;
			yield return Toils_Reserve.Reserve(TargetIndex.B, 1, -1, null);
			yield return Toils_Goto.GotoCell(TargetIndex.B, PathEndMode.OnCell);
			Toil play = new Toil();
			play.initAction = delegate()
			{
				this.job.locomotionUrgency = LocomotionUrgency.Walk;
			};
			play.tickAction = delegate()
			{
				this.pawn.rotationTracker.FaceCell(base.TargetA.Thing.OccupiedRect().ClosestCellTo(this.pawn.Position));
				if (this.ticksLeftThisToil == 300)
				{
					SoundDefOf.PlayBilliards.PlayOneShot(new TargetInfo(this.pawn.Position, this.pawn.Map, false));
				}
				if (Find.TickManager.TicksGame > this.startTick + this.job.def.joyDuration)
				{
					base.EndJobWith(JobCondition.Succeeded);
					return;
				}
				Pawn pawn = this.pawn;
				Building joySource = (Building)base.TargetThingA;
				JoyUtility.JoyTickCheckEnd(pawn, JoyTickFullJoyAction.EndJob, 1f, joySource);
			};
			play.handlingFacing = true;
			play.socialMode = RandomSocialMode.SuperActive;
			play.defaultCompleteMode = ToilCompleteMode.Delay;
			play.defaultDuration = 600;
			play.AddFinishAction(delegate
			{
				JoyUtility.TryGainRecRoomThought(this.pawn);
			});
			yield return play;
			yield return Toils_Reserve.Release(TargetIndex.B);
			yield return Toils_Jump.Jump(chooseCell);
			yield break;
		}

		public override object[] TaleParameters()
		{
			return new object[]
			{
				this.pawn,
				base.TargetA.Thing.def
			};
		}

		[CompilerGenerated]
		private sealed class <MakeNewToils>c__Iterator0 : IEnumerable, IEnumerable<Toil>, IEnumerator, IDisposable, IEnumerator<Toil>
		{
			internal Toil <chooseCell>__0;

			internal Toil <play>__0;

			internal JobDriver_PlayBilliards $this;

			internal Toil $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <MakeNewToils>c__Iterator0()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0u:
					this.EndOnDespawnedOrNull(TargetIndex.A, JobCondition.Incompletable);
					chooseCell = Toils_Misc.FindRandomAdjacentReachableCell(TargetIndex.A, TargetIndex.B);
					this.$current = chooseCell;
					if (!this.$disposing)
					{
						this.$PC = 1;
					}
					return true;
				case 1u:
					this.$current = Toils_Reserve.Reserve(TargetIndex.B, 1, -1, null);
					if (!this.$disposing)
					{
						this.$PC = 2;
					}
					return true;
				case 2u:
					this.$current = Toils_Goto.GotoCell(TargetIndex.B, PathEndMode.OnCell);
					if (!this.$disposing)
					{
						this.$PC = 3;
					}
					return true;
				case 3u:
					play = new Toil();
					play.initAction = delegate()
					{
						this.job.locomotionUrgency = LocomotionUrgency.Walk;
					};
					play.tickAction = delegate()
					{
						this.pawn.rotationTracker.FaceCell(base.TargetA.Thing.OccupiedRect().ClosestCellTo(this.pawn.Position));
						if (this.ticksLeftThisToil == 300)
						{
							SoundDefOf.PlayBilliards.PlayOneShot(new TargetInfo(this.pawn.Position, this.pawn.Map, false));
						}
						if (Find.TickManager.TicksGame > this.startTick + this.job.def.joyDuration)
						{
							base.EndJobWith(JobCondition.Succeeded);
							return;
						}
						Pawn pawn = this.pawn;
						Building joySource = (Building)base.TargetThingA;
						JoyUtility.JoyTickCheckEnd(pawn, JoyTickFullJoyAction.EndJob, 1f, joySource);
					};
					play.handlingFacing = true;
					play.socialMode = RandomSocialMode.SuperActive;
					play.defaultCompleteMode = ToilCompleteMode.Delay;
					play.defaultDuration = 600;
					play.AddFinishAction(delegate
					{
						JoyUtility.TryGainRecRoomThought(this.pawn);
					});
					this.$current = play;
					if (!this.$disposing)
					{
						this.$PC = 4;
					}
					return true;
				case 4u:
					this.$current = Toils_Reserve.Release(TargetIndex.B);
					if (!this.$disposing)
					{
						this.$PC = 5;
					}
					return true;
				case 5u:
					this.$current = Toils_Jump.Jump(chooseCell);
					if (!this.$disposing)
					{
						this.$PC = 6;
					}
					return true;
				case 6u:
					this.$PC = -1;
					break;
				}
				return false;
			}

			Toil IEnumerator<Toil>.Current
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
				this.$disposing = true;
				this.$PC = -1;
			}

			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.System.Collections.Generic.IEnumerable<Verse.AI.Toil>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<Toil> IEnumerable<Toil>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				JobDriver_PlayBilliards.<MakeNewToils>c__Iterator0 <MakeNewToils>c__Iterator = new JobDriver_PlayBilliards.<MakeNewToils>c__Iterator0();
				<MakeNewToils>c__Iterator.$this = this;
				return <MakeNewToils>c__Iterator;
			}

			internal void <>m__0()
			{
				this.job.locomotionUrgency = LocomotionUrgency.Walk;
			}

			internal void <>m__1()
			{
				this.pawn.rotationTracker.FaceCell(base.TargetA.Thing.OccupiedRect().ClosestCellTo(this.pawn.Position));
				if (this.ticksLeftThisToil == 300)
				{
					SoundDefOf.PlayBilliards.PlayOneShot(new TargetInfo(this.pawn.Position, this.pawn.Map, false));
				}
				if (Find.TickManager.TicksGame > this.startTick + this.job.def.joyDuration)
				{
					base.EndJobWith(JobCondition.Succeeded);
					return;
				}
				Pawn pawn = this.pawn;
				Building joySource = (Building)base.TargetThingA;
				JoyUtility.JoyTickCheckEnd(pawn, JoyTickFullJoyAction.EndJob, 1f, joySource);
			}

			internal void <>m__2()
			{
				JoyUtility.TryGainRecRoomThought(this.pawn);
			}
		}
	}
}
