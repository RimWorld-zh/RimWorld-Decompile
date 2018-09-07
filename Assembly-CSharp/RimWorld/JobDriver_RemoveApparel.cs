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
	public class JobDriver_RemoveApparel : JobDriver
	{
		private int duration;

		private const TargetIndex ApparelInd = TargetIndex.A;

		public JobDriver_RemoveApparel()
		{
		}

		private Apparel Apparel
		{
			get
			{
				return (Apparel)this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.duration, "duration", 0, false);
		}

		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return true;
		}

		public override void Notify_Starting()
		{
			base.Notify_Starting();
			this.duration = (int)(this.Apparel.GetStatValue(StatDefOf.EquipDelay, true) * 60f);
		}

		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDestroyedOrNull(TargetIndex.A);
			yield return Toils_General.Wait(this.duration, TargetIndex.None).WithProgressBarToilDelay(TargetIndex.A, false, -0.5f);
			yield return Toils_General.Do(delegate
			{
				if (this.pawn.apparel.WornApparel.Contains(this.Apparel))
				{
					Apparel apparel;
					if (this.pawn.apparel.TryDrop(this.Apparel, out apparel))
					{
						this.job.targetA = apparel;
						if (this.job.haulDroppedApparel)
						{
							apparel.SetForbidden(false, false);
							StoragePriority currentPriority = StoreUtility.CurrentStoragePriorityOf(apparel);
							IntVec3 c;
							if (StoreUtility.TryFindBestBetterStoreCellFor(apparel, this.pawn, base.Map, currentPriority, this.pawn.Faction, out c, true))
							{
								this.job.count = apparel.stackCount;
								this.job.targetB = c;
							}
							else
							{
								base.EndJobWith(JobCondition.Incompletable);
							}
						}
						else
						{
							base.EndJobWith(JobCondition.Succeeded);
						}
					}
					else
					{
						base.EndJobWith(JobCondition.Incompletable);
					}
				}
				else
				{
					base.EndJobWith(JobCondition.Incompletable);
				}
			});
			if (this.job.haulDroppedApparel)
			{
				yield return Toils_Reserve.Reserve(TargetIndex.B, 1, -1, null);
				yield return Toils_Reserve.Reserve(TargetIndex.A, 1, -1, null);
				yield return Toils_Haul.StartCarryThing(TargetIndex.A, false, false, false).FailOn(() => !this.pawn.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation));
				Toil carryToCell = Toils_Haul.CarryHauledThingToCell(TargetIndex.B);
				yield return carryToCell;
				yield return Toils_Haul.PlaceHauledThingInCell(TargetIndex.B, carryToCell, true);
			}
			yield break;
		}

		[CompilerGenerated]
		private sealed class <MakeNewToils>c__Iterator0 : IEnumerable, IEnumerable<Toil>, IEnumerator, IDisposable, IEnumerator<Toil>
		{
			internal Toil <carryToCell>__1;

			internal JobDriver_RemoveApparel $this;

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
					this.$current = Toils_General.Wait(this.duration, TargetIndex.None).WithProgressBarToilDelay(TargetIndex.A, false, -0.5f);
					if (!this.$disposing)
					{
						this.$PC = 1;
					}
					return true;
				case 1u:
					this.$current = Toils_General.Do(delegate
					{
						if (this.pawn.apparel.WornApparel.Contains(base.Apparel))
						{
							Apparel apparel;
							if (this.pawn.apparel.TryDrop(base.Apparel, out apparel))
							{
								this.job.targetA = apparel;
								if (this.job.haulDroppedApparel)
								{
									apparel.SetForbidden(false, false);
									StoragePriority currentPriority = StoreUtility.CurrentStoragePriorityOf(apparel);
									IntVec3 c;
									if (StoreUtility.TryFindBestBetterStoreCellFor(apparel, this.pawn, base.Map, currentPriority, this.pawn.Faction, out c, true))
									{
										this.job.count = apparel.stackCount;
										this.job.targetB = c;
									}
									else
									{
										base.EndJobWith(JobCondition.Incompletable);
									}
								}
								else
								{
									base.EndJobWith(JobCondition.Succeeded);
								}
							}
							else
							{
								base.EndJobWith(JobCondition.Incompletable);
							}
						}
						else
						{
							base.EndJobWith(JobCondition.Incompletable);
						}
					});
					if (!this.$disposing)
					{
						this.$PC = 2;
					}
					return true;
				case 2u:
					if (this.job.haulDroppedApparel)
					{
						this.$current = Toils_Reserve.Reserve(TargetIndex.B, 1, -1, null);
						if (!this.$disposing)
						{
							this.$PC = 3;
						}
						return true;
					}
					break;
				case 3u:
					this.$current = Toils_Reserve.Reserve(TargetIndex.A, 1, -1, null);
					if (!this.$disposing)
					{
						this.$PC = 4;
					}
					return true;
				case 4u:
					this.$current = Toils_Haul.StartCarryThing(TargetIndex.A, false, false, false).FailOn(() => !this.pawn.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation));
					if (!this.$disposing)
					{
						this.$PC = 5;
					}
					return true;
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
					break;
				default:
					return false;
				}
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
				JobDriver_RemoveApparel.<MakeNewToils>c__Iterator0 <MakeNewToils>c__Iterator = new JobDriver_RemoveApparel.<MakeNewToils>c__Iterator0();
				<MakeNewToils>c__Iterator.$this = this;
				return <MakeNewToils>c__Iterator;
			}

			internal void <>m__0()
			{
				if (this.pawn.apparel.WornApparel.Contains(base.Apparel))
				{
					Apparel apparel;
					if (this.pawn.apparel.TryDrop(base.Apparel, out apparel))
					{
						this.job.targetA = apparel;
						if (this.job.haulDroppedApparel)
						{
							apparel.SetForbidden(false, false);
							StoragePriority currentPriority = StoreUtility.CurrentStoragePriorityOf(apparel);
							IntVec3 c;
							if (StoreUtility.TryFindBestBetterStoreCellFor(apparel, this.pawn, base.Map, currentPriority, this.pawn.Faction, out c, true))
							{
								this.job.count = apparel.stackCount;
								this.job.targetB = c;
							}
							else
							{
								base.EndJobWith(JobCondition.Incompletable);
							}
						}
						else
						{
							base.EndJobWith(JobCondition.Succeeded);
						}
					}
					else
					{
						base.EndJobWith(JobCondition.Incompletable);
					}
				}
				else
				{
					base.EndJobWith(JobCondition.Incompletable);
				}
			}

			internal bool <>m__1()
			{
				return !this.pawn.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation);
			}
		}
	}
}
