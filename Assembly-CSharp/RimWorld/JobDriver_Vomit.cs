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
	public class JobDriver_Vomit : JobDriver
	{
		private int ticksLeft;

		public JobDriver_Vomit()
		{
		}

		public override void SetInitialPosture()
		{
		}

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.ticksLeft, "ticksLeft", 0, false);
		}

		public override bool TryMakePreToilReservations()
		{
			return true;
		}

		protected override IEnumerable<Toil> MakeNewToils()
		{
			Toil to = new Toil();
			to.initAction = delegate()
			{
				this.ticksLeft = Rand.Range(300, 900);
				int num = 0;
				IntVec3 c;
				for (;;)
				{
					c = this.pawn.Position + GenAdj.AdjacentCellsAndInside[Rand.Range(0, 9)];
					num++;
					if (num > 12)
					{
						break;
					}
					if (c.InBounds(this.pawn.Map) && c.Standable(this.pawn.Map))
					{
						goto IL_A5;
					}
				}
				c = this.pawn.Position;
				IL_A5:
				this.job.targetA = c;
				this.pawn.pather.StopDead();
			};
			to.tickAction = delegate()
			{
				if (this.ticksLeft % 150 == 149)
				{
					FilthMaker.MakeFilth(this.job.targetA.Cell, base.Map, ThingDefOf.Filth_Vomit, this.pawn.LabelIndefinite(), 1);
					if (this.pawn.needs.food.CurLevelPercentage > 0.1f)
					{
						this.pawn.needs.food.CurLevel -= this.pawn.needs.food.MaxLevel * 0.04f;
					}
				}
				this.ticksLeft--;
				if (this.ticksLeft <= 0)
				{
					base.ReadyForNextToil();
					TaleRecorder.RecordTale(TaleDefOf.Vomited, new object[]
					{
						this.pawn
					});
				}
			};
			to.defaultCompleteMode = ToilCompleteMode.Never;
			to.WithEffect(EffecterDefOf.Vomit, TargetIndex.A);
			to.PlaySustainerOrSound(() => SoundDefOf.Vomit);
			yield return to;
			yield break;
		}

		[CompilerGenerated]
		private sealed class <MakeNewToils>c__Iterator0 : IEnumerable, IEnumerable<Toil>, IEnumerator, IDisposable, IEnumerator<Toil>
		{
			internal Toil <to>__0;

			internal JobDriver_Vomit $this;

			internal Toil $current;

			internal bool $disposing;

			internal int $PC;

			private static Func<SoundDef> <>f__am$cache0;

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
					to = new Toil();
					to.initAction = delegate()
					{
						this.ticksLeft = Rand.Range(300, 900);
						int num2 = 0;
						IntVec3 c;
						for (;;)
						{
							c = this.pawn.Position + GenAdj.AdjacentCellsAndInside[Rand.Range(0, 9)];
							num2++;
							if (num2 > 12)
							{
								break;
							}
							if (c.InBounds(this.pawn.Map) && c.Standable(this.pawn.Map))
							{
								goto IL_A5;
							}
						}
						c = this.pawn.Position;
						IL_A5:
						this.job.targetA = c;
						this.pawn.pather.StopDead();
					};
					to.tickAction = delegate()
					{
						if (this.ticksLeft % 150 == 149)
						{
							FilthMaker.MakeFilth(this.job.targetA.Cell, base.Map, ThingDefOf.Filth_Vomit, this.pawn.LabelIndefinite(), 1);
							if (this.pawn.needs.food.CurLevelPercentage > 0.1f)
							{
								this.pawn.needs.food.CurLevel -= this.pawn.needs.food.MaxLevel * 0.04f;
							}
						}
						this.ticksLeft--;
						if (this.ticksLeft <= 0)
						{
							base.ReadyForNextToil();
							TaleRecorder.RecordTale(TaleDefOf.Vomited, new object[]
							{
								this.pawn
							});
						}
					};
					to.defaultCompleteMode = ToilCompleteMode.Never;
					to.WithEffect(EffecterDefOf.Vomit, TargetIndex.A);
					to.PlaySustainerOrSound(() => SoundDefOf.Vomit);
					this.$current = to;
					if (!this.$disposing)
					{
						this.$PC = 1;
					}
					return true;
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
				JobDriver_Vomit.<MakeNewToils>c__Iterator0 <MakeNewToils>c__Iterator = new JobDriver_Vomit.<MakeNewToils>c__Iterator0();
				<MakeNewToils>c__Iterator.$this = this;
				return <MakeNewToils>c__Iterator;
			}

			internal void <>m__0()
			{
				this.ticksLeft = Rand.Range(300, 900);
				int num = 0;
				IntVec3 c;
				for (;;)
				{
					c = this.pawn.Position + GenAdj.AdjacentCellsAndInside[Rand.Range(0, 9)];
					num++;
					if (num > 12)
					{
						break;
					}
					if (c.InBounds(this.pawn.Map) && c.Standable(this.pawn.Map))
					{
						goto IL_A5;
					}
				}
				c = this.pawn.Position;
				IL_A5:
				this.job.targetA = c;
				this.pawn.pather.StopDead();
			}

			internal void <>m__1()
			{
				if (this.ticksLeft % 150 == 149)
				{
					FilthMaker.MakeFilth(this.job.targetA.Cell, base.Map, ThingDefOf.Filth_Vomit, this.pawn.LabelIndefinite(), 1);
					if (this.pawn.needs.food.CurLevelPercentage > 0.1f)
					{
						this.pawn.needs.food.CurLevel -= this.pawn.needs.food.MaxLevel * 0.04f;
					}
				}
				this.ticksLeft--;
				if (this.ticksLeft <= 0)
				{
					base.ReadyForNextToil();
					TaleRecorder.RecordTale(TaleDefOf.Vomited, new object[]
					{
						this.pawn
					});
				}
			}

			private static SoundDef <>m__2()
			{
				return SoundDefOf.Vomit;
			}
		}
	}
}
