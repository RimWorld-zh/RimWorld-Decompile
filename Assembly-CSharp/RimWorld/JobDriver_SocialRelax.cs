using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JobDriver_SocialRelax : JobDriver
	{
		private const TargetIndex GatherSpotParentInd = TargetIndex.A;

		private const TargetIndex ChairOrSpotInd = TargetIndex.B;

		private const TargetIndex OptionalIngestibleInd = TargetIndex.C;

		public JobDriver_SocialRelax()
		{
		}

		private Thing GatherSpotParent
		{
			get
			{
				return this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		private bool HasChair
		{
			get
			{
				return this.job.GetTarget(TargetIndex.B).HasThing;
			}
		}

		private bool HasDrink
		{
			get
			{
				return this.job.GetTarget(TargetIndex.C).HasThing;
			}
		}

		private IntVec3 ClosestGatherSpotParentCell
		{
			get
			{
				return this.GatherSpotParent.OccupiedRect().ClosestCellTo(this.pawn.Position);
			}
		}

		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			Pawn pawn = this.pawn;
			LocalTargetInfo target = this.job.GetTarget(TargetIndex.B);
			Job job = this.job;
			if (!pawn.Reserve(target, job, 1, -1, null, errorOnFailed))
			{
				return false;
			}
			if (this.HasDrink)
			{
				pawn = this.pawn;
				target = this.job.GetTarget(TargetIndex.C);
				job = this.job;
				if (!pawn.Reserve(target, job, 1, -1, null, errorOnFailed))
				{
					return false;
				}
			}
			return true;
		}

		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.EndOnDespawnedOrNull(TargetIndex.A, JobCondition.Incompletable);
			if (this.HasChair)
			{
				this.EndOnDespawnedOrNull(TargetIndex.B, JobCondition.Incompletable);
			}
			if (this.HasDrink)
			{
				this.FailOnDestroyedNullOrForbidden(TargetIndex.C);
				yield return Toils_Goto.GotoThing(TargetIndex.C, PathEndMode.OnCell).FailOnSomeonePhysicallyInteracting(TargetIndex.C);
				yield return Toils_Haul.StartCarryThing(TargetIndex.C, false, false, false);
			}
			yield return Toils_Goto.GotoCell(TargetIndex.B, PathEndMode.OnCell);
			Toil chew = new Toil();
			chew.tickAction = delegate()
			{
				this.pawn.rotationTracker.FaceCell(this.ClosestGatherSpotParentCell);
				this.pawn.GainComfortFromCellIfPossible();
				JoyUtility.JoyTickCheckEnd(this.pawn, JoyTickFullJoyAction.GoToNextToil, 1f, null);
			};
			chew.handlingFacing = true;
			chew.defaultCompleteMode = ToilCompleteMode.Delay;
			chew.defaultDuration = this.job.def.joyDuration;
			chew.AddFinishAction(delegate
			{
				JoyUtility.TryGainRecRoomThought(this.pawn);
			});
			chew.socialMode = RandomSocialMode.SuperActive;
			Toils_Ingest.AddIngestionEffects(chew, this.pawn, TargetIndex.C, TargetIndex.None);
			yield return chew;
			if (this.HasDrink)
			{
				yield return Toils_Ingest.FinalizeIngest(this.pawn, TargetIndex.C);
			}
			yield break;
		}

		public override bool ModifyCarriedThingDrawPos(ref Vector3 drawPos, ref bool behind, ref bool flip)
		{
			IntVec3 closestGatherSpotParentCell = this.ClosestGatherSpotParentCell;
			return JobDriver_Ingest.ModifyCarriedThingDrawPosWorker(ref drawPos, ref behind, ref flip, closestGatherSpotParentCell, this.pawn);
		}

		[CompilerGenerated]
		private sealed class <MakeNewToils>c__Iterator0 : IEnumerable, IEnumerable<Toil>, IEnumerator, IDisposable, IEnumerator<Toil>
		{
			internal Toil <chew>__0;

			internal JobDriver_SocialRelax $this;

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
					if (base.HasChair)
					{
						this.EndOnDespawnedOrNull(TargetIndex.B, JobCondition.Incompletable);
					}
					if (base.HasDrink)
					{
						this.FailOnDestroyedNullOrForbidden(TargetIndex.C);
						this.$current = Toils_Goto.GotoThing(TargetIndex.C, PathEndMode.OnCell).FailOnSomeonePhysicallyInteracting(TargetIndex.C);
						if (!this.$disposing)
						{
							this.$PC = 1;
						}
						return true;
					}
					break;
				case 1u:
					this.$current = Toils_Haul.StartCarryThing(TargetIndex.C, false, false, false);
					if (!this.$disposing)
					{
						this.$PC = 2;
					}
					return true;
				case 2u:
					break;
				case 3u:
					chew = new Toil();
					chew.tickAction = delegate()
					{
						this.pawn.rotationTracker.FaceCell(base.ClosestGatherSpotParentCell);
						this.pawn.GainComfortFromCellIfPossible();
						JoyUtility.JoyTickCheckEnd(this.pawn, JoyTickFullJoyAction.GoToNextToil, 1f, null);
					};
					chew.handlingFacing = true;
					chew.defaultCompleteMode = ToilCompleteMode.Delay;
					chew.defaultDuration = this.job.def.joyDuration;
					chew.AddFinishAction(delegate
					{
						JoyUtility.TryGainRecRoomThought(this.pawn);
					});
					chew.socialMode = RandomSocialMode.SuperActive;
					Toils_Ingest.AddIngestionEffects(chew, this.pawn, TargetIndex.C, TargetIndex.None);
					this.$current = chew;
					if (!this.$disposing)
					{
						this.$PC = 4;
					}
					return true;
				case 4u:
					if (base.HasDrink)
					{
						this.$current = Toils_Ingest.FinalizeIngest(this.pawn, TargetIndex.C);
						if (!this.$disposing)
						{
							this.$PC = 5;
						}
						return true;
					}
					goto IL_1D6;
				case 5u:
					goto IL_1D6;
				default:
					return false;
				}
				this.$current = Toils_Goto.GotoCell(TargetIndex.B, PathEndMode.OnCell);
				if (!this.$disposing)
				{
					this.$PC = 3;
				}
				return true;
				IL_1D6:
				this.$PC = -1;
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
				JobDriver_SocialRelax.<MakeNewToils>c__Iterator0 <MakeNewToils>c__Iterator = new JobDriver_SocialRelax.<MakeNewToils>c__Iterator0();
				<MakeNewToils>c__Iterator.$this = this;
				return <MakeNewToils>c__Iterator;
			}

			internal void <>m__0()
			{
				this.pawn.rotationTracker.FaceCell(base.ClosestGatherSpotParentCell);
				this.pawn.GainComfortFromCellIfPossible();
				JoyUtility.JoyTickCheckEnd(this.pawn, JoyTickFullJoyAction.GoToNextToil, 1f, null);
			}

			internal void <>m__1()
			{
				JoyUtility.TryGainRecRoomThought(this.pawn);
			}
		}
	}
}
