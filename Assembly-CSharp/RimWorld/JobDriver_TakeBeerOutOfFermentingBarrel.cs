using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JobDriver_TakeBeerOutOfFermentingBarrel : JobDriver
	{
		private const TargetIndex BarrelInd = TargetIndex.A;

		private const TargetIndex BeerToHaulInd = TargetIndex.B;

		private const TargetIndex StorageCellInd = TargetIndex.C;

		private const int Duration = 200;

		public JobDriver_TakeBeerOutOfFermentingBarrel()
		{
		}

		protected Building_FermentingBarrel Barrel
		{
			get
			{
				return (Building_FermentingBarrel)this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		protected Thing Beer
		{
			get
			{
				return this.job.GetTarget(TargetIndex.B).Thing;
			}
		}

		public override bool TryMakePreToilReservations()
		{
			return this.pawn.Reserve(this.Barrel, this.job, 1, -1, null);
		}

		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedNullOrForbidden(TargetIndex.A);
			this.FailOnBurningImmobile(TargetIndex.A);
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch);
			yield return Toils_General.Wait(200).FailOnDestroyedNullOrForbidden(TargetIndex.A).FailOnCannotTouch(TargetIndex.A, PathEndMode.Touch).FailOn(() => !this.Barrel.Fermented).WithProgressBarToilDelay(TargetIndex.A, false, -0.5f);
			yield return new Toil
			{
				initAction = delegate()
				{
					Thing thing = this.Barrel.TakeOutBeer();
					GenPlace.TryPlaceThing(thing, this.pawn.Position, base.Map, ThingPlaceMode.Near, null, null);
					StoragePriority currentPriority = StoreUtility.CurrentStoragePriorityOf(thing);
					IntVec3 c;
					if (StoreUtility.TryFindBestBetterStoreCellFor(thing, this.pawn, base.Map, currentPriority, this.pawn.Faction, out c, true))
					{
						this.job.SetTarget(TargetIndex.C, c);
						this.job.SetTarget(TargetIndex.B, thing);
						this.job.count = thing.stackCount;
					}
					else
					{
						base.EndJobWith(JobCondition.Incompletable);
					}
				},
				defaultCompleteMode = ToilCompleteMode.Instant
			};
			yield return Toils_Reserve.Reserve(TargetIndex.B, 1, -1, null);
			yield return Toils_Reserve.Reserve(TargetIndex.C, 1, -1, null);
			yield return Toils_Goto.GotoThing(TargetIndex.B, PathEndMode.ClosestTouch);
			yield return Toils_Haul.StartCarryThing(TargetIndex.B, false, false, false);
			Toil carryToCell = Toils_Haul.CarryHauledThingToCell(TargetIndex.C);
			yield return carryToCell;
			yield return Toils_Haul.PlaceHauledThingInCell(TargetIndex.C, carryToCell, true);
			yield break;
		}

		[CompilerGenerated]
		private sealed class <MakeNewToils>c__Iterator0 : IEnumerable, IEnumerable<Toil>, IEnumerator, IDisposable, IEnumerator<Toil>
		{
			internal Toil <takeBeer>__0;

			internal Toil <carryToCell>__0;

			internal JobDriver_TakeBeerOutOfFermentingBarrel $this;

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
					this.FailOnDespawnedNullOrForbidden(TargetIndex.A);
					this.FailOnBurningImmobile(TargetIndex.A);
					this.$current = Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch);
					if (!this.$disposing)
					{
						this.$PC = 1;
					}
					return true;
				case 1u:
					this.$current = Toils_General.Wait(200).FailOnDestroyedNullOrForbidden(TargetIndex.A).FailOnCannotTouch(TargetIndex.A, PathEndMode.Touch).FailOn(() => !base.Barrel.Fermented).WithProgressBarToilDelay(TargetIndex.A, false, -0.5f);
					if (!this.$disposing)
					{
						this.$PC = 2;
					}
					return true;
				case 2u:
				{
					Toil takeBeer = new Toil();
					takeBeer.initAction = delegate()
					{
						Thing thing = base.Barrel.TakeOutBeer();
						GenPlace.TryPlaceThing(thing, this.pawn.Position, base.Map, ThingPlaceMode.Near, null, null);
						StoragePriority currentPriority = StoreUtility.CurrentStoragePriorityOf(thing);
						IntVec3 c;
						if (StoreUtility.TryFindBestBetterStoreCellFor(thing, this.pawn, base.Map, currentPriority, this.pawn.Faction, out c, true))
						{
							this.job.SetTarget(TargetIndex.C, c);
							this.job.SetTarget(TargetIndex.B, thing);
							this.job.count = thing.stackCount;
						}
						else
						{
							base.EndJobWith(JobCondition.Incompletable);
						}
					};
					takeBeer.defaultCompleteMode = ToilCompleteMode.Instant;
					this.$current = takeBeer;
					if (!this.$disposing)
					{
						this.$PC = 3;
					}
					return true;
				}
				case 3u:
					this.$current = Toils_Reserve.Reserve(TargetIndex.B, 1, -1, null);
					if (!this.$disposing)
					{
						this.$PC = 4;
					}
					return true;
				case 4u:
					this.$current = Toils_Reserve.Reserve(TargetIndex.C, 1, -1, null);
					if (!this.$disposing)
					{
						this.$PC = 5;
					}
					return true;
				case 5u:
					this.$current = Toils_Goto.GotoThing(TargetIndex.B, PathEndMode.ClosestTouch);
					if (!this.$disposing)
					{
						this.$PC = 6;
					}
					return true;
				case 6u:
					this.$current = Toils_Haul.StartCarryThing(TargetIndex.B, false, false, false);
					if (!this.$disposing)
					{
						this.$PC = 7;
					}
					return true;
				case 7u:
					carryToCell = Toils_Haul.CarryHauledThingToCell(TargetIndex.C);
					this.$current = carryToCell;
					if (!this.$disposing)
					{
						this.$PC = 8;
					}
					return true;
				case 8u:
					this.$current = Toils_Haul.PlaceHauledThingInCell(TargetIndex.C, carryToCell, true);
					if (!this.$disposing)
					{
						this.$PC = 9;
					}
					return true;
				case 9u:
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
				JobDriver_TakeBeerOutOfFermentingBarrel.<MakeNewToils>c__Iterator0 <MakeNewToils>c__Iterator = new JobDriver_TakeBeerOutOfFermentingBarrel.<MakeNewToils>c__Iterator0();
				<MakeNewToils>c__Iterator.$this = this;
				return <MakeNewToils>c__Iterator;
			}

			internal bool <>m__0()
			{
				return !base.Barrel.Fermented;
			}

			internal void <>m__1()
			{
				Thing thing = base.Barrel.TakeOutBeer();
				GenPlace.TryPlaceThing(thing, this.pawn.Position, base.Map, ThingPlaceMode.Near, null, null);
				StoragePriority currentPriority = StoreUtility.CurrentStoragePriorityOf(thing);
				IntVec3 c;
				if (StoreUtility.TryFindBestBetterStoreCellFor(thing, this.pawn, base.Map, currentPriority, this.pawn.Faction, out c, true))
				{
					this.job.SetTarget(TargetIndex.C, c);
					this.job.SetTarget(TargetIndex.B, thing);
					this.job.count = thing.stackCount;
				}
				else
				{
					base.EndJobWith(JobCondition.Incompletable);
				}
			}
		}
	}
}
