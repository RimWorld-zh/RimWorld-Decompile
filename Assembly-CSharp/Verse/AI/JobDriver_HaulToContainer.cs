using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using RimWorld;

namespace Verse.AI
{
	public class JobDriver_HaulToContainer : JobDriver
	{
		private const TargetIndex CarryThingIndex = TargetIndex.A;

		private const TargetIndex DestIndex = TargetIndex.B;

		private const TargetIndex PrimaryDestIndex = TargetIndex.C;

		public JobDriver_HaulToContainer()
		{
		}

		private Thing ThingToCarry
		{
			get
			{
				return (Thing)this.job.GetTarget(TargetIndex.A);
			}
		}

		private Thing Container
		{
			get
			{
				return (Thing)this.job.GetTarget(TargetIndex.B);
			}
		}

		private int Duration
		{
			get
			{
				return (this.Container == null || !(this.Container is Building)) ? 0 : this.Container.def.building.haulToContainerDuration;
			}
		}

		public override string GetReport()
		{
			Thing thing;
			if (this.pawn.CurJob == this.job && this.pawn.carryTracker.CarriedThing != null)
			{
				thing = this.pawn.carryTracker.CarriedThing;
			}
			else
			{
				thing = base.TargetThingA;
			}
			string result;
			if (thing == null || !this.job.targetB.HasThing)
			{
				result = "ReportHaulingUnknown".Translate();
			}
			else
			{
				result = "ReportHaulingTo".Translate(new object[]
				{
					thing.Label,
					this.job.targetB.Thing.LabelShort
				});
			}
			return result;
		}

		public override bool TryMakePreToilReservations()
		{
			this.pawn.ReserveAsManyAsPossible(this.job.GetTargetQueue(TargetIndex.A), this.job, 1, -1, null);
			this.pawn.ReserveAsManyAsPossible(this.job.GetTargetQueue(TargetIndex.B), this.job, 1, -1, null);
			return this.pawn.Reserve(this.job.GetTarget(TargetIndex.A), this.job, 1, -1, null) && this.pawn.Reserve(this.job.GetTarget(TargetIndex.B), this.job, 1, -1, null);
		}

		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDestroyedOrNull(TargetIndex.A);
			this.FailOnDestroyedNullOrForbidden(TargetIndex.B);
			this.FailOn(() => TransporterUtility.WasLoadingCanceled(this.Container));
			this.FailOn(delegate()
			{
				ThingOwner thingOwner = this.Container.TryGetInnerInteractableThingOwner();
				bool result;
				if (thingOwner != null && !thingOwner.CanAcceptAnyOf(this.ThingToCarry, true))
				{
					result = true;
				}
				else
				{
					IHaulDestination haulDestination = this.Container as IHaulDestination;
					result = (haulDestination != null && !haulDestination.Accepts(this.ThingToCarry));
				}
				return result;
			});
			Toil getToHaulTarget = Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.ClosestTouch).FailOnSomeonePhysicallyInteracting(TargetIndex.A);
			yield return getToHaulTarget;
			yield return Toils_Construct.UninstallIfMinifiable(TargetIndex.A).FailOnSomeonePhysicallyInteracting(TargetIndex.A);
			yield return Toils_Haul.StartCarryThing(TargetIndex.A, false, true, false);
			yield return Toils_Haul.JumpIfAlsoCollectingNextTargetInQueue(getToHaulTarget, TargetIndex.A);
			Toil carryToContainer = Toils_Haul.CarryHauledThingToContainer();
			yield return carryToContainer;
			yield return Toils_Goto.MoveOffTargetBlueprint(TargetIndex.B);
			Toil prepare = Toils_General.Wait(this.Duration, TargetIndex.B);
			prepare.WithProgressBarToilDelay(TargetIndex.B, false, -0.5f);
			yield return prepare;
			yield return Toils_Construct.MakeSolidThingFromBlueprintIfNecessary(TargetIndex.B, TargetIndex.C);
			yield return Toils_Haul.DepositHauledThingInContainer(TargetIndex.B, TargetIndex.C);
			yield return Toils_Haul.JumpToCarryToNextContainerIfPossible(carryToContainer, TargetIndex.C);
			yield break;
		}

		[CompilerGenerated]
		private sealed class <MakeNewToils>c__Iterator0 : IEnumerable, IEnumerable<Toil>, IEnumerator, IDisposable, IEnumerator<Toil>
		{
			internal Toil <getToHaulTarget>__0;

			internal Toil <carryToContainer>__0;

			internal Toil <prepare>__1;

			internal JobDriver_HaulToContainer $this;

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
					this.FailOnDestroyedOrNull(TargetIndex.A);
					this.FailOnDestroyedNullOrForbidden(TargetIndex.B);
					this.FailOn(() => TransporterUtility.WasLoadingCanceled(base.Container));
					this.FailOn(delegate()
					{
						ThingOwner thingOwner = base.Container.TryGetInnerInteractableThingOwner();
						bool result;
						if (thingOwner != null && !thingOwner.CanAcceptAnyOf(base.ThingToCarry, true))
						{
							result = true;
						}
						else
						{
							IHaulDestination haulDestination = base.Container as IHaulDestination;
							result = (haulDestination != null && !haulDestination.Accepts(base.ThingToCarry));
						}
						return result;
					});
					getToHaulTarget = Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.ClosestTouch).FailOnSomeonePhysicallyInteracting(TargetIndex.A);
					this.$current = getToHaulTarget;
					if (!this.$disposing)
					{
						this.$PC = 1;
					}
					return true;
				case 1u:
					this.$current = Toils_Construct.UninstallIfMinifiable(TargetIndex.A).FailOnSomeonePhysicallyInteracting(TargetIndex.A);
					if (!this.$disposing)
					{
						this.$PC = 2;
					}
					return true;
				case 2u:
					this.$current = Toils_Haul.StartCarryThing(TargetIndex.A, false, true, false);
					if (!this.$disposing)
					{
						this.$PC = 3;
					}
					return true;
				case 3u:
					this.$current = Toils_Haul.JumpIfAlsoCollectingNextTargetInQueue(getToHaulTarget, TargetIndex.A);
					if (!this.$disposing)
					{
						this.$PC = 4;
					}
					return true;
				case 4u:
					carryToContainer = Toils_Haul.CarryHauledThingToContainer();
					this.$current = carryToContainer;
					if (!this.$disposing)
					{
						this.$PC = 5;
					}
					return true;
				case 5u:
					this.$current = Toils_Goto.MoveOffTargetBlueprint(TargetIndex.B);
					if (!this.$disposing)
					{
						this.$PC = 6;
					}
					return true;
				case 6u:
					prepare = Toils_General.Wait(base.Duration, TargetIndex.B);
					prepare.WithProgressBarToilDelay(TargetIndex.B, false, -0.5f);
					this.$current = prepare;
					if (!this.$disposing)
					{
						this.$PC = 7;
					}
					return true;
				case 7u:
					this.$current = Toils_Construct.MakeSolidThingFromBlueprintIfNecessary(TargetIndex.B, TargetIndex.C);
					if (!this.$disposing)
					{
						this.$PC = 8;
					}
					return true;
				case 8u:
					this.$current = Toils_Haul.DepositHauledThingInContainer(TargetIndex.B, TargetIndex.C);
					if (!this.$disposing)
					{
						this.$PC = 9;
					}
					return true;
				case 9u:
					this.$current = Toils_Haul.JumpToCarryToNextContainerIfPossible(carryToContainer, TargetIndex.C);
					if (!this.$disposing)
					{
						this.$PC = 10;
					}
					return true;
				case 10u:
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
				JobDriver_HaulToContainer.<MakeNewToils>c__Iterator0 <MakeNewToils>c__Iterator = new JobDriver_HaulToContainer.<MakeNewToils>c__Iterator0();
				<MakeNewToils>c__Iterator.$this = this;
				return <MakeNewToils>c__Iterator;
			}

			internal bool <>m__0()
			{
				return TransporterUtility.WasLoadingCanceled(base.Container);
			}

			internal bool <>m__1()
			{
				ThingOwner thingOwner = base.Container.TryGetInnerInteractableThingOwner();
				bool result;
				if (thingOwner != null && !thingOwner.CanAcceptAnyOf(base.ThingToCarry, true))
				{
					result = true;
				}
				else
				{
					IHaulDestination haulDestination = base.Container as IHaulDestination;
					result = (haulDestination != null && !haulDestination.Accepts(base.ThingToCarry));
				}
				return result;
			}
		}
	}
}
