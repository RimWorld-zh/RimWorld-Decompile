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
	public class JobDriver_FixBrokenDownBuilding : JobDriver
	{
		private const TargetIndex BuildingInd = TargetIndex.A;

		private const TargetIndex ComponentInd = TargetIndex.B;

		private const int TicksDuration = 1000;

		public JobDriver_FixBrokenDownBuilding()
		{
		}

		private Building Building
		{
			get
			{
				return (Building)this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		private Thing Components
		{
			get
			{
				return this.job.GetTarget(TargetIndex.B).Thing;
			}
		}

		public override bool TryMakePreToilReservations()
		{
			return this.pawn.Reserve(this.Building, this.job, 1, -1, null) && this.pawn.Reserve(this.Components, this.job, 1, -1, null);
		}

		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedNullOrForbidden(TargetIndex.A);
			yield return Toils_Goto.GotoThing(TargetIndex.B, PathEndMode.Touch).FailOnDespawnedNullOrForbidden(TargetIndex.B).FailOnSomeonePhysicallyInteracting(TargetIndex.B);
			yield return Toils_Haul.StartCarryThing(TargetIndex.B, false, false, false);
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch).FailOnDespawnedOrNull(TargetIndex.A);
			Toil repair = Toils_General.Wait(1000, TargetIndex.None);
			repair.FailOnDespawnedOrNull(TargetIndex.A);
			repair.FailOnCannotTouch(TargetIndex.A, PathEndMode.Touch);
			repair.WithEffect(this.Building.def.repairEffect, TargetIndex.A);
			repair.WithProgressBarToilDelay(TargetIndex.A, false, -0.5f);
			yield return repair;
			yield return new Toil
			{
				initAction = delegate()
				{
					this.Components.Destroy(DestroyMode.Vanish);
					if (Rand.Value > this.pawn.GetStatValue(StatDefOf.FixBrokenDownBuildingSuccessChance, true))
					{
						Vector3 loc = (this.pawn.DrawPos + this.Building.DrawPos) / 2f;
						MoteMaker.ThrowText(loc, base.Map, "TextMote_FixBrokenDownBuildingFail".Translate(), 3.65f);
					}
					else
					{
						this.Building.GetComp<CompBreakdownable>().Notify_Repaired();
					}
				}
			};
			yield break;
		}

		[CompilerGenerated]
		private sealed class <MakeNewToils>c__Iterator0 : IEnumerable, IEnumerable<Toil>, IEnumerator, IDisposable, IEnumerator<Toil>
		{
			internal Toil <repair>__0;

			internal Toil <finish>__0;

			internal JobDriver_FixBrokenDownBuilding $this;

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
					this.$current = Toils_Goto.GotoThing(TargetIndex.B, PathEndMode.Touch).FailOnDespawnedNullOrForbidden(TargetIndex.B).FailOnSomeonePhysicallyInteracting(TargetIndex.B);
					if (!this.$disposing)
					{
						this.$PC = 1;
					}
					return true;
				case 1u:
					this.$current = Toils_Haul.StartCarryThing(TargetIndex.B, false, false, false);
					if (!this.$disposing)
					{
						this.$PC = 2;
					}
					return true;
				case 2u:
					this.$current = Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch).FailOnDespawnedOrNull(TargetIndex.A);
					if (!this.$disposing)
					{
						this.$PC = 3;
					}
					return true;
				case 3u:
					repair = Toils_General.Wait(1000, TargetIndex.None);
					repair.FailOnDespawnedOrNull(TargetIndex.A);
					repair.FailOnCannotTouch(TargetIndex.A, PathEndMode.Touch);
					repair.WithEffect(base.Building.def.repairEffect, TargetIndex.A);
					repair.WithProgressBarToilDelay(TargetIndex.A, false, -0.5f);
					this.$current = repair;
					if (!this.$disposing)
					{
						this.$PC = 4;
					}
					return true;
				case 4u:
				{
					Toil finish = new Toil();
					finish.initAction = delegate()
					{
						base.Components.Destroy(DestroyMode.Vanish);
						if (Rand.Value > this.pawn.GetStatValue(StatDefOf.FixBrokenDownBuildingSuccessChance, true))
						{
							Vector3 loc = (this.pawn.DrawPos + base.Building.DrawPos) / 2f;
							MoteMaker.ThrowText(loc, base.Map, "TextMote_FixBrokenDownBuildingFail".Translate(), 3.65f);
						}
						else
						{
							base.Building.GetComp<CompBreakdownable>().Notify_Repaired();
						}
					};
					this.$current = finish;
					if (!this.$disposing)
					{
						this.$PC = 5;
					}
					return true;
				}
				case 5u:
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
				JobDriver_FixBrokenDownBuilding.<MakeNewToils>c__Iterator0 <MakeNewToils>c__Iterator = new JobDriver_FixBrokenDownBuilding.<MakeNewToils>c__Iterator0();
				<MakeNewToils>c__Iterator.$this = this;
				return <MakeNewToils>c__Iterator;
			}

			internal void <>m__0()
			{
				base.Components.Destroy(DestroyMode.Vanish);
				if (Rand.Value > this.pawn.GetStatValue(StatDefOf.FixBrokenDownBuildingSuccessChance, true))
				{
					Vector3 loc = (this.pawn.DrawPos + base.Building.DrawPos) / 2f;
					MoteMaker.ThrowText(loc, base.Map, "TextMote_FixBrokenDownBuildingFail".Translate(), 3.65f);
				}
				else
				{
					base.Building.GetComp<CompBreakdownable>().Notify_Repaired();
				}
			}
		}
	}
}
