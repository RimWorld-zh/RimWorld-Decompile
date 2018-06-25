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
	public class JobDriver_MarryAdjacentPawn : JobDriver
	{
		private int ticksLeftToMarry = 2500;

		private const TargetIndex OtherFianceInd = TargetIndex.A;

		private const int Duration = 2500;

		public JobDriver_MarryAdjacentPawn()
		{
		}

		private Pawn OtherFiance
		{
			get
			{
				return (Pawn)this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		public int TicksLeftToMarry
		{
			get
			{
				return this.ticksLeftToMarry;
			}
		}

		public override bool TryMakePreToilReservations()
		{
			return true;
		}

		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedOrNull(TargetIndex.A);
			this.FailOn(() => this.OtherFiance.Drafted || !this.pawn.Position.AdjacentTo8WayOrInside(this.OtherFiance));
			Toil marry = new Toil();
			marry.initAction = delegate()
			{
				this.ticksLeftToMarry = 2500;
			};
			marry.tickAction = delegate()
			{
				this.ticksLeftToMarry--;
				if (this.ticksLeftToMarry <= 0)
				{
					this.ticksLeftToMarry = 0;
					base.ReadyForNextToil();
				}
			};
			marry.defaultCompleteMode = ToilCompleteMode.Never;
			marry.FailOn(() => !this.pawn.relations.DirectRelationExists(PawnRelationDefOf.Fiance, this.OtherFiance));
			yield return marry;
			yield return new Toil
			{
				defaultCompleteMode = ToilCompleteMode.Instant,
				initAction = delegate()
				{
					if (this.pawn.thingIDNumber < this.OtherFiance.thingIDNumber)
					{
						MarriageCeremonyUtility.Married(this.pawn, this.OtherFiance);
					}
				}
			};
			yield break;
		}

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.ticksLeftToMarry, "ticksLeftToMarry", 0, false);
		}

		[CompilerGenerated]
		private sealed class <MakeNewToils>c__Iterator0 : IEnumerable, IEnumerable<Toil>, IEnumerator, IDisposable, IEnumerator<Toil>
		{
			internal Toil <marry>__0;

			internal Toil <finalize>__0;

			internal JobDriver_MarryAdjacentPawn $this;

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
					this.FailOn(() => base.OtherFiance.Drafted || !this.pawn.Position.AdjacentTo8WayOrInside(base.OtherFiance));
					marry = new Toil();
					marry.initAction = delegate()
					{
						this.ticksLeftToMarry = 2500;
					};
					marry.tickAction = delegate()
					{
						this.ticksLeftToMarry--;
						if (this.ticksLeftToMarry <= 0)
						{
							this.ticksLeftToMarry = 0;
							base.ReadyForNextToil();
						}
					};
					marry.defaultCompleteMode = ToilCompleteMode.Never;
					marry.FailOn(() => !this.pawn.relations.DirectRelationExists(PawnRelationDefOf.Fiance, base.OtherFiance));
					this.$current = marry;
					if (!this.$disposing)
					{
						this.$PC = 1;
					}
					return true;
				case 1u:
				{
					Toil finalize = new Toil();
					finalize.defaultCompleteMode = ToilCompleteMode.Instant;
					finalize.initAction = delegate()
					{
						if (this.pawn.thingIDNumber < base.OtherFiance.thingIDNumber)
						{
							MarriageCeremonyUtility.Married(this.pawn, base.OtherFiance);
						}
					};
					this.$current = finalize;
					if (!this.$disposing)
					{
						this.$PC = 2;
					}
					return true;
				}
				case 2u:
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
				JobDriver_MarryAdjacentPawn.<MakeNewToils>c__Iterator0 <MakeNewToils>c__Iterator = new JobDriver_MarryAdjacentPawn.<MakeNewToils>c__Iterator0();
				<MakeNewToils>c__Iterator.$this = this;
				return <MakeNewToils>c__Iterator;
			}

			internal bool <>m__0()
			{
				return base.OtherFiance.Drafted || !this.pawn.Position.AdjacentTo8WayOrInside(base.OtherFiance);
			}

			internal void <>m__1()
			{
				this.ticksLeftToMarry = 2500;
			}

			internal void <>m__2()
			{
				this.ticksLeftToMarry--;
				if (this.ticksLeftToMarry <= 0)
				{
					this.ticksLeftToMarry = 0;
					base.ReadyForNextToil();
				}
			}

			internal bool <>m__3()
			{
				return !this.pawn.relations.DirectRelationExists(PawnRelationDefOf.Fiance, base.OtherFiance);
			}

			internal void <>m__4()
			{
				if (this.pawn.thingIDNumber < base.OtherFiance.thingIDNumber)
				{
					MarriageCeremonyUtility.Married(this.pawn, base.OtherFiance);
				}
			}
		}
	}
}
