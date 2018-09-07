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
	public class JobDriver_UnloadInventory : JobDriver
	{
		private const TargetIndex OtherPawnInd = TargetIndex.A;

		private const TargetIndex ItemToHaulInd = TargetIndex.B;

		private const TargetIndex StoreCellInd = TargetIndex.C;

		private const int UnloadDuration = 10;

		public JobDriver_UnloadInventory()
		{
		}

		private Pawn OtherPawn
		{
			get
			{
				return (Pawn)this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			Pawn pawn = this.pawn;
			LocalTargetInfo target = this.OtherPawn;
			Job job = this.job;
			return pawn.Reserve(target, job, 1, -1, null, errorOnFailed);
		}

		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedOrNull(TargetIndex.A);
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch);
			yield return Toils_General.Wait(10, TargetIndex.None);
			yield return new Toil
			{
				initAction = delegate()
				{
					Pawn otherPawn = this.OtherPawn;
					if (!otherPawn.inventory.UnloadEverything)
					{
						base.EndJobWith(JobCondition.Succeeded);
					}
					else
					{
						ThingCount firstUnloadableThing = otherPawn.inventory.FirstUnloadableThing;
						IntVec3 c;
						if (!firstUnloadableThing.Thing.def.EverStorable(false) || !this.pawn.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation) || !StoreUtility.TryFindStoreCellNearColonyDesperate(firstUnloadableThing.Thing, this.pawn, out c))
						{
							Thing thing;
							otherPawn.inventory.innerContainer.TryDrop(firstUnloadableThing.Thing, ThingPlaceMode.Near, firstUnloadableThing.Count, out thing, null, null);
							base.EndJobWith(JobCondition.Succeeded);
							if (thing != null)
							{
								thing.SetForbidden(false, false);
							}
						}
						else
						{
							Thing thing2;
							otherPawn.inventory.innerContainer.TryTransferToContainer(firstUnloadableThing.Thing, this.pawn.carryTracker.innerContainer, firstUnloadableThing.Count, out thing2, true);
							this.job.count = thing2.stackCount;
							this.job.SetTarget(TargetIndex.B, thing2);
							this.job.SetTarget(TargetIndex.C, c);
							firstUnloadableThing.Thing.SetForbidden(false, false);
						}
					}
				}
			};
			yield return Toils_Reserve.Reserve(TargetIndex.C, 1, -1, null);
			Toil carryToCell = Toils_Haul.CarryHauledThingToCell(TargetIndex.C);
			yield return carryToCell;
			yield return Toils_Haul.PlaceHauledThingInCell(TargetIndex.C, carryToCell, true);
			yield break;
		}

		[CompilerGenerated]
		private sealed class <MakeNewToils>c__Iterator0 : IEnumerable, IEnumerable<Toil>, IEnumerator, IDisposable, IEnumerator<Toil>
		{
			internal Toil <dropOrStartCarrying>__0;

			internal Toil <carryToCell>__0;

			internal JobDriver_UnloadInventory $this;

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
					this.FailOnDespawnedOrNull(TargetIndex.A);
					this.$current = Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch);
					if (!this.$disposing)
					{
						this.$PC = 1;
					}
					return true;
				case 1u:
					this.$current = Toils_General.Wait(10, TargetIndex.None);
					if (!this.$disposing)
					{
						this.$PC = 2;
					}
					return true;
				case 2u:
				{
					Toil dropOrStartCarrying = new Toil();
					dropOrStartCarrying.initAction = delegate()
					{
						Pawn otherPawn = base.OtherPawn;
						if (!otherPawn.inventory.UnloadEverything)
						{
							base.EndJobWith(JobCondition.Succeeded);
						}
						else
						{
							ThingCount firstUnloadableThing = otherPawn.inventory.FirstUnloadableThing;
							IntVec3 c;
							if (!firstUnloadableThing.Thing.def.EverStorable(false) || !this.pawn.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation) || !StoreUtility.TryFindStoreCellNearColonyDesperate(firstUnloadableThing.Thing, this.pawn, out c))
							{
								Thing thing;
								otherPawn.inventory.innerContainer.TryDrop(firstUnloadableThing.Thing, ThingPlaceMode.Near, firstUnloadableThing.Count, out thing, null, null);
								base.EndJobWith(JobCondition.Succeeded);
								if (thing != null)
								{
									thing.SetForbidden(false, false);
								}
							}
							else
							{
								Thing thing2;
								otherPawn.inventory.innerContainer.TryTransferToContainer(firstUnloadableThing.Thing, this.pawn.carryTracker.innerContainer, firstUnloadableThing.Count, out thing2, true);
								this.job.count = thing2.stackCount;
								this.job.SetTarget(TargetIndex.B, thing2);
								this.job.SetTarget(TargetIndex.C, c);
								firstUnloadableThing.Thing.SetForbidden(false, false);
							}
						}
					};
					this.$current = dropOrStartCarrying;
					if (!this.$disposing)
					{
						this.$PC = 3;
					}
					return true;
				}
				case 3u:
					this.$current = Toils_Reserve.Reserve(TargetIndex.C, 1, -1, null);
					if (!this.$disposing)
					{
						this.$PC = 4;
					}
					return true;
				case 4u:
					carryToCell = Toils_Haul.CarryHauledThingToCell(TargetIndex.C);
					this.$current = carryToCell;
					if (!this.$disposing)
					{
						this.$PC = 5;
					}
					return true;
				case 5u:
					this.$current = Toils_Haul.PlaceHauledThingInCell(TargetIndex.C, carryToCell, true);
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
				JobDriver_UnloadInventory.<MakeNewToils>c__Iterator0 <MakeNewToils>c__Iterator = new JobDriver_UnloadInventory.<MakeNewToils>c__Iterator0();
				<MakeNewToils>c__Iterator.$this = this;
				return <MakeNewToils>c__Iterator;
			}

			internal void <>m__0()
			{
				Pawn otherPawn = base.OtherPawn;
				if (!otherPawn.inventory.UnloadEverything)
				{
					base.EndJobWith(JobCondition.Succeeded);
				}
				else
				{
					ThingCount firstUnloadableThing = otherPawn.inventory.FirstUnloadableThing;
					IntVec3 c;
					if (!firstUnloadableThing.Thing.def.EverStorable(false) || !this.pawn.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation) || !StoreUtility.TryFindStoreCellNearColonyDesperate(firstUnloadableThing.Thing, this.pawn, out c))
					{
						Thing thing;
						otherPawn.inventory.innerContainer.TryDrop(firstUnloadableThing.Thing, ThingPlaceMode.Near, firstUnloadableThing.Count, out thing, null, null);
						base.EndJobWith(JobCondition.Succeeded);
						if (thing != null)
						{
							thing.SetForbidden(false, false);
						}
					}
					else
					{
						Thing thing2;
						otherPawn.inventory.innerContainer.TryTransferToContainer(firstUnloadableThing.Thing, this.pawn.carryTracker.innerContainer, firstUnloadableThing.Count, out thing2, true);
						this.job.count = thing2.stackCount;
						this.job.SetTarget(TargetIndex.B, thing2);
						this.job.SetTarget(TargetIndex.C, c);
						firstUnloadableThing.Thing.SetForbidden(false, false);
					}
				}
			}
		}
	}
}
