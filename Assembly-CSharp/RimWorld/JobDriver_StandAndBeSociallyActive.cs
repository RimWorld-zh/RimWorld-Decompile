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
	public class JobDriver_StandAndBeSociallyActive : JobDriver
	{
		[CompilerGenerated]
		private static Predicate<Thing> <>f__am$cache0;

		public JobDriver_StandAndBeSociallyActive()
		{
		}

		public override bool TryMakePreToilReservations()
		{
			return true;
		}

		protected override IEnumerable<Toil> MakeNewToils()
		{
			yield return new Toil
			{
				tickAction = delegate()
				{
					Pawn pawn = this.FindClosePawn();
					if (pawn != null)
					{
						this.pawn.rotationTracker.FaceCell(pawn.Position);
					}
					this.pawn.GainComfortFromCellIfPossible();
				},
				socialMode = RandomSocialMode.SuperActive,
				defaultCompleteMode = ToilCompleteMode.Never,
				handlingFacing = true
			};
			yield break;
		}

		private Pawn FindClosePawn()
		{
			IntVec3 position = this.pawn.Position;
			for (int i = 0; i < 24; i++)
			{
				IntVec3 intVec = position + GenRadial.RadialPattern[i];
				if (intVec.InBounds(base.Map))
				{
					Thing thing = intVec.GetThingList(base.Map).Find((Thing x) => x is Pawn);
					if (thing != null && thing != this.pawn)
					{
						if (GenSight.LineOfSight(position, intVec, base.Map, false, null, 0, 0))
						{
							return (Pawn)thing;
						}
					}
				}
			}
			return null;
		}

		[CompilerGenerated]
		private static bool <FindClosePawn>m__0(Thing x)
		{
			return x is Pawn;
		}

		[CompilerGenerated]
		private sealed class <MakeNewToils>c__Iterator0 : IEnumerable, IEnumerable<Toil>, IEnumerator, IDisposable, IEnumerator<Toil>
		{
			internal Toil <stand>__0;

			internal JobDriver_StandAndBeSociallyActive $this;

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
				{
					Toil stand = new Toil();
					stand.tickAction = delegate()
					{
						Pawn pawn = base.FindClosePawn();
						if (pawn != null)
						{
							this.pawn.rotationTracker.FaceCell(pawn.Position);
						}
						this.pawn.GainComfortFromCellIfPossible();
					};
					stand.socialMode = RandomSocialMode.SuperActive;
					stand.defaultCompleteMode = ToilCompleteMode.Never;
					stand.handlingFacing = true;
					this.$current = stand;
					if (!this.$disposing)
					{
						this.$PC = 1;
					}
					return true;
				}
				case 1u:
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
				JobDriver_StandAndBeSociallyActive.<MakeNewToils>c__Iterator0 <MakeNewToils>c__Iterator = new JobDriver_StandAndBeSociallyActive.<MakeNewToils>c__Iterator0();
				<MakeNewToils>c__Iterator.$this = this;
				return <MakeNewToils>c__Iterator;
			}

			internal void <>m__0()
			{
				Pawn pawn = base.FindClosePawn();
				if (pawn != null)
				{
					this.pawn.rotationTracker.FaceCell(pawn.Position);
				}
				this.pawn.GainComfortFromCellIfPossible();
			}
		}
	}
}
