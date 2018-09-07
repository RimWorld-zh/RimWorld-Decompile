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
	public class JobDriver_UnloadYourInventory : JobDriver
	{
		private int countToDrop = -1;

		private const TargetIndex ItemToHaulInd = TargetIndex.A;

		private const TargetIndex StoreCellInd = TargetIndex.B;

		private const int UnloadDuration = 10;

		public JobDriver_UnloadYourInventory()
		{
		}

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.countToDrop, "countToDrop", -1, false);
		}

		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return true;
		}

		protected override IEnumerable<Toil> MakeNewToils()
		{
			yield return Toils_General.Wait(10, TargetIndex.None);
			yield return new Toil
			{
				initAction = delegate()
				{
					if (!this.pawn.inventory.UnloadEverything)
					{
						base.EndJobWith(JobCondition.Succeeded);
					}
					else
					{
						ThingCount firstUnloadableThing = this.pawn.inventory.FirstUnloadableThing;
						IntVec3 c;
						if (!StoreUtility.TryFindStoreCellNearColonyDesperate(firstUnloadableThing.Thing, this.pawn, out c))
						{
							Thing thing;
							this.pawn.inventory.innerContainer.TryDrop(firstUnloadableThing.Thing, ThingPlaceMode.Near, firstUnloadableThing.Count, out thing, null, null);
							base.EndJobWith(JobCondition.Succeeded);
						}
						else
						{
							this.job.SetTarget(TargetIndex.A, firstUnloadableThing.Thing);
							this.job.SetTarget(TargetIndex.B, c);
							this.countToDrop = firstUnloadableThing.Count;
						}
					}
				}
			};
			yield return Toils_Reserve.Reserve(TargetIndex.B, 1, -1, null);
			yield return Toils_Goto.GotoCell(TargetIndex.B, PathEndMode.Touch);
			yield return new Toil
			{
				initAction = delegate()
				{
					Thing thing = this.job.GetTarget(TargetIndex.A).Thing;
					if (thing == null || !this.pawn.inventory.innerContainer.Contains(thing))
					{
						base.EndJobWith(JobCondition.Incompletable);
						return;
					}
					if (!this.pawn.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation) || !thing.def.EverStorable(false))
					{
						this.pawn.inventory.innerContainer.TryDrop(thing, ThingPlaceMode.Near, this.countToDrop, out thing, null, null);
						base.EndJobWith(JobCondition.Succeeded);
					}
					else
					{
						this.pawn.inventory.innerContainer.TryTransferToContainer(thing, this.pawn.carryTracker.innerContainer, this.countToDrop, out thing, true);
						this.job.count = this.countToDrop;
						this.job.SetTarget(TargetIndex.A, thing);
					}
					thing.SetForbidden(false, false);
				}
			};
			Toil carryToCell = Toils_Haul.CarryHauledThingToCell(TargetIndex.B);
			yield return carryToCell;
			yield return Toils_Haul.PlaceHauledThingInCell(TargetIndex.B, carryToCell, true);
			yield break;
		}

		[CompilerGenerated]
		private sealed class <MakeNewToils>c__Iterator0 : IEnumerable, IEnumerable<Toil>, IEnumerator, IDisposable, IEnumerator<Toil>
		{
			internal Toil <pickItem>__0;

			internal Toil <dropOrStartCarrying>__0;

			internal Toil <carryToCell>__0;

			internal JobDriver_UnloadYourInventory $this;

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
					this.$current = Toils_General.Wait(10, TargetIndex.None);
					if (!this.$disposing)
					{
						this.$PC = 1;
					}
					return true;
				case 1u:
				{
					Toil pickItem = new Toil();
					pickItem.initAction = delegate()
					{
						if (!this.pawn.inventory.UnloadEverything)
						{
							base.EndJobWith(JobCondition.Succeeded);
						}
						else
						{
							ThingCount firstUnloadableThing = this.pawn.inventory.FirstUnloadableThing;
							IntVec3 c;
							if (!StoreUtility.TryFindStoreCellNearColonyDesperate(firstUnloadableThing.Thing, this.pawn, out c))
							{
								Thing thing;
								this.pawn.inventory.innerContainer.TryDrop(firstUnloadableThing.Thing, ThingPlaceMode.Near, firstUnloadableThing.Count, out thing, null, null);
								base.EndJobWith(JobCondition.Succeeded);
							}
							else
							{
								this.job.SetTarget(TargetIndex.A, firstUnloadableThing.Thing);
								this.job.SetTarget(TargetIndex.B, c);
								this.countToDrop = firstUnloadableThing.Count;
							}
						}
					};
					this.$current = pickItem;
					if (!this.$disposing)
					{
						this.$PC = 2;
					}
					return true;
				}
				case 2u:
					this.$current = Toils_Reserve.Reserve(TargetIndex.B, 1, -1, null);
					if (!this.$disposing)
					{
						this.$PC = 3;
					}
					return true;
				case 3u:
					this.$current = Toils_Goto.GotoCell(TargetIndex.B, PathEndMode.Touch);
					if (!this.$disposing)
					{
						this.$PC = 4;
					}
					return true;
				case 4u:
				{
					Toil dropOrStartCarrying = new Toil();
					dropOrStartCarrying.initAction = delegate()
					{
						Thing thing = this.job.GetTarget(TargetIndex.A).Thing;
						if (thing == null || !this.pawn.inventory.innerContainer.Contains(thing))
						{
							base.EndJobWith(JobCondition.Incompletable);
							return;
						}
						if (!this.pawn.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation) || !thing.def.EverStorable(false))
						{
							this.pawn.inventory.innerContainer.TryDrop(thing, ThingPlaceMode.Near, this.countToDrop, out thing, null, null);
							base.EndJobWith(JobCondition.Succeeded);
						}
						else
						{
							this.pawn.inventory.innerContainer.TryTransferToContainer(thing, this.pawn.carryTracker.innerContainer, this.countToDrop, out thing, true);
							this.job.count = this.countToDrop;
							this.job.SetTarget(TargetIndex.A, thing);
						}
						thing.SetForbidden(false, false);
					};
					this.$current = dropOrStartCarrying;
					if (!this.$disposing)
					{
						this.$PC = 5;
					}
					return true;
				}
				case 5u:
					carryToCell = Toils_Haul.CarryHauledThingToCell(TargetIndex.B);
					this.$current = carryToCell;
					if (!this.$disposing)
					{
						this.$PC = 6;
					}
					return true;
				case 6u:
					this.$current = Toils_Haul.PlaceHauledThingInCell(TargetIndex.B, carryToCell, true);
					if (!this.$disposing)
					{
						this.$PC = 7;
					}
					return true;
				case 7u:
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
				JobDriver_UnloadYourInventory.<MakeNewToils>c__Iterator0 <MakeNewToils>c__Iterator = new JobDriver_UnloadYourInventory.<MakeNewToils>c__Iterator0();
				<MakeNewToils>c__Iterator.$this = this;
				return <MakeNewToils>c__Iterator;
			}

			internal void <>m__0()
			{
				if (!this.pawn.inventory.UnloadEverything)
				{
					base.EndJobWith(JobCondition.Succeeded);
				}
				else
				{
					ThingCount firstUnloadableThing = this.pawn.inventory.FirstUnloadableThing;
					IntVec3 c;
					if (!StoreUtility.TryFindStoreCellNearColonyDesperate(firstUnloadableThing.Thing, this.pawn, out c))
					{
						Thing thing;
						this.pawn.inventory.innerContainer.TryDrop(firstUnloadableThing.Thing, ThingPlaceMode.Near, firstUnloadableThing.Count, out thing, null, null);
						base.EndJobWith(JobCondition.Succeeded);
					}
					else
					{
						this.job.SetTarget(TargetIndex.A, firstUnloadableThing.Thing);
						this.job.SetTarget(TargetIndex.B, c);
						this.countToDrop = firstUnloadableThing.Count;
					}
				}
			}

			internal void <>m__1()
			{
				Thing thing = this.job.GetTarget(TargetIndex.A).Thing;
				if (thing == null || !this.pawn.inventory.innerContainer.Contains(thing))
				{
					base.EndJobWith(JobCondition.Incompletable);
					return;
				}
				if (!this.pawn.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation) || !thing.def.EverStorable(false))
				{
					this.pawn.inventory.innerContainer.TryDrop(thing, ThingPlaceMode.Near, this.countToDrop, out thing, null, null);
					base.EndJobWith(JobCondition.Succeeded);
				}
				else
				{
					this.pawn.inventory.innerContainer.TryTransferToContainer(thing, this.pawn.carryTracker.innerContainer, this.countToDrop, out thing, true);
					this.job.count = this.countToDrop;
					this.job.SetTarget(TargetIndex.A, thing);
				}
				thing.SetForbidden(false, false);
			}
		}
	}
}
